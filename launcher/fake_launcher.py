"""
Fake Ankama Launcher - Transparent proxy between Dofus.exe and the real Launcher.

Instead of handling IPC itself, this proxy transparently forwards ALL messages
to the real Launcher and back, while intercepting and logging key data
(account info, game tokens). This works because the Launcher knows about
the instance (it launched Dofus itself via zaap-start.bat).

Setup:
    1. zaap-start.bat: set ZAAP_PORT=26666 (before Dofus.exe launch)
    2. This proxy listens on 26666, forwards to real Launcher on 26116
    3. User clicks "Jouer" in Ankama Launcher normally
    4. Dofus connects to us (26666), we forward to Launcher (26116)
    5. We intercept the game token for our MITM proxy

Architecture:
    Dofus.exe --port 26666 --> Proxy (26666) --> Real Launcher (26116)
"""

import asyncio
import struct
from launcher.ipc_protocol import (
    ThriftDecoder, ThriftMessage,
    VERSION_1, VERSION_MASK, MSG_REPLY,
)
from utils import logger


class FakeLauncher:
    """Transparent IPC proxy between Dofus.exe and the real Ankama Launcher."""

    def __init__(self, listen_port: int = 26666, launcher_host: str = "127.0.0.1",
                 launcher_port: int = 26116):
        self.listen_port = listen_port
        self.launcher_host = launcher_host
        self.launcher_port = launcher_port
        self._server = None
        self._connections = 0

        # Intercepted data
        self.account_data: str | None = None
        self.game_token: str | None = None
        self.instance_id: str | None = None

        # Track settings_get key for autoConnectType interception
        self._last_settings_key: str | None = None

        # Event that fires when we have a game token
        self.token_ready = asyncio.Event()

    async def start(self):
        """Start the proxy."""
        self._server = await asyncio.start_server(
            self._handle_client, "127.0.0.1", self.listen_port,
        )
        logger.info(f"Fake Launcher listening on 127.0.0.1:{self.listen_port}")
        logger.info(f"  Proxying to real Launcher: {self.launcher_host}:{self.launcher_port}")

    async def stop(self):
        if self._server:
            self._server.close()
            await self._server.wait_closed()
        logger.info("Fake Launcher stopped.")

    async def _handle_client(self, client_reader: asyncio.StreamReader,
                              client_writer: asyncio.StreamWriter):
        """Handle a Dofus.exe connection - proxy everything to real Launcher."""
        self._connections += 1
        conn_id = self._connections
        peer = client_writer.get_extra_info("peername")
        logger.connection(f"[FakeLauncher #{conn_id}] Dofus.exe connected from {peer}")

        # Connect to real Launcher
        try:
            launcher_reader, launcher_writer = await asyncio.open_connection(
                self.launcher_host, self.launcher_port,
            )
            logger.connection(f"[FakeLauncher #{conn_id}] Connected to real Launcher")
        except Exception as e:
            logger.error(f"[FakeLauncher #{conn_id}] Cannot connect to Launcher: {e}")
            client_writer.close()
            return

        # Bidirectional forwarding with interception
        try:
            await asyncio.gather(
                self._forward_and_intercept(
                    client_reader, launcher_writer,
                    "game_to_launcher", conn_id,
                ),
                self._forward_and_intercept(
                    launcher_reader, client_writer,
                    "launcher_to_game", conn_id,
                ),
            )
        except Exception as e:
            logger.warn(f"[FakeLauncher #{conn_id}] Connection ended: {e}")
        finally:
            logger.connection(f"[FakeLauncher #{conn_id}] Connection closed")
            for w in (client_writer, launcher_writer):
                try:
                    w.close()
                except Exception:
                    pass

    async def _forward_and_intercept(self, reader: asyncio.StreamReader,
                                      writer: asyncio.StreamWriter,
                                      direction: str, conn_id: int):
        """Forward data while intercepting Thrift messages."""
        is_from_game = direction == "game_to_launcher"
        dir_label = "GAME >>> LAUNCHER" if is_from_game else "LAUNCHER >>> GAME"
        dir_color = logger.Colors.GREEN if is_from_game else logger.Colors.BLUE

        while True:
            try:
                data = await reader.read(65536)
            except (ConnectionResetError, ConnectionAbortedError):
                break
            if not data:
                break

            # Try to decode/log/modify Thrift messages (non-blocking, best effort)
            data = self._try_intercept(data, direction, conn_id)

            # Forward (possibly modified) data
            try:
                writer.write(data)
                await writer.drain()
            except (ConnectionResetError, ConnectionAbortedError, BrokenPipeError):
                break

    def _try_intercept(self, data: bytes, direction: str, conn_id: int) -> bytes:
        """Try to decode Thrift messages, extract useful data, and modify if needed.

        Returns the (possibly modified) data bytes.
        """
        modified_data = data
        pos = 0
        while pos < len(data):
            remaining = data[pos:]
            if len(remaining) < 4:
                break

            # Check for valid Thrift header
            header = struct.unpack(">I", remaining[:4])[0]
            if (header & VERSION_MASK) != (VERSION_1 & 0xFFFF0000):
                pos += 1
                continue

            try:
                decoder = ThriftDecoder(remaining)
                msg = decoder.decode_message()
                pos += decoder.bytes_consumed
            except (ValueError, Exception):
                # Can't decode, skip
                break

            # Log the message
            is_from_game = direction == "game_to_launcher"
            arrow = ">>>" if is_from_game else "<<<"
            logger.info(f"[IPC #{conn_id}] {arrow} {msg.type_name} {msg.method}")

            # Extract key data
            if msg.method == "connect" and msg.is_call:
                self.instance_id = msg.get_string(4)
                game = msg.get_string(1)
                variant = msg.get_string(2)
                logger.info(f"  game={game}, variant={variant}, instance={self.instance_id}")

            elif msg.method == "userInfo_get" and msg.is_reply:
                account_json = msg.get_string(0)
                if account_json:
                    self.account_data = account_json
                    # Parse and show key info
                    try:
                        import json
                        info = json.loads(account_json)
                        logger.info(f"  Account: {info.get('nickname', '?')} "
                                    f"(id: {info.get('id', '?')})")
                    except Exception:
                        logger.info(f"  Account data: {len(account_json)} chars")

            elif msg.method == "auth_getGameToken" and msg.is_reply:
                token = msg.get_string(0)
                if token:
                    self.game_token = token
                    self.token_ready.set()
                    logger.info(f"  Game token: {token}")

            elif msg.method == "settings_get":
                if msg.is_call:
                    key = msg.get_string(2)
                    self._last_settings_key = key
                    logger.info(f"  key={key}")
                elif msg.is_reply:
                    val = msg.get_string(0)
                    logger.info(f"  value={val}")

                    # Force autoConnectType to 0 (plain TCP on port 5555)
                    # instead of 1 (TLS on port 443).
                    # The CALL may be missed (batched in same TCP segment),
                    # so also apply when _last_settings_key is None.
                    should_modify = (
                        val == "1"
                        and self._last_settings_key in ("autoConnectType", None)
                    )
                    if should_modify:
                        old_pattern = b'\x0b\x00\x00\x00\x00\x00\x01\x31'
                        new_pattern = b'\x0b\x00\x00\x00\x00\x00\x01\x30'
                        if old_pattern in modified_data:
                            modified_data = modified_data.replace(
                                old_pattern, new_pattern, 1,
                            )
                            logger.info(f"  MODIFIED: autoConnectType 1 -> 0 (force plain TCP)")
                        else:
                            logger.warn(f"  Could not find autoConnectType pattern to modify")
                    self._last_settings_key = None

        return modified_data


# ---------------------------------------------------------------------------
# Entry point
# ---------------------------------------------------------------------------

async def run_fake_launcher(listen_port: int = 26666, launcher_port: int = 26116):
    """Run the transparent IPC proxy until interrupted."""
    launcher = FakeLauncher(
        listen_port=listen_port,
        launcher_port=launcher_port,
    )
    await launcher.start()
    print()

    logger.info("Ready! Click 'Jouer' in Ankama Launcher to start Dofus.")
    logger.info("(zaap-start.bat must redirect ZAAP_PORT to our port)")
    print()

    try:
        await asyncio.Event().wait()
    except asyncio.CancelledError:
        pass
    finally:
        await launcher.stop()


def main():
    import argparse

    parser = argparse.ArgumentParser(description="Fake Ankama Launcher (transparent IPC proxy)")
    parser.add_argument("--port", type=int, default=26666,
                        help="Listen port (default: 26666)")
    parser.add_argument("--launcher-port", type=int, default=26116,
                        help="Real Launcher port (default: 26116)")
    args = parser.parse_args()

    logger.banner()
    logger.info("Fake Launcher Mode (Transparent Proxy)")
    print()

    try:
        asyncio.run(run_fake_launcher(
            listen_port=args.port,
            launcher_port=args.launcher_port,
        ))
    except KeyboardInterrupt:
        print()
        logger.info("Fake Launcher stopped (Ctrl+C)")


if __name__ == "__main__":
    main()

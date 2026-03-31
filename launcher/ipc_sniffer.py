"""
IPC Sniffer - TCP proxy between Dofus.exe and the Ankama Launcher.

Captures ALL traffic between the game client and the Launcher IPC server
to reverse-engineer the IPC protocol.

Architecture:
    Dofus.exe (--port 26667) --> IPC Sniffer (26667) --> Real Launcher (26116)

Usage:
    python -m launcher.ipc_sniffer
    python -m launcher.ipc_sniffer --listen-port 26667 --launcher-port 26116
    python -m launcher.ipc_sniffer --output captures/ipc_capture.jsonl
"""

import asyncio
import json
import os
import struct
import time
from datetime import datetime

from utils import logger


# ---------------------------------------------------------------------------
# Message decoding attempts
# ---------------------------------------------------------------------------

def try_decode_message(data: bytes) -> dict | None:
    """Try multiple strategies to decode IPC data."""

    # Strategy 1: Newline-delimited JSON (most common IPC)
    result = _try_json_newline(data)
    if result:
        return result

    # Strategy 2: Length-prefixed JSON (4-byte big-endian length + JSON)
    result = _try_length_prefixed_json(data)
    if result:
        return result

    # Strategy 3: Length-prefixed JSON (4-byte little-endian)
    result = _try_length_prefixed_json_le(data)
    if result:
        return result

    # Strategy 4: Raw UTF-8 text
    result = _try_raw_text(data)
    if result:
        return result

    # Strategy 5: Thrift binary protocol (Jitsuri uses Thrift)
    result = _try_thrift(data)
    if result:
        return result

    return None


def _try_json_newline(data: bytes) -> dict | None:
    """Try parsing as newline-delimited JSON."""
    try:
        text = data.decode("utf-8").strip()
        if not text:
            return None
        messages = []
        for line in text.split("\n"):
            line = line.strip()
            if line:
                obj = json.loads(line)
                messages.append(obj)
        if messages:
            return {"format": "json_newline", "messages": messages}
    except (UnicodeDecodeError, json.JSONDecodeError):
        pass
    return None


def _try_length_prefixed_json(data: bytes) -> dict | None:
    """Try parsing as 4-byte big-endian length + JSON payload."""
    if len(data) < 5:
        return None
    try:
        length = struct.unpack(">I", data[:4])[0]
        if 1 <= length <= len(data) - 4:
            payload = data[4:4 + length]
            obj = json.loads(payload.decode("utf-8"))
            return {"format": "length_prefixed_be", "length": length, "message": obj}
    except (struct.error, UnicodeDecodeError, json.JSONDecodeError):
        pass
    return None


def _try_length_prefixed_json_le(data: bytes) -> dict | None:
    """Try parsing as 4-byte little-endian length + JSON payload."""
    if len(data) < 5:
        return None
    try:
        length = struct.unpack("<I", data[:4])[0]
        if 1 <= length <= len(data) - 4:
            payload = data[4:4 + length]
            obj = json.loads(payload.decode("utf-8"))
            return {"format": "length_prefixed_le", "length": length, "message": obj}
    except (struct.error, UnicodeDecodeError, json.JSONDecodeError):
        pass
    return None


def _try_raw_text(data: bytes) -> dict | None:
    """Try parsing as raw UTF-8 text."""
    try:
        text = data.decode("utf-8")
        # Only accept if mostly printable
        printable = sum(1 for c in text if c.isprintable() or c in '\n\r\t')
        if printable / max(len(text), 1) > 0.8:
            return {"format": "text", "text": text}
    except UnicodeDecodeError:
        pass
    return None


def _try_thrift(data: bytes) -> dict | None:
    """Try detecting Thrift binary protocol markers."""
    if len(data) < 4:
        return None
    # Thrift binary protocol starts with version 0x80010000 (strict) or method name length
    if data[0:2] == b'\x80\x01':
        return {"format": "thrift_binary", "version": "strict", "hint": "Thrift binary protocol detected"}
    # Thrift compact protocol: first byte has protocol id 0x82
    if data[0] == 0x82:
        return {"format": "thrift_compact", "hint": "Thrift compact protocol detected"}
    return None


def find_ascii_strings(data: bytes, min_len=4) -> list[str]:
    """Extract printable ASCII strings from binary data."""
    strings = []
    current = []
    for b in data:
        if 32 <= b < 127:
            current.append(chr(b))
        else:
            if len(current) >= min_len:
                strings.append("".join(current))
            current = []
    if len(current) >= min_len:
        strings.append("".join(current))
    return strings


# ---------------------------------------------------------------------------
# Capture file writer
# ---------------------------------------------------------------------------

class IPCCapture:
    """Writes IPC traffic to a JSONL capture file."""

    def __init__(self, filepath: str):
        self.filepath = filepath
        self._file = None
        self._start_time = None
        self._count = 0

    def start(self):
        os.makedirs(os.path.dirname(self.filepath), exist_ok=True)
        self._file = open(self.filepath, "a", encoding="utf-8")
        self._start_time = time.time()
        self._count = 0
        self._write({
            "type": "session_start",
            "ts": self._start_time,
            "time": datetime.now().isoformat(),
        })

    def stop(self):
        if self._file:
            self._write({
                "type": "session_end",
                "ts": time.time(),
                "count": self._count,
            })
            self._file.close()
            self._file = None

    def log(self, direction: str, raw: bytes, decoded: dict | None = None,
            strings: list[str] | None = None):
        """Log a single IPC message exchange."""
        if not self._file:
            return

        entry = {
            "type": "ipc",
            "ts": time.time(),
            "ms": int((time.time() - self._start_time) * 1000),
            "dir": direction,  # "game_to_launcher" or "launcher_to_game"
            "size": len(raw),
            "raw_hex": raw.hex(),
        }

        if decoded:
            entry["decoded"] = decoded

        if strings:
            entry["strings"] = strings

        self._write(entry)
        self._count += 1

    def _write(self, obj):
        if self._file:
            self._file.write(json.dumps(obj, ensure_ascii=False) + "\n")
            self._file.flush()

    @property
    def count(self):
        return self._count


# ---------------------------------------------------------------------------
# TCP Proxy (sniffer)
# ---------------------------------------------------------------------------

class IPCSniffer:
    """Async TCP proxy that captures IPC traffic between Dofus and the Launcher."""

    def __init__(self, listen_port: int = 26667, launcher_port: int = 26116,
                 launcher_host: str = "127.0.0.1", output_file: str | None = None):
        self.listen_port = listen_port
        self.launcher_port = launcher_port
        self.launcher_host = launcher_host

        if output_file is None:
            ts = datetime.now().strftime("%Y%m%d_%H%M%S")
            output_file = os.path.join(
                os.path.dirname(os.path.dirname(__file__)),
                "captures", f"ipc_{ts}.jsonl",
            )

        self.capture = IPCCapture(output_file)
        self._server = None
        self._connections = 0

    async def start(self):
        """Start the IPC sniffer proxy."""
        self.capture.start()
        self._server = await asyncio.start_server(
            self._handle_client, "127.0.0.1", self.listen_port,
        )
        logger.info(f"IPC Sniffer listening on 127.0.0.1:{self.listen_port}")
        logger.info(f"  Forwarding to Launcher on {self.launcher_host}:{self.launcher_port}")
        logger.info(f"  Capture file: {self.capture.filepath}")
        print()

    async def stop(self):
        """Stop the sniffer."""
        if self._server:
            self._server.close()
            await self._server.wait_closed()
        self.capture.stop()
        logger.info(f"IPC Sniffer stopped. {self.capture.count} messages captured.")

    async def _handle_client(self, client_reader: asyncio.StreamReader,
                              client_writer: asyncio.StreamWriter):
        """Handle a new connection from Dofus.exe."""
        self._connections += 1
        conn_id = self._connections
        peer = client_writer.get_extra_info("peername")
        logger.connection(f"[IPC #{conn_id}] Dofus.exe connected from {peer}")

        try:
            # Connect to real Launcher
            launcher_reader, launcher_writer = await asyncio.open_connection(
                self.launcher_host, self.launcher_port,
            )
            logger.connection(f"[IPC #{conn_id}] Connected to Launcher on {self.launcher_host}:{self.launcher_port}")
        except Exception as e:
            logger.error(f"[IPC #{conn_id}] Cannot connect to Launcher: {e}")
            logger.error(f"  Is the Ankama Launcher running? (expected on port {self.launcher_port})")
            client_writer.close()
            return

        # Bidirectional forwarding with capture
        try:
            await asyncio.gather(
                self._forward(client_reader, launcher_writer, "game_to_launcher", conn_id),
                self._forward(launcher_reader, client_writer, "launcher_to_game", conn_id),
            )
        except Exception as e:
            logger.warn(f"[IPC #{conn_id}] Connection ended: {e}")
        finally:
            logger.connection(f"[IPC #{conn_id}] Connection closed")
            for w in (client_writer, launcher_writer):
                try:
                    w.close()
                except Exception:
                    pass

    async def _forward(self, reader: asyncio.StreamReader, writer: asyncio.StreamWriter,
                        direction: str, conn_id: int):
        """Forward data from reader to writer, capturing everything."""
        dir_label = "GAME >>> LAUNCHER" if direction == "game_to_launcher" else "LAUNCHER >>> GAME"
        dir_color = logger.Colors.GREEN if direction == "game_to_launcher" else logger.Colors.BLUE

        msg_count = 0
        while True:
            try:
                data = await reader.read(65536)
            except (ConnectionResetError, ConnectionAbortedError):
                break

            if not data:
                break

            msg_count += 1

            # Try to decode
            decoded = try_decode_message(data)
            strings = find_ascii_strings(data)

            # Log to capture file
            self.capture.log(direction, data, decoded, strings if strings else None)

            # Print to console
            print(f"{logger.Colors.GRAY}[{logger._timestamp()}]{logger.Colors.RESET} "
                  f"{dir_color}{logger.Colors.BOLD}{dir_label}{logger.Colors.RESET} "
                  f"#{conn_id}.{msg_count} "
                  f"{logger.Colors.DIM}({len(data)} bytes){logger.Colors.RESET}")

            # Show decoded content
            if decoded:
                fmt = decoded.get("format", "?")
                print(f"  {logger.Colors.CYAN}Format: {fmt}{logger.Colors.RESET}")
                if fmt in ("json_newline",):
                    for msg in decoded.get("messages", []):
                        print(f"  {logger.Colors.WHITE}{json.dumps(msg, ensure_ascii=False)}{logger.Colors.RESET}")
                elif fmt in ("length_prefixed_be", "length_prefixed_le"):
                    msg = decoded.get("message", {})
                    print(f"  {logger.Colors.WHITE}{json.dumps(msg, ensure_ascii=False)}{logger.Colors.RESET}")
                elif fmt == "text":
                    text = decoded.get("text", "")
                    # Show first 200 chars
                    if len(text) > 200:
                        text = text[:200] + "..."
                    print(f"  {logger.Colors.WHITE}{repr(text)}{logger.Colors.RESET}")
                elif fmt in ("thrift_binary", "thrift_compact"):
                    print(f"  {logger.Colors.YELLOW}{decoded.get('hint', '')}{logger.Colors.RESET}")
            else:
                # Show hex dump for unknown format
                hex_preview = data[:64].hex()
                if len(data) > 64:
                    hex_preview += f" ... (+{len(data) - 64} bytes)"
                print(f"  {logger.Colors.DIM}HEX: {hex_preview}{logger.Colors.RESET}")

            # Show extracted strings
            if strings:
                print(f"  {logger.Colors.YELLOW}Strings: {strings[:10]}{logger.Colors.RESET}")
                if len(strings) > 10:
                    print(f"  {logger.Colors.DIM}  ... +{len(strings) - 10} more{logger.Colors.RESET}")

            # Forward to destination
            try:
                writer.write(data)
                await writer.drain()
            except (ConnectionResetError, ConnectionAbortedError, BrokenPipeError):
                break


# ---------------------------------------------------------------------------
# Entry point
# ---------------------------------------------------------------------------

async def run_sniffer(listen_port: int = 26667, launcher_port: int = 26116,
                      output_file: str | None = None):
    """Run the IPC sniffer until interrupted."""
    sniffer = IPCSniffer(
        listen_port=listen_port,
        launcher_port=launcher_port,
        output_file=output_file,
    )
    await sniffer.start()

    logger.info("Waiting for Dofus.exe to connect...")
    logger.info(f"  Launch Dofus with: --port {listen_port}")
    print()
    logger.info("Press Ctrl+C to stop")
    print()

    try:
        await asyncio.Event().wait()  # Run forever
    except asyncio.CancelledError:
        pass
    finally:
        await sniffer.stop()


def main():
    import argparse

    parser = argparse.ArgumentParser(description="IPC Sniffer - Capture Dofus<->Launcher IPC traffic")
    parser.add_argument("--listen-port", type=int, default=26667,
                        help="Port to listen on (default: 26667)")
    parser.add_argument("--launcher-port", type=int, default=26116,
                        help="Real Launcher IPC port (default: 26116)")
    parser.add_argument("--output", "-o", default=None,
                        help="Output JSONL file (default: captures/ipc_TIMESTAMP.jsonl)")
    args = parser.parse_args()

    logger.banner()
    logger.info("IPC Sniffer Mode")
    logger.info(f"  Sniffing: Dofus.exe (:{args.listen_port}) --> Launcher (:{args.launcher_port})")
    print()

    try:
        asyncio.run(run_sniffer(
            listen_port=args.listen_port,
            launcher_port=args.launcher_port,
            output_file=args.output,
        ))
    except KeyboardInterrupt:
        print()
        logger.info("IPC Sniffer stopped (Ctrl+C)")


if __name__ == "__main__":
    main()

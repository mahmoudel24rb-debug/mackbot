"""
Dofus 3 MITM Proxy - Async TCP proxy that intercepts and logs all packets.

Architecture: A single proxy on port 5555 handles BOTH login and game connections.

Flow:
  1. Client connects to 127.0.0.1:5555 -> proxy routes to login server
  2. Login flow: Identification -> IdentificationResponse -> SelectServer -> SSR
  3. Proxy intercepts SSR, rewrites game server host to 127.0.0.1, saves hostname
  4. Client disconnects, reconnects to 127.0.0.1:5555 with a game auth ticket
  5. Proxy detects the game auth packet and routes to the REAL game server
  6. All game packets are now visible through the proxy

No separate game proxy needed - the same port handles everything.
"""

import asyncio
import struct
import re
import os
from proxy.packet_handler import (
    PacketBuffer, format_packet_info, format_raw_fields,
    decode_protobuf_fields, encode_protobuf_fields, decode_varint,
    WIRE_LENGTH_DELIMITED, WIRE_VARINT,
    _is_valid_protobuf,
)
from game.state import GameState
from game.dofus_message import extract_message_info, get_type_name
from game.message_handlers import register_all_handlers
from game.navigation import Navigator
from game.gathering import GatherController
from protocol.auto_matcher import AutoMatcher
from utils import logger
from utils.capture import PacketCapture
import config


PACKET_LOG_FILE = os.path.join(os.path.dirname(os.path.dirname(__file__)), "packets.bin")


# ---------------------------------------------------------------------------
# Helpers
# ---------------------------------------------------------------------------

def find_strings_in_bytes(data):
    """Find all printable ASCII strings (4+ chars) in binary data."""
    strings = []
    current = []
    for b in data:
        if 32 <= b < 127:
            current.append(chr(b))
        else:
            if len(current) >= 4:
                strings.append("".join(current))
            current = []
    if len(current) >= 4:
        strings.append("".join(current))
    return strings


def _try_extract_port_from_bytes(data):
    """Try to decode raw bytes as consecutive varints, return first port-like value."""
    pos = 0
    while pos < len(data):
        try:
            val, pos = decode_varint(data, pos)
            if 1024 <= val <= 65535:
                return val
        except ValueError:
            break
    return None


# ---------------------------------------------------------------------------
# Packet type detection
# ---------------------------------------------------------------------------

def is_select_server(payload):
    """
    Check if a payload is a SelectServer request from the client.
    LoginMessage { field 1 { field 4 = SelectServer } }
    """
    fields = decode_protobuf_fields(payload)
    for fn, wt, val in fields:
        if fn == 1 and wt == WIRE_LENGTH_DELIMITED:
            inner = decode_protobuf_fields(val)
            for ifn, iwt, _ in inner:
                if ifn == 4 and iwt == WIRE_LENGTH_DELIMITED:
                    return True
    return False


def _has_field_4(data):
    """Check if protobuf data has a length-delimited field 4 (game auth marker)."""
    fields = decode_protobuf_fields(data)
    if not fields:
        return False
    for fn, wt, val in fields:
        if fn == 4 and wt == WIRE_LENGTH_DELIMITED:
            # Extra check: look for 'ankama' or 'jol' in the field content
            try:
                text = val.decode("utf-8")
                if "ankama" in text or "jol" in text:
                    logger.debug(f"  [AUTH] Found field 4 with type URL: {text}")
                    return True
            except (UnicodeDecodeError, ValueError):
                pass
            # Could be nested - check inside
            inner = decode_protobuf_fields(val)
            for ifn, iwt, ival in inner:
                if iwt == WIRE_LENGTH_DELIMITED:
                    try:
                        itext = ival.decode("utf-8")
                        if "ankama" in itext or "jol" in itext:
                            logger.debug(f"  [AUTH] Found nested type URL: {itext}")
                            return True
                    except (UnicodeDecodeError, ValueError):
                        pass
            # Field 4 exists even without type URL - still likely game auth
            logger.debug(f"  [AUTH] Found field 4 (len={len(val)}), no type URL detected")
            return True
    return False


def is_game_auth_packet(raw_data):
    """
    Check if raw TCP data is a game server auth packet.

    Game protocol messages contain 'type.ankama.com/' in the protobuf Any
    wrapper. Login protocol messages (account auth) never contain this prefix.
    The 3-letter code after the prefix rotates (was 'jol', now 'jrz', etc.)
    so we just check for the prefix.
    """
    if len(raw_data) < 10:
        return False

    found = b'type.ankama.com/' in raw_data
    if found:
        logger.debug(f"  [AUTH-CHECK] Found type.ankama.com/ -> GAME AUTH ({len(raw_data)} bytes)")
    else:
        logger.debug(f"  [AUTH-CHECK] No type.ankama.com/ -> LOGIN packet ({len(raw_data)} bytes)")
    return found


# ---------------------------------------------------------------------------
# SelectServerResponse detection
# ---------------------------------------------------------------------------

def extract_game_server_info(payload):
    """
    Parse a login server response to find game server host/port.
    SelectServerResponse is in field 4 of the Response wrapper (field 2).
    Returns (host, port) or (None, None).
    """
    fields = decode_protobuf_fields(payload)
    for fn, wt, val in fields:
        if fn == 2 and wt == WIRE_LENGTH_DELIMITED:
            resp_fields = decode_protobuf_fields(val)
            for rfn, rwt, rval in resp_fields:
                if rfn == 4 and rwt == WIRE_LENGTH_DELIMITED:
                    return _search_host_port(rval)
    return None, None


def _search_host_port(data):
    """Recursively search protobuf for host/port."""
    fields = decode_protobuf_fields(data)
    host = None
    port = None

    for fn, wt, val in fields:
        if wt == WIRE_LENGTH_DELIMITED:
            try:
                text = val.decode("utf-8")
                if re.match(r"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$", text):
                    host = text
                elif re.match(r"^[a-zA-Z0-9][\w.-]*\.[a-z]{2,}$", text) and len(text) > 5:
                    if not host:
                        host = text
            except (UnicodeDecodeError, ValueError):
                pass

            # Try as nested protobuf
            nested = decode_protobuf_fields(val)
            if nested and _is_valid_protobuf(nested, val):
                nh, np = _search_host_port(val)
                if nh:
                    host = nh
                if np:
                    port = np
            elif not port:
                # Not valid protobuf - try as packed varints (port list)
                candidate = _try_extract_port_from_bytes(val)
                if candidate:
                    port = candidate

        elif wt == WIRE_VARINT:
            if 1024 <= val <= 65535:
                port = val

    if not host:
        for s in find_strings_in_bytes(data):
            if re.match(r"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}", s):
                host = s
                break

    return host, port


# ---------------------------------------------------------------------------
# Protobuf rewriting (host only - port stays the same)
# ---------------------------------------------------------------------------

def rewrite_select_server_response(payload, new_host):
    """
    Rewrite the game server HOST in a SelectServerResponse.
    Port is left untouched (it's in a packed bytes field and uses the same
    port as the login server).

    Returns (new_payload, original_host, original_port) or (None, None, None).
    """
    orig_host, orig_port = extract_game_server_info(payload)
    if not orig_host:
        return None, None, None

    outer_fields = decode_protobuf_fields(payload)
    new_outer = []

    for fn, wt, val in outer_fields:
        if fn == 2 and wt == WIRE_LENGTH_DELIMITED:
            resp_fields = decode_protobuf_fields(val)
            new_resp = []
            for rfn, rwt, rval in resp_fields:
                if rfn == 4 and rwt == WIRE_LENGTH_DELIMITED:
                    new_ssr = _replace_host_string(rval, orig_host, new_host)
                    new_resp.append((rfn, rwt, new_ssr))
                else:
                    new_resp.append((rfn, rwt, rval))
            new_outer.append((fn, wt, encode_protobuf_fields(new_resp)))
        else:
            new_outer.append((fn, wt, val))

    return encode_protobuf_fields(new_outer), orig_host, orig_port


def _replace_host_string(data, old_host, new_host, depth=0):
    """Recursively replace a host string in protobuf data."""
    if depth > 8:
        return data
    fields = decode_protobuf_fields(data)
    if not fields:
        return data

    new_fields = []
    modified = False

    for fn, wt, val in fields:
        if wt == WIRE_LENGTH_DELIMITED:
            try:
                text = val.decode("utf-8")
                if text == old_host:
                    new_fields.append((fn, wt, new_host.encode("utf-8")))
                    modified = True
                    continue
            except (UnicodeDecodeError, ValueError):
                pass
            # Recurse into nested protobuf
            nested = decode_protobuf_fields(val)
            if nested and _is_valid_protobuf(nested, val):
                new_val = _replace_host_string(val, old_host, new_host, depth + 1)
                if new_val is not val:
                    new_fields.append((fn, wt, new_val))
                    modified = True
                    continue
            new_fields.append((fn, wt, val))
        else:
            new_fields.append((fn, wt, val))

    if modified:
        return encode_protobuf_fields(new_fields)
    return data


# ---------------------------------------------------------------------------
# MITM Proxy
# ---------------------------------------------------------------------------

class MITMProxy:
    def __init__(self, listen_host=None, listen_port=None, server_host=None,
                 server_port=None, game_state=None, event_bus=None,
                 enable_console=True):
        self.listen_host = listen_host or config.PROXY_HOST
        self.listen_port = listen_port or config.PROXY_PORT
        self.server_host = server_host or config.SERVER_HOST or config.SERVER_HOSTNAME
        self.server_port = server_port or config.SERVER_PORT
        self.enable_console = enable_console
        self.connections = 0
        # Populated when SelectServerResponse is detected
        self.game_server_host = None
        self.game_server_port = None

        # Accept injected components (from Orchestrator) or create standalone
        if game_state is not None:
            self.game_state = game_state
            # Navigator/gatherer/spell_manager already set up by Orchestrator
            self.navigator = game_state.navigator
            self.gatherer = game_state.gatherer
            from game.script_engine import ScriptEngine
            self.script_engine = ScriptEngine(
                self.game_state,
                self.navigator,
                self.gatherer,
            )
        else:
            # Standalone mode: create everything internally
            self.game_state = GameState()
            self.navigator = Navigator(self.game_state)
            self.game_state.navigator = self.navigator
            self.gatherer = GatherController(self.game_state)
            self.game_state.gatherer = self.gatherer
            from game.spell_manager import SpellManager
            self.game_state.spell_manager = SpellManager()
            from game.script_engine import ScriptEngine
            self.script_engine = ScriptEngine(
                self.game_state, self.navigator, self.gatherer,
            )
            register_all_handlers(self.game_state)

        # Optional event bus (from Orchestrator for UI events)
        self.event_bus = event_bus

        # Auto-matcher: discovers 3-letter codes from traffic structure
        self.auto_matcher = AutoMatcher(self.game_state.matching) if getattr(config, "AUTO_MATCH", True) else None
        # Packet capture for protocol analysis
        self.capture = PacketCapture()
        # Upstream port pool (cycles through range to avoid TIME_WAIT conflicts)
        self._upstream_port_idx = 0

    async def start(self, blocking=True):
        """Start the proxy server.

        If blocking=True (default, standalone mode), runs serve_forever().
        If blocking=False (launcher integration), returns immediately.
        Call stop() to shut down when blocking=False.
        """
        import socket as _socket

        # Create explicit IPv4 socket with SO_REUSEADDR for reliable binding
        sock = _socket.socket(_socket.AF_INET, _socket.SOCK_STREAM)
        sock.setsockopt(_socket.SOL_SOCKET, _socket.SO_REUSEADDR, 1)
        sock.bind((self.listen_host, self.listen_port))
        sock.listen(100)
        sock.setblocking(False)

        self._server = await asyncio.start_server(
            self._handle_client, sock=sock
        )
        addr = self._server.sockets[0].getsockname()
        logger.info(f"Proxy listening on {addr[0]}:{addr[1]}")
        logger.info(f"Login server: {self.server_host}:{self.server_port}")
        logger.info("Waiting for Dofus client connection...")

        if blocking:
            async with self._server:
                await self._server.serve_forever()
        else:
            # Run serve_forever() in a background task to ensure accepts
            # are properly processed on Windows ProactorEventLoop
            self._serve_task = asyncio.create_task(self._serve_background())

    async def _serve_background(self):
        """Background task that keeps the server accepting connections."""
        async with self._server:
            await self._server.serve_forever()

    async def stop(self):
        """Stop the proxy server."""
        if hasattr(self, '_server') and self._server:
            self._server.close()
            await self._server.wait_closed()
        if hasattr(self, '_serve_task') and self._serve_task:
            self._serve_task.cancel()
            try:
                await self._serve_task
            except asyncio.CancelledError:
                pass
        logger.info("MITM Proxy stopped.")

    async def _handle_client(self, client_reader, client_writer):
        self.connections += 1
        conn_id = self.connections
        client_addr = client_writer.get_extra_info("peername")
        logger.connection(f"[#{conn_id}] Client from {client_addr[0]}:{client_addr[1]}")

        # Read first data to determine: login or game connection?
        try:
            first_data = await asyncio.wait_for(client_reader.read(65536), timeout=10.0)
        except (asyncio.TimeoutError, ConnectionError):
            client_writer.close()
            return

        if not first_data:
            client_writer.close()
            return

        # Debug: show everything about the incoming data
        logger.debug(f"[#{conn_id}] First data: {len(first_data)} bytes")
        logger.debug(f"[#{conn_id}] game_server_host = {self.game_server_host}")
        logger.debug(f"[#{conn_id}] game_server_port = {self.game_server_port}")

        # Show hex dump of first bytes
        hex_first = " ".join(f"{b:02x}" for b in first_data[:64])
        logger.debug(f"[#{conn_id}] Hex: {hex_first}")

        # Show all strings found in the data
        strings = find_strings_in_bytes(first_data)
        if strings:
            logger.debug(f"[#{conn_id}] Strings found: {strings}")

        # Route based on packet type
        is_game = self.game_server_host and is_game_auth_packet(first_data)
        logger.debug(f"[#{conn_id}] is_game_auth_packet result: {is_game}")

        if is_game:
            target_host = self.game_server_host
            target_port = self.game_server_port or self.server_port
            conn_label = "GAME"
            logger.info(f"[#{conn_id}] Game auth detected -> {target_host}:{target_port}")
        else:
            target_host = self.server_host
            target_port = self.server_port
            conn_label = "LOGIN"
            if self.game_server_host:
                logger.warn(f"[#{conn_id}] game_server_host is SET but packet was NOT detected as game auth!")

        # Connect to the target server
        # If upstream_src_port is set, bind to a port in the exclusion range
        # (cycles through pool to avoid TIME_WAIT conflicts)
        try:
            base_port = getattr(self, 'upstream_src_port', None)
            if base_port:
                from launcher.packet_redirect import PROXY_UPSTREAM_PORT_BASE, PROXY_UPSTREAM_PORT_END
                pool_size = PROXY_UPSTREAM_PORT_END - PROXY_UPSTREAM_PORT_BASE + 1
                import socket as _sock
                # Resolve hostname to IP if needed
                resolved_host = target_host
                if not re.match(r'^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$', target_host):
                    import socket as _dns_sock
                    try:
                        resolved_host = _dns_sock.gethostbyname(target_host)
                        logger.info(f"[#{conn_id}] Resolved {target_host} -> {resolved_host}")
                    except _dns_sock.gaierror as e:
                        logger.error(f"[#{conn_id}] DNS resolution failed for {target_host}: {e}")
                        client_writer.close()
                        return
                # Try ports from the pool
                sock = None
                for attempt in range(pool_size):
                    port = PROXY_UPSTREAM_PORT_BASE + (self._upstream_port_idx % pool_size)
                    self._upstream_port_idx += 1
                    try:
                        sock = _sock.socket(_sock.AF_INET, _sock.SOCK_STREAM)
                        sock.setsockopt(_sock.SOL_SOCKET, _sock.SO_REUSEADDR, 1)
                        sock.bind(('', port))
                        sock.setblocking(False)
                        loop = asyncio.get_event_loop()
                        await loop.sock_connect(sock, (resolved_host, target_port))
                        logger.debug(f"[#{conn_id}] Upstream bound to port {port}")
                        break
                    except OSError:
                        sock.close()
                        sock = None
                        continue
                if sock is None:
                    raise OSError(f"All upstream ports {PROXY_UPSTREAM_PORT_BASE}-{PROXY_UPSTREAM_PORT_END} busy")
                server_reader, server_writer = await asyncio.open_connection(sock=sock)
            else:
                server_reader, server_writer = await asyncio.open_connection(
                    target_host, target_port
                )
            logger.connection(f"[{conn_label} #{conn_id}] Connected to {target_host}:{target_port}")
        except (ConnectionRefusedError, OSError) as e:
            logger.error(f"[{conn_label} #{conn_id}] Cannot connect to {target_host}:{target_port}: {e}")
            client_writer.close()
            return

        # Give the navigator access to the game server writer
        if conn_label == "GAME":
            self.navigator.set_server_writer(server_writer)
            logger.info(f"[#{conn_id}] Navigator connected - pathfinding & movement ready")

        # Forward the initial data to the server
        server_writer.write(first_data)
        await server_writer.drain()

        # Set up packet analysis buffers
        c2s_buffer = PacketBuffer()
        s2c_buffer = PacketBuffer()

        # Analyze initial data (already forwarded)
        c2s_buffer.feed(first_data)
        for payload in c2s_buffer.try_extract_packets():
            self._log_packet(payload, "c2s", conn_id, conn_label)
            self._save_packet(payload, "c2s")
            if conn_label == "GAME":
                self._process_game_packet(payload, "c2s")

        # Per-connection state (shared between c2s and s2c tasks via dict)
        state = {"select_server_sent": False, "ssr_handled": False}

        c2s_task = asyncio.create_task(
            self._forward(
                client_reader, server_writer, c2s_buffer,
                "c2s", conn_id, conn_label, state
            )
        )
        s2c_task = asyncio.create_task(
            self._forward(
                server_reader, client_writer, s2c_buffer,
                "s2c", conn_id, conn_label, state
            )
        )

        done, pending = await asyncio.wait(
            [c2s_task, s2c_task],
            return_when=asyncio.FIRST_COMPLETED,
        )

        for task in pending:
            task.cancel()

        for writer in (client_writer, server_writer):
            try:
                writer.close()
                await writer.wait_closed()
            except Exception:
                pass

        logger.connection(f"[{conn_label} #{conn_id}] Connection closed")

    async def _forward(self, reader, writer, packet_buffer, direction,
                       conn_id, conn_label, state):
        """Forward data between client and server."""
        intercept_buffer = None

        try:
            while True:
                data = await reader.read(65536)
                if not data:
                    break

                # Intercept s2c only when SelectServer was sent and SSR not yet handled
                need_intercept = (
                    conn_label == "LOGIN"
                    and direction == "s2c"
                    and state["select_server_sent"]
                    and not state["ssr_handled"]
                )

                if need_intercept:
                    if intercept_buffer is None:
                        intercept_buffer = PacketBuffer()
                    await self._forward_intercept(
                        data, writer, intercept_buffer,
                        conn_id, conn_label, state
                    )
                else:
                    await self._forward_passthrough(
                        data, writer, packet_buffer,
                        direction, conn_id, conn_label, state
                    )

        except (ConnectionResetError, BrokenPipeError, asyncio.CancelledError):
            pass
        except Exception as e:
            logger.error(f"[{conn_label} #{conn_id}] Forward error ({direction}): {e}")

    async def _forward_passthrough(self, data, writer, packet_buffer,
                                   direction, conn_id, conn_label, state):
        """Forward data immediately, analyze afterward."""
        writer.write(data)
        await writer.drain()

        packet_buffer.feed(data)
        packets = packet_buffer.try_extract_packets()

        for payload in packets:
            self._log_packet(payload, direction, conn_id, conn_label)
            self._save_packet(payload, direction)

            # Feed game packets to the state parser
            if conn_label == "GAME":
                self._process_game_packet(payload, direction)

            # Detect SelectServer from client
            if (conn_label == "LOGIN" and direction == "c2s"
                    and not state["select_server_sent"]):
                if is_select_server(payload):
                    state["select_server_sent"] = True
                    logger.info("")
                    logger.info("SelectServer detected! Intercepting next server response...")

        if not packets and len(data) > 0 and packet_buffer.framing is None:
            self._log_raw_data(data, direction, conn_id, conn_label)

    async def _forward_intercept(self, data, writer, packet_buffer,
                                 conn_id, conn_label, state):
        """Buffer data, rewrite SSR if found, then forward."""
        packet_buffer.feed(data)
        packets = packet_buffer.try_extract_packets()

        for payload in packets:
            modified = payload

            # Try to rewrite SelectServerResponse (host only)
            new_payload, orig_host, orig_port = rewrite_select_server_response(
                payload, "127.0.0.1"
            )
            if new_payload:
                state["ssr_handled"] = True
                modified = new_payload

                # Save game server info for routing future connections
                self.game_server_host = orig_host
                self.game_server_port = orig_port

                logger.info("")
                logger.info(f"{'='*60}")
                logger.info(f"  GAME SERVER: {orig_host}:{orig_port}")
                logger.info(f"  HOST REWRITTEN TO: 127.0.0.1")
                logger.info(f"  Next connection will route to game server automatically")
                logger.info(f"{'='*60}")
                logger.info("")

                # Show original packet
                full_decode = format_raw_fields(payload, max_depth=6)
                for line in full_decode.split("\n"):
                    logger.info(f"  [original] {line}")

            self._log_packet(modified, "s2c", conn_id, conn_label)
            self._save_packet(payload, "s2c")

            # Re-frame: 4-byte big-endian length prefix + payload
            frame = struct.pack(">I", len(modified)) + modified
            writer.write(frame)

        # Forward any leftover buffer data
        if state["ssr_handled"] and len(packet_buffer.buffer) > 0:
            writer.write(bytes(packet_buffer.buffer))
            packet_buffer.buffer.clear()

        await writer.drain()

    # --- Game State ---

    def _process_game_packet(self, payload, direction):
        """Extract type URL messages and feed them to the game state."""
        messages = extract_message_info(payload, direction)
        for type_code, msg_data, uid in messages:
            # Auto-match before processing so handler dispatch finds the name
            if self.auto_matcher:
                self.auto_matcher.observe(type_code, msg_data or b"", direction)
            self.game_state.process_message(type_code, msg_data, direction, uid)
            # Capture for analysis
            if self.capture.active:
                name = get_type_name(type_code, self.game_state.matching)
                self.capture.log_message(type_code, name, direction, msg_data, uid)

    # --- Logging ---

    def _log_packet(self, payload, direction, conn_id, conn_label):
        msg_name, size, details = format_packet_info(payload, direction)

        # For game packets, add type URL name
        type_label = ""
        if conn_label == "GAME":
            messages = extract_message_info(payload, direction)
            if messages:
                names = [f"{get_type_name(code, self.game_state.matching)}({code})" for code, _, _ in messages]
                type_label = f" {' | '.join(names)}"

        if direction == "c2s":
            logger.client_to_server(f"[{conn_label}]{type_label}", size)
        else:
            logger.server_to_client(f"[{conn_label}]{type_label}", size)

        if config.VERBOSE and details:
            print(details)

        if config.VERBOSE and size > 0:
            raw = format_raw_fields(payload, max_depth=4)
            if raw:
                for line in raw.split("\n")[:30]:
                    print(f"  {logger.Colors.DIM}{line}{logger.Colors.RESET}")

    def _log_raw_data(self, data, direction, conn_id, conn_label):
        hex_preview = logger.hex_dump(data, config.HEX_DUMP_MAX_BYTES)
        if direction == "c2s":
            logger.client_to_server(f"[{conn_label}] RawTCP", len(data), hex_preview)
        else:
            logger.server_to_client(f"[{conn_label}] RawTCP", len(data), hex_preview)

    def _save_packet(self, payload, direction):
        """Save raw packet to binary log file for offline analysis."""
        try:
            with open(PACKET_LOG_FILE, "ab") as f:
                d = b"\x01" if direction == "c2s" else b"\x02"
                f.write(d + struct.pack(">I", len(payload)) + payload)
        except Exception:
            pass

    # --- Console commands ---

    async def console_loop(self):
        """Async stdin command loop for live bot testing."""
        loop = asyncio.get_event_loop()
        print()
        logger.info("Console ready. Type 'help' for commands.")
        print()

        while True:
            try:
                line = await loop.run_in_executor(None, input)
            except (EOFError, KeyboardInterrupt):
                break

            line = line.strip()
            if not line:
                continue

            parts = line.split()
            cmd = parts[0].lower()
            args = parts[1:]

            try:
                await self._handle_command(cmd, args)
            except Exception as e:
                logger.error(f"Command error: {e}")

    async def _handle_command(self, cmd, args):
        """Dispatch a console command."""
        gs = self.game_state
        nav = self.navigator

        if cmd in ("help", "h", "?"):
            self._cmd_help()

        elif cmd in ("state", "s"):
            print(gs.log_state())

        elif cmd in ("pos", "position"):
            pt = None
            if gs.character.cell_id is not None:
                from game.map_grid import cell_to_point
                pt = cell_to_point(gs.character.cell_id)
            logger.info(f"Cell: {gs.character.cell_id}  MapPoint: {pt}  pos_ref: 0x{gs.pos_ref:08X}" if gs.pos_ref else f"Cell: {gs.character.cell_id}  MapPoint: {pt}  pos_ref: None")

        elif cmd in ("move", "m"):
            if not args:
                logger.error("Usage: move <cell_id>")
                return
            target = int(args[0])
            if not nav.is_ready:
                logger.error("Navigator not ready (no connection or no pos_ref)")
                return
            logger.info(f"Moving to cell {target}...")
            ok = await nav.move_to(target)
            if ok:
                logger.info(f"Arrived at cell {gs.character.cell_id}")
            else:
                logger.error("Move failed!")

        elif cmd in ("path", "p"):
            if not args:
                logger.error("Usage: path <cell_id>")
                return
            target = int(args[0])
            current = gs.character.cell_id
            if current is None:
                logger.error("Unknown current position")
                return
            from game.pathfinding import find_path
            from game.map_grid import cell_to_point, compress_path
            path = find_path(current, target, nav.grid)
            if path:
                logger.info(f"Path ({len(path)} cells): {path}")
                compressed = compress_path(path)
                logger.info(f"Compressed ({len(compressed)} keys): {[f'0x{v:04X}' for v in compressed]}")
            else:
                logger.error(f"No path from {current} to {target}")

        elif cmd in ("map", "mapchange"):
            if not args:
                logger.error("Usage: map <target_ref_hex>")
                logger.info("Example: map 0B640C06")
                return
            ref_str = args[0].replace("0x", "").replace("0X", "")
            target_ref = int(ref_str, 16)
            if not nav.is_ready:
                logger.error("Navigator not ready")
                return
            logger.info(f"Requesting map change to ref 0x{target_ref:08X}...")
            ok = await nav.change_map(target_ref)
            if ok:
                logger.info(f"Map changed! Now at ({gs.map.x}, {gs.map.y})")
            else:
                logger.error("Map change failed!")

        elif cmd in ("walkto", "w"):
            if len(args) < 2:
                logger.error("Usage: walkto <edge_cell> <target_ref_hex>")
                return
            edge = int(args[0])
            ref_str = args[1].replace("0x", "").replace("0X", "")
            target_ref = int(ref_str, 16)
            if not nav.is_ready:
                logger.error("Navigator not ready")
                return
            logger.info(f"Walking to cell {edge} then changing map (ref 0x{target_ref:08X})...")
            ok = await nav.move_and_change_map(edge, target_ref)
            if ok:
                logger.info(f"Done! Now at ({gs.map.x}, {gs.map.y})")
            else:
                logger.error("Walk + map change failed!")

        elif cmd in ("entities", "e"):
            if not gs.entities:
                logger.info("No entities on map")
                return
            for eid, ent in gs.entities.items():
                logger.info(f"  {ent}")

        elif cmd in ("grid", "g"):
            walkable = sum(1 for i in range(len(nav.grid.walkable)) if nav.grid.walkable[i])
            mc = len(nav.grid.map_change_data)
            occ = len(nav.grid.occupied)
            logger.info(f"Grid: {walkable} walkable, {occ} occupied, {mc} map-change cells")

        elif cmd in ("ready", "r"):
            logger.info(f"Connected: {nav.movement.is_connected}")
            logger.info(f"Cell: {gs.character.cell_id}")
            logger.info(f"pos_ref: {'0x{:08X}'.format(gs.pos_ref) if gs.pos_ref else 'None'}")
            logger.info(f"Navigator ready: {nav.is_ready}")

        elif cmd in ("rawmove", "rm"):
            # Send a raw move: rawmove <dest> or rawmove <c1> <c2> ...
            if not args:
                logger.error("Usage: rawmove <dest> or rawmove <c1> <c2> ...")
                logger.info("Example: rawmove 410  (move from current cell to 410)")
                return
            if not nav.movement.is_connected or gs.pos_ref is None:
                logger.error("Not ready (no connection or no pos_ref)")
                return
            if gs.is_busy:
                logger.error(f"Character is busy ({gs.busy_reason}) - can't move!")
                return
            cells = [int(a) for a in args]
            # If only 1 cell given, build path from current cell
            if len(cells) == 1:
                if gs.character.cell_id is None:
                    logger.error("Unknown current cell - give start + dest")
                    return
                target = cells[0]
                current = gs.character.cell_id
                if current == target:
                    logger.info("Already at target cell")
                    return
                # Check if adjacent
                from game.map_grid import get_direction
                direction = get_direction(current, target)
                if direction >= 0:
                    # Adjacent: direct 2-cell path
                    cells = [current, target]
                else:
                    # NOT adjacent: use A* pathfinding
                    from game.pathfinding import find_path
                    path = find_path(current, target, nav.grid)
                    if path:
                        cells = path
                        logger.info(f"Path found: {len(cells)} cells")
                    else:
                        logger.error(f"No path from {current} to {target}")
                        return
            # Validate the compressed path won't be empty
            from game.map_grid import compress_path
            compressed = compress_path(cells)
            if not compressed:
                logger.error(f"Invalid path (no valid directions): {cells}")
                return
            logger.info(f"Raw move: {cells[0]} -> {cells[-1]} ({len(cells)} cells, {len(compressed)} keyframes)")
            ok = await nav.movement.move_to_cell(cells)
            if ok:
                arrived = await nav.movement.wait_move_complete(timeout=10.0)
                if arrived:
                    logger.info(f"Move OK! Cell: {gs.character.cell_id}")
                    await nav.movement.confirm_move()
                elif nav.movement._move_refused:
                    logger.error("Move REFUSED by server (ipd)")
                else:
                    logger.error("Move timed out (no ion received)")
            else:
                logger.error("Failed to send packet")

        elif cmd in ("capture", "cap"):
            # Toggle packet capture
            if self.capture.active:
                self.capture.stop()
                logger.info(f"Capture STOPPED ({self.capture.count} messages saved)")
                logger.info(f"  File: {self.capture.filepath}")
            else:
                self.capture.start()
                logger.info(f"Capture STARTED -> {self.capture.filepath}")
                logger.info("  All packets will be logged in JSONL format")
                logger.info("  Type 'capture' again to stop")

        elif cmd in ("mapview", "mv"):
            from game.map_view import render_map
            for line in render_map(gs, nav):
                logger.info(line)

        elif cmd in ("resources", "res"):
            resources = gs.map.resources
            if not resources:
                logger.info("No resources on this map (iou not received yet)")
                return
            avail = gs.map.get_available_resources()
            logger.info(f"Resources: {len(resources)} total, {len(avail)} available")
            for r in resources:
                status = "OK" if r.available else f"s{r.status}"
                logger.info(f"  elem={r.element_id} cell={r.cell_id} "
                           f"type={r.resource_type} skill={r.skill_uid} [{status}]")

        elif cmd in ("gather", "ga"):
            if not nav.is_ready:
                logger.error("Navigator not ready")
                return
            if gs.is_busy:
                logger.error(f"Character is busy ({gs.busy_reason})")
                return
            # Optional: filter by resource type
            res_type = int(args[0]) if args else None
            avail = gs.map.get_available_resources(res_type)
            if not avail:
                logger.error("No available resources" + (f" of type {res_type}" if res_type else ""))
                return
            # Pick closest resource
            current = gs.character.cell_id
            if current is not None:
                from game.map_grid import cell_to_point
                cp = cell_to_point(current)
                def dist(r):
                    if r.cell_id is None:
                        return 9999
                    rp = cell_to_point(r.cell_id)
                    return abs(rp[0] - cp[0]) + abs(rp[1] - cp[1])
                avail.sort(key=dist)
            target = avail[0]
            logger.info(f"Gathering: {target}")
            ok = await self.gatherer.gather_resource(target)
            if ok:
                logger.info("Gather complete!")
            else:
                logger.error("Gather failed!")

        elif cmd in ("farm",):
            if not nav.is_ready:
                logger.error("Navigator not ready")
                return
            if gs.is_busy:
                logger.error(f"Character is busy ({gs.busy_reason})")
                return
            res_type = int(args[0]) if args else None
            avail = gs.map.get_available_resources(res_type)
            if not avail:
                logger.error("No available resources on this map")
                return
            # Sort by distance
            current = gs.character.cell_id
            if current is not None:
                from game.map_grid import cell_to_point
                cp = cell_to_point(current)
                def dist2(r):
                    if r.cell_id is None:
                        return 9999
                    rp = cell_to_point(r.cell_id)
                    return abs(rp[0] - cp[0]) + abs(rp[1] - cp[1])
                avail.sort(key=dist2)
            logger.info(f"Farming {len(avail)} resources...")
            gathered = 0
            for i, res in enumerate(avail):
                logger.info(f"  [{i+1}/{len(avail)}] {res}")
                ok = await self.gatherer.gather_resource(res)
                if ok:
                    gathered += 1
                else:
                    logger.warn(f"  Skipped resource {res.element_id}")
                await asyncio.sleep(0.5)
            logger.info(f"Farm done: {gathered}/{len(avail)} gathered")

        elif cmd in ("scan",):
            # Try moving to each adjacent cell to find walkable neighbors
            if not nav.is_ready:
                logger.error("Navigator not ready")
                return
            current = gs.character.cell_id
            if current is None:
                logger.error("Unknown position")
                return
            from game.map_grid import get_neighbors, DIRECTION_NAMES
            neighbors = get_neighbors(current, allow_diagonal=True)
            logger.info(f"Scanning {len(neighbors)} neighbors of cell {current}...")
            for neighbor_id, direction in neighbors:
                walkable = nav.grid.is_walkable(neighbor_id)
                dir_name = DIRECTION_NAMES.get(direction, "?")
                logger.info(f"  {dir_name} -> cell {neighbor_id} (walkable={walkable})")

        elif cmd in ("fight", "fi"):
            # Show current fight state
            fm = gs._fighter_manager
            if fm is None or not gs.in_fight:
                logger.info("Not in fight")
                return
            fight = fm.fight
            logger.info(f"Fight state: turn={fight.turn_count}, our_turn={fight.is_our_turn}")
            our = fight.get_our_fighter()
            if our:
                logger.info(f"  Our fighter: {our}")
            for f in fight.fighters.values():
                if f.actor_id != fight.our_actor_id:
                    logger.info(f"  Fighter: {f}")

        elif cmd in ("turnready", "tr"):
            # Signal turn ready (start or end turn)
            if not gs.in_fight:
                logger.error("Not in fight")
                return
            ok = await self.navigator.movement.send_turn_ready()
            if ok:
                logger.info("Turn ready sent")

        elif cmd in ("cast",):
            # cast <spell_id> <cell_id>
            if len(args) < 2:
                logger.error("Usage: cast <spell_id> <cell_id>")
                return
            spell_id = int(args[0])
            cell_id = int(args[1])
            if not gs.in_fight:
                logger.error("Not in fight")
                return
            ok = await self.navigator.movement.send_cast_spell(spell_id, cell_id)
            if ok:
                logger.info(f"Cast spell {spell_id} -> cell {cell_id}")

        elif cmd in ("spells", "sp"):
            sm = gs.spell_manager
            if sm is None or len(sm) == 0:
                logger.info("No spells loaded")
                return
            logger.info(f"Spells ({len(sm)}):")
            for s in sm.get_all():
                logger.info(f"  {s}")

        elif cmd in ("script", "sc"):
            # script load <path> | script run | script stop | script status
            se = self.script_engine
            if not args:
                logger.info(f"Script engine: {se}")
                return
            sub = args[0].lower()
            if sub == "load":
                path = args[1] if len(args) > 1 else None
                if not path:
                    logger.error("Usage: script load <path>")
                    return
                ok = se.load(path)
                if ok:
                    logger.info(f"Script loaded: {path}")
                    logger.info(f"  Route: {se.route_length} steps")
                    logger.info(f"  Elements: {se.elements_to_gather}")
                    logger.info(f"  Max pods: {se.max_pods}%")
            elif sub == "run":
                if se.is_running:
                    logger.warn("Script already running")
                    return
                if not nav.is_ready:
                    logger.error("Navigator not ready")
                    return
                asyncio.get_event_loop().create_task(se.run())
                logger.info("Script started")
            elif sub == "stop":
                se.stop()
            elif sub == "status":
                logger.info(f"Script engine: {se}")
                if se._route:
                    for i, step in enumerate(se._route[:5]):
                        logger.info(f"  Step {i+1}: {step}")
                    if len(se._route) > 5:
                        logger.info(f"  ... {len(se._route) - 5} more steps")
            else:
                logger.error(f"Unknown subcommand: {sub}. Use: load/run/stop/status")

        elif cmd in ("autofarm", "af"):
            if not hasattr(self, '_auto_farmer'):
                from game.auto_farmer import AutoFarmer
                self._auto_farmer = AutoFarmer(gs, nav, gs.gatherer)

            if self._auto_farmer.is_running:
                self._auto_farmer.stop()
                logger.info("Autofarm stopped")
                return

            if len(args) < 2:
                logger.error("Usage: autofarm <x1>,<y1> <x2>,<y2> [resource_type]")
                logger.error("Example: autofarm 3,-28 5,-23")
                return

            try:
                x1, y1 = map(int, args[0].split(","))
                x2, y2 = map(int, args[1].split(","))
                res_type = int(args[2]) if len(args) > 2 else None
            except ValueError:
                logger.error("Invalid coordinates. Use: autofarm x1,y1 x2,y2")
                return

            logger.info(f"Starting autofarm: ({x1},{y1}) to ({x2},{y2})")
            asyncio.create_task(
                self._auto_farmer.run((x1, x2), (y1, y2), res_type)
            )

        elif cmd in ("stopfarm", "sf"):
            if hasattr(self, '_auto_farmer') and self._auto_farmer.is_running:
                self._auto_farmer.stop()
            else:
                logger.info("Autofarm is not running")

        else:
            logger.error(f"Unknown command: {cmd}. Type 'help' for commands.")

    def _cmd_help(self):
        """Print available commands."""
        cmds = [
            ("help (h)",       "Show this help"),
            ("state (s)",      "Show game state summary"),
            ("pos",            "Show current cell + pos_ref"),
            ("ready (r)",      "Check if navigator is ready"),
            ("move (m) <cell>","Move to a cell (uses A* + walkability)"),
            ("rawmove (rm) <dest>",  "Move to cell (auto start from current)"),
            ("path (p) <cell>","Show A* path without moving"),
            ("scan",           "Show adjacent cells + walkability"),
            ("map <ref_hex>",  "Change map (hex target ref)"),
            ("walkto (w) <cell> <ref_hex>", "Walk to cell + change map"),
            ("resources (res)", "List resources on current map"),
            ("mapview (mv)", "ASCII map view (player, resources, mobs)"),
            ("autofarm (af) <x1,y1> <x2,y2>", "Auto-farm in map rectangle"),
            ("stopfarm (sf)", "Stop auto-farming"),
            ("gather (ga) [type]", "Gather nearest resource"),
            ("farm [type]",    "Gather all resources on map"),
            ("entities (e)",   "List entities on map"),
            ("grid (g)",       "Show grid stats"),
            ("capture (cap)",  "Toggle packet capture (JSONL)"),
            ("fight (fi)",     "Show current fight state"),
            ("turnready (tr)", "Send FightTurnReady (start/end turn)"),
            ("cast <spell> <cell>", "Cast a spell in fight"),
            ("spells (sp)",    "List loaded spells"),
            ("script load <path>", "Load a Lua farming script"),
            ("script run",     "Start the loaded script"),
            ("script stop",    "Stop the running script"),
            ("script status",  "Show script engine status"),
        ]
        print()
        logger.info("Bot Console Commands:")
        for name, desc in cmds:
            print(f"  {name:30s} {desc}")
        print()


# ---------------------------------------------------------------------------
# Entry point
# ---------------------------------------------------------------------------

async def run_proxy(listen_host=None, listen_port=None, server_host=None, server_port=None):
    proxy = MITMProxy(
        listen_host=listen_host or config.PROXY_HOST,
        listen_port=listen_port or config.PROXY_PORT,
        server_host=server_host or config.SERVER_HOST,
        server_port=server_port or config.SERVER_PORT,
    )

    # Run proxy server and (optionally) console command loop concurrently
    tasks = [proxy.start()]
    if proxy.enable_console:
        tasks.append(proxy.console_loop())
    await asyncio.gather(*tasks)

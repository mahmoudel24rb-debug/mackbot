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
from utils import logger
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
    Game auth has outer field 4 with 'type.ankama.com/jol' and a ticket.
    Login packets use outer field 1 (Event) or field 2 (Response).

    Tries multiple framing strategies:
    1. 4-byte big-endian length prefix
    2. Varint length prefix
    3. Raw protobuf (no framing)
    """
    if len(raw_data) < 4:
        return False

    hex_preview = " ".join(f"{b:02x}" for b in raw_data[:32])
    logger.debug(f"  [AUTH-CHECK] Raw data ({len(raw_data)} bytes): {hex_preview}")

    # Strategy 1: 4-byte big-endian length prefix
    length = struct.unpack_from(">I", raw_data, 0)[0]
    if 1 <= length <= len(raw_data) - 4:
        payload = raw_data[4:4 + length]
        logger.debug(f"  [AUTH-CHECK] 4byte frame: length={length}, payload={len(payload)} bytes")
        if _has_field_4(payload):
            logger.debug(f"  [AUTH-CHECK] -> GAME AUTH (4byte framing)")
            return True

    # Strategy 2: Varint length prefix
    try:
        vlength, header_size = decode_varint(bytes(raw_data), 0)
        if 1 <= vlength <= len(raw_data) - header_size:
            payload = raw_data[header_size:header_size + vlength]
            logger.debug(f"  [AUTH-CHECK] Varint frame: length={vlength}, header={header_size}, payload={len(payload)} bytes")
            if _has_field_4(payload):
                logger.debug(f"  [AUTH-CHECK] -> GAME AUTH (varint framing)")
                return True
    except ValueError:
        pass

    # Strategy 3: Raw protobuf (no length prefix)
    logger.debug(f"  [AUTH-CHECK] Trying raw protobuf (no framing)")
    if _has_field_4(raw_data):
        logger.debug(f"  [AUTH-CHECK] -> GAME AUTH (raw protobuf)")
        return True

    # Debug: show what fields we DO see
    for label, data_slice in [
        ("4byte", raw_data[4:] if len(raw_data) > 4 else b""),
        ("raw", raw_data),
    ]:
        fields = decode_protobuf_fields(data_slice)
        if fields:
            field_summary = ", ".join(f"f{fn}(wt={wt})" for fn, wt, _ in fields[:10])
            logger.debug(f"  [AUTH-CHECK] Fields seen ({label}): [{field_summary}]")

    logger.debug(f"  [AUTH-CHECK] -> NOT game auth")
    return False


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
    def __init__(self, listen_host, listen_port, server_host, server_port):
        self.listen_host = listen_host
        self.listen_port = listen_port
        self.server_host = server_host
        self.server_port = server_port
        self.connections = 0
        # Populated when SelectServerResponse is detected
        self.game_server_host = None
        self.game_server_port = None

    async def start(self):
        server = await asyncio.start_server(
            self._handle_client, self.listen_host, self.listen_port
        )
        addr = server.sockets[0].getsockname()
        logger.info(f"Proxy listening on {addr[0]}:{addr[1]}")
        logger.info(f"Login server: {self.server_host}:{self.server_port}")
        logger.info("Waiting for Dofus client connection...")

        async with server:
            await server.serve_forever()

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
        try:
            server_reader, server_writer = await asyncio.open_connection(
                target_host, target_port
            )
            logger.connection(f"[{conn_label} #{conn_id}] Connected to {target_host}:{target_port}")
        except (ConnectionRefusedError, OSError) as e:
            logger.error(f"[{conn_label} #{conn_id}] Cannot connect to {target_host}:{target_port}: {e}")
            client_writer.close()
            return

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

    # --- Logging ---

    def _log_packet(self, payload, direction, conn_id, conn_label):
        msg_name, size, details = format_packet_info(payload, direction)

        if direction == "c2s":
            logger.client_to_server(f"[{conn_label}] {msg_name}", size)
        else:
            logger.server_to_client(f"[{conn_label}] {msg_name}", size)

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
    await proxy.start()

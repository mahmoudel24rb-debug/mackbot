"""
Dofus 3 Packet Handler - Raw Protobuf wire format decoder.

Dofus 3 uses length-prefixed Protobuf messages over TCP.
The outer message is a GameMessage (game server) or LoginMessage (login server).

Structure:
  [4-byte big-endian length] [Protobuf payload]

GameMessage contains a oneof Content:
  field 1 = Event (server -> client)
  field 2 = Request (client -> server)
  field 3 = Response (server -> client, reply to request)

Each of Event/Request/Response contains a oneof Content with the specific message type.
"""

import struct
from protocol.messages import ALL_MESSAGES, get_message_category


# Protobuf wire types
WIRE_VARINT = 0
WIRE_64BIT = 1
WIRE_LENGTH_DELIMITED = 2
WIRE_32BIT = 5

WIRE_TYPE_NAMES = {
    0: "varint",
    1: "64-bit",
    2: "bytes",
    5: "32-bit",
}

# GameMessage oneof Content field numbers (derived from protocol analysis)
GAME_MESSAGE_CONTENT = {
    1: "Event",
    2: "Request",
    3: "Response",
}


def decode_varint(data, pos):
    """Decode a protobuf varint starting at pos. Returns (value, new_pos)."""
    result = 0
    shift = 0
    while pos < len(data):
        b = data[pos]
        result |= (b & 0x7F) << shift
        pos += 1
        if (b & 0x80) == 0:
            return result, pos
        shift += 7
    raise ValueError("Truncated varint")


def decode_signed_varint(value):
    """Decode a zigzag-encoded signed varint."""
    return (value >> 1) ^ -(value & 1)


def decode_protobuf_fields(data):
    """
    Decode raw protobuf wire format into a list of (field_number, wire_type, value).
    Without a .proto schema, we decode the raw wire format.
    """
    fields = []
    pos = 0
    while pos < len(data):
        try:
            tag, pos = decode_varint(data, pos)
            field_number = tag >> 3
            wire_type = tag & 0x07

            if wire_type == WIRE_VARINT:
                value, pos = decode_varint(data, pos)
                fields.append((field_number, wire_type, value))
            elif wire_type == WIRE_64BIT:
                if pos + 8 > len(data):
                    break
                value = struct.unpack_from("<Q", data, pos)[0]
                pos += 8
                fields.append((field_number, wire_type, value))
            elif wire_type == WIRE_LENGTH_DELIMITED:
                length, pos = decode_varint(data, pos)
                if pos + length > len(data):
                    break
                value = data[pos:pos + length]
                pos += length
                fields.append((field_number, wire_type, value))
            elif wire_type == WIRE_32BIT:
                if pos + 4 > len(data):
                    break
                value = struct.unpack_from("<I", data, pos)[0]
                pos += 4
                fields.append((field_number, wire_type, value))
            else:
                break  # Unknown wire type
        except (ValueError, struct.error):
            break
    return fields


def identify_game_message(payload):
    """
    Try to identify a GameMessage from its raw protobuf payload.

    Returns a dict with:
      - 'type': 'Event', 'Request', 'Response', or 'Unknown'
      - 'inner_field': the oneof field number of the specific message type
      - 'inner_data': the raw bytes of the inner message
      - 'uid': request/response UID if present
      - 'fields': all decoded fields
    """
    result = {
        "type": "Unknown",
        "inner_field": None,
        "inner_data": None,
        "uid": None,
        "fields": [],
    }

    fields = decode_protobuf_fields(payload)
    result["fields"] = fields

    for field_num, wire_type, value in fields:
        if field_num in GAME_MESSAGE_CONTENT and wire_type == WIRE_LENGTH_DELIMITED:
            result["type"] = GAME_MESSAGE_CONTENT[field_num]
            # Decode the inner Event/Request/Response wrapper
            inner_fields = decode_protobuf_fields(value)
            for inner_num, inner_wt, inner_val in inner_fields:
                if inner_wt == WIRE_LENGTH_DELIMITED and inner_num > 2:
                    # This is likely the Content oneof - the specific message
                    result["inner_field"] = inner_num
                    result["inner_data"] = inner_val
                elif inner_wt == WIRE_VARINT and inner_num <= 2:
                    # Could be uid or eyrq
                    result["uid"] = inner_val

    return result


class PacketBuffer:
    """
    Buffer for accumulating TCP data and extracting framed packets.

    Tries multiple framing strategies:
    1. 4-byte big-endian length prefix (Thrift TFramedTransport style)
    2. Varint length prefix (Protobuf delimited style)
    """

    def __init__(self):
        self.buffer = bytearray()
        self.framing = None  # Auto-detect: "4byte" or "varint"

    def feed(self, data):
        """Add raw TCP data to the buffer."""
        self.buffer.extend(data)

    def try_extract_packets(self):
        """
        Try to extract complete packets from the buffer.
        Returns a list of (payload_bytes,) tuples.
        """
        packets = []

        while len(self.buffer) > 0:
            packet = self._try_extract_one()
            if packet is None:
                break
            packets.append(packet)

        return packets

    def _try_extract_one(self):
        """Try to extract one packet. Returns payload bytes or None."""
        if self.framing == "4byte" or self.framing is None:
            result = self._try_4byte_frame()
            if result is not None:
                self.framing = "4byte"
                return result

        if self.framing == "varint" or self.framing is None:
            result = self._try_varint_frame()
            if result is not None:
                self.framing = "varint"
                return result

        return None

    def _try_4byte_frame(self):
        """Try 4-byte big-endian length prefix."""
        if len(self.buffer) < 4:
            return None

        length = struct.unpack_from(">I", self.buffer, 0)[0]

        # Sanity check: length should be reasonable (< 10MB)
        if length > 10 * 1024 * 1024 or length == 0:
            return None

        if len(self.buffer) < 4 + length:
            return None  # Need more data

        payload = bytes(self.buffer[4:4 + length])
        del self.buffer[:4 + length]
        return payload

    def _try_varint_frame(self):
        """Try varint length prefix."""
        try:
            length, header_size = decode_varint(bytes(self.buffer), 0)
        except ValueError:
            return None

        if length > 10 * 1024 * 1024 or length == 0:
            return None

        if len(self.buffer) < header_size + length:
            return None

        payload = bytes(self.buffer[header_size:header_size + length])
        del self.buffer[:header_size + length]
        return payload


def encode_varint(value):
    """Encode an integer as a protobuf varint."""
    parts = []
    while value > 0x7F:
        parts.append((value & 0x7F) | 0x80)
        value >>= 7
    parts.append(value & 0x7F)
    return bytes(parts)


def encode_protobuf_fields(fields):
    """
    Re-encode a list of (field_number, wire_type, value) tuples to binary protobuf.
    This is the inverse of decode_protobuf_fields.
    """
    result = bytearray()
    for fn, wt, val in fields:
        tag = (fn << 3) | wt
        result.extend(encode_varint(tag))

        if wt == WIRE_VARINT:
            result.extend(encode_varint(val))
        elif wt == WIRE_64BIT:
            result.extend(struct.pack("<Q", val))
        elif wt == WIRE_LENGTH_DELIMITED:
            if isinstance(val, str):
                val = val.encode("utf-8")
            result.extend(encode_varint(len(val)))
            result.extend(val)
        elif wt == WIRE_32BIT:
            result.extend(struct.pack("<I", val))
    return bytes(result)


def format_packet_info(payload, direction):
    """
    Analyze a packet payload and return a formatted string for display.

    Args:
        payload: raw bytes of the protobuf message
        direction: "c2s" (client to server) or "s2c" (server to client)

    Returns:
        (message_name, size, detail_string)
    """
    msg = identify_game_message(payload)

    msg_type = msg["type"]
    inner_field = msg["inner_field"]
    uid = msg["uid"]

    # Build message name
    if msg_type != "Unknown":
        if inner_field is not None:
            name = f"{msg_type}[field={inner_field}]"
        else:
            name = msg_type
        if uid is not None:
            name += f" (uid={uid})"
    else:
        # Couldn't parse as GameMessage - show raw fields
        name = "RawData"
        if msg["fields"]:
            field_summary = ", ".join(
                f"f{fn}={WIRE_TYPE_NAMES.get(wt, '?')}"
                for fn, wt, _ in msg["fields"][:5]
            )
            name = f"RawData [{field_summary}]"

    # Build detail string with inner message raw decode
    details = ""
    if msg["inner_data"]:
        inner_fields = decode_protobuf_fields(msg["inner_data"])
        if inner_fields:
            parts = []
            for fn, wt, val in inner_fields[:8]:
                if wt == WIRE_VARINT:
                    parts.append(f"  f{fn}: {val}")
                elif wt == WIRE_LENGTH_DELIMITED:
                    # Try to decode as UTF-8 string
                    try:
                        text = val.decode("utf-8")
                        if all(32 <= ord(c) < 127 for c in text):
                            parts.append(f'  f{fn}: "{text}"')
                        else:
                            parts.append(f"  f{fn}: <{len(val)} bytes>")
                    except (UnicodeDecodeError, ValueError):
                        parts.append(f"  f{fn}: <{len(val)} bytes>")
                elif wt in (WIRE_32BIT, WIRE_64BIT):
                    parts.append(f"  f{fn}: {val}")
            if parts:
                details = "\n".join(parts)

    return name, len(payload), details


def format_raw_fields(payload, max_depth=2, indent=0):
    """Recursively decode and format protobuf fields for verbose output."""
    fields = decode_protobuf_fields(payload)
    lines = []
    prefix = "  " * indent

    for fn, wt, val in fields:
        wt_name = WIRE_TYPE_NAMES.get(wt, f"wire{wt}")
        if wt == WIRE_VARINT:
            lines.append(f"{prefix}field {fn} ({wt_name}): {val}")
        elif wt == WIRE_LENGTH_DELIMITED:
            # Try as string
            try:
                text = val.decode("utf-8")
                if all(32 <= ord(c) < 127 for c in text) and len(text) < 200:
                    lines.append(f'{prefix}field {fn} (string): "{text}"')
                    continue
            except (UnicodeDecodeError, ValueError):
                pass
            # Try as nested protobuf
            if max_depth > 0 and len(val) > 1:
                nested = decode_protobuf_fields(val)
                if nested and _is_valid_protobuf(nested, val):
                    lines.append(f"{prefix}field {fn} (message, {len(val)} bytes):")
                    lines.append(format_raw_fields(val, max_depth - 1, indent + 1))
                    continue
            # Raw bytes
            hex_preview = " ".join(f"{b:02x}" for b in val[:32])
            if len(val) > 32:
                hex_preview += f" ... (+{len(val) - 32})"
            lines.append(f"{prefix}field {fn} (bytes, {len(val)}): {hex_preview}")
        elif wt == WIRE_64BIT:
            # Try as double
            float_val = struct.unpack("<d", struct.pack("<Q", val))[0]
            lines.append(f"{prefix}field {fn} ({wt_name}): {val} (float: {float_val:.4f})")
        elif wt == WIRE_32BIT:
            float_val = struct.unpack("<f", struct.pack("<I", val))[0]
            lines.append(f"{prefix}field {fn} ({wt_name}): {val} (float: {float_val:.4f})")

    return "\n".join(lines)


def _is_valid_protobuf(fields, raw_data):
    """Heuristic check if decoded fields look like valid protobuf."""
    if not fields:
        return False
    # Check that field numbers are reasonable (1-1000)
    for fn, wt, _ in fields:
        if fn < 1 or fn > 10000:
            return False
        if wt not in (0, 1, 2, 5):
            return False
    # Check that we consumed most of the data
    total_fields = len(fields)
    if total_fields == 0:
        return False
    return True

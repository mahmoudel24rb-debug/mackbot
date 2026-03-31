"""
Thrift Binary Protocol decoder/encoder for Ankama Launcher IPC.

The IPC between Dofus.exe and the Ankama Launcher uses Apache Thrift
binary protocol (strict mode) over TCP.

Captured methods:
    connect(gameName, gameVariant, version, instanceId) -> instanceId
    settings_get(instanceId, key) -> value
    userInfo_get(instanceId) -> JSON account data
    auth_getGameToken(instanceId, gameId) -> token UUID

Message format (Thrift binary strict):
    [version+type: 4 bytes] [method_name_len: 4 bytes] [method_name: N bytes]
    [seq_id: 4 bytes] [fields...] [stop: 0x00]

Field format:
    [type: 1 byte] [field_id: 2 bytes] [value...]
    Types: 2=bool, 3=byte, 6=i16, 8=i32, 10=i64, 11=string, 12=struct, 15=list
"""

import struct
import json
from dataclasses import dataclass, field


# Thrift type constants
THRIFT_STOP = 0
THRIFT_BOOL = 2
THRIFT_BYTE = 3
THRIFT_I16 = 6
THRIFT_I32 = 8
THRIFT_I64 = 10
THRIFT_STRING = 11
THRIFT_STRUCT = 12
THRIFT_MAP = 13
THRIFT_SET = 14
THRIFT_LIST = 15

# Message types
MSG_CALL = 1
MSG_REPLY = 2
MSG_EXCEPTION = 3

VERSION_MASK = 0xFFFF0000
VERSION_1 = 0x80010000


@dataclass
class ThriftMessage:
    """A decoded Thrift IPC message."""
    method: str
    msg_type: int  # MSG_CALL or MSG_REPLY
    seq_id: int
    fields: dict = field(default_factory=dict)
    raw: bytes = b""

    @property
    def is_call(self) -> bool:
        return self.msg_type == MSG_CALL

    @property
    def is_reply(self) -> bool:
        return self.msg_type == MSG_REPLY

    @property
    def type_name(self) -> str:
        return {MSG_CALL: "CALL", MSG_REPLY: "REPLY", MSG_EXCEPTION: "EXCEPTION"}.get(
            self.msg_type, f"UNKNOWN({self.msg_type})"
        )

    def get_string(self, field_id: int) -> str | None:
        """Get a string field value."""
        val = self.fields.get(field_id)
        if isinstance(val, str):
            return val
        if isinstance(val, bytes):
            return val.decode("utf-8", errors="replace")
        return None

    def get_int(self, field_id: int) -> int | None:
        """Get an integer field value."""
        val = self.fields.get(field_id)
        return val if isinstance(val, int) else None


# ---------------------------------------------------------------------------
# Decoder
# ---------------------------------------------------------------------------

class ThriftDecoder:
    """Decode Thrift binary protocol messages."""

    def __init__(self, data: bytes):
        self.data = data
        self.pos = 0

    def _read(self, n: int) -> bytes:
        if self.pos + n > len(self.data):
            raise ValueError(f"Buffer underflow: need {n} bytes at pos {self.pos}, have {len(self.data)}")
        result = self.data[self.pos:self.pos + n]
        self.pos += n
        return result

    def _read_i8(self) -> int:
        return struct.unpack(">b", self._read(1))[0]

    def _read_i16(self) -> int:
        return struct.unpack(">h", self._read(2))[0]

    def _read_i32(self) -> int:
        return struct.unpack(">i", self._read(4))[0]

    def _read_i64(self) -> int:
        return struct.unpack(">q", self._read(8))[0]

    def _read_string(self) -> str:
        length = struct.unpack(">i", self._read(4))[0]
        return self._read(length).decode("utf-8", errors="replace")

    def _read_binary(self) -> bytes:
        length = struct.unpack(">i", self._read(4))[0]
        return self._read(length)

    def _read_field_value(self, type_id: int):
        """Read a field value based on its type."""
        if type_id == THRIFT_BOOL:
            return self._read_i8() != 0
        elif type_id == THRIFT_BYTE:
            return self._read_i8()
        elif type_id == THRIFT_I16:
            return self._read_i16()
        elif type_id == THRIFT_I32:
            return self._read_i32()
        elif type_id == THRIFT_I64:
            return self._read_i64()
        elif type_id == THRIFT_STRING:
            return self._read_string()
        elif type_id == THRIFT_STRUCT:
            return self._read_fields()
        elif type_id == THRIFT_LIST:
            elem_type = self._read_i8()
            count = self._read_i32()
            return [self._read_field_value(elem_type) for _ in range(count)]
        elif type_id == THRIFT_MAP:
            key_type = self._read_i8()
            val_type = self._read_i8()
            count = self._read_i32()
            return {self._read_field_value(key_type): self._read_field_value(val_type)
                    for _ in range(count)}
        elif type_id == THRIFT_SET:
            elem_type = self._read_i8()
            count = self._read_i32()
            return [self._read_field_value(elem_type) for _ in range(count)]
        else:
            raise ValueError(f"Unknown Thrift type: {type_id}")

    def _read_fields(self) -> dict:
        """Read struct fields until STOP byte."""
        fields = {}
        while True:
            type_id = self._read_i8()
            if type_id == THRIFT_STOP:
                break
            field_id = self._read_i16()
            value = self._read_field_value(type_id)
            fields[field_id] = value
        return fields

    @property
    def bytes_consumed(self) -> int:
        return self.pos

    @property
    def has_remaining(self) -> bool:
        return self.pos < len(self.data)

    def decode_message(self) -> ThriftMessage:
        """Decode a complete Thrift message."""
        start_pos = self.pos

        # Read version + message type (strict mode)
        version_and_type = struct.unpack(">I", self._read(4))[0]
        version = version_and_type & VERSION_MASK
        if version != VERSION_1:
            raise ValueError(f"Unsupported Thrift version: 0x{version:08X}")
        msg_type = version_and_type & 0x000000FF

        # Method name
        method = self._read_string()

        # Sequence ID
        seq_id = self._read_i32()

        # Fields
        fields = self._read_fields()

        msg_raw = self.data[start_pos:self.pos]

        return ThriftMessage(
            method=method,
            msg_type=msg_type,
            seq_id=seq_id,
            fields=fields,
            raw=msg_raw,
        )


# ---------------------------------------------------------------------------
# Encoder
# ---------------------------------------------------------------------------

class ThriftEncoder:
    """Encode Thrift binary protocol messages."""

    def __init__(self):
        self.buf = bytearray()

    def _write(self, data: bytes):
        self.buf.extend(data)

    def _write_i8(self, val: int):
        self._write(struct.pack(">b", val))

    def _write_i16(self, val: int):
        self._write(struct.pack(">h", val))

    def _write_i32(self, val: int):
        self._write(struct.pack(">i", val))

    def _write_i64(self, val: int):
        self._write(struct.pack(">q", val))

    def _write_string(self, val: str):
        encoded = val.encode("utf-8")
        self._write_i32(len(encoded))
        self._write(encoded)

    def _write_field(self, field_id: int, type_id: int, value):
        """Write a single field."""
        self._write_i8(type_id)
        self._write_i16(field_id)
        self._write_field_value(type_id, value)

    def _write_field_value(self, type_id: int, value):
        """Write a field value."""
        if type_id == THRIFT_BOOL:
            self._write_i8(1 if value else 0)
        elif type_id == THRIFT_BYTE:
            self._write_i8(value)
        elif type_id == THRIFT_I16:
            self._write_i16(value)
        elif type_id == THRIFT_I32:
            self._write_i32(value)
        elif type_id == THRIFT_I64:
            self._write_i64(value)
        elif type_id == THRIFT_STRING:
            self._write_string(value)

    def _write_stop(self):
        self._write_i8(THRIFT_STOP)

    def encode_message(self, method: str, msg_type: int, seq_id: int,
                       fields: list[tuple[int, int, any]]) -> bytes:
        """
        Encode a Thrift message.

        fields: list of (field_id, type_id, value)
        """
        self.buf = bytearray()

        # Version + type
        self._write(struct.pack(">I", VERSION_1 | msg_type))

        # Method name
        self._write_string(method)

        # Sequence ID
        self._write_i32(seq_id)

        # Fields
        for field_id, type_id, value in fields:
            self._write_field(field_id, type_id, value)

        # Stop
        self._write_stop()

        return bytes(self.buf)


# ---------------------------------------------------------------------------
# High-level helpers for IPC messages
# ---------------------------------------------------------------------------

def decode_ipc(data: bytes) -> ThriftMessage:
    """Decode a raw IPC message from bytes."""
    return ThriftDecoder(data).decode_message()


def encode_connect_reply(seq_id: int, instance_id: str) -> bytes:
    """Encode a 'connect' reply message."""
    return ThriftEncoder().encode_message(
        "connect", MSG_REPLY, seq_id,
        [(0, THRIFT_STRING, instance_id)],
    )


def encode_settings_reply(seq_id: int, value: str) -> bytes:
    """Encode a 'settings_get' reply message."""
    return ThriftEncoder().encode_message(
        "settings_get", MSG_REPLY, seq_id,
        [(0, THRIFT_STRING, value)],
    )


def encode_userinfo_reply(seq_id: int, user_json: str) -> bytes:
    """Encode a 'userInfo_get' reply with JSON account data."""
    return ThriftEncoder().encode_message(
        "userInfo_get", MSG_REPLY, seq_id,
        [(0, THRIFT_STRING, user_json)],
    )


def encode_gametoken_reply(seq_id: int, token: str) -> bytes:
    """Encode an 'auth_getGameToken' reply."""
    return ThriftEncoder().encode_message(
        "auth_getGameToken", MSG_REPLY, seq_id,
        [(0, THRIFT_STRING, token)],
    )


# ---------------------------------------------------------------------------
# Debug / verification
# ---------------------------------------------------------------------------

def verify_against_capture(capture_file: str):
    """Verify decoder against a capture file."""
    import json as _json

    with open(capture_file, "r", encoding="utf-8") as f:
        for line in f:
            entry = _json.loads(line.strip())
            if entry.get("type") != "ipc":
                continue

            raw = bytes.fromhex(entry["raw_hex"])
            direction = entry["dir"]

            try:
                msg = decode_ipc(raw)
                print(f"{'>>>' if 'game' in direction else '<<<'} "
                      f"{msg.type_name} {msg.method}(seq={msg.seq_id})")
                for fid, val in msg.fields.items():
                    val_preview = repr(val)
                    if len(val_preview) > 100:
                        val_preview = val_preview[:100] + "..."
                    print(f"    field {fid}: {val_preview}")
            except Exception as e:
                print(f"DECODE ERROR: {e}")
                print(f"  hex: {raw[:32].hex()}...")


if __name__ == "__main__":
    import sys
    if len(sys.argv) > 1:
        verify_against_capture(sys.argv[1])
    else:
        print("Usage: python -m launcher.ipc_protocol <capture.jsonl>")

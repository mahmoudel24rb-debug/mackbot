"""
Structured packet capture for Dofus 3 protocol analysis.

Captures all game messages to a JSONL file with:
  - Timestamps (ms precision)
  - Direction (c2s / s2c)
  - Message type code and name
  - Recursively parsed protobuf fields
  - Raw hex for offline replay

Usage:
  capture = PacketCapture("capture_session.jsonl")
  capture.start()
  capture.log_message("ipi", "MoveRequest", "c2s", data, uid=1001)
  capture.stop()

Analysis:
  python -m utils.capture capture_session.jsonl --filter movement
"""

import json
import time
import os
from datetime import datetime
from proxy.packet_handler import decode_protobuf_fields, WIRE_VARINT, WIRE_LENGTH_DELIMITED


class PacketCapture:
    """Captures game packets to a structured JSONL file."""

    def __init__(self, filepath=None):
        if filepath is None:
            ts = datetime.now().strftime("%Y%m%d_%H%M%S")
            filepath = os.path.join(
                os.path.dirname(os.path.dirname(__file__)),
                "captures",
                f"capture_{ts}.jsonl",
            )
        self.filepath = filepath
        self._file = None
        self._active = False
        self._start_time = None
        self._count = 0

    @property
    def active(self):
        return self._active

    @property
    def count(self):
        return self._count

    def start(self):
        """Start capturing packets."""
        os.makedirs(os.path.dirname(self.filepath), exist_ok=True)
        self._file = open(self.filepath, "a", encoding="utf-8")
        self._active = True
        self._start_time = time.time()
        self._count = 0
        # Write session header
        self._write({
            "type": "session_start",
            "ts": self._start_time,
            "time": datetime.now().isoformat(),
        })

    def stop(self):
        """Stop capturing."""
        if self._file:
            self._write({
                "type": "session_end",
                "ts": time.time(),
                "count": self._count,
            })
            self._file.close()
            self._file = None
        self._active = False

    def log_message(self, type_code, type_name, direction, data, uid=None):
        """Log a parsed game message."""
        if not self._active:
            return

        entry = {
            "type": "msg",
            "ts": time.time(),
            "ms": int((time.time() - self._start_time) * 1000),
            "dir": direction,
            "code": type_code,
            "name": type_name,
        }

        if uid is not None:
            entry["uid"] = uid

        # Parse fields recursively
        if data:
            entry["fields"] = _parse_fields_recursive(data, max_depth=4)
            entry["raw_hex"] = data.hex()
            entry["size"] = len(data)
        else:
            entry["fields"] = []
            entry["size"] = 0

        self._write(entry)
        self._count += 1

    def log_raw(self, direction, raw_bytes, label=""):
        """Log raw unrecognized data."""
        if not self._active:
            return

        self._write({
            "type": "raw",
            "ts": time.time(),
            "dir": direction,
            "label": label,
            "hex": raw_bytes.hex(),
            "size": len(raw_bytes),
        })

    def _write(self, obj):
        """Write a JSON line."""
        if self._file:
            self._file.write(json.dumps(obj, ensure_ascii=False) + "\n")
            self._file.flush()


def _parse_fields_recursive(data, max_depth=4, depth=0):
    """Parse protobuf fields recursively into JSON-friendly dicts."""
    if depth > max_depth or not data:
        return []

    try:
        fields = decode_protobuf_fields(data)
    except Exception:
        return [{"raw": data.hex()}]

    result = []
    for fn, wt, val in fields:
        entry = {"f": fn}

        if wt == WIRE_VARINT:
            entry["t"] = "v"
            entry["val"] = val
            # Also show hex for large values
            if val > 0xFFFF:
                entry["hex"] = f"0x{val:X}"

        elif wt == WIRE_LENGTH_DELIMITED:
            # Try to decode as string
            try:
                text = val.decode("utf-8")
                if all(32 <= ord(c) < 127 or c in '\n\r\t' for c in text):
                    entry["t"] = "s"
                    entry["val"] = text
                else:
                    raise ValueError()
            except (UnicodeDecodeError, ValueError):
                # Try as nested protobuf
                sub = _parse_fields_recursive(val, max_depth, depth + 1)
                if sub and len(sub) > 0 and not (len(sub) == 1 and "raw" in sub[0]):
                    entry["t"] = "msg"
                    entry["val"] = sub
                    entry["size"] = len(val)
                else:
                    entry["t"] = "b"
                    entry["hex"] = val.hex()
                    entry["size"] = len(val)
        else:
            entry["t"] = f"wt{wt}"
            entry["val"] = val if isinstance(val, int) else val.hex()

        result.append(entry)

    return result


# ---------------------------------------------------------------------------
# Analysis CLI
# ---------------------------------------------------------------------------

# Message categories for filtering
CATEGORIES = {
    "movement": {"ipi", "ion", "inq", "ipa", "ioh", "iqa", "ipd", "iny", "iao", "iaj"},
    "gather": {"klv", "kll", "klj", "kma", "kmi"},
    "fight": {"ibe", "icd", "idh", "idm", "hde"},
    "map": {"hxl", "gxu", "iaa", "hxm", "gxk", "gxb", "gxh", "hjd", "hqo", "ias"},
    "character": {"jrl", "jtl", "jtb", "jtx", "hdm", "juc", "ktg"},
    "social": {"jlm", "jml", "jkj", "jkv", "hcm"},
}


def analyze_capture(filepath, category=None, code_filter=None, direction=None):
    """Analyze a capture file and print summary."""
    messages = []
    with open(filepath, "r", encoding="utf-8") as f:
        for line in f:
            try:
                entry = json.loads(line.strip())
                if entry.get("type") == "msg":
                    messages.append(entry)
            except json.JSONDecodeError:
                continue

    if not messages:
        print("No messages found in capture.")
        return

    # Apply filters
    if category and category in CATEGORIES:
        codes = CATEGORIES[category]
        messages = [m for m in messages if m["code"] in codes]

    if code_filter:
        messages = [m for m in messages if m["code"] == code_filter]

    if direction:
        messages = [m for m in messages if m["dir"] == direction]

    # Print
    print(f"\n{'='*70}")
    print(f"  Capture: {filepath}")
    print(f"  Messages: {len(messages)}")
    if category:
        print(f"  Filter: {category}")
    print(f"{'='*70}\n")

    for m in messages:
        ms = m.get("ms", 0)
        dir_arrow = ">>>" if m["dir"] == "c2s" else "<<<"
        dir_label = "C2S" if m["dir"] == "c2s" else "S2C"
        code = m["code"]
        name = m.get("name", code)
        size = m.get("size", 0)

        print(f"  [{ms:8d}ms] {dir_label} {dir_arrow} {name}({code}) [{size}b]")

        # Show key fields
        if m.get("fields"):
            _print_fields(m["fields"], indent=4)

    # Summary stats
    print(f"\n--- Summary ---")
    code_counts = {}
    for m in messages:
        key = f"{m['dir']} {m['code']}"
        code_counts[key] = code_counts.get(key, 0) + 1
    for key, count in sorted(code_counts.items(), key=lambda x: -x[1]):
        print(f"  {key}: {count}")


def _print_fields(fields, indent=4):
    """Pretty-print parsed fields."""
    prefix = " " * indent
    for f in fields:
        fn = f.get("f", "?")
        t = f.get("t", "?")
        if t == "v":
            val = f["val"]
            hex_val = f.get("hex", "")
            if hex_val:
                print(f"{prefix}f{fn}: {val} ({hex_val})")
            else:
                print(f"{prefix}f{fn}: {val}")
        elif t == "s":
            print(f"{prefix}f{fn}: \"{f['val']}\"")
        elif t == "msg":
            print(f"{prefix}f{fn}: [{f.get('size', '?')}b]")
            _print_fields(f["val"], indent + 2)
        elif t == "b":
            hex_str = f.get("hex", "")
            if len(hex_str) > 64:
                hex_str = hex_str[:64] + "..."
            print(f"{prefix}f{fn}: bytes({f.get('size', '?')}) {hex_str}")


if __name__ == "__main__":
    import sys
    if len(sys.argv) < 2:
        print("Usage: python -m utils.capture <capture.jsonl> [--filter category] [--code xxx] [--dir c2s|s2c]")
        print(f"Categories: {', '.join(CATEGORIES.keys())}")
        sys.exit(1)

    filepath = sys.argv[1]
    category = None
    code = None
    direction = None

    i = 2
    while i < len(sys.argv):
        if sys.argv[i] == "--filter" and i + 1 < len(sys.argv):
            category = sys.argv[i + 1]
            i += 2
        elif sys.argv[i] == "--code" and i + 1 < len(sys.argv):
            code = sys.argv[i + 1]
            i += 2
        elif sys.argv[i] == "--dir" and i + 1 < len(sys.argv):
            direction = sys.argv[i + 1]
            i += 2
        else:
            i += 1

    analyze_capture(filepath, category, code, direction)

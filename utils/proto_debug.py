"""
Recursive protobuf decoder for debugging unknown message structures.

Usage:
    from utils.proto_debug import decode_protobuf_recursive, format_proto_tree

    tree = decode_protobuf_recursive(raw_bytes)
    print(format_proto_tree(tree))
"""

from proxy.packet_handler import decode_varint


def decode_protobuf_recursive(data, max_depth=5, current_depth=0):
    """
    Recursively decode a protobuf buffer, attempting to parse
    length-delimited fields as sub-messages.

    Returns a dict: { field_number: [("varint"|"message"|"bytes"|"fixed32"|"fixed64", value), ...] }
    """
    if not data or current_depth > max_depth:
        return {}

    result = {}
    pos = 0
    while pos < len(data):
        try:
            # Read field tag
            field_tag, new_pos = decode_varint(data, pos)
            if new_pos == pos:
                break
            field_number = field_tag >> 3
            wire_type = field_tag & 0x07

            # Sanity: field numbers should be reasonable (1-10000)
            if field_number < 1 or field_number > 100000:
                break

            if wire_type == 0:  # varint
                value, pos = decode_varint(data, new_pos)
                result.setdefault(field_number, []).append(("varint", value))

            elif wire_type == 2:  # length-delimited
                length, pos2 = decode_varint(data, new_pos)
                if length < 0 or pos2 + length > len(data):
                    break
                raw = data[pos2:pos2 + length]
                pos = pos2 + length

                # Try to parse as sub-message protobuf
                if current_depth < max_depth and len(raw) > 0:
                    try:
                        sub = decode_protobuf_recursive(raw, max_depth, current_depth + 1)
                        if sub and _looks_like_valid_protobuf(sub, raw):
                            result.setdefault(field_number, []).append(("message", sub))
                            continue
                    except Exception:
                        pass

                # Try to decode as UTF-8 string
                try:
                    text = raw.decode("utf-8")
                    if all(32 <= ord(c) < 127 or c in '\n\r\t' for c in text):
                        result.setdefault(field_number, []).append(("string", text))
                        continue
                except (UnicodeDecodeError, ValueError):
                    pass

                # Store as raw bytes
                result.setdefault(field_number, []).append(("bytes", raw.hex()))

            elif wire_type == 5:  # fixed32
                if new_pos + 4 > len(data):
                    break
                value = int.from_bytes(data[new_pos:new_pos + 4], 'little')
                pos = new_pos + 4
                result.setdefault(field_number, []).append(("fixed32", value))

            elif wire_type == 1:  # fixed64
                if new_pos + 8 > len(data):
                    break
                value = int.from_bytes(data[new_pos:new_pos + 8], 'little')
                pos = new_pos + 8
                result.setdefault(field_number, []).append(("fixed64", value))

            else:
                # Unknown wire type — stop parsing
                break

        except (ValueError, IndexError, OverflowError):
            break

    return result


def _looks_like_valid_protobuf(parsed, raw):
    """Heuristic: does the parsed result look like a real protobuf message?"""
    if not parsed:
        return False
    # At least one field should have been parsed
    total_entries = sum(len(v) for v in parsed.values())
    if total_entries == 0:
        return False
    # Field numbers should be small (< 1000 for real messages)
    if any(fn > 10000 for fn in parsed.keys()):
        return False
    return True


def format_proto_tree(tree, indent=0):
    """Format a decoded protobuf tree as a readable string."""
    lines = []
    prefix = "  " * indent
    for fn in sorted(tree.keys()):
        for entry_type, value in tree[fn]:
            if entry_type == "varint":
                # Show both decimal and hex for large values
                if value > 1000:
                    lines.append(f"{prefix}f{fn}: {value} (0x{value:X})")
                else:
                    lines.append(f"{prefix}f{fn}: {value}")
            elif entry_type == "message":
                lines.append(f"{prefix}f{fn}: {{")
                lines.append(format_proto_tree(value, indent + 1))
                lines.append(f"{prefix}}}")
            elif entry_type == "string":
                lines.append(f"{prefix}f{fn}: \"{value}\"")
            elif entry_type == "bytes":
                # Truncate long hex
                hex_str = value[:60] + ("..." if len(value) > 60 else "")
                lines.append(f"{prefix}f{fn}: bytes[{len(value)//2}] {hex_str}")
            elif entry_type in ("fixed32", "fixed64"):
                lines.append(f"{prefix}f{fn}: {entry_type}({value})")
    return "\n".join(lines)


def compare_packets(label_a, hex_a, label_b, hex_b):
    """
    Compare two packets (as hex strings) side by side.
    Decodes both as protobuf, shows trees and byte-level diff.

    Args:
        label_a: name for first packet (e.g. "real_client")
        hex_a: hex string of first packet
        label_b: name for second packet (e.g. "bot")
        hex_b: hex string of second packet

    Returns:
        Formatted comparison string.
    """
    bytes_a = bytes.fromhex(hex_a)
    bytes_b = bytes.fromhex(hex_b)

    lines = []
    lines.append(f"=== Packet Comparison: {label_a} vs {label_b} ===")
    lines.append(f"  {label_a}: {len(bytes_a)} bytes")
    lines.append(f"  {label_b}: {len(bytes_b)} bytes")
    lines.append("")

    # Decode both as protobuf
    tree_a = decode_protobuf_recursive(bytes_a, max_depth=4)
    tree_b = decode_protobuf_recursive(bytes_b, max_depth=4)

    lines.append(f"--- {label_a} protobuf tree ---")
    lines.append(format_proto_tree(tree_a))
    lines.append("")
    lines.append(f"--- {label_b} protobuf tree ---")
    lines.append(format_proto_tree(tree_b))
    lines.append("")

    # Byte-by-byte diff
    lines.append("--- Byte diff ---")
    max_len = max(len(bytes_a), len(bytes_b))
    diffs = []
    for i in range(max_len):
        ba = bytes_a[i] if i < len(bytes_a) else None
        bb = bytes_b[i] if i < len(bytes_b) else None
        if ba != bb:
            sa = f"{ba:02x}" if ba is not None else "--"
            sb = f"{bb:02x}" if bb is not None else "--"
            diffs.append(f"  pos {i:3d}: {label_a}={sa}  {label_b}={sb}")
    if diffs:
        lines.extend(diffs[:50])
        if len(diffs) > 50:
            lines.append(f"  ... and {len(diffs) - 50} more differences")
    else:
        lines.append("  (identical)")

    return "\n".join(lines)


def analyze_ial_walkability(ial_debug_path, walkable_cells, blocked_cells):
    """
    Compare IAL f1 bitmasks between confirmed walkable and blocked cells.

    Args:
        ial_debug_path: path to data/ial_debug.json
        walkable_cells: set of cellIds known to be walkable
        blocked_cells: set of cellIds known to be blocked

    Returns:
        Analysis string identifying candidate walkability bits.
    """
    import json
    with open(ial_debug_path, "r") as f:
        ial_data = json.load(f)

    cell_props = ial_data.get("cell_properties", {})
    lines = []
    lines.append(f"=== IAL Walkability Analysis ===")
    lines.append(f"  Total cells in IAL: {len(cell_props)}")
    lines.append(f"  Walkable sample: {len(walkable_cells)} cells")
    lines.append(f"  Blocked sample: {len(blocked_cells)} cells")

    # Collect f1 values for walkable vs blocked cells
    walk_f1 = set()
    block_f1 = set()
    for cid in walkable_cells:
        vals = cell_props.get(str(cid), [])
        walk_f1.update(vals)
    for cid in blocked_cells:
        vals = cell_props.get(str(cid), [])
        block_f1.update(vals)

    lines.append(f"  Walkable f1 values: {sorted(walk_f1)[:20]}")
    lines.append(f"  Blocked f1 values: {sorted(block_f1)[:20]}")

    # Find f1 values exclusive to walkable or blocked
    only_walk = walk_f1 - block_f1
    only_block = block_f1 - walk_f1
    if only_walk:
        lines.append(f"  f1 values ONLY in walkable: {sorted(only_walk)[:20]}")
    if only_block:
        lines.append(f"  f1 values ONLY in blocked: {sorted(only_block)[:20]}")

    # Bit analysis: for each bit position, check if it correlates with walkability
    all_f1 = walk_f1 | block_f1
    if all_f1:
        max_val = max(all_f1)
        bits_needed = max_val.bit_length() if max_val > 0 else 1
        lines.append(f"\n  Bit analysis (up to {bits_needed} bits):")
        for bit in range(min(bits_needed, 32)):
            mask = 1 << bit
            walk_has = sum(1 for v in walk_f1 if v & mask)
            block_has = sum(1 for v in block_f1 if v & mask)
            if walk_has != block_has:
                lines.append(f"    bit {bit} (0x{mask:X}): walkable={walk_has}/{len(walk_f1)} blocked={block_has}/{len(block_f1)}")

    return "\n".join(lines)


def test_kww_hypotheses(kww_debug_path, ial_debug_path=None):
    """
    Test hypotheses about KWW data format.

    Tests:
    1. Are f1 values cellIds (0-559)?
    2. Are (f3 & 0x3FF) values cellIds?
    3. Cross-check with IAL if available.

    Returns:
        Analysis string.
    """
    import json
    with open(kww_debug_path, "r") as f:
        kww_data = json.load(f)

    entries = kww_data.get("entries", [])
    lines = []
    lines.append(f"=== KWW Hypothesis Tests ({len(entries)} entries) ===")

    # Test 1: f1 as cellId
    f1_cells = [e["f1"] for e in entries if 0 <= e.get("f1", -1) <= 559]
    lines.append(f"  f1 in cell range (0-559): {len(f1_cells)}/{len(entries)}")

    # Test 2: f3 & 0x3FF as cellId
    f3_base = [(e.get("f3", 0) & 0x3FF) for e in entries]
    f3_cells = [v for v in f3_base if 0 <= v <= 559]
    lines.append(f"  (f3 & 0x3FF) in cell range: {len(f3_cells)}/{len(entries)}")

    # Test 3: unique values
    f1_unique = set(e.get("f1", 0) for e in entries)
    f3_base_unique = set(f3_base)
    lines.append(f"  Unique f1 values: {len(f1_unique)}")
    lines.append(f"  Unique (f3 & 0x3FF): {len(f3_base_unique)}")

    # Test 4: f3 flags breakdown
    f3_flags = set((e.get("f3", 0) >> 9) for e in entries)
    lines.append(f"  Unique (f3 >> 9) flags: {sorted(f3_flags)[:20]}")

    # Cross-check with IAL
    if ial_debug_path:
        try:
            with open(ial_debug_path, "r") as f:
                ial_data = json.load(f)
            ial_cells = set(int(k) for k in ial_data.get("cell_properties", {}).keys())
            f1_overlap = f1_unique & ial_cells
            f3_overlap = f3_base_unique & ial_cells
            lines.append(f"\n  Cross-check with IAL ({len(ial_cells)} cells):")
            lines.append(f"    f1 overlap with IAL cells: {len(f1_overlap)}")
            lines.append(f"    (f3 & 0x3FF) overlap with IAL cells: {len(f3_overlap)}")
        except Exception as e:
            lines.append(f"  IAL cross-check failed: {e}")

    return "\n".join(lines)


def find_values_in_range(tree, min_val, max_val, path=""):
    """
    Find all varint values within a range in a decoded protobuf tree.
    Useful for finding cellIds (0-559) buried in nested structures.

    Returns list of (path, value) tuples.
    """
    results = []
    for fn in sorted(tree.keys()):
        current_path = f"{path}.f{fn}" if path else f"f{fn}"
        for entry_type, value in tree[fn]:
            if entry_type == "varint" and min_val <= value <= max_val:
                results.append((current_path, value))
            elif entry_type == "message":
                results.extend(find_values_in_range(value, min_val, max_val, current_path))
    return results

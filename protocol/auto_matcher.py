"""
Auto-matcher: detects which 3-letter code maps to which stable message name
by analysing the protobuf structure (wire types, field count, direction) of
captured traffic.

Each known message type has a unique "signature":
  - direction: "c2s" or "s2c" or "any"
  - fields: list of (field_number, wire_type) that MUST be present
  - min_fields / max_fields: total field count constraints
  - size_min / size_max: raw byte size constraints (optional)
  - unique: if True, stop trying other signatures once this one matches

Wire types:
  0 = varint, 1 = 64-bit, 2 = length-delimited, 5 = 32-bit
"""

from proxy.packet_handler import decode_protobuf_fields, WIRE_VARINT, WIRE_LENGTH_DELIMITED
from protocol.matching import Matching

import os


# ---------------------------------------------------------------------------
# Signature definitions
# Each entry: (message_name, direction, checker_fn)
# checker_fn(fields, raw_bytes, direction) -> bool
# ---------------------------------------------------------------------------

def _field_map(fields):
    """Return {field_number: [wire_types]} from decoded fields."""
    m = {}
    for fn, wt, _ in fields:
        m.setdefault(fn, []).append(wt)
    return m


def _check_MapMovementConfirmRequest(fields, data, direction):
    """C2S — message is empty (0 bytes exactly). Strictest check to avoid false positives."""
    if direction != "c2s":
        return False
    return len(data) == 0


def _check_MapMovementRequest(fields, data, direction):
    """
    C2S — field1=int64(mapId), field2=packed repeated int32(keyCells), field3=bool(cautious)
    Must have field 1 (varint) and field 2 (length-delimited packed ints).
    """
    if direction != "c2s":
        return False
    fm = _field_map(fields)
    has_f1_varint = WIRE_VARINT in fm.get(1, [])
    has_f2_ld = WIRE_LENGTH_DELIMITED in fm.get(2, [])
    return has_f1_varint and has_f2_ld and len(fields) >= 2


def _check_MapMovementEvent(fields, data, direction):
    """
    S2C — broadcast of movement: repeated ActorPositionInformation (field1).
    Large-ish message, length-delimited field 1.
    """
    if direction != "s2c":
        return False
    fm = _field_map(fields)
    has_f1_ld = WIRE_LENGTH_DELIMITED in fm.get(1, [])
    return has_f1_ld and len(data) > 5


def _check_MapChangeRequest(fields, data, direction):
    """
    C2S — single field1=int64(mapId), nothing else.
    """
    if direction != "c2s":
        return False
    fm = _field_map(fields)
    has_f1_varint = WIRE_VARINT in fm.get(1, [])
    # Must have field 1 only (or field 1 + field 2 optional bool)
    return has_f1_varint and 1 <= len(fields) <= 3 and len(data) >= 2


def _check_InteractiveUseRequest(fields, data, direction):
    """
    C2S — field1=int32(elementId), field2=int32(skillInstanceUid). Exactly 2 varint fields.
    """
    if direction != "c2s":
        return False
    fm = _field_map(fields)
    has_f1 = WIRE_VARINT in fm.get(1, [])
    has_f2 = WIRE_VARINT in fm.get(2, [])
    return has_f1 and has_f2 and len(fields) == 2


def _check_MapComplementaryInformationEvent(fields, data, direction):
    """
    S2C — very large message (>1000 bytes) with many nested fields.
    This is the map info dump sent when entering a map.
    Signature: S2C, very large, many fields (8+), has length-delimited fields.
    """
    if direction != "s2c":
        return False
    fm = _field_map(fields)
    # Must be moderately large (400+ bytes) and have many top-level fields
    has_many_fields = len(fm) >= 5
    has_nested = any(WIRE_LENGTH_DELIMITED in wts for wts in fm.values())
    return len(data) > 300 and has_many_fields and has_nested


def _check_InteractiveUsedEvent(fields, data, direction):
    """
    S2C — server confirms interactive use.
    field1=int32(elementId), field2=int32(duration), field3=int64(entityId).
    3 varint fields.
    """
    if direction != "s2c":
        return False
    fm = _field_map(fields)
    has_f1 = WIRE_VARINT in fm.get(1, [])
    has_f2 = WIRE_VARINT in fm.get(2, [])
    has_f3 = WIRE_VARINT in fm.get(3, [])
    return has_f1 and has_f2 and has_f3 and len(fields) == 3


def _check_InteractiveUseEndedEvent(fields, data, direction):
    """
    S2C — end of interactive use.
    field1=int32(elementId), field2=int32(skillId). 2 varint fields.
    """
    if direction != "s2c":
        return False
    fm = _field_map(fields)
    has_f1 = WIRE_VARINT in fm.get(1, [])
    has_f2 = WIRE_VARINT in fm.get(2, [])
    return has_f1 and has_f2 and len(fields) == 2 and len(data) <= 20


def _check_ObjectHarvestedEvent(fields, data, direction):
    """
    S2C — item harvested: field1=int32(gid), field2=int32(quantity).
    Small message, 2 varint fields.
    """
    if direction != "s2c":
        return False
    fm = _field_map(fields)
    has_f1 = WIRE_VARINT in fm.get(1, [])
    has_f2 = WIRE_VARINT in fm.get(2, [])
    return has_f1 and has_f2 and len(fields) == 2 and len(data) <= 30


def _check_MapMovementConfirmResponse(fields, data, direction):
    """
    S2C — server acks arrival: field1=int32(cellId). Single varint.
    """
    if direction != "s2c":
        return False
    fm = _field_map(fields)
    has_f1 = WIRE_VARINT in fm.get(1, [])
    return has_f1 and len(fields) == 1 and len(data) <= 5


# Ordered list: (message_name, checker_fn)
# More specific checks FIRST (to avoid false positives with generic ones)
SIGNATURES = [
    ("MapMovementConfirmRequest",          _check_MapMovementConfirmRequest),
    ("InteractiveUseRequest",              _check_InteractiveUseRequest),
    ("MapMovementRequest",                 _check_MapMovementRequest),
    ("MapChangeRequest",                   _check_MapChangeRequest),
    ("MapComplementaryInformationEvent",   _check_MapComplementaryInformationEvent),
    ("InteractiveUsedEvent",               _check_InteractiveUsedEvent),
    ("InteractiveUseEndedEvent",           _check_InteractiveUseEndedEvent),
    ("ObjectHarvestedEvent",               _check_ObjectHarvestedEvent),
    ("MapMovementConfirmResponse",         _check_MapMovementConfirmResponse),
    ("MapMovementEvent",                   _check_MapMovementEvent),
]

# Names that we already know are stable and don't need auto-detection
ALREADY_KNOWN = {"MapDataResponse"}  # "iou" — confirmed stable in CLAUDE.md


class AutoMatcher:
    """
    Observes traffic and tries to identify which 3-letter codes correspond to
    which stable message names, using protobuf structure signatures.

    Usage:
        am = AutoMatcher(matching)
        # For each captured message:
        am.observe(type_code, raw_data, direction)
    """

    def __init__(self, matching=None):
        self.matching = matching or Matching()
        self._unmatched = {}   # code -> {direction, samples: [bytes]}
        self._stats = {}       # code -> int (observation count)

    def observe(self, type_code, data, direction):
        """
        Called for every decoded message. Tries to identify type_code if unknown.
        Returns the message name (matched or type_code if unknown).
        """
        if not type_code or not isinstance(type_code, str):
            return type_code

        # Already matched
        if self.matching.is_known(type_code):
            return self.matching.get_name(type_code)

        # Track observation count
        self._stats[type_code] = self._stats.get(type_code, 0) + 1

        # Try to match via signatures
        name = self._try_match(type_code, data, direction)
        if name:
            # Don't overwrite if this name already has a DIFFERENT code mapped
            existing_code = self.matching.get_code(name)
            if existing_code and existing_code != type_code:
                # Name already mapped to another code — skip to avoid corruption
                return type_code
            added = self.matching.add(type_code, name)
            if added:
                print(f"[AutoMatcher] NEW MATCH: {type_code!r} -> {name} "
                      f"(direction={direction}, size={len(data)})")
            return name

        return type_code

    def _try_match(self, type_code, data, direction):
        """Try all signatures against this message. Return name if matched."""
        if not data:
            data = b""

        try:
            fields = list(decode_protobuf_fields(data))
        except Exception:
            fields = []

        for name, checker in SIGNATURES:
            # Skip names already matched to a different code — UNLESS
            # that old code hasn't been seen at all this session (stale/rotated)
            existing_code = self.matching.get_code(name)
            if existing_code and existing_code != type_code:
                if existing_code in self._stats:
                    continue  # old code is still active, skip
                # Old code never seen this session — likely rotated, allow remap
            try:
                if checker(fields, data, direction):
                    return name
            except Exception:
                continue
        return None

    def get_stats(self):
        """Return observation counts for unmatched codes."""
        unmatched = {c: n for c, n in self._stats.items()
                     if not self.matching.is_known(c)}
        return unmatched

    def report(self):
        """Print a summary of matching status."""
        known = self.matching.all_codes()
        print(f"[AutoMatcher] Matched: {len(known)} codes")
        for code, name in sorted(known.items()):
            print(f"  {code} -> {name}")
        unmatched = self.get_stats()
        if unmatched:
            print(f"[AutoMatcher] Unmatched codes seen: {len(unmatched)}")
            for code, count in sorted(unmatched.items(), key=lambda x: -x[1])[:20]:
                print(f"  {code}: {count} occurrences")

        needed = [name for name, _ in SIGNATURES
                  if not self.matching.get_code(name)]
        if needed:
            print(f"[AutoMatcher] Still need: {needed}")

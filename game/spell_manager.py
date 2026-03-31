"""
Dofus 3 Spell Manager — Spell data and cost tracking.

Parses SpellsEvent (S2C) to extract:
  - Available spells (id, level)
  - AP cost, range, cooldown

Provides helpers for combat AI:
  - can_cast(spell_id, target_cell, fighter_state)
  - get_affordable_spells(available_ap)
"""

from utils import logger


class SpellLevel:
    """
    A single level of a spell with its stats.
    Corresponds to DetailedSpellLevel in Jitsuri.
    """
    __slots__ = (
        "spell_id", "level",
        "ap_cost", "min_range", "max_range",
        "cast_in_line", "cast_in_diagonal", "cast_test_los",
        "max_cast_per_turn", "max_cast_per_target",
        "global_cooldown",
    )

    def __init__(self, spell_id, level=1):
        self.spell_id = spell_id
        self.level = level
        self.ap_cost = 0
        self.min_range = 0
        self.max_range = 0
        self.cast_in_line = False
        self.cast_in_diagonal = False
        self.cast_test_los = True
        self.max_cast_per_turn = 0   # 0 = unlimited
        self.max_cast_per_target = 0
        self.global_cooldown = 0

    def __repr__(self):
        return (f"Spell(id={self.spell_id}, lv={self.level}, "
                f"ap={self.ap_cost}, range={self.min_range}-{self.max_range})")


class SpellManager:
    """
    Manages the character's known spells.
    Loaded from SpellsEvent (S2C) at login.
    """

    def __init__(self):
        # spell_id -> SpellLevel
        self._spells = {}

    def load_from_event(self, data_fields):
        """
        Parse SpellsEvent fields.

        SpellsEvent structure (reverse-engineered):
        field 2 (repeated message): each spell
          field 1 (varint): spellId
          field 2 (varint): currentLevel
          field 3 (message): SpellLevel details
            field 1 (varint): apCost
            field 2 (varint): minRange
            field 3 (varint): maxRange
            field 4 (bool): castInLine
            field 5 (bool): castInDiagonal
            field 6 (bool): castTestLos
            field 7 (varint): maxCastPerTurn
            field 8 (varint): maxCastPerTarget
            field 9 (varint): globalCooldown
        """
        from proxy.packet_handler import decode_protobuf_fields, WIRE_VARINT, WIRE_LENGTH_DELIMITED

        self._spells.clear()
        count = 0

        for fn, wt, val in data_fields:
            if fn == 2 and wt == WIRE_LENGTH_DELIMITED:
                spell = self._parse_spell_entry(val)
                if spell:
                    self._spells[spell.spell_id] = spell
                    count += 1

        logger.info(f"[SPELLS] Loaded {count} spells")

    def _parse_spell_entry(self, raw_bytes):
        from proxy.packet_handler import decode_protobuf_fields, WIRE_VARINT, WIRE_LENGTH_DELIMITED
        try:
            fields = decode_protobuf_fields(raw_bytes)
        except Exception:
            return None

        spell_id = None
        level = 1
        ap_cost = 0
        min_range = 0
        max_range = 0
        cast_in_line = False
        max_cast_per_turn = 0
        global_cooldown = 0

        for fn, wt, val in fields:
            if wt == WIRE_VARINT:
                if fn == 1:   spell_id = val
                elif fn == 2: level = val
            elif wt == WIRE_LENGTH_DELIMITED and fn == 3:
                # Spell level details
                detail_fields = decode_protobuf_fields(val)
                for dfn, dwt, dval in detail_fields:
                    if dwt == WIRE_VARINT:
                        if dfn == 1:   ap_cost = dval
                        elif dfn == 2: min_range = dval
                        elif dfn == 3: max_range = dval
                        elif dfn == 4: cast_in_line = bool(dval)
                        elif dfn == 7: max_cast_per_turn = dval
                        elif dfn == 9: global_cooldown = dval

        if spell_id is None:
            return None

        s = SpellLevel(spell_id, level)
        s.ap_cost = ap_cost
        s.min_range = min_range
        s.max_range = max_range
        s.cast_in_line = cast_in_line
        s.max_cast_per_turn = max_cast_per_turn
        s.global_cooldown = global_cooldown
        return s

    def get_spell(self, spell_id):
        return self._spells.get(spell_id)

    def get_affordable_spells(self, available_ap):
        """Return spells that can be cast with available_ap."""
        return [s for s in self._spells.values() if s.ap_cost <= available_ap]

    def get_all(self):
        return list(self._spells.values())

    def __len__(self):
        return len(self._spells)

    def __repr__(self):
        return f"SpellManager({len(self._spells)} spells)"

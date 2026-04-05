"""
Dofus 3 message handlers.

Each handler parses a specific message type and updates the GameState.
Handlers are registered by type URL code (3-letter).

Message field structures are reverse-engineered from packet captures.
"""

import time
from proxy.packet_handler import decode_protobuf_fields, WIRE_LENGTH_DELIMITED, WIRE_VARINT
from utils import logger
from utils.proto_debug import decode_protobuf_recursive, format_proto_tree, find_values_in_range
from game.dofus_message import get_type_name
import config

# Seconds after CharacterLoaded to ignore FightStart (init burst filter)
_INIT_GRACE_PERIOD = 10

# Track which code the real client uses for InteractiveUseRequest (itl or idh)
_sniffed_interactive_use_code = None


def get_sniffed_interactive_code():
    """Return the last sniffed InteractiveUseRequest code ('itl' or 'idh'), or None."""
    return _sniffed_interactive_use_code


def _decode(data):
    """Shortcut: decode protobuf fields from bytes, return empty list if None."""
    if not data:
        return []
    return decode_protobuf_fields(data)


def _get_field(fields, target_fn, target_wt=None):
    """Get the first field matching field number (and optionally wire type)."""
    for fn, wt, val in fields:
        if fn == target_fn:
            if target_wt is None or wt == target_wt:
                return val
    return None


def _get_string(fields, target_fn):
    """Get a string field."""
    val = _get_field(fields, target_fn, WIRE_LENGTH_DELIMITED)
    if val:
        try:
            return val.decode("utf-8")
        except (UnicodeDecodeError, ValueError):
            pass
    return None


def _get_varint(fields, target_fn):
    """Get a varint field."""
    return _get_field(fields, target_fn, WIRE_VARINT)


def _get_all_fields(fields, target_fn, target_wt=None):
    """Get all fields matching field number."""
    result = []
    for fn, wt, val in fields:
        if fn == target_fn:
            if target_wt is None or wt == target_wt:
                result.append(val)
    return result


# ---------------------------------------------------------------------------
# Character List (jtl)
# ---------------------------------------------------------------------------
# field 1 (message): repeated Character
#   field 1 (varint): character_id
#   field 3 (bytes): character info (name, breed, look, etc.)

def handle_character_list(state, data, direction, uid):
    """Parse CharacterList event - shows available characters."""
    if direction != "s2c":
        return
    fields = _decode(data)
    characters = _get_all_fields(fields, 1, WIRE_LENGTH_DELIMITED)

    char_list = []
    for char_data in characters:
        char_fields = _decode(char_data)
        char_id = _get_varint(char_fields, 1)
        # Field 3 contains nested character info with name
        info_bytes = _get_field(char_fields, 3, WIRE_LENGTH_DELIMITED)
        name = None
        level = None
        breed = None
        if info_bytes:
            info_fields = _decode(info_bytes)
            name = _get_string(info_fields, 1)
            # Try to find level/breed in nested fields
            for fn, wt, val in info_fields:
                if fn == 2 and wt == WIRE_LENGTH_DELIMITED:
                    inner = _decode(val)
                    for ifn, iwt, ival in inner:
                        if ifn == 1 and iwt == WIRE_LENGTH_DELIMITED:
                            # Deeper nesting for breed/look info
                            breed_fields = _decode(ival)
                            breed_str = _get_string(breed_fields, 1)
                            if breed_str and breed_str.isdigit():
                                breed = int(breed_str)

        char_list.append({"id": char_id, "name": name, "level": level, "breed": breed})

    if char_list:
        logger.info("")
        logger.info(f"  CHARACTER LIST ({len(char_list)} characters):")
        for c in char_list:
            logger.info(f"    [{c['id']}] {c['name'] or '?'} (breed={c['breed']})")
        logger.info("")


# ---------------------------------------------------------------------------
# Character Select (jtb) - client request
# ---------------------------------------------------------------------------
# field 1 (varint): character_id

def handle_character_select(state, data, direction, uid):
    """Parse CharacterSelect request - client selects a character."""
    if direction != "c2s":
        return
    fields = _decode(data)
    char_id = _get_varint(fields, 1)
    if char_id:
        state.character.id = char_id
        logger.info(f"  -> Character selected: ID {char_id}")


# ---------------------------------------------------------------------------
# Character Loaded (jrl)
# ---------------------------------------------------------------------------
# field 1 (message): CharacterData
#   field 1 (bytes): basic info
#   field 4 (varint): some ID
#   field 5 (varint): some ID
#   field 6 (varint): cell/position?
#   field 8 (bytes): repeated - characteristics

def _try_extract_character_name(state, data, max_depth=3):
    """Try to extract character name from nested protobuf data.
    Looks for UTF-8 strings of 3-25 chars that look like a player name.
    Only called from handle_character_loaded (jrl) — NOT from ISU actors."""
    if not data or max_depth <= 0:
        return False
    # Exclusion list: server names, common false positives
    _EXCLUDE = {"dofus2-", "ankama", "orukam", "type.", "http", ".com",
                "192.168", "127.0", "localhost"}
    fields = _decode(data)
    for fn, wt, val in fields:
        if wt == WIRE_LENGTH_DELIMITED:
            try:
                text = val.decode("utf-8")
                if (3 <= len(text) <= 25
                        and not text.isdigit()
                        and "/" not in text
                        and " " not in text
                        and any(c.isalpha() for c in text)
                        and not any(ex in text.lower() for ex in _EXCLUDE)
                        and len([c for c in text if c.isalpha()]) >= 3):
                    state.character.name = text
                    logger.info(f"  -> Character name found: '{text}' (f{fn})")
                    return True
            except (UnicodeDecodeError, ValueError):
                pass
            if _try_extract_character_name(state, val, max_depth - 1):
                return True
    return False


def handle_character_loaded(state, data, direction, uid):
    """Parse CharacterLoaded - character data after selection."""
    if direction != "s2c":
        return
    fields = _decode(data)
    char_data = _get_field(fields, 1, WIRE_LENGTH_DELIMITED)
    if not char_data:
        return

    char_fields = _decode(char_data)

    # Basic info in field 1 (bytes)
    basic_info = _get_field(char_fields, 1, WIRE_LENGTH_DELIMITED)
    if basic_info:
        basic_fields = _decode(basic_info)
        # Level is often a varint in the basic info
        level = _get_varint(basic_fields, 1)
        if level and level < 300:
            state.character.level = level

    # Field 6 was previously assumed to be cell_id but 871 is invalid
    # (Dofus 3 maps have 560 cells max: 0-559). Real cell comes from hxl entities.
    cell_candidate = _get_varint(char_fields, 6)
    if cell_candidate is not None:
        if cell_candidate < 560:
            state.character.cell_id = cell_candidate
        else:
            logger.debug(f"    jrl f6={cell_candidate} (NOT a cell, >559)")

    # Try to extract character name from multiple sources
    if state.character.name is None:
        _try_extract_character_name(state, data)
    if state.character.name is None and char_data:
        _try_extract_character_name(state, char_data)

    state.connected = True
    state._connect_time = time.time()
    logger.info(f"  -> Character loaded: name={state.character.name}, level={state.character.level}, cell={state.character.cell_id}")


# ---------------------------------------------------------------------------
# Character Stats (hdm)
# ---------------------------------------------------------------------------
# field 1 (varint): current value (HP?)
# field 2 (varint): max value
# field 3 (varint): another max/current

def handle_character_stats(state, data, direction, uid):
    """Parse CharacterStats - HP/MP update."""
    if direction != "s2c":
        return
    fields = _decode(data)

    val1 = _get_varint(fields, 1)
    val2 = _get_varint(fields, 2)
    val3 = _get_varint(fields, 3)

    if val1 is not None and val2 is not None:
        state.stats.hp = val1
        state.stats.max_hp = val2
        if val3 is not None:
            state.stats.max_hp = max(val2, val3)
        logger.info(f"  -> Stats: HP={val1}/{val2} (f3={val3})")


# ---------------------------------------------------------------------------
# Map Coordinates (iaa)
# ---------------------------------------------------------------------------
# field 2 (varint): x coordinate
# field 3 (varint): y coordinate

def handle_map_coordinates(state, data, direction, uid):
    """Parse MapCoordinates - map position."""
    if direction != "s2c":
        return
    fields = _decode(data)

    x = _get_varint(fields, 2)
    y = _get_varint(fields, 3)

    if x is not None and y is not None:
        state.map.x = x
        state.map.y = y
        logger.info(f"  -> Map coordinates: ({x}, {y})")


# ---------------------------------------------------------------------------
# Current Cell (iny)
# ---------------------------------------------------------------------------
# field 1 (varint): composite value - NOT a raw cell ID
# Values seen: 191106052, 191105028 (too large for cell IDs)
# Low 12 bits might be cell ID, upper bits may be map/entity ref

def handle_current_cell(state, data, direction, uid):
    """Parse CurrentCellId (iny) — saves pos_ref for movement packets.

    IMPORTANT: iny.f1 is a composite position reference, NOT a cellId.
    It is used in ipi.f2 and ioh.f2 for movement requests.
    The real cellId after map change comes from:
      1. The first MoveRequest (C2S) sniffed on the new map
      2. The first MoveEvent (S2C) received after map change

    DO NOT set state.character.cell_id here — it causes mismatches.
    """
    if direction != "s2c":
        return
    fields = _decode(data)
    val = _get_varint(fields, 1)
    if val is not None:
        state.pos_ref = val
        logger.info(f"  -> iny pos_ref=0x{val:08X} (saved for movement, NOT a cellId)")
        state._needs_cell_update = True


# ---------------------------------------------------------------------------
# Map Complementary Info (hxl)
# ---------------------------------------------------------------------------
# Complex message with entities, interactive elements, etc.
# field 1 (message): repeated - entities/actors
#   field 1 (bytes): actor data
#   field 4 (varint): map_id or type

def _search_entity_for_char(fields, char_id, depth=0):
    """Search entity fields for our character ID, up to 3 levels deep."""
    if depth > 3 or not char_id:
        return False
    for fn, wt, val in fields:
        if wt == WIRE_VARINT and val == char_id:
            return True
        if wt == WIRE_LENGTH_DELIMITED and depth < 3:
            sub_fields = _decode(val)
            if _search_entity_for_char(sub_fields, char_id, depth + 1):
                return True
    return False


def _extract_cell_from_entity(fields, depth=0):
    """Extract a cell-like value (0-559) from entity fields. Search all levels."""
    # Collect all small varints as cell candidates
    candidates = []
    for fn, wt, val in fields:
        if wt == WIRE_VARINT and 0 <= val < 560:
            candidates.append((fn, val, depth))
            logger.info(f"    {'  ' * depth}POSSIBLE CELL: f{fn} = {val}")
        if wt == WIRE_VARINT:
            logger.debug(f"    {'  ' * depth}entity f{fn}(varint): {val}")
        if wt == WIRE_LENGTH_DELIMITED and depth < 3:
            sub_fields = _decode(val)
            sub_result = _extract_cell_from_entity(sub_fields, depth + 1)
            if sub_result is not None:
                candidates.append((fn, sub_result, depth))

    # Return first cell candidate found
    if candidates:
        return candidates[0][1]
    return None


def handle_map_info(state, data, direction, uid):
    """Parse MapComplementaryInfo - full map data on map change.

    Current protocol structure (2026-03):
      f2  = repeated InteractiveElement { f1=elementId, f4=cellId }
      f6  = repeated StatedElement { f2=skillId, f3=elementId, f4=status }
      f7  = varint (bool/count)
      f11 = repeated ActorPositionInformation { f1=header, f2=actorId, f3=nested }
      f13 = subareaId
      f14 = mapId
    """
    if direction != "s2c":
        return
    fields = _decode(data)

    # --- MapId (field 14, fallback field 8) ---
    map_id = _get_varint(fields, 14) or _get_varint(fields, 8)
    if map_id and map_id > 100000:
        # Detect involuntary map change (only if we didn't request it)
        old_map = state.map.map_id
        if old_map is not None and old_map != map_id:
            if state._expecting_map_change:
                state._expecting_map_change = False
                logger.info(f"  -> Intentional map change: {old_map} -> {map_id}")
            else:
                old_cell = state.character.cell_id
                logger.warn(f"  -> Involuntary map change: {old_map} -> {map_id} "
                            f"(was at cell {old_cell})")
                state._involuntary_map_change = True
                state._involuntary_from_cell = old_cell

        state.map.map_id = map_id
        state.pos_ref = map_id
        logger.info(f"  -> MapId: {map_id}")

        # Apply pending walkable grid from KWW (received just before ISU)
        pending = getattr(state, '_pending_walkable', None)
        if pending is not None:
            state._walkable_cells = pending
            state._walkable_cells_map_id = map_id
            if not hasattr(state, '_walkable_cache'):
                state._walkable_cache = {}
            state._walkable_cache[map_id] = pending
            state._pending_walkable = None
            logger.info(f"  -> Walkable grid applied: {len(pending)} cells")
            state.save_walkable_cache()
        elif hasattr(state, '_walkable_cache') and map_id in state._walkable_cache:
            # Reuse cached grid for this map
            state._walkable_cells = state._walkable_cache[map_id]
            state._walkable_cells_map_id = map_id
            logger.info(f"  -> Walkable grid from cache: {len(state._walkable_cells)} cells")

    # --- Actors (field 11) ---
    # Use recursive protobuf decoder to find cellIds buried in nested structures
    actors = _get_all_fields(fields, 11, WIRE_LENGTH_DELIMITED)
    state.entities.clear()
    char_id = state.character.id

    for actor_data in actors:
        actor_fields = _decode(actor_data)
        actor_id = _get_varint(actor_fields, 2)

        # NPC/monsters use f1.f7 structure, player uses f1.f8 (different layout).
        # Player cellId is NOT in ISU actors (confirmed 2026-03-26).
        # NPC/monster cellId is at f1.f7.f4.f1.f4 (confirmed from logs).
        cell_id = None
        nested_data = _get_field(actor_fields, 3, WIRE_LENGTH_DELIMITED)
        if nested_data:
            tree = decode_protobuf_recursive(nested_data, max_depth=5)
            # Direct path for NPC/monsters: f1.f7.f4.f1.f4
            candidates = find_values_in_range(tree, 0, 559)
            # Filter: only f1.f7.f4.f1.f4 path (confirmed as cellId for mobs)
            cell_candidates = [(p, v) for p, v in candidates
                               if "f7.f4" in p and p.endswith(".f4")
                               and v > 5]
            if cell_candidates:
                cell_id = cell_candidates[0][1]

        entity = {"id": actor_id, "cell_id": cell_id}
        state.entities[actor_id] = entity

        # Auto-detect our character ONLY if we don't have one yet
        # AND this is the first actor (index 0) with a positive small-ish ID
        # Don't overwrite if already set from CharacterSelect (jtb) or CharacterLoaded (jrl)
        if actor_id is not None and not char_id:
            # Only auto-detect if we have NO character.id at all
            if state.character.id is None and actor_id < 0x8000000000000000:
                state.character.id = actor_id
                char_id = actor_id
                logger.info(f"  -> Character detected (auto): id={actor_id}")
            elif state.character.id is not None:
                char_id = state.character.id

        if char_id and actor_id == char_id:
            if cell_id is not None:
                state.character.cell_id = cell_id
                logger.info(f"  -> Character cell from isu: {cell_id}")
            # Extract name ONLY for OUR character (char_id is verified)
            if state.character.name is None:
                nested = _get_field(actor_fields, 3, WIRE_LENGTH_DELIMITED)
                if nested:
                    _try_extract_character_name(state, nested)

    # --- Interactive elements (field 2) ---
    interactives = _get_all_fields(fields, 2, WIRE_LENGTH_DELIMITED)
    state.map.interactive_elements = []
    elem_cells = {}  # elementId -> cellId mapping
    for i, elem_data in enumerate(interactives):
        ef = _decode(elem_data)
        elem_id = _get_varint(ef, 1)

        # Real cellId is in f2.f4 (NOT top-level f4 which is a type/template id)
        # f2 is a sub-message containing f4=cellId
        cell = None
        f2_data = _get_field(ef, 2, WIRE_LENGTH_DELIMITED)
        if f2_data:
            f2_fields = _decode(f2_data)
            cell = _get_varint(f2_fields, 4)

        if elem_id:
            state.map.interactive_elements.append({"id": elem_id, "cell_id": cell})
            if cell is not None:
                elem_cells[elem_id] = cell
            logger.debug(f"    [ISU-INTERACTIVE #{i}] elemId={elem_id} cellId(f2.f4)={cell}")

    # --- Stated elements / resources (field 6) ---
    # Build Resource objects from stated elements + interactive element cell mapping
    from game.state import Resource
    stated = _get_all_fields(fields, 6, WIRE_LENGTH_DELIMITED)
    resources = []
    for i, se_data in enumerate(stated):
        sf = _decode(se_data)
        # Correct StatedElement structure (confirmed 2026-03-26):
        #   f2 = cellId (NOT skillId! verified: 337 is adjacent to 323/338/351)
        #   f3 = elementId
        #   f4 = status (1 = available)
        cell_id = _get_varint(sf, 2)
        elem_id = _get_varint(sf, 3)
        status = _get_varint(sf, 4)

        if elem_id:
            res = Resource(
                element_id=elem_id,
                cell_id=cell_id,
                status=status if status is not None else 0,
            )
            resources.append(res)
            logger.debug(f"    [ISU-RESOURCE #{i}] elem={elem_id} cell={cell_id} status={status}")
    state.map.resources = resources
    avail = [r for r in resources if r.available]
    logger.info(f"  -> Resources detail: {len(avail)} available at cells {[r.cell_id for r in avail]}")

    logger.info(f"  -> Map loaded: {len(actors)} actors, {len(interactives)} interactive, "
                f"{len(avail)}/{len(resources)} resources, "
                f"mapId={state.map.map_id}, cell={state.character.cell_id}")

    # On map change, the cell from the old map is inaccurate but USABLE as fallback.
    # It will be corrected by the first MoveRequest or MoveEvent sniffed on the new map.
    # Don't reset to None — that blocks the Navigator and requires manual movement.
    if state.character.cell_id is not None:
        logger.debug(f"  -> Cell kept as {state.character.cell_id} (will update on first move)")

    # Clear busy state on map change
    state.is_busy = False
    state.busy_reason = None

    # Notify navigator that map changed (new map data arrived)
    if hasattr(state, 'navigator') and state.navigator:
        state.navigator.grid.clear()
        state.navigator.movement.on_map_changed()


# ---------------------------------------------------------------------------
# Acquaintance List (jlm) - Friends/contacts online
# ---------------------------------------------------------------------------
# field 1 (message): repeated AcquaintanceInfo
#   field 1 (bytes): player data (contains name, tag)
#   field 2 (varint): player_id
#   field 4 (varint): level or position
#   field 7 (varint): status (-1 = unknown)

def handle_acquaintance_list(state, data, direction, uid):
    """Parse AcquaintanceList - online friends/contacts (NOT players on map)."""
    if direction != "s2c":
        return
    fields = _decode(data)
    friends = _get_all_fields(fields, 1, WIRE_LENGTH_DELIMITED)

    names = []
    for friend_data in friends:
        f_fields = _decode(friend_data)
        info_bytes = _get_field(f_fields, 1, WIRE_LENGTH_DELIMITED)
        name = None
        if info_bytes:
            info_fields = _decode(info_bytes)
            name = _get_string(info_fields, 2)
        if name:
            names.append(name)

    if names:
        logger.info(f"  -> Friends online: {', '.join(names[:10])}")


# ---------------------------------------------------------------------------
# Map Interactive Elements (hqo)
# ---------------------------------------------------------------------------
# field 3 (message): repeated InteractiveElement
#   field 1 (bytes): element data
#   field 3 (varint): element_id

def handle_interactive_elements(state, data, direction, uid):
    """Parse MapInteractiveElements - gatherable resources, NPCs, etc."""
    if direction != "s2c":
        return
    fields = _decode(data)
    elements = _get_all_fields(fields, 3, WIRE_LENGTH_DELIMITED)

    state.map.interactive_elements = []
    for elem_data in elements:
        e_fields = _decode(elem_data)
        elem_id = _get_varint(e_fields, 3)
        if elem_id:
            state.map.interactive_elements.append({"id": elem_id, "raw": e_fields})

    if elements:
        logger.info(f"  -> Interactive elements: {len(elements)}")


# ---------------------------------------------------------------------------
# Character Selected Info (jtx)
# ---------------------------------------------------------------------------
# Contains character name and look after selection

def handle_character_selected_info(state, data, direction, uid):
    """Parse CharacterSelectedInfo - character details after selection."""
    if direction != "s2c":
        return
    fields = _decode(data)
    # Look for character info in field 3
    info = _get_field(fields, 3, WIRE_LENGTH_DELIMITED)
    if info:
        info_fields = _decode(info)
        # Try to find name in nested data
        for fn, wt, val in info_fields:
            if wt == WIRE_LENGTH_DELIMITED:
                nested = _decode(val)
                name = _get_string(nested, 1)
                if name and len(name) > 2 and not name.startswith("type."):
                    state.character.name = name
                    logger.info(f"  -> Character name: {name}")
                    return


# ---------------------------------------------------------------------------
# Character Appearance (ktg)
# ---------------------------------------------------------------------------
# field 1 (string): session/instance UUID
# field 2 (message): appearance data
#   field 1 (string): breed number
#   ...

def handle_character_appearance(state, data, direction, uid):
    """Parse CharacterAppearance."""
    if direction != "s2c":
        return
    fields = _decode(data)
    appearance = _get_field(fields, 2, WIRE_LENGTH_DELIMITED)
    if appearance:
        app_fields = _decode(appearance)
        breed_str = _get_string(app_fields, 1)
        if breed_str and breed_str.isdigit():
            state.character.breed = int(breed_str)
            logger.info(f"  -> Breed: {breed_str}")


# ---------------------------------------------------------------------------
# Spell List (hwa)
# ---------------------------------------------------------------------------

def handle_spells(state, data, direction, uid):
    """Parse SpellsEvent - character spells. Feeds SpellManager if available."""
    if direction != "s2c":
        return
    fields = _decode(data)
    spells = _get_all_fields(fields, 2, WIRE_LENGTH_DELIMITED)
    if spells:
        logger.info(f"  -> Spells loaded: {len(spells)} spells")
    # Feed the spell manager if present
    if hasattr(state, 'spell_manager') and state.spell_manager is not None:
        state.spell_manager.load_from_event(fields)


# ---------------------------------------------------------------------------
# Fight Events
# ---------------------------------------------------------------------------

def _get_fighter_manager(state):
    """Return the FighterManager, creating it if needed."""
    if not hasattr(state, '_fighter_manager') or state._fighter_manager is None:
        from game.fighter import FighterManager
        state._fighter_manager = FighterManager(state)
    return state._fighter_manager


def handle_fight_join(state, data, direction, uid):
    """FightJoinEvent — we joined a fight."""
    if direction != "s2c":
        return
    if not state.connected:
        return
    elapsed = time.time() - (state._connect_time or 0)
    if elapsed < _INIT_GRACE_PERIOD:
        return
    fm = _get_fighter_manager(state)
    fields = _decode(data)
    # Set our actor ID from the fight join event if available
    for fn, wt, val in fields:
        if wt == WIRE_VARINT and fn == 1:
            fm.fight.our_actor_id = val
            break
    fm.on_fight_join(fields)


def handle_fight_start(state, data, direction, uid):
    """FightStartingEvent — fight officially starts."""
    if not state.connected:
        logger.debug(f"  -> FightStart ignored (not connected yet)")
        return
    elapsed = time.time() - (state._connect_time or 0)
    if elapsed < _INIT_GRACE_PERIOD:
        logger.debug(f"  -> FightStart ignored (init phase, {elapsed:.1f}s since connect)")
        return
    fm = _get_fighter_manager(state)
    fields = _decode(data)
    fm.on_fight_starting(fields)


def handle_fight_end(state, data, direction, uid):
    """FightEndEvent — fight over."""
    fm = _get_fighter_manager(state)
    fields = _decode(data)
    fm.on_fight_end(fields)
    state.entities.clear()
    logger.info(f"  -> FIGHT ENDED")


def handle_fight_synchronize(state, data, direction, uid):
    """GameFightSynchronizeEvent — full fighter list."""
    if direction != "s2c":
        return
    fm = _get_fighter_manager(state)
    fields = _decode(data)
    # field 1 = repeated fighter data
    fighters_data = _get_all_fields(fields, 1, WIRE_LENGTH_DELIMITED)
    fm.on_synchronize(fighters_data)


def handle_fight_show_fighter(state, data, direction, uid):
    """GameFightShowFighterEvent — single fighter added."""
    if direction != "s2c":
        return
    if not data:
        return
    fm = _get_fighter_manager(state)
    fm.on_show_fighter(data)


def handle_fight_turn_start(state, data, direction, uid):
    """FightTurnStartEvent — actor's turn begins."""
    if direction != "s2c":
        return
    fields = _decode(data)
    # field 1 = actorId (varint), field 2 = waitTime (varint)
    actor_id = _get_varint(fields, 1)
    if actor_id is None:
        return
    fm = _get_fighter_manager(state)
    # Link our character ID if not set
    if fm.fight.our_actor_id is None and state.character.id:
        fm.fight.our_actor_id = state.character.id
    fm.on_turn_start(actor_id)


def handle_fight_turn_end(state, data, direction, uid):
    """FightTurnEndEvent — actor's turn ends."""
    if direction != "s2c":
        return
    fields = _decode(data)
    actor_id = _get_varint(fields, 1)
    if actor_id is None:
        return
    fm = _get_fighter_manager(state)
    fm.on_turn_end(actor_id)


def handle_fight_new_round(state, data, direction, uid):
    """GameFightNewRoundEvent — new round."""
    if direction != "s2c":
        return
    fields = _decode(data)
    round_num = _get_varint(fields, 1) or 0
    fm = _get_fighter_manager(state)
    fm.on_new_round(round_num)


def handle_fight_action(state, data, direction, uid):
    """GameActionFightEvent — combat action (damage, move, death...)."""
    if direction != "s2c":
        return
    if not data:
        return
    fields = _decode(data)
    fm = _get_fighter_manager(state)
    fm.on_fight_action(fields)


def handle_fight_placement_positions(state, data, direction, uid):
    """FightPlacementPossiblePositionsEvent — where we can place our character."""
    if direction != "s2c":
        return
    fields = _decode(data)
    # field 1 = repeated cellId (varint) for our team
    cells = _get_all_fields(fields, 1, WIRE_VARINT)
    fm = _get_fighter_manager(state)
    fm.on_placement_positions(cells)


# ---------------------------------------------------------------------------
# Movement Events
# ---------------------------------------------------------------------------
# MapMovementEvent (hxk) - server confirms/broadcasts movement
# field 1 (varint): actor_id
# field 2 (bytes): path data (compressed cells)

def handle_movement_event(state, data, direction, uid):
    """Parse MapMovementEvent - movement confirmed by server."""
    if direction != "s2c":
        return
    fields = _decode(data)
    actor_id = _get_varint(fields, 1)

    # Log ALL fields for reverse engineering
    path_bytes = _get_field(fields, 2, WIRE_LENGTH_DELIMITED)
    path_varints = _get_all_fields(fields, 2, WIRE_VARINT)

    # Notify the movement controller
    if hasattr(state, 'navigator') and state.navigator:
        state.navigator.movement.on_movement_event()

    cell_info = ""
    if path_varints:
        cells = [v & 0xFFF for v in path_varints]
        dirs = [(v >> 12) & 0xF for v in path_varints]
        cell_info = f" cells={cells} dirs={dirs}"
    if path_bytes:
        hex_preview = " ".join(f"{b:02x}" for b in path_bytes[:32])
        cell_info += f" path_bytes={hex_preview}"
    logger.info(f"  -> Movement: actor={actor_id}{cell_info}")

    if getattr(config, 'MOVEMENT_DEBUG', False):
        for fn, wt, val in fields:
            if wt == WIRE_VARINT:
                logger.debug(f"    hxk f{fn}(varint): {val}")
            elif wt == WIRE_LENGTH_DELIMITED:
                hex_str = " ".join(f"{b:02x}" for b in val[:48])
                logger.debug(f"    hxk f{fn}(bytes, {len(val)}): {hex_str}")


# MapMovementRefusedEvent (hxn) - movement rejected
def handle_movement_refused(state, data, direction, uid):
    """Parse MapMovementRefused - server rejected movement."""
    if direction != "s2c":
        return

    if hasattr(state, 'navigator') and state.navigator:
        state.navigator.movement.on_movement_refused()

    logger.warn(f"  -> Movement REFUSED")


# MapMovementRequest (hqn) - client movement request (log outgoing)
def handle_move_request(state, data, direction, uid):
    """Log outgoing MapMovementRequest for debugging."""
    if direction != "c2s":
        return
    fields = _decode(data)

    logger.info(f"  -> MoveRequest: {len(fields)} fields")
    if getattr(config, 'MOVEMENT_DEBUG', False):
        for fn, wt, val in fields:
            if wt == WIRE_VARINT:
                cell = val & 0xFFF
                dir_val = (val >> 12) & 0xF
                logger.debug(f"    hqn f{fn}(varint): {val} (cell={cell}, dir={dir_val})")
            elif wt == WIRE_LENGTH_DELIMITED:
                hex_str = " ".join(f"{b:02x}" for b in val[:48])
                logger.debug(f"    hqn f{fn}(bytes, {len(val)}): {hex_str}")


# MapCurrentEvent (hxm) - server tells us current map id
def handle_map_current(state, data, direction, uid):
    """Parse MapCurrentEvent - current map reference."""
    if direction != "s2c":
        return
    fields = _decode(data)
    map_id = _get_varint(fields, 1)
    if map_id:
        state.map.map_id = map_id
        logger.info(f"  -> Map ID: {map_id}")


# MapCellsData (gxu) - cell walkability data from server
def handle_cells_data(state, data, direction, uid):
    """Parse MapCellsData - cell walkability information.

    field 1 (bytes) = packed varints, each varint encodes per-cell data.
    We decode as packed varints and extract walkability (bit 0 of each value).
    Also save raw bytes for offline analysis.
    """
    if direction != "s2c":
        return
    fields = _decode(data)

    cell_bytes = _get_field(fields, 1, WIRE_LENGTH_DELIMITED)
    if not cell_bytes:
        logger.info(f"  -> CellsData: no field 1 bytes, raw={len(data)} bytes")
        return

    # Decode as packed varints
    from proxy.packet_handler import decode_varint as _dv
    varints = []
    pos = 0
    while pos < len(cell_bytes):
        try:
            val, pos = _dv(cell_bytes, pos)
            varints.append(val)
        except ValueError:
            break

    # Save raw bytes to file for offline analysis
    import os
    dump_path = os.path.join(os.path.dirname(os.path.dirname(__file__)), "gxu_dump.bin")
    try:
        with open(dump_path, "wb") as f:
            f.write(cell_bytes)
    except Exception:
        pass

    # Populate walkability grid from the navigator
    nav = state.navigator if hasattr(state, 'navigator') else None
    if nav and varints:
        nav.grid.clear()
        # Each varint = cell data. Bit 0 of low byte likely = walkable.
        # Try: low bit 0 = mov (walkable), bits 1-4 = mapChangeData
        walkable_count = 0
        mc_count = 0
        for cell_id, val in enumerate(varints):
            mov = val & 1
            map_change = (val >> 1) & 0xF
            nav.grid.set_walkable(cell_id, mov)
            if map_change:
                nav.grid.map_change_data[cell_id] = map_change
                mc_count += 1
            if mov:
                walkable_count += 1
        logger.info(f"  -> CellsData: {len(varints)} cells decoded, "
                     f"{walkable_count} walkable, {mc_count} map-change")
    else:
        logger.info(f"  -> CellsData: {len(varints)} varints from {len(cell_bytes)} bytes")

    # Debug: log first few values
    if getattr(config, 'MOVEMENT_DEBUG', False):
        sample = varints[:20]
        logger.debug(f"    gxu first 20 varints: {[f'0x{v:X}' for v in sample]}")
        # Also show binary breakdown of first few
        for i, v in enumerate(sample[:5]):
            logger.debug(f"    cell {i}: val=0x{v:X} mov={v&1} mc={(v>>1)&0xF} rest=0x{v>>5:X}")


# ---------------------------------------------------------------------------
# Dofus 3 Movement Protocol (reverse-engineered from live packets)
# ---------------------------------------------------------------------------
# Client move:  ipi (request) -> ion (server confirms) -> inq (client arrived) -> ipa (ack)
# Cancel:       iqa (client cancels mid-move)
# Map change:   ioh (request) -> iny (new position ref) + kmh (server time)
#
# Path encoding:
#   ipi.f1 (bytes) = packed varints, each = (direction << 12) | cellId  (compressed keyframes)
#   ion.f2 (bytes) = packed varints, each = plain cellId               (full expanded path)
#   ipi.f2 (varint) = position reference (composite map+cell, stays same on one map)
#   ion.f3 (varint) = direction/type
#   ion.f5 (varint) = actor/entity ID

from proxy.packet_handler import decode_varint as _decode_varint


def _decode_packed_varints(data):
    """Decode a bytes field as a sequence of packed varints (NOT protobuf fields)."""
    values = []
    pos = 0
    while pos < len(data):
        try:
            val, pos = _decode_varint(data, pos)
            values.append(val)
        except ValueError:
            break
    return values


def _decode_compressed_path(packed_varints):
    """Decode compressed path varints into (cellId, direction) tuples."""
    result = []
    for val in packed_varints:
        cell_id = val & 0xFFF
        direction = (val >> 12) & 0xF
        result.append((cell_id, direction))
    return result


def handle_map_entity_data(state, data, direction, uid):
    """Parse ias - MapEntityData (S2C). Individual entity data, may contain our character."""
    if direction != "s2c":
        return
    if not data:
        return
    fields = _decode(data)

    char_id = state.character.id
    if not char_id:
        return

    # Search for our character ID in this entity data
    if _search_entity_for_char(fields, char_id, 0):
        logger.info(f"  -> FOUND OUR CHARACTER in ias (MapEntityData)!")
        cell = _extract_cell_from_entity(fields)
        if cell is not None:
            old_cell = state.character.cell_id
            state.character.cell_id = cell
            logger.info(f"  -> Cell from ias entity: {cell} (was {old_cell})")


def handle_ipi_move_request(state, data, direction, uid):
    """Parse ipi - MoveRequest (C2S).
    f1 (bytes) = compressed path: packed varints of (dir<<12)|cellId
    f2 (varint) = position reference
    """
    if direction != "c2s":
        return
    if not data:
        return
    fields = _decode(data)

    # Debug: dump ALL fields to understand current protocol structure
    all_fields_debug = []
    for fn, wt, val in fields:
        if wt == WIRE_VARINT:
            all_fields_debug.append(f"f{fn}=varint({val}, 0x{val:X})")
        elif wt == WIRE_LENGTH_DELIMITED:
            # Try to decode as packed varints (keyCells)
            try:
                packed = _decode_packed_varints(val)
                all_fields_debug.append(f"f{fn}=packed({packed})")
            except Exception:
                all_fields_debug.append(f"f{fn}=bytes({len(val)})")
    logger.debug(f"    [MOVE-REQ] fields: {all_fields_debug}")

    # Current protocol: f2=varint(mapId), f3=packed keyCells
    map_id_from_move = _get_varint(fields, 2)
    path_bytes = _get_field(fields, 3, WIRE_LENGTH_DELIMITED)
    if not path_bytes:
        # Fallback: try other field numbers for keyCells
        for try_f in [1, 2, 4]:
            path_bytes = _get_field(fields, try_f, WIRE_LENGTH_DELIMITED)
            if path_bytes:
                logger.debug(f"    [MOVE-REQ] found path in field {try_f}")
                break

    if path_bytes:
        raw_vals = _decode_packed_varints(path_bytes)
        keyframes = _decode_compressed_path(raw_vals)
        cells = [c for c, d in keyframes]
        dirs = [d for c, d in keyframes]
        logger.info(f"  -> MOVE REQUEST: cells={cells} dirs={dirs}")

        if cells:
            # Detect cell mismatch (validates iny cellId extraction)
            if state.character.cell_id is not None and state.character.cell_id != cells[0]:
                logger.warn(f"  -> CELL MISMATCH: state says {state.character.cell_id}, "
                            f"but real client starts from {cells[0]}!")
            # If cell was None or needs update (after map change), cells[0] is the real position
            if state.character.cell_id is None or state._needs_cell_update:
                logger.info(f"  -> Spawn cell detected: {cells[0]}")
                state._needs_cell_update = False
            # Set cells[0] as current position (start), NOT cells[-1] (destination)
            state.character.cell_id = cells[0]
            logger.info(f"  -> Cell: {cells[0]} -> {cells[-1]} ({len(cells)} steps)")

            # Learn walkable cells from real client's accepted MoveRequests
            map_id = state.map.map_id
            if map_id:
                if map_id not in state._observed_walkable:
                    state._observed_walkable[map_id] = set()
                for c in cells:
                    state._observed_walkable[map_id].add(c)
    else:
        logger.info(f"  -> MOVE REQUEST: no path data (all fields logged above)")

    # Store mapId from move request as pos_ref (needed by movement.py)
    if map_id_from_move is not None:
        state.pos_ref = map_id_from_move
        logger.debug(f"    mapId=0x{map_id_from_move:X} ({map_id_from_move})")

    # Mark that we sent a move request (used for character_id auto-detection)
    import time as _time
    state._last_move_request_time = _time.time()


def handle_ion_move_event(state, data, direction, uid):
    """Parse ion - MoveEvent (S2C).
    f2 (bytes) = full path: packed varints of plain cellIds
    f3 (varint) = direction/movement type
    f5 (varint) = actor/entity ID
    """
    if direction != "s2c":
        return
    if not data:
        return
    fields = _decode(data)

    # MoveEvent structure (verified 2026-03-26 from live traffic):
    # f2 (varint) = direction/movement type
    # f3 (bytes)  = packed varints of cellIds (the full path)
    # f4 (varint) = actorId
    # For jsi (matched MapMovementEvent): f1 = bytes (seems different, list of positions)
    actor_id = _get_varint(fields, 4)
    move_type = _get_varint(fields, 2)
    path_bytes = _get_field(fields, 3, WIRE_LENGTH_DELIMITED)

    if path_bytes:
        cells = _decode_packed_varints(path_bytes)
        is_player = (actor_id == state.character.id) if state.character.id and actor_id else False

        # Learn ALL cells from accepted MoveEvents as walkable (server confirmed them)
        map_id = state.map.map_id
        if map_id and cells:
            if map_id not in state._observed_walkable:
                state._observed_walkable[map_id] = set()
            for c in cells:
                state._observed_walkable[map_id].add(c)

        if is_player and cells:
            state.character.cell_id = cells[-1]
            if state._needs_cell_update:
                logger.info(f"  -> Cell resolved after map change: {cells[-1]}")
                state._needs_cell_update = False
            logger.info(f"  -> MOVE EVENT (PLAYER): {cells[0]} -> {cells[-1]} ({len(cells)} cells)")
        elif actor_id and cells:
            if actor_id in state.entities:
                state.entities[actor_id]["cell_id"] = cells[-1]
            logger.debug(f"  -> MOVE EVENT (actor={actor_id}): {cells[0]} -> {cells[-1]}")
    elif _get_field(fields, 1, WIRE_LENGTH_DELIMITED):
        # jsi format: f1 = packed varints (list of actor positions, not a path)
        pos_bytes = _get_field(fields, 1, WIRE_LENGTH_DELIMITED)
        cell_ids = _decode_packed_varints(pos_bytes)
        logger.debug(f"  -> MOVE EVENT (positions): {len(cell_ids)} cells")
    else:
        tree = decode_protobuf_recursive(data, max_depth=3)
        logger.debug(f"  -> MOVE EVENT (unknown):\n{format_proto_tree(tree)}")


def handle_inq_move_confirm(state, data, direction, uid):
    """Parse inq - MoveConfirm (C2S). Cell is already set by MoveRequest handler."""
    if direction != "c2s":
        return
    logger.info(f"  -> MOVE CONFIRM (cell={state.character.cell_id})")


def handle_ipa_move_ack(state, data, direction, uid):
    """Parse ipa - MoveAck (S2C). Usually has no inner data."""
    if direction != "s2c":
        return
    logger.info(f"  -> MOVE ACK")


def handle_ioh_map_change(state, data, direction, uid):
    """Parse ioh - MapChangeRequest (C2S).
    f2 (varint) = target position reference
    """
    if direction != "c2s":
        return
    # Any C2S MapChangeRequest is intentional (real client or bot)
    state._expecting_map_change = True
    if not data:
        logger.info(f"  -> MAP CHANGE REQUEST")
        return
    fields = _decode(data)
    target_ref = _get_varint(fields, 2)
    logger.info(f"  -> MAP CHANGE REQUEST: target=0x{target_ref:X}" if target_ref else "  -> MAP CHANGE REQUEST")


def handle_iao_entity_move(state, data, direction, uid):
    """Parse iao - entity movement event (S2C)."""
    if direction != "s2c":
        return
    if not data:
        return
    fields = _decode(data)
    path_bytes = _get_field(fields, 2, WIRE_LENGTH_DELIMITED)
    actor_id = _get_varint(fields, 5)
    if path_bytes:
        cells = _decode_packed_varints(path_bytes)
        logger.debug(f"  -> ENTITY MOVE: actor={actor_id} path={cells}")
    else:
        logger.debug(f"  -> ENTITY MOVE: actor={actor_id}")


def handle_iaj_entity_move2(state, data, direction, uid):
    """Parse iaj - entity movement event variant (S2C)."""
    if direction != "s2c":
        return
    if not data:
        return
    fields = _decode(data)
    actor_id = _get_varint(fields, 5)
    logger.debug(f"  -> ENTITY MOVE2: actor={actor_id}")


def handle_ipd_move_refused(state, data, direction, uid):
    """Parse ipd - MoveRefused (S2C). Server rejected our ipi request."""
    if direction != "s2c":
        return
    fields = _decode(data) if data else []
    # Log whatever the server sent back
    info_parts = []
    for fn, wt, val in fields:
        if wt == WIRE_VARINT:
            info_parts.append(f"f{fn}={val}")
        elif wt == WIRE_LENGTH_DELIMITED:
            info_parts.append(f"f{fn}=bytes({len(val)})")
    detail = " " + " ".join(info_parts) if info_parts else ""
    logger.warn(f"  -> MOVE REFUSED (ipd){detail}")

    # Notify navigator so it stops waiting
    if hasattr(state, 'navigator') and state.navigator:
        state.navigator.movement.on_movement_refused()


# ---------------------------------------------------------------------------
# Gather Events
# ---------------------------------------------------------------------------

def handle_interact_check_response(state, data, direction, uid):
    """InteractiveUseCheckResponse (ite) — server responds to pre-interaction check.
    Debug: log full hex + decoded protobuf tree for analysis."""
    if direction != "s2c":
        return
    state.interaction_check_ok = True
    hex_dump = data.hex() if data else "(empty)"
    logger.info(f"  -> INTERACT CHECK OK (ite) [{len(data) if data else 0} bytes]: {hex_dump}")
    if data:
        tree = decode_protobuf_recursive(data, max_depth=3)
        logger.debug(f"    [ite decoded]:\n{format_proto_tree(tree)}")


def handle_interact_check_request(state, data, direction, uid):
    """InteractiveUseCheckRequest (itk) — C2S pre-interaction check.
    Debug: log full hex + decoded protobuf tree for comparison with bot-built packets."""
    if direction != "c2s":
        return
    hex_dump = data.hex() if data else "(empty)"
    logger.info(f"  -> INTERACT CHECK REQ (itk) [{len(data) if data else 0} bytes]: {hex_dump}")
    if data:
        tree = decode_protobuf_recursive(data, max_depth=3)
        logger.debug(f"    [itk decoded]:\n{format_proto_tree(tree)}")


def handle_interactive_use_request_sniff(state, data, direction, uid):
    """Sniff which code (itl or idh) the real client uses for InteractiveUseRequest."""
    global _sniffed_interactive_use_code
    if direction == "c2s":
        code = getattr(state, '_current_type_code', None)
        if code in ("itl", "idh"):
            _sniffed_interactive_use_code = code
            logger.info(f"  -> Sniffed InteractiveUseRequest code: {code}")
    fields = _decode(data)
    elem_id = _get_varint(fields, 1)
    if elem_id and direction == "c2s":
        logger.info(f"  -> Client InteractiveUseRequest: elem={elem_id}")


def handle_gather_ready(state, data, direction, uid):
    """InteractiveUsedEvent (jan) — server confirms interaction started."""
    if direction != "s2c":
        return
    state.is_busy = True
    state.busy_reason = "gathering"
    logger.info(f"  -> GATHER STARTED (InteractiveUsedEvent, busy=True)")


def handle_gather_started(state, data, direction, uid):
    """InteractiveUseEndedEvent (jbf) — gather finished."""
    if direction != "s2c":
        return
    logger.info(f"  -> GATHER ENDED (InteractiveUseEndedEvent)")
    state.last_harvest_complete = True
    if state.gatherer:
        state.gatherer.on_gather_ended()
    else:
        state.is_busy = False
        state.busy_reason = None


def handle_gather_request(state, data, direction, uid):
    """Legacy handler — kept for compatibility."""
    pass


def handle_gather_end(state, data, direction, uid):
    """ObjectHarvestedEvent (izr) — item(s) received."""
    if direction != "s2c":
        return
    fields = _decode(data)
    gid = _get_varint(fields, 1)
    qty = _get_varint(fields, 2)
    logger.info(f"  -> HARVESTED gid={gid} qty={qty}")


def handle_iqa_move_cancel(state, data, direction, uid):
    """Parse iqa - movement cancel (C2S).
    f1 (varint) = current cell ID where character stopped
    """
    if direction != "c2s":
        return
    if not data:
        return
    fields = _decode(data)
    cell_id = _get_varint(fields, 1)
    if cell_id is not None:
        state.character.cell_id = cell_id
        logger.info(f"  -> MOVE CANCEL at cell {cell_id}")
    else:
        logger.info(f"  -> MOVE CANCEL")


# ---------------------------------------------------------------------------
# Map Data Response (iou) - Resource positions
# ---------------------------------------------------------------------------
# field 2 (message): repeated - map elements (element_id -> cell_id)
#   f1 = element_id, f3 = layer, f4 = cell_id
# field 6 (message): repeated - resource details
#   f1 = element_id, f2 = { f2 = skill_id, f3 = skillUID }, f4 = status, f5 = resource_type, f6 = enabled

def handle_map_data_response(state, data, direction, uid):
    """Parse MapDataResponse (iou) - extract harvestable resources."""
    if direction != "s2c":
        return
    if not data:
        return

    from game.state import Resource

    fields = _decode(data)

    # Step 1: Build element_id -> cell_id mapping from field 2 (repeated)
    elem_cells = {}
    for elem_data in _get_all_fields(fields, 2, WIRE_LENGTH_DELIMITED):
        ef = _decode(elem_data)
        element_id = _get_varint(ef, 1)
        cell_id = _get_varint(ef, 4)
        if element_id is not None and cell_id is not None:
            elem_cells[element_id] = cell_id

    # Step 2: Parse resource details from field 6 (repeated)
    resources = []
    for res_data in _get_all_fields(fields, 6, WIRE_LENGTH_DELIMITED):
        rf = _decode(res_data)
        element_id = _get_varint(rf, 1)
        if element_id is None:
            continue

        # f2 contains skill info (nested message)
        skill_id = None
        skill_uid = None
        skill_bytes = _get_field(rf, 2, WIRE_LENGTH_DELIMITED)
        if skill_bytes:
            sf = _decode(skill_bytes)
            skill_id = _get_varint(sf, 2)
            skill_uid = _get_varint(sf, 3)

        status = _get_varint(rf, 4)         # 0 = available
        resource_type = _get_varint(rf, 5)   # e.g. 43 = Orge, 45 = Avoine
        enabled = _get_varint(rf, 6)         # 1 = enabled

        cell_id = elem_cells.get(element_id)

        res = Resource(
            element_id=element_id,
            cell_id=cell_id,
            skill_id=skill_id,
            skill_uid=skill_uid,
            resource_type=resource_type,
            status=status if status is not None else 0,
            enabled=bool(enabled) if enabled is not None else True,
        )
        resources.append(res)

    state.map.resources = resources
    avail = [r for r in resources if r.available]
    logger.info(f"  -> Resources detail: {len(avail)} available at cells {[r.cell_id for r in avail]}")
    logger.info(f"  -> Resources: {len(resources)} total, {len(avail)} available")
    for r in avail[:5]:
        logger.info(f"    {r}")
    if len(avail) > 5:
        logger.info(f"    ... and {len(avail) - 5} more")


# ---------------------------------------------------------------------------
# Phase 4: Debug handlers for unknown large messages
# ---------------------------------------------------------------------------

def handle_kww_pre_map(state, data, direction, uid):
    """Handler for kww (~2100 bytes) — cell properties sent BEFORE each isu on map change.

    Format discovered (2026-03-26):
      f3 = (flags << 10) | cellId
      Entries come in PAIRS: same cellId with flags=0 (layer A) and flags=2 (layer B).
      f1 = property value for that cell+layer.
      ~297 entries = ~148 cells with special properties.
      Cells ABSENT from KWW are walkable by default.
    """
    if direction != "s2c":
        return
    if not data:
        return
    fields = _decode(data)
    entries = _get_all_fields(fields, 1, WIRE_LENGTH_DELIMITED)

    parsed_entries = []
    # cellId -> {flags -> f1_value}
    cell_props = {}
    special_cells = set()

    for i, entry_data in enumerate(entries):
        ef = _decode(entry_data)
        f1_val = _get_varint(ef, 1) or 0
        f2_val = _get_varint(ef, 2)
        f3_val = _get_varint(ef, 3) or 0
        f4_val = _get_varint(ef, 4)

        # f3 = (flags << 10) | cellId
        cell_id = f3_val & 0x3FF  # lower 10 bits = cellId
        flags = f3_val >> 10       # upper bits = flags (0=layer A, 2=layer B)

        if 0 <= cell_id <= 559:
            special_cells.add(cell_id)
            if cell_id not in cell_props:
                cell_props[cell_id] = {}
            cell_props[cell_id][flags] = f1_val

        parsed_entries.append({
            "f1": f1_val, "f2": f2_val, "f3": f3_val, "f4": f4_val,
            "cell_id": cell_id, "flags": flags,
        })

        if i < 10:
            logger.debug(f"    [KWW] entry[{i}]: f1={f1_val} f3={f3_val} "
                         f"-> cell={cell_id} flags={flags}")

    # Cells NOT in KWW are walkable by default
    all_cells = set(range(560))
    walkable = all_cells - special_cells

    logger.info(f"  -> KWW: {len(entries)} entries, {len(special_cells)} special cells, "
                f"{len(walkable)} default walkable")

    # Store as pending (kww arrives BEFORE isu, so mapId unknown yet)
    state._pending_kww_cells = cell_props
    state._pending_walkable = walkable

    # Save debug dump
    try:
        import json, os
        debug_data = {
            "total_entries": len(entries),
            "special_cells_count": len(special_cells),
            "walkable_count": len(walkable),
            "special_cells": sorted(special_cells),
            "entries": parsed_entries,
        }
        debug_path = os.path.join(os.path.dirname(os.path.dirname(__file__)), "data", "kww_debug.json")
        with open(debug_path, "w") as f:
            json.dump(debug_data, f, indent=2)
        logger.debug(f"    [KWW] Debug dump saved to {debug_path}")
    except Exception as e:
        logger.debug(f"    [KWW] Failed to save debug dump: {e}")


def handle_ial_large_data(state, data, direction, uid):
    """Handler for ial (~81KB) — full map cell data.
    Field 2 contains ~9000 entries. Each entry has f1=property/bitmask, f2=16384+cellId.
    Parses all entries, extracts cell properties, saves debug dump to data/ial_debug.json."""
    if direction != "s2c":
        return
    if not data:
        return
    fields = _decode(data)
    entries = _get_all_fields(fields, 2, WIRE_LENGTH_DELIMITED)

    if not entries:
        logger.debug(f"  -> IAL: no f2 entries ({len(data)} bytes)")
        return

    # Parse all entries: f1=property, f2=16384+cellId
    cell_properties = {}  # cellId -> [f1_values]
    all_f1_values = set()
    walkable_cells = set()

    for i, entry_data in enumerate(entries):
        ef = _decode(entry_data)
        f1_val = _get_varint(ef, 1)
        f2_val = _get_varint(ef, 2)

        # cellId = f2 - 16384
        cell_id = None
        if f2_val is not None and f2_val >= 16384:
            cell_id = f2_val - 16384
            if 0 <= cell_id <= 559:
                walkable_cells.add(cell_id)
                if cell_id not in cell_properties:
                    cell_properties[cell_id] = []
                if f1_val is not None:
                    cell_properties[cell_id].append(f1_val)
                    all_f1_values.add(f1_val)

        # Log first 5 entries for debug
        if i < 5:
            logger.debug(f"    [IAL] entry[{i}]: f1={f1_val} f2={f2_val} -> cell={cell_id}")

    # Store in state for walkability analysis
    state.ial_cell_properties = cell_properties

    # Save debug dump to data/ial_debug.json
    try:
        import json, os
        debug_data = {
            "total_entries": len(entries),
            "unique_cells": len(walkable_cells),
            "cell_properties": {str(k): v for k, v in cell_properties.items()},
            "unique_f1_values": sorted(all_f1_values),
        }
        debug_path = os.path.join(os.path.dirname(os.path.dirname(__file__)), "data", "ial_debug.json")
        with open(debug_path, "w") as f:
            json.dump(debug_data, f, indent=2)
        logger.debug(f"    [IAL] Debug dump saved to {debug_path}")
    except Exception as e:
        logger.debug(f"    [IAL] Failed to save debug dump: {e}")

    # Store walkable cells
    if walkable_cells:
        map_id = state.map.map_id
        if map_id:
            if not hasattr(state, '_walkable_cache'):
                state._walkable_cache = {}
            state._walkable_cache[map_id] = walkable_cells
            state._walkable_cells = walkable_cells
            if map_id not in state._observed_walkable:
                state._observed_walkable[map_id] = set()
            state._observed_walkable[map_id].update(walkable_cells)

    logger.info(f"  -> IAL: {len(entries)} entries, {len(walkable_cells)} unique cells, "
                f"{len(all_f1_values)} unique f1 values")


# ---------------------------------------------------------------------------
# Registration
# ---------------------------------------------------------------------------

def register_all_handlers(game_state):
    """
    Register all known message handlers with the GameState.
    Keys are stable message names — codes rotate but names never change.
    The GameState.process_message() resolves code->name via Matching at runtime.
    """
    handlers = {
        # --- Character ---
        "CharacterListEvent":          handle_character_list,
        "CharacterSelectEvent":        handle_character_select,
        "CharacterSelectedEvent":      handle_character_loaded,
        "CharacterSelectedSuccessEvent": handle_character_selected_info,
        "CharacterCharacteristicsEvent": handle_character_stats,

        # --- Map ---
        "MapComplementaryInformationEvent": handle_map_info,
        "MapCurrentEvent":             handle_map_current,
        "MapCoordinatesEvent":         handle_map_coordinates,
        "CurrentCellEvent":            handle_current_cell,
        "MapCellPropertiesEvent":      handle_kww_pre_map,
        "MapCellDataEvent":            handle_ial_large_data,
        "MapDataResponse":             handle_map_data_response,
        "InteractiveMapUpdateEvent":   handle_interactive_elements,

        # --- Movement ---
        "MapMovementRequest":          handle_ipi_move_request,    # C2S
        "MapMovementEvent":            handle_ion_move_event,      # S2C
        "MapMovementConfirmRequest":   handle_inq_move_confirm,    # C2S
        "MapMovementConfirmResponse":  handle_ipa_move_ack,        # S2C
        "MapChangeRequest":            handle_ioh_map_change,      # C2S
        "MapMovementCancelRequest":    handle_iqa_move_cancel,     # C2S
        "MapMovementRefusedEvent":     handle_ipd_move_refused,    # S2C

        # --- Interaction / Gather ---
        "InteractiveUseCheckRequest":  handle_interact_check_request,  # C2S pre-check
        "InteractiveUseCheckResponse": handle_interact_check_response, # S2C pre-check reply
        "InteractiveUsedEvent":        handle_gather_ready,        # S2C confirm
        "InteractiveUseEndedEvent":    handle_gather_started,      # S2C end
        "ObjectHarvestedEvent":        handle_gather_end,          # S2C item drop

        # --- Social ---
        "AcquaintancesListEvent":      handle_acquaintance_list,

        # --- Spells ---
        "SpellsEvent":                 handle_spells,

        # --- Fight ---
        "FightJoinEvent":                       handle_fight_join,
        "FightStartingEvent":                   handle_fight_start,
        "FightEndEvent":                        handle_fight_end,
        "GameFightSynchronizeEvent":            handle_fight_synchronize,
        "GameFightShowFighterEvent":            handle_fight_show_fighter,
        "FightTurnStartEvent":                  handle_fight_turn_start,
        "FightTurnEndEvent":                    handle_fight_turn_end,
        "GameFightNewRoundEvent":               handle_fight_new_round,
        "GameActionFightEvent":                 handle_fight_action,
        "FightPlacementPossiblePositionsEvent": handle_fight_placement_positions,
    }

    # Legacy raw-code fallbacks for codes not yet in matching
    # These fire if the stable name hasn't been matched yet but we see the old code
    legacy = {
        "jtl": handle_character_list,
        "jrl": handle_character_loaded,
        "jtx": handle_character_selected_info,
        "hdm": handle_character_stats,
        "hxl": handle_map_info,
        "hxm": handle_map_current,
        "iou": handle_map_data_response,
        "hqo": handle_interactive_elements,
        "ipi": handle_ipi_move_request,
        "ion": handle_ion_move_event,
        "inq": handle_inq_move_confirm,
        "ipa": handle_ipa_move_ack,
        "ioh": handle_ioh_map_change,
        "iqa": handle_iqa_move_cancel,
        "ipd": handle_ipd_move_refused,
        "iao": handle_iao_entity_move,
        "iaj": handle_iaj_entity_move2,
        "klv": handle_gather_ready,
        "kma": handle_gather_started,
        "klj": handle_gather_end,
        "jlm": handle_acquaintance_list,
        "hwa": handle_spells,
        "ibe": handle_fight_start,
        "iaa": handle_map_coordinates,
        "iny": handle_current_cell,
        "gxu": handle_cells_data,
        "ias": handle_map_entity_data,
        "ktg": handle_character_appearance,
        "jtb": handle_character_select,
        "hqn": handle_move_request,
        "hxk": handle_movement_event,
        "hxn": handle_movement_refused,
        # isj = unmatched movement events (other actors on map, same structure as jsi)
        "isj": handle_ion_move_event,
        # isl = movement rejected by server (wrong cell, obstacle, etc.)
        "isl": handle_ipd_move_refused,
        # Interaction pre-check (debug sniffing)
        "itk": handle_interact_check_request,
        "ite": handle_interact_check_response,
        # Dual code InteractiveUseRequest — register BOTH for sniffing
        "itl": handle_interactive_use_request_sniff,
        "idh": handle_interactive_use_request_sniff,
        # Phase 4: debug handlers for unknown large messages
        "kww": handle_kww_pre_map,
        "ial": handle_ial_large_data,
    }

    for name, handler in handlers.items():
        game_state.register_handler(name, handler)
    for code, handler in legacy.items():
        game_state.register_handler(code, handler)

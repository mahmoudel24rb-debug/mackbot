"""
Dofus 3 message handlers.

Each handler parses a specific message type and updates the GameState.
Handlers are registered by type URL code (3-letter).

Message field structures are reverse-engineered from packet captures.
"""

from proxy.packet_handler import decode_protobuf_fields, WIRE_LENGTH_DELIMITED, WIRE_VARINT
from utils import logger
from game.dofus_message import get_type_name


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

    # Map cell from field 6
    cell = _get_varint(char_fields, 6)
    if cell and cell < 1000:
        state.character.cell_id = cell

    state.connected = True
    logger.info(f"  -> Character loaded: level={state.character.level}, cell={state.character.cell_id}")


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
# field 1 (varint): cell_id or entity_position_id

def handle_current_cell(state, data, direction, uid):
    """Parse CurrentCellId - current position on map."""
    if direction != "s2c":
        return
    fields = _decode(data)
    cell = _get_varint(fields, 1)
    if cell is not None:
        state.character.cell_id = cell
        logger.info(f"  -> Current cell: {cell}")


# ---------------------------------------------------------------------------
# Map Complementary Info (hxl)
# ---------------------------------------------------------------------------
# Complex message with entities, interactive elements, etc.
# field 1 (message): repeated - entities/actors
#   field 1 (bytes): actor data
#   field 4 (varint): map_id or type

def handle_map_info(state, data, direction, uid):
    """Parse MapComplementaryInfo - full map data on map change."""
    if direction != "s2c":
        return
    fields = _decode(data)

    # Count entities
    entities = _get_all_fields(fields, 1, WIRE_LENGTH_DELIMITED)
    state.entities.clear()

    entity_count = len(entities)
    logger.info(f"  -> Map loaded: {entity_count} entities on map")


# ---------------------------------------------------------------------------
# Map Players (jlm)
# ---------------------------------------------------------------------------
# field 1 (message): repeated PlayerInfo
#   field 1 (bytes): player data (contains name, level)
#   field 2 (varint): player_id
#   field 4 (varint): cell_id

def handle_map_players(state, data, direction, uid):
    """Parse MapPlayersInfo - players currently on the map."""
    if direction != "s2c":
        return
    fields = _decode(data)
    players = _get_all_fields(fields, 1, WIRE_LENGTH_DELIMITED)

    for player_data in players:
        p_fields = _decode(player_data)
        player_id = _get_varint(p_fields, 2)
        cell_id = _get_varint(p_fields, 4)

        # Player name is inside field 1 (bytes) - nested protobuf
        name = None
        info_bytes = _get_field(p_fields, 1, WIRE_LENGTH_DELIMITED)
        if info_bytes:
            info_fields = _decode(info_bytes)
            name = _get_string(info_fields, 2)

        if player_id:
            from game.state import Entity
            entity = Entity(entity_id=player_id, name=name, cell_id=cell_id)
            entity.entity_type = "player"
            state.entities[player_id] = entity

    if players:
        names = [state.entities[pid].name or str(pid)
                 for pid in state.entities
                 if state.entities[pid].entity_type == "player"]
        logger.info(f"  -> Players on map: {', '.join(names[:10])}")


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
    """Parse SpellList - character spells."""
    if direction != "s2c":
        return
    fields = _decode(data)
    spells = _get_all_fields(fields, 2, WIRE_LENGTH_DELIMITED)
    if spells:
        logger.info(f"  -> Spells loaded: {len(spells)} spells")


# ---------------------------------------------------------------------------
# Fight Events
# ---------------------------------------------------------------------------

def handle_fight_start(state, data, direction, uid):
    """Fight started."""
    state.in_fight = True
    logger.info(f"  -> FIGHT STARTED")


def handle_fight_end(state, data, direction, uid):
    """Fight ended."""
    state.in_fight = False
    state.entities.clear()
    logger.info(f"  -> FIGHT ENDED")


# ---------------------------------------------------------------------------
# Registration
# ---------------------------------------------------------------------------

def register_all_handlers(game_state):
    """Register all known message handlers with the GameState."""
    handlers = {
        "jtl": handle_character_list,
        "jtb": handle_character_select,
        "jrl": handle_character_loaded,
        "jtx": handle_character_selected_info,
        "ktg": handle_character_appearance,
        "hdm": handle_character_stats,
        "iaa": handle_map_coordinates,
        "iny": handle_current_cell,
        "hxl": handle_map_info,
        "jlm": handle_map_players,
        "hqo": handle_interactive_elements,
        "hwa": handle_spells,
        "ibe": handle_fight_start,
    }

    for code, handler in handlers.items():
        game_state.register_handler(code, handler)

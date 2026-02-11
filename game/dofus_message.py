"""
Dofus 3 message parser.

Extracts type URLs and message payloads from raw protobuf game packets.
The protocol uses Google's Any type with URLs like 'type.ankama.com/xxx'.

Game server packet structure:
  - S2C Response: field 3 { field 2 { field 1 = type_url, field 2 = data } }
  - S2C Event:    field 1 { field 1 = uid, field 2 { field 1 = type_url, field 2 = data } }
  - C2S Request:  field 4 { field 1 { field 1 = type_url, field 2 = data }, field 2 = uid }
"""

from proxy.packet_handler import decode_protobuf_fields, WIRE_LENGTH_DELIMITED, WIRE_VARINT


# Known 3-letter type URL codes mapped to readable names.
# Identified from packet analysis. Updated as we discover more.
TYPE_URL_NAMES = {
    # Auth / Connection
    "jol": "GameAuthTicket",
    "joq": "GameAuthAccepted",
    "jon": "Acknowledgement",

    # Server info
    "kmh": "ServerTime",
    "hdn": "ServerConfig",
    "jos": "ServerFeatures",
    "joi": "ServerOptionalFeatures",
    "kme": "ServerMessage",
    "kmg": "ServerMOTD",

    # Account
    "laz": "AccountInfo",
    "hdr": "AccountConfig",

    # Character selection
    "jtl": "CharacterList",
    "jtb": "CharacterSelect",
    "jtx": "CharacterSelectedInfo",
    "jrl": "CharacterLoaded",
    "jud": "CharacterDataRequest",
    "jpc": "CharacterCreation",

    # Character stats
    "hdm": "CharacterStats",
    "juc": "CharacterXP",

    # Map
    "iaa": "MapCoordinates",
    "hxl": "MapComplementaryInfo",
    "gxu": "MapCellsData",
    "gxk": "MapId",
    "gxb": "MapBackground",
    "gxh": "MapPosition",
    "hqo": "MapInteractiveElements",
    "iny": "CurrentCellId",
    "hjd": "MapLoaded",

    # Social / Friends
    "jlm": "AcquaintanceList",
    "jml": "FriendsOnlineList",

    # Movement
    "hqn": "MoveRequest",
    "hqu": "MoveConfirm",
    "hqs": "MapChangeRequest",

    # Spells
    "hwa": "SpellList",
    "hvv": "SpellVariants",
    "hwb": "SpellLevels",

    # Jobs
    "hft": "JobExperience",
    "hgh": "JobList",
    "hci": "JobLevelUp",

    # Fight
    "hde": "FightReady",
    "ibe": "FightStart",
    "icd": "FightTeams",
    "idh": "FightOptions",
    "idm": "FightId",

    # Inventory / Items
    "iqz": "QuestList",
    "laf": "AchievementList",
    "hhb": "SocialInfo",

    # Gathering
    "klv": "GatherReady",
    "kll": "GatherRequest",

    # Social
    "jkj": "FriendsListRequest",
    "jkv": "FriendsListResponse",
    "jkm": "IgnoredListResponse",

    # Misc
    "lbk": "ServerSettings1",
    "lbm": "ServerSettings2",
    "jph": "ServerSettings3",
    "jsv": "PingPong",
    "jvm": "ServerLimits",
    "kre": "ServerRules",
    "ksy": "CraftReady",
    "ksc": "CraftInfo",
    "ktc": "TreasureHunt",
    "ktg": "CharacterAppearance",
    "kts": "TaxCollector",
    "hzh": "EmoteList",
    "jrj": "AlmanaxInfo",
    "hrg": "AlignmentInfo",
    "ify": "ServerEvent",
    "jno": "GuildInfoRequest",
    "jna": "AllianceInfoRequest",
    "jne": "PvpInfoRequest",
    "jmz": "SocialNotifications",
    "jly": "NpcDialogRequest",
    "hcm": "ChatRequest",
    "ijc": "InteractiveUseRequest",
    "igs": "ExchangeRequest",
    "job": "GameContextRequest",
    "jnm": "PartyInfoRequest",

    # Map movement
    "hqr": "MapChangeConfirm",
    "kpo": "PrestigeInfo",
    "jfj": "ShortcutBar",
}


def get_type_name(code):
    """Get readable name for a 3-letter type URL code."""
    return TYPE_URL_NAMES.get(code, code)


def extract_message_info(payload, direction):
    """
    Extract the type URL code and inner message data from a game packet.

    Args:
        payload: raw protobuf bytes of the outer GameMessage
        direction: "c2s" or "s2c"

    Returns:
        list of (type_code, message_data, uid) tuples.
        Multiple messages can be in one packet (though rare).
    """
    messages = []
    fields = decode_protobuf_fields(payload)

    for fn, wt, val in fields:
        if wt != WIRE_LENGTH_DELIMITED:
            continue

        # S2C Response: field 3 -> field 2 has Any
        if fn == 3 and direction == "s2c":
            msg = _parse_response(val)
            if msg:
                messages.append(msg)

        # S2C Event: field 1 -> field 2 has Any
        elif fn == 1 and direction == "s2c":
            msg = _parse_event(val)
            if msg:
                messages.append(msg)

        # C2S Request: field 4 -> field 1 has Any
        elif fn == 4 and direction == "c2s":
            msg = _parse_request(val)
            if msg:
                messages.append(msg)

        # Login: field 1 for c2s, field 2 for s2c
        elif fn == 1 and direction == "c2s":
            msg = _parse_login_request(val)
            if msg:
                messages.append(msg)
        elif fn == 2 and direction == "s2c":
            msg = _parse_login_response(val)
            if msg:
                messages.append(msg)

    return messages


def _extract_any(data):
    """
    Extract type_url and inner data from a Google Any message.
    Any: field 1 = type_url (string), field 2 = value (bytes)
    """
    fields = decode_protobuf_fields(data)
    type_url = None
    value = None
    for fn, wt, val in fields:
        if fn == 1 and wt == WIRE_LENGTH_DELIMITED:
            try:
                type_url = val.decode("utf-8")
            except (UnicodeDecodeError, ValueError):
                pass
        elif fn == 2 and wt == WIRE_LENGTH_DELIMITED:
            value = val
    return type_url, value


def _url_to_code(type_url):
    """Extract 3-letter code from type URL like 'type.ankama.com/jrl'."""
    if type_url and "/" in type_url:
        return type_url.rsplit("/", 1)[-1]
    return type_url


def _parse_response(wrapper_data):
    """Parse a Response wrapper (field 3 of GameMessage)."""
    fields = decode_protobuf_fields(wrapper_data)
    for fn, wt, val in fields:
        if fn == 2 and wt == WIRE_LENGTH_DELIMITED:
            type_url, data = _extract_any(val)
            code = _url_to_code(type_url)
            if code:
                return (code, data, None)
    return None


def _parse_event(wrapper_data):
    """Parse an Event wrapper (field 1 of GameMessage)."""
    fields = decode_protobuf_fields(wrapper_data)
    uid = None
    for fn, wt, val in fields:
        if fn == 1 and wt == WIRE_VARINT:
            uid = val
        elif fn == 2 and wt == WIRE_LENGTH_DELIMITED:
            type_url, data = _extract_any(val)
            code = _url_to_code(type_url)
            if code:
                return (code, data, uid)
    return None


def _parse_request(wrapper_data):
    """Parse a C2S Request wrapper (field 4 of GameMessage)."""
    fields = decode_protobuf_fields(wrapper_data)
    uid = None
    for fn, wt, val in fields:
        if fn == 2 and wt == WIRE_VARINT:
            uid = val
        elif fn == 1 and wt == WIRE_LENGTH_DELIMITED:
            type_url, data = _extract_any(val)
            code = _url_to_code(type_url)
            if code:
                return (code, data, uid)
    return None


def _parse_login_request(wrapper_data):
    """Parse a login c2s message (field 1 of LoginMessage)."""
    fields = decode_protobuf_fields(wrapper_data)
    # Login messages have nested content without Any
    # Just return the wrapper field number
    for fn, wt, val in fields:
        if fn >= 3 and wt == WIRE_LENGTH_DELIMITED:
            return (f"login_req_{fn}", val, None)
    return None


def _parse_login_response(wrapper_data):
    """Parse a login s2c message (field 2 of LoginMessage)."""
    fields = decode_protobuf_fields(wrapper_data)
    for fn, wt, val in fields:
        if fn >= 3 and wt == WIRE_LENGTH_DELIMITED:
            return (f"login_resp_{fn}", val, None)
    return None

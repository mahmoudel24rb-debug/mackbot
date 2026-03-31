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

    # Movement (Dofus 3 actual protocol - reverse engineered from logs)
    "ipi": "MoveRequest",          # C2S: client clicks to move
    "ion": "MoveEvent",            # S2C: server confirms/broadcasts movement
    "inq": "MoveConfirm",          # C2S: client confirms arrival
    "ipa": "MoveAck",              # S2C: server acknowledges arrival
    "ioh": "MapChangeRequest",     # C2S: client requests map change
    "iqa": "MoveCancel",           # C2S: client cancels movement (?)
    "ipd": "MoveRefused",          # S2C: server rejects movement request
    "iao": "EntityMoveEvent",      # S2C: NPC/entity movement
    "iaj": "EntityMoveEvent2",     # S2C: NPC/entity movement variant

    # Movement (legacy codes from Dofus 2 - may not be used in Dofus 3)
    "hqn": "MapMovementRequest_v2",
    "hqu": "MapMovementConfirm_v2",
    "hxk": "MapMovementEvent_v2",
    "hxn": "MapMovementRefused_v2",
    "hqs": "MapMovementCancel_v2",
    "hqc": "MapChangeRequest_v2",
    "hqr": "MapChangeOrientation",
    "hxj": "MapChangeOrientationEvent",
    "hxm": "MapCurrentEvent",

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
    "klj": "GatherComplete",
    "kma": "GatherStarted",
    "kmi": "GatherAction",

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

    # Map data
    "kpo": "PrestigeInfo",
    "jfj": "ShortcutBar",

    # Seen in logs - unknown purpose
    "jsc": "GameInit1",
    "had": "GameInit2",
    "hbo": "GameInit3",
    "hzf": "GameInit4",
    "hfh": "JobDetails",
    "jdn": "LargeDataDump",         # 88KB - inventory/world data?
    "jbs": "ClientReady",
    "hgj": "JobAction",             # C2S, repeated during init
    "hfe": "JobConfig",
    "jpa": "PingRequest",           # C2S
    "jor": "PingResponse",          # S2C
    "kmx": "ClientConfig1",
    "iic": "ClientConfig2",
    "kml": "ServerConfig2",
    "hcz": "ChatConfig",
    "jrg": "AlmanaxRequest",
    "hck": "ChatConfig2",
    "jqr": "QuestProgress",         # S2C, repeated
    "jqx": "QuestConfig",
    "ktq": "TreasureHuntRequest",
    "ipw": "MapDataRequest",         # C2S
    "iou": "MapDataResponse",        # S2C
    "ias": "MapEntityData",          # S2C, repeated
    "kpu": "PrestigeConfig",
    "ktm": "TreasureHuntData",
}


def get_type_name(code, matching=None):
    """
    Get readable name for a 3-letter type URL code.
    Checks the dynamic matching first (if provided), then the static dict.
    """
    if matching is not None:
        name = matching.get_name(code)
        if name != code:
            return name
    return TYPE_URL_NAMES.get(code, code)


def extract_message_info(payload, direction):
    """
    Extract the type URL code and inner message data from a game packet.

    Tries all known wrapper structures (and falls back to deep search) to be
    resilient against outer field number changes between game versions.

    Known structures (may vary after Ankama updates):
      S2C Event:    outer field 1 or 3  { field1=uid(varint), field2=Any }
      S2C Response: outer field 3       { field2=Any }
      C2S Request:  outer field 2 or 4  { field1=Any, field2=uid(varint) }

    Returns:
        list of (type_code, message_data, uid) tuples.
    """
    messages = []
    try:
        fields = list(decode_protobuf_fields(payload))
    except Exception:
        return messages

    for fn, wt, val in fields:
        if wt != WIRE_LENGTH_DELIMITED:
            continue

        # Try the documented wrapper structures first
        msg = _try_all_parsers(val)
        if msg:
            messages.append(msg)
            continue

        # Login messages (non-Any nested content)
        if fn == 1 and direction == "c2s":
            msg = _parse_login_request(val)
            if msg:
                messages.append(msg)
        elif fn == 2 and direction == "s2c":
            msg = _parse_login_response(val)
            if msg:
                messages.append(msg)

    return messages


def _try_all_parsers(wrapper_data):
    """
    Try to extract an Any message from a wrapper, attempting all known
    field layouts. Returns (code, data, uid) or None.
    """
    try:
        inner = list(decode_protobuf_fields(wrapper_data))
    except Exception:
        return None

    uid = None
    any_candidate = None

    for fn, wt, val in inner:
        if wt == WIRE_VARINT:
            # uid is typically field 1 or 2 as a varint
            if fn in (1, 2) and uid is None:
                uid = val
        elif wt == WIRE_LENGTH_DELIMITED:
            # Any candidate: field 1 or 2 containing type.ankama.com/
            if fn in (1, 2) and any_candidate is None:
                type_url, data = _extract_any(val)
                if type_url and "type.ankama.com/" in type_url:
                    any_candidate = (type_url, data)

    if any_candidate:
        type_url, data = any_candidate
        code = _url_to_code(type_url)
        if code:
            return (code, data, uid)
    return None


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

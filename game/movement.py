"""
Dofus 3 Movement - Packet construction and sending.

Protocol (verified 2026-03-26 from live traffic):
  Move:       MapMovementRequest (C2S) -> MapMovementEvent (S2C) -> MapMovementConfirmRequest (C2S) -> MapMovementConfirmResponse (S2C)
  Map change: MapChangeRequest (C2S) -> MapComplementaryInformationEvent (S2C)

MapMovementRequest encoding (verified):
  field 2 (varint) = mapId (int64)
  field 3 (length-delimited) = keyCells (packed repeated int32, each = (direction << 12) | cellId)
  Directions: 0=E, 1=SE, 2=S, 3=SW, 4=W, 5=NW, 6=N, 7=NE

MapMovementConfirmRequest:
  field 1 (varint) = 1 (bool true)

MapChangeRequest (verified):
  field 2 (varint) = target mapId

Framing: varint length prefix (NOT 4-byte big-endian).
"""

import asyncio
from proxy.packet_handler import (
    encode_protobuf_fields, encode_varint,
    WIRE_VARINT, WIRE_LENGTH_DELIMITED,
)
from game.map_grid import compress_path
from utils import logger


# ---------------------------------------------------------------------------
# Packet builders
# ---------------------------------------------------------------------------

def build_any_message(type_code, inner_data):
    """Build a Google Any wrapper: field 1 = type_url, field 2 = data."""
    type_url = f"type.ankama.com/{type_code}"
    fields = [(1, WIRE_LENGTH_DELIMITED, type_url.encode("utf-8"))]
    if inner_data:
        fields.append((2, WIRE_LENGTH_DELIMITED, inner_data))
    return encode_protobuf_fields(fields)


def build_c2s_request(type_code, inner_data, uid):
    """
    Build a C2S GameMessage request.

    Verified from real client traffic (game auth hex dump):
      GameMessage field 2 (NOT field 4!) contains:
        field 1 = uid (varint, real client uses -1 = 0xFFFFFFFFFFFFFFFF)
        field 2 = Any{type_url, data}
    """
    any_msg = build_any_message(type_code, inner_data)
    # Real client: field 2 wrapper with f1=uid, f2=Any
    request_wrapper = encode_protobuf_fields([
        (1, WIRE_VARINT, uid),
        (2, WIRE_LENGTH_DELIMITED, any_msg),
    ])
    return encode_protobuf_fields([
        (2, WIRE_LENGTH_DELIMITED, request_wrapper),
    ])


def _encode_packed_varints(values):
    """Encode a list of integers as concatenated varints (packed encoding)."""
    parts = []
    for v in values:
        parts.append(encode_varint(v))
    return b"".join(parts)


def build_move_request(path_cells, map_id):
    """
    Build MapMovementRequest inner message.

    Protocol (verified 2026-03-26):
      field 2 (varint)           = mapId (int64)
      field 3 (length-delimited) = keyCells (packed repeated int32)

    Args:
        path_cells: list of cellIds from A* pathfinding
        map_id: current mapId (from state.pos_ref which IS the mapId)

    keyCells are compressed keyframes: (direction << 12) | cellId
    """
    compressed = compress_path(path_cells)
    path_data = _encode_packed_varints(compressed)

    fields = []
    # field 2 = mapId (varint) — verified from sniffed traffic
    if map_id is not None:
        fields.append((2, WIRE_VARINT, map_id))
    # field 3 = keyCells (packed repeated int32) — verified from sniffed traffic
    fields.append((3, WIRE_LENGTH_DELIMITED, path_data))
    return encode_protobuf_fields(fields)


def build_move_confirm():
    """
    Build MapMovementConfirmRequest inner message.

    Protocol (verified 2026-03-26): payload is 4 bytes containing field 1 = varint 1 (bool true).
    The real client sends this, NOT an empty body.
    """
    # field 1 = 1 (bool true) — matches sniffed traffic (4 bytes payload)
    return encode_protobuf_fields([
        (1, WIRE_VARINT, 1),
    ])


def build_map_change_request(map_id):
    """
    Build MapChangeRequest inner message.

    Protocol (verified 2026-03-26): the sniffed handler reads field 2 as target mapId.
    field 2 (varint) = target MapId
    TODO: Verify by comparing bot-sent packet with real client packet via sniffer.
    """
    return encode_protobuf_fields([
        (2, WIRE_VARINT, map_id),
    ])


def build_pre_interact_request():
    """
    Build InteractiveUseCheckRequest (itk) inner message.

    Confirmed from live traffic (2026-03-26): payload is EMPTY (0 bytes).
    The itk message is just a wrapped GameMessage with no inner data.
    """
    return None


def build_interact_request(element_id):
    """
    Build InteractiveUseRequest (itl) inner message.

    Confirmed from live traffic (2026-03-26):
      field 1 (varint) = elementId ONLY
      NO skillId field — real client sends 4 bytes inner (not 7).
      Bot was sending f1=elementId + f2=skillId = 46 bytes total.
      Real client sends f1=elementId only = 43 bytes total.
    """
    return encode_protobuf_fields([
        (1, WIRE_VARINT, element_id),
    ])


def build_gather_request(action_counter):
    """Build GatherRequest (kll) inner message.
    f2 (varint) = action_counter (incremental per session)
    """
    return encode_protobuf_fields([
        (2, WIRE_VARINT, action_counter),
    ])


def build_gather_action(action_type=71):
    """Build GatherAction (kmi) inner message.
    f1 (varint) = action_type (71 = harvest)
    """
    return encode_protobuf_fields([
        (1, WIRE_VARINT, action_type),
    ])


def build_fight_turn_ready():
    """Build FightTurnReadyRequest (C2S) — signal ready to start/end our turn.
    Empty body (no inner data).
    """
    return None


def build_fight_cast_request(spell_id, target_cell):
    """Build GameActionFightCastRequest (C2S) — cast a spell.
    field 1 (varint) = spellId
    field 2 (varint) = targetCellId
    """
    return encode_protobuf_fields([
        (1, WIRE_VARINT, spell_id),
        (2, WIRE_VARINT, target_cell),
    ])


def build_fight_move_request(path_cells):
    """Build GameActionFightMoveRequest (C2S) — move in fight.
    field 1 (bytes) = packed path varints (same format as map move)
    """
    from game.map_grid import compress_path
    compressed = compress_path(path_cells)
    path_data = _encode_packed_varints(compressed)
    return encode_protobuf_fields([
        (1, WIRE_LENGTH_DELIMITED, path_data),
    ])


def frame_packet(payload):
    """Add varint length prefix (Dofus 3 game server framing)."""
    return encode_varint(len(payload)) + payload


# ---------------------------------------------------------------------------
# MovementController
# ---------------------------------------------------------------------------

class MovementController:
    """
    Handles character movement through the proxy.
    Uses the proxy's server_writer to inject packets into the game stream.
    """

    def __init__(self, game_state):
        self.game_state = game_state
        self._server_writer = None
        self._uid_counter = 1000
        self._moving = False
        self._move_refused = False
        self._move_complete = asyncio.Event()
        self._map_changed = asyncio.Event()

    def set_server_writer(self, writer):
        """Set the asyncio writer for sending to the game server."""
        self._server_writer = writer

    @property
    def is_connected(self):
        return self._server_writer is not None

    def _next_uid(self):
        # Real client uses uid = -1 (0xFFFFFFFFFFFFFFFF as unsigned).
        # Protobuf encodes this as 10-byte varint: ff ff ff ff ff ff ff ff ff 01
        return 0xFFFFFFFFFFFFFFFF

    async def send_packet(self, payload):
        """Send a framed packet to the game server."""
        if not self._server_writer:
            logger.error("[MOV] No server writer!")
            return False

        frame = frame_packet(payload)
        # Debug: log raw bytes of injected packet for comparison with real client
        hex_preview = " ".join(f"{b:02x}" for b in frame[:80])
        logger.debug(f"  [BOT-SEND] {len(frame)} bytes: {hex_preview}{'...' if len(frame) > 80 else ''}")
        self._server_writer.write(frame)
        await self._server_writer.drain()
        return True

    async def move_to_cell(self, path):
        """
        Send a movement request along a path of cell IDs.

        Args:
            path: list of cellIds (from pathfinding)

        Returns:
            True if request was sent
        """
        if not path or len(path) < 2:
            return False

        # pos_ref is actually the mapId (verified 2026-03-26)
        map_id = self.game_state.pos_ref
        if map_id is None:
            logger.error("[MOV] No mapId available (move manually first to detect it)")
            return False

        self._moving = True
        self._move_refused = False
        self._move_complete.clear()

        code = self.game_state.matching.get_code("MapMovementRequest")
        if not code:
            logger.error("[MOV] MapMovementRequest code unknown — run sniffer first")
            self._moving = False
            return False

        inner = build_move_request(path, map_id)
        uid = self._next_uid()
        packet = build_c2s_request(code, inner, uid)
        # Debug: show inner payload hex for comparison with sniffed real client packets
        logger.debug(f"  [BOT] code={code}, uid={uid}, mapId={map_id}, inner={inner.hex()}")

        logger.info(f"  [BOT] Moving: cell {path[0]} -> {path[-1]} ({len(path)} steps)")

        success = await self.send_packet(packet)
        if not success:
            self._moving = False
        return success

    async def confirm_move(self):
        """Send movement confirmation (MapMovementConfirmRequest with bool payload)."""
        code = self.game_state.matching.get_code("MapMovementConfirmRequest")
        if not code:
            logger.error("[MOV] MapMovementConfirmRequest code unknown")
            return False
        # build_move_confirm() now returns field 1=1 (bool true), not None
        inner = build_move_confirm()
        packet = build_c2s_request(code, inner, self._next_uid())
        logger.info(f"  [BOT] Move confirmed")
        return await self.send_packet(packet)

    async def request_map_change(self, map_id):
        """Request a map change to a target mapId."""
        code = self.game_state.matching.get_code("MapChangeRequest")
        if not code:
            logger.error("[MOV] MapChangeRequest code unknown")
            return False
        inner = build_map_change_request(map_id)
        packet = build_c2s_request(code, inner, self._next_uid())
        logger.info(f"  [BOT] Map change -> mapId={map_id}")
        return await self.send_packet(packet)

    # --- Event callbacks (called by message handlers) ---

    def on_movement_event(self):
        """Called when server confirms movement (ion)."""
        self._moving = False
        self._move_complete.set()

    def on_movement_refused(self):
        """Called when server refuses movement."""
        self._moving = False
        self._move_refused = True
        self._move_complete.set()
        logger.warn("  [BOT] Movement refused by server!")

    def on_map_changed(self):
        """Called when new map data arrives (hxl)."""
        self._map_changed.set()

    async def wait_move_complete(self, timeout=10.0):
        """Wait for movement to complete (ion event). Returns False on timeout or refusal."""
        try:
            await asyncio.wait_for(self._move_complete.wait(), timeout)
            if self._move_refused:
                return False
            return True
        except asyncio.TimeoutError:
            self._moving = False
            return False

    async def wait_map_change(self, timeout=15.0):
        """Wait for map change to complete (hxl + iny)."""
        self._map_changed.clear()
        try:
            await asyncio.wait_for(self._map_changed.wait(), timeout)
            return True
        except asyncio.TimeoutError:
            return False

    # --- Fight actions ---

    async def send_turn_ready(self):
        """Send FightTurnReadyRequest — signal ready to start/end our turn."""
        code = self.game_state.matching.get_code("FightTurnReadyRequest")
        if not code:
            logger.error("[MOV] FightTurnReadyRequest code unknown")
            return False
        inner = build_fight_turn_ready()
        packet = build_c2s_request(code, inner, self._next_uid())
        logger.info("  [BOT] Fight: turn ready")
        return await self.send_packet(packet)

    async def send_cast_spell(self, spell_id, target_cell):
        """Send GameActionFightCastRequest — cast a spell."""
        code = self.game_state.matching.get_code("GameActionFightCastRequest")
        if not code:
            logger.error("[MOV] GameActionFightCastRequest code unknown")
            return False
        inner = build_fight_cast_request(spell_id, target_cell)
        packet = build_c2s_request(code, inner, self._next_uid())
        logger.info(f"  [BOT] Fight: cast spell {spell_id} -> cell {target_cell}")
        return await self.send_packet(packet)

    async def send_fight_move(self, path_cells):
        """Send GameActionFightMoveRequest — move in fight."""
        code = self.game_state.matching.get_code("GameActionFightMoveRequest")
        if not code:
            logger.error("[MOV] GameActionFightMoveRequest code unknown")
            return False
        inner = build_fight_move_request(path_cells)
        packet = build_c2s_request(code, inner, self._next_uid())
        logger.info(f"  [BOT] Fight: move to cell {path_cells[-1]}")
        return await self.send_packet(packet)

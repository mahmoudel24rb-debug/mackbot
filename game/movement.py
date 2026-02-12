"""
Dofus 3 Movement - Packet construction and sending.

Movement protocol:
  1. C2S: MapMovementRequest (hqn) - send compressed path
  2. S2C: MapMovementEvent (hxk) - server confirms/broadcasts
  3. C2S: MapMovementConfirmRequest (hqu) - confirm arrival

Map change:
  1. Walk to edge cell with MapChangeData flag
  2. C2S: MapChangeRequest (hqs) - request transition
  3. S2C: MapComplementaryInfo (hxl) - new map data
"""

import asyncio
import struct
from proxy.packet_handler import (
    encode_protobuf_fields, encode_varint,
    WIRE_VARINT, WIRE_LENGTH_DELIMITED,
)
from game.map_grid import compress_path, MAP_CHANGE_RIGHT, MAP_CHANGE_BOTTOM, MAP_CHANGE_LEFT, MAP_CHANGE_TOP
from utils import logger


# ---------------------------------------------------------------------------
# Packet builders
# ---------------------------------------------------------------------------

def build_any_message(type_code, inner_data):
    """Build a Google Any wrapper: field 1 = type_url, field 2 = data."""
    type_url = f"type.ankama.com/{type_code}"
    return encode_protobuf_fields([
        (1, WIRE_LENGTH_DELIMITED, type_url.encode("utf-8")),
        (2, WIRE_LENGTH_DELIMITED, inner_data),
    ])


def build_c2s_request(type_code, inner_data, uid):
    """
    Build a C2S GameMessage request.
    Structure: field 4 { field 1 = Any{type_url, data}, field 2 = uid }
    """
    any_msg = build_any_message(type_code, inner_data)
    request_wrapper = encode_protobuf_fields([
        (1, WIRE_LENGTH_DELIMITED, any_msg),
        (2, WIRE_VARINT, uid),
    ])
    return encode_protobuf_fields([
        (4, WIRE_LENGTH_DELIMITED, request_wrapper),
    ])


def build_move_request(path_cells):
    """
    Build MapMovementRequest (hqn) inner message.

    Sends compressed path: repeated varint field 1.
    Each value = (direction << 12) | cellId.
    """
    compressed = compress_path(path_cells)
    fields = [(1, WIRE_VARINT, cell) for cell in compressed]
    return encode_protobuf_fields(fields)


def build_move_confirm():
    """Build MapMovementConfirmRequest (hqu) - empty message."""
    return b""


def build_map_change_request(direction_flag):
    """Build MapChangeRequest (hqs) inner message."""
    return encode_protobuf_fields([
        (1, WIRE_VARINT, direction_flag),
    ])


def frame_packet(payload):
    """Add 4-byte big-endian length prefix."""
    return struct.pack(">I", len(payload)) + payload


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
        self._move_complete = asyncio.Event()
        self._map_changed = asyncio.Event()

    def set_server_writer(self, writer):
        """Set the asyncio writer for sending to the game server."""
        self._server_writer = writer

    @property
    def is_connected(self):
        return self._server_writer is not None

    def _next_uid(self):
        self._uid_counter += 1
        return self._uid_counter

    async def send_packet(self, payload):
        """Send a framed packet to the game server."""
        if not self._server_writer:
            logger.error("[MOV] No server writer!")
            return False

        frame = frame_packet(payload)
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

        self._moving = True
        self._move_complete.clear()

        inner = build_move_request(path)
        packet = build_c2s_request("hqn", inner, self._next_uid())

        logger.info(f"  [BOT] Moving: cell {path[0]} -> {path[-1]} ({len(path)} steps)")

        success = await self.send_packet(packet)
        if not success:
            self._moving = False
        return success

    async def confirm_move(self):
        """Send movement confirmation after arriving."""
        inner = build_move_confirm()
        packet = build_c2s_request("hqu", inner, self._next_uid())
        logger.info(f"  [BOT] Move confirmed")
        return await self.send_packet(packet)

    async def request_map_change(self, direction_flag):
        """Request a map change in the given direction."""
        inner = build_map_change_request(direction_flag)
        packet = build_c2s_request("hqs", inner, self._next_uid())

        dir_names = {1: "right", 2: "bottom", 4: "left", 8: "top"}
        logger.info(f"  [BOT] Map change: {dir_names.get(direction_flag, '?')}")
        return await self.send_packet(packet)

    # --- Event callbacks (called by message handlers) ---

    def on_movement_event(self):
        """Called when server confirms movement (hxk)."""
        self._moving = False
        self._move_complete.set()

    def on_movement_refused(self):
        """Called when server refuses movement (hxn)."""
        self._moving = False
        self._move_complete.set()
        logger.warn("  [BOT] Movement refused by server!")

    def on_map_changed(self):
        """Called when new map data arrives (hxl)."""
        self._map_changed.set()

    async def wait_move_complete(self, timeout=10.0):
        """Wait for movement to complete."""
        try:
            await asyncio.wait_for(self._move_complete.wait(), timeout)
            return True
        except asyncio.TimeoutError:
            self._moving = False
            return False

    async def wait_map_change(self, timeout=15.0):
        """Wait for map change to complete."""
        self._map_changed.clear()
        try:
            await asyncio.wait_for(self._map_changed.wait(), timeout)
            return True
        except asyncio.TimeoutError:
            return False

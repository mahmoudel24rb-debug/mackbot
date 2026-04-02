"""
Dofus 3 Gathering - Orchestrates the harvest sequence.

The bot only handles the interaction (itk+itl). Movement is done manually
by the player — the bot does NOT inject MoveRequests for gathering.

Real client gather sequence (confirmed 2026-03-26):
  1. Player moves adjacent to resource (manual or script)
  2. C2S itk (empty payload) + C2S itl (f1=elementId) back-to-back (~6ms apart)
  3. S2C ite (arrives after itl, not waited for)
  4. S2C irj (interaction validated)
  5. S2C kof (InteractiveUseEndedEvent — harvest done)
  6. S2C kot (ObjectHarvestedEvent — item received)
"""

import asyncio
from game.movement import build_c2s_request, build_interact_request, build_pre_interact_request
from game.map_grid import get_neighbors
from utils import logger


class GatherController:
    """
    Handles the gather interaction through the MITM proxy.
    Does NOT move the character — only sends itk+itl when already adjacent.
    """

    def __init__(self, game_state):
        self.game_state = game_state
        self._gather_done = asyncio.Event()

    @property
    def navigator(self):
        return self.game_state.navigator

    @property
    def movement(self):
        return self.navigator.movement if self.navigator else None

    def on_gather_ended(self):
        """Called by message handler when InteractiveUseEndedEvent arrives."""
        self.game_state.is_busy = False
        self.game_state.busy_reason = None
        self.game_state.interaction_check_ok = False  # Reset for next gather
        self._gather_done.set()

    def _is_adjacent(self, player_cell, resource_cell):
        """Check if player is adjacent to resource (any of 8 neighbors, WIDTH=14)."""
        if player_cell is None or resource_cell is None:
            return False
        neighbors = set(n_id for n_id, _ in get_neighbors(resource_cell))
        return player_cell in neighbors

    async def send_pre_interact(self):
        """Send InteractiveUseCheckRequest (itk) — empty payload."""
        code = self.game_state.matching.get_code("InteractiveUseCheckRequest")
        if not code:
            logger.error("[GATHER] InteractiveUseCheckRequest code unknown")
            return False
        inner = build_pre_interact_request()
        packet = build_c2s_request(code, inner, self.movement._next_uid())
        logger.info(f"  [GATHER] Sending itk")
        return await self.movement.send_packet(packet)

    async def send_interact(self, element_id):
        """Send InteractiveUseRequest (itl) — f1=elementId ONLY."""
        code = self.game_state.matching.get_code("InteractiveUseRequest")
        if not code:
            logger.error("[GATHER] InteractiveUseRequest code unknown")
            return False
        inner = build_interact_request(element_id)
        packet = build_c2s_request(code, inner, self.movement._next_uid())
        logger.info(f"  [GATHER] Sending itl: elem={element_id}")
        return await self.movement.send_packet(packet)

    async def wait_gather_complete(self, timeout=15.0):
        """Wait for InteractiveUseEndedEvent (kof)."""
        self._gather_done.clear()
        try:
            await asyncio.wait_for(self._gather_done.wait(), timeout)
            return True
        except asyncio.TimeoutError:
            logger.warn(f"  [GATHER] Timeout waiting for gather complete ({timeout}s)")
            self.game_state.is_busy = False
            self.game_state.busy_reason = None
            return False

    async def gather_resource(self, resource):
        """
        Gather a resource. Player must already be adjacent.
        Sends itk + itl back-to-back (like real client), then waits for kof.

        Returns:
            True if gathered successfully
        """
        # Reset interaction state for a clean gather
        self.game_state.interaction_check_ok = False
        self.game_state.last_harvest_complete = False

        if not self.navigator or not self.movement or not self.movement.is_connected:
            logger.error("[GATHER] Not connected")
            return False

        if resource.cell_id is None:
            logger.error(f"[GATHER] Resource {resource.element_id} has no cell_id")
            return False

        current_cell = self.game_state.character.cell_id
        if current_cell is None:
            logger.error("[GATHER] Unknown position — move manually first")
            return False

        # Check adjacency — all 8 neighbors with WIDTH=14
        if not self._is_adjacent(current_cell, resource.cell_id):
            logger.error(f"[GATHER] Not adjacent to resource at cell {resource.cell_id} "
                         f"(player at {current_cell}). Move closer manually and retry.")
            return False

        logger.info(f"  [GATHER] Adjacent at cell {current_cell}, resource at {resource.cell_id}")

        self.game_state.is_busy = True
        self.game_state.busy_reason = "gathering"

        # Check if ite already arrived (real client auto-sends itk on arrival)
        ite_received = self.game_state.interaction_check_ok

        if not ite_received:
            # Wait up to 5s for auto-ite (player may have just arrived)
            logger.info(f"  [GATHER] Waiting for auto ite...")
            for _ in range(1000):  # 5s at 5ms intervals
                if self.game_state.interaction_check_ok:
                    ite_received = True
                    break
                await asyncio.sleep(0.005)

        if not ite_received:
            # No auto-itk from client — send our own itk as fallback
            logger.info(f"  [GATHER] No auto ite, sending itk")
            self.game_state.interaction_check_ok = False
            ok = await self.send_pre_interact()
            if not ok:
                self.game_state.is_busy = False
                self.game_state.busy_reason = None
                return False
            # Wait for ite
            for _ in range(600):  # 3s at 5ms
                if self.game_state.interaction_check_ok:
                    ite_received = True
                    break
                await asyncio.sleep(0.005)
            if not ite_received:
                logger.warn(f"  [GATHER] No ite after itk — aborting")
                self.game_state.is_busy = False
                self.game_state.busy_reason = None
                return False

        # Send itl IMMEDIATELY after ite
        logger.info(f"  [GATHER] ite received, sending itl")
        ok = await self.send_interact(resource.element_id)
        if not ok:
            self.game_state.is_busy = False
            self.game_state.busy_reason = None
            return False

        # Wait for kof (InteractiveUseEndedEvent)
        complete = await self.wait_gather_complete(timeout=15.0)
        if complete:
            logger.info(f"  [GATHER] Harvested resource {resource.element_id}!")
        return complete

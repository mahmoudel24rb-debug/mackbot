"""
Dofus 3 Navigation - High-level controller for movement and map changes.

Combines pathfinding + movement + map transitions into a simple API:
  - navigator.move_to(cell)
  - navigator.change_map(target_ref)
  - navigator.follow_route([ref1, ref2, ref3])

Uses the real Dofus 3 protocol: ipi/inq/ioh (not legacy hqn/hqu/hqc).
"""

import asyncio
from game.map_grid import (
    MapGrid, cell_distance, get_neighbors,
    MAP_CHANGE_RIGHT, MAP_CHANGE_BOTTOM, MAP_CHANGE_LEFT, MAP_CHANGE_TOP,
)
from game.pathfinding import find_path, find_closest_walkable
from game.movement import MovementController
from game.world_graph import WorldGraph
from utils import logger


class Navigator:
    """
    High-level navigation: move to cells, change maps, follow routes.
    """

    def __init__(self, game_state):
        self.game_state = game_state
        self.grid = MapGrid()
        self.movement = MovementController(game_state)
        self.world_graph = WorldGraph()

    def set_server_writer(self, writer):
        """Set the server writer for sending packets."""
        self.movement.set_server_writer(writer)

    @property
    def is_ready(self):
        """Check if navigation is possible (connected + have position + have pos_ref)."""
        return (
            self.movement.is_connected
            and self.game_state.character.cell_id is not None
            and self.game_state.pos_ref is not None
        )

    def _apply_observed_walkability(self):
        """Apply walkability data to the grid.
        Priority: KWW > cache > observed > IAL > full grid.
        Returns source name for logging."""
        from game.map_grid import CELL_COUNT
        map_id = self.game_state.map.map_id

        # 1. KWW (best: ~437 cells, map-specific)
        kww_walkable = getattr(self.game_state, '_walkable_cells', None)
        kww_map = getattr(self.game_state, '_walkable_cells_map_id', None)
        if kww_walkable and len(kww_walkable) >= 100 and kww_map == map_id:
            self.grid.walkable = bytearray(b'\x00' * CELL_COUNT)
            for cell_id in kww_walkable:
                self.grid.set_walkable(cell_id, True)
            logger.debug(f"  [NAV] Applied KWW: {len(kww_walkable)} cells")
            return "kww"

        # 2. Cache (KWW from a previous visit to this map)
        cache = getattr(self.game_state, '_walkable_cache', {})
        cached = cache.get(map_id)
        if cached and len(cached) >= 100:
            self.grid.walkable = bytearray(b'\x00' * CELL_COUNT)
            for cell_id in cached:
                self.grid.set_walkable(cell_id, True)
            logger.debug(f"  [NAV] Applied cached: {len(cached)} cells")
            return "cached"

        # 3. Observed (cells seen in real client MoveRequests/MoveEvents)
        observed = self.game_state._observed_walkable.get(map_id)
        n = len(observed) if observed else 0
        if n >= 20:
            self.grid.walkable = bytearray(b'\x00' * CELL_COUNT)
            for cell_id in observed:
                self.grid.set_walkable(cell_id, True)
            logger.debug(f"  [NAV] Applied {n} observed cells")
            return "observed"

        # 4. IAL (all cells mentioned in IAL are probably walkable)
        ial = self.game_state.ial_cell_properties
        if ial and len(ial) >= 50:
            self.grid.walkable = bytearray(b'\x00' * CELL_COUNT)
            for cell_id in ial:
                self.grid.set_walkable(cell_id, True)
            logger.debug(f"  [NAV] Applied IAL: {len(ial)} cells")
            return "ial"

        # 5. Fallback: full grid (UNSAFE)
        self.grid.walkable = bytearray(b'\x01' * CELL_COUNT)
        logger.warn(f"  [NAV] NO walkability data for map {map_id}! Using full grid (UNSAFE)")
        return "fallback"

    # Each MoveRequest sends exactly 2 keyCells (start + end).
    # Path is split so each segment is a straight line (same direction).
    MAX_SEGMENT_LENGTH = 2  # 2 cells = start + end = 2 keyCells

    async def move_to(self, target_cell, stop_adjacent=False):
        """
        Move character to a target cell on the current map.
        Long paths are split into short segments (max 6 cells each),
        matching real client behavior.

        Args:
            target_cell: destination cellId
            stop_adjacent: stop next to the target instead of on it

        Returns:
            True if movement completed successfully
        """
        current = self.game_state.character.cell_id
        if current is None:
            logger.error("[NAV] Unknown current position!")
            return False

        if current == target_cell:
            return True

        # Apply walkability to the grid before pathfinding
        source = self._apply_observed_walkability()
        logger.info(f"[NAV] Walkability source: {source}")

        # Find full path using A*
        full_path = find_path(current, target_cell, self.grid,
                              stop_adjacent=stop_adjacent)
        if not full_path:
            logger.error(f"[NAV] No path: {current} -> {target_cell}")
            return False

        logger.info(f"[NAV] Path: {len(full_path)} cells, {current} -> {full_path[-1]}")

        # Send full path in one MoveRequest (like real client: 4-8 keyCells).
        # KWW walkability ensures paths are valid.
        ok = await self._send_segment(full_path)
        if not ok:
            return False

        self.game_state.character.cell_id = full_path[-1]
        logger.info(f"[NAV] Arrived at cell {full_path[-1]}")
        return True

    async def _send_segment(self, path):
        """Send a single short movement segment, wait for MoveEvent, then send MoveConfirm."""
        self.movement._move_refused = False
        success = await self.movement.move_to_cell(path)
        if not success:
            return False

        # Wait for server response (MoveEvent)
        move_time = len(path) * 0.3 + 0.5
        move_time = min(move_time, 5.0)
        logger.debug(f"  [NAV] Waiting {move_time:.1f}s for segment ({len(path)} cells)...")

        elapsed = 0.0
        while elapsed < move_time:
            await asyncio.sleep(0.2)
            elapsed += 0.2
            if self.movement._move_refused:
                logger.error(f"[NAV] Movement REFUSED by server — aborting")
                return False

        # Update position to segment end
        self.game_state.character.cell_id = path[-1]

        # Do NOT send MoveConfirm here — the real Dofus client receives
        # the MoveEvent via the MITM proxy and sends MoveConfirm automatically.
        # Sending a second MoveConfirm corrupts server state.

        return True

    async def change_map(self, target_map_id, exit_cell=None):
        """
        Change to an adjacent map.

        Args:
            target_map_id: mapId of the destination map
            exit_cell: cell to walk to before triggering the map change (optional)

        Returns:
            True if map change completed
        """
        # Walk to exit cell first if provided
        if exit_cell is not None:
            current = self.game_state.character.cell_id
            if current != exit_cell:
                ok = await self.move_to(exit_cell)
                if not ok:
                    logger.error(f"[NAV] Can't reach exit cell {exit_cell}")
                    return False

        # Anti-detection: small delay before map change request
        import random
        await asyncio.sleep(random.uniform(0.3, 1.0))

        # Send MapChangeRequest with target mapId
        success = await self.movement.request_map_change(target_map_id)
        if not success:
            return False

        # Wait for new map data (MapComplementaryInformationEvent)
        changed = await self.movement.wait_map_change(timeout=15.0)
        if not changed:
            logger.warn("[NAV] Map change timed out")
            return False

        # Reset grid for the new map
        self.grid.clear()
        logger.info(f"[NAV] Arrived at map {self.game_state.map.map_id} "
                    f"({self.game_state.map.x}, {self.game_state.map.y})")
        return True

    async def move_and_change_map(self, edge_cell, target_map_id):
        """Walk to an edge cell then change map. Kept for compatibility."""
        return await self.change_map(target_map_id, exit_cell=edge_cell)

    async def travel_to(self, target_map_id):
        """
        Navigate from the current map to any target map using the WorldGraph BFS.

        Requires data/worldgraph.json to be populated.
        Each step: walk to exit cell -> send MapChangeRequest -> wait for arrival.

        Returns:
            True if arrived at target_map_id
        """
        current_map = self.game_state.map.map_id
        if current_map is None:
            logger.error("[NAV] travel_to: current map unknown")
            return False

        if current_map == target_map_id:
            return True

        if not self.world_graph.is_loaded():
            logger.error("[NAV] travel_to: worldgraph not loaded "
                         "(populate data/worldgraph.json first)")
            return False

        path = self.world_graph.find_path(current_map, target_map_id)
        if path is None:
            logger.error(f"[NAV] travel_to: no path {current_map} -> {target_map_id}")
            return False

        logger.info(f"[NAV] travel_to: {len(path)} map transitions to reach {target_map_id}")

        for i, step in enumerate(path):
            logger.info(f"[NAV] Step {i+1}/{len(path)}: "
                        f"{step['from_map']} -> {step['to_map']} via cell {step['cell_id']}")

            ok = await self.change_map(
                target_map_id=step["to_map"],
                exit_cell=step["cell_id"] if step["cell_id"] >= 0 else None,
            )
            if not ok:
                logger.error(f"[NAV] travel_to failed at step {i+1}")
                return False

            await asyncio.sleep(0.3)  # small delay between maps

        arrived = self.game_state.map.map_id == target_map_id
        if arrived:
            logger.info(f"[NAV] Arrived at target map {target_map_id}")
        else:
            logger.warn(f"[NAV] Expected map {target_map_id}, got {self.game_state.map.map_id}")
        return arrived

    async def follow_route(self, route):
        """
        Follow a route of map changes.

        Args:
            route: list of target_ref values for consecutive map changes

        Returns:
            number of successful map changes
        """
        count = 0
        for target_ref in route:
            logger.info(f"[NAV] Route step {count + 1}/{len(route)}: target=0x{target_ref:X}")
            success = await self.change_map(target_ref)
            if not success:
                logger.error(f"[NAV] Route failed at step {count + 1}")
                break
            count += 1
            # Small delay between map changes
            await asyncio.sleep(0.5)

        logger.info(f"[NAV] Route done: {count}/{len(route)} maps")
        return count

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
        if self.world_graph.is_loaded():
            logger.info(f"[NAV] WorldGraph loaded: {len(self.world_graph._adjacency)} maps")
        else:
            logger.warn("[NAV] WorldGraph not loaded — inter-map navigation unavailable")

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

        # Clear old occupied cells (entities may have moved)
        self.grid.occupied.clear()

        # Apply walkability to the grid before pathfinding
        source = self._apply_observed_walkability()
        logger.info(f"[NAV] Walkability source: {source}")

        # Mark NPC/monster cells as occupied (Jitsuri: "PNJ Occupied cells: ...")
        occupied = self._get_occupied_cells()
        for cell_id in occupied:
            self.grid.set_occupied(cell_id, True)
        if occupied:
            logger.info(f"[NAV] PNJ Occupied cells: {', '.join(str(c) for c in sorted(occupied))}")

        # Find path with retry on refusal
        max_attempts = 3
        for attempt in range(1, max_attempts + 1):
            full_path = find_path(current, target_cell, self.grid,
                                  stop_adjacent=stop_adjacent)
            if not full_path:
                logger.error(f"[NAV] No path: {current} -> {target_cell}")
                return False

            logger.info(f"[NAV] Path: {len(full_path)} cells, {current} -> {full_path[-1]}")

            ok = await self._send_segment(full_path)
            if ok:
                self.game_state.character.cell_id = full_path[-1]
                logger.info(f"[NAV] Arrived at cell {full_path[-1]}")
                from game.anti_detect import maybe_pause
                await maybe_pause()
                return True

            if attempt < max_attempts:
                logger.warn(f"[NAV] Movement refused (attempt {attempt}/{max_attempts}), "
                            f"retrying with alternative path...")
                current = self.game_state.character.cell_id
                if current is None:
                    return False
                self.grid.set_occupied(full_path[-1], True)
                await asyncio.sleep(0.5)

        logger.error(f"[NAV] Movement failed after {max_attempts} attempts")
        return True

    def _get_occupied_cells(self):
        """Get cells occupied by NPCs and monsters (exclude our character)."""
        occupied = set()
        char_id = self.game_state.character.id
        for eid, entity in self.game_state.entities.items():
            if char_id and eid == char_id:
                continue
            cell_id = entity.get("cell_id") if isinstance(entity, dict) else getattr(entity, "cell_id", None)
            if cell_id is not None and 0 <= cell_id < 560:
                occupied.add(cell_id)
        return occupied

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

    async def change_map(self, target_map_id, exit_cell=None, max_attempts=3):
        """
        Change to an adjacent map with retry logic.
        Tries alternative exit cells if the first attempt fails.
        """
        import random

        for attempt in range(1, max_attempts + 1):
            if exit_cell is not None:
                current = self.game_state.character.cell_id
                if current != exit_cell:
                    ok = await self.move_to(exit_cell)
                    if not ok:
                        logger.warn(f"[NAV] Can't reach exit cell {exit_cell} "
                                    f"(attempt {attempt})")
                        exit_cell = self._find_alternative_exit(target_map_id, exit_cell)
                        continue

            await asyncio.sleep(random.uniform(0.3, 1.0))

            # Flag intentional map change so ISU handler doesn't flag as involuntary
            self.game_state._expecting_map_change = True
            success = await self.movement.request_map_change(target_map_id)
            if not success:
                logger.warn(f"[NAV] MapChangeRequest failed (attempt {attempt})")
                continue

            changed = await self.movement.wait_map_change(timeout=10.0)
            if changed:
                self.grid.clear()
                logger.info(f"[NAV] Map change OK (attempt {attempt}): "
                            f"arrived at {self.game_state.map.map_id} "
                            f"({self.game_state.map.x}, {self.game_state.map.y})")
                return True

            logger.warn(f"[NAV] Map change timed out (attempt {attempt}/{max_attempts})")
            if attempt < max_attempts:
                exit_cell = self._find_alternative_exit(target_map_id, exit_cell)
                await asyncio.sleep(1.0)

        logger.error(f"[NAV] Map change to {target_map_id} failed after {max_attempts} attempts")
        return False

    def _find_alternative_exit(self, target_map_id, failed_cell):
        """Find an alternative exit cell, excluding the failed one."""
        if self.world_graph.is_loaded():
            edges = self.world_graph.get_neighbors(self.game_state.map.map_id)
            for edge in edges:
                if edge.get("to_map") == target_map_id and edge.get("cell_id") != failed_cell:
                    return edge["cell_id"]

        for cell_id, flags in self.grid.map_change_data.items():
            if cell_id != failed_cell and flags > 0:
                return cell_id

        return failed_cell

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

    async def travel_to_via_zaap(self, target_x, target_y, target_map_id):
        """
        Travel to a distant map using zaap (like Jitsuri's GoToNearestZaap).

        Flow:
        1. Enter havre-sac
        2. Interact with zaap
        3. Select destination (nearest zaap to target)
        4. Walk remaining distance via WorldGraph

        NOTE: Steps 1-3 require sniffing the zaap protocol first.
        Currently falls back to walking via WorldGraph.
        """
        from game.zaap_data import ZaapDatabase

        if not hasattr(self, '_zaap_db'):
            self._zaap_db = ZaapDatabase()

        nearest, zaap_dist = self._zaap_db.find_nearest(target_x, target_y)
        if nearest is None:
            logger.warn("[NAV] No zaaps known — walking instead")
            return await self.travel_to(target_map_id)

        logger.info(f"[NAV] Zaap plan: teleport to ({nearest.x},{nearest.y}) "
                    f"then walk {zaap_dist} maps to ({target_x},{target_y})")

        # TODO: Implement havre-sac entry + zaap interaction
        # Requires sniffing the protocol (see PLAN_PHASE4.md step 4.2)
        # For now, fall back to walking
        logger.warn("[NAV] Zaap teleport not yet implemented — walking via WorldGraph")
        return await self.travel_to(target_map_id)

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

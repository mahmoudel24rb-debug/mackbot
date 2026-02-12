"""
Dofus 3 Navigation - High-level controller for movement and map changes.

Combines pathfinding + movement + map transitions into a simple API:
  - navigator.move_to(cell)
  - navigator.change_map("right")
  - navigator.follow_route(["right", "right", "bottom"])
"""

import asyncio
from game.map_grid import (
    MapGrid, cell_distance, get_neighbors,
    MAP_CHANGE_RIGHT, MAP_CHANGE_BOTTOM, MAP_CHANGE_LEFT, MAP_CHANGE_TOP,
)
from game.pathfinding import find_path, find_closest_walkable
from game.movement import MovementController
from utils import logger


class Navigator:
    """
    High-level navigation: move to cells, change maps, follow routes.
    """

    def __init__(self, game_state):
        self.game_state = game_state
        self.grid = MapGrid()
        self.movement = MovementController(game_state)

    def set_server_writer(self, writer):
        """Set the server writer for sending packets."""
        self.movement.set_server_writer(writer)

    @property
    def is_ready(self):
        """Check if navigation is possible (connected + have position)."""
        return (
            self.movement.is_connected
            and self.game_state.character.cell_id is not None
        )

    async def move_to(self, target_cell, stop_adjacent=False):
        """
        Move character to a target cell on the current map.

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

        # Find path using A*
        path = find_path(current, target_cell, self.grid,
                         stop_adjacent=stop_adjacent)
        if not path:
            logger.error(f"[NAV] No path: {current} -> {target_cell}")
            return False

        logger.info(f"[NAV] Path: {len(path)} cells, {current} -> {path[-1]}")

        # Send movement request
        success = await self.movement.move_to_cell(path)
        if not success:
            return False

        # Wait for server to confirm
        arrived = await self.movement.wait_move_complete(timeout=10.0)
        if not arrived:
            logger.warn("[NAV] Movement timed out")
            return False

        # Confirm movement
        await self.movement.confirm_move()

        # Update position
        self.game_state.character.cell_id = path[-1]
        return True

    async def change_map(self, direction):
        """
        Change to an adjacent map.

        Args:
            direction: "right", "left", "top", "bottom"

        Returns:
            True if map change completed
        """
        dir_map = {
            "right": MAP_CHANGE_RIGHT,
            "bottom": MAP_CHANGE_BOTTOM,
            "left": MAP_CHANGE_LEFT,
            "top": MAP_CHANGE_TOP,
        }

        flag = dir_map.get(direction)
        if flag is None:
            logger.error(f"[NAV] Invalid direction: {direction}")
            return False

        # Find edge cells with this map change flag
        change_cells = self.grid.get_map_change_cells(flag)
        if not change_cells:
            logger.warn(f"[NAV] No map-change cells for '{direction}' "
                        f"(have {len(self.grid.map_change_data)} total)")
            return False

        # Pick the closest one
        current = self.game_state.character.cell_id
        if current is None:
            return False

        best = min(change_cells, key=lambda c: cell_distance(current, c))

        # Walk there first
        if current != best:
            success = await self.move_to(best)
            if not success:
                return False

        # Request map change
        success = await self.movement.request_map_change(flag)
        if not success:
            return False

        # Wait for new map data
        changed = await self.movement.wait_map_change(timeout=15.0)
        if not changed:
            logger.warn("[NAV] Map change timed out")
            return False

        # Reset grid for the new map
        self.grid.clear()
        logger.info(f"[NAV] Arrived at map ({self.game_state.map.x}, {self.game_state.map.y})")
        return True

    async def follow_route(self, route):
        """
        Follow a route of map directions.

        Args:
            route: list of directions, e.g. ["right", "right", "bottom"]

        Returns:
            number of successful map changes
        """
        count = 0
        for direction in route:
            logger.info(f"[NAV] Route step {count + 1}/{len(route)}: {direction}")
            success = await self.change_map(direction)
            if not success:
                logger.error(f"[NAV] Route failed at step {count + 1}")
                break
            count += 1
            # Small delay between map changes
            await asyncio.sleep(0.5)

        logger.info(f"[NAV] Route done: {count}/{len(route)} maps")
        return count

"""
Dofus 3 Auto-Farmer - Automatic gathering loop across multiple maps.

Usage via console:
  autofarm <x1>,<y1> <x2>,<y2> [resource_type]
  autofarm 3,-28 5,-23           -> farm everything in this rectangle
  autofarm 3,-28 5,-23 42        -> farm only type 42

Logic:
  1. On current map, list available resources
  2. For each resource (sorted by distance):
     a. Move adjacent (pathfinding)
     b. Send itk+itl (gather)
     c. Wait for kof (gather end)
  3. When all resources are done, change map
  4. Pick next map: snake pattern in rectangle
  5. Loop forever (stopfarm to stop)
"""

import asyncio
import random
from game.anti_detect import HumanDelay, maybe_pause, pick_resource
from utils import logger


class AutoFarmer:
    def __init__(self, game_state, navigator, gatherer):
        self.gs = game_state
        self.nav = navigator
        self.gatherer = gatherer
        self._running = False
        self._stats = {"gathered": 0, "maps_visited": 0, "failed": 0}

    def stop(self):
        self._running = False
        logger.info("[AUTOFARM] Stopping...")

    @property
    def is_running(self):
        return self._running

    async def run(self, x_range, y_range, resource_type=None):
        """
        Main farming loop.

        Args:
            x_range: tuple (x_min, x_max)
            y_range: tuple (y_min, y_max)
            resource_type: filter by type (None = all)
        """
        self._running = True
        self._stats = {"gathered": 0, "maps_visited": 0, "failed": 0}

        route = self._generate_snake_route(x_range, y_range)
        logger.info(f"[AUTOFARM] Route: {len(route)} maps in "
                    f"x=[{x_range[0]},{x_range[1]}] y=[{y_range[0]},{y_range[1]}]")

        route_idx = 0
        while self._running:
            # 1. Farm current map
            farmed = await self._farm_current_map(resource_type)
            self._stats["maps_visited"] += 1

            logger.info(f"[AUTOFARM] Map done: {farmed} gathered, "
                        f"total={self._stats['gathered']}, "
                        f"maps={self._stats['maps_visited']}")

            if not self._running:
                break

            # 2. Navigate to next map in route
            target_x, target_y = route[route_idx % len(route)]
            route_idx += 1

            ok = await self._navigate_to_coords(target_x, target_y)
            if not ok:
                logger.warn(f"[AUTOFARM] Can't reach ({target_x},{target_y}), skipping")
                continue

            # Anti-detection: human-like pause between maps
            delay = await HumanDelay.wait("map_change")
            logger.debug(f"[AUTOFARM] Waited {delay:.1f}s before farming next map")

        logger.info(f"[AUTOFARM] Stopped. Stats: {self._stats}")

    def _generate_snake_route(self, x_range, y_range):
        """Generate a snake pattern route through the coordinate rectangle."""
        x_min, x_max = min(x_range), max(x_range)
        y_min, y_max = min(y_range), max(y_range)

        route = []
        xs = list(range(x_min, x_max + 1))
        for y in range(y_min, y_max + 1):
            for x in xs:
                route.append((x, y))
            xs.reverse()

        return route

    async def _farm_current_map(self, resource_type=None):
        """Harvest all available resources on current map."""
        gathered = 0

        while self._running:
            avail = self.gs.map.get_available_resources(resource_type)
            if not avail:
                break

            # Pick resource (80% closest, 20% random — anti-detection)
            current = self.gs.character.cell_id
            target = pick_resource(avail, current)
            logger.info(f"[AUTOFARM] Target: elem={target.element_id} "
                        f"cell={target.cell_id} ({len(avail)} remaining)")

            # Move adjacent if needed
            if current != target.cell_id:
                move_ok = await self.nav.move_to(target.cell_id, stop_adjacent=True)
                if not move_ok:
                    logger.warn(f"[AUTOFARM] Can't reach resource at cell {target.cell_id}")
                    self._stats["failed"] += 1
                    target.enabled = False
                    continue
                await HumanDelay.wait("move")

            # Gather
            ok = await self.gatherer.gather_resource(target)
            if ok:
                gathered += 1
                self._stats["gathered"] += 1
            else:
                self._stats["failed"] += 1
                target.enabled = False

            await HumanDelay.wait("gather")

        return gathered

    async def _navigate_to_coords(self, target_x, target_y):
        """Navigate to a map at coordinates (x, y) one step at a time."""
        current_x = self.gs.map.x
        current_y = self.gs.map.y

        if current_x == target_x and current_y == target_y:
            return True

        logger.info(f"[AUTOFARM] Navigating ({current_x},{current_y}) -> ({target_x},{target_y})")

        max_steps = abs(target_x - current_x) + abs(target_y - current_y) + 5

        for _ in range(max_steps):
            if not self._running:
                return False

            cx, cy = self.gs.map.x, self.gs.map.y
            if cx == target_x and cy == target_y:
                return True

            # Pick direction: horizontal first, then vertical
            if cx != target_x:
                direction = "right" if target_x > cx else "left"
            else:
                direction = "bottom" if target_y > cy else "top"

            ok = await self._change_map_direction(direction)
            if not ok:
                # Try alternative directions
                for alt in ["top", "bottom", "left", "right"]:
                    if alt != direction and await self._change_map_direction(alt):
                        break
                else:
                    logger.error(f"[AUTOFARM] Stuck at ({cx},{cy})")
                    return False

            await asyncio.sleep(0.5)

        return self.gs.map.x == target_x and self.gs.map.y == target_y

    async def _change_map_direction(self, direction):
        """Change map in a given direction using WorldGraph or edge cells."""
        from game.map_grid import (MAP_CHANGE_RIGHT, MAP_CHANGE_BOTTOM,
                                    MAP_CHANGE_LEFT, MAP_CHANGE_TOP)

        flag_map = {
            "right": MAP_CHANGE_RIGHT,
            "bottom": MAP_CHANGE_BOTTOM,
            "left": MAP_CHANGE_LEFT,
            "top": MAP_CHANGE_TOP,
        }
        flag = flag_map.get(direction)
        if flag is None:
            return False

        # Find edge cells for this direction
        edge_cells = self.nav.grid.get_map_change_cells(flag)

        if not edge_cells:
            logger.debug(f"[AUTOFARM] No edge cells for direction {direction}")
            return False

        # Pick closest edge cell
        current = self.gs.character.cell_id
        if current is not None:
            from game.map_grid import cell_distance
            edge_cells.sort(key=lambda c: cell_distance(current, c))

        target_cell = edge_cells[0]

        # Move to edge cell
        if current != target_cell:
            ok = await self.nav.move_to(target_cell)
            if not ok:
                return False

        # Send map change request
        # The navigator.change_map needs a target_map_id
        # For now, use the edge cell to trigger the map change
        return await self.nav.change_map(None, exit_cell=target_cell)

"""
Dofus 3 Script Engine — Execute Jitsuri-compatible Lua farming scripts.

Script format (Jitsuri):
  ELEMENTS_TO_GATHER = {287, 288}   -- resource type IDs
  MAX_PODS = 90                      -- stop & bank at 90% pods

  function move()
    return {
      { map = "123456", path = "right", gather = true, fight = false },
      { map = "789012", path = "left",  gather = true, fight = false },
      ...
    }
  end

  function bank()
    return { { map = "192415750", npcBank = true } }
  end

  function phenix()  -- optional: respawn route
    return {}
  end

Route step fields:
  map    = current mapId (string) — must match game_state.map.map_id when arriving
  path   = direction ("left"/"right"/"top"/"bottom") OR next mapId (string)
  gather = true/false — whether to harvest resources here
  fight  = true/false — whether to fight monsters (not implemented)
  cell   = specific exit cell (int, optional) — forces a specific departure cell

Engine flow:
  1. Load script (.lua file)
  2. Parse move() to get route steps
  3. For each step:
     a. Wait until we are on step.map
     b. If gather: harvest all ELEMENTS_TO_GATHER resources on this map
     c. Navigate to step.path (direction or mapId)
  4. When pods >= MAX_PODS: run bank() route instead
  5. Loop when finished
"""

import asyncio
import os
from utils import logger

try:
    import lupa
    _LUPA_AVAILABLE = True
except ImportError:
    _LUPA_AVAILABLE = False
    logger.warn("[SCRIPT] lupa not installed — pip install lupa")


# Direction name -> internal direction constant
_DIR_MAP = {
    "right":  "right",
    "left":   "left",
    "top":    "top",
    "bottom": "bottom",
    "north":  "top",
    "south":  "bottom",
    "east":   "right",
    "west":   "left",
}


def _lua_table_to_list(t):
    """Convert a Lua sequential table to a Python list of dicts."""
    result = []
    if t is None:
        return result
    try:
        for _k, v in t.items():
            try:
                result.append(dict(v))
            except Exception:
                pass
    except Exception:
        pass
    return result


def _lua_table_to_set(t):
    """Convert a Lua array table to a Python set of values."""
    if t is None:
        return set()
    try:
        return set(t.values())
    except Exception:
        return set()


class RouteStep:
    """A single step in a farming route."""
    __slots__ = ("map_id", "path", "gather", "fight", "cell", "npc_bank")

    def __init__(self, d):
        self.map_id = str(d.get("map", "")) if d.get("map") is not None else None
        path = d.get("path")
        self.path = str(path) if path is not None else None
        self.gather = bool(d.get("gather", False))
        self.fight = bool(d.get("fight", False))
        self.cell = d.get("cell")     # optional exit cell override
        self.npc_bank = bool(d.get("npcBank", False))

    @property
    def is_direction(self):
        return self.path and self.path.lower() in _DIR_MAP

    @property
    def next_map_id(self):
        """If path is a mapId (numeric string), return it."""
        if self.path and self.path.lstrip("-").isdigit():
            return int(self.path)
        return None

    def __repr__(self):
        return (f"Step(map={self.map_id}, path={self.path!r}, "
                f"gather={self.gather})")


class ScriptEngine:
    """
    Loads and executes Jitsuri-compatible Lua farming scripts.
    Requires lupa (pip install lupa).

    Usage:
        engine = ScriptEngine(game_state, navigator, gatherer)
        await engine.load("scripts/farm.lua")
        await engine.run()  # loops until stopped
    """

    def __init__(self, game_state, navigator, gatherer):
        self.game_state = game_state
        self.navigator = navigator
        self.gatherer = gatherer

        self._lua = None
        self._script_path = None
        self._running = False
        self._stop_event = asyncio.Event()

        # Script-defined values (loaded from Lua globals)
        self.elements_to_gather = set()    # resource type IDs
        self.max_pods = 90
        self._route = []       # list of RouteStep (from move())
        self._bank_route = []  # list of RouteStep (from bank())

    # ------------------------------------------------------------------
    # Load
    # ------------------------------------------------------------------

    def load(self, script_path):
        """
        Load and execute a Lua script file.
        Parses ELEMENTS_TO_GATHER, MAX_PODS, move(), bank().

        Returns True on success.
        """
        if not _LUPA_AVAILABLE:
            logger.error("[SCRIPT] lupa not available — pip install lupa")
            return False

        if not os.path.exists(script_path):
            logger.error(f"[SCRIPT] File not found: {script_path}")
            return False

        self._script_path = script_path

        # Create Lua runtime and expose API
        self._lua = lupa.LuaRuntime(unpack_returned_tuples=True)
        self._expose_api()

        try:
            with open(script_path, "r", encoding="utf-8") as f:
                code = f.read()
            self._lua.execute(code)
        except Exception as e:
            logger.error(f"[SCRIPT] Error loading {script_path}: {e}")
            return False

        # Read globals
        try:
            et = self._lua.eval("ELEMENTS_TO_GATHER")
            self.elements_to_gather = _lua_table_to_set(et)
        except Exception:
            self.elements_to_gather = set()

        try:
            mp = self._lua.eval("MAX_PODS")
            if mp is not None:
                self.max_pods = int(mp)
        except Exception:
            pass

        # Parse routes
        try:
            move_result = self._lua.eval("move()")
            self._route = [RouteStep(d) for d in _lua_table_to_list(move_result)]
        except Exception as e:
            logger.error(f"[SCRIPT] Error calling move(): {e}")
            self._route = []

        try:
            bank_result = self._lua.eval("bank()")
            self._bank_route = [RouteStep(d) for d in _lua_table_to_list(bank_result)]
        except Exception:
            self._bank_route = []

        logger.info(f"[SCRIPT] Loaded: {os.path.basename(script_path)}")
        logger.info(f"  elements={self.elements_to_gather}, max_pods={self.max_pods}%")
        logger.info(f"  route: {len(self._route)} steps, bank: {len(self._bank_route)} steps")
        return True

    # ------------------------------------------------------------------
    # Run
    # ------------------------------------------------------------------

    async def run(self, loop=True):
        """
        Execute the farming route with Jitsuri-style dynamic matching:
        1. Find which route step matches our current map
        2. If no match -> navigate to first step's map
        3. Execute from matching step to end of route
        4. Loop when finished
        """
        if not self._route:
            logger.error("[SCRIPT] No route loaded")
            return

        self._running = True
        self._stop_event.clear()
        iteration = 0

        try:
            while self._running and not self._stop_event.is_set():
                iteration += 1
                logger.info(f"[SCRIPT] === Route iteration {iteration} ===")

                # Find the step matching our current map
                start_index = self._find_matching_step_index()

                if start_index is None:
                    # Not on any route map - navigate to first step
                    first_map = self._route[0].map_id
                    logger.warn(f"[SCRIPT] Not on any route map. "
                                f"Navigating to first step: {first_map}")
                    ok = await self._ensure_on_map(first_map)
                    if not ok:
                        logger.error("[SCRIPT] Failed to reach first route map. "
                                     "Retrying in 5s...")
                        await asyncio.sleep(5)
                        continue
                    start_index = 0

                # Execute steps from the matching point
                for i in range(start_index, len(self._route)):
                    if self._stop_event.is_set():
                        break

                    step = self._route[i]
                    logger.info(f"[SCRIPT] Step {i+1}/{len(self._route)}: {step}")

                    # Verify we're on the right map (navigate if needed)
                    ok = await self._ensure_on_map(step.map_id)
                    if not ok:
                        logger.warn(f"[SCRIPT] Cannot reach map {step.map_id}, "
                                    f"re-matching route...")
                        break  # Re-enter while loop to re-match

                    # Gather if requested
                    if step.gather and self.elements_to_gather:
                        await self._do_gather()

                    # Navigate to next map
                    if step.path:
                        await self._do_map_change(step)

                    # Small delay between steps
                    await asyncio.sleep(0.3)

                if not loop:
                    break

        except asyncio.CancelledError:
            pass
        finally:
            self._running = False
            logger.info("[SCRIPT] Route stopped")

    def stop(self):
        """Request the script to stop after the current step."""
        self._running = False
        self._stop_event.set()
        logger.info("[SCRIPT] Stop requested")

    # ------------------------------------------------------------------
    # Step execution
    # ------------------------------------------------------------------

    async def _ensure_on_map(self, expected_map_id):
        """
        Verify we're on the expected map. If not, navigate there via WorldGraph.

        Matches by mapId (string) or coordinates "x,y".
        """
        if not expected_map_id:
            return True

        current = str(self.game_state.map.map_id or "")
        expected = str(expected_map_id)

        # Direct mapId match
        if current == expected:
            return True

        # Match by coordinates (scripts can use "x,y" format)
        if "," in expected:
            try:
                parts = expected.split(",")
                ex, ey = int(parts[0].strip()), int(parts[1].strip())
                cx, cy = self.game_state.map.x, self.game_state.map.y
                if cx == ex and cy == ey:
                    return True
            except (ValueError, TypeError):
                pass
        else:
            # Expected is mapId — check if coords match
            if not hasattr(self, '_map_coords'):
                from game.map_coordinates import MapCoordinates
                self._map_coords = MapCoordinates()
            if self._map_coords.is_loaded():
                try:
                    expected_pos = self._map_coords.get_position(int(expected))
                    if expected_pos:
                        cx, cy = self.game_state.map.x, self.game_state.map.y
                        if cx == expected_pos[0] and cy == expected_pos[1]:
                            logger.info(f"[SCRIPT] Coords match ({cx},{cy}) "
                                        f"even though mapId differs")
                            return True
                except (ValueError, TypeError):
                    pass

        # Not on the right map — navigate there
        logger.info(f"[SCRIPT] Map mismatch: on {current}, need {expected}. Navigating...")

        target_map_id = self._resolve_target_map(expected)
        if target_map_id is None:
            logger.error(f"[SCRIPT] Cannot resolve target map: {expected}")
            return False

        ok = await self.navigator.travel_to(target_map_id)
        if ok:
            logger.info(f"[SCRIPT] Successfully navigated to map {expected}")
            return True
        else:
            logger.error(f"[SCRIPT] Failed to navigate to map {expected}")
            return False

    def _resolve_target_map(self, map_str):
        """Resolve a map string (mapId or 'x,y' coords) to a numeric mapId."""
        try:
            return int(map_str)
        except (ValueError, TypeError):
            pass

        if "," in str(map_str):
            try:
                parts = str(map_str).split(",")
                x, y = int(parts[0].strip()), int(parts[1].strip())
                if not hasattr(self, '_map_coords'):
                    from game.map_coordinates import MapCoordinates
                    self._map_coords = MapCoordinates()
                if self._map_coords.is_loaded():
                    candidates = self._map_coords.get_map_ids(x, y)
                    if candidates:
                        return candidates[0]
            except (ValueError, TypeError):
                pass

        return None

    def _find_matching_step_index(self):
        """
        Find the route step that matches our current map.
        Tries: mapId string match, coords "x,y" match, coord lookup.
        Returns step index or None.
        """
        current_map = str(self.game_state.map.map_id or "")
        current_x = self.game_state.map.x
        current_y = self.game_state.map.y
        current_coords = f"{current_x},{current_y}" if current_x is not None else None

        for i, step in enumerate(self._route):
            step_map = str(step.map_id) if step.map_id else ""

            # Match by mapId
            if step_map == current_map:
                logger.debug(f"[SCRIPT] Matched step {i} by mapId: {step_map}")
                return i

            # Match by coordinates
            if current_coords and step_map == current_coords:
                logger.debug(f"[SCRIPT] Matched step {i} by coords: {step_map}")
                return i

            # Match by coord lookup (step mapId -> coords -> compare)
            if step_map.lstrip("-").isdigit() and current_coords:
                if not hasattr(self, '_map_coords'):
                    from game.map_coordinates import MapCoordinates
                    self._map_coords = MapCoordinates()
                if self._map_coords.is_loaded():
                    try:
                        step_pos = self._map_coords.get_position(int(step_map))
                        if step_pos and f"{step_pos[0]},{step_pos[1]}" == current_coords:
                            logger.debug(f"[SCRIPT] Matched step {i} by coord lookup")
                            return i
                    except (ValueError, TypeError):
                        pass

        logger.debug(f"[SCRIPT] No matching step for map {current_map} ({current_coords})")
        return None

    async def _do_gather(self):
        """Harvest all available resources. Moves adjacent to each resource first."""
        gs = self.game_state
        if gs.is_busy:
            logger.info("[SCRIPT] Busy, skipping gather")
            return

        resources = gs.map.get_available_resources()
        if self.elements_to_gather:
            resources = [r for r in resources
                         if r.resource_type in self.elements_to_gather]

        if not resources:
            logger.info("[SCRIPT] No matching resources on this map")
            return

        logger.info(f"[SCRIPT] Gathering {len(resources)} resources...")
        for res in resources:
            if self._stop_event.is_set():
                break
            if not res.available or res.cell_id is None:
                continue

            # Check for fight interruption
            if gs.in_fight:
                logger.info("[SCRIPT] In fight, pausing gather...")
                while gs.in_fight and not self._stop_event.is_set():
                    await asyncio.sleep(1.0)
                logger.info("[SCRIPT] Fight ended, resuming gather")
                break  # Refresh resources after fight

            # Move adjacent to the resource
            current = gs.character.cell_id
            if current is not None:
                from game.map_grid import get_neighbors
                adj_cells = [n_id for n_id, _ in get_neighbors(res.cell_id)]
                if current not in adj_cells:
                    ok = await self.navigator.move_to(res.cell_id, stop_adjacent=True)
                    if not ok:
                        logger.warn(f"[SCRIPT] Can't reach resource {res.element_id} "
                                    f"at cell {res.cell_id}")
                        continue

            ok = await self.gatherer.gather_resource(res)
            if ok:
                logger.info(f"[SCRIPT]   Gathered {res.element_id}")
            else:
                logger.warn(f"[SCRIPT]   Failed to gather {res.element_id}")
            await asyncio.sleep(0.5)

    async def _do_map_change(self, step):
        """Navigate to the next map based on path (direction, mapId, or coords 'x,y')."""
        if not step.path:
            return

        # Check for involuntary map change during gather/move
        if self.game_state._involuntary_map_change:
            self.game_state._involuntary_map_change = False
            logger.warn(f"[SCRIPT] Involuntary map change from cell "
                        f"{self.game_state._involuntary_from_cell} "
                        f"-> now at {self.game_state.map.map_id}")
            return  # Don't try to change map again

        # Direction string (left/right/top/bottom)
        if step.is_direction:
            await self._change_by_direction(step.path.lower(), step.cell)
            return

        # Numeric mapId string
        next_map = step.next_map_id
        if next_map:
            exit_cell = int(step.cell) if step.cell else None
            ok = await self.navigator.change_map(next_map, exit_cell=exit_cell)
            if not ok:
                logger.error(f"[SCRIPT] Map change to {next_map} failed")
            return

        # Coords "x,y" format (e.g. "20,-28")
        if "," in step.path:
            target_map = self._resolve_target_map(step.path)
            if target_map:
                # Determine direction from current coords to target coords
                try:
                    parts = step.path.split(",")
                    tx, ty = int(parts[0].strip()), int(parts[1].strip())
                    cx, cy = self.game_state.map.x, self.game_state.map.y
                    if cx is not None and cy is not None:
                        dx, dy = tx - cx, ty - cy
                        if abs(dx) <= 1 and abs(dy) <= 1:
                            # Adjacent map — use direction-based change
                            direction = None
                            if dx == 1:  direction = "right"
                            elif dx == -1: direction = "left"
                            elif dy == 1:  direction = "bottom"
                            elif dy == -1: direction = "top"
                            if direction:
                                await self._change_by_direction(direction, step.cell)
                                return
                except (ValueError, TypeError):
                    pass
                # Fallback: navigate via WorldGraph
                ok = await self.navigator.travel_to(target_map)
                if not ok:
                    logger.error(f"[SCRIPT] Map change to {step.path} failed")
                return

        logger.warn(f"[SCRIPT] Unknown path: {step.path!r}")

    async def _change_by_direction(self, direction, exit_cell_override=None):
        """
        Change map in a direction (left/right/top/bottom).
        Finds the exit cell from map_change_data and sends MapChangeRequest.
        """
        from game.map_grid import MAP_CHANGE_RIGHT, MAP_CHANGE_BOTTOM, MAP_CHANGE_LEFT, MAP_CHANGE_TOP
        dir_to_flag = {
            "right":  MAP_CHANGE_RIGHT,
            "left":   MAP_CHANGE_LEFT,
            "top":    MAP_CHANGE_TOP,
            "bottom": MAP_CHANGE_BOTTOM,
        }
        flag = dir_to_flag.get(direction)
        if flag is None:
            logger.error(f"[SCRIPT] Unknown direction: {direction}")
            return

        if exit_cell_override:
            exit_cell = int(exit_cell_override)
        else:
            # Find exit cells with the matching direction flag
            grid = self.navigator.grid
            exit_cells = [cell_id for cell_id, mc in grid.map_change_data.items()
                          if mc & flag]
            if not exit_cells:
                logger.error(f"[SCRIPT] No exit cell found for direction {direction}")
                return
            exit_cell = exit_cells[0]

        # Walk to the exit cell
        current = self.game_state.character.cell_id
        if current != exit_cell:
            ok = await self.navigator.move_to(exit_cell)
            if not ok:
                logger.warn(f"[SCRIPT] Can't reach exit cell {exit_cell}")

        # Determine target mapId from map_change_data or game state
        # We don't know the target mapId from direction alone — use MapChangeRequest
        # with the exit cell which the server uses to determine the target
        target_map = self._get_map_in_direction(direction)
        if target_map:
            ok = await self.navigator.change_map(target_map, exit_cell=None)
            if not ok:
                logger.error(f"[SCRIPT] Map change {direction} failed")
        else:
            logger.error(f"[SCRIPT] Cannot determine target map for direction {direction}")

    def _get_map_in_direction(self, direction):
        """Get the adjacent mapId in a direction from the current map's coordinates."""
        gs = self.game_state
        if gs.map.x is None or gs.map.y is None:
            return None

        dx, dy = {
            "right":  (1, 0),
            "left":   (-1, 0),
            "top":    (0, -1),
            "bottom": (0, 1),
        }.get(direction, (0, 0))

        target_x = gs.map.x + dx
        target_y = gs.map.y + dy

        # Strategy 1: MapCoordinates lookup (x,y → mapId)
        from game.map_coordinates import MapCoordinates
        if not hasattr(self, "_map_coords"):
            self._map_coords = MapCoordinates()
        if self._map_coords.is_loaded():
            candidates = self._map_coords.get_map_ids(target_x, target_y)
            if candidates:
                # If multiple maps at this position, prefer one reachable via WorldGraph
                wg = self.navigator.world_graph
                if wg.is_loaded() and len(candidates) > 1:
                    neighbors = {e["to_map"] for e in wg.get_neighbors(gs.map.map_id)}
                    for cid in candidates:
                        if cid in neighbors:
                            logger.debug(f"[SCRIPT] Direction '{direction}' → map {cid} "
                                         f"(coord+graph match)")
                            return cid
                logger.debug(f"[SCRIPT] Direction '{direction}' → map {candidates[0]} "
                             f"(coord {target_x},{target_y})")
                return candidates[0]

        # Strategy 2: WorldGraph neighbors filtered by direction hint
        wg = self.navigator.world_graph
        if wg.is_loaded():
            from game.map_grid import MAP_CHANGE_RIGHT, MAP_CHANGE_BOTTOM, MAP_CHANGE_LEFT, MAP_CHANGE_TOP
            dir_flag = {
                "right": MAP_CHANGE_RIGHT, "left": MAP_CHANGE_LEFT,
                "top": MAP_CHANGE_TOP, "bottom": MAP_CHANGE_BOTTOM,
            }.get(direction)
            grid = self.navigator.grid
            if dir_flag and hasattr(grid, "map_change_data"):
                exit_cells = [c for c, f in grid.map_change_data.items() if f & dir_flag]
                if exit_cells:
                    neighbors = wg.get_neighbors(gs.map.map_id)
                    for edge in neighbors:
                        if edge["cell_id"] in exit_cells:
                            logger.debug(f"[SCRIPT] Direction '{direction}' → map {edge['to_map']} "
                                         f"(graph exit cell match)")
                            return edge["to_map"]

        logger.warn(f"[SCRIPT] Direction '{direction}': target map unknown "
                    f"(no match at {target_x},{target_y})")
        return None

    # ------------------------------------------------------------------
    # Lua API exposed to scripts
    # ------------------------------------------------------------------

    def _expose_api(self):
        """Expose Python functions to the Lua runtime."""
        lua = self._lua
        gs = self.game_state

        def get_map_id():
            mid = gs.map.map_id
            return str(mid) if mid is not None else ""

        def get_cell_id():
            return gs.character.cell_id or 0

        def get_current_pos():
            x = gs.map.x
            y = gs.map.y
            if x is not None and y is not None:
                return f"{x},{y}"
            return ""

        def get_level():
            return gs.character.level or 1

        def get_pods():
            # Pods not directly tracked yet — return 0
            return 0

        def get_max_pods():
            return self.max_pods

        def print_message(msg, color="white"):
            logger.info(f"[LUA] {msg}")

        def is_inventory_full():
            return False

        def get_item_quantity(gid):
            return 0

        # Register in Lua globals
        lua.globals().getMapId = get_map_id
        lua.globals().getCellId = get_cell_id
        lua.globals().getCurrentPos = get_current_pos
        lua.globals().getLevel = get_level
        lua.globals().getPods = get_pods
        lua.globals().getMaxPods = get_max_pods
        lua.globals().printMessage = print_message
        lua.globals().isInventoryFull = is_inventory_full
        lua.globals().getItemQuantity = get_item_quantity

    # ------------------------------------------------------------------
    # Properties
    # ------------------------------------------------------------------

    @property
    def is_running(self):
        return self._running

    @property
    def route_length(self):
        return len(self._route)

    def __repr__(self):
        loaded = os.path.basename(self._script_path) if self._script_path else "none"
        return f"ScriptEngine(script={loaded}, steps={len(self._route)}, running={self._running})"

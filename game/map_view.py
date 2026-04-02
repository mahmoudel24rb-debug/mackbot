"""
ASCII map viewer for debug.
Shows the grid in terminal with resources, mobs, and player position.

Usage via console:
  mapview (mv)    -> display current map
"""

from game.map_grid import CELL_COUNT, cell_to_point, point_to_cell


def render_map(game_state, navigator=None):
    """
    Render ASCII map of current state.
    Returns list of strings (lines to print).
    """
    lines = []

    # Collect info
    player_cell = game_state.character.cell_id
    resources = {r.cell_id: r for r in game_state.map.resources if r.cell_id is not None}
    entities = {}
    for eid, e in game_state.entities.items():
        cell = e.get("cell_id") if isinstance(e, dict) else getattr(e, "cell_id", None)
        if cell is not None:
            entities[cell] = e

    # Walkability
    walkable = set()
    if navigator:
        for c in range(CELL_COUNT):
            if navigator.grid.is_walkable(c):
                walkable.add(c)

    # Find grid bounds in MapPoint coordinates
    all_points = []
    for c in range(CELL_COUNT):
        pt = cell_to_point(c)
        if pt:
            all_points.append(pt)

    if not all_points:
        return ["[MapView] No grid data"]

    x_min = min(p[0] for p in all_points)
    x_max = max(p[0] for p in all_points)
    y_min = min(p[1] for p in all_points)
    y_max = max(p[1] for p in all_points)

    map_id = game_state.map.map_id
    map_x = game_state.map.x
    map_y = game_state.map.y
    lines.append(f"=== MAP VIEW === map={map_id} ({map_x},{map_y}) cell={player_cell} ===")

    # Render row by row
    for y in range(y_min, y_max + 1):
        row = ""
        for x in range(x_min, x_max + 1):
            cell_id = point_to_cell(x, y)
            if cell_id < 0:
                row += " "
                continue

            if cell_id == player_cell:
                row += "P"
            elif cell_id in resources:
                r = resources[cell_id]
                row += "R" if r.available else "x"
            elif cell_id in entities:
                row += "M"
            elif cell_id in walkable:
                row += "."
            else:
                row += "#"

        lines.append(row)

    # Legend
    avail = sum(1 for r in resources.values() if r.available)
    total_r = len(resources)
    lines.append(f"P=player R=resource({avail}/{total_r}) M=mob .=walkable #=blocked")

    return lines

"""
Dofus 3 Pathfinding - A* algorithm on the isometric diamond grid.

Uses MapPoint coordinates for distance heuristic.
Supports diagonal movement (8 directions) and adjacent-stop mode.
"""

import heapq
from game.map_grid import (
    cell_to_point, get_neighbors, cell_distance,
    is_valid_cell, MapGrid,
)


def find_path(start, end, grid, allow_diagonal=True, stop_adjacent=False):
    """
    A* pathfinding from start cell to end cell.

    Args:
        start: starting cellId
        end: target cellId
        grid: MapGrid instance
        allow_diagonal: allow 8-direction movement (vs 4-direction)
        stop_adjacent: stop next to the target instead of on it

    Returns:
        list of cellIds from start to end (inclusive), or [] if no path
    """
    if not is_valid_cell(start) or not is_valid_cell(end):
        return []

    if start == end:
        return [start]

    # Determine valid target cells
    if stop_adjacent:
        targets = set()
        for neighbor_id, _ in get_neighbors(end, allow_diagonal):
            if grid.is_walkable(neighbor_id):
                targets.add(neighbor_id)
        if not targets:
            return []
    else:
        targets = {end}

    # A*
    open_set = []
    heapq.heappush(open_set, (0, start))

    came_from = {}
    g_score = {start: 0}

    closed = set()

    while open_set:
        _, current = heapq.heappop(open_set)

        if current in targets:
            path = [current]
            while current in came_from:
                current = came_from[current]
                path.append(current)
            path.reverse()
            return path

        if current in closed:
            continue
        closed.add(current)

        for neighbor_id, direction in get_neighbors(current, allow_diagonal):
            if neighbor_id in closed:
                continue

            # Allow walking onto target even if not marked walkable
            if neighbor_id not in targets and not grid.is_walkable(neighbor_id):
                continue

            # Diagonal moves cost sqrt(2), cardinal cost 1
            cost = 1.414 if direction % 2 == 1 else 1.0
            tentative_g = g_score[current] + cost

            if tentative_g < g_score.get(neighbor_id, float('inf')):
                came_from[neighbor_id] = current
                g_score[neighbor_id] = tentative_g
                f = tentative_g + cell_distance(neighbor_id, end)
                heapq.heappush(open_set, (f, neighbor_id))

    return []


def find_closest_walkable(cell_id, grid, max_radius=10):
    """
    Find the closest walkable cell to the given cell using BFS.
    Returns -1 if none found within radius.
    """
    if grid.is_walkable(cell_id):
        return cell_id

    visited = {cell_id}
    queue = [cell_id]

    for _ in range(max_radius * 8):
        if not queue:
            break
        current = queue.pop(0)

        for neighbor_id, _ in get_neighbors(current, True):
            if neighbor_id in visited:
                continue
            visited.add(neighbor_id)

            if grid.is_walkable(neighbor_id):
                return neighbor_id

            queue.append(neighbor_id)

    return -1

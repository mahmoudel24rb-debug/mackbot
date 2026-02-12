"""
Dofus 3 Map Grid - Cell coordinate system and walkability.

Dofus uses an isometric diamond grid:
  - 560 cells (14 columns x 40 rows)
  - CellId 0-559
  - MapPoint coordinates: converted from cellId using Ankama's formula
  - 8 movement directions

Cell coordinate conversion (from Jitsuri JsPathFinder.cs):
  num = cellId % 14 - cellId // 28
  x = num + 19
  y = num + cellId // 14
"""

MAP_WIDTH = 14
MAP_HEIGHT = 20
# Classic Dofus uses 560 (14x40), but Dofus 3 maps can be larger (cell 871 seen).
# We build lookup for a safe upper bound.
CELL_COUNT = 1200

# 8 directions in MapPoint coordinate space
# Index matches Dofus direction encoding (0-7)
DIRECTIONS = {
    0: (1, 0),    # East
    1: (1, -1),   # South-East
    2: (0, -1),   # South
    3: (-1, -1),  # South-West
    4: (-1, 0),   # West
    5: (-1, 1),   # North-West
    6: (0, 1),    # North
    7: (1, 1),    # North-East
}

DIRECTION_NAMES = {
    0: "E", 1: "SE", 2: "S", 3: "SW",
    4: "W", 5: "NW", 6: "N", 7: "NE",
}

DIRECTION_FROM_DELTA = {v: k for k, v in DIRECTIONS.items()}

# Map change cell flags (from Cell.MapChangeData)
MAP_CHANGE_RIGHT = 1
MAP_CHANGE_BOTTOM = 2
MAP_CHANGE_LEFT = 4
MAP_CHANGE_TOP = 8

# ---------------------------------------------------------------------------
# Cell <-> MapPoint lookup tables
# ---------------------------------------------------------------------------

_cell_to_point = {}
_point_to_cell = {}


def _build_lookup():
    """Build cell_id <-> (x, y) lookup tables for all 560 cells."""
    for cell_id in range(CELL_COUNT):
        num = cell_id % 14 - cell_id // 28
        x = num + 19
        y = num + cell_id // 14
        _cell_to_point[cell_id] = (x, y)
        _point_to_cell[(x, y)] = cell_id


_build_lookup()


def cell_to_point(cell_id):
    """Convert cellId (0-559) to MapPoint (x, y) coordinates."""
    return _cell_to_point.get(cell_id)


def point_to_cell(x, y):
    """Convert MapPoint (x, y) to cellId. Returns -1 if invalid."""
    return _point_to_cell.get((x, y), -1)


def is_valid_cell(cell_id):
    """Check if a cellId is valid (0-559)."""
    return 0 <= cell_id < CELL_COUNT


def get_neighbors(cell_id, allow_diagonal=True):
    """
    Get neighboring cells for a given cellId.

    Returns:
        list of (neighbor_cell_id, direction_index) tuples
    """
    point = cell_to_point(cell_id)
    if point is None:
        return []

    x, y = point
    neighbors = []

    for direction, (dx, dy) in DIRECTIONS.items():
        if not allow_diagonal and direction % 2 == 1:
            continue
        nx, ny = x + dx, y + dy
        neighbor_id = point_to_cell(nx, ny)
        if neighbor_id >= 0:
            neighbors.append((neighbor_id, direction))

    return neighbors


def get_direction(from_cell, to_cell):
    """Get direction from one cell to an adjacent cell. Returns -1 if not adjacent."""
    p1 = cell_to_point(from_cell)
    p2 = cell_to_point(to_cell)
    if p1 is None or p2 is None:
        return -1
    dx = p2[0] - p1[0]
    dy = p2[1] - p1[1]
    return DIRECTION_FROM_DELTA.get((dx, dy), -1)


def cell_distance(cell_a, cell_b):
    """Manhattan distance between two cells in MapPoint coordinates."""
    pa = cell_to_point(cell_a)
    pb = cell_to_point(cell_b)
    if pa is None or pb is None:
        return float('inf')
    return abs(pa[0] - pb[0]) + abs(pa[1] - pb[1])


def compress_path(path):
    """
    Compress a path of cell IDs into Dofus format.
    Each step: (direction << 12) | cellId
    Only keyframes where direction changes + final cell.
    """
    if len(path) < 2:
        return path

    compressed = []
    last_dir = -1

    for i in range(len(path) - 1):
        direction = get_direction(path[i], path[i + 1])
        if direction == -1:
            continue
        if direction != last_dir:
            compressed.append((direction << 12) | path[i])
            last_dir = direction

    # Always add the last cell with last direction
    if path and last_dir >= 0:
        compressed.append((last_dir << 12) | path[-1])

    return compressed


def decompress_path(compressed):
    """
    Decompress a Dofus compressed path into individual cell IDs.
    """
    if not compressed:
        return []

    path = []
    for i, entry in enumerate(compressed):
        cell_id = entry & 0xFFF
        direction = (entry >> 12) & 0xF

        path.append(cell_id)

        if i < len(compressed) - 1:
            next_cell = compressed[i + 1] & 0xFFF
            dx, dy = DIRECTIONS.get(direction, (0, 0))
            current = cell_id

            while True:
                pt = cell_to_point(current)
                if pt is None:
                    break
                nx, ny = pt[0] + dx, pt[1] + dy
                next_id = point_to_cell(nx, ny)
                if next_id < 0 or next_id == next_cell:
                    break
                current = next_id
                path.append(current)

    return path


# ---------------------------------------------------------------------------
# MapGrid - per-map walkability state
# ---------------------------------------------------------------------------

class MapGrid:
    """
    Represents the walkability grid for a single map.
    Tracks which cells are walkable and which are occupied.
    """

    def __init__(self):
        self.walkable = bytearray(b'\x01' * CELL_COUNT)
        self.occupied = set()
        self.map_change_data = {}  # cell_id -> flags

    def is_walkable(self, cell_id):
        """Check if a cell is walkable and not occupied."""
        if not is_valid_cell(cell_id):
            return False
        return self.walkable[cell_id] and cell_id not in self.occupied

    def set_walkable(self, cell_id, walkable):
        if is_valid_cell(cell_id):
            self.walkable[cell_id] = walkable

    def set_occupied(self, cell_id, occupied=True):
        if occupied:
            self.occupied.add(cell_id)
        else:
            self.occupied.discard(cell_id)

    def clear(self):
        """Reset for a new map."""
        self.walkable = bytearray(b'\x01' * CELL_COUNT)
        self.occupied.clear()
        self.map_change_data.clear()

    def get_map_change_cells(self, direction_flag=None):
        """
        Get cells that trigger map changes.

        MapChangeData flags: 1=right, 2=bottom, 4=left, 8=top
        """
        cells = []
        for cell_id, flags in self.map_change_data.items():
            if direction_flag is None or (flags & direction_flag):
                cells.append(cell_id)
        return cells

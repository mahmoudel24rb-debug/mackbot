"""
Map Coordinates — (x, y) ↔ mapId lookup from data/map_coordinates.json.

JSON format (extracted from Jitsuri.dll embedded resource):
  [{"map-ids": [mapId, ...], "position": {"x": int, "y": int}}, ...]

One (x, y) position can have multiple mapIds (different sub-areas/floors).

Usage:
    from game.map_coordinates import MapCoordinates
    mc = MapCoordinates()
    map_ids = mc.get_map_ids(x=-5, y=10)   # → [168430592, ...]
    x, y = mc.get_position(168430592)       # → (-5, 10)
"""

import json
import os
from collections import defaultdict


class MapCoordinates:
    """(x, y) ↔ mapId bidirectional lookup."""

    def __init__(self, filepath="data/map_coordinates.json"):
        self._filepath = filepath
        # (x, y) → list of mapIds
        self._pos_to_maps: dict[tuple, list[int]] = defaultdict(list)
        # mapId → (x, y)
        self._map_to_pos: dict[int, tuple] = {}
        self._loaded = False
        self.load()

    def load(self, filepath=None):
        path = filepath or self._filepath
        if not os.path.exists(path):
            return
        try:
            with open(path, "r", encoding="utf-8") as f:
                data = json.load(f)
        except (json.JSONDecodeError, OSError):
            return

        for entry in data:
            pos = entry.get("position", {})
            x = pos.get("x")
            y = pos.get("y")
            if x is None or y is None:
                continue
            key = (x, y)
            for map_id in entry.get("map-ids", []):
                self._pos_to_maps[key].append(map_id)
                self._map_to_pos[map_id] = key

        self._loaded = len(self._map_to_pos) > 0

    def get_map_ids(self, x: int, y: int) -> list[int]:
        """Return all mapIds at position (x, y)."""
        return list(self._pos_to_maps.get((x, y), []))

    def get_position(self, map_id: int) -> tuple[int, int] | None:
        """Return (x, y) for a given mapId, or None."""
        return self._map_to_pos.get(map_id)

    def get_adjacent_map(self, map_id: int, dx: int, dy: int) -> int | None:
        """
        Return the first mapId adjacent to map_id in direction (dx, dy).
        Returns None if no map is known at the adjacent coordinates.
        """
        pos = self._map_to_pos.get(map_id)
        if pos is None:
            return None
        target_x = pos[0] + dx
        target_y = pos[1] + dy
        candidates = self._pos_to_maps.get((target_x, target_y), [])
        return candidates[0] if candidates else None

    def is_loaded(self) -> bool:
        return self._loaded

    def __repr__(self):
        return (f"MapCoordinates(maps={len(self._map_to_pos)}, "
                f"positions={len(self._pos_to_maps)}, loaded={self._loaded})")

"""
Zaap database — known zaap locations for fast travel.

Loaded from data/zaaps.json or built dynamically by observing
zaap interactive elements in ISU messages.
"""

import json
import os
from collections import namedtuple
from utils import logger

ZaapInfo = namedtuple("ZaapInfo", ["map_id", "x", "y", "name"])


class ZaapDatabase:
    """Registry of known zaap locations."""

    HAVRESAC_MAP_ID = 162791424  # (0,0) — personal haven bag

    def __init__(self, filepath="data/zaaps.json"):
        self._zaaps = []
        self._filepath = filepath
        self.load()

    def load(self, filepath=None):
        path = filepath or self._filepath
        if not os.path.exists(path):
            return
        try:
            with open(path, "r", encoding="utf-8") as f:
                data = json.load(f)
            self._zaaps = [
                ZaapInfo(map_id=z["mapId"], x=z["x"], y=z["y"],
                         name=z.get("name", ""))
                for z in data
            ]
            if self._zaaps:
                logger.info(f"[ZAAP] Loaded {len(self._zaaps)} zaaps")
        except (json.JSONDecodeError, OSError, KeyError) as e:
            logger.debug(f"[ZAAP] Failed to load zaaps: {e}")

    def save(self):
        data = [{"mapId": z.map_id, "x": z.x, "y": z.y, "name": z.name}
                for z in self._zaaps]
        os.makedirs(os.path.dirname(self._filepath) or ".", exist_ok=True)
        with open(self._filepath, "w", encoding="utf-8") as f:
            json.dump(data, f, indent=2)

    def add_zaap(self, map_id, x, y, name=""):
        """Add a zaap discovered during play."""
        for z in self._zaaps:
            if z.map_id == map_id:
                return
        self._zaaps.append(ZaapInfo(map_id, x, y, name))
        self.save()
        logger.info(f"[ZAAP] Discovered new zaap at ({x},{y}) mapId={map_id}")

    def find_nearest(self, target_x, target_y):
        """Find the zaap closest to target coordinates.
        Returns (ZaapInfo, manhattan_distance) or (None, inf)."""
        best = None
        best_dist = float('inf')
        for z in self._zaaps:
            dist = abs(z.x - target_x) + abs(z.y - target_y)
            if dist < best_dist:
                best = z
                best_dist = dist
        return best, best_dist

    def should_use_zaap(self, current_x, current_y, target_x, target_y, threshold=6):
        """Decide if zaap is worth it vs walking.
        Zaap is worth it if: zaap_to_target + 2 (overhead) < walking distance."""
        walk_dist = abs(current_x - target_x) + abs(current_y - target_y)

        if walk_dist <= threshold:
            return False

        nearest, zaap_to_target = self.find_nearest(target_x, target_y)
        if nearest is None:
            return False

        return (zaap_to_target + 2) < walk_dist

    @property
    def count(self):
        return len(self._zaaps)

    def __repr__(self):
        return f"ZaapDatabase({self.count} zaaps)"

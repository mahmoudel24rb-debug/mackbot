"""
Dofus 3 WorldGraph — Navigation inter-maps via BFS.

Loads data/worldgraph.json (extracted from Jitsuri.dll embedded resource).
Provides path finding between any two mapIds.

JSON structure (Unity/Jitsuri embedded format):
{
  "m_vertices": { "m_keys": {"Array": [mapId, ...]}, "m_values": {"Array": [...]} },
  "m_edges":    { "m_keys": {"Array": [idx, ...]},   "m_values": {"Array": [
    { "m_values": { "Array": [
      { "m_from": {"m_mapId": 123, "m_zoneId": 1},
        "m_to":   {"m_mapId": 456, "m_zoneId": 1},
        "m_transitions": { "Array": [
          { "m_cellId": 512, "m_direction": 2, "m_type": 1,
            "m_criterion": "", "m_transitionMapId": 456 }
        ] } }
    ] } }
  ] } }
}

Also supports legacy format (Vertices/Edges/Transitions keys).

A "step" returned by find_path is:
  { "from_map": int, "to_map": int, "cell_id": int, "direction": int }
  cell_id = exit cell on from_map to walk to before map change
"""

import json
import os
from collections import defaultdict, deque


class WorldGraph:
    """
    Bidirectional map adjacency graph. Nodes = (mapId, zoneId).
    Loaded from data/worldgraph.json at startup.
    """

    def __init__(self, filepath="data/worldgraph.json"):
        self._filepath = filepath
        # adjacency: mapId -> list of { to_map, to_zone, cell_id, direction, type }
        self._adjacency = defaultdict(list)
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

        if not data:
            return

        # Detect format: Unity/Jitsuri embedded (m_edges) or legacy (Edges/edges)
        if "m_edges" in data:
            count = self._load_unity_format(data)
        else:
            count = self._load_legacy_format(data)

        self._loaded = count > 0

    def _load_unity_format(self, data):
        """
        Parse the Unity/Jitsuri embedded format:
        m_edges.m_values.Array -> edge groups -> m_values.Array -> transitions
        """
        count = 0

        edges_root = data.get("m_edges", {})
        edge_groups = edges_root.get("m_values", {}).get("Array", [])

        for group in edge_groups:
            inner = group.get("m_values", {}).get("Array", [])
            for edge in inner:
                from_node = edge.get("m_from", {})
                to_node = edge.get("m_to", {})
                from_map = from_node.get("m_mapId", 0)
                to_map = to_node.get("m_mapId", 0)
                to_zone = to_node.get("m_zoneId", 0)

                transitions = edge.get("m_transitions", {}).get("Array", [])
                for t in transitions:
                    cell_id = t.get("m_cellId", -1)
                    direction = t.get("m_direction", -1)
                    t_type = t.get("m_type", 0)
                    criterion = t.get("m_criterion", "")

                    # Skip transitions with complex criteria (level requirements, etc.)
                    if criterion and criterion not in ("", "null"):
                        continue

                    # Skip some known problematic transitions (from WorldGraphManager.cs)
                    if criterion in ("PO<23515,1", "PO<23514,1"):
                        continue

                    self._adjacency[from_map].append({
                        "to_map": to_map,
                        "to_zone": to_zone,
                        "cell_id": cell_id,
                        "direction": direction,
                        "type": t_type,
                    })
                    count += 1

        return count

    def _load_legacy_format(self, data):
        """
        Parse the legacy Jitsuri JSON format (Vertices/Edges keys, CamelCase).
        """
        count = 0

        edges_root = data.get("Edges", data.get("edges", {}))
        values = edges_root.get("Values", edges_root.get("values", []))

        for entry in values:
            transitions = entry.get("Transitions", entry.get("transitions", []))
            for t in transitions:
                from_node = t.get("From", t.get("from", {}))
                to_node = t.get("To", t.get("to", {}))
                from_map = from_node.get("MapId", from_node.get("mapId", 0))
                to_map = to_node.get("MapId", to_node.get("mapId", 0))
                to_zone = to_node.get("ZoneId", to_node.get("zoneId", 0))

                for trans in t.get("Transitions", t.get("transitions", [])):
                    cell_id = trans.get("CellId", trans.get("cellId", -1))
                    direction = trans.get("Direction", trans.get("direction", -1))
                    t_type = trans.get("Type", trans.get("type", 0))
                    criterion = trans.get("Criterion", trans.get("criterion", ""))

                    if criterion and criterion not in ("", "null"):
                        continue

                    self._adjacency[from_map].append({
                        "to_map": to_map,
                        "to_zone": to_zone,
                        "cell_id": cell_id,
                        "direction": direction,
                        "type": t_type,
                    })
                    count += 1

        return count

    def find_path(self, start_map_id, target_map_id, start_zone_id=-1):
        """
        BFS from start_map_id to target_map_id.

        Returns list of steps:
          [{ "from_map": int, "to_map": int, "cell_id": int, "direction": int }, ...]
        or None if no path found.
        """
        if not self._loaded:
            return None

        if start_map_id == target_map_id:
            return []

        # BFS: state = (mapId, zoneId)
        # Queue entries: (mapId, zoneId, path_so_far)
        start_zones = [start_zone_id] if start_zone_id >= 0 else [0]
        # Try all zones of the start map
        neighbors = self._adjacency.get(start_map_id, [])
        start_zone_set = {start_zone_id} if start_zone_id >= 0 else {0}
        # Actually use mapId only for BFS (zones add complexity, skip for now)

        visited = set()
        queue = deque()

        # Seed: all transitions from start_map
        for edge in self._adjacency.get(start_map_id, []):
            state = (edge["to_map"],)
            if state not in visited:
                step = {
                    "from_map": start_map_id,
                    "to_map": edge["to_map"],
                    "cell_id": edge["cell_id"],
                    "direction": edge["direction"],
                }
                queue.append((edge["to_map"], [step]))
                visited.add(state)

        while queue:
            current_map, path = queue.popleft()

            if current_map == target_map_id:
                return path

            if len(path) > 50:  # max path length
                continue

            for edge in self._adjacency.get(current_map, []):
                state = (edge["to_map"],)
                if state not in visited:
                    visited.add(state)
                    step = {
                        "from_map": current_map,
                        "to_map": edge["to_map"],
                        "cell_id": edge["cell_id"],
                        "direction": edge["direction"],
                    }
                    queue.append((edge["to_map"], path + [step]))

        return None

    def get_neighbors(self, map_id):
        """Return all maps directly reachable from map_id."""
        return list(self._adjacency.get(map_id, []))

    def is_loaded(self):
        return self._loaded

    def __repr__(self):
        return f"WorldGraph(maps={len(self._adjacency)}, loaded={self._loaded})"

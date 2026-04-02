"""
Dofus 3 Game State - Tracks the current state of the game session.

Updated by message handlers as packets flow through the proxy.
Provides access to navigation (pathfinding + movement) via self.navigator.
"""

from utils import logger
from game.dofus_message import get_type_name
from protocol.matching import Matching
import config


class Resource:
    """A harvestable resource on the map."""
    __slots__ = ("element_id", "cell_id", "skill_id", "skill_uid",
                 "resource_type", "status", "enabled")

    def __init__(self, element_id, cell_id=None, skill_id=None, skill_uid=None,
                 resource_type=None, status=0, enabled=True):
        self.element_id = element_id
        self.cell_id = cell_id
        self.skill_id = skill_id
        self.skill_uid = skill_uid
        self.resource_type = resource_type
        self.status = status          # 0 = available
        self.enabled = enabled

    @property
    def available(self):
        # Protocol: status=1 means available (verified from live traffic 2026-03-26)
        return self.status == 1 and self.enabled

    def __repr__(self):
        avail = "OK" if self.available else f"s{self.status}"
        return (f"Resource(elem={self.element_id}, cell={self.cell_id}, "
                f"type={self.resource_type}, skill={self.skill_uid}, {avail})")


class Character:
    """Current character info."""
    __slots__ = ("id", "name", "level", "breed", "cell_id", "direction", "map_id")

    def __init__(self):
        self.id = None
        self.name = None
        self.level = None
        self.breed = None
        self.cell_id = None
        self.direction = None
        self.map_id = None

    def __repr__(self):
        return f"Character({self.name}, lv{self.level}, map={self.map_id}, cell={self.cell_id})"


class Stats:
    """Character stats (HP, MP, AP, etc.)."""
    __slots__ = ("hp", "max_hp", "mp", "max_mp", "ap", "max_ap",
                 "energy", "max_energy", "kamas")

    def __init__(self):
        self.hp = None
        self.max_hp = None
        self.mp = None
        self.max_mp = None
        self.ap = None
        self.max_ap = None
        self.energy = None
        self.max_energy = None
        self.kamas = None


class MapInfo:
    """Current map information."""
    __slots__ = ("map_id", "x", "y", "actors", "interactive_elements",
                 "stated_elements", "resources")

    def __init__(self):
        self.map_id = None
        self.x = None
        self.y = None
        self.actors = []           # list of dicts
        self.interactive_elements = []
        self.stated_elements = []
        self.resources = []        # list of Resource

    def get_available_resources(self, resource_type=None):
        """Get resources that are available to harvest."""
        results = [r for r in self.resources if r.available]
        if resource_type is not None:
            results = [r for r in results if r.resource_type == resource_type]
        return results


class Entity:
    """An entity on the map (player, monster, NPC)."""
    __slots__ = ("id", "name", "cell_id", "level", "entity_type", "breed_id")

    def __init__(self, entity_id=None, name=None, cell_id=None):
        self.id = entity_id
        self.name = name
        self.cell_id = cell_id
        self.level = None
        self.entity_type = None  # "player", "monster", "npc"
        self.breed_id = None

    def __repr__(self):
        return f"Entity({self.name or self.id}, cell={self.cell_id})"


class GameState:
    """
    Central game state container.
    Updated by message handlers as packets are intercepted by the proxy.
    """

    def __init__(self):
        self.character = Character()
        self.stats = Stats()
        self.map = MapInfo()
        self.entities = {}        # entity_id -> Entity
        self.pos_ref = None       # position reference from iny (composite varint, constant per map)
        self.in_fight = False
        self.is_busy = False      # True when gathering, crafting, etc.
        self.busy_reason = None   # "gathering", "crafting", etc.
        self.connected = False
        self.navigator = None        # set after import to avoid circular deps
        self.gatherer = None         # set after import to avoid circular deps
        self._fighter_manager = None # lazy-created by message_handlers on first fight event
        self.spell_manager = None    # set after import to avoid circular deps
        self.matching = Matching(getattr(config, "MATCHING_FILE", "data/matching.json"))
        self._handlers = {}       # message_name -> handler function
        self._message_log = []    # recent messages for debugging
        self._connect_time = None # timestamp when character loaded (for init filtering)
        # Walkability learned by observing real client MoveRequests/MoveEvents
        self._observed_walkable = {}  # mapId -> set of walkable cellIds
        self._walkable_cache = {}     # mapId -> set of walkable cellIds (from KWW)
        # Gather sequence flags
        self.interaction_check_ok = False   # Set True when ite (InteractiveUseCheckResponse) arrives
        self.last_harvest_complete = False  # Set True when kof (InteractiveUseEndedEvent) arrives
        # IAL cell properties for walkability analysis
        self.ial_cell_properties = {}  # cellId -> [f1_values]
        # Load persisted walkable cache
        self._load_walkable_cache()

    _WALKABLE_CACHE_PATH = "data/walkable_cache.json"

    def _load_walkable_cache(self):
        """Load persisted walkable cache from disk."""
        import json, os
        if os.path.exists(self._WALKABLE_CACHE_PATH):
            try:
                with open(self._WALKABLE_CACHE_PATH, "r") as f:
                    data = json.load(f)
                self._walkable_cache = {int(k): set(v) for k, v in data.items()}
                logger.info(f"  Loaded walkable cache: {len(self._walkable_cache)} maps")
            except Exception as e:
                logger.debug(f"  Failed to load walkable cache: {e}")

    def save_walkable_cache(self):
        """Persist walkable cache to disk."""
        import json
        if not self._walkable_cache:
            return
        try:
            serializable = {str(k): sorted(v) for k, v in self._walkable_cache.items()}
            with open(self._WALKABLE_CACHE_PATH, "w") as f:
                json.dump(serializable, f)
        except Exception as e:
            logger.debug(f"  Failed to save walkable cache: {e}")

    def register_handler(self, message_name, handler):
        """
        Register a handler for a stable message name (e.g. "MapMovementEvent").
        Also accepts raw 3-letter codes as fallback for legacy callers.
        """
        self._handlers[message_name] = handler

    def process_message(self, type_code, data, direction, uid=None):
        """
        Process a decoded game message.

        Args:
            type_code: 3-letter type URL code (e.g. "jrl", "hxl")
            data: raw bytes of the inner message (after Any unwrap)
            direction: "c2s" or "s2c"
            uid: message UID if present
        """
        # Resolve to stable name via matching, fallback to dofus_message dict
        name = self.matching.get_name(type_code)
        if name == type_code:
            # Not in matching yet — try legacy dict
            name = get_type_name(type_code)

        # Log the message
        self._message_log.append((type_code, name, direction))
        if len(self._message_log) > 200:
            self._message_log = self._message_log[-100:]

        # Look up handler by stable name first, then by raw code (legacy fallback)
        handler = self._handlers.get(name) or self._handlers.get(type_code)
        if handler:
            try:
                handler(self, data, direction, uid)
            except Exception as e:
                logger.error(f"Handler error for {name} ({type_code}): {e}")

    def log_state(self):
        """Print current game state summary."""
        lines = []
        lines.append(f"  Character: {self.character}")
        if self.stats.hp is not None:
            lines.append(f"  HP: {self.stats.hp}/{self.stats.max_hp}  MP: {self.stats.mp}/{self.stats.max_mp}")
        if self.map.map_id is not None:
            lines.append(f"  Map: {self.map.map_id} ({self.map.x}, {self.map.y})")
        if self.entities:
            lines.append(f"  Entities: {len(self.entities)} on map")
        if self.in_fight:
            lines.append(f"  IN FIGHT")
        return "\n".join(lines)

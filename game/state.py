"""
Dofus 3 Game State - Tracks the current state of the game session.

Updated by message handlers as packets flow through the proxy.
Provides access to navigation (pathfinding + movement) via self.navigator.
"""

from utils import logger
from game.dofus_message import get_type_name


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
                 "stated_elements")

    def __init__(self):
        self.map_id = None
        self.x = None
        self.y = None
        self.actors = []           # list of dicts
        self.interactive_elements = []
        self.stated_elements = []


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
        self.in_fight = False
        self.connected = False
        self.navigator = None     # set after import to avoid circular deps
        self._handlers = {}       # type_code -> handler function
        self._message_log = []    # recent messages for debugging

    def register_handler(self, type_code, handler):
        """Register a handler function for a specific message type code."""
        self._handlers[type_code] = handler

    def process_message(self, type_code, data, direction, uid=None):
        """
        Process a decoded game message.

        Args:
            type_code: 3-letter type URL code (e.g. "jrl", "hxl")
            data: raw bytes of the inner message (after Any unwrap)
            direction: "c2s" or "s2c"
            uid: message UID if present
        """
        name = get_type_name(type_code)

        # Log the message
        self._message_log.append((type_code, name, direction))
        if len(self._message_log) > 200:
            self._message_log = self._message_log[-100:]

        # Call handler if registered
        handler = self._handlers.get(type_code)
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

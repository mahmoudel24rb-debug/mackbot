"""
Dofus 3 Combat System — Fighter state tracking and AI.

Tracks fight state from protobuf events:
  FightJoinEvent          → fight started, get placement options
  FightStartingEvent      → fight officially starts
  GameFightSynchronizeEvent → full fighter list sync
  GameFightShowFighterEvent → single fighter added
  FightTurnStartEvent     → our turn begins (act!)
  FightTurnEndEvent       → turn ended (server confirms)
  GameActionFightEvent    → action happened (damage, move, death, etc.)
  FightEndEvent           → fight over

C2S sends:
  FightTurnReadyRequest   → signal ready to start/end turn
  GameActionFightCastRequest → cast a spell
  GameActionFightMoveRequest → move in fight
"""

import asyncio
from utils import logger


# ---------------------------------------------------------------------------
# Characteristic keywords (matches Jitsuri's CharacteristicKeyword enum)
# ---------------------------------------------------------------------------

class Characteristic:
    """Common characteristic IDs used in fight."""
    ACTION_POINTS  = 1    # AP (PA)
    MOVEMENT_POINTS = 2   # MP (PM)
    HEALTH_POINTS  = 3    # HP
    SHIELD         = 4
    PERMANENT_DAMAGE = 5
    INITIATIVE     = 13
    RANGE          = 14
    STRENGTH       = 15
    INTELLIGENCE   = 16
    CHANCE         = 17
    AGILITY        = 18
    WISDOM         = 19
    VITALITY       = 20


# ---------------------------------------------------------------------------
# Fight characteristics dict
# ---------------------------------------------------------------------------

class FightCharacteristics(dict):
    """
    Map of characteristic_id -> current_value.
    Populated from GameFightMinimalStats or CharacterCharacteristics protobuf.
    """

    def get_ap(self):
        return self.get(Characteristic.ACTION_POINTS, 0)

    def get_mp(self):
        return self.get(Characteristic.MOVEMENT_POINTS, 0)

    def get_hp(self):
        return self.get(Characteristic.HEALTH_POINTS, 0)

    def __repr__(self):
        ap = self.get_ap()
        mp = self.get_mp()
        hp = self.get_hp()
        return f"Stats(ap={ap}, mp={mp}, hp={hp})"


# ---------------------------------------------------------------------------
# Actor (combatant)
# ---------------------------------------------------------------------------

class ActorFighter:
    """
    Represents a single combatant in a fight.
    Populated from GameFightShowFighterEvent / GameFightSynchronizeEvent.
    """
    __slots__ = (
        "actor_id", "gid", "cell_id", "team_id",
        "is_alive", "is_summon", "characteristics", "name",
    )

    def __init__(self, actor_id, cell_id=0, team_id=0):
        self.actor_id = actor_id
        self.gid = None
        self.cell_id = cell_id
        self.team_id = team_id
        self.is_alive = True
        self.is_summon = False
        self.characteristics = FightCharacteristics()
        self.name = None

    @property
    def is_enemy(self):
        return self.team_id != 0  # team 0 = player's team

    def __repr__(self):
        alive = "alive" if self.is_alive else "dead"
        return (f"Actor(id={self.actor_id}, cell={self.cell_id}, "
                f"team={self.team_id}, {alive}, {self.characteristics})")


# ---------------------------------------------------------------------------
# Fight state
# ---------------------------------------------------------------------------

class FightState:
    """
    Full state of the current fight.
    Updated by FighterManager as events arrive.
    """

    def __init__(self):
        self.fight_id = None
        self.fighters = {}          # actor_id -> ActorFighter
        self.our_actor_id = None    # set from CharacterSelectedEvent
        self.current_turn_actor = None
        self.turn_count = 0
        self.placement_cells = []   # available cells to place on
        self.is_our_turn = False
        self._turn_started = asyncio.Event()

    def clear(self):
        self.fight_id = None
        self.fighters.clear()
        self.current_turn_actor = None
        self.turn_count = 0
        self.placement_cells = []
        self.is_our_turn = False
        self._turn_started.clear()

    def get_our_fighter(self):
        if self.our_actor_id:
            return self.fighters.get(self.our_actor_id)
        return None

    def get_enemies(self):
        our = self.get_our_fighter()
        if our is None:
            return [f for f in self.fighters.values() if f.team_id != 0]
        return [f for f in self.fighters.values()
                if f.team_id != our.team_id and f.is_alive]

    def get_allies(self):
        our = self.get_our_fighter()
        if our is None:
            return []
        return [f for f in self.fighters.values()
                if f.team_id == our.team_id and f.actor_id != our.actor_id]

    def on_turn_start(self, actor_id):
        self.current_turn_actor = actor_id
        self.is_our_turn = (actor_id == self.our_actor_id)
        if self.is_our_turn:
            self._turn_started.set()

    def on_turn_end(self, actor_id):
        if actor_id == self.our_actor_id:
            self.is_our_turn = False
            self._turn_started.clear()

    async def wait_our_turn(self, timeout=60.0):
        """Wait until it's our turn. Returns True if it's our turn."""
        try:
            await asyncio.wait_for(self._turn_started.wait(), timeout)
            return self.is_our_turn
        except asyncio.TimeoutError:
            return False


# ---------------------------------------------------------------------------
# FighterManager — parse events and update FightState
# ---------------------------------------------------------------------------

class FighterManager:
    """
    Parses combat protobuf messages and keeps FightState up to date.
    Called by message_handlers for each fight event.
    """

    def __init__(self, game_state):
        self.game_state = game_state
        self.fight = FightState()

    def on_fight_join(self, data_fields):
        """FightJoinEvent — we've joined a fight."""
        self.fight.clear()
        logger.info("[FIGHT] Joined fight")

    def on_fight_starting(self, data_fields):
        """FightStartingEvent — fight officially begins."""
        self.game_state.in_fight = True
        logger.info(f"[FIGHT] Fight starting — {len(self.fight.fighters)} fighters")

    def on_synchronize(self, fighters_data):
        """
        GameFightSynchronizeEvent — full list of fighters.
        fighters_data: list of raw fighter bytes (field 1 repeated)
        """
        self.fight.fighters.clear()
        for raw in fighters_data:
            fighter = self._parse_fighter(raw)
            if fighter:
                self.fight.fighters[fighter.actor_id] = fighter
        logger.info(f"[FIGHT] Sync: {len(self.fight.fighters)} fighters")
        for f in self.fight.fighters.values():
            logger.info(f"  {f}")

    def on_show_fighter(self, raw_bytes):
        """GameFightShowFighterEvent — single fighter added/updated."""
        fighter = self._parse_fighter(raw_bytes)
        if fighter:
            self.fight.fighters[fighter.actor_id] = fighter
            logger.info(f"[FIGHT] Fighter shown: {fighter}")

    def on_turn_start(self, actor_id, turn_id=None):
        """FightTurnStartEvent — actor_id's turn begins."""
        self.fight.on_turn_start(actor_id)
        if self.fight.is_our_turn:
            our = self.fight.get_our_fighter()
            logger.info(f"[FIGHT] OUR TURN (turn {self.fight.turn_count}) — {our}")
        else:
            logger.info(f"[FIGHT] Turn: actor={actor_id}")

    def on_turn_end(self, actor_id):
        """FightTurnEndEvent (or next turn start = implicit end)."""
        self.fight.on_turn_end(actor_id)

    def on_new_round(self, round_number):
        """GameFightNewRoundEvent — new round starts."""
        self.fight.turn_count = round_number
        logger.info(f"[FIGHT] Round {round_number}")

    def on_fight_action(self, action_fields):
        """
        GameActionFightEvent — something happened (damage, death, move...).
        Parse key events to update fighter state.
        """
        from proxy.packet_handler import decode_protobuf_fields, WIRE_VARINT, WIRE_LENGTH_DELIMITED
        # action_type in field 1 or 2
        for fn, wt, val in action_fields:
            if wt == WIRE_LENGTH_DELIMITED:
                # Try to parse as nested action
                inner = decode_protobuf_fields(val)
                self._process_action_inner(inner)

    def on_fight_end(self, result_fields):
        """FightEndEvent — fight over."""
        self.game_state.in_fight = False
        self.fight.clear()
        logger.info("[FIGHT] Fight ended")

    def on_placement_positions(self, cells):
        """FightPlacementPossiblePositionsEvent — available placement cells."""
        self.fight.placement_cells = cells
        logger.info(f"[FIGHT] Placement cells: {len(cells)} available")

    # --- Internal parsers ---

    def _parse_fighter(self, raw_bytes):
        """Parse a single fighter from raw protobuf bytes."""
        from proxy.packet_handler import decode_protobuf_fields, WIRE_VARINT, WIRE_LENGTH_DELIMITED
        try:
            fields = decode_protobuf_fields(raw_bytes)
        except Exception:
            return None

        actor_id = None
        cell_id = 0
        team_id = 0
        is_alive = True

        for fn, wt, val in fields:
            if wt == WIRE_VARINT:
                if fn == 1:    # actorId (int64)
                    actor_id = val
                elif fn == 2:  # cellId
                    cell_id = val
                elif fn == 3:  # teamId / disposition
                    team_id = val
                elif fn == 4:  # alive
                    is_alive = bool(val)
            elif wt == WIRE_LENGTH_DELIMITED:
                pass  # characteristics nested message — skip for now

        if actor_id is None:
            return None

        f = ActorFighter(actor_id, cell_id, team_id)
        f.is_alive = is_alive
        return f

    def _process_action_inner(self, fields):
        """Try to extract fighter state changes from a combat action."""
        from proxy.packet_handler import WIRE_VARINT
        # Common patterns: field 1 = actorId, field 2 = action data
        actor_id = None
        for fn, wt, val in fields:
            if wt == WIRE_VARINT and fn == 1:
                actor_id = val
                break

        if actor_id and actor_id in self.fight.fighters:
            fighter = self.fight.fighters[actor_id]
            for fn, wt, val in fields:
                if wt == WIRE_VARINT:
                    if fn == 3:  # new cell after move
                        fighter.cell_id = val
                    elif fn == 5 and val == 0:  # died
                        fighter.is_alive = False
                        logger.info(f"[FIGHT] Actor {actor_id} died")

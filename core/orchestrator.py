"""
Orchestrator — central controller that ties together:
  - FakeLauncher (IPC proxy to Ankama Launcher)
  - MITMProxy (game traffic interception)
  - GameState + Navigator + GatherController
  - ScriptEngine (Lua/Python farming scripts)
  - EventBus (pub/sub for UI ↔ bot communication)

Typical flow:
    orch = Orchestrator()
    await orch.start()          # Starts proxy + fake launcher
    await orch.run_script("scripts/farm.lua")
    await orch.stop()
"""

import asyncio
import os

from core.event_bus import EventBus
from game.state import GameState
from game.navigation import Navigator
from game.gathering import GatherController
from game.script_engine import ScriptEngine
from game.spell_manager import SpellManager
from utils import logger
import config


class Orchestrator:
    """Central bot controller."""

    def __init__(self, event_bus: EventBus = None):
        self.bus = event_bus or EventBus()

        # Core components
        self.game_state = GameState()
        self.navigator: Navigator | None = None
        self.gatherer: GatherController | None = None
        self.script_engine: ScriptEngine | None = None
        self.proxy = None            # MITMProxy instance (lazy)
        self.fake_launcher = None    # FakeLauncher instance (lazy)

        # Asyncio tasks
        self._proxy_task: asyncio.Task | None = None
        self._launcher_task: asyncio.Task | None = None
        self._script_task: asyncio.Task | None = None

        self._running = False

    # ------------------------------------------------------------------
    # Lifecycle
    # ------------------------------------------------------------------

    async def start(self):
        """Start the proxy and fake launcher. Returns when both are ready."""
        if self._running:
            return

        self._running = True
        self.bus.set_loop(asyncio.get_running_loop())

        # Wire up game components
        self._init_game_components()

        # Start fake launcher
        await self._start_fake_launcher()

        # Start MITM proxy
        await self._start_proxy()

        logger.info("[ORC] Orchestrator started")
        self.bus.emit("bot.started", {"status": "running"})

    async def stop(self):
        """Graceful shutdown."""
        if not self._running:
            return
        self._running = False

        if self._script_task and not self._script_task.done():
            self._script_task.cancel()
            try:
                await self._script_task
            except asyncio.CancelledError:
                pass

        if self._proxy_task and not self._proxy_task.done():
            self._proxy_task.cancel()
            try:
                await self._proxy_task
            except asyncio.CancelledError:
                pass

        if self._launcher_task and not self._launcher_task.done():
            self._launcher_task.cancel()
            try:
                await self._launcher_task
            except asyncio.CancelledError:
                pass

        logger.info("[ORC] Orchestrator stopped")
        self.bus.emit("bot.stopped", {})

    # ------------------------------------------------------------------
    # Script control
    # ------------------------------------------------------------------

    def load_script(self, path: str) -> bool:
        """Load a Lua script. Returns True on success."""
        if self.script_engine is None:
            self._init_game_components()
        return self.script_engine.load(path)

    async def run_script(self, path: str = None, loop: bool = True):
        """Load (optional) and run a farming script."""
        if path:
            ok = self.load_script(path)
            if not ok:
                return

        if self.script_engine is None or not self.script_engine.route_length:
            logger.error("[ORC] No script loaded")
            return

        self.bus.emit("bot.started", {"script": path or "current"})

        async def _run():
            try:
                await self.script_engine.run(loop=loop)
            except asyncio.CancelledError:
                pass
            finally:
                self.bus.emit("bot.stopped", {})

        self._script_task = asyncio.create_task(_run())
        await self._script_task

    def stop_script(self):
        """Stop a running script."""
        if self.script_engine:
            self.script_engine.stop()
        if self._script_task and not self._script_task.done():
            self._script_task.cancel()

    # ------------------------------------------------------------------
    # Status
    # ------------------------------------------------------------------

    def get_status(self) -> dict:
        """Return current bot status dict (for UI)."""
        gs = self.game_state
        return {
            "connected": gs.connected,
            "character": gs.character.name,
            "level": gs.character.level,
            "map_id": gs.map.map_id,
            "map_x": gs.map.x,
            "map_y": gs.map.y,
            "cell_id": gs.character.cell_id,
            "hp": gs.stats.hp,
            "max_hp": gs.stats.max_hp,
            "kamas": gs.stats.kamas,
            "in_fight": gs.in_fight,
            "is_busy": gs.is_busy,
            "busy_reason": gs.busy_reason,
            "script_running": (self.script_engine.is_running
                               if self.script_engine else False),
            "script_steps": (self.script_engine.route_length
                             if self.script_engine else 0),
            "resources_on_map": len(gs.map.get_available_resources()),
        }

    # ------------------------------------------------------------------
    # Internal setup
    # ------------------------------------------------------------------

    def _init_game_components(self):
        """Wire up Navigator, GatherController, ScriptEngine."""
        from game.message_handlers import register_all_handlers
        register_all_handlers(self.game_state)

        self.game_state.spell_manager = SpellManager()
        self._wire_event_bus()

        self.navigator = Navigator(self.game_state)
        self.gatherer = GatherController(self.game_state, self.navigator)
        self.script_engine = ScriptEngine(self.game_state, self.navigator, self.gatherer)

        # Back-references
        self.game_state.navigator = self.navigator
        self.game_state.gatherer = self.gatherer

    def _wire_event_bus(self):
        """Hook GameState updates to emit EventBus events."""
        gs = self.game_state
        bus = self.bus
        orig_process = gs.process_message

        def patched_process(type_code, data, direction, uid=None):
            result = orig_process(type_code, data, direction, uid)
            # Emit key events to the bus
            name = gs.matching.get_name(type_code)
            if "MapComplementary" in name or "MapData" in name:
                bus.emit("map.changed", {
                    "map_id": gs.map.map_id,
                    "x": gs.map.x,
                    "y": gs.map.y,
                })
            elif "Harvested" in name or "InteractiveUseEnded" in name:
                bus.emit("gather.completed", {
                    "map_id": gs.map.map_id,
                })
            elif "FightJoin" in name or "FightStart" in name:
                bus.emit("fight.started", {})
            elif "CharacterSelected" in name or "AuthenticationTicket" in name:
                bus.emit("game.connected", {
                    "name": gs.character.name,
                })
            return result

        gs.process_message = patched_process

    async def _start_fake_launcher(self):
        """Start the FakeLauncher IPC proxy as a background task."""
        try:
            from launcher.fake_launcher import FakeLauncher
            self.fake_launcher = FakeLauncher(
                listen_port=config.FAKE_LAUNCHER_PORT,
                launcher_port=config.LAUNCHER_IPC_PORT,
            )

            async def _run_launcher():
                try:
                    await self.fake_launcher.start()
                except asyncio.CancelledError:
                    pass
                except Exception as e:
                    logger.error(f"[ORC] FakeLauncher error: {e}")

            self._launcher_task = asyncio.create_task(_run_launcher())
            logger.info(f"[ORC] FakeLauncher started on port {config.FAKE_LAUNCHER_PORT}")
        except Exception as e:
            logger.warn(f"[ORC] FakeLauncher unavailable: {e}")

    async def _start_proxy(self):
        """Start the MITM proxy as a background task."""
        try:
            from proxy.mitm_proxy import MITMProxy
            self.proxy = MITMProxy(
                game_state=self.game_state,
                event_bus=self.bus,
                enable_console=False,
            )

            async def _run_proxy():
                try:
                    await self.proxy.start()
                except asyncio.CancelledError:
                    pass
                except Exception as e:
                    logger.error(f"[ORC] MITMProxy error: {e}")

            self._proxy_task = asyncio.create_task(_run_proxy())
            logger.info(f"[ORC] MITMProxy started on port {config.PROXY_PORT}")
        except Exception as e:
            logger.error(f"[ORC] MITMProxy failed to start: {e}")

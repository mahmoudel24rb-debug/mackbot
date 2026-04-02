"""
Orchestrator — central controller that ties together:
  - WinDivert packet redirect (replaces FakeLauncher)
  - MITMProxy (game traffic interception)
  - GameState + Navigator + GatherController
  - ScriptEngine (Lua/Python farming scripts)
  - EventBus (pub/sub for UI <-> bot communication)
  - Sniffer mode for matching code correction
"""

import asyncio
import os
import re
import subprocess

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
        self.proxy = None
        self.redirect = None  # WinDivert PacketRedirect

        # Connection state
        self._connected_emitted = False

        # Sniffer mode
        self._sniff_mode = False
        self._sniff_log = []  # list of (code, name, direction, size)

        # Asyncio tasks
        self._proxy_task: asyncio.Task | None = None
        self._script_task: asyncio.Task | None = None
        self._running = False

    # ------------------------------------------------------------------
    # Lifecycle
    # ------------------------------------------------------------------

    async def start(self):
        """Start WinDivert redirect + MITM proxy."""
        if self._running:
            return

        self._running = True
        self.bus.set_loop(asyncio.get_running_loop())

        # Wire up game components
        self._init_game_components()

        # Start WinDivert redirect
        await self._start_windivert()

        # Start MITM proxy (no console — UI handles commands)
        await self._start_proxy()

        logger.info("[ORC] Orchestrator started (WinDivert mode)")
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

        # Stop WinDivert
        if self.redirect:
            try:
                self.redirect.stop()
                logger.info("[ORC] WinDivert stopped")
            except Exception as e:
                logger.debug(f"[ORC] WinDivert stop error: {e}")

        logger.info("[ORC] Orchestrator stopped")
        self.bus.emit("bot.stopped", {})

    # ------------------------------------------------------------------
    # Script control
    # ------------------------------------------------------------------

    def load_script(self, path: str) -> bool:
        if self.script_engine is None:
            self._init_game_components()
        return self.script_engine.load(path)

    async def run_script(self, path: str = None, loop: bool = True):
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
        if self.script_engine:
            self.script_engine.stop()
        if self._script_task and not self._script_task.done():
            self._script_task.cancel()

    # ------------------------------------------------------------------
    # Sniffer mode
    # ------------------------------------------------------------------

    def start_sniffing(self):
        """Enable sniffer mode — logs all traffic for matching correction."""
        self._sniff_mode = True
        self._sniff_log.clear()
        logger.info("[ORC] Sniffer mode ENABLED — play manually to capture codes")

    def stop_sniffing(self):
        """Disable sniffer mode."""
        self._sniff_mode = False
        logger.info(f"[ORC] Sniffer mode DISABLED — captured {len(self._sniff_log)} packets")

    def save_matching(self):
        """Save current matching to disk."""
        self.game_state.matching.save()
        logger.info("[ORC] Matching saved to disk")

    def get_matching_codes(self):
        """Return current code->name dict."""
        return dict(self.game_state.matching._code_to_name)

    # ------------------------------------------------------------------
    # Gather command
    # ------------------------------------------------------------------

    async def gather_nearest(self, resource_type=None):
        """Gather the nearest available resource."""
        if not self.gatherer:
            return False
        gs = self.game_state
        avail = gs.map.get_available_resources(resource_type)
        if not avail:
            logger.error("[ORC] No available resources")
            return False

        current = gs.character.cell_id
        if current is not None:
            from game.map_grid import cell_distance
            avail.sort(key=lambda r: cell_distance(current, r.cell_id)
                       if r.cell_id is not None else 9999)

        target = avail[0]
        logger.info(f"[ORC] Gathering: {target}")
        return await self.gatherer.gather_resource(target)

    # ------------------------------------------------------------------
    # Status
    # ------------------------------------------------------------------

    def get_status(self) -> dict:
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
            "sniffing": self._sniff_mode,
        }

    # ------------------------------------------------------------------
    # Internal setup
    # ------------------------------------------------------------------

    def _init_game_components(self):
        from game.message_handlers import register_all_handlers
        register_all_handlers(self.game_state)

        self.game_state.spell_manager = SpellManager()
        self._wire_event_bus()

        self.navigator = Navigator(self.game_state)
        self.gatherer = GatherController(self.game_state)
        self.script_engine = ScriptEngine(self.game_state, self.navigator, self.gatherer)

        # Back-references
        self.game_state.navigator = self.navigator
        self.game_state.gatherer = self.gatherer

    def _wire_event_bus(self):
        gs = self.game_state
        bus = self.bus
        orig_process = gs.process_message

        def patched_process(type_code, data, direction, uid=None):
            result = orig_process(type_code, data, direction, uid)
            name = gs.matching.get_name(type_code)

            # Sniffer: emit traffic to UI
            if self._sniff_mode:
                size = len(data) if data else 0
                self._sniff_log.append((type_code, name, direction, size))
                bus.emit("sniffer.traffic", {
                    "code": type_code, "name": name,
                    "direction": direction, "size": size,
                })
                # Detect new matches from auto-matcher
                if name != type_code:
                    bus.emit("sniffer.match", {
                        "code": type_code, "name": name, "is_new": False,
                    })

            # Emit key game events
            if "MapComplementary" in name or "MapData" in name:
                bus.emit("map.changed", {
                    "map_id": gs.map.map_id, "x": gs.map.x, "y": gs.map.y,
                })
                # If connected but not yet reported, emit connected
                if gs.connected and not self._connected_emitted:
                    self._connected_emitted = True
                    bus.emit("game.connected", {
                        "name": gs.character.name or f"ID:{gs.character.id}",
                        "level": gs.character.level,
                    })
            elif "Harvested" in name or "InteractiveUseEnded" in name:
                # Filter out init burst: only count as gather if we've been
                # connected for more than 10 seconds
                import time
                if gs._connect_time and (time.time() - gs._connect_time) > 10:
                    bus.emit("gather.completed", {"map_id": gs.map.map_id})
            elif "FightJoin" in name or "FightStart" in name:
                bus.emit("fight.started", {})
            elif "CharacterSelected" in name:
                self._connected_emitted = True
                bus.emit("game.connected", {
                    "name": gs.character.name or f"ID:{gs.character.id}",
                    "level": gs.character.level,
                })
            return result

        gs.process_message = patched_process

    def _resolve_server(self):
        """Resolve game server IP via DNS."""
        try:
            result = subprocess.run(
                ["nslookup", config.SERVER_HOSTNAME],
                capture_output=True, text=True, timeout=10,
                encoding='utf-8', errors='replace',
            )
            for line in result.stdout.split("\n"):
                match = re.search(r"(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})", line)
                if match and not match.group(1).startswith("127."):
                    return match.group(1)
        except Exception:
            pass
        return None

    async def _start_windivert(self):
        """Start WinDivert packet redirect (like bot.py)."""
        try:
            from launcher.packet_redirect import PacketRedirect, PROXY_UPSTREAM_SRC_PORT

            server_ip = self._resolve_server()
            if not server_ip:
                logger.warn("[ORC] Cannot resolve game server — WinDivert skipped")
                self.bus.emit("bot.log", {"text": "DNS resolution failed", "level": "error"})
                return

            self._server_ip = server_ip
            logger.info(f"[ORC] Game server: {server_ip}")

            self.redirect = PacketRedirect(
                game_port=config.SERVER_PORT,
                proxy_port=config.PROXY_PORT,
            )
            self.redirect.start()
            logger.info("[ORC] WinDivert redirect ACTIVE")
            self.bus.emit("bot.log", {
                "text": f"WinDivert active — intercepting port {config.SERVER_PORT}",
                "level": "success",
            })
        except Exception as e:
            logger.error(f"[ORC] WinDivert failed: {e}")
            self.bus.emit("bot.log", {
                "text": f"WinDivert error: {e}. Run as Administrator!",
                "level": "error",
            })

    async def _start_proxy(self):
        """Start the MITM proxy as a background task."""
        try:
            from proxy.mitm_proxy import MITMProxy
            from launcher.packet_redirect import PROXY_UPSTREAM_SRC_PORT

            server_ip = getattr(self, '_server_ip', None) or self._resolve_server()

            self.proxy = MITMProxy(
                listen_host="127.0.0.1",
                listen_port=config.PROXY_PORT,
                server_host=server_ip or config.SERVER_HOSTNAME,
                server_port=config.SERVER_PORT,
                game_state=self.game_state,
                event_bus=self.bus,
                enable_console=False,
            )
            if server_ip:
                self.proxy.game_server_host = server_ip
                self.proxy.game_server_port = config.SERVER_PORT
            self.proxy.upstream_src_port = PROXY_UPSTREAM_SRC_PORT

            async def _run_proxy():
                try:
                    await self.proxy.start()
                except asyncio.CancelledError:
                    pass
                except Exception as e:
                    logger.error(f"[ORC] MITMProxy error: {e}")

            self._proxy_task = asyncio.create_task(_run_proxy())
            logger.info(f"[ORC] MITMProxy started on 127.0.0.1:{config.PROXY_PORT}")
        except Exception as e:
            logger.error(f"[ORC] MITMProxy failed: {e}")

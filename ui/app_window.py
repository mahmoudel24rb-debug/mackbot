"""
Main application window — CustomTkinter + asyncio bridge.

Tkinter has its own event loop and asyncio has its own.
Solution: asyncio runs in a background thread; communication via queue.Queue.

Architecture:
    MainThread   : Tkinter mainloop()
    BotThread    : asyncio.run(bot_main())
    Bridge       : queue.Queue for bot→UI messages
                   asyncio.Queue for UI→bot commands
"""
import asyncio
import queue
import threading
import traceback

import customtkinter as ctk

from ui import theme
from ui.sidebar import Sidebar
from ui.tabs.dashboard import DashboardTab
from ui.tabs.harvest import HarvestTab
from ui.tabs.map_view import MapViewTab
from ui.tabs.sniffer import SnifferTab
from ui.tabs.settings import SettingsTab
from utils import logger
import config


class AppWindow(ctk.CTk):
    """Main application window."""

    POLL_MS = 200   # UI poll interval in ms

    def __init__(self):
        super().__init__()

        theme.apply()
        self.title(config.UI_TITLE)
        self.geometry(f"{config.UI_WIDTH}x{config.UI_HEIGHT}")
        self.minsize(900, 600)
        self.configure(fg_color=theme.BG)

        # Message queues
        self._ui_queue: queue.Queue = queue.Queue()   # bot → UI messages
        self._cmd_queue: asyncio.Queue | None = None  # UI → bot commands (set in bot thread)

        # Bot orchestrator (lives in bot thread)
        self._orchestrator = None
        self._bot_thread: threading.Thread | None = None
        self._bot_loop: asyncio.AbstractEventLoop | None = None
        self._bot_running = False

        self._build()
        self._start_bot_thread()
        self._start_ui_poll()

    # ------------------------------------------------------------------
    # Build UI
    # ------------------------------------------------------------------

    def _build(self):
        self.grid_columnconfigure(1, weight=1)
        self.grid_rowconfigure(0, weight=1)

        # Init tabs dict before sidebar (sidebar calls _switch_tab during build)
        self.tabs = {}

        # Sidebar
        self.sidebar = Sidebar(self, on_tab_change=self._switch_tab)
        self.sidebar.grid(row=0, column=0, sticky="nsew")

        # Tab container
        self._tab_frame = ctk.CTkFrame(self, fg_color=theme.BG,
                                       corner_radius=0)
        self._tab_frame.grid(row=0, column=1, sticky="nsew")
        self._tab_frame.grid_columnconfigure(0, weight=1)
        self._tab_frame.grid_rowconfigure(0, weight=1)

        # Create tabs
        self.tabs = {
            "dashboard": DashboardTab(self._tab_frame),
            "harvest":   HarvestTab(
                self._tab_frame,
                on_start=self._on_start_bot,
                on_stop=self._on_stop_bot,
                on_script_select=self._on_script_select,
            ),
            "map_view":  MapViewTab(self._tab_frame),
            "sniffer":   SnifferTab(
                self._tab_frame,
                on_start_sniff=self._on_start_sniff,
                on_stop_sniff=self._on_stop_sniff,
                on_save_matching=self._on_save_matching,
            ),
            "settings":  SettingsTab(self._tab_frame, on_save=self._on_settings_save),
        }
        for tab in self.tabs.values():
            tab.grid(row=0, column=0, sticky="nsew")

        # Wire map click → move command
        self.tabs["map_view"].on_cell_click(self._on_map_click)

        self._switch_tab("dashboard")

    def _switch_tab(self, name: str):
        if not self.tabs:
            return  # Tabs not created yet (sidebar init)
        for tab in self.tabs.values():
            tab.grid_remove()
        if name in self.tabs:
            self.tabs[name].grid()

    # ------------------------------------------------------------------
    # Bot thread
    # ------------------------------------------------------------------

    def _start_bot_thread(self):
        """Start asyncio bot loop in a background thread."""
        self._bot_thread = threading.Thread(
            target=self._bot_thread_main, daemon=True, name="BotThread",
        )
        self._bot_thread.start()

    def _bot_thread_main(self):
        """Runs in background thread — owns the asyncio event loop."""
        self._bot_loop = asyncio.new_event_loop()
        asyncio.set_event_loop(self._bot_loop)
        self._cmd_queue = asyncio.Queue()

        try:
            self._bot_loop.run_until_complete(self._bot_main())
        except Exception:
            tb = traceback.format_exc()
            self._ui_queue.put(("log", f"[BOT CRASH]\n{tb}", "error"))
        finally:
            self._bot_loop.close()

    async def _bot_main(self):
        """Main async task — starts orchestrator and processes UI commands."""
        from core.orchestrator import Orchestrator
        from core.event_bus import EventBus

        bus = EventBus()
        bus.set_loop(asyncio.get_running_loop())

        # Forward bus events → UI queue
        @bus.on("game.connected")
        async def on_connected(data):
            self._ui_queue.put(("connected", data))

        @bus.on("map.changed")
        async def on_map(data):
            self._ui_queue.put(("map_changed", data))

        @bus.on("gather.completed")
        async def on_gather(data):
            self._ui_queue.put(("gather_completed", data))

        @bus.on("bot.stopped")
        async def on_stopped(_data):
            self._ui_queue.put(("bot_stopped", {}))

        @bus.on("bot.log")
        async def on_log(data):
            self._ui_queue.put(("log", data.get("text", ""), data.get("level", "info")))

        @bus.on("sniffer.traffic")
        async def on_sniff_traffic(data):
            self._ui_queue.put(("sniff_traffic", data))

        @bus.on("sniffer.match")
        async def on_sniff_match(data):
            self._ui_queue.put(("sniff_match", data))

        self._orchestrator = Orchestrator(event_bus=bus)
        self._bot_running = True

        # Start WinDivert + proxy
        await self._orchestrator.start()
        self._ui_queue.put(("log", "WinDivert + Proxy démarrés. Lancez Dofus...", "success"))

        # Command processing loop
        while True:
            try:
                cmd = await asyncio.wait_for(self._cmd_queue.get(), timeout=1.0)
            except asyncio.TimeoutError:
                # Periodic status push
                if self._orchestrator:
                    self._ui_queue.put(("status", self._orchestrator.get_status()))
                continue

            action = cmd.get("action")
            if action == "start_script":
                path = cmd.get("path")
                asyncio.create_task(self._orchestrator.run_script(path))
                self._ui_queue.put(("log", f"Script démarré: {path}", "success"))

            elif action == "stop_script":
                self._orchestrator.stop_script()
                self._ui_queue.put(("log", "Script arrêté.", "warning"))
                self._ui_queue.put(("bot_stopped", {}))

            elif action == "move_to":
                cell = cmd.get("cell_id")
                asyncio.create_task(self._move_to_cell(cell))

            elif action == "gather":
                asyncio.create_task(self._gather())

            elif action == "start_sniff":
                self._orchestrator.start_sniffing()
                # Send current matching to UI
                codes = self._orchestrator.get_matching_codes()
                self._ui_queue.put(("sniff_matching_refresh", codes))

            elif action == "stop_sniff":
                self._orchestrator.stop_sniffing()

            elif action == "save_matching":
                self._orchestrator.save_matching()
                self._ui_queue.put(("log", "Matching sauvegardé!", "success"))

            elif action == "quit":
                break

        await self._orchestrator.stop()

    async def _move_to_cell(self, cell_id: int):
        if self._orchestrator and self._orchestrator.navigator:
            ok = await self._orchestrator.navigator.move_to(cell_id)
            level = "success" if ok else "warning"
            self._ui_queue.put(("log", f"Déplacement vers {cell_id}: {'OK' if ok else 'échec'}", level))

    async def _gather(self):
        if self._orchestrator:
            ok = await self._orchestrator.gather_nearest()
            level = "success" if ok else "warning"
            self._ui_queue.put(("log", f"Gather: {'OK' if ok else 'échec'}", level))

    # ------------------------------------------------------------------
    # UI → bot commands
    # ------------------------------------------------------------------

    def _send_cmd(self, **kwargs):
        """Send a command to the bot thread (thread-safe)."""
        if self._bot_loop and self._cmd_queue:
            self._bot_loop.call_soon_threadsafe(
                self._cmd_queue.put_nowait, kwargs
            )

    def _on_start_bot(self, script_path: str):
        self._send_cmd(action="start_script", path=script_path)
        self.tabs["harvest"].append_log(
            f"Chargement script: {script_path}", "nav")

    def _on_stop_bot(self):
        self._send_cmd(action="stop_script")

    def _on_script_select(self, path: str):
        self.tabs["harvest"].append_log(f"Script sélectionné: {path}", "debug")

    def _on_map_click(self, cell_id: int):
        self._send_cmd(action="move_to", cell_id=cell_id)
        self.tabs["harvest"].append_log(f"Déplacement vers cellule {cell_id}", "nav")

    def _on_start_sniff(self):
        self._send_cmd(action="start_sniff")

    def _on_stop_sniff(self):
        self._send_cmd(action="stop_sniff")

    def _on_save_matching(self):
        self._send_cmd(action="save_matching")

    def _on_settings_save(self):
        self.tabs["harvest"].append_log("Paramètres sauvegardés.", "success")

    # ------------------------------------------------------------------
    # UI poll (bot → UI messages)
    # ------------------------------------------------------------------

    def _start_ui_poll(self):
        self._poll()

    def _poll(self):
        """Process all pending bot→UI messages."""
        try:
            while True:
                msg = self._ui_queue.get_nowait()
                self._handle_ui_msg(msg)
        except queue.Empty:
            pass
        self.after(self.POLL_MS, self._poll)

    def _handle_ui_msg(self, msg):
        kind = msg[0]

        if kind == "status":
            status = msg[1]
            self.tabs["dashboard"].update_status(status)
            gs = None
            if self._orchestrator:
                gs = self._orchestrator.game_state
            if gs and gs.map.map_id:
                self._refresh_map_view(gs)

        elif kind == "connected":
            data = msg[1]
            self.sidebar.set_connected(
                data.get("name"), data.get("level"), data.get("server"))
            self.tabs["harvest"].append_log(
                f"Connecte: {data.get('name')}", "success")

        elif kind == "map_changed":
            data = msg[1]
            self.tabs["harvest"].append_log(
                f"Map changée → {data.get('map_id')} ({data.get('x')},{data.get('y')})", "nav")

        elif kind == "gather_completed":
            self.tabs["harvest"].record_harvest()
            self.tabs["harvest"].append_log("Ressource récoltée.", "gather")

        elif kind == "bot_stopped":
            self.tabs["harvest"].set_running(False)

        elif kind == "log":
            _, text, level = msg
            self.tabs["harvest"].append_log(text, level)

        elif kind == "sniff_traffic":
            data = msg[1]
            self.tabs["sniffer"].add_traffic(
                data["code"], data["name"], data["direction"], data["size"])

        elif kind == "sniff_match":
            data = msg[1]
            self.tabs["sniffer"].add_new_match(
                data["code"], data["name"], data.get("is_new", False))

        elif kind == "sniff_matching_refresh":
            codes = msg[1]
            self.tabs["sniffer"].refresh_matching(codes)

    def _refresh_map_view(self, game_state):
        """Update the map canvas from current game state."""
        try:
            from game.map_grid import MapGrid
            grid = self._orchestrator.navigator.grid if self._orchestrator else None
            if grid is None:
                return
            walkable = set(grid.walkable_cells) if hasattr(grid, "walkable_cells") else set()
            map_change = set(grid.map_change_data.keys()) if hasattr(grid, "map_change_data") else set()
            resources = game_state.map.resources or []
            char_cell = game_state.character.cell_id
            self.tabs["map_view"].render_map(walkable, map_change, resources, char_cell)
        except Exception:
            pass

    # ------------------------------------------------------------------
    # Cleanup
    # ------------------------------------------------------------------

    def destroy(self):
        self._send_cmd(action="quit")
        super().destroy()

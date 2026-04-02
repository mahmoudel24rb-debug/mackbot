"""
Sidebar — modern left navigation with character info.
"""
import customtkinter as ctk
from ui import theme


class Sidebar(ctk.CTkFrame):
    """Vertical sidebar with character info and nav buttons."""

    TABS = [
        ("dashboard",  "\u25C9  Accueil"),
        ("harvest",    "\u26CF  Recolte"),
        ("map_view",   "\u2726  Carte"),
        ("sniffer",    "\u25C8  Sniffer"),
        ("settings",   "\u2699  Parametres"),
    ]

    def __init__(self, parent, on_tab_change, **kwargs):
        super().__init__(parent, width=220, corner_radius=0,
                         fg_color=theme.BG_PANEL, **kwargs)
        self.on_tab_change = on_tab_change
        self._active = "dashboard"
        self._buttons = {}
        self._indicators = {}
        self._build()

    def _build(self):
        self.grid_rowconfigure(20, weight=1)

        # --- Character section ---
        char_frame = ctk.CTkFrame(self, fg_color="transparent")
        char_frame.grid(row=0, column=0, padx=16, pady=(16, 4), sticky="ew")

        self._char_name = ctk.CTkLabel(
            char_frame, text="Deconnecte",
            font=theme.FONT_HEAD, text_color=theme.TEXT)
        self._char_name.grid(row=0, column=0, sticky="w")

        self._char_server = ctk.CTkLabel(
            char_frame, text="",
            font=theme.FONT_SMALL, text_color=theme.TEXT_DIM)
        self._char_server.grid(row=1, column=0, sticky="w")

        # Status dot + label
        status_frame = ctk.CTkFrame(self, fg_color="transparent")
        status_frame.grid(row=1, column=0, padx=16, pady=(0, 12), sticky="w")

        self._status_dot = ctk.CTkLabel(
            status_frame, text="\u25CF",
            font=("Segoe UI", 10), text_color=theme.TEXT_RED, width=14)
        self._status_dot.grid(row=0, column=0, padx=(0, 4))

        self._status_label = ctk.CTkLabel(
            status_frame, text="Deconnecte",
            font=theme.FONT_SMALL, text_color=theme.TEXT_DIM)
        self._status_label.grid(row=0, column=1)

        # Separator
        sep = ctk.CTkFrame(self, height=1, fg_color=theme.BORDER)
        sep.grid(row=2, column=0, sticky="ew", padx=12, pady=4)

        # --- Nav buttons ---
        for i, (tab_id, label) in enumerate(self.TABS):
            row_frame = ctk.CTkFrame(self, fg_color="transparent", height=38)
            row_frame.grid(row=3 + i, column=0, sticky="ew", padx=0, pady=1)
            row_frame.grid_columnconfigure(1, weight=1)

            # Left accent indicator (3px)
            indicator = ctk.CTkFrame(row_frame, width=3, height=32,
                                     fg_color="transparent", corner_radius=0)
            indicator.grid(row=0, column=0, padx=(0, 0), pady=3)
            self._indicators[tab_id] = indicator

            btn = ctk.CTkButton(
                row_frame, text=label, anchor="w",
                font=theme.FONT_BODY,
                fg_color="transparent",
                text_color=theme.TEXT_DIM,
                hover_color=theme.BG_CARD,
                height=34, corner_radius=4,
                command=lambda t=tab_id: self._switch(t),
            )
            btn.grid(row=0, column=1, padx=(4, 8), sticky="ew")
            self._buttons[tab_id] = btn

        # --- Bottom: version ---
        self._version = ctk.CTkLabel(
            self, text="v1.0.0",
            font=theme.FONT_SMALL, text_color=theme.TEXT_DIM)
        self._version.grid(row=21, column=0, padx=16, pady=(8, 12), sticky="sw")

        self._switch("dashboard")

    def _switch(self, tab_id):
        # Deactivate previous
        if self._active and self._active in self._buttons:
            self._buttons[self._active].configure(
                fg_color="transparent", text_color=theme.TEXT_DIM)
            self._indicators[self._active].configure(fg_color="transparent")

        # Activate new
        self._active = tab_id
        if tab_id in self._buttons:
            self._buttons[tab_id].configure(
                fg_color="transparent", text_color=theme.TEXT)
            self._indicators[tab_id].configure(fg_color=theme.ACCENT)

        self.on_tab_change(tab_id)

    def set_connected(self, name: str = None, level: int = None, server: str = None):
        if name:
            self._status_dot.configure(text_color=theme.TEXT_GREEN)
            self._status_label.configure(text="Connecte", text_color=theme.TEXT_GREEN)
            display = name
            if level:
                display += f"  Nv.{level}"
            self._char_name.configure(text=display)
            self._char_server.configure(text=server or "Orukam")
        else:
            self.set_disconnected()

    def set_disconnected(self):
        self._status_dot.configure(text_color=theme.TEXT_RED)
        self._status_label.configure(text="Deconnecte", text_color=theme.TEXT_DIM)
        self._char_name.configure(text="Deconnecte")
        self._char_server.configure(text="")

"""
Sidebar — left navigation panel with account info and tab buttons.
"""
import customtkinter as ctk
from ui import theme


class Sidebar(ctk.CTkFrame):
    """Vertical sidebar with nav buttons."""

    TABS = [
        ("dashboard",  "  Dashboard"),
        ("harvest",    "  Récolte"),
        ("map_view",   "  Carte"),
        ("sniffer",    "  Sniffer"),
        ("settings",   "  Paramètres"),
    ]

    def __init__(self, parent, on_tab_change, **kwargs):
        super().__init__(parent, width=180, corner_radius=0,
                         fg_color=theme.BG_PANEL, **kwargs)
        self.on_tab_change = on_tab_change
        self._active = "dashboard"
        self._buttons = {}

        self._build()

    def _build(self):
        self.grid_rowconfigure(20, weight=1)  # spacer pushes status to bottom

        # Title
        title = ctk.CTkLabel(self, text="DofusBot",
                             font=theme.FONT_TITLE,
                             text_color=theme.ACCENT)
        title.grid(row=0, column=0, padx=16, pady=(20, 4), sticky="w")

        # Status indicator
        self._status_dot = ctk.CTkLabel(self, text="● Déconnecté",
                                        font=theme.FONT_SMALL,
                                        text_color=theme.TEXT_RED)
        self._status_dot.grid(row=1, column=0, padx=16, pady=(0, 16), sticky="w")

        sep = ctk.CTkFrame(self, height=1, fg_color=theme.BG_CARD)
        sep.grid(row=2, column=0, sticky="ew", padx=10, pady=4)

        # Nav buttons
        for i, (tab_id, label) in enumerate(self.TABS):
            btn = ctk.CTkButton(
                self, text=label, anchor="w",
                font=theme.FONT_BODY,
                fg_color="transparent",
                text_color=theme.TEXT_DIM,
                hover_color=theme.BG_CARD,
                height=36,
                command=lambda t=tab_id: self._switch(t),
            )
            btn.grid(row=3 + i, column=0, padx=8, pady=2, sticky="ew")
            self._buttons[tab_id] = btn

        # Character info (bottom)
        self._char_label = ctk.CTkLabel(self, text="—",
                                        font=theme.FONT_SMALL,
                                        text_color=theme.TEXT_DIM,
                                        wraplength=160)
        self._char_label.grid(row=21, column=0, padx=12, pady=8, sticky="sw")

        self._switch("dashboard")

    def _switch(self, tab_id):
        if self._active:
            btn = self._buttons.get(self._active)
            if btn:
                btn.configure(fg_color="transparent",
                              text_color=theme.TEXT_DIM)
        self._active = tab_id
        btn = self._buttons.get(tab_id)
        if btn:
            btn.configure(fg_color=theme.ACCENT, text_color="white")
        self.on_tab_change(tab_id)

    def set_connected(self, name: str = None, level: int = None):
        if name:
            self._status_dot.configure(text="● Connecté",
                                       text_color=theme.TEXT_GREEN)
            info = f"{name}"
            if level:
                info += f"  lv{level}"
            self._char_label.configure(text=info)
        else:
            self._status_dot.configure(text="● Déconnecté",
                                       text_color=theme.TEXT_RED)
            self._char_label.configure(text="—")

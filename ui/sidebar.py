"""
Sidebar — Jitsuri-style left navigation with character info and status.
"""
import customtkinter as ctk
from ui import theme


class Sidebar(ctk.CTkFrame):
    """Vertical sidebar with character info, status indicator, and nav buttons."""

    # (tab_id, icon+label, enabled)
    TABS = [
        ("dashboard",  "◉  Accueil",      True),
        ("harvest",    "⛏  Récolte",       True),
        ("fight",      "⚔  Chasse",        False),
        ("market",     "🪙  HDV",           False),
        ("map_view",   "✦  Carte",         True),
        ("fm",         "🔧  FM",            False),
        ("sniffer",    "◈  Sniffer",       True),
        ("settings",   "⚙  Paramètres",    True),
    ]

    WIDTH = 220

    def __init__(self, parent, on_tab_change, **kwargs):
        super().__init__(parent, width=self.WIDTH, corner_radius=0,
                         fg_color=theme.BG_PANEL, **kwargs)
        self.on_tab_change = on_tab_change
        self._active = "dashboard"
        self._buttons = {}
        self._indicators = {}
        self._build()

    def _build(self):
        self.grid_rowconfigure(20, weight=1)
        self.grid_propagate(False)
        self.configure(width=self.WIDTH)

        # ── Logo / Title ─────────────────────────────────────────────
        logo_frame = ctk.CTkFrame(self, fg_color="transparent")
        logo_frame.grid(row=0, column=0, padx=16, pady=(16, 0), sticky="ew")

        ctk.CTkLabel(
            logo_frame, text="MackBot",
            font=("Segoe UI", 20, "bold"),
            text_color=theme.ACCENT,
        ).pack(side="left")

        # Version badge
        badge = ctk.CTkLabel(
            logo_frame, text=f" {getattr(__import__('config'), 'UI_VERSION', '1.0')} ",
            font=theme.FONT_BADGE,
            text_color=theme.ACCENT,
            fg_color=theme.ACCENT_BG,
            corner_radius=4,
        )
        badge.pack(side="left", padx=(8, 0))

        # ── Character section ────────────────────────────────────────
        char_frame = ctk.CTkFrame(self, fg_color="transparent")
        char_frame.grid(row=1, column=0, padx=16, pady=(16, 0), sticky="ew")
        char_frame.grid_columnconfigure(1, weight=1)

        # Avatar circle (initials)
        self._avatar = ctk.CTkLabel(
            char_frame, text="?",
            font=("Segoe UI", 13, "bold"),
            text_color=theme.BG,
            fg_color=theme.ACCENT_DIM,
            corner_radius=18,
            width=36, height=36,
        )
        self._avatar.grid(row=0, column=0, rowspan=2, padx=(0, 10))

        self._char_name = ctk.CTkLabel(
            char_frame, text="Déconnecté",
            font=theme.FONT_HEAD, text_color=theme.TEXT,
            anchor="w",
        )
        self._char_name.grid(row=0, column=1, sticky="w")

        self._char_server = ctk.CTkLabel(
            char_frame, text="",
            font=theme.FONT_SMALL, text_color=theme.TEXT_DIM,
            anchor="w",
        )
        self._char_server.grid(row=1, column=1, sticky="w")

        # ── Status row ───────────────────────────────────────────────
        status_frame = ctk.CTkFrame(self, fg_color="transparent")
        status_frame.grid(row=2, column=0, padx=16, pady=(6, 12), sticky="w")

        self._status_dot = ctk.CTkLabel(
            status_frame, text="●",
            font=("Segoe UI", 10), text_color=theme.TEXT_RED,
            width=14,
        )
        self._status_dot.grid(row=0, column=0, padx=(0, 4))

        self._status_label = ctk.CTkLabel(
            status_frame, text="Déconnecté",
            font=theme.FONT_SMALL, text_color=theme.TEXT_DIM,
        )
        self._status_label.grid(row=0, column=1)

        # ── Separator ────────────────────────────────────────────────
        sep = ctk.CTkFrame(self, height=1, fg_color=theme.BORDER)
        sep.grid(row=3, column=0, sticky="ew", padx=12, pady=2)

        # ── Nav buttons ──────────────────────────────────────────────
        for i, (tab_id, label, enabled) in enumerate(self.TABS):
            row_frame = ctk.CTkFrame(self, fg_color="transparent", height=38)
            row_frame.grid(row=4 + i, column=0, sticky="ew", padx=0, pady=1)
            row_frame.grid_columnconfigure(1, weight=1)

            # Left accent indicator (3px bar)
            indicator = ctk.CTkFrame(
                row_frame, width=3, height=30,
                fg_color="transparent", corner_radius=0,
            )
            indicator.grid(row=0, column=0, pady=4)
            self._indicators[tab_id] = indicator

            text_color = theme.TEXT_DIM if enabled else theme.BORDER
            btn = ctk.CTkButton(
                row_frame, text=label, anchor="w",
                font=theme.FONT_BODY,
                fg_color="transparent",
                text_color=text_color,
                hover_color=theme.BG_HOVER if enabled else "transparent",
                height=32, corner_radius=6,
                state="normal" if enabled else "disabled",
                command=(lambda t=tab_id: self._switch(t)) if enabled else None,
            )
            btn.grid(row=0, column=1, padx=(4, 8), sticky="ew")
            self._buttons[tab_id] = btn

        # ── Bottom: version info ─────────────────────────────────────
        self._version = ctk.CTkLabel(
            self, text="MackBot v1.0.0",
            font=theme.FONT_TINY, text_color=theme.BORDER,
        )
        self._version.grid(row=21, column=0, padx=16, pady=(8, 12), sticky="sw")

        # Activate default tab
        self._switch("dashboard")

    # ─── Tab switching ───────────────────────────────────────────────
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

    # ─── Public methods ──────────────────────────────────────────────
    def set_connected(self, name: str = None, level: int = None, server: str = None):
        """Update character info and status to connected."""
        if name:
            self._status_dot.configure(text_color=theme.TEXT_GREEN)
            self._status_label.configure(text="Connecté", text_color=theme.TEXT_GREEN)

            display = name
            if level:
                display += f"  Nv.{level}"
            self._char_name.configure(text=display)
            self._char_server.configure(text=server or "Orukam")

            # Avatar: first letter of name
            initials = name[0].upper() if name else "?"
            self._avatar.configure(text=initials)
        else:
            self.set_disconnected()

    def set_disconnected(self):
        """Reset to disconnected state."""
        self._status_dot.configure(text_color=theme.TEXT_RED)
        self._status_label.configure(text="Déconnecté", text_color=theme.TEXT_DIM)
        self._char_name.configure(text="Déconnecté")
        self._char_server.configure(text="")
        self._avatar.configure(text="?")

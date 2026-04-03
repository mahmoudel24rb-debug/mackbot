"""
Settings tab — proxy ports, paths, delays, UI.
Organized in labeled sections with modern dark theme.
"""
import customtkinter as ctk
import config
from ui import theme


class SettingsTab(ctk.CTkFrame):
    def __init__(self, parent, on_save=None, **kwargs):
        super().__init__(parent, fg_color=theme.BG, corner_radius=0, **kwargs)
        self.on_save = on_save
        self._entries = {}
        self._build()

    def _build(self):
        self.grid_columnconfigure(0, weight=1)

        ctk.CTkLabel(
            self, text="Paramètres",
            font=theme.FONT_TITLE, text_color=theme.TEXT,
        ).grid(row=0, column=0, padx=20, pady=(16, 12), sticky="w")

        # Scrollable content
        scroll = ctk.CTkScrollableFrame(
            self, fg_color="transparent",
        )
        scroll.grid(row=1, column=0, padx=12, pady=0, sticky="nsew")
        scroll.grid_columnconfigure(0, weight=1)
        self.grid_rowconfigure(1, weight=1)

        # ── Section: Proxy ───────────────────────────────────────────
        self._section(scroll, "Réseau / Proxy", [
            ("Proxy port",          "PROXY_PORT",
             str(config.PROXY_PORT)),
            ("Serveur hostname",    "SERVER_HOSTNAME",
             config.SERVER_HOSTNAME),
            ("Serveur port",        "SERVER_PORT",
             str(config.SERVER_PORT)),
            ("Fake Launcher port",  "FAKE_LAUNCHER_PORT",
             str(config.FAKE_LAUNCHER_PORT)),
        ], row=0)

        # ── Section: Paths ───────────────────────────────────────────
        self._section(scroll, "Dossiers", [
            ("Dossier scripts", "SCRIPTS_DIR",
             getattr(config, "SCRIPTS_DIR", "scripts")),
            ("Dossier routes",  "ROUTES_DIR",
             getattr(config, "ROUTES_DIR", "routes")),
        ], row=1)

        # ── Section: Anti-detection ──────────────────────────────────
        self._section(scroll, "Anti-détection", [
            ("Délai min (s)", "ACTION_DELAY_MIN",
             str(getattr(config, "ACTION_DELAY_MIN", 0.3))),
            ("Délai max (s)", "ACTION_DELAY_MAX",
             str(getattr(config, "ACTION_DELAY_MAX", 0.8))),
            ("Délai map change (s)", "MAP_CHANGE_DELAY",
             str(getattr(config, "MAP_CHANGE_DELAY", 1.2))),
        ], row=2)

        # ── Save button ─────────────────────────────────────────────
        btn_frame = ctk.CTkFrame(self, fg_color="transparent")
        btn_frame.grid(row=2, column=0, padx=14, pady=(8, 6), sticky="w")

        ctk.CTkButton(
            btn_frame, text="Enregistrer", font=theme.FONT_BODY,
            fg_color=theme.ACCENT_DIM, hover_color=theme.ACCENT_HOVER,
            text_color=theme.BG,
            width=140, height=34, corner_radius=8,
            command=self._save,
        ).pack(side="left")

        self._save_label = ctk.CTkLabel(
            btn_frame, text="",
            font=theme.FONT_SMALL, text_color=theme.TEXT_GREEN,
        )
        self._save_label.pack(side="left", padx=12)

        ctk.CTkLabel(
            self,
            text="Les modifications prennent effet au prochain démarrage du proxy.",
            font=theme.FONT_TINY, text_color=theme.TEXT_DIM,
        ).grid(row=3, column=0, padx=14, pady=(0, 12), sticky="w")

    # ─── Section builder ─────────────────────────────────────────────

    def _section(self, parent, title: str, fields: list, row: int):
        """Create a titled settings section with input fields."""
        card = ctk.CTkFrame(parent, fg_color=theme.BG_CARD, corner_radius=10)
        card.grid(row=row, column=0, padx=0, pady=(0, 8), sticky="ew")
        card.grid_columnconfigure(1, weight=1)

        # Section title
        ctk.CTkLabel(
            card, text=title,
            font=theme.FONT_HEAD, text_color=theme.ACCENT,
        ).grid(row=0, column=0, columnspan=2, padx=14, pady=(12, 6),
               sticky="w")

        for i, (label, key, default) in enumerate(fields, start=1):
            ctk.CTkLabel(
                card, text=label, font=theme.FONT_BODY,
                text_color=theme.TEXT, anchor="w",
            ).grid(row=i, column=0, padx=(14, 8), pady=5, sticky="w")

            entry = ctk.CTkEntry(
                card, font=theme.FONT_BODY, height=32,
                corner_radius=6,
                fg_color=theme.BG_INPUT,
                border_color=theme.BORDER,
                text_color=theme.TEXT,
            )
            entry.insert(0, default)
            entry.grid(row=i, column=1, padx=(0, 14), pady=5, sticky="ew")
            self._entries[key] = entry

        # Bottom padding
        spacer = ctk.CTkFrame(card, height=6, fg_color="transparent")
        spacer.grid(row=len(fields) + 1, column=0, columnspan=2)

    # ─── Save ────────────────────────────────────────────────────────

    def _save(self):
        """Write settings back to the config module."""
        for key, entry in self._entries.items():
            val = entry.get().strip()
            if val:
                try:
                    if key in ("PROXY_PORT", "SERVER_PORT",
                               "FAKE_LAUNCHER_PORT"):
                        setattr(config, key, int(val))
                    elif key in ("ACTION_DELAY_MIN", "ACTION_DELAY_MAX",
                                 "MAP_CHANGE_DELAY"):
                        setattr(config, key, float(val))
                    else:
                        setattr(config, key, val)
                except ValueError:
                    pass

        self._save_label.configure(text="✓ Sauvegardé")
        self.after(3000, lambda: self._save_label.configure(text=""))

        if self.on_save:
            self.on_save()

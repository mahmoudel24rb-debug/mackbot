"""
Settings tab — proxy ports, Dofus path, script directory, delays.
"""
import os
import customtkinter as ctk
import config


class SettingsTab(ctk.CTkFrame):
    def __init__(self, parent, on_save=None, **kwargs):
        from ui import theme
        self._theme = theme
        super().__init__(parent, fg_color=theme.BG, **kwargs)
        self.on_save = on_save
        self._entries = {}
        self._build()

    def _build(self):
        t = self._theme
        self.grid_columnconfigure(0, weight=1)

        ctk.CTkLabel(self, text="Paramètres", font=t.FONT_TITLE,
                     text_color=t.TEXT).grid(
            row=0, column=0, padx=20, pady=(16, 12), sticky="w")

        card = ctk.CTkFrame(self, fg_color=t.BG_CARD, corner_radius=8)
        card.grid(row=1, column=0, padx=12, pady=4, sticky="ew")
        card.grid_columnconfigure(1, weight=1)

        fields = [
            ("Proxy port",      "PROXY_PORT",         str(config.PROXY_PORT)),
            ("Serveur hostname","SERVER_HOSTNAME",     config.SERVER_HOSTNAME),
            ("Serveur port",    "SERVER_PORT",         str(config.SERVER_PORT)),
            ("Fake Launcher port","FAKE_LAUNCHER_PORT",str(config.FAKE_LAUNCHER_PORT)),
            ("Dossier scripts", "SCRIPTS_DIR",         getattr(config, "SCRIPTS_DIR", "scripts")),
            ("Dossier routes",  "ROUTES_DIR",          getattr(config, "ROUTES_DIR", "routes")),
            ("Délai min (s)",   "ACTION_DELAY_MIN",    str(getattr(config, "ACTION_DELAY_MIN", 0.3))),
            ("Délai max (s)",   "ACTION_DELAY_MAX",    str(getattr(config, "ACTION_DELAY_MAX", 0.8))),
        ]

        for i, (label, key, default) in enumerate(fields):
            ctk.CTkLabel(card, text=label, font=t.FONT_BODY,
                         text_color=t.TEXT, anchor="w").grid(
                row=i, column=0, padx=(14, 8), pady=5, sticky="w")
            entry = ctk.CTkEntry(card, font=t.FONT_BODY, height=32)
            entry.insert(0, default)
            entry.grid(row=i, column=1, padx=(0, 14), pady=5, sticky="ew")
            self._entries[key] = entry

        ctk.CTkButton(self, text="Enregistrer", font=t.FONT_BODY,
                      width=140, command=self._save).grid(
            row=2, column=0, padx=14, pady=16, sticky="w")

        ctk.CTkLabel(self, text="Les modifications prennent effet au prochain démarrage du proxy.",
                     font=t.FONT_SMALL, text_color=t.TEXT_DIM).grid(
            row=3, column=0, padx=14, sticky="w")

    def _save(self):
        for key, entry in self._entries.items():
            val = entry.get().strip()
            if val:
                try:
                    # Convert numeric fields
                    if key in ("PROXY_PORT", "SERVER_PORT", "FAKE_LAUNCHER_PORT"):
                        setattr(config, key, int(val))
                    elif key in ("ACTION_DELAY_MIN", "ACTION_DELAY_MAX"):
                        setattr(config, key, float(val))
                    else:
                        setattr(config, key, val)
                except ValueError:
                    pass
        if self.on_save:
            self.on_save()

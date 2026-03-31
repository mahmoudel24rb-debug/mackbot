"""
Dashboard tab — character info, map position, bot status at a glance.
"""
import customtkinter as ctk
from ui import theme


class DashboardTab(ctk.CTkFrame):
    def __init__(self, parent, **kwargs):
        super().__init__(parent, fg_color=theme.BG, **kwargs)
        self._build()

    def _build(self):
        self.grid_columnconfigure(0, weight=1)
        self.grid_columnconfigure(1, weight=1)
        self.grid_rowconfigure(2, weight=1)

        # Title
        ctk.CTkLabel(self, text="Dashboard", font=theme.FONT_TITLE,
                     text_color=theme.TEXT).grid(
            row=0, column=0, columnspan=2, padx=20, pady=(20, 12), sticky="w")

        # Left card — character
        self._char_card = self._card(row=1, col=0, title="Personnage")
        self._hp_bar = ctk.CTkProgressBar(self._char_card, width=200,
                                          progress_color=theme.TEXT_GREEN)
        self._hp_bar.set(0)
        self._hp_bar.pack(padx=12, pady=(0, 8), fill="x")

        self._char_labels = {}
        for key in ("Nom", "Niveau", "Race", "HP", "Kamas"):
            row = ctk.CTkFrame(self._char_card, fg_color="transparent")
            row.pack(fill="x", padx=12, pady=1)
            ctk.CTkLabel(row, text=key + ":", font=theme.FONT_SMALL,
                         text_color=theme.TEXT_DIM, width=70,
                         anchor="w").pack(side="left")
            lbl = ctk.CTkLabel(row, text="—", font=theme.FONT_SMALL,
                               text_color=theme.TEXT, anchor="w")
            lbl.pack(side="left", fill="x", expand=True)
            self._char_labels[key] = lbl

        # Right card — map / position
        self._map_card = self._card(row=1, col=1, title="Carte & Position")
        self._map_labels = {}
        for key in ("Map ID", "Coordonnées", "Cellule", "Ressources dispo"):
            row = ctk.CTkFrame(self._map_card, fg_color="transparent")
            row.pack(fill="x", padx=12, pady=1)
            ctk.CTkLabel(row, text=key + ":", font=theme.FONT_SMALL,
                         text_color=theme.TEXT_DIM, width=120,
                         anchor="w").pack(side="left")
            lbl = ctk.CTkLabel(row, text="—", font=theme.FONT_SMALL,
                               text_color=theme.TEXT, anchor="w")
            lbl.pack(side="left", fill="x", expand=True)
            self._map_labels[key] = lbl

        # Bottom card — bot status
        bot_card = self._card(row=2, col=0, colspan=2, title="Statut bot")
        self._bot_status = ctk.CTkLabel(bot_card, text="Inactif",
                                        font=theme.FONT_HEAD,
                                        text_color=theme.TEXT_DIM)
        self._bot_status.pack(padx=12, pady=4, anchor="w")
        self._bot_detail = ctk.CTkLabel(bot_card, text="",
                                        font=theme.FONT_SMALL,
                                        text_color=theme.TEXT_DIM)
        self._bot_detail.pack(padx=12, pady=0, anchor="w")

    # ------------------------------------------------------------------

    def _card(self, row, col, title, colspan=1):
        frame = ctk.CTkFrame(self, fg_color=theme.BG_CARD, corner_radius=8)
        frame.grid(row=row, column=col, columnspan=colspan,
                   padx=12, pady=8, sticky="nsew")
        ctk.CTkLabel(frame, text=title, font=theme.FONT_HEAD,
                     text_color=theme.ACCENT).pack(
            padx=12, pady=(10, 6), anchor="w")
        return frame

    def update_status(self, status: dict):
        """Refresh all labels from a status dict (from Orchestrator.get_status)."""
        c = self._char_labels
        m = self._map_labels

        c["Nom"].configure(text=status.get("character") or "—")
        c["Niveau"].configure(text=str(status.get("level") or "—"))
        c["Race"].configure(text="—")
        hp = status.get("hp")
        max_hp = status.get("max_hp")
        if hp is not None and max_hp:
            c["HP"].configure(text=f"{hp} / {max_hp}")
            self._hp_bar.set(hp / max_hp)
        kamas = status.get("kamas")
        c["Kamas"].configure(text=f"{kamas:,}" if kamas is not None else "—")

        m["Map ID"].configure(text=str(status.get("map_id") or "—"))
        x, y = status.get("map_x"), status.get("map_y")
        m["Coordonnées"].configure(
            text=f"({x}, {y})" if x is not None else "—")
        m["Cellule"].configure(text=str(status.get("cell_id") or "—"))
        m["Ressources dispo"].configure(
            text=str(status.get("resources_on_map", 0)))

        if status.get("in_fight"):
            self._bot_status.configure(text="⚔  En combat",
                                       text_color=theme.TEXT_RED)
        elif status.get("script_running"):
            self._bot_status.configure(text="▶  Script actif",
                                       text_color=theme.TEXT_GREEN)
            self._bot_detail.configure(
                text=f"{status.get('script_steps', 0)} étapes de route")
        elif status.get("is_busy"):
            self._bot_status.configure(
                text=f"⏳  {status.get('busy_reason', 'occupé')}",
                text_color=theme.TEXT_YELLOW)
        else:
            self._bot_status.configure(text="Inactif",
                                       text_color=theme.TEXT_DIM)
            self._bot_detail.configure(text="")

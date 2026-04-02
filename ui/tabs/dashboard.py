"""
Dashboard tab — character info, map position, bot status.
Modern dark layout inspired by Jitsuri.
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
        ctk.CTkLabel(self, text="Tableau de bord", font=theme.FONT_TITLE,
                     text_color=theme.TEXT).grid(
            row=0, column=0, columnspan=2, padx=20, pady=(20, 12), sticky="w")

        # --- Left card: Character ---
        char_card = self._card(row=1, col=0, title="Personnage")

        # HP bar
        hp_frame = ctk.CTkFrame(char_card, fg_color="transparent")
        hp_frame.pack(fill="x", padx=16, pady=(0, 8))
        self._hp_bar = ctk.CTkProgressBar(hp_frame, height=12,
                                           progress_color=theme.TEXT_GREEN,
                                           fg_color=theme.BG_CARD_ALT)
        self._hp_bar.set(0)
        self._hp_bar.pack(fill="x")
        self._hp_pct = ctk.CTkLabel(hp_frame, text="",
                                     font=theme.FONT_SMALL, text_color=theme.TEXT_DIM)
        self._hp_pct.pack(anchor="e")

        # Character fields
        self._char_labels = {}
        for key in ("Nom", "Niveau", "Race", "HP", "Kamas"):
            row = ctk.CTkFrame(char_card, fg_color="transparent")
            row.pack(fill="x", padx=16, pady=2)
            ctk.CTkLabel(row, text=key + ":", font=theme.FONT_BODY,
                         text_color=theme.TEXT_DIM, width=80,
                         anchor="w").pack(side="left")
            lbl = ctk.CTkLabel(row, text="\u2014", font=theme.FONT_BODY,
                               text_color=theme.TEXT, anchor="w")
            lbl.pack(side="left", fill="x", expand=True)
            self._char_labels[key] = lbl

        # --- Right card: Position ---
        map_card = self._card(row=1, col=1, title="Carte & Position")

        self._map_labels = {}
        for key in ("Map ID", "Coordonnees", "Cellule", "Ressources dispo"):
            row = ctk.CTkFrame(map_card, fg_color="transparent")
            row.pack(fill="x", padx=16, pady=2)
            ctk.CTkLabel(row, text=key + ":", font=theme.FONT_BODY,
                         text_color=theme.TEXT_DIM, width=130,
                         anchor="w").pack(side="left")
            lbl = ctk.CTkLabel(row, text="\u2014", font=theme.FONT_BODY,
                               text_color=theme.TEXT, anchor="w")
            lbl.pack(side="left", fill="x", expand=True)
            self._map_labels[key] = lbl

        # --- Bottom card: Bot status ---
        bot_card = self._card(row=2, col=0, colspan=2, title="Statut bot")

        self._bot_status = ctk.CTkLabel(bot_card, text="Inactif",
                                         font=theme.FONT_HEAD,
                                         text_color=theme.TEXT_DIM)
        self._bot_status.pack(padx=16, pady=(4, 2), anchor="w")

        self._bot_detail = ctk.CTkLabel(bot_card, text="",
                                         font=theme.FONT_SMALL,
                                         text_color=theme.TEXT_DIM)
        self._bot_detail.pack(padx=16, pady=(0, 8), anchor="w")

    def _card(self, row, col, title, colspan=1):
        frame = ctk.CTkFrame(self, fg_color=theme.BG_CARD, corner_radius=10)
        frame.grid(row=row, column=col, columnspan=colspan,
                   padx=12, pady=8, sticky="nsew")
        ctk.CTkLabel(frame, text=title, font=theme.FONT_HEAD,
                     text_color=theme.ACCENT).pack(
            padx=16, pady=(12, 8), anchor="w")
        return frame

    def update_status(self, status: dict):
        """Refresh all labels from Orchestrator.get_status()."""
        c = self._char_labels
        m = self._map_labels

        c["Nom"].configure(text=status.get("character") or "\u2014")
        c["Niveau"].configure(text=str(status.get("level") or "\u2014"))
        c["Race"].configure(text="\u2014")

        hp = status.get("hp")
        max_hp = status.get("max_hp")
        if hp is not None and max_hp:
            c["HP"].configure(text=f"{hp} / {max_hp}")
            ratio = hp / max_hp
            self._hp_bar.set(ratio)
            pct = int(ratio * 100)
            self._hp_pct.configure(text=f"{pct}%")
            # Dynamic color
            if ratio > 0.5:
                self._hp_bar.configure(progress_color=theme.TEXT_GREEN)
            elif ratio > 0.2:
                self._hp_bar.configure(progress_color=theme.TEXT_YELLOW)
            else:
                self._hp_bar.configure(progress_color=theme.TEXT_RED)

        kamas = status.get("kamas")
        c["Kamas"].configure(text=f"{kamas:,}" if kamas is not None else "\u2014")

        m["Map ID"].configure(text=str(status.get("map_id") or "\u2014"))
        x, y = status.get("map_x"), status.get("map_y")
        m["Coordonnees"].configure(
            text=f"({x}, {y})" if x is not None else "\u2014")
        m["Cellule"].configure(text=str(status.get("cell_id") or "\u2014"))
        m["Ressources dispo"].configure(
            text=str(status.get("resources_on_map", 0)))

        # Bot status
        if status.get("in_fight"):
            self._bot_status.configure(text="\u2694  En combat",
                                        text_color=theme.TEXT_RED)
        elif status.get("script_running"):
            self._bot_status.configure(text="\u25B6  Script actif",
                                        text_color=theme.TEXT_GREEN)
            self._bot_detail.configure(
                text=f"{status.get('script_steps', 0)} etapes de route")
        elif status.get("is_busy"):
            self._bot_status.configure(
                text=f"\u23F3  {status.get('busy_reason', 'occupe')}",
                text_color=theme.TEXT_YELLOW)
        else:
            self._bot_status.configure(text="Inactif",
                                        text_color=theme.TEXT_DIM)
            self._bot_detail.configure(text="")

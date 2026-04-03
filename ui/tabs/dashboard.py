"""
Dashboard tab — Character info, map position, bot status.
Modern dark layout with animated bars and clean cards.
"""
import customtkinter as ctk
from ui import theme


class DashboardTab(ctk.CTkFrame):
    def __init__(self, parent, **kwargs):
        super().__init__(parent, fg_color=theme.BG, corner_radius=0, **kwargs)
        self._build()

    def _build(self):
        self.grid_columnconfigure(0, weight=1)
        self.grid_columnconfigure(1, weight=1)
        self.grid_rowconfigure(2, weight=1)

        # ── Title ────────────────────────────────────────────────────
        ctk.CTkLabel(
            self, text="Tableau de bord",
            font=theme.FONT_TITLE, text_color=theme.TEXT,
        ).grid(row=0, column=0, columnspan=2, padx=20, pady=(20, 12), sticky="w")

        # ── Left card: Character ─────────────────────────────────────
        char_card = self._card(row=1, col=0, title="Personnage")

        # HP bar
        self._hp_bar, self._hp_pct = self._stat_bar(
            char_card, "HP", theme.TEXT_GREEN)

        # MP bar
        self._mp_bar, self._mp_pct = self._stat_bar(
            char_card, "MP", theme.TEXT_BLUE)

        # AP bar
        self._ap_bar, self._ap_pct = self._stat_bar(
            char_card, "PA", theme.TEXT_YELLOW)

        # Character fields
        self._char_labels = {}
        for key in ("Nom", "Niveau", "Race", "HP", "MP", "PA", "Kamas"):
            row = ctk.CTkFrame(char_card, fg_color="transparent")
            row.pack(fill="x", padx=16, pady=2)
            ctk.CTkLabel(
                row, text=f"{key}:", font=theme.FONT_BODY,
                text_color=theme.TEXT_DIM, width=80, anchor="w",
            ).pack(side="left")
            lbl = ctk.CTkLabel(
                row, text="—", font=theme.FONT_BODY,
                text_color=theme.TEXT, anchor="w",
            )
            lbl.pack(side="left", fill="x", expand=True)
            self._char_labels[key] = lbl

        # ── Right card: Position ─────────────────────────────────────
        map_card = self._card(row=1, col=1, title="Carte & Position")

        self._map_labels = {}
        for key in ("Map ID", "Coordonnées", "Cellule", "Ressources dispo"):
            row = ctk.CTkFrame(map_card, fg_color="transparent")
            row.pack(fill="x", padx=16, pady=2)
            ctk.CTkLabel(
                row, text=f"{key}:", font=theme.FONT_BODY,
                text_color=theme.TEXT_DIM, width=130, anchor="w",
            ).pack(side="left")
            lbl = ctk.CTkLabel(
                row, text="—", font=theme.FONT_BODY,
                text_color=theme.TEXT, anchor="w",
            )
            lbl.pack(side="left", fill="x", expand=True)
            self._map_labels[key] = lbl

        # Entities section
        sep = ctk.CTkFrame(map_card, height=1, fg_color=theme.BORDER)
        sep.pack(fill="x", padx=16, pady=(8, 4))

        ctk.CTkLabel(
            map_card, text="Entités sur la carte",
            font=theme.FONT_SMALL, text_color=theme.TEXT_DIM,
        ).pack(padx=16, anchor="w")

        entity_row = ctk.CTkFrame(map_card, fg_color="transparent")
        entity_row.pack(fill="x", padx=16, pady=(2, 8))

        self._entity_counts = {}
        for label, color in [("Monstres", theme.TEXT_RED),
                              ("PNJ", theme.MAP_NPC),
                              ("Joueurs", theme.TEXT_BLUE)]:
            f = ctk.CTkFrame(entity_row, fg_color="transparent")
            f.pack(side="left", padx=(0, 16))
            lbl = ctk.CTkLabel(
                f, text="0", font=theme.FONT_HEAD,
                text_color=color,
            )
            lbl.pack(side="left")
            ctk.CTkLabel(
                f, text=f"  {label}", font=theme.FONT_SMALL,
                text_color=theme.TEXT_DIM,
            ).pack(side="left")
            self._entity_counts[label] = lbl

        # ── Bottom card: Bot status ──────────────────────────────────
        bot_card = self._card(row=2, col=0, colspan=2, title="Statut bot")

        self._bot_status = ctk.CTkLabel(
            bot_card, text="Inactif",
            font=theme.FONT_HEAD, text_color=theme.TEXT_DIM,
        )
        self._bot_status.pack(padx=16, pady=(4, 2), anchor="w")

        self._bot_detail = ctk.CTkLabel(
            bot_card, text="",
            font=theme.FONT_SMALL, text_color=theme.TEXT_DIM,
        )
        self._bot_detail.pack(padx=16, pady=(0, 8), anchor="w")

    # ─── Helpers ─────────────────────────────────────────────────────

    def _card(self, row, col, title, colspan=1):
        """Create a themed card with a title."""
        frame = ctk.CTkFrame(self, fg_color=theme.BG_CARD, corner_radius=12)
        frame.grid(
            row=row, column=col, columnspan=colspan,
            padx=10, pady=6, sticky="nsew",
        )
        ctk.CTkLabel(
            frame, text=title, font=theme.FONT_HEAD,
            text_color=theme.ACCENT,
        ).pack(padx=16, pady=(12, 8), anchor="w")
        return frame

    def _stat_bar(self, parent, label: str, color: str):
        """Create a labeled progress bar and percentage label."""
        frame = ctk.CTkFrame(parent, fg_color="transparent")
        frame.pack(fill="x", padx=16, pady=(0, 4))

        ctk.CTkLabel(
            frame, text=label, font=theme.FONT_SMALL,
            text_color=theme.TEXT_DIM, width=30,
        ).pack(side="left")

        bar = ctk.CTkProgressBar(
            frame, height=10,
            progress_color=color,
            fg_color=theme.BG_CARD_ALT,
            corner_radius=5,
        )
        bar.set(0)
        bar.pack(side="left", fill="x", expand=True, padx=(4, 8))

        pct = ctk.CTkLabel(
            frame, text="", font=theme.FONT_TINY,
            text_color=theme.TEXT_DIM, width=36,
        )
        pct.pack(side="right")

        return bar, pct

    # ─── Update from Orchestrator status ─────────────────────────────

    def update_status(self, status: dict):
        """Refresh all labels from Orchestrator.get_status()."""
        c = self._char_labels
        m = self._map_labels

        c["Nom"].configure(text=status.get("character") or "—")
        c["Niveau"].configure(text=str(status.get("level") or "—"))
        c["Race"].configure(text="—")

        # HP bar
        hp = status.get("hp")
        max_hp = status.get("max_hp")
        if hp is not None and max_hp:
            c["HP"].configure(text=f"{hp} / {max_hp}")
            ratio = hp / max_hp
            self._hp_bar.set(ratio)
            self._hp_pct.configure(text=f"{int(ratio * 100)}%")
            # Dynamic color
            if ratio > 0.5:
                self._hp_bar.configure(progress_color=theme.TEXT_GREEN)
            elif ratio > 0.2:
                self._hp_bar.configure(progress_color=theme.TEXT_YELLOW)
            else:
                self._hp_bar.configure(progress_color=theme.TEXT_RED)

        # MP bar
        mp = status.get("mp")
        max_mp = status.get("max_mp")
        if mp is not None and max_mp:
            c["MP"].configure(text=f"{mp} / {max_mp}")
            self._mp_bar.set(mp / max_mp)
            self._mp_pct.configure(text=f"{mp}/{max_mp}")

        # AP bar
        ap = status.get("ap")
        max_ap = status.get("max_ap")
        if ap is not None and max_ap:
            c["PA"].configure(text=f"{ap} / {max_ap}")
            self._ap_bar.set(ap / max_ap)
            self._ap_pct.configure(text=f"{ap}/{max_ap}")

        # Kamas
        kamas = status.get("kamas")
        c["Kamas"].configure(text=f"{kamas:,}" if kamas is not None else "—")

        # Map info
        m["Map ID"].configure(text=str(status.get("map_id") or "—"))
        x, y = status.get("map_x"), status.get("map_y")
        m["Coordonnées"].configure(
            text=f"({x}, {y})" if x is not None else "—")
        m["Cellule"].configure(text=str(status.get("cell_id") or "—"))
        m["Ressources dispo"].configure(
            text=str(status.get("resources_on_map", 0)))

        # Entity counts
        monsters = status.get("monster_count", 0)
        npcs = status.get("npc_count", 0)
        players = status.get("player_count", 0)
        self._entity_counts["Monstres"].configure(text=str(monsters))
        self._entity_counts["PNJ"].configure(text=str(npcs))
        self._entity_counts["Joueurs"].configure(text=str(players))

        # Bot status
        if status.get("in_fight"):
            self._bot_status.configure(
                text="⚔  En combat", text_color=theme.TEXT_RED)
            self._bot_detail.configure(text="")
        elif status.get("script_running"):
            self._bot_status.configure(
                text="▶  Script actif", text_color=theme.TEXT_GREEN)
            self._bot_detail.configure(
                text=f"{status.get('script_steps', 0)} étapes de route")
        elif status.get("is_busy"):
            reason = status.get("busy_reason", "occupé")
            self._bot_status.configure(
                text=f"⏳  {reason}", text_color=theme.TEXT_YELLOW)
            self._bot_detail.configure(text="")
        else:
            self._bot_status.configure(
                text="Inactif", text_color=theme.TEXT_DIM)
            self._bot_detail.configure(text="")

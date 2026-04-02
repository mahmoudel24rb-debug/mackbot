"""
Harvest tab — script selection, live logs, resource stats.
"""
import os
import customtkinter as ctk
import tkinter as tk
from ui import theme
import config


class HarvestTab(ctk.CTkFrame):
    def __init__(self, parent, on_start, on_stop, on_script_select, **kwargs):
        super().__init__(parent, fg_color=theme.BG, **kwargs)
        self.on_start = on_start
        self.on_stop = on_stop
        self.on_script_select = on_script_select
        self._script_path: str | None = None
        self._resource_counts: dict[str, int] = {}
        self._build()

    def _build(self):
        self.grid_columnconfigure(0, weight=1)
        self.grid_columnconfigure(1, weight=2)
        self.grid_rowconfigure(1, weight=1)

        # Header
        header = ctk.CTkFrame(self, fg_color="transparent")
        header.grid(row=0, column=0, columnspan=2, padx=16, pady=(16, 4),
                    sticky="ew")
        ctk.CTkLabel(header, text="Récolte automatique",
                     font=theme.FONT_TITLE, text_color=theme.TEXT).pack(
            side="left")

        self._start_btn = ctk.CTkButton(
            header, text="▶  Démarrer", font=theme.FONT_BODY,
            fg_color=theme.TEXT_GREEN, hover_color="#388e3c",
            width=120, command=self._start_clicked,
        )
        self._start_btn.pack(side="right", padx=4)
        self._stop_btn = ctk.CTkButton(
            header, text="■  Arrêter", font=theme.FONT_BODY,
            fg_color=theme.TEXT_RED, hover_color="#c62828",
            width=120, state="disabled", command=self._stop_clicked,
        )
        self._stop_btn.pack(side="right", padx=4)

        # Left panel — script browser + stats
        left = ctk.CTkFrame(self, fg_color=theme.BG_PANEL, corner_radius=8)
        left.grid(row=1, column=0, padx=(12, 6), pady=8, sticky="nsew")
        left.grid_rowconfigure(2, weight=1)

        ctk.CTkLabel(left, text="Scripts", font=theme.FONT_HEAD,
                     text_color=theme.ACCENT).grid(
            row=0, column=0, padx=12, pady=(10, 4), sticky="w")

        # Script list
        self._script_list = tk.Listbox(
            left, bg=theme.BG_CARD, fg=theme.TEXT,
            selectbackground=theme.ACCENT, selectforeground="white",
            font=theme.FONT_MONO, borderwidth=0, highlightthickness=0,
        )
        self._script_list.grid(row=1, column=0, padx=8, pady=4,
                               sticky="nsew")
        self._script_list.bind("<<ListboxSelect>>", self._on_script_click)
        left.grid_rowconfigure(1, weight=1)
        left.grid_columnconfigure(0, weight=1)

        ctk.CTkButton(left, text="Actualiser", font=theme.FONT_SMALL,
                      height=28, command=self._refresh_scripts).grid(
            row=2, column=0, padx=8, pady=4, sticky="ew")

        # Stats card
        stats_card = ctk.CTkFrame(left, fg_color=theme.BG_CARD,
                                  corner_radius=6)
        stats_card.grid(row=3, column=0, padx=8, pady=(4, 10), sticky="ew")
        ctk.CTkLabel(stats_card, text="Ressources récoltées",
                     font=theme.FONT_SMALL, text_color=theme.ACCENT).pack(
            padx=10, pady=(6, 2), anchor="w")
        self._stats_text = ctk.CTkLabel(stats_card, text="—",
                                        font=theme.FONT_SMALL,
                                        text_color=theme.TEXT, justify="left",
                                        anchor="w")
        self._stats_text.pack(padx=10, pady=(0, 8), fill="x")

        # Right panel — logs
        right = ctk.CTkFrame(self, fg_color=theme.BG_PANEL, corner_radius=8)
        right.grid(row=1, column=1, padx=(6, 12), pady=8, sticky="nsew")
        right.grid_rowconfigure(1, weight=1)
        right.grid_columnconfigure(0, weight=1)

        log_header = ctk.CTkFrame(right, fg_color="transparent")
        log_header.grid(row=0, column=0, padx=8, pady=(8, 2), sticky="ew")
        ctk.CTkLabel(log_header, text="Logs", font=theme.FONT_HEAD,
                     text_color=theme.ACCENT).pack(side="left")
        ctk.CTkButton(log_header, text="Vider", font=theme.FONT_SMALL,
                      height=24, width=60,
                      command=self._clear_logs).pack(side="right")

        self._log_text = tk.Text(
            right, bg=theme.BG_CARD, fg=theme.TEXT,
            font=theme.FONT_MONO, borderwidth=0, highlightthickness=0,
            state="disabled", wrap="word",
        )
        self._log_text.grid(row=1, column=0, padx=8, pady=(0, 8),
                            sticky="nsew")
        # Configure log colour tags
        for tag, colour in theme.LOG_COLORS.items():
            self._log_text.tag_configure(tag, foreground=colour)

        scrollbar = ctk.CTkScrollbar(right, command=self._log_text.yview)
        scrollbar.grid(row=1, column=1, sticky="ns", pady=(0, 8))
        self._log_text.configure(yscrollcommand=scrollbar.set)

        # Populate script list
        self._refresh_scripts()

    # ------------------------------------------------------------------
    # Script browsing
    # ------------------------------------------------------------------

    def _refresh_scripts(self):
        self._script_list.delete(0, "end")
        scripts_dir = getattr(config, "SCRIPTS_DIR", "scripts")
        fix_dir = "fix script"
        sources = [scripts_dir, fix_dir]
        for src in sources:
            if os.path.isdir(src):
                for name in sorted(os.listdir(src)):
                    if name.endswith((".lua", ".py")):
                        self._script_list.insert("end", os.path.join(src, name))

    def _on_script_click(self, _event=None):
        sel = self._script_list.curselection()
        if sel:
            path = self._script_list.get(sel[0])
            self._script_path = path
            self.on_script_select(path)

    # ------------------------------------------------------------------
    # Start / Stop
    # ------------------------------------------------------------------

    def _start_clicked(self):
        if self._script_path:
            self._start_btn.configure(state="disabled")
            self._stop_btn.configure(state="normal")
            self.on_start(self._script_path)

    def _stop_clicked(self):
        self._start_btn.configure(state="normal")
        self._stop_btn.configure(state="disabled")
        self.on_stop()

    def set_running(self, running: bool):
        """Update button states from outside."""
        if running:
            self._start_btn.configure(state="disabled")
            self._stop_btn.configure(state="normal")
        else:
            self._start_btn.configure(state="normal")
            self._stop_btn.configure(state="disabled")

    # ------------------------------------------------------------------
    # Logs
    # ------------------------------------------------------------------

    def append_log(self, text: str, level: str = "info"):
        from datetime import datetime
        tag = level if level in theme.LOG_COLORS else "info"
        ts = datetime.now().strftime("[%H:%M:%S] ")
        self._log_text.configure(state="normal")
        self._log_text.insert("end", ts + text + "\n", tag)
        self._log_text.see("end")
        self._log_text.configure(state="disabled")

    def _clear_logs(self):
        self._log_text.configure(state="normal")
        self._log_text.delete("1.0", "end")
        self._log_text.configure(state="disabled")

    # ------------------------------------------------------------------
    # Resource stats
    # ------------------------------------------------------------------

    def record_harvest(self, resource_name: str = "Ressource"):
        self._resource_counts[resource_name] = (
            self._resource_counts.get(resource_name, 0) + 1
        )
        lines = [f"{k}: {v}" for k, v in
                 sorted(self._resource_counts.items())]
        self._stats_text.configure(text="\n".join(lines) or "—")

    def reset_stats(self):
        self._resource_counts.clear()
        self._stats_text.configure(text="—")

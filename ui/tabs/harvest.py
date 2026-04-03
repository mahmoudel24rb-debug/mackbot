"""
Harvest tab — script selection, live logs, resource stats, session timer.
Replaces tk.Listbox with CTk-native scrollable list for consistent theming.
"""
import os
import time
import customtkinter as ctk
import tkinter as tk
from datetime import datetime
from ui import theme
import config


class HarvestTab(ctk.CTkFrame):
    def __init__(self, parent, on_start, on_stop, on_script_select, **kwargs):
        super().__init__(parent, fg_color=theme.BG, corner_radius=0, **kwargs)
        self.on_start = on_start
        self.on_stop = on_stop
        self.on_script_select = on_script_select
        self._script_path: str | None = None
        self._resource_counts: dict[str, int] = {}
        self._session_start: float | None = None
        self._total_harvests: int = 0
        self._build()

    def _build(self):
        self.grid_columnconfigure(0, weight=1)
        self.grid_columnconfigure(1, weight=2)
        self.grid_rowconfigure(1, weight=1)

        # ── Header ───────────────────────────────────────────────────
        header = ctk.CTkFrame(self, fg_color="transparent")
        header.grid(row=0, column=0, columnspan=2, padx=16, pady=(16, 6),
                    sticky="ew")
        header.grid_columnconfigure(0, weight=1)

        ctk.CTkLabel(
            header, text="Récolte automatique",
            font=theme.FONT_TITLE, text_color=theme.TEXT,
        ).grid(row=0, column=0, sticky="w")

        # Session timer
        self._timer_label = ctk.CTkLabel(
            header, text="",
            font=theme.FONT_SMALL, text_color=theme.TEXT_DIM,
        )
        self._timer_label.grid(row=0, column=1, padx=8)

        self._stop_btn = ctk.CTkButton(
            header, text="■  Arrêter", font=theme.FONT_BODY,
            fg_color=theme.TEXT_RED, hover_color="#b91c1c",
            width=120, height=32, corner_radius=8,
            state="disabled", command=self._stop_clicked,
        )
        self._stop_btn.grid(row=0, column=2, padx=4)

        self._start_btn = ctk.CTkButton(
            header, text="▶  Démarrer", font=theme.FONT_BODY,
            fg_color=theme.ACCENT_DIM, hover_color=theme.ACCENT_HOVER,
            text_color=theme.BG,
            width=120, height=32, corner_radius=8,
            command=self._start_clicked,
        )
        self._start_btn.grid(row=0, column=3, padx=4)

        # ── Left panel — Scripts + Stats ─────────────────────────────
        left = ctk.CTkFrame(self, fg_color=theme.BG_PANEL, corner_radius=10)
        left.grid(row=1, column=0, padx=(12, 6), pady=6, sticky="nsew")
        left.grid_rowconfigure(1, weight=1)
        left.grid_columnconfigure(0, weight=1)

        # Scripts header
        scripts_header = ctk.CTkFrame(left, fg_color="transparent")
        scripts_header.grid(row=0, column=0, padx=10, pady=(10, 4), sticky="ew")
        scripts_header.grid_columnconfigure(0, weight=1)

        ctk.CTkLabel(
            scripts_header, text="Scripts",
            font=theme.FONT_HEAD, text_color=theme.ACCENT,
        ).grid(row=0, column=0, sticky="w")

        ctk.CTkButton(
            scripts_header, text="↻", font=theme.FONT_BODY,
            width=28, height=28, corner_radius=6,
            fg_color=theme.BG_CARD_ALT,
            hover_color=theme.BG_HOVER,
            command=self._refresh_scripts,
        ).grid(row=0, column=1)

        # Script list (CTk ScrollableFrame instead of tk.Listbox)
        self._script_frame = ctk.CTkScrollableFrame(
            left, fg_color=theme.BG_CARD,
            corner_radius=6,
        )
        self._script_frame.grid(
            row=1, column=0, padx=8, pady=4, sticky="nsew")
        self._script_buttons: list[ctk.CTkButton] = []
        self._selected_idx: int = -1

        # Stats card
        stats_card = ctk.CTkFrame(
            left, fg_color=theme.BG_CARD, corner_radius=8)
        stats_card.grid(row=2, column=0, padx=8, pady=(6, 10), sticky="ew")

        ctk.CTkLabel(
            stats_card, text="Session",
            font=theme.FONT_SMALL, text_color=theme.ACCENT,
        ).pack(padx=10, pady=(8, 2), anchor="w")

        self._stats_text = ctk.CTkLabel(
            stats_card, text="—",
            font=theme.FONT_SMALL, text_color=theme.TEXT,
            justify="left", anchor="w",
        )
        self._stats_text.pack(padx=10, pady=(0, 4), fill="x")

        self._rate_text = ctk.CTkLabel(
            stats_card, text="",
            font=theme.FONT_TINY, text_color=theme.TEXT_DIM,
            justify="left", anchor="w",
        )
        self._rate_text.pack(padx=10, pady=(0, 8), fill="x")

        # ── Right panel — Logs ───────────────────────────────────────
        right = ctk.CTkFrame(self, fg_color=theme.BG_PANEL, corner_radius=10)
        right.grid(row=1, column=1, padx=(6, 12), pady=6, sticky="nsew")
        right.grid_rowconfigure(1, weight=1)
        right.grid_columnconfigure(0, weight=1)

        log_header = ctk.CTkFrame(right, fg_color="transparent")
        log_header.grid(row=0, column=0, padx=10, pady=(10, 4), sticky="ew")
        log_header.grid_columnconfigure(0, weight=1)

        ctk.CTkLabel(
            log_header, text="Logs",
            font=theme.FONT_HEAD, text_color=theme.ACCENT,
        ).grid(row=0, column=0, sticky="w")

        ctk.CTkButton(
            log_header, text="Vider", font=theme.FONT_SMALL,
            height=24, width=56, corner_radius=6,
            fg_color=theme.BG_CARD_ALT,
            hover_color=theme.BG_HOVER,
            command=self._clear_logs,
        ).grid(row=0, column=1)

        # Log text area
        self._log_text = tk.Text(
            right, bg=theme.BG_CARD, fg=theme.TEXT,
            font=theme.FONT_MONO, borderwidth=0, highlightthickness=0,
            state="disabled", wrap="word",
            insertbackground=theme.TEXT,
            selectbackground=theme.ACCENT_DIM,
            padx=8, pady=6,
        )
        self._log_text.grid(
            row=1, column=0, padx=8, pady=(0, 8), sticky="nsew")

        # Configure log colour tags
        for tag, colour in theme.LOG_COLORS.items():
            self._log_text.tag_configure(tag, foreground=colour)

        scrollbar = ctk.CTkScrollbar(right, command=self._log_text.yview)
        scrollbar.grid(row=1, column=1, sticky="ns", pady=(0, 8))
        self._log_text.configure(yscrollcommand=scrollbar.set)

        # Populate scripts
        self._refresh_scripts()

    # ─── Script browsing ─────────────────────────────────────────────

    def _refresh_scripts(self):
        """Scan script directories and populate the list."""
        # Clear existing
        for btn in self._script_buttons:
            btn.destroy()
        self._script_buttons.clear()
        self._selected_idx = -1

        scripts_dir = getattr(config, "SCRIPTS_DIR", "scripts")
        fix_dir = "fix script"
        sources = [scripts_dir, fix_dir]

        idx = 0
        for src in sources:
            if os.path.isdir(src):
                for name in sorted(os.listdir(src)):
                    if name.endswith((".lua", ".py")):
                        path = os.path.join(src, name)
                        btn = ctk.CTkButton(
                            self._script_frame,
                            text=name,
                            anchor="w",
                            font=theme.FONT_MONO,
                            fg_color="transparent",
                            text_color=theme.TEXT_DIM,
                            hover_color=theme.BG_HOVER,
                            height=28, corner_radius=4,
                            command=lambda p=path, i=idx: self._select_script(p, i),
                        )
                        btn.pack(fill="x", pady=1)
                        self._script_buttons.append(btn)
                        idx += 1

    def _select_script(self, path: str, idx: int):
        """Select a script from the list."""
        # Deselect previous
        if 0 <= self._selected_idx < len(self._script_buttons):
            self._script_buttons[self._selected_idx].configure(
                fg_color="transparent", text_color=theme.TEXT_DIM)

        # Select new
        self._selected_idx = idx
        self._script_path = path
        if 0 <= idx < len(self._script_buttons):
            self._script_buttons[idx].configure(
                fg_color=theme.ACCENT_BG, text_color=theme.ACCENT)

        self.on_script_select(path)

    # ─── Start / Stop ────────────────────────────────────────────────

    def _start_clicked(self):
        if self._script_path:
            self._start_btn.configure(state="disabled")
            self._stop_btn.configure(state="normal")
            self._session_start = time.time()
            self._total_harvests = 0
            self._update_timer()
            self.on_start(self._script_path)

    def _stop_clicked(self):
        self._start_btn.configure(state="normal")
        self._stop_btn.configure(state="disabled")
        self._session_start = None
        self.on_stop()

    def set_running(self, running: bool):
        """Update button states from outside."""
        if running:
            self._start_btn.configure(state="disabled")
            self._stop_btn.configure(state="normal")
            if self._session_start is None:
                self._session_start = time.time()
                self._update_timer()
        else:
            self._start_btn.configure(state="normal")
            self._stop_btn.configure(state="disabled")
            self._session_start = None

    # ─── Session timer ───────────────────────────────────────────────

    def _update_timer(self):
        """Update the session duration display."""
        if self._session_start is None:
            return
        elapsed = int(time.time() - self._session_start)
        hours = elapsed // 3600
        mins = (elapsed % 3600) // 60
        secs = elapsed % 60
        self._timer_label.configure(
            text=f"⏱ {hours:02d}:{mins:02d}:{secs:02d}")

        # Update harvest rate
        if elapsed > 0 and self._total_harvests > 0:
            rate_h = self._total_harvests / (elapsed / 3600)
            self._rate_text.configure(text=f"~{rate_h:.0f} récoltes/h")

        self.after(1000, self._update_timer)

    # ─── Logs ────────────────────────────────────────────────────────

    def append_log(self, text: str, level: str = "info"):
        """Append a timestamped log entry."""
        tag = level if level in theme.LOG_COLORS else "info"
        ts = datetime.now().strftime("[%H:%M:%S] ")
        self._log_text.configure(state="normal")
        self._log_text.insert("end", ts + text + "\n", tag)
        # Auto-trim (keep last 500 lines)
        line_count = int(self._log_text.index("end-1c").split(".")[0])
        if line_count > 500:
            self._log_text.delete("1.0", f"{line_count - 400}.0")
        self._log_text.see("end")
        self._log_text.configure(state="disabled")

    def _clear_logs(self):
        self._log_text.configure(state="normal")
        self._log_text.delete("1.0", "end")
        self._log_text.configure(state="disabled")

    # ─── Resource stats ──────────────────────────────────────────────

    def record_harvest(self, resource_name: str = "Ressource"):
        """Record a harvested resource."""
        self._resource_counts[resource_name] = (
            self._resource_counts.get(resource_name, 0) + 1)
        self._total_harvests += 1

        lines = [f"{k}: {v}" for k, v in
                 sorted(self._resource_counts.items())]
        total = sum(self._resource_counts.values())
        lines.append(f"─── Total: {total}")
        self._stats_text.configure(text="\n".join(lines))

    def reset_stats(self):
        self._resource_counts.clear()
        self._total_harvests = 0
        self._stats_text.configure(text="—")
        self._rate_text.configure(text="")

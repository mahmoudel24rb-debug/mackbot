"""
Sniffer/Matching tab - Capture and correct 3-letter protocol codes.

Start sniffing -> play manually -> bot captures all codes -> save to matching.json.
Shows real-time traffic with code -> name mappings.
"""
import customtkinter as ctk
import tkinter as tk
from ui import theme


class SnifferTab(ctk.CTkFrame):
    """Tab for sniffing and correcting protocol code matching."""

    def __init__(self, parent, on_start_sniff=None, on_stop_sniff=None,
                 on_save_matching=None, **kwargs):
        super().__init__(parent, fg_color=theme.BG, corner_radius=0, **kwargs)
        self._on_start = on_start_sniff
        self._on_stop = on_stop_sniff
        self._on_save = on_save_matching
        self._sniffing = False
        self._build()

    def _build(self):
        self.grid_columnconfigure(0, weight=1)
        self.grid_columnconfigure(1, weight=2)
        self.grid_rowconfigure(1, weight=1)

        # --- Header ---
        header = ctk.CTkFrame(self, fg_color=theme.BG_PANEL, corner_radius=8)
        header.grid(row=0, column=0, columnspan=2, padx=12, pady=(12, 6), sticky="ew")
        header.grid_columnconfigure(1, weight=1)

        ctk.CTkLabel(header, text="Sniffer / Matching",
                     font=theme.FONT_HEAD,
                     text_color=theme.ACCENT).grid(row=0, column=0, padx=12, pady=8)

        self._status_label = ctk.CTkLabel(header, text="Idle",
                                           font=theme.FONT_SMALL,
                                           text_color=theme.TEXT_DIM)
        self._status_label.grid(row=0, column=1, padx=8, pady=8, sticky="w")

        self._btn_start = ctk.CTkButton(header, text="Start Sniffing",
                                         font=theme.FONT_BODY,
                                         fg_color=theme.ACCENT,
                                         width=120, height=32,
                                         command=self._toggle_sniff)
        self._btn_start.grid(row=0, column=2, padx=4, pady=8)

        self._btn_clear = ctk.CTkButton(header, text="Clear",
                                        font=theme.FONT_BODY,
                                        fg_color=theme.BG_CARD_ALT,
                                        width=80, height=32,
                                        command=self._clear_traffic)
        self._btn_clear.grid(row=0, column=3, padx=4, pady=8)

        self._btn_save = ctk.CTkButton(header, text="Save Matching",
                                        font=theme.FONT_BODY,
                                        fg_color=theme.TEXT_GREEN,
                                        width=120, height=32,
                                        command=self._save_clicked)
        self._btn_save.grid(row=0, column=4, padx=(4, 12), pady=8)

        # Direction filter
        self._dir_filter = ctk.CTkSegmentedButton(
            header, values=["Both", "C2S", "S2C"],
            font=theme.FONT_SMALL, height=28,
            command=self._on_filter_change)
        self._dir_filter.set("Both")
        self._dir_filter.grid(row=0, column=5, padx=(4, 12), pady=8)
        self._current_filter = "Both"

        # --- Left: Matching Table ---
        left = ctk.CTkFrame(self, fg_color=theme.BG_PANEL, corner_radius=8)
        left.grid(row=1, column=0, padx=(12, 6), pady=6, sticky="nsew")
        left.grid_rowconfigure(1, weight=1)
        left.grid_columnconfigure(0, weight=1)

        ctk.CTkLabel(left, text="Code Matching",
                     font=theme.FONT_BODY,
                     text_color=theme.TEXT).grid(row=0, column=0, padx=8, pady=(8, 4), sticky="w")

        self._match_text = tk.Text(left, bg=theme.BG, fg=theme.TEXT,
                                    font=("Consolas", 10),
                                    insertbackground=theme.TEXT,
                                    selectbackground=theme.ACCENT,
                                    relief="flat", wrap="none",
                                    state="disabled")
        self._match_text.grid(row=1, column=0, padx=8, pady=(0, 8), sticky="nsew")

        # --- Right: Live Traffic ---
        right = ctk.CTkFrame(self, fg_color=theme.BG_PANEL, corner_radius=8)
        right.grid(row=1, column=1, padx=(6, 12), pady=6, sticky="nsew")
        right.grid_rowconfigure(1, weight=1)
        right.grid_columnconfigure(0, weight=1)

        ctk.CTkLabel(right, text="Live Traffic",
                     font=theme.FONT_BODY,
                     text_color=theme.TEXT).grid(row=0, column=0, padx=8, pady=(8, 4), sticky="w")

        self._traffic_text = tk.Text(right, bg=theme.BG, fg=theme.TEXT,
                                      font=("Consolas", 9),
                                      insertbackground=theme.TEXT,
                                      selectbackground=theme.ACCENT,
                                      relief="flat", wrap="none",
                                      state="disabled")
        self._traffic_text.grid(row=1, column=0, padx=8, pady=(0, 8), sticky="nsew")

        # Configure text tags for colors
        for widget in [self._traffic_text, self._match_text]:
            widget.tag_configure("c2s", foreground="#4FC3F7")
            widget.tag_configure("s2c", foreground="#81C784")
            widget.tag_configure("code", foreground=theme.ACCENT)
            widget.tag_configure("new", foreground="#FFD54F")
            widget.tag_configure("dim", foreground=theme.TEXT_DIM)

        # Stats
        stats = ctk.CTkFrame(self, fg_color=theme.BG_PANEL, corner_radius=8)
        stats.grid(row=2, column=0, columnspan=2, padx=12, pady=(6, 12), sticky="ew")
        self._stats_label = ctk.CTkLabel(stats, text="Packets: 0 | Codes: 0 | New: 0",
                                          font=theme.FONT_SMALL,
                                          text_color=theme.TEXT_DIM)
        self._stats_label.grid(row=0, column=0, padx=12, pady=6)

        self._packet_count = 0
        self._new_codes = 0

    def _toggle_sniff(self):
        if self._sniffing:
            self._sniffing = False
            self._btn_start.configure(text="Start Sniffing", fg_color=theme.ACCENT)
            self._status_label.configure(text="Stopped", text_color=theme.TEXT_DIM)
            if self._on_stop:
                self._on_stop()
        else:
            self._sniffing = True
            self._btn_start.configure(text="Stop Sniffing", fg_color=theme.TEXT_RED)
            self._status_label.configure(text="Sniffing...", text_color=theme.TEXT_GREEN)
            if self._on_start:
                self._on_start()

    def _save_clicked(self):
        if self._on_save:
            self._on_save()
        self._status_label.configure(text="Matching saved!", text_color=theme.TEXT_GREEN)

    def _clear_traffic(self):
        self._traffic_text.configure(state="normal")
        self._traffic_text.delete("1.0", "end")
        self._traffic_text.configure(state="disabled")
        self._packet_count = 0
        self._update_stats()

    def _on_filter_change(self, value):
        self._current_filter = value

    def add_traffic(self, code, name, direction, size):
        """Add a traffic entry to the live view."""
        self._packet_count += 1
        # Apply direction filter
        if self._current_filter == "C2S" and direction != "c2s":
            self._update_stats()
            return
        if self._current_filter == "S2C" and direction != "s2c":
            self._update_stats()
            return
        tag = "c2s" if direction == "c2s" else "s2c"
        arrow = ">>>" if direction == "c2s" else "<<<"
        line = f"{arrow} {code:>3s} ({name}) [{size}B]\n"

        self._traffic_text.configure(state="normal")
        self._traffic_text.insert("end", line, tag)
        # Auto-scroll and limit lines
        lines = int(self._traffic_text.index("end-1c").split(".")[0])
        if lines > 500:
            self._traffic_text.delete("1.0", "100.0")
        self._traffic_text.see("end")
        self._traffic_text.configure(state="disabled")

        self._update_stats()

    def add_new_match(self, code, name, is_new=False):
        """Add or update a code->name match in the table."""
        if is_new:
            self._new_codes += 1
        tag = "new" if is_new else "dim"
        line = f"{'NEW ' if is_new else '    '}{code} -> {name}\n"

        self._match_text.configure(state="normal")
        self._match_text.insert("end", line, tag)
        self._match_text.see("end")
        self._match_text.configure(state="disabled")

        self._update_stats()

    def refresh_matching(self, codes_dict):
        """Refresh the full matching table from a dict."""
        self._match_text.configure(state="normal")
        self._match_text.delete("1.0", "end")
        for code, name in sorted(codes_dict.items(), key=lambda x: x[1]):
            self._match_text.insert("end", f"  {code} -> {name}\n", "dim")
        self._match_text.configure(state="disabled")

    def _update_stats(self):
        total_codes = int(self._match_text.index("end-1c").split(".")[0]) - 1
        self._stats_label.configure(
            text=f"Packets: {self._packet_count} | Codes: {total_codes} | New: {self._new_codes}")

    @property
    def is_sniffing(self):
        return self._sniffing

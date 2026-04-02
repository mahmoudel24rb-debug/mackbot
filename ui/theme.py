"""
Dark theme — Jitsuri-inspired modern dark palette.
"""
import customtkinter as ctk
import config


def apply():
    ctk.set_appearance_mode(config.UI_THEME)
    ctk.set_default_color_theme("green")


# Colour palette (Jitsuri-inspired dark)
BG          = "#1a1a1a"
BG_PANEL    = "#222222"
BG_CARD     = "#2a2a2a"
BG_CARD_ALT = "#333333"
BG_INPUT    = "#333333"
ACCENT      = "#4caf50"
ACCENT_DIM  = "#2e7d32"
ACCENT_HOVER = "#388e3c"
BORDER      = "#3a3a3a"
TEXT        = "#e0e0e0"
TEXT_DIM    = "#888888"
TEXT_GREEN  = "#4caf50"
TEXT_RED    = "#f44336"
TEXT_YELLOW = "#ffc107"
TEXT_BLUE   = "#64b5f6"
TEXT_ORANGE = "#ff9800"
TEXT_PINK   = "#e91e63"

# Fonts
FONT_TITLE  = ("Segoe UI", 18, "bold")
FONT_HEAD   = ("Segoe UI", 14, "bold")
FONT_BODY   = ("Segoe UI", 12)
FONT_SMALL  = ("Segoe UI", 10)
FONT_MONO   = ("Consolas", 11)
FONT_BADGE  = ("Segoe UI", 9, "bold")

# Log colours
LOG_COLORS = {
    "info":    TEXT,
    "success": TEXT_GREEN,
    "warning": TEXT_YELLOW,
    "error":   TEXT_RED,
    "debug":   TEXT_DIM,
    "gather":  "#80cbc4",
    "nav":     TEXT_BLUE,
}

"""
Dark theme constants for the CustomTkinter UI.
"""
import customtkinter as ctk
import config


def apply():
    """Apply global theme settings."""
    ctk.set_appearance_mode(config.UI_THEME)
    ctk.set_default_color_theme("blue")


# Colour palette
BG          = "#1a1a2e"      # main background
BG_PANEL    = "#16213e"      # panel / sidebar
BG_CARD     = "#0f3460"      # card / input background
ACCENT      = config.UI_ACCENT
TEXT        = "#e0e0e0"
TEXT_DIM    = "#888888"
TEXT_GREEN  = "#4caf50"
TEXT_RED    = "#f44336"
TEXT_YELLOW = "#ffc107"
TEXT_BLUE   = "#64b5f6"

# Font sizes
FONT_TITLE  = ("Segoe UI", 16, "bold")
FONT_HEAD   = ("Segoe UI", 13, "bold")
FONT_BODY   = ("Segoe UI", 12)
FONT_SMALL  = ("Segoe UI", 10)
FONT_MONO   = ("Consolas", 11)

# Log colours (Tk tag → hex)
LOG_COLORS = {
    "info":    TEXT,
    "success": TEXT_GREEN,
    "warning": TEXT_YELLOW,
    "error":   TEXT_RED,
    "debug":   TEXT_DIM,
    "gather":  "#80cbc4",
    "nav":     TEXT_BLUE,
}

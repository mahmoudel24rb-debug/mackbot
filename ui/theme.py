"""
Dark theme — Modern gaming-inspired palette (Jitsuri-style).
"""
import customtkinter as ctk
import config


def apply():
    ctk.set_appearance_mode(config.UI_THEME)
    ctk.set_default_color_theme("green")


# ─── Colour palette (modern dark blue-gray) ─────────────────────────────
BG          = "#0f1117"       # Main background
BG_PANEL    = "#161921"       # Sidebar / panels
BG_CARD     = "#1c1f2b"       # Cards
BG_CARD_ALT = "#252836"       # Card alt / inputs
BG_INPUT    = "#252836"
BG_HOVER    = "#2a2d3d"       # Hover state

ACCENT      = "#4ade80"       # Primary green (brighter, modern)
ACCENT_DIM  = "#22c55e"
ACCENT_HOVER = "#16a34a"
ACCENT_BG   = "#0d3320"       # Green background tint

BORDER      = "#2a2d3d"
BORDER_LIGHT = "#363a4d"

TEXT        = "#e2e8f0"       # Primary text
TEXT_DIM    = "#64748b"       # Muted text
TEXT_GREEN  = "#4ade80"
TEXT_RED    = "#f87171"
TEXT_YELLOW = "#fbbf24"
TEXT_BLUE   = "#60a5fa"
TEXT_ORANGE = "#fb923c"
TEXT_PINK   = "#f472b6"
TEXT_CYAN   = "#22d3ee"

# ─── Map cell colours ────────────────────────────────────────────────────
MAP_WALKABLE      = "#1a2e2e"   # Dark teal — walkable floor
MAP_WALKABLE_ALT  = "#1e3434"   # Slightly lighter for alternating rows
MAP_WALL          = "#0c0e14"   # Very dark — walls/obstacles
MAP_HOLE          = "#060810"   # Near-black — holes/void
MAP_CHANGE        = "#1d4ed8"   # Blue — map transition cells
MAP_CHANGE_DIM    = "#1e3a5f"   # Dim blue outline
MAP_RESOURCE_OK   = "#22c55e"   # Green — available resource
MAP_RESOURCE_USED = "#14532d"   # Dim green — used resource
MAP_CHAR          = "#facc15"   # Yellow — player position
MAP_CHAR_GLOW     = "#fde047"   # Bright yellow glow
MAP_MONSTER       = "#ef4444"   # Red — monster
MAP_NPC           = "#a78bfa"   # Purple — NPC
MAP_PLAYER        = "#38bdf8"   # Light blue — other players
MAP_GRID_LINE     = "#0f1219"   # Grid outline
MAP_CELL_TEXT     = "#3a4a5a"   # CellId number colour
MAP_CELL_TEXT_MC  = "#4a6a9a"   # CellId on map-change cells

# ─── Fonts ───────────────────────────────────────────────────────────────
FONT_TITLE  = ("Segoe UI", 18, "bold")
FONT_HEAD   = ("Segoe UI", 14, "bold")
FONT_BODY   = ("Segoe UI", 12)
FONT_SMALL  = ("Segoe UI", 10)
FONT_TINY   = ("Segoe UI", 8)
FONT_MONO   = ("Consolas", 11)
FONT_BADGE  = ("Segoe UI", 9, "bold")
FONT_CELL   = ("Consolas", 7)           # CellId text on map

# ─── Log colours ─────────────────────────────────────────────────────────
LOG_COLORS = {
    "info":    TEXT,
    "success": TEXT_GREEN,
    "warning": TEXT_YELLOW,
    "error":   TEXT_RED,
    "debug":   TEXT_DIM,
    "gather":  TEXT_CYAN,
    "nav":     TEXT_BLUE,
    "fight":   TEXT_RED,
}

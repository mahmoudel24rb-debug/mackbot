"""
Map View tab — 2D canvas showing the 560-cell isometric grid.

Cells are drawn as small diamonds. Colours:
  - walkable      → dark teal
  - blocked       → dark grey
  - map-change    → blue
  - character pos → yellow
  - resource      → bright green (available) / dim (used)
"""
import customtkinter as ctk
import tkinter as tk
from ui import theme


# Canvas size
CANVAS_W = 700
CANVAS_H = 480

# Cell size (diamond half-axes)
CELL_W = 22  # horizontal half-width
CELL_H = 11  # vertical half-height

# Grid dimensions (Dofus 3 standard map)
MAP_COLS = 14
MAP_ROWS = 40


def cell_to_canvas(cell_id: int):
    """Convert a cell_id (0-559) to canvas pixel coordinates (center)."""
    row = cell_id // MAP_COLS
    col = cell_id % MAP_COLS
    # Isometric offset: odd rows shifted right by half a cell
    x_offset = (row % 2) * CELL_W
    cx = 30 + col * CELL_W * 2 + x_offset + CELL_W
    cy = 20 + row * CELL_H + CELL_H
    return cx, cy


class MapViewTab(ctk.CTkFrame):
    def __init__(self, parent, **kwargs):
        super().__init__(parent, fg_color=theme.BG, **kwargs)
        self._cell_items: dict[int, int] = {}   # cell_id → canvas item id
        self._char_item: int | None = None
        self._build()

    def _build(self):
        self.grid_columnconfigure(0, weight=1)
        self.grid_rowconfigure(1, weight=1)

        ctk.CTkLabel(self, text="Carte", font=theme.FONT_TITLE,
                     text_color=theme.TEXT).grid(
            row=0, column=0, padx=20, pady=(16, 8), sticky="w")

        # Canvas inside a scrollable frame
        canvas_frame = ctk.CTkFrame(self, fg_color=theme.BG_PANEL,
                                    corner_radius=8)
        canvas_frame.grid(row=1, column=0, padx=12, pady=(0, 12),
                          sticky="nsew")

        self.canvas = tk.Canvas(canvas_frame, width=CANVAS_W, height=CANVAS_H,
                                bg=theme.BG_CARD, highlightthickness=0)
        self.canvas.pack(padx=8, pady=8)
        self.canvas.bind("<Button-1>", self._on_canvas_click)
        self._click_callback = None

        # Legend
        legend = ctk.CTkFrame(self, fg_color="transparent")
        legend.grid(row=2, column=0, padx=12, pady=(0, 8), sticky="w")
        self._legend_item(legend, "#1e5f5f", "Passable")
        self._legend_item(legend, "#555555", "Bloqué")
        self._legend_item(legend, "#1565c0", "Changement carte")
        self._legend_item(legend, "#ffd600", "Personnage")
        self._legend_item(legend, "#4caf50", "Ressource dispo")

    def _legend_item(self, parent, color, label):
        f = ctk.CTkFrame(parent, fg_color="transparent")
        f.pack(side="left", padx=8)
        ctk.CTkLabel(f, text="■", text_color=color,
                     font=theme.FONT_SMALL).pack(side="left")
        ctk.CTkLabel(f, text=label, font=theme.FONT_SMALL,
                     text_color=theme.TEXT_DIM).pack(side="left", padx=(2, 0))

    # ------------------------------------------------------------------
    # Map rendering
    # ------------------------------------------------------------------

    def render_map(self, walkable: set, map_change: set,
                   resources: list, char_cell: int = None):
        """
        Redraw the map canvas.

        Args:
            walkable:    set of walkable cell IDs
            map_change:  set of cell IDs that trigger a map change
            resources:   list of Resource objects (with .cell_id and .available)
            char_cell:   character's current cell ID
        """
        self.canvas.delete("all")
        self._cell_items.clear()
        self._char_item = None

        resource_cells_avail = {r.cell_id for r in resources if r.available}
        resource_cells_used  = {r.cell_id for r in resources if not r.available}

        for cell_id in range(MAP_COLS * MAP_ROWS):
            cx, cy = cell_to_canvas(cell_id)
            # Pick colour
            if cell_id in map_change:
                fill = "#1565c0"
            elif cell_id in resource_cells_avail:
                fill = "#4caf50"
            elif cell_id in resource_cells_used:
                fill = "#2e7d32"
            elif cell_id in walkable:
                fill = "#1e5f5f"
            else:
                fill = "#2a2a2a"

            item = self._draw_diamond(cx, cy, fill)
            self._cell_items[cell_id] = item

        if char_cell is not None:
            cx, cy = cell_to_canvas(char_cell)
            self._char_item = self._draw_diamond(cx, cy, "#ffd600")

    def _draw_diamond(self, cx, cy, fill):
        pts = [
            cx,          cy - CELL_H,
            cx + CELL_W, cy,
            cx,          cy + CELL_H,
            cx - CELL_W, cy,
        ]
        return self.canvas.create_polygon(pts, fill=fill,
                                          outline="#0a0a0a", width=1)

    def update_char_pos(self, cell_id: int):
        """Move the character marker without full redraw."""
        if self._char_item:
            self.canvas.delete(self._char_item)
        cx, cy = cell_to_canvas(cell_id)
        self._char_item = self._draw_diamond(cx, cy, "#ffd600")

    # ------------------------------------------------------------------
    # Click handler
    # ------------------------------------------------------------------

    def _on_canvas_click(self, event):
        # Reverse map: find nearest cell
        best_cell = None
        best_dist = float("inf")
        for cell_id in range(MAP_COLS * MAP_ROWS):
            cx, cy = cell_to_canvas(cell_id)
            dist = abs(event.x - cx) + abs(event.y - cy)
            if dist < best_dist:
                best_dist = dist
                best_cell = cell_id
        if best_cell is not None and best_dist < CELL_W + CELL_H:
            if self._click_callback:
                self._click_callback(best_cell)

    def on_cell_click(self, callback):
        """Register a callback called with (cell_id) when user clicks a cell."""
        self._click_callback = callback

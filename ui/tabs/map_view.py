"""
Map View — Jitsuri-style isometric grid renderer.

Renders the 560-cell diamond grid with real map geometry:
  - Walkable cells → dark teal diamonds with cellId number
  - Wall/obstacle cells → very dark (almost invisible, no cellId)
  - Map-change cells → blue tint with cellId
  - Character position → bright yellow diamond
  - Resources → green (available) / dim green (used)
  - Monsters → red dot, NPCs → purple dot, Players → blue dot
  - Hover highlight + info label
  - Click to move
"""
import customtkinter as ctk
import tkinter as tk
from ui import theme

# ─── Grid constants ──────────────────────────────────────────────────────
MAP_COLS = 14
MAP_ROWS = 40
CELL_COUNT = MAP_COLS * MAP_ROWS  # 560

# Diamond half-axes
CELL_W = 24    # horizontal half-width
CELL_H = 12    # vertical half-height

# Canvas margin
MARGIN_X = 50
MARGIN_Y = 28

# Computed canvas size
CANVAS_W = MAP_COLS * CELL_W * 2 + CELL_W + MARGIN_X * 2  # ~770
CANVAS_H = MAP_ROWS * CELL_H + CELL_H * 2 + MARGIN_Y * 2  # ~540


def cell_to_canvas(cell_id: int) -> tuple[float, float]:
    """Convert cellId (0-559) to canvas pixel center (cx, cy)."""
    row = cell_id // MAP_COLS
    col = cell_id % MAP_COLS
    # Isometric: odd rows shifted right by half a diamond
    x_offset = (row % 2) * CELL_W
    cx = MARGIN_X + col * CELL_W * 2 + x_offset + CELL_W
    cy = MARGIN_Y + row * CELL_H + CELL_H
    return cx, cy


def canvas_to_cell(px: float, py: float) -> int | None:
    """
    Reverse map: pixel coords → nearest cellId.
    Uses grid math for O(1) lookup instead of O(n) brute force.
    Returns None if click is outside any cell.
    """
    # Approximate row/col from pixel position
    approx_row = round((py - MARGIN_Y - CELL_H) / CELL_H)
    approx_row = max(0, min(MAP_ROWS - 1, approx_row))

    best_cell = None
    best_dist = CELL_W + CELL_H  # max threshold

    # Check a 3x3 neighborhood around the approximate position
    for dr in range(-2, 3):
        row = approx_row + dr
        if row < 0 or row >= MAP_ROWS:
            continue
        x_offset = (row % 2) * CELL_W
        approx_col = round((px - MARGIN_X - x_offset - CELL_W) / (CELL_W * 2))
        for dc in range(-2, 3):
            col = approx_col + dc
            if col < 0 or col >= MAP_COLS:
                continue
            cid = row * MAP_COLS + col
            if 0 <= cid < CELL_COUNT:
                cx, cy = cell_to_canvas(cid)
                # Manhattan distance in diamond space
                dist = abs(px - cx) / CELL_W + abs(py - cy) / CELL_H
                if dist < 1.0 and dist < best_dist:
                    best_dist = dist
                    best_cell = cid

    return best_cell


class MapViewTab(ctk.CTkFrame):
    """Map view tab with Jitsuri-style isometric grid rendering."""

    def __init__(self, parent, **kwargs):
        super().__init__(parent, fg_color=theme.BG, corner_radius=0, **kwargs)

        # ── Cell state ───────────────────────────────────────────────
        self._cell_polys: dict[int, int] = {}     # cellId → canvas polygon id
        self._cell_texts: dict[int, int] = {}     # cellId → canvas text id
        self._cell_fills: dict[int, str] = {}     # cellId → current fill color
        self._cell_types: dict[int, str] = {}     # cellId → type string

        self._char_poly: int | None = None
        self._entity_items: list[int] = []
        self._hover_cell: int | None = None
        self._click_callback = None

        # ── Cached map data ──────────────────────────────────────────
        self._last_walkable: set = set()
        self._last_map_change: dict = {}

        self._build()

    # ─── Build UI ────────────────────────────────────────────────────

    def _build(self):
        self.grid_columnconfigure(0, weight=1)
        self.grid_rowconfigure(1, weight=1)

        # ── Header bar ───────────────────────────────────────────────
        header = ctk.CTkFrame(self, fg_color="transparent")
        header.grid(row=0, column=0, padx=16, pady=(16, 4), sticky="ew")
        header.grid_columnconfigure(1, weight=1)

        ctk.CTkLabel(
            header, text="Carte",
            font=theme.FONT_TITLE, text_color=theme.TEXT,
        ).grid(row=0, column=0, sticky="w")

        # Cell info label (updated on hover)
        self._info_label = ctk.CTkLabel(
            header, text="",
            font=theme.FONT_SMALL, text_color=theme.TEXT_DIM,
        )
        self._info_label.grid(row=0, column=1, padx=16, sticky="e")

        # Map coordinates label
        self._coords_label = ctk.CTkLabel(
            header, text="",
            font=theme.FONT_BADGE,
            text_color=theme.ACCENT,
            fg_color=theme.ACCENT_BG,
            corner_radius=4,
        )
        self._coords_label.grid(row=0, column=2, sticky="e")

        # ── Canvas container ─────────────────────────────────────────
        canvas_frame = ctk.CTkFrame(
            self, fg_color=theme.BG_CARD, corner_radius=10,
        )
        canvas_frame.grid(row=1, column=0, padx=12, pady=(4, 4), sticky="nsew")

        self.canvas = tk.Canvas(
            canvas_frame,
            width=CANVAS_W, height=CANVAS_H,
            bg=theme.BG_CARD, highlightthickness=0,
            borderwidth=0,
        )
        self.canvas.pack(padx=8, pady=8, expand=True)

        # Bind events
        self.canvas.bind("<Motion>", self._on_motion)
        self.canvas.bind("<Button-1>", self._on_click)
        self.canvas.bind("<Leave>", self._on_leave)

        # ── Legend ───────────────────────────────────────────────────
        legend = ctk.CTkFrame(self, fg_color="transparent")
        legend.grid(row=2, column=0, padx=16, pady=(0, 8), sticky="w")

        legend_items = [
            (theme.MAP_WALKABLE, "Passable"),
            (theme.MAP_WALL, "Mur/Obstacle"),
            (theme.MAP_CHANGE, "Changement carte"),
            (theme.MAP_CHAR, "Personnage"),
            (theme.MAP_RESOURCE_OK, "Ressource dispo"),
            (theme.MAP_MONSTER, "Monstre"),
            (theme.MAP_NPC, "NPC"),
        ]
        for color, label in legend_items:
            self._legend_item(legend, color, label)

    def _legend_item(self, parent, color: str, label: str):
        f = ctk.CTkFrame(parent, fg_color="transparent")
        f.pack(side="left", padx=(0, 14))
        ctk.CTkLabel(
            f, text="■", text_color=color,
            font=theme.FONT_SMALL,
        ).pack(side="left")
        ctk.CTkLabel(
            f, text=label, font=theme.FONT_SMALL,
            text_color=theme.TEXT_DIM,
        ).pack(side="left", padx=(3, 0))

    # ─── Map rendering ───────────────────────────────────────────────

    def render_map(
        self,
        walkable: set,
        special_cells: dict | None,
        map_change: dict,
        resources: list,
        char_cell: int | None,
        entities: dict | None = None,
        map_coords: tuple | None = None,
        map_id: int | None = None,
    ):
        """
        Full map redraw with Jitsuri-style cell classification.

        Args:
            walkable:      set of walkable cellIds (cells NOT in KWW)
            special_cells: dict cellId → {flags → f1_value} from KWW (or None)
            map_change:    dict cellId → direction_flags
            resources:     list of Resource objects (with .cell_id, .available)
            char_cell:     character's current cellId
            entities:      dict entityId → Entity (with .cell_id, .entity_type)
            map_coords:    (x, y) map coordinates for header display
            map_id:        current map id for header display
        """
        self.canvas.delete("all")
        self._cell_polys.clear()
        self._cell_texts.clear()
        self._cell_fills.clear()
        self._cell_types.clear()
        self._char_poly = None
        self._entity_items.clear()
        self._hover_cell = None

        # Cache for hover restoration
        self._last_walkable = walkable
        self._last_map_change = map_change

        # Pre-compute resource cell sets
        res_avail = set()
        res_used = set()
        if resources:
            for r in resources:
                if r.cell_id is not None:
                    if r.available:
                        res_avail.add(r.cell_id)
                    else:
                        res_used.add(r.cell_id)

        # Update header labels
        if map_coords:
            self._coords_label.configure(
                text=f"  Map {map_id or '?'}  ({map_coords[0]}, {map_coords[1]})  ")
        elif map_id:
            self._coords_label.configure(text=f"  Map {map_id}  ")

        # ── Draw all 560 cells ───────────────────────────────────────
        for cell_id in range(CELL_COUNT):
            cx, cy = cell_to_canvas(cell_id)
            row = cell_id // MAP_COLS

            # Determine cell type and fill color
            if cell_id in map_change:
                fill = theme.MAP_CHANGE
                outline = theme.MAP_CHANGE_DIM
                cell_type = "map_change"
            elif cell_id in res_avail:
                fill = theme.MAP_RESOURCE_OK
                outline = theme.MAP_GRID_LINE
                cell_type = "resource"
            elif cell_id in res_used:
                fill = theme.MAP_RESOURCE_USED
                outline = theme.MAP_GRID_LINE
                cell_type = "resource_used"
            elif cell_id in walkable:
                # Alternate row tint for subtle depth
                fill = theme.MAP_WALKABLE if row % 2 == 0 else theme.MAP_WALKABLE_ALT
                outline = theme.MAP_GRID_LINE
                cell_type = "walkable"
            else:
                # Non-walkable: wall/obstacle/hole
                fill = theme.MAP_WALL
                outline = ""  # No outline for walls (they blend into void)
                cell_type = "wall"

            # Draw diamond polygon
            poly = self._draw_diamond(cx, cy, fill, outline)
            self._cell_polys[cell_id] = poly
            self._cell_fills[cell_id] = fill
            self._cell_types[cell_id] = cell_type

            # Draw cellId text (only on walkable / map-change / resource cells)
            if cell_type != "wall":
                text_color = theme.MAP_CELL_TEXT_MC if cell_type == "map_change" else theme.MAP_CELL_TEXT
                # Resources get brighter text
                if cell_type == "resource":
                    text_color = "#1a5c32"
                txt = self.canvas.create_text(
                    cx, cy,
                    text=str(cell_id),
                    font=theme.FONT_CELL,
                    fill=text_color,
                    anchor="center",
                )
                self._cell_texts[cell_id] = txt

        # ── Draw entities (monsters, NPCs, other players) ───────────
        if entities:
            for eid, entity in entities.items():
                if entity.cell_id is None:
                    continue
                cx, cy = cell_to_canvas(entity.cell_id)
                etype = getattr(entity, "entity_type", None)
                if etype == "monster":
                    color = theme.MAP_MONSTER
                elif etype == "npc":
                    color = theme.MAP_NPC
                else:
                    color = theme.MAP_PLAYER
                dot = self.canvas.create_oval(
                    cx - 4, cy - 4, cx + 4, cy + 4,
                    fill=color, outline="",
                )
                self._entity_items.append(dot)

        # ── Draw character position (on top of everything) ──────────
        if char_cell is not None and 0 <= char_cell < CELL_COUNT:
            cx, cy = cell_to_canvas(char_cell)
            # Glow effect: slightly larger diamond behind
            self._draw_diamond(cx, cy, theme.MAP_CHAR_GLOW, "",
                               w_scale=1.3, h_scale=1.3)
            self._char_poly = self._draw_diamond(
                cx, cy, theme.MAP_CHAR, theme.MAP_GRID_LINE)
            # Character cellId on top in dark text
            self.canvas.create_text(
                cx, cy, text=str(char_cell),
                font=theme.FONT_CELL,
                fill="#1a1a00",
                anchor="center",
            )

    def _draw_diamond(
        self, cx: float, cy: float,
        fill: str, outline: str = "",
        w_scale: float = 1.0, h_scale: float = 1.0,
    ) -> int:
        """Draw a diamond polygon centered at (cx, cy)."""
        w = CELL_W * w_scale
        h = CELL_H * h_scale
        pts = [
            cx,     cy - h,   # top
            cx + w, cy,       # right
            cx,     cy + h,   # bottom
            cx - w, cy,       # left
        ]
        return self.canvas.create_polygon(
            pts, fill=fill,
            outline=outline,
            width=0.5 if outline else 0,
        )

    # ─── Hover interaction ───────────────────────────────────────────

    def _on_motion(self, event):
        """Highlight cell under cursor and show info."""
        cell = canvas_to_cell(event.x, event.y)

        if cell == self._hover_cell:
            return

        # Restore previous cell
        if self._hover_cell is not None and self._hover_cell in self._cell_polys:
            orig = self._cell_fills.get(self._hover_cell, theme.MAP_WALL)
            self.canvas.itemconfigure(self._cell_polys[self._hover_cell], fill=orig)

        # Highlight new cell
        if cell is not None and cell in self._cell_polys:
            cell_type = self._cell_types.get(cell, "wall")
            if cell_type != "wall":
                # Lighten the fill slightly for hover
                self.canvas.itemconfigure(
                    self._cell_polys[cell], fill=theme.BG_HOVER)

        self._hover_cell = cell

        # Update info label
        if cell is not None:
            cell_type = self._cell_types.get(cell, "?")
            type_labels = {
                "walkable": "passable",
                "wall": "mur",
                "map_change": "changement carte",
                "resource": "ressource dispo",
                "resource_used": "ressource utilisée",
            }
            label = type_labels.get(cell_type, cell_type)
            self._info_label.configure(text=f"Cell {cell}  ·  {label}")
        else:
            self._info_label.configure(text="")

    def _on_leave(self, _event):
        """Reset hover when cursor leaves canvas."""
        if self._hover_cell is not None and self._hover_cell in self._cell_polys:
            orig = self._cell_fills.get(self._hover_cell, theme.MAP_WALL)
            self.canvas.itemconfigure(self._cell_polys[self._hover_cell], fill=orig)
        self._hover_cell = None
        self._info_label.configure(text="")

    # ─── Click interaction ───────────────────────────────────────────

    def _on_click(self, event):
        """Handle cell click — send move command."""
        cell = canvas_to_cell(event.x, event.y)
        if cell is not None and self._click_callback:
            cell_type = self._cell_types.get(cell, "wall")
            # Only allow clicking on walkable or map-change cells
            if cell_type in ("walkable", "map_change", "resource", "resource_used"):
                self._click_callback(cell)

    def on_cell_click(self, callback):
        """Register a callback called with (cell_id) when user clicks a cell."""
        self._click_callback = callback

    # ─── Partial updates (avoid full redraw) ─────────────────────────

    def update_char_pos(self, cell_id: int):
        """Move character marker without full redraw."""
        if self._char_poly:
            self.canvas.delete(self._char_poly)
        cx, cy = cell_to_canvas(cell_id)
        self._draw_diamond(cx, cy, theme.MAP_CHAR_GLOW, "",
                           w_scale=1.3, h_scale=1.3)
        self._char_poly = self._draw_diamond(
            cx, cy, theme.MAP_CHAR, theme.MAP_GRID_LINE)
        self.canvas.create_text(
            cx, cy, text=str(cell_id),
            font=theme.FONT_CELL,
            fill="#1a1a00", anchor="center",
        )

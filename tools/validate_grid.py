"""
Validation de la grille hexagonale Dofus 3.

HexGridHelper.cs (deobfuscated) confirms: 6-direction hex grid with offset rows.
Even rows: E(+1,0) NE(0,-1) NW(-1,-1) W(-1,0) SW(-1,+1) SE(0,+1)
Odd rows:  E(+1,0) NE(+1,-1) NW(0,-1) W(-1,0) SW(0,+1) SE(+1,+1)

Usage: python tools/validate_grid.py
Fill SNIFFED_MOVES with data from bot.py logs (MoveRequest keyCells).
"""

import sys, os
sys.path.insert(0, os.path.dirname(os.path.dirname(__file__)))

from game.map_grid import cell_to_point, point_to_cell, get_direction, get_neighbors

# Paste sniffed keyCells here: list of (cellId, direction) from MoveRequest logs.
# Example: MoveRequest cells=[323, 327, 356, 357] dirs=[0, 1, 0, 0]
# => [(323, 0), (327, 1), (356, 0), (357, 0)]
SNIFFED_MOVES = [
    # Session 2 data (from logs):
    # 174 -> 187 = +13 (delta)
    # 187 -> 173 = -14
    # 310 -> 323 = +13
    # 483 -> 497 = +14
    # Fill with real keyCells from your logs
]


def validate():
    """Verify get_direction() returns correct direction for each cell pair."""
    if not SNIFFED_MOVES:
        print("No sniffed moves to validate. Fill SNIFFED_MOVES with data from logs.")
        print("Format: [(cellId, direction), ...]")
        print()
        print("Quick delta analysis from known moves:")
        known_deltas = [(174, 187), (187, 173), (310, 323), (483, 497),
                        (302, 289), (351, 337), (320, 306)]
        for a, b in known_deltas:
            d = get_direction(a, b)
            pa, pb = cell_to_point(a), cell_to_point(b)
            print(f"  {a} -> {b} (delta={b-a:+d}): dir={d}, points={pa}->{pb}")
        return

    errors = 0
    for i in range(len(SNIFFED_MOVES) - 1):
        cell_a, dir_a = SNIFFED_MOVES[i]
        cell_b, _ = SNIFFED_MOVES[i + 1]

        computed_dir = get_direction(cell_a, cell_b)
        delta = cell_b - cell_a

        status = "OK" if computed_dir == dir_a else "FAIL"
        if computed_dir != dir_a:
            errors += 1

        pa = cell_to_point(cell_a)
        pb = cell_to_point(cell_b)
        print(f"  {cell_a} -> {cell_b} (delta={delta:+d}): "
              f"expected dir={dir_a}, got dir={computed_dir}, "
              f"points={pa}->{pb}  [{status}]")

    print(f"\n{'ALL GOOD' if errors == 0 else f'{errors} ERRORS'}")


def analyze_hex_width():
    """Try to determine hex grid WIDTH from known delta patterns."""
    print("=== Hex grid WIDTH analysis ===")
    # Known: E direction = +1 (always), so col = cellId % WIDTH
    # SE on even row = +WIDTH, NE on even row = -WIDTH
    # SE on odd row = +WIDTH+1, NE on odd row = -WIDTH+1

    for width in [14, 20, 28, 34, 40]:
        print(f"\nWIDTH={width}:")
        # Check cell 174 -> 187 (delta=+13)
        row_174 = 174 // width
        col_174 = 174 % width
        even_174 = row_174 % 2 == 0
        print(f"  174: row={row_174}, col={col_174}, {'even' if even_174 else 'odd'}")

        # delta +13: if even row, SE=+width => width=13? NE(odd)=-width+1 => width=14 (delta=-13)
        # if width=14: even row SE=+14, odd row NE=-14+1=-13, odd row SE=+14+1=+15
        # delta +13 with width=14: odd row, NE would be -13, not +13. SE odd = +15.
        # Hmm, +13 = width-1 = SE on even row if width=13... let's check

        for cell_a, cell_b in [(174, 187), (187, 173), (310, 323), (483, 497)]:
            delta = cell_b - cell_a
            row_a = cell_a // width
            even_a = row_a % 2 == 0
            print(f"  {cell_a}->{cell_b} (d={delta:+d}): row={row_a} {'even' if even_a else 'odd'}")


if __name__ == "__main__":
    validate()
    print()
    analyze_hex_width()

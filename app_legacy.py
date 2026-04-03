"""
DofusBot — Desktop application entry point.

Usage:
    python app.py

Requirements:
    pip install customtkinter lupa

The GUI runs on the main thread (Tkinter requirement).
The asyncio bot loop runs in a background thread.
Communication is via queue.Queue (bot→UI) and asyncio.Queue (UI→bot).
"""
import sys
import os

# Ensure project root is on sys.path
sys.path.insert(0, os.path.dirname(__file__))

from ui.app_window import AppWindow


def main():
    app = AppWindow()
    app.mainloop()


if __name__ == "__main__":
    main()

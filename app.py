"""
MackBot — Desktop application entry point (Web UI version).

Starts the bot backend (WinDivert + proxy + orchestrator + WebSocket server)
in a background thread, then opens the web frontend in a pywebview window.

Usage:
    python app.py

Requirements:
    pip install pywebview websockets
"""
import sys
import os
import asyncio
import threading
import time

# Ensure project root is on sys.path
sys.path.insert(0, os.path.dirname(__file__))

import config
from utils import logger


def start_bot_backend():
    """
    Start the bot backend in a background thread.
    Runs WinDivert + MITM proxy + WebSocket server.
    """
    loop = asyncio.new_event_loop()
    asyncio.set_event_loop(loop)

    try:
        loop.run_until_complete(_bot_main(loop))
    except Exception as e:
        logger.error(f"Bot backend crashed: {e}")
        import traceback
        traceback.print_exc()
    finally:
        loop.close()


async def _bot_main(loop):
    """Main async function for the bot backend."""
    from core.orchestrator import Orchestrator
    from core.event_bus import EventBus

    bus = EventBus()
    bus.set_loop(loop)

    orchestrator = Orchestrator(event_bus=bus)

    # Start everything (WinDivert, proxy, WebSocket server)
    await orchestrator.start()
    logger.info(f"Bot backend started. WebSocket on ws://localhost:{config.WS_PORT}")
    logger.info("Lancez Dofus...")

    # Keep running until interrupted
    try:
        while True:
            await asyncio.sleep(1)
    except asyncio.CancelledError:
        pass
    finally:
        await orchestrator.stop()


def start_electron():
    """Open the web frontend in an Electron window."""
    import subprocess

    # Wait for WebSocket server to be ready
    time.sleep(2)

    project_dir = os.path.dirname(os.path.abspath(__file__))
    npx = "npx.cmd" if sys.platform == "win32" else "npx"

    logger.info("Starting Electron window...")
    try:
        proc = subprocess.Popen(
            [npx, "electron", "."],
            cwd=project_dir,
            stdout=subprocess.PIPE,
            stderr=subprocess.PIPE,
        )
        proc.wait()  # Blocks until Electron window is closed
    except FileNotFoundError:
        logger.error("Electron not found. Run: npm install electron --save-dev")
        logger.error("Falling back to browser...")
        _fallback_browser()
    except Exception as e:
        logger.error(f"Electron failed: {e}")
        _fallback_browser()


def _fallback_browser():
    """Last resort: serve via HTTP and open browser."""
    import http.server
    import webbrowser
    import functools

    web_dir = os.path.join(os.path.dirname(__file__), 'web')
    port = 8080
    handler = functools.partial(http.server.SimpleHTTPRequestHandler, directory=web_dir)
    server = http.server.HTTPServer(('localhost', port), handler)
    logger.info(f"Web UI at http://localhost:{port}")
    webbrowser.open(f"http://localhost:{port}")
    try:
        server.serve_forever()
    except KeyboardInterrupt:
        server.shutdown()


def main():
    """Main entry point."""
    logger.info("=" * 50)
    logger.info("  MackBot — Starting...")
    logger.info("=" * 50)

    # Start bot backend in background thread
    bot_thread = threading.Thread(
        target=start_bot_backend,
        daemon=True,
        name="BotBackend",
    )
    bot_thread.start()

    # Open Electron window (blocks until closed)
    start_electron()

    logger.info("MackBot stopped.")


if __name__ == "__main__":
    main()

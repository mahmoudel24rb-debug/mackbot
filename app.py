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


def start_webview():
    """Open the web frontend in a pywebview window."""
    import webview

    # Wait for WebSocket server to be ready
    time.sleep(2)

    # Determine the path to web/index.html
    web_dir = os.path.join(os.path.dirname(__file__), 'web')
    index_path = os.path.join(web_dir, 'index.html')

    if not os.path.exists(index_path):
        logger.error(f"Frontend not found: {index_path}")
        logger.error("Make sure the web/ directory exists with index.html")
        sys.exit(1)

    # Create the window
    window = webview.create_window(
        title=config.UI_TITLE,
        url=index_path,
        width=config.UI_WIDTH,
        height=config.UI_HEIGHT,
        min_size=(960, 640),
        background_color='#0f1117',
        text_select=False,
    )

    # Start webview (blocks until window is closed)
    webview.start(
        debug=False,       # Set True for dev tools (F12)
        http_server=True,  # Serve files via HTTP (needed for JS/assets)
    )


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

    # Open web UI (blocks until window closed)
    start_webview()

    logger.info("MackBot stopped.")


if __name__ == "__main__":
    main()

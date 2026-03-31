"""
Async Event Bus — lightweight pub/sub for decoupled communication.

Event namespaces:
  game.*   — game state events (connected, map.changed, gather.completed, ...)
  bot.*    — bot lifecycle events (started, stopped, log, route.step)
  ui.*     — UI-driven commands (start_bot, stop_bot, script_selected)

Usage:
    bus = EventBus()

    # Subscribe
    @bus.on("map.changed")
    async def on_map(data):
        print(f"New map: {data['map_id']}")

    # Publish (from sync code)
    bus.emit("map.changed", {"map_id": 123})

    # Publish (from async code)
    await bus.emit_async("map.changed", {"map_id": 123})
"""

import asyncio
from collections import defaultdict
from utils import logger


class EventBus:
    """Thread-safe async event bus."""

    def __init__(self):
        # event_name -> list of async callbacks
        self._listeners: dict[str, list] = defaultdict(list)
        self._loop: asyncio.AbstractEventLoop | None = None

    # ------------------------------------------------------------------
    # Registration
    # ------------------------------------------------------------------

    def on(self, event: str):
        """Decorator: register an async handler for an event."""
        def decorator(fn):
            self._listeners[event].append(fn)
            return fn
        return decorator

    def subscribe(self, event: str, handler):
        """Register a callback (sync or async) for an event."""
        self._listeners[event].append(handler)

    def unsubscribe(self, event: str, handler):
        """Remove a previously registered callback."""
        lst = self._listeners.get(event, [])
        if handler in lst:
            lst.remove(handler)

    # ------------------------------------------------------------------
    # Publishing
    # ------------------------------------------------------------------

    def emit(self, event: str, data=None):
        """
        Fire event from sync or async context.
        Schedules async handlers on the running loop (non-blocking).
        """
        handlers = list(self._listeners.get(event, []))
        if not handlers:
            return

        loop = self._get_loop()
        if loop and loop.is_running():
            for handler in handlers:
                if asyncio.iscoroutinefunction(handler):
                    asyncio.run_coroutine_threadsafe(handler(data), loop)
                else:
                    loop.call_soon_threadsafe(handler, data)
        else:
            # No running loop — call sync handlers directly
            for handler in handlers:
                if not asyncio.iscoroutinefunction(handler):
                    try:
                        handler(data)
                    except Exception as e:
                        logger.error(f"[BUS] Error in handler for {event}: {e}")

    async def emit_async(self, event: str, data=None):
        """Fire event from an async context — await all handlers."""
        handlers = list(self._listeners.get(event, []))
        for handler in handlers:
            try:
                if asyncio.iscoroutinefunction(handler):
                    await handler(data)
                else:
                    handler(data)
            except Exception as e:
                logger.error(f"[BUS] Error in async handler for {event}: {e}")

    # ------------------------------------------------------------------
    # Helpers
    # ------------------------------------------------------------------

    def set_loop(self, loop: asyncio.AbstractEventLoop):
        """Register the asyncio event loop (called once at startup)."""
        self._loop = loop

    def _get_loop(self) -> asyncio.AbstractEventLoop | None:
        if self._loop:
            return self._loop
        try:
            return asyncio.get_running_loop()
        except RuntimeError:
            return None

    def clear(self, event: str = None):
        """Remove all listeners for one event (or all events if None)."""
        if event:
            self._listeners.pop(event, None)
        else:
            self._listeners.clear()

    def __repr__(self):
        total = sum(len(v) for v in self._listeners.values())
        return f"EventBus(events={len(self._listeners)}, handlers={total})"

"""
WebSocket server — bridges bot backend to web frontend.

Runs in the bot's asyncio event loop.
Broadcasts game events as JSON to all connected clients.
Receives commands from the web UI and forwards to the orchestrator.

Message format (backend -> frontend):
{
    "type": "CharacterData" | "MapInformation" | "Log" | "Status" | ...,
    "payload": { ... },
    "processId": 1234  (optional)
}

Message format (frontend -> backend):
{
    "action": "loadLua" | "stopLua" | "moveTo" | "startGathering" | ...,
    "data": { ... }
}
"""
import asyncio
import json
import logging
import websockets

logger = logging.getLogger("ws_server")


class BotWebSocketServer:
    """WebSocket server for bot <-> web UI communication."""

    def __init__(self, host="localhost", port=7777):
        self.host = host
        self.port = port
        self.clients: set = set()
        self.server = None
        self._command_handler = None

    def set_command_handler(self, handler):
        """
        Register a callback for processing UI commands.
        handler signature: async def handler(action: str, data: dict)
        """
        self._command_handler = handler

    async def start(self):
        """Start the WebSocket server."""
        self.server = await websockets.serve(
            self._on_connect,
            self.host,
            self.port,
            ping_interval=30,
            ping_timeout=10,
        )
        logger.info(f"WebSocket server started on ws://{self.host}:{self.port}")

    async def stop(self):
        """Stop the WebSocket server."""
        if self.server:
            self.server.close()
            await self.server.wait_closed()
            logger.info("WebSocket server stopped")

    async def _on_connect(self, websocket):
        """Handle a new client connection."""
        self.clients.add(websocket)
        remote = websocket.remote_address
        logger.info(f"Client connected: {remote}")

        try:
            async for raw_message in websocket:
                try:
                    message = json.loads(raw_message)
                    action = message.get("action", "")
                    data = message.get("data", {})
                    if self._command_handler:
                        await self._command_handler(action, data)
                    else:
                        logger.warning(f"No command handler set, ignoring: {action}")
                except json.JSONDecodeError:
                    logger.error(f"Invalid JSON from client: {raw_message[:100]}")
                except Exception as e:
                    logger.error(f"Error processing command: {e}")
        except websockets.exceptions.ConnectionClosed:
            pass
        finally:
            self.clients.discard(websocket)
            logger.info(f"Client disconnected: {remote}")

    async def broadcast(self, msg_type: str, payload: dict, process_id: int = None):
        """Send a message to ALL connected web clients."""
        if not self.clients:
            return

        msg = json.dumps({
            "type": msg_type,
            "payload": payload,
            "processId": process_id,
        }, default=str)

        dead = set()
        for ws in self.clients:
            try:
                await ws.send(msg)
            except websockets.exceptions.ConnectionClosed:
                dead.add(ws)
            except Exception as e:
                logger.error(f"Error sending to client: {e}")
                dead.add(ws)

        self.clients -= dead

    async def send_log(self, text: str, level: str = "info", process_id: int = None):
        """Shortcut to broadcast a log message."""
        await self.broadcast("Log", {
            "type": level,
            "message": text,
            "color": {
                "info": "#e2e8f0",
                "success": "#4ade80",
                "warning": "#fbbf24",
                "error": "#f87171",
                "debug": "#64748b",
                "gather": "#22d3ee",
                "nav": "#60a5fa",
                "fight": "#f87171",
            }.get(level, "#e2e8f0"),
        }, process_id)

    @property
    def client_count(self):
        return len(self.clients)

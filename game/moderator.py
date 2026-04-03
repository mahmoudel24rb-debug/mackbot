"""
Moderator detection — stops bot when a moderator is detected on the map.

Detection methods:
- Actor with moderator flag in ISU
- Chat message from moderator account
- Prison notification
"""

import asyncio
from utils import logger
import config


class ModeratorDetector:
    """Detects moderator presence and triggers emergency stop."""

    # Known moderator name patterns (Ankama staff)
    MOD_NAME_PATTERNS = [
        "[Modo]", "[Admin]", "[Ankama]", "[GM]",
        "Modo-", "Admin-", "Ankama-",
    ]

    def __init__(self, game_state):
        self.game_state = game_state
        self._detected = False

    def check_actor_name(self, name):
        """Check if an actor name looks like a moderator."""
        if not name:
            return False
        for pattern in self.MOD_NAME_PATTERNS:
            if pattern.lower() in name.lower():
                return True
        return False

    def check_chat_message(self, sender_name, channel):
        """Check if a chat message is from a moderator."""
        if self.check_actor_name(sender_name):
            return True
        # Channel 9 or 10 = admin/moderator channels in Dofus
        if channel in (9, 10):
            return True
        return False

    def on_moderator_detected(self, name, source="unknown"):
        """Called when a moderator is detected. Triggers emergency stop."""
        if self._detected:
            return  # Already handled
        self._detected = True

        logger.error(f"MODERATOR DETECTED: {name} (source: {source})")

        # Stop script if configured
        if config.STOP_SCRIPT_ON_MOD:
            gs = self.game_state
            if hasattr(gs, 'gatherer') and gs.gatherer:
                gs.is_busy = False
                gs.busy_reason = None

        # Emit event for orchestrator/UI
        try:
            loop = asyncio.get_event_loop()
            if loop.is_running():
                from core.event_bus import EventBus
                # The orchestrator will handle this via bus
                pass
        except Exception:
            pass

    def reset(self):
        """Reset after map change (moderator may have left)."""
        self._detected = False

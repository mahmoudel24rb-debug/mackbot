"""
Private message handler — send and receive PMs.
Stub implementation — needs protocol codes from sniffer.
"""

import time
from utils import logger


class PrivateMessageHandler:
    def __init__(self, game_state):
        self.game_state = game_state
        self.conversations = {}  # characterId -> [messages]

    def handle_received(self, sender_id, sender_name, content):
        """Handle an incoming private message."""
        if sender_id not in self.conversations:
            self.conversations[sender_id] = []
        self.conversations[sender_id].append({
            "sender": sender_name,
            "content": content,
            "fromMe": False,
            "timestamp": time.time(),
        })
        logger.info(f"[PM] From {sender_name}: {content}")

    async def send_message(self, character_name, content):
        """Send a private message to a character."""
        logger.info(f"[PM] To {character_name}: {content}")
        # TODO: Construct and send the protobuf message once code is identified

    def get_conversation(self, character_id):
        """Get all messages with a character."""
        return self.conversations.get(character_id, [])

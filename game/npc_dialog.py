"""
NPC Dialog handler — open dialogs, select replies, close.
Used by treasure hunt, quests, and zaap navigation.

Stub implementation — handlers need to be wired when protocol codes
for NPC dialogs are identified via sniffer.
"""

from utils import logger


class NpcDialog:
    def __init__(self, game_state):
        self.game_state = game_state
        self.current_npc_id = None
        self.available_replies = []

    async def open_dialog(self, npc_element_id):
        """Send an interaction request to open NPC dialog."""
        logger.info(f"[NPC] Opening dialog with NPC {npc_element_id}")
        # TODO: Send the appropriate protobuf message once code is identified
        self.current_npc_id = npc_element_id

    async def select_reply(self, reply_id):
        """Select a dialog reply option."""
        logger.info(f"[NPC] Selecting reply {reply_id}")
        # TODO: Send reply selection packet

    async def close_dialog(self):
        """Close the current NPC dialog."""
        logger.info(f"[NPC] Closing dialog")
        self.current_npc_id = None
        self.available_replies = []

    def on_dialog_opened(self, npc_id, replies):
        """Called by message handler when NPC dialog opens."""
        self.current_npc_id = npc_id
        self.available_replies = replies
        logger.info(f"[NPC] Dialog opened: NPC {npc_id}, {len(replies)} replies")

    def on_dialog_closed(self):
        """Called by message handler when dialog closes."""
        self.current_npc_id = None
        self.available_replies = []

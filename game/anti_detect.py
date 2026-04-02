"""
Anti-detection measures for the MITM bot.
Adds realistic human-like delays and variance.
"""

import asyncio
import random


class HumanDelay:
    """Simulates realistic human delays."""

    CLICK_DELAY = (0.1, 0.3)
    MOVE_DELAY = (0.3, 0.8)
    GATHER_DELAY = (0.5, 1.5)
    MAP_CHANGE_DELAY = (1.0, 3.0)
    IDLE_CHANCE = 0.05
    IDLE_DELAY = (3.0, 8.0)

    @staticmethod
    async def wait(delay_type="click"):
        delays = {
            "click": HumanDelay.CLICK_DELAY,
            "move": HumanDelay.MOVE_DELAY,
            "gather": HumanDelay.GATHER_DELAY,
            "map_change": HumanDelay.MAP_CHANGE_DELAY,
        }

        lo, hi = delays.get(delay_type, HumanDelay.CLICK_DELAY)

        if random.random() < HumanDelay.IDLE_CHANCE:
            lo, hi = HumanDelay.IDLE_DELAY

        delay = random.uniform(lo, hi)
        await asyncio.sleep(delay)
        return delay

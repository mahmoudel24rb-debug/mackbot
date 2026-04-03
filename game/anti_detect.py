"""
Anti-detection measures for the MITM bot.
Gaussian delays, random pauses, resource picking variance.
"""

import asyncio
import random
import logging

logger = logging.getLogger("anti_detect")


def human_delay(min_delay, max_delay):
    """Generate a human-like random delay (gaussian distribution)."""
    mean = (min_delay + max_delay) / 2
    std = (max_delay - min_delay) / 4
    delay = random.gauss(mean, std)
    return max(min_delay, min(max_delay, delay))


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

        # Occasionally pause longer (simulates distracted player)
        if random.random() < HumanDelay.IDLE_CHANCE:
            lo, hi = HumanDelay.IDLE_DELAY

        delay = human_delay(lo, hi)
        await asyncio.sleep(delay)
        return delay


async def maybe_pause():
    """Occasionally insert a human-like pause."""
    roll = random.random()
    if roll < 0.02:
        pause = random.uniform(3, 8)
        logger.debug(f"Human pause: {pause:.1f}s")
        await asyncio.sleep(pause)
    elif roll < 0.10:
        pause = random.uniform(0.5, 2)
        await asyncio.sleep(pause)


def pick_resource(resources, current_cell=None):
    """Pick a resource with some randomness (not always the closest)."""
    if not resources:
        return None
    if len(resources) == 1:
        return resources[0]

    # 80% closest, 20% random
    if random.random() < 0.8 and current_cell is not None:
        from game.map_grid import cell_distance
        return min(resources, key=lambda r: cell_distance(current_cell, r.cell_id)
                   if r.cell_id is not None else 9999)
    else:
        return random.choice(resources)

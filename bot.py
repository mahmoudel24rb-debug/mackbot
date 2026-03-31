"""
Dofus 3 Bot - WinDivert redirect + MITM proxy.

Intercepts game connections at the OS level and redirects through our proxy,
enabling packet injection for bot control while Jitsuri handles game launch.

Flow:
  1. Start this bot (requires Administrator)
  2. Connect to game via Jitsuri (or reconnect if already in-game)
  3. WinDivert intercepts all TCP traffic on port 5555
  4. Our proxy decodes, forwards, and enables console commands

Usage:
  python bot.py                # Auto-detect game server
  python bot.py --server IP    # Manual server IP
"""

import argparse
import asyncio
import re
import subprocess
import sys
import os

sys.path.insert(0, os.path.dirname(__file__))

from utils import logger
import config


def resolve_game_server():
    """Resolve game server IP via DNS."""
    try:
        result = subprocess.run(
            ["nslookup", config.SERVER_HOSTNAME],
            capture_output=True, text=True, timeout=10,
            encoding='utf-8', errors='replace',
        )
        lines = result.stdout.split("\n")
        in_answer = False
        for line in lines:
            if "nom :" in line.lower() or "name:" in line.lower():
                in_answer = True
                continue
            if in_answer:
                match = re.search(r"(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})", line)
                if match:
                    return match.group(1)
    except Exception:
        pass
    return None


def detect_active_servers():
    """Detect game server IPs from active connections on port 5555."""
    ips = set()
    try:
        result = subprocess.run(
            ["powershell", "-Command",
             "Get-NetTCPConnection -State Established -RemotePort 5555 "
             "| Select-Object -ExpandProperty RemoteAddress"],
            capture_output=True, text=True, timeout=10,
            encoding='utf-8', errors='replace',
        )
        for line in result.stdout.strip().split('\n'):
            line = line.strip()
            if line and not line.startswith('127.'):
                ips.add(line)
    except Exception:
        pass
    return list(ips)


async def run_bot(server_ip=None):
    from launcher.packet_redirect import PacketRedirect, PROXY_UPSTREAM_SRC_PORT
    from proxy.mitm_proxy import MITMProxy

    logger.banner()

    # --- Step 1: Detect game server ---
    login_server_ip = resolve_game_server()

    if server_ip:
        game_server_ip = server_ip
        logger.info(f"Game server: {game_server_ip} (manual)")
    else:
        logger.info("Detecting game server...")
        active = detect_active_servers()
        if active:
            game_server_ip = active[0]
            logger.info(f"  Active connection: {game_server_ip}")
        elif login_server_ip:
            game_server_ip = login_server_ip
            logger.info(f"  DNS resolved: {game_server_ip}")
        else:
            logger.error("Cannot detect game server! Use --server IP")
            return

    # Use DNS IP for login routing, detected IP for game routing
    effective_login_ip = login_server_ip or game_server_ip

    print()
    logger.info("Configuration:")
    logger.info(f"  Proxy:       127.0.0.1:{config.PROXY_PORT}")
    logger.info(f"  Login:       {effective_login_ip}:{config.SERVER_PORT}")
    logger.info(f"  Game:        {game_server_ip}:{config.SERVER_PORT}")
    logger.info(f"  Upstream:    src_port={PROXY_UPSTREAM_SRC_PORT}")
    print()

    # --- Step 2: Setup proxy ---
    proxy = MITMProxy(
        listen_host="127.0.0.1",
        listen_port=config.PROXY_PORT,
        server_host=effective_login_ip,
        server_port=config.SERVER_PORT,
    )
    # Pre-set game server for routing (handles case where Jitsuri is already connected)
    proxy.game_server_host = game_server_ip
    proxy.game_server_port = config.SERVER_PORT
    # Set upstream src port so WinDivert excludes our proxy's outbound connections
    proxy.upstream_src_port = PROXY_UPSTREAM_SRC_PORT

    # --- Step 3: Start WinDivert redirect ---
    logger.info("Starting WinDivert redirect...")
    redirect = PacketRedirect(
        game_port=config.SERVER_PORT,
        proxy_port=config.PROXY_PORT,
    )
    try:
        redirect.start()
    except Exception as e:
        logger.error(f"WinDivert failed: {e}")
        logger.error("Run as Administrator!")
        return

    print()
    logger.info("Bot ready! WinDivert active — play Dofus normally.")
    logger.info("WinDivert intercepts all TCP on port 5555.")
    logger.info("Type 'help' for console commands.")
    print()

    # --- Step 4: Run proxy + console ---
    try:
        await asyncio.gather(
            proxy.start(),
            proxy.console_loop(),
        )
    except KeyboardInterrupt:
        pass
    finally:
        redirect.stop()
        logger.info("Bot stopped.")


if __name__ == "__main__":
    parser = argparse.ArgumentParser(
        description="Dofus 3 Bot - WinDivert redirect + MITM proxy",
    )
    parser.add_argument("--server", "-s", default=None, help="Game server IP")
    args = parser.parse_args()

    try:
        asyncio.run(run_bot(args.server))
    except KeyboardInterrupt:
        print("\nBye!")

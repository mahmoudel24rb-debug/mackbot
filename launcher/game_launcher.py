"""
Game Launcher - Orchestrates Fake Launcher + Winsock Hook + MITM Proxy.

Approach: Hook ws2_32.dll!connect in Dofus.exe to redirect game server
connections to 127.0.0.1 (our MITM proxy). Same technique as Jitsuri's
HookingPlusPlus but implemented in pure Python.

Full flow:
    1. Resolve real server IP (via DNS)
    2. Patch zaap-start.bat: ZAAP_PORT=26666
    3. Start Fake Launcher (transparent IPC proxy on port 26666)
    4. Start MITM Proxy on port 5555
    5. User clicks "Jouer" in Ankama Launcher
    6. Dofus starts -> Fake Launcher intercepts IPC (token, account)
    7. Winsock hook redirects game connection to 127.0.0.1:5555
    8. MITM Proxy forwards to real game server

Usage:
    python -m launcher.game_launcher

Requires admin (for OpenProcess on Dofus.exe).
"""

import asyncio
import os
import re
import socket
import subprocess

from utils import logger
import config

# Paths
DOFUS_DIR = os.path.join(
    os.environ.get("LOCALAPPDATA", ""),
    "Ankama", "Dofus-dofus3",
)
ZAAP_START_BAT = os.path.join(DOFUS_DIR, "zaap-start.bat")


# ---------------------------------------------------------------------------
# zaap-start.bat patching (ZAAP_PORT only - no configUrl needed)
# ---------------------------------------------------------------------------

def patch_zaap_start(port: int = None) -> bool:
    """Patch zaap-start.bat: set ZAAP_PORT for fake launcher."""
    port = port or config.FAKE_LAUNCHER_PORT
    try:
        with open(ZAAP_START_BAT, "r", encoding="utf-8") as f:
            content = f.read()

        if f"set ZAAP_PORT={port}" in content:
            logger.info(f"  zaap-start.bat already patched (ZAAP_PORT={port})")
            return True

        content = content.replace(
            'Dofus.exe',
            f'set ZAAP_PORT={port}\nDofus.exe',
            1,
        )

        with open(ZAAP_START_BAT, "w", encoding="utf-8") as f:
            f.write(content)

        logger.info(f"  zaap-start.bat patched: ZAAP_PORT={port}")
        return True
    except Exception as e:
        logger.error(f"  Failed to patch zaap-start.bat: {e}")
        return False


def unpatch_zaap_start() -> bool:
    """Remove ZAAP_PORT patch from zaap-start.bat."""
    try:
        with open(ZAAP_START_BAT, "r", encoding="utf-8") as f:
            content = f.read()

        lines = content.split("\n")
        cleaned = [l for l in lines if not l.strip().startswith("set ZAAP_PORT=")]

        # Also restore configUrl if it was modified by previous versions
        restored = "\n".join(cleaned)
        restored = re.sub(
            r'--configUrl\s+(http://127\.0\.0\.1:\d+/\S+|file:///\S+)',
            f'--configUrl {config.ORIGINAL_CONFIG_URL}',
            restored,
        )

        with open(ZAAP_START_BAT, "w", encoding="utf-8") as f:
            f.write(restored)

        logger.info("  zaap-start.bat restored")
        return True
    except Exception as e:
        logger.error(f"  Failed to restore zaap-start.bat: {e}")
        return False


# ---------------------------------------------------------------------------
# Server IP resolution
# ---------------------------------------------------------------------------

def resolve_server_ip() -> str | None:
    """Resolve real game server IP via nslookup."""
    try:
        result = subprocess.run(
            ["nslookup", config.SERVER_HOSTNAME],
            capture_output=True, timeout=10,
        )
        stdout = result.stdout.decode("utf-8", errors="replace")
        lines = stdout.split("\n")
        in_answer = False
        for line in lines:
            if "nom :" in line.lower() or "name:" in line.lower():
                in_answer = True
                continue
            if in_answer:
                match = re.search(r"(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})", line)
                if match:
                    ip = match.group(1)
                    if ip != "127.0.0.1":
                        return ip
    except Exception as e:
        logger.warn(f"  nslookup failed: {e}")

    try:
        ip = socket.gethostbyname(config.SERVER_HOSTNAME)
        if ip != "127.0.0.1":
            return ip
    except Exception as e:
        logger.warn(f"  socket resolution failed: {e}")

    return None


# ---------------------------------------------------------------------------
# Main orchestrator
# ---------------------------------------------------------------------------

async def run_full():
    """Run the full bot launch flow."""
    from launcher.fake_launcher import FakeLauncher
    from launcher.winsock_hook import install_hooks, find_dofus_pid, verify_hooks_still_active, read_hook_counters
    from launcher.packet_redirect import PacketRedirect, PROXY_UPSTREAM_SRC_PORT
    from proxy.mitm_proxy import MITMProxy

    fake_launcher = None
    mitm_proxy = None
    packet_redirect = None

    # Step 1: Resolve real server IP
    logger.info("Resolving game server IP...")
    server_ip = resolve_server_ip()
    if not server_ip:
        logger.error("Cannot resolve game server IP!")
        return
    logger.info(f"  Server: {server_ip}")
    print()

    # Step 2: Patch zaap-start.bat (ZAAP_PORT only)
    logger.info("Patching zaap-start.bat...")
    if not patch_zaap_start(config.FAKE_LAUNCHER_PORT):
        return
    print()

    try:
        # Step 3: Start Fake Launcher
        logger.info("Starting Fake Launcher...")
        fake_launcher = FakeLauncher(
            listen_port=config.FAKE_LAUNCHER_PORT,
            launcher_port=config.LAUNCHER_IPC_PORT,
        )
        await fake_launcher.start()
        print()

        # Step 4: Start MITM Proxy (non-blocking)
        logger.info("Starting MITM Proxy...")
        mitm_proxy = MITMProxy(
            listen_host="0.0.0.0",
            listen_port=config.PROXY_PORT,
            server_host=server_ip,
            server_port=config.SERVER_PORT,
        )
        # Set upstream source port so WinDivert can exclude proxy's own traffic
        mitm_proxy.upstream_src_port = PROXY_UPSTREAM_SRC_PORT
        await mitm_proxy.start(blocking=False)
        print()

        # Step 5: Start WinDivert packet redirect
        logger.info("Starting WinDivert packet redirect...")
        try:
            packet_redirect = PacketRedirect(
                game_port=config.SERVER_PORT,
                proxy_port=config.PROXY_PORT,
            )
            packet_redirect.start()
            logger.info("")
            logger.info("=" * 55)
            logger.info("  All services ready!")
            logger.info(f"  Launcher:  127.0.0.1:{config.FAKE_LAUNCHER_PORT} -> 127.0.0.1:{config.LAUNCHER_IPC_PORT}")
            logger.info(f"  Proxy:     0.0.0.0:{config.PROXY_PORT} -> {server_ip}:{config.SERVER_PORT}")
            logger.info(f"  Redirect:  WinDivert (port {config.SERVER_PORT} -> 127.0.0.1)")
            logger.info("")
            logger.info("  Click 'Jouer' in Ankama Launcher now!")
            logger.info("=" * 55)
        except Exception as e:
            logger.warn(f"  WinDivert failed: {e}")
            logger.warn("  Falling back to Winsock hooks only")
            packet_redirect = None
            logger.info("")
            logger.info("=" * 55)
            logger.info("  Services ready (hook mode)!")
            logger.info(f"  Launcher:  127.0.0.1:{config.FAKE_LAUNCHER_PORT} -> 127.0.0.1:{config.LAUNCHER_IPC_PORT}")
            logger.info(f"  Proxy:     0.0.0.0:{config.PROXY_PORT} -> {server_ip}:{config.SERVER_PORT}")
            logger.info("")
            logger.info("  Click 'Jouer' in Ankama Launcher now!")
            logger.info("  Winsock hook will be injected when Dofus starts.")
            logger.info("=" * 55)
        print()

        # Step 6: Also inject Winsock hooks (for diagnostics + double coverage)
        hook_installed = asyncio.Event()

        async def wait_for_dofus_and_hook():
            """Background task: detect Dofus.exe and inject Winsock hooks."""
            loop = asyncio.get_event_loop()
            while not hook_installed.is_set():
                pid = await loop.run_in_executor(None, find_dofus_pid)
                if pid:
                    logger.info(f"Dofus.exe detected (PID={pid}), injecting hooks...")
                    await asyncio.sleep(1.0)
                    ok = await loop.run_in_executor(
                        None, install_hooks, pid, "127.0.0.1", config.PROXY_PORT,
                    )
                    if ok:
                        logger.info(f"  Winsock hooks installed (diagnostic counters active)")

                        # Read counters after 15s to see if hooks fire
                        await asyncio.sleep(15)
                        counters = await loop.run_in_executor(
                            None, read_hook_counters, pid,
                        )
                        for func_name, count in counters.items():
                            status = "CALLED" if count > 0 else "NOT CALLED"
                            logger.info(f"  COUNTER {func_name}: {count} calls ({status})")

                        any_called = any(c > 0 for c in counters.values())
                        if not any_called:
                            logger.info("  Hooks not called - WinDivert redirect is handling traffic")
                        else:
                            logger.info("  Hooks ARE being called!")
                    hook_installed.set()
                    return
                await asyncio.sleep(0.5)

        hook_task = asyncio.create_task(wait_for_dofus_and_hook())

        # Console loop for bot commands
        await mitm_proxy.console_loop()

        hook_task.cancel()

    except asyncio.CancelledError:
        pass
    finally:
        print()
        logger.info("Cleaning up...")
        unpatch_zaap_start()
        if packet_redirect:
            packet_redirect.stop()
        if fake_launcher:
            await fake_launcher.stop()
        if mitm_proxy:
            await mitm_proxy.stop()
        logger.info("Done!")


def main():
    import ctypes

    logger.banner()
    logger.info("Mode: WinDivert + Fake Launcher + MITM Proxy")

    # Check admin
    try:
        is_admin = ctypes.windll.shell32.IsUserAnAdmin() != 0
    except Exception:
        is_admin = False

    if not is_admin:
        logger.warn("  Not running as admin - hook injection may fail!")
        logger.warn("  Right-click terminal -> Run as Administrator")
    else:
        logger.info("  Running as Administrator")
    print()

    try:
        asyncio.run(run_full())
    except KeyboardInterrupt:
        print()
        logger.info("Cleaning up...")
        unpatch_zaap_start()
        logger.info("Stopped (Ctrl+C)")


if __name__ == "__main__":
    main()

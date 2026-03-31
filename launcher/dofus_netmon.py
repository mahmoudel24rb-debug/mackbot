"""
Real-time network monitor for Dofus.exe.
Shows ALL TCP connections the game makes, to understand where traffic goes.

Usage: python -m launcher.dofus_netmon
Run this in a SEPARATE terminal while launching the game (via our launcher or Jitsuri).
"""

import subprocess
import time
import sys

def get_dofus_connections():
    """Get all TCP connections from Dofus.exe processes."""
    try:
        # Get Dofus PIDs
        result = subprocess.run(
            ["powershell", "-NoProfile", "-Command",
             "(Get-Process -Name Dofus -ErrorAction SilentlyContinue).Id"],
            capture_output=True, timeout=3,
        )
        stdout = result.stdout.decode("utf-8", errors="replace").strip()
        if not stdout:
            return None, []

        pids = set()
        for line in stdout.split("\n"):
            line = line.strip()
            if line.isdigit():
                pids.add(line)

        if not pids:
            return None, []

        # Get all TCP connections
        result = subprocess.run(
            ["netstat", "-ano", "-p", "tcp"],
            capture_output=True, timeout=5,
        )
        stdout = result.stdout.decode("utf-8", errors="replace")

        connections = []
        for line in stdout.split("\n"):
            line = line.strip()
            parts = line.split()
            if len(parts) >= 5 and parts[-1] in pids:
                proto = parts[0]
                local = parts[1]
                remote = parts[2]
                state = parts[3]
                pid = parts[4]
                connections.append({
                    "local": local,
                    "remote": remote,
                    "state": state,
                    "pid": pid,
                })

        return pids, connections

    except Exception as e:
        return None, []


def main():
    print()
    print("=" * 65)
    print("  Dofus.exe Network Monitor")
    print("  Watching ALL TCP connections in real-time")
    print("  Run this while launching the game")
    print("=" * 65)
    print()

    seen = set()
    prev_pids = set()

    try:
        while True:
            pids, connections = get_dofus_connections()

            if pids is None:
                if prev_pids:
                    print(f"  [{time.strftime('%H:%M:%S')}] Dofus.exe exited")
                    prev_pids = set()
                    seen.clear()
                else:
                    sys.stdout.write(f"\r  Waiting for Dofus.exe...  ")
                    sys.stdout.flush()
                time.sleep(0.5)
                continue

            if pids != prev_pids:
                print(f"\n  [{time.strftime('%H:%M:%S')}] Dofus.exe detected: PID(s) = {', '.join(pids)}")
                print(f"  {'STATE':<15} {'LOCAL':<25} {'REMOTE':<25} {'PID':<8}")
                print(f"  {'-'*15} {'-'*25} {'-'*25} {'-'*8}")
                prev_pids = pids

            for conn in connections:
                key = (conn["local"], conn["remote"], conn["state"], conn["pid"])
                if key not in seen:
                    seen.add(key)

                    # Highlight important connections
                    remote = conn["remote"]
                    marker = ""
                    if ":5555" in remote:
                        marker = " *** PORT 5555 ***"
                    elif ":443" in remote:
                        marker = " (TLS)"
                    elif ":26666" in remote or ":26116" in remote:
                        marker = " (IPC Launcher)"
                    elif "127.0.0.1" in remote:
                        marker = " (loopback)"

                    print(f"  {conn['state']:<15} {conn['local']:<25} {conn['remote']:<25} {conn['pid']:<8}{marker}")

            # Also remove stale entries
            current_keys = set()
            for conn in connections:
                current_keys.add((conn["local"], conn["remote"], conn["state"], conn["pid"]))

            closed = seen - current_keys
            for key in closed:
                local, remote, state, pid = key
                if state in ("ESTABLISHED", "SYN_SENT"):
                    print(f"  {'CLOSED':<15} {local:<25} {remote:<25} {pid:<8}")
            seen = current_keys

            time.sleep(0.3)

    except KeyboardInterrupt:
        print("\n\n  Stopped.")


if __name__ == "__main__":
    main()

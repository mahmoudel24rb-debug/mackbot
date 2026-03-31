"""
Jitsuri Monitor - Observe how Jitsuri launches Dofus and redirects traffic.

Monitors in real-time:
  - Network connections (netstat polling)
  - Process creation (Dofus.exe, Jitsuri)
  - Port usage changes
  - hosts file changes
  - zaap-start.bat changes

Usage:
    python -m launcher.jitsuri_monitor

Then launch Jitsuri and click "Lancer" - this script captures everything.
"""

import os
import subprocess
import time
import re
import json
from datetime import datetime


HOSTS_FILE = r"C:\Windows\System32\drivers\etc\hosts"
DOFUS_DIR = os.path.join(
    os.environ.get("LOCALAPPDATA", ""),
    "Ankama", "Dofus-dofus3",
)
ZAAP_START_BAT = os.path.join(DOFUS_DIR, "zaap-start.bat")
ZAAP_YML = os.path.join(DOFUS_DIR, "zaap.yml")
LAUNCHER_LOG = os.path.join(
    os.environ.get("APPDATA", ""),
    "zaap", "application.log",
)
DOFUS_LOG = os.path.join(
    os.environ.get("APPDATA", ""),
    "zaap", "gamesLogs", "dofus-dofus3", "dofus.143047241.log",
)

# Ports to monitor
WATCH_PORTS = {5555, 5556, 5657, 26116, 26666, 7777, 443}

LOG_FILE = os.path.join(
    os.path.dirname(os.path.dirname(__file__)),
    "captures",
    f"jitsuri_monitor_{datetime.now():%Y%m%d_%H%M%S}.jsonl",
)


def ts():
    return datetime.now().strftime("%H:%M:%S.%f")[:-3]


def log(category, msg, data=None):
    """Log to console and file."""
    color = {
        "NET": "\033[36m",      # Cyan
        "PROC": "\033[33m",     # Yellow
        "FILE": "\033[35m",     # Magenta
        "EVENT": "\033[32m",    # Green
        "DIFF": "\033[91m",     # Red
    }.get(category, "\033[0m")

    print(f"\033[90m[{ts()}]\033[0m {color}[{category}]\033[0m {msg}")

    entry = {
        "ts": ts(),
        "cat": category,
        "msg": msg,
    }
    if data:
        entry["data"] = data

    try:
        os.makedirs(os.path.dirname(LOG_FILE), exist_ok=True)
        with open(LOG_FILE, "a", encoding="utf-8") as f:
            f.write(json.dumps(entry, ensure_ascii=False) + "\n")
    except Exception:
        pass


def _run_cmd(cmd, timeout=5):
    """Run a command and return decoded stdout."""
    result = subprocess.run(cmd, capture_output=True, timeout=timeout)
    return result.stdout.decode("utf-8", errors="replace")


def get_connections():
    """Get all TCP connections from netstat."""
    try:
        stdout = _run_cmd(["netstat", "-ano"])
        connections = {}
        for line in stdout.split("\n"):
            line = line.strip()
            if not line.startswith("TCP"):
                continue
            parts = line.split()
            if len(parts) < 5:
                continue
            proto, local, remote, state, pid = parts[:5]
            # Check if any watched port is involved
            local_port = int(local.split(":")[-1]) if ":" in local else 0
            remote_port = int(remote.split(":")[-1]) if ":" in remote else 0
            if local_port in WATCH_PORTS or remote_port in WATCH_PORTS:
                key = f"{local}->{remote}({state},pid={pid})"
                connections[key] = {
                    "proto": proto,
                    "local": local,
                    "remote": remote,
                    "state": state,
                    "pid": int(pid),
                    "local_port": local_port,
                    "remote_port": remote_port,
                }
        return connections
    except Exception as e:
        return {}


def get_processes():
    """Get relevant processes."""
    try:
        stdout = _run_cmd(["tasklist", "/V", "/FO", "CSV"])
        procs = {}
        for line in stdout.split("\n"):
            lower = line.lower()
            if any(name in lower for name in ["dofus", "jitsuri", "python"]):
                parts = line.strip().strip('"').split('","')
                if len(parts) >= 2:
                    name = parts[0]
                    pid = parts[1]
                    procs[f"{name}({pid})"] = {
                        "name": name,
                        "pid": pid,
                        "mem": parts[4] if len(parts) > 4 else "",
                    }
        return procs
    except Exception:
        return {}


def get_dofus_cmdline():
    """Get Dofus.exe command line arguments."""
    try:
        stdout = _run_cmd(["wmic", "process", "where", "name='Dofus.exe'",
                           "get", "CommandLine,ProcessId", "/format:list"])
        lines = stdout.strip().split("\n")
        cmdlines = []
        current = {}
        for line in lines:
            line = line.strip()
            if line.startswith("CommandLine="):
                current["cmdline"] = line[12:]
            elif line.startswith("ProcessId="):
                current["pid"] = line[10:]
                if current.get("cmdline"):
                    cmdlines.append(current.copy())
                current = {}
        return cmdlines
    except Exception:
        return []


def read_file_safe(path):
    """Read a file, return content or None."""
    try:
        with open(path, "r", encoding="utf-8") as f:
            return f.read()
    except Exception:
        return None


def main():
    print()
    print("\033[1;33m" + "=" * 60 + "\033[0m")
    print("\033[1;33m  Jitsuri Monitor - Observing launch flow\033[0m")
    print("\033[1;33m" + "=" * 60 + "\033[0m")
    print()
    print(f"  Log file: {LOG_FILE}")
    print(f"  Watching ports: {sorted(WATCH_PORTS)}")
    print()
    print("  1. Launch Jitsuri")
    print("  2. Click 'Lancer' in Jitsuri")
    print("  3. This script will capture everything")
    print()
    print("  Press Ctrl+C to stop.")
    print()

    # Initial state
    prev_conns = get_connections()
    prev_procs = get_processes()
    prev_hosts = read_file_safe(HOSTS_FILE)
    prev_bat = read_file_safe(ZAAP_START_BAT)
    prev_yml = read_file_safe(ZAAP_YML)
    prev_launcher_log_size = os.path.getsize(LAUNCHER_LOG) if os.path.exists(LAUNCHER_LOG) else 0
    prev_dofus_log_size = os.path.getsize(DOFUS_LOG) if os.path.exists(DOFUS_LOG) else 0

    log("EVENT", f"Initial state: {len(prev_conns)} connections, {len(prev_procs)} processes")
    if prev_conns:
        for key, conn in prev_conns.items():
            log("NET", f"  {conn['local']} -> {conn['remote']} [{conn['state']}] pid={conn['pid']}")

    iteration = 0
    dofus_cmdline_logged = False

    try:
        while True:
            time.sleep(0.5)
            iteration += 1

            # Check network changes
            curr_conns = get_connections()
            new_conns = set(curr_conns.keys()) - set(prev_conns.keys())
            gone_conns = set(prev_conns.keys()) - set(curr_conns.keys())

            for key in new_conns:
                conn = curr_conns[key]
                log("NET", f"+ NEW: {conn['local']} -> {conn['remote']} [{conn['state']}] pid={conn['pid']}", conn)

            for key in gone_conns:
                conn = prev_conns[key]
                log("NET", f"- GONE: {conn['local']} -> {conn['remote']} [{conn['state']}] pid={conn['pid']}", conn)

            prev_conns = curr_conns

            # Check process changes (every 2 seconds)
            if iteration % 4 == 0:
                curr_procs = get_processes()
                new_procs = set(curr_procs.keys()) - set(prev_procs.keys())
                gone_procs = set(prev_procs.keys()) - set(curr_procs.keys())

                for key in new_procs:
                    proc = curr_procs[key]
                    log("PROC", f"+ STARTED: {proc['name']} (pid={proc['pid']}, mem={proc['mem']})", proc)

                for key in gone_procs:
                    proc = prev_procs[key]
                    log("PROC", f"- STOPPED: {proc['name']} (pid={proc['pid']})", proc)

                prev_procs = curr_procs

            # Check Dofus command line (once, when Dofus appears)
            if not dofus_cmdline_logged:
                cmdlines = get_dofus_cmdline()
                if cmdlines:
                    dofus_cmdline_logged = True
                    for entry in cmdlines:
                        log("PROC", f"Dofus.exe cmdline (pid={entry['pid']}):")
                        log("PROC", f"  {entry['cmdline']}")

                        # Parse key args
                        cmdline = entry['cmdline']
                        for arg in ["--port", "--connectionPort", "--gameName",
                                    "--instanceId", "--hash", "--canLogin",
                                    "--autoConnectType"]:
                            match = re.search(rf"{arg}\s+(\S+)", cmdline)
                            if match:
                                log("PROC", f"    {arg} = {match.group(1)}")

            # Check file changes (every 3 seconds)
            if iteration % 6 == 0:
                curr_hosts = read_file_safe(HOSTS_FILE)
                if curr_hosts != prev_hosts:
                    log("FILE", "hosts file CHANGED!")
                    log("FILE", f"  Content: {repr(curr_hosts[:200])}")
                    prev_hosts = curr_hosts

                curr_bat = read_file_safe(ZAAP_START_BAT)
                if curr_bat != prev_bat:
                    log("FILE", "zaap-start.bat CHANGED!")
                    log("FILE", f"  Content: {repr(curr_bat[:300])}")
                    prev_bat = curr_bat

                curr_yml = read_file_safe(ZAAP_YML)
                if curr_yml != prev_yml:
                    log("FILE", "zaap.yml CHANGED!")
                    # Show diff-like output
                    if prev_yml and curr_yml:
                        old_lines = set(prev_yml.split("\n"))
                        new_lines = set(curr_yml.split("\n"))
                        added = new_lines - old_lines
                        removed = old_lines - new_lines
                        for line in removed:
                            if line.strip():
                                log("DIFF", f"  - {line.strip()}")
                        for line in added:
                            if line.strip():
                                log("DIFF", f"  + {line.strip()}")
                    prev_yml = curr_yml

            # Check new log entries (every 2 seconds)
            if iteration % 4 == 0:
                # Launcher log
                if os.path.exists(LAUNCHER_LOG):
                    curr_size = os.path.getsize(LAUNCHER_LOG)
                    if curr_size > prev_launcher_log_size:
                        try:
                            with open(LAUNCHER_LOG, "r", encoding="utf-8") as f:
                                f.seek(prev_launcher_log_size)
                                new_content = f.read()
                                for line in new_content.strip().split("\n"):
                                    line = line.strip()
                                    if line and any(kw in line.lower() for kw in
                                                    ["spawn", "launch", "connect", "port",
                                                     "release", "token", "auth", "error"]):
                                        log("FILE", f"[launcher.log] {line[:150]}")
                        except Exception:
                            pass
                        prev_launcher_log_size = curr_size

                # Dofus log
                if os.path.exists(DOFUS_LOG):
                    curr_size = os.path.getsize(DOFUS_LOG)
                    if curr_size > prev_dofus_log_size:
                        try:
                            with open(DOFUS_LOG, "r", encoding="utf-8") as f:
                                f.seek(prev_dofus_log_size)
                                new_content = f.read()
                                for line in new_content.strip().split("\n"):
                                    line = line.strip()
                                    if line and any(kw in line.lower() for kw in
                                                    ["connect", "error", "socket",
                                                     "server", "endpoint", "127.0.0.1"]):
                                        log("FILE", f"[dofus.log] {line[:150]}")
                        except Exception:
                            pass
                        prev_dofus_log_size = curr_size

    except KeyboardInterrupt:
        print()
        log("EVENT", "Monitor stopped (Ctrl+C)")
        print()
        print(f"  Full log saved to: {LOG_FILE}")
        print()


if __name__ == "__main__":
    main()

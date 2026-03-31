"""
Quick script to capture Dofus.exe command line arguments.

Polls every 0.3s using PowerShell (faster than wmic on Win11).
Run this BEFORE launching Jitsuri/Dofus.

Usage:
    python -m launcher.catch_dofus_cmdline
"""

import subprocess
import time
import re


def get_dofus_cmdline():
    """Get Dofus.exe command line via PowerShell (no truncation)."""
    try:
        # Use ForEach + Write-Output to avoid Format-List truncation
        result = subprocess.run(
            ["powershell", "-NoProfile", "-Command",
             "Get-CimInstance Win32_Process -Filter \"Name='Dofus.exe'\" | "
             "ForEach-Object { Write-Output \"PID=$($_.ProcessId)\"; "
             "Write-Output \"CMD=$($_.CommandLine)\"; Write-Output '---' }"],
            capture_output=True, timeout=5,
        )
        stdout = result.stdout.decode("utf-8", errors="replace")
        entries = []
        current = {}
        for line in stdout.split("\n"):
            line = line.strip()
            if line.startswith("PID="):
                current["pid"] = line[4:]
            elif line.startswith("CMD="):
                current["cmdline"] = line[4:]
            elif line == "---" and current.get("pid"):
                entries.append(current.copy())
                current = {}
        if current.get("pid"):
            entries.append(current)
        return entries
    except Exception as e:
        print(f"  Error: {e}")
        return []


def main():
    print()
    print("=" * 60)
    print("  Dofus.exe Command Line Catcher")
    print("=" * 60)
    print()
    print("  Waiting for Dofus.exe to appear...")
    print("  Launch Jitsuri and click 'Lancer'")
    print("  Press Ctrl+C to stop.")
    print()

    seen_pids = set()

    try:
        while True:
            entries = get_dofus_cmdline()
            for entry in entries:
                pid = entry.get("pid", "")
                if pid in seen_pids:
                    continue
                seen_pids.add(pid)

                cmdline = entry.get("cmdline", "")
                print(f"\n{'='*60}")
                print(f"  DOFUS.EXE FOUND! PID={pid}")
                print(f"{'='*60}")
                print(f"\n  Full command line:")
                print(f"  {cmdline}")
                print()

                # Parse key arguments
                for arg in ["--port", "--connectionPort", "--gameName",
                            "--gameRelease", "--instanceId", "--hash",
                            "--canLogin", "--autoConnectType", "--langCode",
                            "--logFile"]:
                    match = re.search(rf"{arg}\s+(\S+)", cmdline)
                    if match:
                        print(f"    {arg:25s} = {match.group(1)}")

                # Check for ZAAP env vars
                print()
                print("  Environment variables (ZAAP_*):")
                try:
                    env_result = subprocess.run(
                        ["powershell", "-NoProfile", "-Command",
                         f"(Get-CimInstance Win32_Process -Filter \"ProcessId={pid}\")."
                         "GetOwner() | Out-Null; "
                         f"[System.Diagnostics.Process]::GetProcessById({pid})."
                         "StartInfo.EnvironmentVariables | Out-String"],
                        capture_output=True, timeout=5,
                    )
                except Exception:
                    pass

                print()
                print("  (Keeping watch for additional Dofus instances...)")

            time.sleep(0.3)

    except KeyboardInterrupt:
        print()
        if seen_pids:
            print(f"  Captured {len(seen_pids)} Dofus instance(s)")
        else:
            print("  No Dofus.exe detected")
        print()


if __name__ == "__main__":
    main()

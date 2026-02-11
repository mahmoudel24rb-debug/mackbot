import sys
import os
import time
from datetime import datetime

# Force UTF-8 output on Windows
if sys.platform == "win32":
    os.environ.setdefault("PYTHONIOENCODING", "utf-8")
    if hasattr(sys.stdout, "reconfigure"):
        sys.stdout.reconfigure(encoding="utf-8")
    if hasattr(sys.stderr, "reconfigure"):
        sys.stderr.reconfigure(encoding="utf-8")


# ANSI color codes (Windows 10+ supports them)
class Colors:
    RESET = "\033[0m"
    BOLD = "\033[1m"
    DIM = "\033[2m"

    RED = "\033[91m"
    GREEN = "\033[92m"
    YELLOW = "\033[93m"
    BLUE = "\033[94m"
    MAGENTA = "\033[95m"
    CYAN = "\033[96m"
    WHITE = "\033[97m"
    GRAY = "\033[90m"

    BG_RED = "\033[41m"
    BG_GREEN = "\033[42m"
    BG_BLUE = "\033[44m"


# Enable ANSI on Windows
if sys.platform == "win32":
    import ctypes
    kernel32 = ctypes.windll.kernel32
    kernel32.SetConsoleMode(kernel32.GetStdHandle(-11), 7)


def _timestamp():
    return datetime.now().strftime("%H:%M:%S.%f")[:-3]


def info(msg):
    print(f"{Colors.GRAY}[{_timestamp()}]{Colors.RESET} {Colors.CYAN}[INFO]{Colors.RESET} {msg}")


def warn(msg):
    print(f"{Colors.GRAY}[{_timestamp()}]{Colors.RESET} {Colors.YELLOW}[WARN]{Colors.RESET} {msg}")


def error(msg):
    print(f"{Colors.GRAY}[{_timestamp()}]{Colors.RESET} {Colors.RED}[ERROR]{Colors.RESET} {msg}")


def client_to_server(msg_name, size, raw_preview=""):
    arrow = f"{Colors.GREEN}{Colors.BOLD}CLIENT >>> SERVER{Colors.RESET}"
    name = f"{Colors.GREEN}{msg_name}{Colors.RESET}"
    sz = f"{Colors.DIM}({size} bytes){Colors.RESET}"
    print(f"{Colors.GRAY}[{_timestamp()}]{Colors.RESET} {arrow} {name} {sz}")
    if raw_preview:
        print(f"  {Colors.DIM}{raw_preview}{Colors.RESET}")


def server_to_client(msg_name, size, raw_preview=""):
    arrow = f"{Colors.BLUE}{Colors.BOLD}SERVER >>> CLIENT{Colors.RESET}"
    name = f"{Colors.BLUE}{msg_name}{Colors.RESET}"
    sz = f"{Colors.DIM}({size} bytes){Colors.RESET}"
    print(f"{Colors.GRAY}[{_timestamp()}]{Colors.RESET} {arrow} {name} {sz}")
    if raw_preview:
        print(f"  {Colors.DIM}{raw_preview}{Colors.RESET}")


def hex_dump(data, max_bytes=128):
    """Format bytes as a readable hex dump string."""
    truncated = data[:max_bytes]
    hex_str = " ".join(f"{b:02x}" for b in truncated)
    ascii_str = "".join(chr(b) if 32 <= b < 127 else "." for b in truncated)
    result = f"HEX: {hex_str}"
    if len(data) > max_bytes:
        result += f" ... (+{len(data) - max_bytes} bytes)"
    result += f"\n  ASCII: {ascii_str}"
    return result


def debug(msg):
    print(f"{Colors.GRAY}[{_timestamp()}]{Colors.RESET} {Colors.GRAY}[DEBUG]{Colors.RESET} {Colors.DIM}{msg}{Colors.RESET}")


def connection(msg):
    print(f"{Colors.GRAY}[{_timestamp()}]{Colors.RESET} {Colors.MAGENTA}[CONN]{Colors.RESET} {msg}")


def banner():
    print(f"""
{Colors.CYAN}{Colors.BOLD}╔══════════════════════════════════════════╗
║     Dofus 3 MITM Proxy Sniffer          ║
║     Python Edition                       ║
╚══════════════════════════════════════════╝{Colors.RESET}
""")

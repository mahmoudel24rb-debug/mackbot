PROXY_HOST = "127.0.0.1"
PROXY_PORT = 5555

# Login server hostname (resolved at startup via DNS, bypassing hosts file)
SERVER_HOSTNAME = "dofus2-co-production.ankama-games.com"
SERVER_HOST = None  # Auto-resolved at startup
SERVER_PORT = 5555

# Port for the auto-started game server proxy
GAME_PROXY_PORT = 5556

# Ankama Launcher IPC
LAUNCHER_IPC_PORT = 26116       # Real Launcher IPC port
SNIFFER_IPC_PORT = 26667        # Our sniffer listens here
FAKE_LAUNCHER_PORT = 26666      # Our fake launcher port

# Config server (serves modified config JSON to redirect Dofus to our proxy)
CONFIG_SERVER_PORT = 8888

# Original Ankama config URL (for reference)
ORIGINAL_CONFIG_URL = "https://dofus2.cdn.ankama.com/config/release_windows.json"

VERBOSE = False           # Full protobuf decode for every packet (very noisy)
HEX_DUMP_MAX_BYTES = 128
MOVEMENT_DEBUG = True     # Extra detail for movement/map change packets

# Matching system
MATCHING_FILE = "data/matching.json"
AUTO_MATCH = True         # Auto-detect 3-letter codes from traffic structure

# Scripts & routes directories
SCRIPTS_DIR = "scripts"
ROUTES_DIR  = "routes"

# UI settings
UI_THEME     = "dark"           # "dark" or "light"
UI_ACCENT    = "#1f6aa5"        # Blue accent colour
UI_WIDTH     = 1100
UI_HEIGHT    = 700
UI_TITLE     = "DofusBot"

# Anti-detection delays (seconds)
ACTION_DELAY_MIN  = 0.3
ACTION_DELAY_MAX  = 0.8
MAP_CHANGE_DELAY  = 1.2

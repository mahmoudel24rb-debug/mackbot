"""
Dofus 3 Passive Network Sniffer - Auto-detecting version.

Captures all game traffic between Jitsuri and the Dofus server.
Auto-detects server IP and network interface.

Requirements:
  - npcap installed (https://npcap.com/)
  - pip install scapy

Usage:
  python sniffer.py              # Auto-detect everything
  python sniffer.py --server IP  # Manual server IP
  python sniffer.py --list       # List interfaces
"""

import sys
import os
import time
import subprocess
import argparse
import json
from datetime import datetime

sys.path.insert(0, os.path.dirname(__file__))
sys.stdout.reconfigure(encoding='utf-8', errors='replace')

# Fix scapy cache permission issue on Windows
# Redirect cache to local project dir instead of user home
_scapy_cache = os.path.join(os.path.dirname(__file__), '.scapy_cache')
os.makedirs(_scapy_cache, exist_ok=True)
os.environ['XDG_CACHE_HOME'] = _scapy_cache
os.environ['LOCALAPPDATA'] = os.environ.get('LOCALAPPDATA', _scapy_cache)

from proxy.packet_handler import PacketBuffer, decode_protobuf_fields
from game.dofus_message import extract_message_info, get_type_name
from utils.capture import PacketCapture


# ---------------------------------------------------------------------------
# Auto-detection
# ---------------------------------------------------------------------------

def detect_game_servers():
    """Auto-detect ALL game server IPs by checking active TCP connections.

    Returns a list of unique IPs connected on port 5555.
    """
    ips = set()
    try:
        result = subprocess.run(
            ["powershell", "-Command",
             "Get-NetTCPConnection -State Established -RemotePort 5555 "
             "| Select-Object -ExpandProperty RemoteAddress"],
            capture_output=True, text=True, timeout=10, encoding='utf-8', errors='replace'
        )
        for line in result.stdout.strip().split('\n'):
            line = line.strip()
            if line and not line.startswith('127.'):
                ips.add(line)
    except Exception:
        pass

    # Fallback: try netstat
    if not ips:
        try:
            result = subprocess.run(
                ["netstat", "-n", "-p", "tcp"],
                capture_output=True, text=True, timeout=10, encoding='utf-8', errors='replace'
            )
            for line in result.stdout.split('\n'):
                if ':5555' in line and 'ESTABLISHED' in line:
                    parts = line.split()
                    for part in parts:
                        if ':5555' in part and not part.startswith('127.'):
                            ip = part.rsplit(':', 1)[0]
                            ips.add(ip)
        except Exception:
            pass

    return list(ips)


def detect_local_ip():
    """Detect the local IP used for game connections."""
    try:
        result = subprocess.run(
            ["powershell", "-Command",
             "Get-NetTCPConnection -State Established -RemotePort 5555 "
             "| Select-Object -ExpandProperty LocalAddress"],
            capture_output=True, text=True, timeout=10, encoding='utf-8', errors='replace'
        )
        ips = [line.strip() for line in result.stdout.strip().split('\n') if line.strip()]
        if ips:
            return ips[0]
    except Exception:
        pass
    return None


def find_interface_for_ip(local_ip):
    """Find the scapy interface that matches the local IP.

    On Windows with npcap, we need to return the actual interface object
    or its proper identifier, not just a string name.
    """
    try:
        from scapy.all import conf
        conf.verb = 0

        # On Windows, conf.ifaces has the real interface objects
        if hasattr(conf, 'ifaces'):
            for iface_id, iface_obj in conf.ifaces.items():
                try:
                    ip = getattr(iface_obj, 'ip', None)
                    if ip == local_ip:
                        # Return the interface object itself - scapy sniff() accepts it
                        return iface_obj
                except Exception:
                    continue

        # Fallback: try get_if_list + get_if_addr
        from scapy.all import get_if_list, get_if_addr
        for iface in get_if_list():
            try:
                addr = get_if_addr(iface)
                if addr == local_ip:
                    return iface
            except Exception:
                continue
    except Exception:
        pass
    return None


# ---------------------------------------------------------------------------
# TCP Stream Tracker
# ---------------------------------------------------------------------------

class TCPStream:
    def __init__(self, direction):
        self.direction = direction
        self.buffer = PacketBuffer()

    def feed(self, data):
        self.buffer.feed(data)

    def extract_packets(self):
        return self.buffer.try_extract_packets()


class ConnectionTracker:
    def __init__(self, server_ips, server_port):
        self.server_ips = set(server_ips) if isinstance(server_ips, list) else {server_ips}
        self.server_port = server_port
        self.streams = {}

    def get_direction(self, src_ip, dst_ip):
        if dst_ip in self.server_ips:
            return "c2s"
        elif src_ip in self.server_ips:
            return "s2c"
        return None

    def process(self, src_ip, src_port, dst_ip, dst_port, payload):
        if not payload:
            return []

        direction = self.get_direction(src_ip, dst_ip)
        if direction is None:
            return []

        key = (src_ip, src_port, dst_ip, dst_port)
        if key not in self.streams:
            self.streams[key] = TCPStream(direction)

        stream = self.streams[key]
        stream.feed(payload)

        results = []
        for pkt in stream.extract_packets():
            results.append((direction, pkt))
        return results

    def cleanup(self, src_ip, src_port, dst_ip, dst_port):
        self.streams.pop((src_ip, src_port, dst_ip, dst_port), None)
        self.streams.pop((dst_ip, dst_port, src_ip, src_port), None)


# ---------------------------------------------------------------------------
# Display
# ---------------------------------------------------------------------------

class Colors:
    GREEN = "\033[92m"
    BLUE = "\033[94m"
    YELLOW = "\033[93m"
    RED = "\033[91m"
    GRAY = "\033[90m"
    BOLD = "\033[1m"
    RESET = "\033[0m"


def display_message(type_code, type_name, direction, data, uid, start_time, verbose=False):
    elapsed = time.time() - start_time
    ms = int(elapsed * 1000)

    if direction == "c2s":
        arrow = f"{Colors.GREEN}C2S >>>{Colors.RESET}"
    else:
        arrow = f"{Colors.BLUE}S2C <<<{Colors.RESET}"

    size = len(data) if data else 0
    uid_str = f" uid={uid}" if uid else ""

    print(f"{Colors.GRAY}[+{ms:8d}ms]{Colors.RESET} "
          f"{arrow} {Colors.BOLD}{type_name}{Colors.RESET}"
          f"({type_code}) [{size}b]{uid_str}")

    if verbose and data:
        fields = decode_protobuf_fields(data)
        for fn, wt, val in fields[:10]:
            if wt == 0:  # varint
                print(f"            f{fn}: {val}")
            elif wt == 2:  # bytes
                try:
                    text = val.decode('utf-8')
                    if all(32 <= ord(c) < 127 for c in text):
                        print(f'            f{fn}: "{text}"')
                        continue
                except Exception:
                    pass
                print(f"            f{fn}: <{len(val)}b>")


# ---------------------------------------------------------------------------
# Main
# ---------------------------------------------------------------------------

def _resolve_interface_name(name, conf):
    """Resolve a friendly interface name (e.g. 'Wi-Fi') to a scapy interface object."""
    if not hasattr(conf, 'ifaces'):
        return None
    name_lower = name.lower()
    for iface_id, iface_obj in conf.ifaces.items():
        obj_name = getattr(iface_obj, 'name', '')
        obj_desc = getattr(iface_obj, 'description', '')
        if (obj_name.lower() == name_lower
                or name_lower in obj_name.lower()
                or name_lower in obj_desc.lower()):
            return iface_obj
    return None


def run_sniffer(server_ip=None, server_port=5555, interface=None, verbose=False):
    try:
        from scapy.all import sniff, TCP, IP, conf
    except ImportError:
        print(f"{Colors.RED}ERREUR: scapy non installe.{Colors.RESET}")
        print("  pip install scapy")
        print("  + installer npcap: https://npcap.com/")
        sys.exit(1)

    conf.verb = 0

    # Auto-detect server IPs
    if server_ip is None:
        print(f"{Colors.YELLOW}[*] Auto-detection des serveurs Dofus...{Colors.RESET}")
        server_ips = detect_game_servers()
        if not server_ips:
            print(f"{Colors.RED}[!] Aucune connexion Dofus detectee (port 5555){Colors.RESET}")
            print("    Lance Jitsuri et connecte-toi au jeu d'abord.")
            print("    Ou specifie l'IP: python sniffer.py --server IP")
            sys.exit(1)
        for ip in server_ips:
            print(f"{Colors.GREEN}[+] Serveur detecte: {ip}:{server_port}{Colors.RESET}")
    else:
        server_ips = [server_ip]

    # Resolve --iface name to scapy interface object on Windows
    if interface is not None:
        resolved = _resolve_interface_name(interface, conf)
        if resolved:
            print(f"{Colors.GREEN}[+] Interface: {interface}{Colors.RESET}")
            interface = resolved
        else:
            print(f"{Colors.YELLOW}[*] Interface: {interface} (non resolu, utilise tel quel){Colors.RESET}")

    # Auto-detect interface
    if interface is None:
        local_ip = detect_local_ip()
        if local_ip:
            print(f"{Colors.YELLOW}[*] IP locale: {local_ip}{Colors.RESET}")
            interface = find_interface_for_ip(local_ip)
            if interface:
                iface_name = getattr(interface, 'name', str(interface))
                print(f"{Colors.GREEN}[+] Interface: {iface_name} ({local_ip}){Colors.RESET}")
            else:
                print(f"{Colors.RED}[!] Interface non trouvee pour {local_ip}{Colors.RESET}")
                print(f"    Essaie: python sniffer.py --iface \"Wi-Fi\"")
                print(f"    Ou:     python sniffer.py --list")

    # Setup capture file
    capture = PacketCapture()
    capture.start()
    print(f"{Colors.YELLOW}[*] Capture: {capture.filepath}{Colors.RESET}")

    # Setup tracker
    tracker = ConnectionTracker(server_ips, server_port)
    start_time = time.time()
    stats = {"tcp": 0, "messages": 0, "errors": 0}

    # BPF filter: capture traffic to/from ALL detected server IPs on port 5555
    if len(server_ips) == 1:
        bpf = f"host {server_ips[0]} and tcp port {server_port}"
    else:
        host_filter = " or ".join(f"host {ip}" for ip in server_ips)
        bpf = f"({host_filter}) and tcp port {server_port}"
    print(f"{Colors.YELLOW}[*] Filtre BPF: {bpf}{Colors.RESET}")
    print()
    print(f"{Colors.BOLD}=== SNIFFER DEMARRE ==={Colors.RESET}")
    print(f"{Colors.GRAY}Ctrl+C pour arreter{Colors.RESET}")
    print()

    def on_packet(pkt):
        if not pkt.haslayer(TCP) or not pkt.haslayer(IP):
            return

        ip = pkt[IP]
        tcp = pkt[TCP]
        payload = bytes(tcp.payload)

        if not payload:
            if tcp.flags & 0x01 or tcp.flags & 0x04:
                tracker.cleanup(ip.src, tcp.sport, ip.dst, tcp.dport)
            return

        stats["tcp"] += 1

        # Show first packet as proof of capture
        if stats["tcp"] == 1:
            print(f"{Colors.GREEN}[+] Premier paquet capture! Le sniffer fonctionne.{Colors.RESET}")
            print()

        try:
            results = tracker.process(ip.src, tcp.sport, ip.dst, tcp.dport, payload)

            for direction, packet_payload in results:
                messages = extract_message_info(packet_payload, direction)
                for type_code, msg_data, uid in messages:
                    type_name = get_type_name(type_code)
                    stats["messages"] += 1
                    display_message(type_code, type_name, direction, msg_data, uid, start_time, verbose)
                    capture.log_message(type_code, type_name, direction, msg_data, uid)

        except Exception as e:
            stats["errors"] += 1
            if stats["errors"] <= 10:
                print(f"{Colors.RED}[E] Parse error: {e}{Colors.RESET}")

    try:
        sniff(
            filter=bpf,
            prn=on_packet,
            store=False,
            iface=interface,
        )
    except KeyboardInterrupt:
        pass
    except PermissionError:
        print()
        print(f"{Colors.RED}[!] Permission refusee! Lance en Administrateur.{Colors.RESET}")
        sys.exit(1)
    except Exception as e:
        print(f"{Colors.RED}[!] Erreur: {e}{Colors.RESET}")
        print("    Verifie que npcap est installe: https://npcap.com/")
        sys.exit(1)

    capture.stop()
    print()
    print(f"{Colors.BOLD}=== SNIFFER ARRETE ==={Colors.RESET}")
    print(f"  Paquets TCP: {stats['tcp']}")
    print(f"  Messages jeu: {stats['messages']}")
    print(f"  Erreurs: {stats['errors']}")
    print(f"  Fichier: {capture.filepath}")


def list_interfaces():
    try:
        from scapy.all import conf
        conf.verb = 0
        if hasattr(conf, 'ifaces'):
            print("Interfaces disponibles:")
            for iface_id, iface_obj in conf.ifaces.items():
                name = getattr(iface_obj, 'name', str(iface_id))
                ip = getattr(iface_obj, 'ip', '?')
                desc = getattr(iface_obj, 'description', '')
                print(f"  {name}")
                print(f"    IP: {ip}")
                if desc:
                    print(f"    {desc}")
        else:
            from scapy.all import get_if_list
            for iface in get_if_list():
                print(f"  {iface}")
    except ImportError:
        print("scapy non installe. pip install scapy")


if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Dofus 3 Sniffer - Auto-detecting")
    parser.add_argument("--server", "-s", default=None, help="IP du serveur Dofus")
    parser.add_argument("--port", "-p", type=int, default=5555, help="Port (default: 5555)")
    parser.add_argument("--iface", "-i", default=None, help="Interface reseau")
    parser.add_argument("--list", action="store_true", help="Lister les interfaces")
    parser.add_argument("--verbose", "-v", action="store_true", help="Afficher les champs protobuf")

    args = parser.parse_args()

    if args.list:
        list_interfaces()
        sys.exit(0)

    run_sniffer(args.server, args.port, args.iface, args.verbose)

"""
Dofus 3 Passive Sniffer using WinDivert 2.2 (SNIFF mode).

Captures game traffic without interfering - packets pass through normally.
No npcap needed, uses our existing WinDivert 2.2 DLL.

Auto-detects server IPs from active connections on port 5555.

Usage:
  python sniffer_wd.py                  # Auto-detect, sniff only
  python sniffer_wd.py --verbose        # Show protobuf field details
  python sniffer_wd.py --redirect       # Active redirect mode (MITM proxy)

Requires: Administrator privileges
"""

import sys
import os
import time
import struct
import subprocess
import argparse
import threading
from datetime import datetime

sys.path.insert(0, os.path.dirname(__file__))
sys.stdout.reconfigure(encoding='utf-8', errors='replace')

from proxy.packet_handler import PacketBuffer, decode_protobuf_fields
from game.dofus_message import extract_message_info, get_type_name
from utils.capture import PacketCapture
from protocol.matching import Matching
from protocol.auto_matcher import AutoMatcher
import config

_matching = Matching(getattr(config, "MATCHING_FILE", "data/matching.json"))
_auto_matcher = AutoMatcher(_matching) if getattr(config, "AUTO_MATCH", True) else None

# WinDivert flags
WINDIVERT_FLAG_SNIFF = 0x0001
WINDIVERT_FLAG_RECV_ONLY = 0x0004


# ---------------------------------------------------------------------------
# Auto-detection
# ---------------------------------------------------------------------------

def detect_game_servers():
    """Auto-detect game server IPs from active TCP connections on port 5555."""
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

    # Fallback: netstat
    if not ips:
        try:
            result = subprocess.run(
                ["netstat", "-n", "-p", "tcp"],
                capture_output=True, text=True, timeout=10,
                encoding='utf-8', errors='replace',
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


# ---------------------------------------------------------------------------
# TCP Stream Tracker
# ---------------------------------------------------------------------------

class TCPStream:
    """Track a single TCP direction and extract game messages."""

    def __init__(self, direction):
        self.direction = direction
        self.buffer = PacketBuffer()
        self.next_seq = None  # Next expected sequence number

    def feed(self, tcp_seq, payload):
        """Feed payload, handling overlaps from RSC/GRO coalesced segments.

        RSC (Receive Segment Coalescing) causes WinDivert to deliver 65KB
        aggregated packets that overlap with subsequent segments. We must
        handle partial overlaps to avoid losing data.
        """
        end_seq = tcp_seq + len(payload)

        if self.next_seq is None:
            # First packet in this stream
            self.next_seq = end_seq
            self.buffer.feed(payload)
            return

        if end_seq <= self.next_seq:
            # Fully within already-seen range (true retransmission)
            return

        if tcp_seq < self.next_seq:
            # Partial overlap: skip bytes we already have, feed the rest
            skip = self.next_seq - tcp_seq
            self.buffer.feed(payload[skip:])
        else:
            # No overlap (might have a gap - feed everything)
            self.buffer.feed(payload)

        self.next_seq = end_seq

    def extract_packets(self):
        return self.buffer.try_extract_packets()


class ConnectionTracker:
    """Track multiple TCP connections and their streams."""

    def __init__(self, server_ips, server_port=5555):
        self.server_ips = set(server_ips)
        self.server_port = server_port
        self.streams = {}

    def get_direction(self, src_ip, dst_ip):
        if dst_ip in self.server_ips:
            return "c2s"
        elif src_ip in self.server_ips:
            return "s2c"
        return None

    def process(self, src_ip, src_port, dst_ip, dst_port, tcp_seq, payload):
        if not payload:
            return []

        direction = self.get_direction(src_ip, dst_ip)
        if direction is None:
            return []

        key = (src_ip, src_port, dst_ip, dst_port)
        if key not in self.streams:
            self.streams[key] = TCPStream(direction)

        stream = self.streams[key]
        stream.feed(tcp_seq, payload)

        results = []
        for pkt in stream.extract_packets():
            results.append((direction, pkt))
        return results

    def cleanup(self, src_ip, src_port, dst_ip, dst_port):
        self.streams.pop((src_ip, src_port, dst_ip, dst_port), None)
        self.streams.pop((dst_ip, dst_port, src_ip, src_port), None)


# ---------------------------------------------------------------------------
# Colors
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
# WinDivert Sniffer
# ---------------------------------------------------------------------------

def run_wd_sniffer(server_ips, server_port=5555, verbose=False):
    """Run WinDivert-based passive sniffer."""
    from launcher.windivert2 import (
        WinDivert2, parse_ipv4, parse_tcp,
    )

    # Build filter for all server IPs
    ip_filters = []
    for ip in server_ips:
        ip_filters.append(f"ip.SrcAddr == {ip}")
        ip_filters.append(f"ip.DstAddr == {ip}")
    ip_filter = " or ".join(ip_filters)

    filt = f"tcp.DstPort == {server_port} or tcp.SrcPort == {server_port}"

    print(f"{Colors.YELLOW}[*] WinDivert filter: {filt}{Colors.RESET}")

    # Open with SNIFF flag (passive, non-destructive)
    flags = WINDIVERT_FLAG_SNIFF | WINDIVERT_FLAG_RECV_ONLY
    wd = WinDivert2(filt, flags=flags)
    try:
        wd.open()
    except Exception as e:
        print(f"{Colors.RED}[!] WinDivert failed: {e}{Colors.RESET}")
        print("    Run as Administrator!")
        return

    print(f"{Colors.GREEN}[+] WinDivert SNIFF mode active (passive, non-blocking){Colors.RESET}")

    # Setup
    capture = PacketCapture()
    capture.start()
    print(f"{Colors.YELLOW}[*] Capture: {capture.filepath}{Colors.RESET}")

    tracker = ConnectionTracker(server_ips, server_port)
    start_time = time.time()
    stats = {"packets": 0, "tcp_data": 0, "messages": 0, "errors": 0,
             "recv_errors": 0, "fed": 0, "extracted": 0, "skipped_retrans": 0}
    last_stats_time = time.time()

    print()
    print(f"{Colors.BOLD}=== SNIFFER WinDivert DEMARRE ==={Colors.RESET}")
    print(f"{Colors.GRAY}Ctrl+C pour arreter{Colors.RESET}")
    print()

    try:
        while True:
            try:
                raw, addr = wd.recv()
            except KeyboardInterrupt:
                break
            except Exception as e:
                stats["recv_errors"] += 1
                if stats["recv_errors"] <= 5:
                    print(f"{Colors.RED}[!] recv error: {e}{Colors.RESET}")
                continue

            stats["packets"] += 1

            # Periodic stats (every 10 seconds)
            now = time.time()
            if now - last_stats_time > 10:
                elapsed = int(now - start_time)
                print(f"{Colors.GRAY}[STATS +{elapsed}s] pkts={stats['packets']} "
                      f"data={stats['tcp_data']} fed={stats['fed']} "
                      f"extracted={stats['extracted']} msgs={stats['messages']} "
                      f"errs={stats['errors']} recv_errs={stats['recv_errors']} "
                      f"retrans={stats['skipped_retrans']}{Colors.RESET}")
                last_stats_time = now

            # Parse IP header
            ip = parse_ipv4(raw)
            if not ip or ip["protocol"] != 6:
                continue

            # Parse TCP header
            tcp = parse_tcp(raw, ip["ihl"])
            if not tcp:
                continue

            src_ip = ip["src_ip"]
            dst_ip = ip["dst_ip"]
            src_port = tcp["src_port"]
            dst_port = tcp["dst_port"]

            # Dynamic server detection: add any new server IPs
            if dst_port == server_port and dst_ip not in tracker.server_ips and dst_ip != "127.0.0.1":
                tracker.server_ips.add(dst_ip)
                print(f"{Colors.GREEN}[+] New server detected: {dst_ip}{Colors.RESET}")
            if src_port == server_port and src_ip not in tracker.server_ips and src_ip != "127.0.0.1":
                tracker.server_ips.add(src_ip)
                print(f"{Colors.GREEN}[+] New server detected: {src_ip}{Colors.RESET}")

            # Check for FIN/RST (connection close)
            if tcp["fin"] or tcp["rst"]:
                tracker.cleanup(src_ip, src_port, dst_ip, dst_port)
                continue

            # Extract TCP payload using IP total_length (NOT raw buffer size!)
            # WinDivert recv_len may return the full 65535 buffer size
            # instead of the actual packet size, padding with zeros.
            # The IP total_length field is the authoritative packet size.
            actual_pkt_len = min(ip["total_len"], len(raw))
            payload_off = ip["ihl"] + tcp["data_off"]
            if payload_off >= actual_pkt_len:
                continue
            payload = raw[payload_off:actual_pkt_len]
            if not payload:
                continue

            stats["tcp_data"] += 1

            if stats["tcp_data"] <= 3:
                # Debug first few data packets
                direction = tracker.get_direction(src_ip, dst_ip)
                print(f"{Colors.GRAY}[DBG] pkt#{stats['tcp_data']} {direction or '?'} "
                      f"{src_ip}:{src_port}->{dst_ip}:{dst_port} "
                      f"seq={tcp['seq']} len={len(payload)} "
                      f"raw_len={len(raw)}{Colors.RESET}")

            if stats["tcp_data"] == 1:
                print(f"{Colors.GREEN}[+] Premier paquet data capture!{Colors.RESET}")
                print()

            # Process through tracker (with debug)
            try:
                key = (src_ip, src_port, dst_ip, dst_port)
                stream = tracker.streams.get(key)
                end_seq = tcp["seq"] + len(payload)

                # Check what feed will do (for stats)
                if stream and stream.next_seq is not None:
                    if end_seq <= stream.next_seq:
                        stats["skipped_retrans"] += 1
                        if stats["skipped_retrans"] <= 3:
                            print(f"{Colors.GRAY}[DBG] SKIP full retrans: end_seq={end_seq} <= next_seq={stream.next_seq}{Colors.RESET}")
                        # Don't continue - still call process() which handles it
                    elif tcp["seq"] < stream.next_seq:
                        new_bytes = end_seq - stream.next_seq
                        if stats["tcp_data"] <= 20:
                            print(f"{Colors.GRAY}[DBG] OVERLAP: {new_bytes} new bytes "
                                  f"(skip {stream.next_seq - tcp['seq']}){Colors.RESET}")

                results = tracker.process(
                    src_ip, src_port, dst_ip, dst_port,
                    tcp["seq"], payload,
                )
                stats["fed"] += 1

                if results:
                    stats["extracted"] += len(results)

                # Debug: show buffer state and hex dump when stuck
                stream_after = tracker.streams.get(key)
                if stream_after:
                    buf_len = len(stream_after.buffer.buffer)
                    if buf_len > 0 and stats["tcp_data"] <= 10:
                        head = bytes(stream_after.buffer.buffer[:40])
                        hex_str = ' '.join(f'{b:02x}' for b in head)
                        # Also check for type URL
                        buf_bytes = bytes(stream_after.buffer.buffer)
                        url_pos = buf_bytes.find(b"type.ankama.com/")
                        url_info = f" type_url@{url_pos}" if url_pos >= 0 else " NO type_url"
                        print(f"{Colors.GRAY}[DBG] buf={buf_len}b framing={stream_after.buffer.framing}"
                              f"{url_info} head=[{hex_str}]{Colors.RESET}")

                for direction, packet_payload in results:
                    messages = extract_message_info(packet_payload, direction)
                    for type_code, msg_data, uid in messages:
                        if _auto_matcher:
                            _auto_matcher.observe(type_code, msg_data or b"", direction)
                        type_name = get_type_name(type_code, _matching)
                        stats["messages"] += 1
                        display_message(
                            type_code, type_name, direction,
                            msg_data, uid, start_time, verbose,
                        )
                        capture.log_message(type_code, type_name, direction, msg_data, uid)
            except Exception as e:
                stats["errors"] += 1
                if stats["errors"] <= 20:
                    import traceback
                    print(f"{Colors.RED}[E] Parse error: {e}{Colors.RESET}")
                    traceback.print_exc()

    except KeyboardInterrupt:
        pass
    finally:
        wd.close()
        capture.stop()

    print()
    print(f"{Colors.BOLD}=== SNIFFER ARRETE ==={Colors.RESET}")
    print(f"  Paquets IP: {stats['packets']}")
    print(f"  Paquets TCP data: {stats['tcp_data']}")
    print(f"  Fed to buffer: {stats['fed']}")
    print(f"  Extracted frames: {stats['extracted']}")
    print(f"  Messages jeu: {stats['messages']}")
    print(f"  Retransmissions: {stats['skipped_retrans']}")
    print(f"  Recv errors: {stats['recv_errors']}")
    print(f"  Parse errors: {stats['errors']}")
    print(f"  Fichier: {capture.filepath}")
    # Show remaining buffer state
    for key, stream in tracker.streams.items():
        buf_len = len(stream.buffer.buffer)
        print(f"  Stream {stream.direction} {key[0]}:{key[1]}->{key[2]}:{key[3]}: "
              f"buf={buf_len}b framing={stream.buffer.framing} next_seq={stream.next_seq}")


# ---------------------------------------------------------------------------
# Main
# ---------------------------------------------------------------------------

if __name__ == "__main__":
    parser = argparse.ArgumentParser(
        description="Dofus 3 Sniffer - WinDivert 2.2 (no npcap needed)",
    )
    parser.add_argument("--server", "-s", default=None, help="Server IP (auto-detect if omitted)")
    parser.add_argument("--port", "-p", type=int, default=5555, help="Server port (default: 5555)")
    parser.add_argument("--verbose", "-v", action="store_true", help="Show protobuf field details")
    args = parser.parse_args()

    # Auto-detect or use provided server IP
    if args.server:
        server_ips = [args.server]
    else:
        print(f"{Colors.YELLOW}[*] Auto-detection des serveurs Dofus...{Colors.RESET}")
        server_ips = detect_game_servers()
        if server_ips:
            for ip in server_ips:
                print(f"{Colors.GREEN}[+] Serveur detecte: {ip}:{args.port}{Colors.RESET}")
        else:
            print(f"{Colors.YELLOW}[*] Aucune connexion active, capture tout le port {args.port}{Colors.RESET}")

    run_wd_sniffer(server_ips or [], args.port, args.verbose)

"""
Packet-level TCP redirect using WinDivert 2.2.

Intercepts game server connections at the OS network stack and redirects
them to the local MITM proxy transparently using NAT-style rewriting.

How it works:
    1. Capture outbound TCP to game server port (not from proxy, not to loopback)
    2. Rewrite dst IP to 127.0.0.1, inject with FRESH addr (Impostor + Outbound)
       → WinDivert 2.x re-routes the packet to the loopback adapter
       → Proxy on 127.0.0.1:5555 receives the connection
    3. Capture loopback return traffic from proxy (src=127.0.0.1, dst=game)
    4. Rewrite src IP back to original game server, clear loopback flag
       → Game's TCP stack sees responses from the "real" server

WinDivert 2.2 is required for:
    - Loopback traffic capture (return path from proxy to game)
    - Impostor flag (force re-routing after IP rewrite)

Requires: Administrator privileges
"""

import struct
import threading

from utils import logger

# The proxy binds its upstream sockets to ports in this range
# so WinDivert can exclude them from capture (prevent infinite loop).
# A range is needed because multiple connections may overlap (TIME_WAIT).
PROXY_UPSTREAM_PORT_BASE = 55555
PROXY_UPSTREAM_PORT_END = 55564  # inclusive, 10 ports
# Legacy alias used by bot.py
PROXY_UPSTREAM_SRC_PORT = PROXY_UPSTREAM_PORT_BASE

try:
    from launcher.windivert2 import (
        WinDivert2, WDAddr, parse_ipv4, parse_tcp,
        set_ipv4_dst, set_ipv4_src,
    )
    HAS_WINDIVERT = True
except ImportError as e:
    HAS_WINDIVERT = False
    _import_error = str(e)


class PacketRedirect:
    """Redirect game server TCP traffic to local proxy using WinDivert 2.2 NAT."""

    def __init__(self, game_port: int = 5555, proxy_port: int = 5555):
        self.game_port = game_port
        self.proxy_port = proxy_port
        self._wd = None
        self._thread = None
        self._running = False
        # NAT table: game_src_port -> original_dst_ip
        self._nat_table = {}
        self._stats = {
            "outbound": 0, "inbound": 0,
            "redirected": 0, "errors": 0,
            "send_errors": 0,
        }

    def start(self):
        """Start the WinDivert packet redirect."""
        if not HAS_WINDIVERT:
            raise RuntimeError(f"WinDivert 2.2 not available: {_import_error}")

        # Filter (two clauses, OR):
        # 1. Outbound to game port, NOT loopback, NOT from proxy upstream
        # 2. Return from proxy to our fake client: src=127.0.0.1, dst=127.0.0.2
        filt = (
            f"(tcp.DstPort == {self.game_port} "
            f"and ip.DstAddr != 127.0.0.1 "
            f"and ip.SrcAddr != 127.0.0.1 "
            f"and (tcp.SrcPort < {PROXY_UPSTREAM_PORT_BASE} or tcp.SrcPort > {PROXY_UPSTREAM_PORT_END}))"
            f" or "
            f"(tcp.SrcPort == {self.proxy_port} "
            f"and ip.SrcAddr == 127.0.0.1 "
            f"and ip.DstAddr == 127.0.0.2)"
        )

        logger.info(f"  WinDivert 2.2 filter: {filt}")

        try:
            self._wd = WinDivert2(filt)
            self._wd.open()
        except Exception as e:
            logger.error(f"  WinDivert 2.2 failed to open: {e}")
            logger.error("  Make sure to run as Administrator")
            raise

        self._running = True
        self._thread = threading.Thread(target=self._redirect_loop, daemon=True)
        self._thread.start()
        logger.info(f"  Packet redirect ACTIVE (port {self.game_port} -> 127.0.0.1:{self.proxy_port})")
        logger.info(f"  Using FRESH WDAddr for injection (Impostor + Outbound)")
        logger.info(f"  Proxy upstream excluded: src_port={PROXY_UPSTREAM_SRC_PORT}")

    def _redirect_loop(self):
        """Main packet capture and redirect loop."""
        while self._running:
            try:
                raw, addr = self._wd.recv()
            except Exception:
                if self._running:
                    continue
                break

            try:
                self._process_packet(raw, addr)
            except Exception as e:
                self._stats["errors"] += 1
                logger.warn(f"  [WD] Error: {e}")
                # Try to pass through unmodified
                try:
                    self._wd.send(raw, addr)
                except Exception:
                    pass

    def _process_packet(self, raw: bytes, addr: WDAddr):
        """Process a single captured packet."""
        ip = parse_ipv4(raw)
        if not ip or ip["protocol"] != 6:  # TCP only
            self._wd.send(raw, addr)
            return

        tcp = parse_tcp(raw, ip["ihl"])
        if not tcp:
            self._wd.send(raw, addr)
            return

        src_ip = ip["src_ip"]
        dst_ip = ip["dst_ip"]
        src_port = tcp["src_port"]
        dst_port = tcp["dst_port"]

        # Build flags string for logging
        flags = []
        if tcp["syn"]: flags.append("SYN")
        if tcp["ack_flag"]: flags.append("ACK")
        if tcp["fin"]: flags.append("FIN")
        if tcp["rst"]: flags.append("RST")
        if tcp["psh"]: flags.append("PSH")
        flag_str = "+".join(flags) if flags else "---"

        is_syn = tcp["syn"] and not tcp["ack_flag"]
        is_synack = tcp["syn"] and tcp["ack_flag"]

        # Case 1: Traffic to server (dst=server:5555, src!=proxy, src!=loopback)
        if (dst_port == self.game_port
                and dst_ip != "127.0.0.1"
                and src_port != PROXY_UPSTREAM_SRC_PORT
                and src_ip != "127.0.0.1"):
            self._handle_outbound(raw, addr, src_ip, dst_ip, src_port, dst_port, flag_str, is_syn)
            return

        # Case 2: Return traffic from proxy (src=127.0.0.1:proxy_port, dst=127.0.0.2)
        if src_ip == "127.0.0.1" and src_port == self.proxy_port and dst_ip == "127.0.0.2":
            self._handle_return(raw, addr, src_ip, dst_ip, src_port, dst_port, flag_str, is_synack)
            return

        # Unknown packet matching our filter, pass through
        logger.info(
            f"  [WD?] {'OUT' if addr.outbound else 'IN'}"
            f"{'(lo)' if addr.loopback else ''} "
            f"{src_ip}:{src_port} -> {dst_ip}:{dst_port} [{flag_str}]"
        )
        self._wd.send(raw, addr)

    def _handle_outbound(self, raw, addr, src_ip, dst_ip, src_port, dst_port,
                         flag_str, is_syn):
        """Outbound: game -> server. Redirect to local proxy via full NAT.

        Windows loopback adapter rejects packets with non-loopback source IP.
        So we rewrite BOTH src and dst to loopback addresses:
          src: 192.168.x.x -> 127.0.0.2  (fake client on loopback)
          dst: game_server  -> 127.0.0.1  (our proxy)
        """
        # Record original IPs + interface info in NAT table for return path
        self._nat_table[src_port] = (src_ip, dst_ip, addr.if_idx, addr.sub_if_idx)

        # Full NAT: rewrite both src and dst to loopback
        modified = bytearray(raw)
        set_ipv4_src(modified, "127.0.0.2")
        set_ipv4_dst(modified, "127.0.0.1")

        # Impostor=True: re-compute routing for the new dst
        # Loopback=True: inject on the loopback adapter
        inject_addr = WDAddr.make_inject(outbound=True, impostor=True, loopback=True)

        self._stats["outbound"] += 1
        self._stats["redirected"] += 1

        if is_syn:
            logger.info(
                f"  [REDIRECT] {src_ip}:{src_port} -> {dst_ip}:{dst_port}"
                f" => 127.0.0.2:{src_port} -> 127.0.0.1:{self.proxy_port} [{flag_str}]"
                f" (if={addr.if_idx}/{addr.sub_if_idx})"
            )

        sent, err = self._wd.send(bytes(modified), inject_addr)
        if err:
            self._stats["send_errors"] += 1
            logger.error(f"  [WD] SEND FAILED (outbound): {err}")
        elif is_syn:
            logger.info(f"  [WD] Send OK ({sent} bytes)")

    def _handle_return(self, raw, addr, src_ip, dst_ip, src_port, dst_port,
                       flag_str, is_synack):
        """Return: proxy -> game. Restore original IPs.

        Proxy responds: src=127.0.0.1:5555, dst=127.0.0.2:client_port
        We restore:     src=game_server:5555, dst=original_client:client_port
        """
        entry = self._nat_table.get(dst_port)
        if not entry:
            # No NAT entry, pass through
            self._wd.send(raw, addr)
            return

        original_client_ip, original_server_ip, orig_if_idx, orig_sub_if_idx = entry

        # Restore both IPs
        modified = bytearray(raw)
        set_ipv4_src(modified, original_server_ip)
        set_ipv4_dst(modified, original_client_ip)

        # Inbound packet from "game server" to the real client
        # Must inject on the same physical NIC interface that the outbound SYN used
        inject_addr = WDAddr.make_inject(outbound=False, impostor=True, loopback=False)
        inject_addr.if_idx = orig_if_idx
        inject_addr.sub_if_idx = orig_sub_if_idx

        self._stats["inbound"] += 1

        if is_synack:
            logger.info(
                f"  [RETURN] 127.0.0.1:{src_port} -> 127.0.0.2:{dst_port}"
                f" => {original_server_ip}:{src_port} -> {original_client_ip}:{dst_port} [{flag_str}]"
                f" (if={orig_if_idx}/{orig_sub_if_idx})"
            )

        sent, err = self._wd.send(bytes(modified), inject_addr)
        if err:
            self._stats["send_errors"] += 1
            logger.error(f"  [WD] SEND FAILED (return): {err}")
        elif is_synack:
            logger.info(f"  [WD] Return send OK ({sent} bytes)")

    def get_stats(self) -> dict:
        """Get redirect statistics."""
        return dict(self._stats)

    def stop(self):
        """Stop the packet redirect."""
        self._running = False
        if self._wd:
            try:
                self._wd.close()
            except Exception:
                pass
        if self._thread:
            self._thread.join(timeout=2)
        logger.info("  WinDivert packet redirect stopped")
        logger.info(f"  Stats: {self._stats}")

from launcher.windivert2 import WinDivert2, parse_ipv4, parse_tcp
from proxy.packet_handler import PacketBuffer
from game.dofus_message import extract_message_info
import subprocess, time

SNIFF = 0x0001
RECV = 0x0004

# detect server IPs
result = subprocess.run(
    ["powershell", "-Command",
     "Get-NetTCPConnection -State Established -RemotePort 5555 | Select-Object -ExpandProperty RemoteAddress"],
    capture_output=True, text=True, timeout=5
)
server_ips = set()
for line in result.stdout.strip().split():
    l = line.strip()
    if l and not l.startswith("127."):
        server_ips.add(l)
print("Servers:", server_ips)

wd = WinDivert2("tcp.DstPort==5555 or tcp.SrcPort==5555", flags=SNIFF|RECV)
wd.open()
print("WinDivert ouvert, capture 15 secondes...")

from collections import defaultdict
streams_c2s = defaultdict(lambda: PacketBuffer())
streams_s2c = defaultdict(lambda: PacketBuffer())

count = 0
start = time.time()

while time.time() - start < 15:
    try:
        raw, addr = wd.recv()
    except Exception:
        continue
    ip = parse_ipv4(raw)
    if not ip or ip["protocol"] != 6:
        continue
    tcp = parse_tcp(raw, ip["ihl"])
    if not tcp:
        continue
    off = ip["ihl"] + tcp["data_off"]
    p = raw[off:min(ip["total_len"], len(raw))]
    if not p:
        continue

    src_ip = ip["src_ip"]
    dst_ip = ip["dst_ip"]

    if dst_ip in server_ips:
        direction = "c2s"
        buf = streams_c2s[(src_ip, tcp["src_port"])]
    elif src_ip in server_ips:
        direction = "s2c"
        buf = streams_s2c[(src_ip, tcp["src_port"])]
    else:
        continue

    buf.feed(p)
    for frame in buf.try_extract_packets():
        msgs = extract_message_info(frame, direction)
        count += 1
        if msgs:
            for code, data, uid in msgs:
                sz = len(data) if data else 0
                print(f"[{direction}] {code} size={sz} uid={uid}")
        else:
            print(f"[{direction}] frame#{count} len={len(frame)} NO MSG hex={frame[:16].hex()}")
        if count >= 30:
            break
    if count >= 30:
        break

wd.close()
print(f"Total frames: {count}")

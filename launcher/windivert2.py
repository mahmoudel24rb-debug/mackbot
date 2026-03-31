"""
WinDivert 2.2 ctypes wrapper.

Uses the WinDivert 2.2.2 DLL directly (from launcher/windivert/) instead
of pydivert's bundled WinDivert 1.3 which lacks loopback + Impostor support.

Key improvements over pydivert:
    - Full 80-byte WINDIVERT_ADDRESS structure
    - Loopback traffic capture/injection
    - Impostor flag for forced re-routing after dst IP changes
"""

import ctypes
import os
import struct
from ctypes import (
    WinDLL, POINTER, byref, c_char, c_char_p, c_int, c_int16,
    c_uint, c_uint8, c_uint32, c_uint64, c_void_p, windll,
)
from ctypes.wintypes import HANDLE

# ---------------------------------------------------------------------------
# DLL path
# ---------------------------------------------------------------------------
_HERE = os.path.abspath(os.path.dirname(__file__))
DLL_PATH = os.path.join(_HERE, "windivert", "WinDivert.dll")

# ---------------------------------------------------------------------------
# WINDIVERT_ADDRESS (80 bytes for WinDivert 2.x)
# ---------------------------------------------------------------------------
ADDR_SIZE = 80

# Byte offsets within the address buffer
_OFF_TIMESTAMP = 0   # INT64  (8 bytes)
_OFF_FLAGS     = 8   # UINT32 bitfield (4 bytes): Layer:8, Event:8, flags:8, Reserved1:8
_OFF_RESERVED2 = 12  # UINT32 (4 bytes)
_OFF_IFIDX     = 16  # UINT32 Network.IfIdx
_OFF_SUBIFIDX  = 20  # UINT32 Network.SubIfIdx

# Bit masks within the flags byte (offset 10)
_FLAGS_BYTE = 10
_BIT_SNIFFED     = 0x01
_BIT_OUTBOUND    = 0x02
_BIT_LOOPBACK    = 0x04
_BIT_IMPOSTOR    = 0x08
_BIT_IPV6        = 0x10
_BIT_IPCHECKSUM  = 0x20
_BIT_TCPCHECKSUM = 0x40
_BIT_UDPCHECKSUM = 0x80


class WDAddr:
    """Thin wrapper over WinDivert 2.x address buffer (80 bytes)."""

    __slots__ = ("buf",)

    def __init__(self):
        self.buf = (c_char * ADDR_SIZE)()

    def _flag(self, mask):
        return bool(self.buf[_FLAGS_BYTE] & mask)

    def _set_flag(self, mask, val):
        b = self.buf[_FLAGS_BYTE]
        if isinstance(b, int):
            byte_val = b
        else:
            byte_val = b[0] if isinstance(b, bytes) else ord(b)
        if val:
            byte_val |= mask
        else:
            byte_val &= ~mask & 0xFF
        self.buf[_FLAGS_BYTE] = byte_val

    @property
    def outbound(self):
        b = self.buf[_FLAGS_BYTE]
        v = b if isinstance(b, int) else (b[0] if isinstance(b, bytes) else ord(b))
        return bool(v & _BIT_OUTBOUND)

    @outbound.setter
    def outbound(self, val):
        self._set_flag(_BIT_OUTBOUND, val)

    @property
    def loopback(self):
        b = self.buf[_FLAGS_BYTE]
        v = b if isinstance(b, int) else (b[0] if isinstance(b, bytes) else ord(b))
        return bool(v & _BIT_LOOPBACK)

    @loopback.setter
    def loopback(self, val):
        self._set_flag(_BIT_LOOPBACK, val)

    @property
    def impostor(self):
        b = self.buf[_FLAGS_BYTE]
        v = b if isinstance(b, int) else (b[0] if isinstance(b, bytes) else ord(b))
        return bool(v & _BIT_IMPOSTOR)

    @impostor.setter
    def impostor(self, val):
        self._set_flag(_BIT_IMPOSTOR, val)

    @property
    def if_idx(self):
        return struct.unpack_from("<I", self.buf, _OFF_IFIDX)[0]

    @if_idx.setter
    def if_idx(self, val):
        struct.pack_into("<I", self.buf, _OFF_IFIDX, val)

    @property
    def sub_if_idx(self):
        return struct.unpack_from("<I", self.buf, _OFF_SUBIFIDX)[0]

    @sub_if_idx.setter
    def sub_if_idx(self, val):
        struct.pack_into("<I", self.buf, _OFF_SUBIFIDX, val)

    def dump_flags_byte(self):
        """Return hex string of the flags byte for debugging."""
        b = self.buf[_FLAGS_BYTE]
        v = b if isinstance(b, int) else (b[0] if isinstance(b, bytes) else ord(b))
        return f"0x{v:02x}"

    @staticmethod
    def make_inject(outbound=True, impostor=True, loopback=False):
        """Create a clean WDAddr for packet injection (no stale interface data)."""
        addr = WDAddr()
        addr.outbound = outbound
        addr.impostor = impostor
        addr.loopback = loopback
        return addr

    def __repr__(self):
        return (
            f"WDAddr(out={self.outbound}, loop={self.loopback}, "
            f"imp={self.impostor}, if={self.if_idx}/{self.sub_if_idx}, "
            f"flags={self.dump_flags_byte()})"
        )


# ---------------------------------------------------------------------------
# DLL function loading
# ---------------------------------------------------------------------------
_dll = None


def _cleanup_old_driver():
    """Try to stop/remove any old WinDivert driver from pydivert 1.3."""
    import subprocess
    for svc in ("WinDivert", "WinDivert64", "WinDivert14"):
        try:
            subprocess.run(
                ["sc", "stop", svc],
                capture_output=True, timeout=5,
            )
            subprocess.run(
                ["sc", "delete", svc],
                capture_output=True, timeout=5,
            )
        except Exception:
            pass


def _load_dll():
    global _dll
    if _dll is not None:
        return _dll

    if not os.path.exists(DLL_PATH):
        raise FileNotFoundError(f"WinDivert 2.2 DLL not found: {DLL_PATH}")

    # Clean up any old WinDivert 1.x drivers that might conflict
    _cleanup_old_driver()

    _dll = WinDLL(DLL_PATH)

    # WinDivertOpen
    _dll.WinDivertOpen.argtypes = [c_char_p, c_int, c_int16, c_uint64]
    _dll.WinDivertOpen.restype = HANDLE

    # WinDivertRecv (WinDivert 2.x order: pRecvLen BEFORE pAddr)
    _dll.WinDivertRecv.argtypes = [HANDLE, c_void_p, c_uint, POINTER(c_uint), c_void_p]
    _dll.WinDivertRecv.restype = c_int

    # WinDivertSend (WinDivert 2.x order: pSendLen BEFORE pAddr)
    _dll.WinDivertSend.argtypes = [HANDLE, c_void_p, c_uint, POINTER(c_uint), c_void_p]
    _dll.WinDivertSend.restype = c_int

    # WinDivertClose
    _dll.WinDivertClose.argtypes = [HANDLE]
    _dll.WinDivertClose.restype = c_int

    # WinDivertHelperCalcChecksums
    _dll.WinDivertHelperCalcChecksums.argtypes = [c_void_p, c_uint, c_void_p, c_uint64]
    _dll.WinDivertHelperCalcChecksums.restype = c_int

    # WinDivertSetParam
    _dll.WinDivertSetParam.argtypes = [HANDLE, c_int, c_uint64]
    _dll.WinDivertSetParam.restype = c_int

    return _dll


# ---------------------------------------------------------------------------
# WinDivert handle wrapper
# ---------------------------------------------------------------------------
# WinDivert layers
WINDIVERT_LAYER_NETWORK = 0

# WinDivert params
WINDIVERT_PARAM_QUEUE_LENGTH = 0
WINDIVERT_PARAM_QUEUE_TIME = 1
WINDIVERT_PARAM_QUEUE_SIZE = 2


class WinDivert2:
    """WinDivert 2.2 handle with proper address support."""

    def __init__(self, filter_str: str, layer: int = WINDIVERT_LAYER_NETWORK,
                 priority: int = 0, flags: int = 0):
        self._filter = filter_str
        self._layer = layer
        self._priority = priority
        self._flags = flags
        self._handle = None
        self._dll = None

    def open(self):
        self._dll = _load_dll()
        windll.kernel32.SetLastError(0)
        handle = self._dll.WinDivertOpen(
            self._filter.encode("ascii"),
            c_int(self._layer),
            c_int16(self._priority),
            c_uint64(self._flags),
        )
        err = ctypes.GetLastError()
        windll.kernel32.SetLastError(0)
        if err:
            raise ctypes.WinError(err)
        self._handle = handle

    def set_param(self, param, value):
        ok = self._dll.WinDivertSetParam(self._handle, c_int(param), c_uint64(value))
        if not ok:
            err = ctypes.GetLastError()
            if err:
                windll.kernel32.SetLastError(0)

    def recv(self, bufsize=65535):
        """Receive a packet. Returns (raw_bytes, WDAddr)."""
        buf = (c_char * bufsize)()
        addr = WDAddr()
        recv_len = c_uint(0)

        windll.kernel32.SetLastError(0)
        ok = self._dll.WinDivertRecv(
            self._handle, buf, c_uint(bufsize),
            byref(recv_len), addr.buf,
        )
        if not ok:
            err = ctypes.GetLastError()
            windll.kernel32.SetLastError(0)
            raise ctypes.WinError(err)

        raw = bytes(buf[:recv_len.value])
        return raw, addr

    def send(self, raw: bytes, addr: WDAddr, recalc_checksums=True):
        """Send (re-inject) a packet. Returns (send_len, error_str or None)."""
        if recalc_checksums:
            pkt_buf = (c_char * len(raw)).from_buffer_copy(raw)
            self._dll.WinDivertHelperCalcChecksums(
                pkt_buf, c_uint(len(raw)), addr.buf, c_uint64(0),
            )
            raw = bytes(pkt_buf)

        send_buf = (c_char * len(raw)).from_buffer_copy(raw)
        send_len = c_uint(0)

        windll.kernel32.SetLastError(0)
        ok = self._dll.WinDivertSend(
            self._handle, send_buf, c_uint(len(raw)),
            byref(send_len), addr.buf,
        )
        if not ok:
            err = ctypes.GetLastError()
            windll.kernel32.SetLastError(0)
            err_msg = f"WinDivertSend failed: {ctypes.WinError(err)}"
            return 0, err_msg

        return send_len.value, None

    def close(self):
        if self._handle is not None:
            self._dll.WinDivertClose(self._handle)
            self._handle = None

    def __enter__(self):
        self.open()
        return self

    def __exit__(self, *args):
        self.close()


# ---------------------------------------------------------------------------
# Packet helpers
# ---------------------------------------------------------------------------

def parse_ipv4(raw: bytes):
    """Parse IPv4 header fields. Returns dict or None."""
    if len(raw) < 20:
        return None
    ver_ihl = raw[0]
    ver = ver_ihl >> 4
    if ver != 4:
        return None
    ihl = (ver_ihl & 0x0F) * 4
    total_len = struct.unpack_from("!H", raw, 2)[0]
    protocol = raw[9]
    src_ip = f"{raw[12]}.{raw[13]}.{raw[14]}.{raw[15]}"
    dst_ip = f"{raw[16]}.{raw[17]}.{raw[18]}.{raw[19]}"
    return {
        "ihl": ihl,
        "total_len": total_len,
        "protocol": protocol,
        "src_ip": src_ip,
        "dst_ip": dst_ip,
    }


def parse_tcp(raw: bytes, ip_hdr_len: int):
    """Parse TCP header fields. Returns dict or None."""
    off = ip_hdr_len
    if len(raw) < off + 20:
        return None
    src_port = struct.unpack_from("!H", raw, off)[0]
    dst_port = struct.unpack_from("!H", raw, off + 2)[0]
    seq = struct.unpack_from("!I", raw, off + 4)[0]
    ack = struct.unpack_from("!I", raw, off + 8)[0]
    data_off = (raw[off + 12] >> 4) * 4
    flags_byte = raw[off + 13]
    fin = bool(flags_byte & 0x01)
    syn = bool(flags_byte & 0x02)
    rst = bool(flags_byte & 0x04)
    psh = bool(flags_byte & 0x08)
    ack_flag = bool(flags_byte & 0x10)
    payload_off = off + data_off
    payload_len = len(raw) - payload_off if payload_off < len(raw) else 0
    return {
        "src_port": src_port,
        "dst_port": dst_port,
        "seq": seq,
        "ack": ack,
        "data_off": data_off,
        "syn": syn,
        "ack_flag": ack_flag,
        "fin": fin,
        "rst": rst,
        "psh": psh,
        "payload_off": payload_off,
        "payload_len": payload_len,
    }


def set_ipv4_dst(raw: bytearray, ip: str):
    """Set the destination IPv4 address in-place."""
    parts = [int(x) for x in ip.split(".")]
    raw[16] = parts[0]
    raw[17] = parts[1]
    raw[18] = parts[2]
    raw[19] = parts[3]


def set_ipv4_src(raw: bytearray, ip: str):
    """Set the source IPv4 address in-place."""
    parts = [int(x) for x in ip.split(".")]
    raw[12] = parts[0]
    raw[13] = parts[1]
    raw[14] = parts[2]
    raw[15] = parts[3]

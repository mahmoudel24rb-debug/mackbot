"""
Winsock Hook - Pure Python x64 inline hook for ws2_32!connect.

Redirects game server connections (port 5555) to 127.0.0.1 (our MITM proxy).
Uses ctypes to write x64 machine code directly into the target process.
No compiler or external DLL needed.

How it works:
    1. Get address of ws2_32.dll!connect (same across all processes per boot)
    2. Open Dofus.exe, allocate executable memory
    3. Write hook function (x64 asm): if port==5555, change IP to 127.0.0.1
    4. Overwrite connect() prologue with a JMP to our hook
    5. Hook executes saved prologue bytes then JMPs back to original+N
       where N is aligned to instruction boundaries (not a fixed 14!)

Also hooks mswsock.dll!ConnectEx (used by .NET async sockets / DotNetty).
"""

import ctypes
from ctypes import wintypes
import socket
import struct
import subprocess

from utils import logger

# Windows API constants
PROCESS_ALL_ACCESS = 0x001F0FFF
MEM_COMMIT = 0x00001000
MEM_RESERVE = 0x00002000
PAGE_EXECUTE_READWRITE = 0x40

# Minimum JMP stub: FF 25 00 00 00 00 + 8-byte addr = 14 bytes
JMP_STUB_SIZE = 14

# Module-level storage for hook counter addresses (for diagnostic reads)
# Maps func_name -> (hook_addr, counter_offset_in_hook)
_hook_counter_info = {}

# ctypes kernel32 with proper error handling
kernel32 = ctypes.WinDLL('kernel32', use_last_error=True)

# Function prototypes
kernel32.OpenProcess.argtypes = [wintypes.DWORD, wintypes.BOOL, wintypes.DWORD]
kernel32.OpenProcess.restype = wintypes.HANDLE

kernel32.ReadProcessMemory.argtypes = [
    wintypes.HANDLE, ctypes.c_void_p, ctypes.c_void_p,
    ctypes.c_size_t, ctypes.POINTER(ctypes.c_size_t),
]
kernel32.ReadProcessMemory.restype = wintypes.BOOL

kernel32.WriteProcessMemory.argtypes = [
    wintypes.HANDLE, ctypes.c_void_p, ctypes.c_void_p,
    ctypes.c_size_t, ctypes.POINTER(ctypes.c_size_t),
]
kernel32.WriteProcessMemory.restype = wintypes.BOOL

kernel32.VirtualAllocEx.argtypes = [
    wintypes.HANDLE, ctypes.c_void_p, ctypes.c_size_t,
    wintypes.DWORD, wintypes.DWORD,
]
kernel32.VirtualAllocEx.restype = ctypes.c_void_p

kernel32.VirtualProtectEx.argtypes = [
    wintypes.HANDLE, ctypes.c_void_p, ctypes.c_size_t,
    wintypes.DWORD, ctypes.POINTER(wintypes.DWORD),
]
kernel32.VirtualProtectEx.restype = wintypes.BOOL

kernel32.FlushInstructionCache.argtypes = [
    wintypes.HANDLE, ctypes.c_void_p, ctypes.c_size_t,
]
kernel32.FlushInstructionCache.restype = wintypes.BOOL

kernel32.CloseHandle.argtypes = [wintypes.HANDLE]
kernel32.CloseHandle.restype = wintypes.BOOL

kernel32.GetProcAddress.argtypes = [wintypes.HMODULE, ctypes.c_char_p]
kernel32.GetProcAddress.restype = ctypes.c_void_p

kernel32.LoadLibraryA.argtypes = [ctypes.c_char_p]
kernel32.LoadLibraryA.restype = wintypes.HMODULE

kernel32.CreateRemoteThread.argtypes = [
    wintypes.HANDLE, ctypes.c_void_p, ctypes.c_size_t,
    ctypes.c_void_p, ctypes.c_void_p, wintypes.DWORD,
    ctypes.POINTER(wintypes.DWORD),
]
kernel32.CreateRemoteThread.restype = wintypes.HANDLE

kernel32.WaitForSingleObject.argtypes = [wintypes.HANDLE, wintypes.DWORD]
kernel32.WaitForSingleObject.restype = wintypes.DWORD

kernel32.VirtualFreeEx.argtypes = [
    wintypes.HANDLE, ctypes.c_void_p, ctypes.c_size_t, wintypes.DWORD,
]
kernel32.VirtualFreeEx.restype = wintypes.BOOL

kernel32.IsWow64Process.argtypes = [wintypes.HANDLE, ctypes.POINTER(wintypes.BOOL)]
kernel32.IsWow64Process.restype = wintypes.BOOL

# psapi for enumerating modules in remote process
psapi = ctypes.WinDLL('psapi', use_last_error=True)

psapi.EnumProcessModulesEx.argtypes = [
    wintypes.HANDLE, ctypes.POINTER(wintypes.HMODULE), wintypes.DWORD,
    ctypes.POINTER(wintypes.DWORD), wintypes.DWORD,
]
psapi.EnumProcessModulesEx.restype = wintypes.BOOL

psapi.GetModuleBaseNameA.argtypes = [
    wintypes.HANDLE, wintypes.HMODULE, ctypes.c_char_p, wintypes.DWORD,
]
psapi.GetModuleBaseNameA.restype = wintypes.DWORD


# ---------------------------------------------------------------------------
# x64 Instruction Length Decoder (minimal, for function prologues)
# ---------------------------------------------------------------------------

def _x64_insn_len(code: bytes, offset: int = 0) -> int:
    """Calculate length of one x64 instruction starting at offset.

    Minimal decoder sufficient for typical Windows function prologues.
    Handles: REX prefixes, 1-byte and 2-byte opcodes, ModRM/SIB,
    displacement, and immediate operands.
    """
    i = offset
    end = len(code)

    if i >= end:
        return 0

    # --- Legacy prefixes ---
    has_66 = False
    while i < end and code[i] in (0x26, 0x2E, 0x36, 0x3E, 0x64, 0x65, 0x66, 0x67, 0xF0, 0xF2, 0xF3):
        if code[i] == 0x66:
            has_66 = True
        i += 1

    if i >= end:
        return i - offset

    # --- REX prefix (0x40-0x4F) ---
    rex_w = False
    if 0x40 <= code[i] <= 0x4F:
        rex_w = bool(code[i] & 0x08)
        i += 1

    if i >= end:
        return i - offset

    # --- Opcode ---
    op = code[i]
    i += 1
    two_byte = False

    if op == 0x0F:
        two_byte = True
        if i >= end:
            return i - offset
        op = code[i]
        i += 1

    # --- Determine ModRM presence and immediate size ---
    has_modrm = False
    imm_size = 0

    if two_byte:
        # Most 0F XX opcodes have ModRM
        has_modrm = True
        # Some specific 0F opcodes have immediates
        if op in (0x70, 0x71, 0x72, 0x73, 0xA4, 0xAC, 0xBA, 0xC2, 0xC4, 0xC5, 0xC6):
            imm_size = 1
        elif 0x80 <= op <= 0x8F:
            # Jcc rel32 (0F 80 - 0F 8F)
            has_modrm = False
            imm_size = 4
        elif op in (0xB6, 0xB7, 0xBE, 0xBF):
            # MOVZX/MOVSX - ModRM, no immediate
            has_modrm = True
    else:
        # --- One-byte opcode classification ---
        if op <= 0x3F:
            # ALU instructions: ADD/OR/ADC/SBB/AND/SUB/XOR/CMP
            low = op & 0x07
            if low <= 3:
                has_modrm = True
            elif low == 4:
                imm_size = 1        # AL, imm8
            elif low == 5:
                imm_size = 4        # rAX, imm32
            # 6, 7 = PUSH/POP seg (invalid in x64 long mode for some)
        elif 0x50 <= op <= 0x5F:
            pass  # PUSH/POP reg - no ModRM, no imm
        elif op in (0x63,):
            has_modrm = True  # MOVSXD
        elif op == 0x68:
            imm_size = 4  # PUSH imm32
        elif op == 0x69:
            has_modrm = True
            imm_size = 2 if has_66 else 4
        elif op == 0x6A:
            imm_size = 1  # PUSH imm8
        elif op == 0x6B:
            has_modrm = True
            imm_size = 1
        elif 0x70 <= op <= 0x7F:
            imm_size = 1  # Jcc rel8
        elif op in (0x80, 0x82):
            has_modrm = True
            imm_size = 1
        elif op == 0x81:
            has_modrm = True
            imm_size = 2 if has_66 else 4
        elif op == 0x83:
            has_modrm = True
            imm_size = 1
        elif 0x84 <= op <= 0x8E:
            has_modrm = True  # TEST, XCHG, MOV, LEA, MOV seg
        elif op == 0x8F:
            has_modrm = True  # POP r/m
        elif 0x90 <= op <= 0x97:
            pass  # NOP / XCHG rAX, reg
        elif op in (0x98, 0x99, 0x9B, 0x9C, 0x9D, 0x9E, 0x9F):
            pass  # CBW/CWD/WAIT/PUSHF/POPF/SAHF/LAHF
        elif op in (0xA0, 0xA1):
            imm_size = 8  # MOV AL/rAX, moffs64
        elif op in (0xA2, 0xA3):
            imm_size = 8  # MOV moffs64, AL/rAX
        elif op == 0xA8:
            imm_size = 1  # TEST AL, imm8
        elif op == 0xA9:
            imm_size = 2 if has_66 else 4  # TEST rAX, imm
        elif 0xA4 <= op <= 0xA7 or 0xAA <= op <= 0xAF:
            pass  # String ops
        elif 0xB0 <= op <= 0xB7:
            imm_size = 1  # MOV r8, imm8
        elif 0xB8 <= op <= 0xBF:
            imm_size = 8 if rex_w else 4  # MOV r64, imm64 / MOV r32, imm32
        elif op in (0xC0, 0xC1):
            has_modrm = True
            imm_size = 1  # Shift r/m, imm8
        elif op == 0xC2:
            imm_size = 2  # RET imm16
        elif op == 0xC3:
            pass  # RET
        elif op == 0xC6:
            has_modrm = True
            imm_size = 1  # MOV r/m8, imm8
        elif op == 0xC7:
            has_modrm = True
            imm_size = 2 if has_66 else 4  # MOV r/m, imm
        elif op == 0xCC:
            pass  # INT3
        elif op == 0xCD:
            imm_size = 1  # INT imm8
        elif op in (0xD0, 0xD1, 0xD2, 0xD3):
            has_modrm = True  # Shifts
        elif op == 0xE8:
            imm_size = 4  # CALL rel32
        elif op == 0xE9:
            imm_size = 4  # JMP rel32
        elif op == 0xEB:
            imm_size = 1  # JMP rel8
        elif op in (0xF6,):
            has_modrm = True
            # F6 /0-1 = TEST r/m8, imm8 (has imm), others don't
            if i < end:
                reg = (code[i] >> 3) & 7
                if reg <= 1:
                    imm_size = 1
        elif op in (0xF7,):
            has_modrm = True
            if i < end:
                reg = (code[i] >> 3) & 7
                if reg <= 1:
                    imm_size = 2 if has_66 else 4
        elif op in (0xFE, 0xFF):
            has_modrm = True

    # --- ModRM + SIB + displacement ---
    if has_modrm:
        if i >= end:
            return i - offset
        modrm = code[i]
        i += 1
        mod = (modrm >> 6) & 3
        rm = modrm & 7

        if mod != 3:  # Memory operand
            if rm == 4:  # SIB follows
                if i >= end:
                    return i - offset
                sib = code[i]
                i += 1
                base = sib & 7
                if mod == 0 and base == 5:
                    i += 4  # disp32 with SIB
            elif mod == 0 and rm == 5:
                i += 4  # RIP-relative disp32

            if mod == 1:
                i += 1  # disp8
            elif mod == 2:
                i += 4  # disp32

    # --- Immediate ---
    i += imm_size

    return i - offset


def _find_stub_size(prologue: bytes, min_size: int = JMP_STUB_SIZE) -> int:
    """Find smallest instruction-aligned size >= min_size.

    Walks through instructions until we've covered at least min_size bytes,
    ensuring we never cut an instruction in the middle.
    """
    pos = 0
    while pos < min_size:
        length = _x64_insn_len(prologue, pos)
        if length == 0:
            # Decoder failed, fallback to min_size (risky but better than infinite loop)
            logger.warn(f"  Instruction decoder failed at offset {pos}")
            return min_size
        pos += length
    return pos


# ---------------------------------------------------------------------------
# Helpers
# ---------------------------------------------------------------------------

def find_dofus_pid() -> int | None:
    """Find Dofus.exe PID using PowerShell. Returns first PID if multiple."""
    try:
        result = subprocess.run(
            ["powershell", "-NoProfile", "-Command",
             "(Get-Process -Name Dofus -ErrorAction SilentlyContinue).Id"],
            capture_output=True, timeout=5,
        )
        stdout = result.stdout.decode("utf-8", errors="replace").strip()
        if not stdout:
            return None
        # Handle multiple PIDs (one per line)
        for line in stdout.split("\n"):
            line = line.strip()
            if line.isdigit():
                return int(line)
    except Exception:
        pass
    return None


def get_function_address(dll_name: str, func_name: str) -> int | None:
    """Get function address (valid across processes for system DLLs)."""
    h_module = kernel32.LoadLibraryA(dll_name.encode())
    if not h_module:
        return None
    addr = kernel32.GetProcAddress(h_module, func_name.encode())
    return addr


MEM_RELEASE = 0x00008000
INFINITE = 0xFFFFFFFF


def _ensure_dll_loaded(h_process: int, dll_name: str) -> bool:
    """Force-load a DLL into the target process via CreateRemoteThread.

    Uses the classic DLL injection technique:
    1. Allocate memory in target for the DLL name string
    2. Write the DLL name
    3. CreateRemoteThread calling LoadLibraryA with our string
    4. Wait for completion, clean up
    """
    dll_bytes = dll_name.encode("ascii") + b'\x00'

    # Get LoadLibraryA address (same across processes)
    load_lib_addr = kernel32.GetProcAddress(
        kernel32.LoadLibraryA(b"kernel32.dll"),
        b"LoadLibraryA",
    )
    if not load_lib_addr:
        logger.warn(f"  Cannot find LoadLibraryA for remote injection")
        return False

    # Allocate memory for DLL name string in target
    str_addr = kernel32.VirtualAllocEx(
        h_process, None, len(dll_bytes),
        MEM_COMMIT | MEM_RESERVE, PAGE_EXECUTE_READWRITE,
    )
    if not str_addr:
        logger.warn(f"  VirtualAllocEx failed for DLL name string")
        return False

    # Write DLL name
    n_written = ctypes.c_size_t()
    if not kernel32.WriteProcessMemory(
        h_process, str_addr, dll_bytes, len(dll_bytes), ctypes.byref(n_written)
    ):
        logger.warn(f"  Cannot write DLL name to target process")
        return False

    # Create remote thread calling LoadLibraryA(dll_name)
    thread_id = wintypes.DWORD()
    h_thread = kernel32.CreateRemoteThread(
        h_process, None, 0,
        load_lib_addr, str_addr,
        0, ctypes.byref(thread_id),
    )
    if not h_thread:
        logger.warn(f"  CreateRemoteThread failed for {dll_name}")
        return False

    # Wait for LoadLibraryA to complete (5 second timeout)
    kernel32.WaitForSingleObject(h_thread, 5000)
    kernel32.CloseHandle(h_thread)

    # Free the string memory
    kernel32.VirtualFreeEx(h_process, str_addr, 0, MEM_RELEASE)

    logger.info(f"  Injected {dll_name} into target process")
    return True


def _check_wow64(h_process: int) -> bool:
    """Check if target process is 32-bit running under WOW64."""
    is_wow64 = wintypes.BOOL(False)
    kernel32.IsWow64Process(h_process, ctypes.byref(is_wow64))
    return bool(is_wow64.value)


def _get_remote_dll_base(h_process: int, dll_name: str) -> int | None:
    """Get the actual base address of a DLL in the remote process."""
    MAX_MODULES = 1024
    h_modules = (wintypes.HMODULE * MAX_MODULES)()
    cb_needed = wintypes.DWORD()
    LIST_MODULES_ALL = 0x03

    if not psapi.EnumProcessModulesEx(
        h_process, h_modules, ctypes.sizeof(h_modules),
        ctypes.byref(cb_needed), LIST_MODULES_ALL,
    ):
        return None

    count = cb_needed.value // ctypes.sizeof(wintypes.HMODULE)
    name_buf = ctypes.create_string_buffer(260)
    target = dll_name.lower().encode()

    for i in range(min(count, MAX_MODULES)):
        if psapi.GetModuleBaseNameA(h_process, h_modules[i], name_buf, 260):
            mod_name = name_buf.value.lower()
            if mod_name == target:
                # HMODULE IS the base address
                return h_modules[i] if isinstance(h_modules[i], int) else int(h_modules[i] or 0)
    return None


def get_connectex_address() -> int | None:
    """Get ConnectEx address via WSAIoctl (not a standard export).

    ConnectEx is obtained through SIO_GET_EXTENSION_FUNCTION_POINTER,
    not GetProcAddress. The address is in mswsock.dll and is valid
    across all processes (same base address per boot).
    """
    SIO_GET_EXTENSION_FUNCTION_POINTER = 0xC8000006
    # WSAID_CONNECTEX = {25a207b9-ddf3-4660-8ee9-76e58c74063e}
    WSAID_CONNECTEX = bytes([
        0xb9, 0x07, 0xa2, 0x25, 0xf3, 0xdd, 0x60, 0x46,
        0x8e, 0xe9, 0x76, 0xe5, 0x8c, 0x74, 0x06, 0x3e,
    ])

    ws2_32 = ctypes.WinDLL('ws2_32', use_last_error=True)

    s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    try:
        func_ptr = ctypes.c_void_p(0)
        bytes_returned = ctypes.c_ulong(0)

        ret = ws2_32.WSAIoctl(
            s.fileno(),
            SIO_GET_EXTENSION_FUNCTION_POINTER,
            WSAID_CONNECTEX, 16,
            ctypes.byref(func_ptr), ctypes.sizeof(func_ptr),
            ctypes.byref(bytes_returned),
            None, None,
        )

        if ret == 0 and func_ptr.value:
            return func_ptr.value
    except Exception:
        pass
    finally:
        s.close()

    return None


def _build_hook_code(
    saved_bytes: bytes,
    return_addr: int,
    redirect_ip: str,
    target_port: int,
) -> tuple[bytes, int]:
    """Build x64 hook function that redirects connections.

    Returns (hook_bytes, counter_offset) where counter_offset is the
    position of a 4-byte call counter within the hook code.

    x64 calling convention: rcx=socket, rdx=sockaddr*, r8=namelen
    Works for connect(), WSAConnect(), and ConnectEx() since sockaddr
    is always in RDX.

    Assembly:
        lock inc dword ptr [rip+N]      ; increment call counter
        cmp word ptr [rdx], 2           ; AF_INET?
        jne skip
        cmp word ptr [rdx+2], port_be   ; target port? (big-endian)
        jne skip
        mov dword ptr [rdx+4], ip       ; redirect IP
      skip:
        <saved original bytes>          ; trampoline (instruction-aligned)
        jmp [rip+0]                     ; jump back to original+stub_size
        .quad return_addr
        .long 0                         ; call counter (4 bytes)
    """
    ip_bytes = bytes(int(x) for x in redirect_ip.split("."))
    port_be = struct.pack(">H", target_port)

    hook = bytearray()

    # [0..6] lock inc dword ptr [rip+offset] - call counter (7 bytes)
    # Offset is a placeholder, fixed up after we know total code size
    inc_pos = len(hook)
    hook += b'\xF0\xFF\x05'          # LOCK INC dword ptr [RIP+rel32]
    hook += b'\x00\x00\x00\x00'      # rel32 placeholder

    # [7..10] cmp word ptr [rdx], 2  (sin_family == AF_INET)
    hook += b'\x66\x83\x3A\x02'

    # [11..12] jne skip (+15 bytes ahead) - same offset as before
    hook += b'\x75\x0F'

    # [13..18] cmp word ptr [rdx+2], port_be  (sin_port == target)
    hook += b'\x66\x81\x7A\x02' + port_be

    # [19..20] jne skip (+7 bytes ahead)
    hook += b'\x75\x07'

    # [21..27] mov dword ptr [rdx+4], ip_bytes  (sin_addr = redirect_ip)
    hook += b'\xC7\x42\x04' + ip_bytes

    # [28] skip: trampoline (execute original prologue - properly aligned)
    hook += saved_bytes

    # jmp [rip+0] -> return to original function after saved prologue
    hook += b'\xFF\x25\x00\x00\x00\x00'
    hook += struct.pack("<Q", return_addr)

    # Call counter (4 bytes, initially 0) - placed AFTER the jmp (never executed)
    counter_offset = len(hook)
    hook += struct.pack("<I", 0)

    # Fix up the RIP-relative offset for the lock inc instruction
    # RIP after the 7-byte inc instruction = inc_pos + 7
    rip_after_inc = inc_pos + 7
    rel32 = counter_offset - rip_after_inc
    struct.pack_into("<i", hook, inc_pos + 3, rel32)

    return bytes(hook), counter_offset


def _install_single_hook(
    h_process: int,
    func_addr: int,
    func_name: str,
    redirect_ip: str,
    target_port: int,
) -> bool:
    """Install an inline hook on a single function."""
    # Read enough bytes for instruction boundary analysis
    READ_SIZE = 32
    original = (ctypes.c_char * READ_SIZE)()
    n_read = ctypes.c_size_t()
    if not kernel32.ReadProcessMemory(
        h_process, func_addr, original, READ_SIZE, ctypes.byref(n_read)
    ):
        logger.error(f"  Cannot read {func_name} prologue")
        return False

    raw = bytes(original)

    # Find instruction-aligned stub size (>= 14 bytes)
    stub_size = _find_stub_size(raw, JMP_STUB_SIZE)
    saved = raw[:stub_size]

    logger.info(f"  {func_name} prologue ({stub_size} bytes): {saved.hex()}")

    # Disassemble and log individual instructions
    pos = 0
    while pos < stub_size:
        insn_len = _x64_insn_len(saved, pos)
        if insn_len == 0:
            break
        insn_hex = saved[pos:pos + insn_len].hex()
        logger.info(f"    +{pos:02d}: {insn_hex}")
        pos += insn_len

    # Safety: warn if relative instructions found in saved bytes
    for idx in range(len(saved)):
        b = saved[idx]
        if b in (0xE8, 0xE9) and idx > 0 and saved[idx - 1] not in range(0x40, 0x50):
            logger.warn(f"  Possible relative call/jmp at offset {idx} - hook may crash!")

    # Build hook code
    return_addr = func_addr + stub_size
    hook_code, counter_offset = _build_hook_code(saved, return_addr, redirect_ip, target_port)

    # Allocate executable memory in target process
    hook_addr = kernel32.VirtualAllocEx(
        h_process, None, len(hook_code),
        MEM_COMMIT | MEM_RESERVE, PAGE_EXECUTE_READWRITE,
    )
    if not hook_addr:
        logger.error(f"  VirtualAllocEx failed for {func_name}")
        return False

    logger.info(f"  Hook @ 0x{hook_addr:016X} ({len(hook_code)} bytes)")

    # Write hook code
    n_written = ctypes.c_size_t()
    if not kernel32.WriteProcessMemory(
        h_process, hook_addr, hook_code, len(hook_code), ctypes.byref(n_written)
    ):
        logger.error(f"  Cannot write hook code for {func_name}")
        return False

    # Build JMP stub: jmp [rip+0]; .quad hook_addr; <NOP padding>
    jmp_stub = b'\xFF\x25\x00\x00\x00\x00' + struct.pack("<Q", hook_addr)
    # Pad with INT3 (0xCC) to fill up to stub_size (safer than NOP if executed unexpectedly)
    jmp_stub += b'\xCC' * (stub_size - JMP_STUB_SIZE)

    # Make function prologue writable
    old_protect = wintypes.DWORD()
    if not kernel32.VirtualProtectEx(
        h_process, func_addr, stub_size,
        PAGE_EXECUTE_READWRITE, ctypes.byref(old_protect),
    ):
        logger.error(f"  VirtualProtectEx failed for {func_name}")
        return False

    # Overwrite prologue with JMP to our hook
    if not kernel32.WriteProcessMemory(
        h_process, func_addr, jmp_stub, stub_size, ctypes.byref(n_written)
    ):
        logger.error(f"  Cannot write JMP stub for {func_name}")
        return False

    # Restore protection
    kernel32.VirtualProtectEx(
        h_process, func_addr, stub_size,
        old_protect.value, ctypes.byref(old_protect),
    )

    # Flush instruction cache
    kernel32.FlushInstructionCache(h_process, func_addr, stub_size)

    # ---- VERIFICATION: read back the prologue to confirm JMP stub was written ----
    verify_buf = (ctypes.c_char * stub_size)()
    n_verify = ctypes.c_size_t()
    if kernel32.ReadProcessMemory(
        h_process, func_addr, verify_buf, stub_size, ctypes.byref(n_verify)
    ):
        actual = bytes(verify_buf)
        logger.info(f"  VERIFY {func_name}: read-back = {actual.hex()}")
        logger.info(f"  VERIFY {func_name}: expected  = {jmp_stub.hex()}")
        if actual == jmp_stub:
            logger.info(f"  VERIFY {func_name}: JMP stub CONFIRMED")
        elif actual[:6] == b'\xFF\x25\x00\x00\x00\x00':
            target_addr = struct.unpack("<Q", actual[6:14])[0]
            logger.warn(f"  VERIFY {func_name}: JMP found but target=0x{target_addr:016X} (expected 0x{hook_addr:016X})")
        else:
            logger.error(f"  VERIFY {func_name}: PROLOGUE NOT OVERWRITTEN!")
            logger.error(f"  Original bytes still there - WriteProcessMemory may have silently failed")
            return False
    else:
        logger.warn(f"  VERIFY {func_name}: Cannot read back after hook write")

    # Also verify the hook code was written correctly
    hook_verify = (ctypes.c_char * len(hook_code))()
    if kernel32.ReadProcessMemory(
        h_process, hook_addr, hook_verify, len(hook_code), ctypes.byref(n_verify)
    ):
        hook_actual = bytes(hook_verify)
        if hook_actual == hook_code:
            logger.info(f"  VERIFY {func_name}: Hook code at 0x{hook_addr:016X} CONFIRMED")
        else:
            logger.error(f"  VERIFY {func_name}: Hook code MISMATCH!")
            logger.error(f"    Written: {hook_code.hex()}")
            logger.error(f"    ReadBack: {hook_actual.hex()}")

    # Store counter address for later diagnostic reads
    _hook_counter_info[func_name] = (hook_addr, counter_offset)
    logger.info(f"  COUNTER {func_name}: addr=0x{hook_addr + counter_offset:016X}")

    return True


def read_hook_counters(pid: int) -> dict[str, int]:
    """Read call counters from all installed hooks.

    Returns func_name -> call_count. If count > 0, the function IS being called.
    If count == 0, the function is NOT being called (game bypasses it).
    """
    if not _hook_counter_info:
        return {}

    h_process = kernel32.OpenProcess(PROCESS_ALL_ACCESS, False, pid)
    if not h_process:
        return {}

    results = {}
    try:
        for func_name, (hook_addr, counter_off) in _hook_counter_info.items():
            counter_addr = hook_addr + counter_off
            buf = (ctypes.c_char * 4)()
            n_read = ctypes.c_size_t()
            if kernel32.ReadProcessMemory(
                h_process, counter_addr, buf, 4, ctypes.byref(n_read)
            ):
                count = struct.unpack("<I", bytes(buf))[0]
                results[func_name] = count
            else:
                results[func_name] = -1  # read failed
    finally:
        kernel32.CloseHandle(h_process)

    return results


# ---------------------------------------------------------------------------
# Public API
# ---------------------------------------------------------------------------

def install_hooks(pid: int, redirect_ip: str = "127.0.0.1", target_port: int = 5555) -> bool:
    """Install Winsock hooks in the target process.

    Hooks connect(), WSAConnect(), and ConnectEx() to redirect connections
    on target_port to redirect_ip.
    """
    logger.info(f"Installing Winsock hooks in PID {pid}...")

    # Get function addresses (valid across processes for system DLLs)
    hooks = []
    for dll, func in [
        ("ws2_32.dll", "connect"),
        ("ws2_32.dll", "WSAConnect"),
    ]:
        addr = get_function_address(dll, func)
        if addr:
            logger.info(f"  {dll}!{func} @ 0x{addr:016X}")
            hooks.append((addr, f"{dll}!{func}"))
        else:
            logger.warn(f"  {dll}!{func} not found (skipping)")

    # ConnectEx is NOT a standard export - must use WSAIoctl to get its address
    connectex_addr = get_connectex_address()
    if connectex_addr:
        logger.info(f"  mswsock!ConnectEx @ 0x{connectex_addr:016X} (via WSAIoctl)")
        hooks.append((connectex_addr, "mswsock!ConnectEx"))
    else:
        logger.warn("  ConnectEx not found via WSAIoctl (skipping)")

    if not hooks:
        logger.error("  No hookable functions found!")
        return False

    # Open target process
    h_process = kernel32.OpenProcess(PROCESS_ALL_ACCESS, False, pid)
    if not h_process:
        err = ctypes.get_last_error()
        logger.error(f"  OpenProcess failed (error {err})")
        if err == 5:
            logger.error("  Access denied - run as Administrator!")
        return False

    try:
        # ---- DIAGNOSTICS: Check process architecture ----
        is_wow64 = _check_wow64(h_process)
        if is_wow64:
            logger.error("  TARGET IS 32-BIT (WOW64)! Our 64-bit hooks CANNOT work!")
            logger.error("  Need 32-bit function addresses for a WOW64 process.")
            return False
        logger.info(f"  Target: 64-bit native (good)")

        # ---- DIAGNOSTICS: Compare DLL base addresses ----
        for dll_name in ["ws2_32.dll", "mswsock.dll"]:
            local_base = kernel32.LoadLibraryA(dll_name.encode())
            remote_base = _get_remote_dll_base(h_process, dll_name)
            if local_base and remote_base:
                if local_base == remote_base:
                    logger.info(f"  {dll_name}: base MATCH @ 0x{local_base:016X}")
                else:
                    logger.error(f"  {dll_name}: BASE MISMATCH! local=0x{local_base:016X} remote=0x{remote_base:016X}")
                    logger.error(f"  Hook addresses will be WRONG! Offset = {remote_base - local_base}")
            elif remote_base is None:
                logger.warn(f"  {dll_name}: NOT LOADED in target (will inject)")
            else:
                logger.warn(f"  {dll_name}: Cannot determine local base")

        # Ensure mswsock.dll is loaded in target (needed for ConnectEx hook)
        if connectex_addr:
            logger.info("  Injecting mswsock.dll into target process...")
            _ensure_dll_loaded(h_process, "mswsock.dll")

        success = 0
        for func_addr, func_name in hooks:
            logger.info(f"  Hooking {func_name}...")
            if _install_single_hook(h_process, func_addr, func_name, redirect_ip, target_port):
                logger.info(f"  {func_name} hooked!")
                success += 1
            else:
                logger.warn(f"  {func_name} hook failed")

        if success > 0:
            logger.info(f"  {success}/{len(hooks)} hooks installed")
            logger.info(f"  All port {target_port} connections -> {redirect_ip}")
            return True
        else:
            logger.error("  All hooks failed!")
            return False

    finally:
        kernel32.CloseHandle(h_process)


def verify_hooks_still_active(pid: int, target_port: int = 5555) -> dict:
    """Re-check if hooks are still in place (call AFTER game is connected).

    Returns dict with status for each function.
    Useful to detect if something restored original bytes after our hook.
    """
    results = {}
    h_process = kernel32.OpenProcess(PROCESS_ALL_ACCESS, False, pid)
    if not h_process:
        return {"error": "Cannot open process"}

    try:
        for dll, func in [("ws2_32.dll", "connect"), ("ws2_32.dll", "WSAConnect")]:
            addr = get_function_address(dll, func)
            if not addr:
                results[func] = "address_not_found"
                continue
            buf = (ctypes.c_char * 16)()
            n_read = ctypes.c_size_t()
            if kernel32.ReadProcessMemory(h_process, addr, buf, 16, ctypes.byref(n_read)):
                raw = bytes(buf)
                if raw[:6] == b'\xFF\x25\x00\x00\x00\x00':
                    target = struct.unpack("<Q", raw[6:14])[0]
                    results[f"{dll}!{func}"] = f"HOOKED -> 0x{target:016X}"
                    logger.info(f"  RE-VERIFY {dll}!{func}: HOOKED -> 0x{target:016X}")
                else:
                    results[f"{dll}!{func}"] = f"NOT_HOOKED: {raw[:14].hex()}"
                    logger.error(f"  RE-VERIFY {dll}!{func}: NOT HOOKED! bytes={raw[:14].hex()}")
            else:
                results[f"{dll}!{func}"] = "read_failed"

        # ConnectEx
        cex_addr = get_connectex_address()
        if cex_addr:
            buf = (ctypes.c_char * 16)()
            if kernel32.ReadProcessMemory(h_process, cex_addr, buf, 16, ctypes.byref(n_read)):
                raw = bytes(buf)
                if raw[:6] == b'\xFF\x25\x00\x00\x00\x00':
                    target = struct.unpack("<Q", raw[6:14])[0]
                    results["mswsock!ConnectEx"] = f"HOOKED -> 0x{target:016X}"
                    logger.info(f"  RE-VERIFY mswsock!ConnectEx: HOOKED -> 0x{target:016X}")
                else:
                    results["mswsock!ConnectEx"] = f"NOT_HOOKED: {raw[:14].hex()}"
                    logger.error(f"  RE-VERIFY mswsock!ConnectEx: NOT HOOKED! bytes={raw[:14].hex()}")
    finally:
        kernel32.CloseHandle(h_process)

    return results


def wait_and_hook(redirect_ip: str = "127.0.0.1", target_port: int = 5555,
                  timeout: float = 60.0) -> bool:
    """Wait for Dofus.exe to appear, then install hooks."""
    import time

    logger.info(f"Waiting for Dofus.exe (timeout: {timeout}s)...")
    start = time.time()

    while time.time() - start < timeout:
        pid = find_dofus_pid()
        if pid:
            logger.info(f"  Dofus.exe found! PID={pid}")
            # Small delay to let the process initialize
            time.sleep(0.5)
            return install_hooks(pid, redirect_ip, target_port)
        time.sleep(0.3)

    logger.error("  Timeout - Dofus.exe not found")
    return False


# ---------------------------------------------------------------------------
# Standalone test
# ---------------------------------------------------------------------------

if __name__ == "__main__":
    import sys

    print()
    print("=" * 50)
    print("  Winsock Hook - Standalone Test")
    print("=" * 50)
    print()

    if len(sys.argv) > 1:
        pid = int(sys.argv[1])
        print(f"  Using PID: {pid}")
    else:
        print("  Searching for Dofus.exe...")
        pid = find_dofus_pid()
        if not pid:
            print("  Dofus.exe not found! Pass PID as argument or start Dofus first.")
            sys.exit(1)
        print(f"  Found Dofus.exe PID: {pid}")

    print()
    ok = install_hooks(pid)
    print()
    if ok:
        print("  Hooks installed successfully!")
        print("  Game connections on port 5555 will be redirected to 127.0.0.1")
    else:
        print("  Hook installation failed!")
    print()

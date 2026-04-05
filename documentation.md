# Mackbot - Dofus 3 MITM Bot - Documentation

**Date**: 2026-04-05
**Repo**: https://github.com/mahmoudel24rb-debug/mackbot

---

## How to Run

```bash
# Desktop app (Electron window + bot backend)
cd c:\Users\mahmo\Documents\bot && python app.py

# CLI only (no GUI, console commands)
cd c:\Users\mahmo\Documents\bot && python bot.py
```

First time setup: `npm install` + `pip install websockets pywin32 comtypes`

---

## Architecture

```
app.py                      <- Entry point: bot backend thread + Electron window
electron-main.js             <- Electron window config (loads web/index.html)
bot.py                      <- CLI entry point (WinDivert + console, no GUI)
server/ws_server.py          <- WebSocket ws://localhost:7777
core/orchestrator.py         <- Central controller (WinDivert + proxy + WS + commands)
web/                         <- HTML/CSS/JS SPA frontend (served by Electron)
  index.html, css/, js/, assets/
game/                        <- Game logic (state, handlers, movement, gathering)
proxy/                       <- MITM TCP proxy + packet handler
launcher/                    <- WinDivert packet redirect
protocol/                    <- Code matching + auto-matcher
```

---

## What Works (2026-04-05)

- Electron desktop window with dark Jitsuri-inspired theme
- WebSocket bridge backend <-> frontend (Status every 1s)
- Dashboard: HP/MP/AP bars, character info, map position, entity counts
- Map Canvas: isometric 14x40 grid with Jitsuri assets, hover, click
- Harvest tab: script list, start/stop, timer, stats/h
- Sniffer: live traffic with C2S/S2C filter, code matching
- Settings: network, folders, anti-detection config
- Character name detection from ISU actors ('Arclight-Sett', 'Moulinenbois')
- Cell tracking from MoveRequest + MoveEvent
- Resource detection from StatedElement (f2=cellId, f3=elementId, f4=status)
- KWW walkability with persistent cache
- Gather sequence: itk -> ite -> itl/idh -> irj -> kof (WORKS)
- Anti-detection: gaussian delays, random pauses

---

## Known Bugs

### 1. Two codes for InteractiveUseRequest (itl AND idh)
The game uses BOTH `itl` and `idh` for InteractiveUseRequest depending on
the map/session. The bot's `get_code()` returns only one. The matching.json
should keep both codes mapped. The bot should sniff which code the real
client uses on the current map and use that one.

### 2. Map canvas doesn't update on map change
MapCellData is broadcast via ensure_future from sync context in patched_process.
The broadcast may silently fail. Need to also send MapCellData in periodic
Status broadcast when map_id changes.

### 3. CellId mismatch after map change
Cell stays as old value until first MoveRequest sniffed. Multiple extraction
strategies in iny handler but not validated.

### 4. Periodic Status broadcast was missing in app.py
Fixed in commit 039e985 — now broadcasts Status every 1s to WS clients.
Character name should now appear in the sidebar after connection.

---

## Protocol Reference

### Gather Sequence (confirmed working)
```
itk (empty) -> ite (empty) -> itl OR idh (f1=elementId) -> irj -> kof
```

### Key Codes (2026-04-05)
```
iro=MapMovementRequest  jsi=MapMovementEvent  jse=MapMovementConfirmReq
lds=MapMovementConfirmResp  isg=MapChangeRequest  isu=MapComplementaryInfo
itk=InteractiveUseCheckReq  ite=InteractiveUseCheckResp
itl=InteractiveUseRequest  idh=InteractiveUseRequest (alternate code!)
hfb=InteractiveUsedEvent  kof=InteractiveUseEndedEvent  kot=ObjectHarvested
jrl=CharacterSelectedEvent  kww=MapCellProperties  ial=MapCellData
```

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
- Map Canvas: isometric 14x40 grid with Jitsuri assets, hover, click-to-move
- Map updates on map change (MapCellData broadcast in periodic loop)
- Harvest tab: script list, start/stop, timer, stats/h
- Sniffer: live traffic with C2S/S2C filter, code matching, save
- Settings: network, folders, anti-detection config
- Character name detection from jrl (CharacterLoaded) — NOT from ISU actors
- Character ID preserved from CharacterSelect (jtb) — not overwritten by other players
- Map coordinates (x,y) from MapCoordinatesEvent (iaa)
- Cell tracking from MoveRequest + MoveEvent
- Resource detection from StatedElement (f2=cellId, f3=elementId, f4=status)
- KWW walkability with persistent cache (grows over time)
- Gather sequence: itk -> ite -> itl/idh -> irj -> kof
- Anti-detection: gaussian delays, random pauses, resource variance
- Moderator detection stub (name pattern matching)
- Lua script engine with route parsing (64-step routes)

---

## Known Bugs (2026-04-05)

### 1. Two codes for InteractiveUseRequest (itl AND idh)
The game uses BOTH `itl` and `idh` for InteractiveUseRequest depending on
the map/session. matching.json has `idh` mapped. The real client sometimes
sends `itl` instead. Bot should sniff which code the client uses.

### 2. CellId mismatch after map change
Cell stays as old value until first MoveRequest sniffed. The iny handler
tries multiple extraction strategies (low10, low12, mid10, mid12) but
results not validated. CELL MISMATCH warnings appear in logs.

### 3. Script engine can't navigate between maps
Lua scripts define routes with map coordinates (20,-29 etc.) but the
script engine doesn't have navigation between maps implemented. All 64
steps show "Map mismatch" because the bot can't change maps autonomously.
The map coordinates (x,y) from iaa handler should now work — but the
script engine needs to use WorldGraph for inter-map navigation.

### 4. pywebview doesn't work on Python 3.14
pythonnet can't compile. Falls back to Electron (which works fine).
pywebview dependency can be removed.

---

## Bugs Fixed This Session

### Character name overwritten by other players
**Before**: `_try_extract_character_name` was called for ALL actors in ISU,
picking up names of other players on the map (e.g. "Moulinenbois").
**Fix**: Name extraction removed from ISU actors. Only extracted from
`handle_character_loaded` (jrl) which contains only OUR character.

### Character ID overwritten by other players
**Before**: Auto-detect took the first positive actorId in ISU actors list.
Other players also have positive IDs, so our ID got overwritten.
**Fix**: Auto-detect only fires if `state.character.id is None`. If already
set from CharacterSelect (jtb) or CharacterLoaded (jrl), it's preserved.

### Map coordinates missing (None, None)
**Before**: `MapCoordinatesEvent` stable name wasn't registered as handler.
The legacy code `iaa` worked but matching.json mapped it to a different
stable name that had no handler.
**Fix**: Added `MapCoordinatesEvent`, `CurrentCellEvent`,
`MapCellPropertiesEvent`, `MapCellDataEvent` as stable name handlers.

### Map canvas not updating on map change
**Before**: MapCellData was broadcast via `asyncio.ensure_future` from sync
context in `patched_process`, which silently failed.
**Fix**: MapCellData + MapEntities are now broadcast in the periodic loop
in `app.py` when `map_id` changes.

### Status broadcast missing in app.py
**Before**: The periodic Status broadcast was only in `app_legacy.py`
(CustomTkinter). The new `app.py` just did `await asyncio.sleep(1)`.
**Fix**: Added Status broadcast every 1s to WS clients in `app.py`.

---

## Protocol Reference

### Gather Sequence (confirmed working)
```
itk (empty) -> ite (empty) -> itl OR idh (f1=elementId) -> irj -> kof
```

### Key Codes (2026-04-05)
```
iro=MapMovementRequest       jsi=MapMovementEvent
jse=MapMovementConfirmReq    lds=MapMovementConfirmResp
isg=MapChangeRequest         isu=MapComplementaryInfoEvent
itk=InteractiveUseCheckReq   ite=InteractiveUseCheckResp
itl=InteractiveUseRequest    idh=InteractiveUseRequest (alt code)
hfb=InteractiveUsedEvent     kof=InteractiveUseEndedEvent
kot=ObjectHarvestedEvent     jrl=CharacterSelectedEvent
iaa=MapCoordinatesEvent      iny=CurrentCellEvent
kww=MapCellPropertiesEvent   ial=MapCellDataEvent
```

### Resource Detection from ISU
```
InteractiveElements (field 2): f1=elementId, f2.f4=cellId
StatedElements (field 6): f2=cellId, f3=elementId, f4=status (1=available)
```

### KWW Walkability
```
f3 = (flags << 10) | cellId
Cells NOT in KWW = walkable by default (~437/560)
Cache persisted in data/walkable_cache.json
```

---

## Next Steps

1. Fix script engine inter-map navigation (use WorldGraph)
2. Validate iny cellId extraction for post-map-change position
3. Add dual-code support for InteractiveUseRequest (itl/idh)
4. Improve map canvas rendering (entity positions, resources)
5. Test autofarm loop end-to-end

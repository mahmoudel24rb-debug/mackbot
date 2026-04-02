# Mackbot - Dofus 3 MITM Bot - Documentation

**Date**: 2026-04-02  
**Repo**: https://github.com/mahmoudel24rb-debug/mackbot

---

## Architecture

```
app.py                     <- GUI entry point (CustomTkinter)
bot.py                     <- CLI entry point (WinDivert + console)
core/orchestrator.py       <- Central controller (WinDivert + proxy + game components)
core/event_bus.py          <- Pub/sub for UI <-> bot communication
proxy/mitm_proxy.py        <- MITM TCP proxy, packet interception
launcher/packet_redirect.py <- WinDivert packet-level NAT redirect
game/state.py              <- GameState (Character, Resource, MapInfo, flags)
game/message_handlers.py   <- All protobuf message handlers (~1500 lines)
game/map_grid.py           <- Isometric grid (WIDTH=14), cell<->point, compress_path
game/pathfinding.py        <- A* on isometric grid
game/movement.py           <- Packet builders + MovementController
game/navigation.py         <- Navigator (move_to with walkability)
game/gathering.py          <- GatherController (wait ite -> itl)
game/auto_farmer.py        <- AutoFarmer (multi-map farming loop)
game/map_view.py           <- ASCII map viewer
game/anti_detect.py        <- Human-like delay simulation
protocol/matching.py       <- 3-letter code <-> stable name mapping
protocol/auto_matcher.py   <- Auto-detection of codes
ui/                        <- CustomTkinter GUI (Dashboard, Harvest, MapView, Sniffer, Settings)
data/matching.json         <- Current code mapping
data/walkable_cache.json   <- Persisted walkable cells per map
```

---

## What Works (Confirmed 2026-04-02)

- MITM Proxy via WinDivert (bot.py) or GUI (app.py)
- GUI with CustomTkinter: Dashboard, Harvest, Carte, Sniffer, Parametres
- MapView in GUI: shows resources (green), player (yellow), mobs on grid
- Cell tracking from MoveRequest C2S + MoveEvent S2C
- Resource detection from ISU StatedElement: f2=cellId, f3=elementId, f4=status
- KWW walkability: cellId = f3 & 0x3FF, flags = f3 >> 10, ~437 walkable cells
- Walkable cache persists between sessions (data/walkable_cache.json)
- IAL cell properties: cellId = f2 - 16384
- Sniffer tab captures live traffic and shows code->name matches
- Move command works for short paths
- Real client gather works: itk -> ite -> idh -> irj -> kof

---

## Known Bugs (As of 2026-04-02)

### Bug 1: "Deconnecte" in sidebar even when connected
The game.connected event doesn't fire reliably. Character name is often None.
Root cause: CharacterSelectedEvent handler doesn't always fire, character.name never set from ISU.

### Bug 2: InteractiveUseRequest code changed (itl -> idh)
After game update, the code changed from itl to idh. Fixed in matching.json.
The bot's gather still needs testing with the new code.

### Bug 3: CellId = None after map change
After map change, character.cell_id stays as old map's cell or None.
Player cellId NOT in ISU actors (player uses f1.f8, not f1.f7).
Workaround: corrected on first MoveRequest sniffed.

### Bug 4: False "Ressource recoltee" on login
InteractiveUseEndedEvent fires during login burst. 10s grace period filter added.

### Bug 5: MapView shows 3 resources instead of 4
One resource may be clipped by canvas rendering or at edge cell.

### Bug 6: Pathfinding keyCells invalid for long paths
compress_path() produces invalid keyCells. Server refuses error 16/23.
Movement automation disabled in gather. Player moves manually.

### Bug 7: Sniffer doesn't auto-detect new code->name mappings
Captures traffic but auto-matcher doesn't identify new codes. Manual correction needed.

---

## Protocol Reference

### GameMessage C2S Wrapping
```
GameMessage field 2 (NOT field 4!):
  field 1 = uid (varint, -1 = 0xFFFFFFFFFFFFFFFF)
  field 2 = Any { field 1 = type_url, field 2 = inner_payload }
```

### Key Message Codes (Session 2026-04-02)
```
iro  -> MapMovementRequest (C2S)
jsi  -> MapMovementEvent (S2C)
jse  -> MapMovementConfirmRequest (C2S)
lds  -> MapMovementConfirmResponse (S2C)
isg  -> MapChangeRequest (C2S)
isu  -> MapComplementaryInformationEvent (S2C)
itk  -> InteractiveUseCheckRequest (C2S, empty payload)
ite  -> InteractiveUseCheckResponse (S2C, empty payload)
idh  -> InteractiveUseRequest (C2S, f1=elementId only) *** CHANGED from itl ***
hfb  -> InteractiveUsedEvent (S2C)
kof  -> InteractiveUseEndedEvent (S2C)
kot  -> ObjectHarvestedEvent (S2C)
```

### Gather Sequence (Real Client)
```
1. Player moves adjacent to resource
2. CLIENT: itk (empty payload)
3. SERVER: ite (empty payload)  
4. CLIENT: idh (f1=elementId, NO skillId)  *** was itl, now idh ***
5. SERVER: irj (interaction validated)
6. SERVER: kof (gather ended)
```

### Resource Detection from ISU
```
InteractiveElements (field 2): f1=elementId, f2.f4=cellId (NOT top-level f4!)
StatedElements (field 6): f2=cellId, f3=elementId, f4=status (1=available)
```

### KWW Walkability
```
Entries in pairs: f3 = (flags << 10) | cellId
Cells NOT in KWW = walkable by default (~437/560)
```

---

## Next Steps (Priority Order)

1. Fix connection detection - Parse character name from ISU or jrl
2. Fix gather bot - Use idh code, test itk->idh flow
3. Fix pathfinding - Validate grid with sniffed data
4. Automate gather - Re-enable movement once pathfinding works
5. Test autofarm - autofarm x1,y1 x2,y2 command

# Documentation technique — Bot MITM Dofus 3

**Derniere mise a jour** : 2026-03-26
**Etat** : Move fonctionne, gather en cours de debug, cell tracking S2C OK

---

## Etat AVANT le PLAN_IMPLEMENTATION (debut de session)

- Sniffer passif fonctionnel (bot.py + WinDivert)
- Matching dynamique (auto-detection codes 3 lettres)
- Cell tracking : NE MARCHAIT PAS (mauvais field numbers, cell toujours 3)
- Move : NE MARCHAIT PAS (mauvais wrapping GameMessage, mauvais field numbers)
- Gather : NE MARCHAIT PAS (pas de resources detectees, mauvais code)
- Pathfinding : grille statique Dofus 2 (largeur 14), incompatible Dofus 3

## Etat APRES les corrections (fin de session)

### Ce qui MARCHE :
- **Move** (`move <cell>`) : le perso bouge dans le jeu ✅
- **Cell tracking S2C** : MoveEvent (isj) decode f2=dir, f3=path, f4=actorId ✅
- **Cell tracking C2S** : MoveRequest (iro) decode f2=mapId, f3=keyCells ✅
- **MapId tracking** : mis a jour depuis ISU et MoveRequest ✅
- **Spawn cell detection** : premier MoveRequest apres map change donne cells[0] ✅
- **Resources detectees** : ISU field 2 (interactive) + field 6 (stated) croises ✅
- **Abort sur refus** : isl detecte, bot arrete immediatement ✅
- **IAL parse** : 8987 entries, 485 cellIds extraits ✅
- **Matching protege** : auto-matcher ne peut plus ecraser un nom deja mappe ✅
- **Invalidation temporelle** : matching reset apres 21 jours ✅

### Ce qui NE MARCHE PAS ENCORE :
- **Gather** : le bot envoie `itl` (InteractiveUseRequest) mais le serveur ne repond pas
- **Pathfinding** : grille Dofus 2 (largeur 14) vs Dofus 3 (largeur probablement 34)
- **Cell depuis ISU** : le cellId du joueur n'est pas dans les nested actors
- **KWW** : format non-RLE, pas encore decode (total 196K != 560)

---

## Bugs identifies et a corriger (prochaine session)

### BUG 1 — InteractiveUseRequest (itl) ignore par le serveur
**Symptome** : Le bot envoie `itl` avec elementId+skillId, le serveur ne repond pas.
**Causes probables** :
1. Le vrai client envoie `itk` (36 bytes) AVANT `itl`. Le bot saute cette etape.
2. Les field numbers sont peut-etre inverses (f1=skillId, f2=elementId au lieu de f1=elementId, f2=skillId)
3. Taille differente : vrai client itl=43 bytes, bot itl=46 bytes (3 bytes de difference dans le payload)

**Sequence reelle du client** :
```
CLIENT: itk (36 bytes)  ← pre-interaction / check
SERVER: ite (36 bytes)  ← response
CLIENT: itl (43 bytes)  ← InteractiveUseRequest (la vraie recolte)
SERVER: irj (32 bytes)  ← interaction validated
SERVER: kof (36 bytes)  ← InteractiveUseEndedEvent
```

**Fix a tester** :
1. Envoyer `itk` avant `itl`
2. Inverser les fields dans build_interact_request()
3. Comparer le hex du vrai `itl` client avec celui du bot

### BUG 2 — Grille Dofus 3 incompatible avec map_grid.py
**Symptome** : `stop_adjacent` choisit cell 302 (voisin Dofus 2) au lieu de 323/351 (vrais voisins Dofus 3)
**Cause** : map_grid.py utilise une grille isometrique largeur 14 (Dofus 2). Dofus 3 utilise probablement largeur 34.
**Preuve** : 323 - 289 = 34 (voisin direct). 351 - 289 = 62 (2 rangees).
**Fix** : Modifier MAP_WIDTH dans map_grid.py de 14 a 34. Mais attention, NE PAS TOUCHER l'algo A* lui-meme.

### BUG 3 — Cell inconnue apres map change
**Symptome** : cell gardee de l'ancienne map, incorrecte sur la nouvelle
**Workaround actuel** : garder la derniere cell connue, corrigee au premier MoveRequest sniffe
**Fix reel** : decoder le cellId depuis ISU actors (toujours pas trouve dans les nested bytes)

---

## Protocole Dofus 3 — Decouvertes VERIFIEES (2026-03-26)

### GameMessage wrapping (C2S)
```
GameMessage field 2 (NOT field 4!) :
  field 1 = uid (varint, -1 = 0xFFFFFFFFFFFFFFFF)
  field 2 = Any { field 1 = type_url, field 2 = inner_payload }
```
**IMPORTANT** : Le vrai client utilise field 2 du GameMessage, pas field 4.

### MapMovementRequest (C2S) — code `iro`
```
field 2 (varint) = mapId (int64)
field 3 (length-delimited) = keyCells (packed repeated int32)
  Chaque int = (direction << 12) | cellId
  Directions: 0=E, 1=SE, 2=S, 3=SW, 4=W, 5=NW, 6=N, 7=NE
```

### MapMovementConfirmRequest (C2S) — code `jse`
```
field 1 (varint) = 1 (bool true, 4 bytes payload)
```

### MapChangeRequest (C2S) — code `isg`
```
field 2 (varint) = target mapId
```

### MapComplementaryInformationEvent (S2C) — code `isu`
```
field 2  (repeated msg) = InteractiveElements (f1=elemId, f4=cellId?)
field 6  (repeated msg) = StatedElements (f2=skillId, f3=elemId, f4=status)
field 7  (varint) = ??? (toujours 1)
field 11 (repeated msg) = Actors (f1=header, f2=actorId, f3=nested position)
field 13 (varint) = SubareaId
field 14 (varint) = MapId
```
**Status des ressources** : status=1 signifie DISPONIBLE (pas 0)

### MoveEvent (S2C) — code `isj` (non-matche) et `jsi` (matche)
```
isj structure :
  f2 (varint) = direction/type
  f3 (bytes)  = packed varints de cellIds (le chemin complet)
  f4 (varint) = actorId
jsi structure :
  f1 (bytes)  = packed varints (positions d'acteurs, pas un chemin)
```

### InteractiveUseRequest (C2S) — code `itl`
```
Taille vrai client : 43 bytes total
Taille bot : 46 bytes total (3 bytes de plus)
Inner payload bot : f1=elementId(varint), f2=skillId(varint) ← A VERIFIER
Sequence requise : itk → ite → itl (le bot saute itk)
```

### Mouvement refuse (S2C) — code `isl`
```
f1 (varint) = ??? (grand nombre negatif)
f2 (varint) = code erreur (11, 12, 13, 22, 23 observes)
```

### IAL (S2C) — 81KB, donnees de map
```
field 2 : ~8987 entries (toutes les cellules avec proprietes)
Chaque entry : f1=varint(grand nombre), f2=varint(16384+index)
485 cellIds uniques extraits (0-559)
```

### KWW (S2C) — 2100 bytes, pre-map change
```
field 1 : 297 entries
Chaque entry : f1=varint, f2=???, f3=varint(souvent >1000), f4=???
Format : PAS du RLE (sum(f1)+sum(f3) = 196K != 560)
f3 contient des bitmasks : 1024+n, 512+n etc.
```

---

## Codes 3 lettres actuels (matching.json 2026-03-26)

```json
{
  "iro": "MapMovementRequest",
  "itl": "InteractiveUseRequest",
  "isu": "MapComplementaryInformationEvent",
  "kot": "ObjectHarvestedEvent",
  "hfb": "InteractiveUsedEvent",
  "kof": "InteractiveUseEndedEvent",
  "jsi": "MapMovementEvent",
  "lds": "MapMovementConfirmResponse",
  "jse": "MapMovementConfirmRequest",
  "isg": "MapChangeRequest"
}
```

Codes non-matches importants :
- `isj` : MoveEvent pour autres acteurs (meme structure que jsi mais f2/f3/f4)
- `isl` : Mouvement refuse par le serveur
- `itk` : Pre-interaction check (36 bytes, C2S, envoye AVANT itl)
- `ite` : Response au pre-interaction (36 bytes, S2C)
- `kww` : Grille pre-map change (297 entries, format inconnu)
- `ial` : Donnees massives de map (81KB, 8987 entries)
- `iur` : Donnees initiales (7188 bytes)

---

## Architecture des fichiers modifies

```
game/movement.py      — Builders de paquets (f2=mapId, f3=keyCells) + wrapping field 2
game/navigation.py    — Pathfinding + walkability observee + abort sur isl
game/gathering.py     — Sequence gather (skip movement, envoie itl directement)
game/message_handlers.py — Handlers ISU, MoveEvent, KWW, IAL, refus mouvement
game/state.py         — Resource.available = status==1, _observed_walkable
protocol/matching.py  — Invalidation temporelle (21 jours)
protocol/auto_matcher.py — Protection anti-ecrasement, signature stricte
utils/proto_debug.py  — NOUVEAU: decode_protobuf_recursive, format_proto_tree, find_values_in_range
data/matching.json    — itl au lieu de idh pour InteractiveUseRequest
```

---

## Prochaines etapes (ordre de priorite)

1. **Fixer InteractiveUseRequest** : ajouter itk pre-request, verifier field order
2. **Fixer la grille Dofus 3** : largeur 34 au lieu de 14 dans map_grid
3. **Decoder KWW** : comprendre le format des 297 entries pour la walkability
4. **Decoder IAL** : parser correctement les 8987 entries pour la grille complete
5. **Cell depuis ISU** : trouver le cellId dans les nested bytes des actors
6. **Navigation inter-maps** : tester travel_to() avec le WorldGraph
7. **Script Lua** : tester script load/run avec les corrections de mouvement

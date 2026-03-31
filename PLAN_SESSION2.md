# Plan d'implémentation Session 2 — Bot MITM Dofus 3

**Date** : 2026-03-26
**Objectif** : Débloquer le gather complet + décoder IAL/KWW pour la walkability grid
**Prérequis** : Le move fonctionne, le cell tracking S2C est OK, les resources sont détectées depuis ISU

---

## CONTEXTE CRITIQUE — Lire AVANT de coder

### Ce qui MARCHE déjà (NE PAS CASSER) :
- `move <cell>` fonctionne ✅
- Cell tracking S2C (MoveEvent isj) : f2=dir, f3=path, f4=actorId ✅
- Cell tracking C2S (MoveRequest iro) : f2=mapId, f3=keyCells ✅
- Resources détectées depuis ISU field 2 + field 6 ✅
- Abort sur refus serveur (isl) ✅
- IAL parse basique (8987 entries, 485 cellIds) ✅
- GameMessage wrapping C2S utilise **field 2** (PAS field 4)

### GameMessage wrapping rappel (C2S) :
```
GameMessage field 2 (NOT field 4!) :
  field 1 = uid (varint, -1 = 0xFFFFFFFFFFFFFFFF)
  field 2 = Any { field 1 = type_url, field 2 = inner_payload }
```

### Codes 3 lettres actuels :
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

Codes NON-matchés mais importants :
- `itk` : Pre-interaction check (36 bytes, C2S)
- `ite` : Response au pre-interaction (36 bytes, S2C)
- `isl` : Mouvement refusé
- `kww` : Données pré-map change (2100 bytes, 297 entries)
- `ial` : Données massives de map (81KB, 8987 entries)

### Fichiers du projet concernés :
```
game/movement.py         — Builders de paquets + wrapping
game/navigation.py       — Pathfinding + walkability
game/gathering.py        — Séquence gather
game/message_handlers.py — Handlers ISU, MoveEvent, KWW, IAL
game/state.py            — GameState, Resource, positions
game/map_grid.py         — Grille isométrique + get_neighbors()
utils/proto_debug.py     — decode_protobuf_recursive, format_proto_tree
data/matching.json       — Mapping code 3 lettres → nom stable
```

---

## PHASE 1 — Fixer la grille Dofus 3 (5 min, débloque stop_adjacent)

### Tâche 1.1 — Changer MAP_WIDTH de 14 à 34

**Fichier** : `game/map_grid.py`

**Problème** : La grille utilise une largeur de 14 (Dofus 2). Dofus 3 utilise une largeur de 34.
**Preuve** : Sur la map, cellule 289 a pour vrai voisin 323. 323 - 289 = 34. En Dofus 2, le voisin serait 303 (289 + 14).

**Actions** :
1. Ouvrir `game/map_grid.py`
2. Chercher la constante `MAP_WIDTH` (ou `GRID_WIDTH`, `WIDTH`, ou `14` utilisé comme largeur de grille)
3. Changer la valeur de **14** à **34**
4. NE PAS TOUCHER l'algorithme A* lui-même, ni le nombre total de cellules (560)
5. Vérifier que `get_neighbors()` utilise bien cette constante pour calculer les voisins

**Vérification des voisins après correction** :
Pour une cellule `c` avec largeur 34, les 8 voisins en grille isométrique Dofus sont :
```
Nord-Ouest: c - 34        Nord-Est: c - 33
Ouest:      c - 1          Est:      c + 1
Sud-Ouest:  c + 33        Sud-Est:  c + 34
Nord:       c - 67 (c - 2*34 + 1)   Sud: c + 67 (c + 2*34 - 1)
```

**ATTENTION** : Le pattern de voisinage pourrait être DIFFÉRENT de Dofus 2. Si la grille Dofus 3 est rectangulaire standard (pas isométrique losange), les voisins sont simplement :
```
Haut: c - 34
Bas:  c + 34
Gauche: c - 1  (si c % 34 != 0)
Droite: c + 1  (si c % 34 != 33)
```
Tester d'abord avec les 4 voisins orthogonaux. Si le pathfinding donne des résultats aberrants, passer aux 8 voisins.

**NE PAS** :
- Toucher à l'algo A*
- Changer le nombre total de cellules (560)
- Modifier le format de keyCells (direction << 12 | cellId)

---

## PHASE 2 — Fixer InteractiveUseRequest (débloquer le gather)

### Tâche 2.1 — Ajouter itk/ite dans le matching

**Fichier** : `data/matching.json` ET `game/message_handlers.py`

**Actions** :
1. Ajouter manuellement dans `matching.json` :
```json
{
  "itk": "InteractiveUseCheckRequest",
  "ite": "InteractiveUseCheckResponse"
}
```
2. Ajouter un handler pour `ite` (S2C) dans `message_handlers.py` qui :
   - Log la réception de `ite`
   - Set un flag `state.interaction_check_ok = True`
   - Dump le payload complet en hex + protobuf récursif pour analyse

**Note sur le matching protégé** : L'auto-matcher ne peut pas écraser un nom déjà mappé. En ajoutant `itk` et `ite` manuellement, on est sûr qu'ils ne seront pas ré-assignés par erreur.

---

### Tâche 2.2 — Sniffer et décoder itk (pré-interaction)

**Fichier** : `utils/proto_debug.py` (pour le debug) puis `game/movement.py` (pour le builder)

**Contexte** : Le vrai client envoie `itk` (36 bytes total) AVANT `itl`. Le serveur répond `ite` (36 bytes). Sans cette étape, le serveur ignore `itl`.

**Actions** :
1. Ajouter un log temporaire dans le sniffer qui capture TOUT paquet C2S avec le code `itk` :
```python
# Dans le handler de paquets C2S, ajouter :
if code == "itk" or (hasattr(matching, 'get_name') and matching.get_name(code) == "InteractiveUseCheckRequest"):
    logger.info(f"[CAPTURE] itk raw hex: {payload.hex()}")
    tree = decode_protobuf_recursive(payload)
    logger.info(f"[CAPTURE] itk decoded: {format_proto_tree(tree)}")
```
2. Jouer manuellement dans le jeu : se positionner à côté d'une ressource et cliquer dessus pour récolter
3. Observer les logs pour capturer le `itk` envoyé par le vrai client
4. Analyser le payload décodé — il contient probablement :
   - `elementId` de la ressource ciblée (varint)
   - Possiblement `skillId` aussi
   - Possiblement un `cellId` ou `mapId`
5. Comparer avec le `itl` qui suit immédiatement dans les logs

**Résultat attendu** : On connaît la structure exacte du payload de `itk`.

---

### Tâche 2.3 — Implémenter build_pre_interact_request()

**Fichier** : `game/movement.py` (ou créer `game/interaction.py` si movement.py est trop gros)

**Actions** :
1. Créer la fonction `build_pre_interact_request(element_id, skill_id)` basée sur le sniff de la tâche 2.2
2. Le wrapping GameMessage DOIT être IDENTIQUE à celui utilisé pour le move (field 2, pas field 4) :
```python
def build_pre_interact_request(element_id: int, skill_id: int) -> bytes:
    """
    Construit un InteractiveUseCheckRequest (itk).
    Structure exacte à confirmer par le sniff de la tâche 2.2.
    """
    inner = b""
    # STRUCTURE À DÉTERMINER APRÈS SNIFF — les fields ci-dessous sont une HYPOTHÈSE
    # Hypothèse 1 : même structure que itl (f1=elementId, f2=skillId)
    # Hypothèse 2 : juste elementId
    # Le sniff de la tâche 2.2 tranchera
    inner += encode_varint_field(field_number=1, value=element_id)
    inner += encode_varint_field(field_number=2, value=skill_id)
    return inner
```
3. Wrapper avec GameMessage en utilisant le code `itk` depuis matching.json :
```python
code = matching.get_code("InteractiveUseCheckRequest")  # → "itk"
# Même wrapping que build_move_request() : GameMessage field 2
```

---

### Tâche 2.4 — Sniffer et comparer itl client vs itl bot

**But** : Comprendre la différence de 3 bytes entre le vrai client (43 bytes) et le bot (46 bytes).

**Actions** :
1. Capturer un `itl` du vrai client (même technique que tâche 2.2)
2. Capturer le `itl` que le bot envoie
3. Comparer byte par byte les deux payloads :
```python
# Script de comparaison temporaire à ajouter dans proto_debug.py :
def compare_packets(label_a: str, hex_a: str, label_b: str, hex_b: str):
    bytes_a = bytes.fromhex(hex_a)
    bytes_b = bytes.fromhex(hex_b)
    logger.info(f"--- Comparaison {label_a} ({len(bytes_a)}B) vs {label_b} ({len(bytes_b)}B) ---")
    tree_a = decode_protobuf_recursive(bytes_a)
    tree_b = decode_protobuf_recursive(bytes_b)
    logger.info(f"{label_a}: {format_proto_tree(tree_a)}")
    logger.info(f"{label_b}: {format_proto_tree(tree_b)}")
    # Diff hex
    max_len = max(len(bytes_a), len(bytes_b))
    for i in range(max_len):
        a = f"{bytes_a[i]:02x}" if i < len(bytes_a) else "--"
        b = f"{bytes_b[i]:02x}" if i < len(bytes_b) else "--"
        if a != b:
            logger.info(f"  DIFF byte {i}: {label_a}={a} {label_b}={b}")
```
4. Les différences possibles :
   - **Field order inversé** : f1=skillId, f2=elementId au lieu de f1=elementId, f2=skillId
   - **Varint encoding différent** : le bot encode un int32 sur plus de bytes que nécessaire
   - **Field supplémentaire** : le bot ajoute un field que le vrai client n'envoie pas
   - **Wrapping différent** : le uid ou le type_url diffère
5. Corriger `build_interact_request()` en conséquence

---

### Tâche 2.5 — Modifier gathering.py pour la séquence complète

**Fichier** : `game/gathering.py`

**Problème** : Le bot envoie `itl` directement. La séquence correcte est :
```
1. Bot envoie itk (pre-check)        → serveur
2. Serveur répond ite (OK)            ← serveur
3. Bot envoie itl (InteractiveUse)    → serveur
4. Serveur répond irj (validated)     ← serveur
5. Serveur envoie kof (ended)         ← serveur
```

**Actions** :
1. Trouver la fonction qui envoie `itl` dans `gathering.py` (probablement `gather()` ou `interact_with_resource()`)
2. AVANT l'envoi de `itl`, ajouter :
```python
async def gather_resource(self, element_id: int, skill_id: int):
    """Séquence complète de récolte."""

    # Étape 1 : Pré-check (itk)
    logger.info(f"Sending pre-interact check (itk) for element {element_id}")
    pre_check = build_pre_interact_request(element_id, skill_id)
    pre_check_wrapped = wrap_game_message(
        code=matching.get_code("InteractiveUseCheckRequest"),
        payload=pre_check
    )
    await self.send_to_server(pre_check_wrapped)

    # Étape 2 : Attendre ite (response du pré-check)
    logger.info("Waiting for pre-interact response (ite)...")
    state.interaction_check_ok = False
    try:
        # Attendre que le handler de ite set le flag
        for _ in range(30):  # 3 secondes max (30 * 100ms)
            await asyncio.sleep(0.1)
            if state.interaction_check_ok:
                break
        if not state.interaction_check_ok:
            logger.error("Pre-interact check timeout — server did not respond with ite")
            return False
    except Exception as e:
        logger.error(f"Pre-interact check failed: {e}")
        return False

    # Étape 3 : Délai humain entre pré-check et interact
    delay = random.randint(100, 300) / 1000.0
    await asyncio.sleep(delay)

    # Étape 4 : Envoyer itl (InteractiveUseRequest)
    logger.info(f"Sending interact request (itl) for element {element_id}, skill {skill_id}")
    interact = build_interact_request(element_id, skill_id)
    interact_wrapped = wrap_game_message(
        code=matching.get_code("InteractiveUseRequest"),
        payload=interact
    )
    await self.send_to_server(interact_wrapped)

    # Étape 5 : Attendre la fin de récolte (kof = InteractiveUseEndedEvent)
    logger.info("Waiting for harvest completion (kof)...")
    # Le handler de kof mettra à jour state.last_harvest_complete
    for _ in range(100):  # 10 secondes max
        await asyncio.sleep(0.1)
        if state.last_harvest_complete:
            logger.info("Harvest complete!")
            state.last_harvest_complete = False
            return True

    logger.warning("Harvest timeout — no kof received in 10s")
    return False
```

3. Ajouter dans `state.py` :
```python
self.interaction_check_ok = False
self.last_harvest_complete = False
```

4. Ajouter le handler `ite` dans `message_handlers.py` :
```python
def handle_interactive_check_response(direction: str, payload: bytes, state: GameState):
    """Handler pour InteractiveUseCheckResponse (ite, S2C)."""
    logger.info(f"Received pre-interact response (ite), {len(payload)} bytes")
    tree = decode_protobuf_recursive(payload)
    logger.debug(f"ite payload: {format_proto_tree(tree)}")
    state.interaction_check_ok = True
```

5. S'assurer que le handler `kof` (InteractiveUseEndedEvent) existe et fait :
```python
def handle_interactive_use_ended(direction: str, payload: bytes, state: GameState):
    """Handler pour InteractiveUseEndedEvent (kof, S2C)."""
    logger.info("Harvest ended (kof received)")
    state.last_harvest_complete = True
```

---

## PHASE 3 — Décoder IAL (walkability grid depuis les sockets)

### Tâche 3.1 — Analyser la structure détaillée de IAL

**Fichier** : `game/message_handlers.py` + `utils/proto_debug.py`

**Contexte** : IAL est un message S2C de 81KB contenant 8987 entries dans field 2. Chaque entry a :
- `f1` = varint (grand nombre) → probablement un bitmask de propriétés
- `f2` = varint (16384 + index) → encode le cellId comme `f2 - 16384`

485 cellIds uniques ont été extraits dans le range 0-559.

**Actions** :
1. Modifier le handler IAL existant pour faire un dump COMPLET et structuré :
```python
def handle_ial_detailed(direction: str, payload: bytes, state: GameState):
    """Analyse détaillée de IAL pour extraire la walkability grid."""
    tree = decode_protobuf_recursive(payload)

    if 2 not in tree:
        logger.warning("IAL: no field 2 found")
        return

    entries = tree[2]  # Liste de tuples ("message", sub_tree) ou ("bytes", hex)
    logger.info(f"IAL: {len(entries)} entries in field 2")

    # Construire un dict cellId → liste de propriétés (f1 values)
    cell_properties = {}  # cellId → [f1_value, f1_value, ...]
    f1_values_set = set()

    for i, (entry_type, entry_data) in enumerate(entries):
        if entry_type != "message":
            continue

        f1_vals = entry_data.get(1, [])
        f2_vals = entry_data.get(2, [])

        f1_value = f1_vals[0][1] if f1_vals else None
        f2_value = f2_vals[0][1] if f2_vals else None

        if f2_value is not None and f2_value >= 16384:
            cell_id = f2_value - 16384
            if 0 <= cell_id <= 559:
                cell_properties.setdefault(cell_id, []).append(f1_value)
                if f1_value is not None:
                    f1_values_set.add(f1_value)

    logger.info(f"IAL: {len(cell_properties)} unique cellIds extracted")
    logger.info(f"IAL: {len(f1_values_set)} unique f1 property values")

    # Analyser les f1 values — chercher des patterns de bits
    for val in sorted(f1_values_set)[:20]:  # Les 20 premières
        logger.info(f"  f1={val} (bin={val:032b})")

    # Sauvegarder pour analyse offline
    import json
    debug_path = "data/ial_debug.json"
    with open(debug_path, "w") as f:
        json.dump({
            "total_entries": len(entries),
            "unique_cells": len(cell_properties),
            "cell_properties": {str(k): v for k, v in sorted(cell_properties.items())},
            "unique_f1_values": sorted(list(f1_values_set))
        }, f, indent=2)
    logger.info(f"IAL debug data saved to {debug_path}")

    # Stocker dans state pour utilisation par le pathfinding
    state.ial_cell_properties = cell_properties
```

2. Exécuter le bot, changer de map, et analyser le fichier `data/ial_debug.json` généré

---

### Tâche 3.2 — Identifier les bits de walkability dans f1

**Prérequis** : Le fichier `data/ial_debug.json` de la tâche 3.1

**Méthode de test contrôlé** :
1. Se positionner sur une map dans le jeu
2. Identifier visuellement 5 cellules walkable (où le personnage peut aller) et noter leurs cellIds (en utilisant un `move <cell>` qui marche + confirmation serveur)
3. Identifier visuellement 5 cellules NON-walkable (murs, eau, obstacles) — celles où `move <cell>` est refusé (isl)
4. Comparer les valeurs `f1` de ces deux groupes dans `ial_debug.json`
5. Chercher un bit commun aux walkable et absent des non-walkable

```python
# Script d'analyse à ajouter dans utils/proto_debug.py :
def analyze_ial_walkability(ial_debug_path: str, walkable_cells: list[int], blocked_cells: list[int]):
    """
    Compare les propriétés f1 des cellules walkable vs bloquées.
    walkable_cells: [256, 289, 323, ...] — cellules confirmées walkable
    blocked_cells: [0, 1, 559, ...] — cellules confirmées bloquées
    """
    import json
    with open(ial_debug_path) as f:
        data = json.load(f)

    props = data["cell_properties"]

    print("=== WALKABLE CELLS ===")
    for cell in walkable_cells:
        vals = props.get(str(cell), [])
        for v in vals:
            print(f"  cell {cell}: f1={v} (bin={v:032b})" if v else f"  cell {cell}: f1=None")

    print("\n=== BLOCKED CELLS ===")
    for cell in blocked_cells:
        vals = props.get(str(cell), [])
        for v in vals:
            print(f"  cell {cell}: f1={v} (bin={v:032b})" if v else f"  cell {cell}: f1=None")
        if not vals:
            print(f"  cell {cell}: NOT IN IAL (absent = peut-être bloqué par défaut?)")

    # Analyse automatique des bits
    walk_bits = set(range(32))
    block_bits = set(range(32))
    for cell in walkable_cells:
        for v in props.get(str(cell), []):
            if v is not None:
                for bit in range(32):
                    if not (v & (1 << bit)):
                        walk_bits.discard(bit)
    for cell in blocked_cells:
        for v in props.get(str(cell), []):
            if v is not None:
                for bit in range(32):
                    if not (v & (1 << bit)):
                        block_bits.discard(bit)

    common_walk = walk_bits - block_bits
    print(f"\nBits TOUJOURS set dans walkable, JAMAIS dans bloqué: {common_walk}")
    print("Ce sont les candidats pour le bit de walkability")
```

**Action** : Exécuter cette analyse avec les cellules confirmées, identifier le bit de walkability, puis passer à la tâche 3.3.

---

### Tâche 3.3 — Intégrer la walkability IAL dans le pathfinding

**Fichier** : `game/map_grid.py` ou `game/navigation.py`

**Prérequis** : Le bit de walkability est identifié (tâche 3.2)

**Actions** :
1. Créer une fonction qui construit le set de cellules walkable depuis IAL :
```python
WALKABILITY_BIT = ???  # À déterminer tâche 3.2 (ex: bit 3, bit 5, etc.)

def build_walkable_set_from_ial(cell_properties: dict) -> set:
    """
    Construit le set de cellules walkable depuis les données IAL.
    cell_properties: {cellId: [f1_value, ...]} depuis state.ial_cell_properties
    """
    walkable = set()
    for cell_id, props in cell_properties.items():
        cell_id = int(cell_id) if isinstance(cell_id, str) else cell_id
        for f1_val in props:
            if f1_val is not None and (f1_val & (1 << WALKABILITY_BIT)):
                walkable.add(cell_id)
                break
    return walkable
```

2. Intégrer dans `navigation.py` — remplacer la walkability statique par la walkability IAL :
```python
# AVANT (walkability observée seulement) :
# if cell in state._observed_walkable:

# APRÈS (IAL + observée comme fallback) :
def is_cell_walkable(cell_id: int, state: GameState) -> bool:
    # Priorité 1 : IAL data (grille serveur)
    if hasattr(state, 'ial_walkable_set') and state.ial_walkable_set:
        return cell_id in state.ial_walkable_set
    # Priorité 2 : walkability observée (fallback)
    return cell_id in state._observed_walkable
```

3. Mettre à jour `state.ial_walkable_set` chaque fois qu'on reçoit un IAL :
```python
# Dans handle_ial_detailed(), après avoir construit cell_properties :
state.ial_walkable_set = build_walkable_set_from_ial(cell_properties)
logger.info(f"Walkable set from IAL: {len(state.ial_walkable_set)} cells")
```

**IMPORTANT** : NE PAS supprimer la walkability observée (`_observed_walkable`). Elle sert de fallback si l'analyse IAL est incomplète ou si le bit de walkability est mal identifié.

---

## PHASE 4 — Décoder KWW (données pré-map change)

### Tâche 4.1 — Dump structuré de KWW

**Fichier** : `game/message_handlers.py`

**Contexte** : KWW arrive JUSTE AVANT ISU lors d'un changement de map. 2100 bytes, 297 entries dans field 1. Chaque entry a f1, f2, f3, f4. Les f3 contiennent des bitmasks (1024+n, 512+n).

**Actions** :
1. Modifier le handler KWW existant pour un dump complet :
```python
def handle_kww_detailed(direction: str, payload: bytes, state: GameState):
    """Analyse détaillée de KWW pour comprendre le format."""
    tree = decode_protobuf_recursive(payload)

    if 1 not in tree:
        logger.warning("KWW: no field 1 found")
        return

    entries = tree[1]
    logger.info(f"KWW: {len(entries)} entries in field 1")

    # Dump les 10 premières entries pour analyse
    for i, (entry_type, entry_data) in enumerate(entries[:10]):
        if entry_type == "message":
            f1 = entry_data.get(1, [])
            f2 = entry_data.get(2, [])
            f3 = entry_data.get(3, [])
            f4 = entry_data.get(4, [])
            logger.info(f"  KWW entry {i}: f1={f1} f2={f2} f3={f3} f4={f4}")

    # Analyser f3 en tant que bitmasks
    f3_values = []
    for entry_type, entry_data in entries:
        if entry_type == "message":
            for vtype, val in entry_data.get(3, []):
                if vtype == "varint":
                    f3_values.append(val)

    logger.info(f"KWW f3 values: min={min(f3_values) if f3_values else 'N/A'}, max={max(f3_values) if f3_values else 'N/A'}")

    # Décomposer les f3 en bits connus
    for val in sorted(set(f3_values))[:20]:
        base = val & 0x1FF  # 9 bits bas = cellId potentiel ?
        flags = val >> 9    # bits hauts = flags ?
        logger.info(f"  f3={val}: base={base}, flags={flags} (bin flags={flags:08b})")

    # Sauvegarder pour analyse offline
    import json
    debug_path = "data/kww_debug.json"
    all_entries = []
    for entry_type, entry_data in entries:
        if entry_type == "message":
            entry = {}
            for fn in [1, 2, 3, 4]:
                vals = entry_data.get(fn, [])
                entry[f"f{fn}"] = [v for _, v in vals]
            all_entries.append(entry)
    with open(debug_path, "w") as f:
        json.dump({"total": len(all_entries), "entries": all_entries}, f, indent=2)
    logger.info(f"KWW debug data saved to {debug_path}")
```

2. Exécuter le bot, changer de map, analyser `data/kww_debug.json`

---

### Tâche 4.2 — Hypothèses à tester sur KWW

**Après avoir le dump de la tâche 4.1, tester ces hypothèses** :

**Hypothèse A** : f3 encode `(flags << 10) | cellId` (comme keyCells encode `(direction << 12) | cellId`)
- Test : extraire `f3 & 0x3FF` pour chaque entry → est-ce dans le range 0-559 ?
- Si oui, les flags (bits hauts) indiquent le type d'obstacle

**Hypothèse B** : KWW contient les cellules NON-walkable (297 entries sur 560 = 263 walkable)
- Test : comparer les cellIds extraites de KWW avec le set walkable de IAL
- Si les 297 cellIds de KWW sont ABSENTES du set walkable IAL → KWW = liste des obstacles

**Hypothèse C** : KWW contient les cellules avec propriétés spéciales (transitions de map, escaliers, etc.)
- Test : comparer avec les transitions de map connues dans worldgraph.json
- Si certains cellIds de KWW correspondent à des cellules de transition → KWW enrichit la grille

**Hypothèse D** : f1 = cellId, f3 = bitmask de propriétés
- Test : est-ce que f1 est dans le range 0-559 ?
- Si oui, f3 contient les flags (walkable, obstacle type, etc.)

```python
# Script de test des hypothèses (à exécuter manuellement) :
def test_kww_hypotheses(kww_debug_path: str, ial_debug_path: str = None):
    import json
    with open(kww_debug_path) as f:
        kww = json.load(f)

    entries = kww["entries"]

    # Hypothèse A : f3 & 0x3FF = cellId ?
    print("=== HYPOTHÈSE A: f3 & 0x3FF = cellId ===")
    cells_from_f3 = set()
    for e in entries:
        for v in e.get("f3", []):
            cell = v & 0x3FF
            if 0 <= cell <= 559:
                cells_from_f3.add(cell)
    print(f"  {len(cells_from_f3)} cellIds dans range 0-559 depuis f3 & 0x3FF")

    # Hypothèse D : f1 = cellId ?
    print("\n=== HYPOTHÈSE D: f1 = cellId ===")
    cells_from_f1 = set()
    for e in entries:
        for v in e.get("f1", []):
            if 0 <= v <= 559:
                cells_from_f1.add(v)
    print(f"  {len(cells_from_f1)} cellIds dans range 0-559 depuis f1")

    # Cross-check avec IAL si disponible
    if ial_debug_path:
        with open(ial_debug_path) as f:
            ial = json.load(f)
        ial_cells = set(int(k) for k in ial["cell_properties"].keys())
        print(f"\n=== CROSS-CHECK ===")
        print(f"  IAL cells: {len(ial_cells)}")
        print(f"  KWW f3 cells: {len(cells_from_f3)}")
        overlap = ial_cells & cells_from_f3
        print(f"  Overlap: {len(overlap)}")
        only_kww = cells_from_f3 - ial_cells
        only_ial = ial_cells - cells_from_f3
        print(f"  Only in KWW: {len(only_kww)}")
        print(f"  Only in IAL: {len(only_ial)}")
```

---

## PHASE 5 — Décoder cellId du joueur depuis ISU (BUG 3)

### Tâche 5.1 — Dump protobuf récursif des actors ISU

**Fichier** : `game/message_handlers.py`

**Contexte** : ISU field 11 contient les acteurs. Chaque acteur a f1=header(5 bytes), f2=actorId(varint), f3=position(90-139 bytes). Le cellId est caché dans f3 mais on ne sait pas où exactement. On sait que f3.f5.f2 = 3 pour TOUS les acteurs (c'est un enum, PAS le cellId).

**Actions** :
1. Ajouter un mode debug dans le handler ISU qui dump f3 de chaque acteur :
```python
def handle_isu_actor_debug(direction: str, payload: bytes, state: GameState):
    """Mode debug : dump complet des acteurs ISU pour trouver le cellId."""
    tree = decode_protobuf_recursive(payload)

    actors = tree.get(11, [])
    map_id = None
    for vtype, val in tree.get(14, []):
        if vtype == "varint":
            map_id = val

    logger.info(f"ISU debug: mapId={map_id}, {len(actors)} actors")

    for i, (actor_type, actor_data) in enumerate(actors):
        if actor_type != "message":
            continue

        # Extraire actorId
        actor_id = None
        for vtype, val in actor_data.get(2, []):
            if vtype == "varint":
                actor_id = val

        # Extraire f3 (position bytes) et le décoder récursivement
        position_entries = actor_data.get(3, [])
        for ptype, pdata in position_entries:
            if ptype == "message":
                # f3 est déjà décodé récursivement
                logger.info(f"  Actor {i} (id={actor_id}): f3 tree = {format_proto_tree(pdata)}")

                # Chercher toutes les valeurs varint dans le range 0-559
                values_in_range = find_values_in_range(pdata, 0, 559)
                if values_in_range:
                    logger.info(f"    → Values 0-559 found: {values_in_range}")
            elif ptype == "bytes":
                # f3 est des bytes bruts — décoder manuellement
                raw = bytes.fromhex(pdata)
                sub_tree = decode_protobuf_recursive(raw)
                logger.info(f"  Actor {i} (id={actor_id}): f3 decoded = {format_proto_tree(sub_tree)}")

                values_in_range = find_values_in_range(sub_tree, 0, 559)
                if values_in_range:
                    logger.info(f"    → Values 0-559 found: {values_in_range}")
```

2. Ajouter `find_values_in_range()` dans `utils/proto_debug.py` s'il n'existe pas déjà :
```python
def find_values_in_range(tree: dict, min_val: int, max_val: int, path: str = "") -> list:
    """
    Cherche récursivement toutes les valeurs varint dans [min_val, max_val].
    Retourne une liste de (path, value).
    """
    results = []
    for field_num, entries in tree.items():
        for entry_type, entry_data in entries:
            current_path = f"{path}.f{field_num}" if path else f"f{field_num}"
            if entry_type == "varint" and min_val <= entry_data <= max_val:
                results.append((current_path, entry_data))
            elif entry_type == "message" and isinstance(entry_data, dict):
                results.extend(find_values_in_range(entry_data, min_val, max_val, current_path))
    return results
```

---

### Tâche 5.2 — Test contrôlé pour identifier le cellId

**Prérequis** : Le dump debug de la tâche 5.1 fonctionne

**Procédure** :
1. Se positionner sur une cellule CONNUE (ex: faire `move 289`, confirmer que ça marche)
2. Changer de map (aller sur la map d'à côté)
3. Observer le dump ISU : chercher la valeur du cellId de spawn parmi les "Values 0-559 found"
4. Se souvenir du chemin (ex: `f3.f7.f2` = 289)
5. Répéter 3 fois sur des maps différentes pour confirmer que c'est toujours le même chemin
6. Hardcoder l'extraction :
```python
# Une fois le chemin identifié (exemple : f3.f7.f2) :
CELL_ID_PATH = [3, 7, 2]  # À déterminer par le test

def extract_cell_from_actor(actor_tree: dict) -> int | None:
    """Extrait le cellId d'un acteur ISU en suivant le chemin connu."""
    current = actor_tree
    for field_num in CELL_ID_PATH[:-1]:
        entries = current.get(field_num, [])
        found = False
        for entry_type, entry_data in entries:
            if entry_type == "message":
                current = entry_data
                found = True
                break
        if not found:
            return None
    # Dernier champ = varint
    last_field = CELL_ID_PATH[-1]
    for entry_type, entry_data in current.get(last_field, []):
        if entry_type == "varint" and 0 <= entry_data <= 559:
            return entry_data
    return None
```

7. Identifier le JOUEUR parmi les acteurs :
```python
# Le joueur est identifiable par son actorId (= characterId du GameState)
# OU par f3.f5.f5 = 1 (race=joueur, vs 3591-3622=monstres)
for actor in actors:
    actor_id = actor["f2"]
    cell_id = extract_cell_from_actor(actor["f3"])
    if actor_id == state.character_id:
        state.current_cell = cell_id
        state.position_source = "map_info"
        logger.info(f"Player cell from ISU: {cell_id}")
```

---

## PHASE 6 — Validation bout en bout du gather

### Tâche 6.1 — Test gather complet

**Prérequis** : Phases 1-2 complétées (grille + séquence itk/itl)

**Procédure** :
1. Lancer le bot en mode actif
2. Se positionner manuellement PRÈS d'une ressource récoltable (à 1-2 cellules)
3. Vérifier dans les logs que la ressource est détectée (ISU field 2 + field 6, status=1)
4. Exécuter `gather`
5. Vérifier dans les logs la séquence complète :
```
[INFO] Closest resource: elementId=XXX, cellId=YYY, skillId=ZZZ
[INFO] Moving to adjacent cell AAA
[INFO] Sending pre-interact check (itk) for element XXX
[INFO] Received pre-interact response (ite)          ← CRITIQUE : si absent, le serveur ignore itk
[INFO] Sending interact request (itl) for element XXX, skill ZZZ
[INFO] Harvest complete! (kof received)               ← CRITIQUE : si absent, itl est ignoré
```

6. Si `ite` n'arrive pas après `itk` :
   - Vérifier que le wrapping GameMessage est identique au move (field 2, pas field 4)
   - Vérifier que le code `itk` est correct dans matching.json
   - Comparer le paquet envoyé avec le sniff du vrai client

7. Si `kof` n'arrive pas après `itl` :
   - Comparer hex du itl bot vs itl vrai client (tâche 2.4)
   - Vérifier le field order (f1=elementId vs f1=skillId)
   - Vérifier la taille (43 bytes attendu vs 46 bytes bot)

---

## Résumé de l'ordre d'exécution

```
PHASE 1 — Grille Dofus 3 (5 min)
  1.1  MAP_WIDTH 14 → 34 dans map_grid.py

PHASE 2 — Fixer le gather (1-2h, nécessite tests manuels)
  2.1  Ajouter itk/ite dans matching.json
  2.2  Sniffer itk du vrai client (jeu manuel)
  2.3  Implémenter build_pre_interact_request()
  2.4  Comparer itl client vs itl bot (hex diff)
  2.5  Modifier gathering.py : séquence itk → ite → itl → kof

PHASE 3 — Décoder IAL (30 min code + analyse manuelle)
  3.1  Dump structuré IAL → data/ial_debug.json
  3.2  Identifier le bit de walkability (test contrôlé)
  3.3  Intégrer walkability IAL dans le pathfinding

PHASE 4 — Décoder KWW (30 min code + analyse manuelle)
  4.1  Dump structuré KWW → data/kww_debug.json
  4.2  Tester les hypothèses A/B/C/D

PHASE 5 — CellId joueur depuis ISU (30 min code + test contrôlé)
  5.1  Dump protobuf récursif des actors
  5.2  Test contrôlé → identifier le chemin du cellId → hardcoder

PHASE 6 — Validation
  6.1  Test gather complet (séquence itk → ite → itl → kof)
```

---

## Notes pour Claude Code

- **NE PAS TOUCHER** : l'algo A* dans pathfinding.py — seul `get_neighbors()` change via MAP_WIDTH
- **NE PAS TOUCHER** : le décodeur wire format protobuf dans packet_handler.py
- **NE PAS TOUCHER** : le wrapping GameMessage — il marche déjà pour le move, le réutiliser tel quel pour itk/itl
- Le wrapping C2S utilise **field 2 du GameMessage** (PAS field 4). C'est vérifié et le move fonctionne avec.
- Toujours utiliser `matching.get_code("NomStable")` pour obtenir le code 3 lettres actuel
- Les phases 3, 4, 5 nécessitent des TESTS MANUELS dans le jeu (déplacements, captures de paquets). Le code génère les dumps, l'humain les analyse.
- Les fichiers `data/ial_debug.json` et `data/kww_debug.json` sont des fichiers temporaires de debug — ne pas les versionner
- La séquence gather (phase 2) est le bloqueur principal. Les phases 3-5 améliorent la qualité mais ne sont pas strictement nécessaires pour un premier gather fonctionnel.
- Si le bit de walkability dans IAL n'est pas identifiable, garder la walkability observée (`_observed_walkable`) comme système principal et ajouter les cellules confirmées walkable par les mouvements réussis.

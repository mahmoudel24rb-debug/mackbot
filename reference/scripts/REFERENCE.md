# Jitsuri Lua Script Reference

## Structure d'un script

Tout script Jitsuri est un fichier `.lua` avec :
1. Des **variables globales** de configuration
2. Une fonction **`move()`** (obligatoire) - boucle principale
3. Une fonction **`bank()`** (optionnel) - quand pods pleins

```lua
-- === CONFIGURATION ===
ELEMENTS_TO_GATHER = { 38 }           -- IDs interactifs a recolter
MAX_PODS = 90                          -- % pods avant retour banque
AUTO_DELETE = {}                       -- GIDs items a supprimer auto
MIN_MONSTERS = 1                       -- Min monstres par groupe
MAX_MONSTERS = 8                       -- Max monstres par groupe
FORBIDDEN_MONSTERS = {}                -- IDs monstres a eviter
FORCE_MONSTERS = {}                    -- IDs monstres a forcer
FIGHT_LOCK_SPEC = true                 -- Bloquer spectateurs
FIGHT_LOCK_JOIN = true                 -- Bloquer join combat
FIGHT_LOCK_PARTY = false               -- Seuls les membres du groupe peuvent join

-- === BOUCLE PRINCIPALE ===
function move()
    return {
        { map = "5,-18", path = "bottom", gather = true, fight = false },
        { map = "5,-17", path = "left", gather = true, fight = false },
        { map = "4,-17", path = "top", gather = true, fight = false },
        { map = "4,-18", path = "5,-18", gather = false, fight = false },
    }
end

-- === BANQUE ===
function bank()
    return {
        { map = "192415750", npcBank = true }
    }
end
```

---

## Cles d'action (dans move/bank)

| Cle | Type | Description |
|-----|------|-------------|
| `map` | `"x,y"` ou `int` | Coordonnees de la map ou mapId |
| `path` | `string` ou `int` | Direction de sortie: `"top"`, `"bottom"`, `"left"`, `"right"`, mapId, ou `"zaap(mapId)"` |
| `changeMap` | `string` | Alias de path (format FlatyBot) |
| `cell` | `int` | Cell ID precise, declanche changement de map si sur bord |
| `gather` | `bool` | Recolter les elements interactifs de la map |
| `fight` | `bool` | Combattre les monstres sur la map |
| `custom` | `string/function` | Fonction custom executee AVANT le deplacement |
| `npcBank` | `bool` | Ouvre PNJ banque, depose tout, ferme |
| `chestGuild` | `bool` | Ouvre coffre de guilde, depose l'inventaire |
| `chestGuildNumber` | `int` | Numero du coffre de guilde |
| `lockedHouse` | `string` | `"NomPerso\|Code"` (-1 si proprietaire) |
| `lockedStorage` | `string` | `"CellId\|Code"` (-1 si pas de code) |
| `crafting` | `bool` | Lancer sequence de craft |
| `craftItemGid` | `int` | GID de l'item a crafter |
| `craftBankMapId` | `int` | MapId de la banque pour le craft |
| `craftWorkshopMapId` | `int` | MapId de l'atelier |
| `craftSpecialWorkshop` | `bool` | Utiliser atelier special |
| `craftReturnBank` | `bool` | Retour banque apres craft |

---

## API Lua - Fonctions disponibles

### Navigation
| Fonction | Retour | Description |
|----------|--------|-------------|
| `getCurrentPos()` | `"x,y"` | Position actuelle en coordonnees |
| `getMapId()` | `int` | MapId de la map actuelle |
| `goToCellId(cellId)` | - | Deplacer vers une cellule |
| `changeMapByCellId(cellId)` | - | Deplacer + changer de map |
| `goToMapId(mapId)` | - | Aller a une map (utilise zaap si loin) |
| `goUseInteractive(cellId)` | - | Interagir avec objet a la cellule |

### Inventaire
| Fonction | Retour | Description |
|----------|--------|-------------|
| `getInventoryItemCount(gid)` | `int` | Quantite d'un item |
| `getInventoryItemByGid(gid)` | `{uid, gid, quantity}` ou `nil` | Details d'un item |
| `getEquippedItemAtPosition(pos)` | item ou `nil` | Item equipe a la position |
| `useInventoryItem(gid, qty)` | - | Utiliser un item |
| `equipItem(uid, position)` | - | Equiper un item |
| `unequipItem(uid)` | - | Desequiper un item |
| `DropItem(gid, qty)` | - | Jeter un item |
| `deleteItem(gid)` | - | Supprimer toute la quantite |

### Banque/Stockage
| Fonction | Retour | Description |
|----------|--------|-------------|
| `putAllItems()` | - | Deposer tout en banque |
| `putExistingItems()` | - | Deposer seulement les items deja en banque |
| `getAllItems()` | - | Retirer tout du stockage |
| `getExistingItems()` | - | Retirer seulement les items deja en inventaire |
| `getMaxQuantitesByGid(gid)` | `int` | Quantite max dispo dans stockage ouvert |

### Infos personnage
| Fonction | Retour | Description |
|----------|--------|-------------|
| `getPods()` | `int` | Pods actuels |
| `getPodsMax()` | `int` | Pods max |
| `getJobLevel(jobId)` | `int` | Niveau d'un metier |
| `getCharacterLevel()` | `int` | Niveau du personnage |
| `getClass()` | `int` | ID de la classe |
| `getTeam()` | `{breedId, name, level}[]` | Infos de l'equipe |

### PNJ / Dialogue
| Fonction | Retour | Description |
|----------|--------|-------------|
| `talkNpc(npcActorId)` | - | Ouvrir dialogue PNJ |
| `replyNpc(index)` | - | Repondre au dialogue (index) |
| `replyNpcAndChangeMap(index)` | - | Repondre + attendre changement de map |
| `leaveDialog()` | - | Fermer le dialogue |

### Actions d'equipe
| Fonction | Retour | Description |
|----------|--------|-------------|
| `teamUseInventoryItem(gid, qty)` | - | Tous les membres utilisent l'item |
| `teamGoToCellId(cellId)` | - | Deplacement de toute l'equipe |
| `upgradeCharacterStatsBatch(agi,str,vit,cha,int,wis)` | - | Monter les stats |
| `upgradeCharacterStatsBatchByName(name,...)` | - | Monter stats d'un membre par nom |

### Marketplace (HDV)
| Fonction | Retour | Description |
|----------|--------|-------------|
| `openSellerHdv(cellId)` | - | Ouvrir HDV vendeur |
| `sellAllItems()` | - | Vendre tout l'inventaire |
| `closeSellerHdv()` | - | Fermer HDV |

### Craft
| Fonction | Retour | Description |
|----------|--------|-------------|
| `craftItem(gid, bankMapId, workshopMapId, special, returnBank)` | - | Executer chaine de craft |

### Maison / Coffre
| Fonction | Retour | Description |
|----------|--------|-------------|
| `goToAndEnterHouse(doorMapId, nickname, code)` | - | Aller et entrer dans une maison |
| `goAndOpenChest(chestMapId, chestCellId, code)` | - | Ouvrir un coffre verrouille |

### Utilitaires
| Fonction | Retour | Description |
|----------|--------|-------------|
| `printMessage(msg, color)` | - | Afficher message (colors: green, yellow, red, blue, orange, info) |
| `delay(ms)` | - | Pause en millisecondes |
| `stopScript()` | - | Arreter le script proprement |

---

## IDs des ressources recoltables (ELEMENTS_TO_GATHER)

### Paysan (JobID: 28)
| ID | Ressource |
|----|-----------|
| 38 | Ble |
| 43 | Orge |
| 44 | Seigle |
| 45 | Avoine |
| 46 | Chanvre |
| 47 | Malt |
| 111 | Riz |
| 260 | Mais |
| 261 | Millet |
| 63 | Froment |
| 64 | Epeautre |
| 65 | Sorgho |

### Bucheron (JobID: 2)
| ID | Ressource |
|----|-----------|
| 1 | Frene |
| 8 | Chene |
| 28 | If |
| 29 | Ebene |
| 30 | Orme |
| 31 | Erable |
| 32 | Charme |
| 33 | Chataignier |
| 34 | Noyer |
| 35 | Merisier |
| 108 | Bambou |
| 109 | Bambou sombre |
| 110 | Bambou sacre |
| 121 | Kaliptus |
| 133 | Tremble |
| 401 | Pin |

### Mineur (JobID: 24)
| ID | Ressource |
|----|-----------|
| 17 | Fer |
| 24 | Argent |
| 25 | Or |
| 26 | Bauxite |
| 37 | Kobalte |
| 52 | Etain |
| 53 | Cuivre |
| 54 | Manganese |
| 55 | Bronze |
| 113 | Dolomite |
| 114 | Silicate |
| 131 | Perce-neige |
| 132 | Poisskaille |
| 134 | Frostiz |
| 135 | Obsidienne |
| 291 | Ecume de mer |

### Alchimiste (JobID: 26)
| ID | Ressource |
|----|-----------|
| 254 | Ortie |
| 255 | Sauge |
| 256 | Ginseng |
| 257 | Belladone |
| 258 | Mandragore |

### Pecheur (JobID: 36)
| ID | Ressource |
|----|-----------|
| 71 | Greuvette |
| 74 | Truite |
| 75 | Goujon |
| 76 | Poisson-Chaton |
| 77 | Crabe |
| 78 | Poisson Pane |
| 79 | Carpe d'Iem |
| 81 | Sardine Brillante |
| 263 | Brochet |
| 264 | Kralamoure |
| 265 | Anguille |
| 266 | Dorade Grise |
| 267 | Perche |
| 268 | Raie |
| 269 | Lotte |
| 270 | Requin Marteau-Faucille |
| 271 | Bar Rikain |
| 272 | Morue |
| 273 | Tanche |
| 274 | Espadon |

### Autres
| ID | Ressource |
|----|-----------|
| 61 | Edelweiss |
| 66 | Menthe Sauvage |
| 67 | Trefle a 5 feuilles |
| 68 | Orchidee Freyesque |
| 84 | Puits |
| 98 | Bombu |
| 101 | Oliviolet |
| 112 | Pandouille |
| 259 | Noisetier |
| 287 | Aquajou |
| 288 | Salikrone |
| 289 | Quisnoa |
| 297 | Patelle |
| 365 | Pichon d'encre |

---

## IDs des metiers (JobID)

| JobID | Metier |
|-------|--------|
| 2 | Bucheron |
| 11 | Forgeron |
| 13 | Sculpteur |
| 15 | Cordonnier |
| 16 | Bijoutier |
| 24 | Mineur |
| 26 | Alchimiste |
| 27 | Tailleur |
| 28 | Paysan |
| 36 | Pecheur |
| 41 | Chasseur |
| 44 | Forgemage |
| 48 | Sculptemage |
| 60 | Faconneur |
| 62 | Cordomage |
| 63 | Joaillomage |
| 64 | Costumage |
| 65 | Bricoleur |
| 74 | Facomage |

---

## Cycle d'execution du moteur Jitsuri

1. Charge le script Lua
2. Execute `move()` en boucle
3. Pour chaque waypoint dans la table retournee:
   a. Si `custom` defini → execute la fonction custom
   b. Si `gather = true` → recolte selon ELEMENTS_TO_GATHER
   c. Si `fight = true` → combat selon regles (MIN/MAX_MONSTERS, FORBIDDEN, etc.)
   d. Execute `path`/`changeMap` → deplacement vers la prochaine map
4. Quand pods >= MAX_PODS:
   a. Supprime les items dans AUTO_DELETE
   b. Execute `bank()`
   c. Reprend `move()` depuis le debut
5. Le moteur utilise automatiquement le Zaap le plus proche pour les destinations lointaines

---

## Conversion FlatyBot → Jitsuri

```
-- FlatyBot:
{ map = "9,0", gather = true, changeMap = "top" }

-- Jitsuri:
{ map = "9,0", path = "top", gather = true, fight = false }
```

Changements:
- `changeMap` → `path`
- Ajouter `fight = false` explicitement

---

## Conseils

- Tester le debut du trajet pour confirmer que le bot demarre correctement
- Verifier que chaque point de passage est atteignable
- Diviser les routes en segments pour tester avant de combiner
- Inclure des commentaires dans le code
- Faire des backups avant modifications majeures
- Le bot traite les instructions ligne par ligne

---

## Patterns avances

### Multi-Routes sans conditions (rotation automatique)

Au lieu de mettre des `if/else` partout, utiliser une table de routes avec un compteur.
Les verifications de conditions sont couteuses (surtout en string) - les eviter quand possible.

```lua
NB_ROUTES = 4
CURRENT_ROUTE = 1

ROUTES = {
    -- Route A
    {
        { map = 88082704, path = "bottom" },
        { map = 88082703, path = "bottom" },
        { map = 88082702, gather = true, custom = function() ChangeRoute() end }
    },
    -- Route B
    {
        { map = 88082702, path = "right" },
        { map = 88083214, path = "right" },
        { map = 88083726, gather = true, custom = function() ChangeRoute() end }
    },
    -- Route C
    {
        { map = 88083726, path = "top", gather = true },
        { map = 88083727, path = "top", gather = true },
        { map = 88083728, gather = true, custom = function() ChangeRoute() end }
    },
    -- Route D
    {
        { map = 88083728, path = "left", gather = true },
        { map = 88083216, path = "left", gather = true },
        { map = 88082704, gather = true, custom = function() ChangeRoute() end }
    }
}

function ChangeRoute()
    if CURRENT_ROUTE >= NB_ROUTES then CURRENT_ROUTE = 1
    else CURRENT_ROUTE = CURRENT_ROUTE + 1 end
end

function move() return ROUTES[CURRENT_ROUTE] end
```

**Principes :**
- `ROUTES` = table de tables, chaque sous-table est un trajet complet
- `ChangeRoute()` appelee via `custom` a la FIN de chaque route
- `move()` retourne simplement `ROUTES[CURRENT_ROUTE]`
- Le dernier waypoint de chaque route DOIT appeler `ChangeRoute()`
- Utiliser `custom = function() ... end` pour les fonctions anonymes inline
- Les conditions restent necessaires pour: niveaux de metier, verification d'objets

### Template Donjon standard

Pattern reutilisable pour tous les donjons simples. Utilise dans: Kharnozor, AbraAncestral, Mantiscore, etc.

```lua
MIN_MONSTERS = 1
MAX_MONSTERS = 8
FIGHT_LOCK_SPEC = true
FIGHT_LOCK_JOIN = true
MAX_PODS = 90

function move()
    return {
        -- Map devant le donjon (coordonnees ou mapId)
        {map = "-3,25", path = "right", custom = interactWithEntryNPC},
        -- Salles de combat
        {map = "199491584", fight = true, path = "right"},
        {map = "199492608", fight = true, path = "bottom"},
        {map = "199493632", fight = true, path = "bottom"},
        {map = "199494656", fight = true, path = "right"},
        {map = "199495680", fight = true, path = "top"},  -- Boss
        -- PNJ de sortie
        {map = "199495682", path = "right", custom = interactWithExitNPC}
    }
end

function interactWithEntryNPC()
    delay(300)
    talkNpc(-20000)        -- -20000 = 1er PNJ sur la map
    delay(300)
    replyNpc(0)            -- 1ere reponse
    delay(300)
    replyNpc(0)            -- 2eme reponse
    delay(300)
    replyNpcAndChangeMap(0) -- repondre + changer de map
end

function interactWithExitNPC()
    talkNpc(-20000)        -- ou -20001, -20003 selon le PNJ
    delay(300)
    replyNpcAndChangeMap(0)
end

function bank() return {{map = "192415750", npcBank = true}} end
```

**Regles NPC ID :**
- `-20000` = 1er PNJ de la map
- `-20001` = 2eme PNJ
- `-20003` = 4eme PNJ
- Le numero depend de l'ordre des PNJ sur la map

### Phase-based State Machine (Donjons complexes)

Pour les donjons avec plusieurs chemins/boss (ex: DJ Blop avec 4 boss couleurs).

```lua
phase = "donjon"  -- variable globale de phase

function move()
    if phase == "donjon" then
        return cheminDonjon()
    elseif phase == "bossindigo" then
        return cheminBossIndigo()
    elseif phase == "bossmulti" then
        return bossMulti()
    else
        printMessage("Phase inconnue: " .. tostring(phase), "red")
        stopScript()
    end
end

function cheminDonjon()
    return {
        {map = "166985728", custom = interactWithEntryNPC},
        {map = "166986752", fight = true},
        {map = "166990848", custom = chooseBossRoom},  -- PNJ de choix
    }
end

function chooseBossRoom()
    local colorReply = { indigo = 0, griotte = 1, reinette = 2, coco = 3 }
    talkNpc(-20000)
    delay(800)
    replyNpc(0)
    delay(200)
    replyNpc(colorReply[bossColor] or 2)
    delay(200)
    phase = "boss" .. bossColor  -- change la phase
end
```

**Principes :**
- Variable globale `phase` controle le flux
- `move()` dispatch vers des fonctions differentes selon la phase
- `custom` modifie la phase pour orienter le prochain cycle
- Lookup tables (`colorReply`) au lieu de if/else pour les choix PNJ

### Filtrage dynamique de monstres par salle (checkMapId)

Adapter FORCE_MONSTERS/FORBIDDEN_MONSTERS selon la salle du donjon.

```lua
function move()
    checkMapId()  -- ajuster les filtres avant le retour
    return {
        {map = "8,29", path = "bottom", custom = interactWithEntryNPC},
        {map = "98566657", fight = true, path = "right"},
        {map = "98567681", fight = true, path = "bottom"},
        -- ...
    }
end

function checkMapId()
    local mapId = getMapId()
    if mapId == 98566657 then
        FORCE_MONSTERS = {429}         -- Gelee Citron
        FORBIDDEN_MONSTERS = {55, 56, 57}
    elseif mapId == 98567681 then
        FORCE_MONSTERS = {57, 55}      -- Fraise + Bleue
        FORBIDDEN_MONSTERS = {56, 429}
    elseif mapId == 98568705 then
        goUseInteractive(268)          -- Interagir avec objet
        delay(500)
    end
end
```

**Principes :**
- `checkMapId()` appele AU DEBUT de `move()` (avant return)
- Modifie les globales FORCE_MONSTERS/FORBIDDEN_MONSTERS dynamiquement
- `goUseInteractive(cellId)` pour interagir avec des elements non-PNJ
- `getMapId()` retourne le mapId numerique actuel

### Utilisation d'items (consommables)

```lua
function interactWithEntryNPC()
    useInventoryItem(996, 1)  -- GID 996 = MultiGely, quantite 1
    delay(1000)               -- attendre que l'item soit consomme
end
```

### Utilitaire printOnce (eviter le spam de logs)

Pattern commun dans tous les scripts de donjon :

```lua
local printedMessages = {}

function printOnce(message)
    if not printedMessages[message] then
        printMessage("[INFO] " .. message, "green")
        printedMessages[message] = true
    end
end
```

### Script modulaire avec dofile (Phossile)

Pour les scripts tres complexes, decouper en fichiers:

```lua
-- main.lua (point d'entree)
local function loadModule(name)
    local ok, result = pcall(function() return dofile(name .. ".lua") end)
    if ok then return result
    else printMessage("ERREUR " .. name .. ": " .. tostring(result), "red") end
end

loadModule("config")
loadModule("utils")
loadModule("combat_strategies")

function move()
    -- utilise les fonctions chargees par les modules
end
```

**Notes :** Le script Phossile utilise une API differente (console.print, npc.talk, global.sleep, map.fight) qui semble etre une surcouche custom. L'API standard Jitsuri utilise: printMessage, talkNpc, delay.

### Settings / Config externe (pattern Phossile)

```lua
-- settings.lua
local Settings = {}
Settings.PAIN_GID = 17189
Settings.PAIN_QUANTITY = 30
Settings.ENERGY_POTION_GID = 17195

-- Equipement auto par niveau
Settings.items = {
    {gid = 1489, pos = 0, level = 28},   -- Amulette (pos=0)
    {gid = 835,  pos = 1, level = 27},   -- Arme (pos=1)
    {gid = 732,  pos = 2, level = 26},   -- Anneau 1 (pos=2)
    {gid = 1487, pos = 3, level = 48},   -- Ceinture (pos=3)
    {gid = 732,  pos = 4, level = 26},   -- Anneau 2 (pos=4)
    {gid = 1665, pos = 5, level = 50},   -- Bottes (pos=5)
    {gid = 712,  pos = 6, level = 32},   -- Coiffe (pos=6)
    {gid = 6927, pos = 7, level = 37},   -- Cape (pos=7)
    {gid = 1711, pos = 8, level = 1},    -- Familier (pos=8)
    {gid = 972,  pos = 9, level = 60},   -- Dofus (pos=9)
    {gid = 18676,pos = 15,level = 1},    -- Bouclier (pos=15)
}

-- Stats
Settings.listStats = {
    {id = 1, name = "VIE"},
    {id = 2, name = "TERRE"},
    {id = 3, name = "FEU"},
    {id = 4, name = "EAU"},
    {id = 5, name = "AIR"},
    {id = 6, name = "SAGESSE"}
}
Settings.ChoiceStats = 6  -- SAGESSE

return Settings
```

**Positions d'equipement :**
| pos | Slot |
|-----|------|
| 0 | Amulette |
| 1 | Arme |
| 2 | Anneau 1 |
| 3 | Ceinture |
| 4 | Anneau 2 |
| 5 | Bottes |
| 6 | Coiffe |
| 7 | Cape |
| 8 | Familier |
| 9 | Dofus |
| 15 | Bouclier |

### Recolte multi-zones (Peche longue distance)

```lua
MAX_PODS = 95
ELEMENTS_TO_GATHER = {132, 273, 272, 274}  -- Poisskaille, Tanche, Morue, Espadon

function move()
    return {
        {map = "0,-57",  path = "1,-57",  gather = true, fight = false},
        {map = "1,-57",  path = "2,-57",  gather = true, fight = false},
        {map = "2,-57",  path = "4,-57",  gather = true, fight = false},
        -- ... 30+ maps, le bot boucle automatiquement
    }
end
-- Pas de bank() = le bot s'arrete quand pods pleins
```

**Principes :**
- `path` peut etre des coordonnees de la prochaine map (pas juste top/bottom/left/right)
- Pas besoin de `bank()` si on veut juste s'arreter quand pods pleins
- `gather = true` + `fight = false` sur TOUTES les maps de recolte
- Le moteur Jitsuri utilise les Zaaps automatiquement pour les maps distantes

---

## Jitsuri JSON Settings (combat AI)

Le fichier `.json` de Jitsuri contient la config complete du bot de combat:
- `confort_settings` : FPS, auto-accept, hide players, etc.
- `bot_settings.fight_settings` : placement, spells, kick, etc.
- Export/import depuis l'interface Jitsuri

---

## Exemples de scripts

### Recolte Ble Astrub (simple)
```lua
ELEMENTS_TO_GATHER = { 38 }
MAX_PODS = 90

function move()
    return {
        { map = "5,-18", path = "bottom", gather = true, fight = false },
        { map = "5,-17", path = "left", gather = true, fight = false },
        { map = "4,-17", path = "top", gather = true, fight = false },
        { map = "4,-18", path = "5,-18", gather = false, fight = false },
    }
end

function bank()
    return {
        { map = "192415750", npcBank = true }
    }
end
```

### Recolte multi-ressources
```lua
ELEMENTS_TO_GATHER = { 261, 254 } -- Millet, Ortie
MAX_PODS = 90

function move()
    return {
        { map = "5,-18", path = "bottom", gather = false, fight = false },
        { map = "5,-17", path = "left", gather = true, fight = false },
        { map = "4,-17", path = "top", gather = false, fight = false },
        { map = "4,-18", path = "5,-18", gather = false, fight = false },
    }
end
```

---

## Fichiers de donnees disponibles

- `Interactives.txt` - 389 elements interactifs (ID → Nom)
- `monstres.txt` - 2835 monstres (ID → Nom)
- `Objets.txt` - 19029 objets/items (GID → Nom)

---

## Notes d'analyse reseau (captures sniffer)

### Protocole de recolte (confirme par capture)
```
ipi (MoveRequest)  → Deplacement vers la ressource
ion (MoveEvent)    → Serveur confirme le mouvement
inq (MoveConfirm)  → Client confirme l'arrivee
ipa (MoveAck)      → Serveur acknowledge
iah                → Interaction/Recolte (f1=element_id, f3=instance_id)
iao/iaj/iam        → Animation de recolte (~3.5s)
```

### Protocole de changement de map
```
ipi → ion → inq → ipa  (move vers le bord)
ioh (MapChangeRequest)  → Client demande changement
ipq + iny (CurrentCellId) + kmh (ServerTime)  → Serveur repond
ipw (MapDataRequest)    → Client demande la nouvelle map
iou (MapDataResponse)   → Serveur envoie les donnees
```

### Message iah (InteractiveUse)
- f1 = ID de l'element interactif sur la map
- f3 = ID d'instance unique
- Envoye apres chaque deplacement vers une ressource
- ~3.5-4.1s entre chaque recolte (temps d'animation)
- 8-12 recoltes par map de farming typique

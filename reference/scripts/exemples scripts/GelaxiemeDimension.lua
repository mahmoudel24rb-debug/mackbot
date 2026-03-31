MIN_MONSTERS = 1
MAX_MONSTERS = 8
FIGHT_LOCK_SPEC = true
FIGHT_LOCK_JOIN = true

MAX_PODS = 90
local printedMessages = {}

function move()
    checkMapId()
    return {
        {map = "8,29", path = "bottom", custom = interactWithEntryNPC}, --map de la péninsule des gelées selctionnée arbitrairement pour utiliser le multigely
        
        -- Salles du donjon
        {map = "98566657", fight = true, path = "right"},
        {map = "98567681", fight = true, path = "bottom"},
        {map = "98566659", fight = true, path = "bottom"},
        {map = "98567683", fight = true, path = "right"},

        {map = "98568707", fight = true, path = "top", custom = interactWithExitNPC}, -- Salle du boss, on parle a une gelée
        
        {map = "98567687", fight = true, path = "right"}, -- Combat des 4 boss
        {map = "98567685", fight = true, path = "right"}, -- Combat de boss menthe
        {map = "98568709", fight = true, path = "right"}, -- Combat de boss fraise
        
    }
end

function interactWithEntryNPC()
    printOnce("On mange un multigely")
    useInventoryItem(996, 1)
    delay(1000)
end

function interactWithExitNPC()
    talkNpc(-20000)
    delay(300)
    replyNpcAndChangeMap(0)
end

function bank() return {{map = "192415750", npcBank = true}} end

function printOnce(message)
    if not printedMessages[message] then
        printMessage("[INFO] " .. message, "green")
        printedMessages[message] = true
    end
end

function checkMapId()
    mapId = getMapId()
    printOnce(mapId)

    --429 - Gelée Citron
    --55  - Gelée Bleue
    --56  - Gelée Menthe
    --57  - Gelée Fraise

    if mapId == 98566657 then
        printOnce("Salle 1 détectée")
        FORCE_MONSTERS = {429} --citron
        FORBIDDEN_MONSTERS = {55, 56, 57}
        delay(500)

    elseif mapId == 98567681 then
        printOnce("Salle 2 détectée")
        FORCE_MONSTERS = {57, 55}
        FORBIDDEN_MONSTERS = {56, 429}
        delay(500)

    elseif mapId == 98566659 then
        printOnce("Salle 3 détectée")
        FORCE_MONSTERS = {56, 55, 57}
        FORBIDDEN_MONSTERS = {429}
        delay(500)

    elseif mapId == 98567683 then
        printOnce("Salle 4 détectée")
        --FORCE_MONSTERS = {55, 429, 57} n'est pas possible pour l'instant
        --FORBIDDEN_MONSTERS = {56}
        FORCE_MONSTERS = {}
        FORBIDDEN_MONSTERS = {}
        delay(500)

    elseif mapId == 98568705 then
        printOnce("Sortie détectée")
        goUseInteractive(268)
        delay(500)

    elseif mapId == 88084997 then
        return {
            {map = "8,29", path = "bottom", custom = interactWithEntryNPC},
        }

    else
        print("Carte inconnue : " .. tostring(mapId))
    end
end
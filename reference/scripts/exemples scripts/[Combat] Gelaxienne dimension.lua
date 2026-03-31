--Créé par MaxxDuBedo et corrigé par Flag pour Jitsuri bot

MIN_MONSTERS = 1
MAX_MONSTERS = 8
FIGHT_LOCK_SPEC = true
FIGHT_LOCK_JOIN = true

MAX_PODS = 99
local printedMessages = {}

function move()
checkMapId()
    return {
        {map = "11,29", custom = LSD}, --map de la péninsule des gelées selctionnée arbitrairement pour utiliser le multigely
        
        -- Salles du donjon
        {map = "98566657", fight = true},
        {map = "98567681", fight = true},
        {map = "98566659", fight = true},
        {map = "98567683", fight = true},

        {map = "98568707", fight = true, custom = Combat4gelees}, -- Salle du boss, on parle a une gelée
        
        {map = "98567687", fight = true}, -- Combat des 4 boss
}
end

function LSD()
    printOnce("Mieux vaut tous les manger !")
    useInventoryItem(996, 1)
    delay(1000)
end

function Combat4gelees()
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
        FORCE_MONSTERS = {55} --bleue
        FORBIDDEN_MONSTERS = {429, 56, 57}
        delay(500)

    elseif mapId == 98567681 then
        printOnce("Salle 2 détectée")
        FORCE_MONSTERS = {} --choisi citron après ban fraise
        FORBIDDEN_MONSTERS = {57}
        delay(500)

    elseif mapId == 98566659 then
        printOnce("Salle 3 détectée")
        FORCE_MONSTERS = {56} --vert
        FORBIDDEN_MONSTERS = {429}
        delay(500)

    elseif mapId == 98567683 then
        printOnce("Salle 4 détectée")
        FORCE_MONSTERS = {} --rouge cherche pas mec, ça marche
        FORBIDDEN_MONSTERS = {}
        delay(500)

    elseif mapId == 98568705 then
        printOnce("Redescente")
        goUseInteractive(268)
        delay(500)
        checkMapId()

    elseif mapId == 88084997 then
    useInventoryItem(996, 1)
    delay(1000)
    checkMapId()
     
    else
        print("Carte inconnue : " .. tostring(mapId))
  
end
end
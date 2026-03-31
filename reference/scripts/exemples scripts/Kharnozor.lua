MIN_MONSTERS = 1
MAX_MONSTERS = 8
FIGHT_LOCK_SPEC = true
FIGHT_LOCK_JOIN = true

MAX_PODS = 90
local printedMessages = {}
function move()
    return {
        -- Entrée du repaire du kharnozor
        {map = "-3,25", path = "right", custom = interactWithEntryNPC},
        -- Salles du donjon
        {map = "199491584", fight = true, path = "right"},
        {map = "199492608", fight = true, path = "bottom"},
        {map = "199493632", fight = true, path = "bottom"},
        {map = "199494656", fight = true, path = "right"},
        {map = "199495680", fight = true, path = "top"}, -- Salle du boss
        -- PNJ après le boss pour quitter le donjon
        {map = "199495682", path = "right", custom = interactWithExitNPC}
    }
end

function interactWithEntryNPC()
    delay(300)
    talkNpc(-20000)
    delay(300)
    replyNpc(0)
    delay(300)
    replyNpc(0)
    delay(300)
    replyNpcAndChangeMap(0)
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
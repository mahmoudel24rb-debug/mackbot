MIN_MONSTERS = 1
MAX_MONSTERS = 8
FIGHT_LOCK_SPEC = true
FIGHT_LOCK_JOIN = true

MAX_PODS = 90
local printedMessages = {}
function move()
    return {
        -- devant donjon AA
        {map = "147851777", path = "right", custom = interactWithEntryNPC},



        -- Salles du donjon
        {map = "149684224", fight = true, path = "right"},
        {map = "149685248", fight = true, path = "bottom"},
        {map = "149686272", fight = true, path = "bottom"},
        {map = "149687296", fight = true, path = "right"},
        {map = "149688320", fight = true, path = "top"}, -- Salle du boss
        -- PNJ après le boss pour quitter le donjon
        {map = "149689344", path = "right", custom = interactWithExitNPC}



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
    talkNpc(-20003)
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
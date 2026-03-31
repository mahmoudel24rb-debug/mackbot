
MIN_MONSTERS = 1
MAX_MONSTERS = 8
MAX_PODS = 90
FIGHT_LOCK_SPEC = true
FIGHT_LOCK_JOIN = true

local printedMessages = {}
local adjacentes = {}
startPos = ''

function move()
    if startPos == '' then
        startPos = getCurrentPos()
        local xStr, yStr = startPos:match("([^,]+),([^,]+)")
        local x = tonumber(xStr)
        local y = tonumber(yStr)

        printOnce("x = " .. x)
        printOnce("y = " .. y)

        for dx = -1, 1 do
            for dy = -1, 1 do
                local newX = x + dx
                local newY = y + dy
                local coordStr = string.format("%d,%d", newX, newY)
                table.insert(adjacentes, coordStr)
            end
        end
    end

    local currentPos = getCurrentPos()
    local cxStr, cyStr = currentPos:match("([^,]+),([^,]+)")
    local cx = tonumber(cxStr)
    local cy = tonumber(cyStr)

    local voisinsPossibles = {}

    for _, coord in ipairs(adjacentes) do
        local xStr, yStr = coord:match("([^,]+),([^,]+)")
        local x = tonumber(xStr)
        local y = tonumber(yStr)

        local dx = math.abs(x - cx)
        local dy = math.abs(y - cy)

        local isLigneDroite = (dx == 1 and dy == 0) or (dx == 0 and dy == 1)
        local isNotPrevious = (coord ~= previousPos)

        if isLigneDroite and isNotPrevious then
            table.insert(voisinsPossibles, coord)
        end
    end

    if #voisinsPossibles == 0 then
        printOnce("Aucune case valide (ligne droite sans retour) depuis " .. currentPos)
        return {}
    end

    math.randomseed(os.time())
    local randomIndex = math.random(1, #voisinsPossibles)
    local destination = voisinsPossibles[randomIndex]

    printOnce("Depuis " .. currentPos .. " vers " .. destination)

    previousPos = currentPos  -- mise à jour de la case précédente

    return {
        { map = currentPos, fight = true, path = destination },
    }
end



function bank() return {{map = "192415750", npcBank = true}} end

function printOnce(message)
    if not printedMessages[message] then
        printMessage("[INFO] " .. message, "green")
        printedMessages[message] = true
    end
end

function phenix()
    return {
    }
end

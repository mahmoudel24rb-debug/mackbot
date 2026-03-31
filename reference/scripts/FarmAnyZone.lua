
MIN_MONSTERS = 1
MAX_MONSTERS = 8
MAX_PODS = 90 -- 90% de pods
FIGHT_LOCK_SPEC = true
FIGHT_LOCK_JOIN = true

local printedMessages = {}
-- Tableau pour stocker les positions adjacentes
local adjacentes = {}
startPos = ''

function move()
    

    if startPos == '' then
        startPos = getCurrentPos()
        local xStr, yStr =  startPos:match("([^,]+),([^,]+)")
        local x = tonumber(xStr)
        local y = tonumber(yStr)
        
        printOnce("x =".. x)
        printOnce("y =".. y)


        -- Génération des 9 coordonnées (de x-1 à x+1 et y-1 à y+1)
        for dx = -1, 1 do
            for dy = -1, 1 do
                local newX = x + dx
                local newY = y + dy
                local coordStr = string.format("%d,%d", newX, newY)
                printOnce("Insertion :".. coordStr) -- <- vérifie si ça s'affiche
                table.insert(adjacentes, coordStr)
            end
        end
    end

    -- Tirer une coordonnée au hasard
    math.randomseed(os.time())  -- pour rendre le random plus aléatoire
    local randomIndex = math.random(1, #adjacentes)
    printOnce("randomIndex = ".. randomIndex)

    local coordAleatoire = adjacentes[randomIndex]
    printOnce("nouvelle destination = ".. coordAleatoire)

    currentPos = getCurrentPos()

    return {
        { map = currentPos, fight = true, path = coordAleatoire }, 
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

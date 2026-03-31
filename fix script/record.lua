-- ==================================================
-- RECORDER JITSURI
-- ==================================================

local FILE_PATH = "MonTrajet.txt"
local last_map_id = 0

-- Fonction simple pour ne rien faire
function neRienFaire()
   -- On attend un peu pour ne pas spammer le CPU
   delay(500) 
end

function move()
    -- 1. Récupération des infos avec les fonctions Jitsuri
    local current_id = getMapId()
    local current_pos = getCurrentPos()
    
    -- 2. Logique d'enregistrement
    if current_id ~= last_map_id then
        
        -- On essaie d'écrire dans le fichier
        local file = io.open(FILE_PATH, "a")
        if file then
            -- On convertit tout en texte (tostring) pour éviter les bugs
            local line = '{ map = "' .. tostring(current_id) .. '", path = "DIRECTION", gather = true, fight = false, custom = VerificationPassage }, -- ' .. tostring(current_pos) .. '\n'
            
            file:write(line)
            file:close()
            
            -- CORRECTION ICI : Toujours mettre la couleur (2ème argument)
            printMessage("[REC] Map enregistrée : " .. tostring(current_pos) .. " (ID: " .. tostring(current_id) .. ")", "green")
        else
            printMessage("[ERREUR] Impossible d'écrire dans le fichier.", "red")
        end
        
        last_map_id = current_id
    end
    
    -- 3. On retourne une action vide pour la map actuelle
    -- On ne met PAS de "path" pour éviter que le bot essaie de cliquer. 
    -- S'il râle qu'il manque "path", remplace par : path = "top"
    return {
        { 
            map = tostring(current_id), 
            custom = neRienFaire 
        }
    }
end

function bank()
    return {}
end
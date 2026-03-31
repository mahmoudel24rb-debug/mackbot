-- Script récolte : Aquajou, Salikrone, Quisnoa, Écume de mer
-- Zone : 19,26 -> 27,34 (MapID + directions)
-- FIX : map 168821770 (20,31) -> 168820746 (19,31) utilise cell = "336"
--       car cell 252 (auto) est bloqué sur cette map

MAX_PODS = 90
ELEMENTS_TO_GATHER = {287,288,289,291}

function move()
        return {
        -- Départ : 23,26
        { map = "169083904", path = "left", gather = true, fight = false },      -- 23,26 -> 22,26
        { map = "169082880", path = "bottom", gather = true, fight = false },    -- 22,26 -> 22,27
        { map = "169082882", path = "left", gather = true, fight = false },      -- 22,27 -> 21,27
        { map = "168822786", path = "top", gather = true, fight = false },       -- 21,27 -> 21,26
        { map = "168822784", path = "left", gather = true, fight = false },      -- 21,26 -> 20,26
        { map = "168821760", path = "bottom", gather = true, fight = false },    -- 20,26 -> 20,27
        { map = "168821762", path = "left", gather = true, fight = false },      -- 20,27 -> 19,27
        { map = "168820738", path = "bottom", gather = true, fight = false },    -- 19,27 -> 19,28
        { map = "168820740", path = "right", gather = true, fight = false },     -- 19,28 -> 20,28
        { map = "168821764", path = "right", gather = true, fight = false },     -- 20,28 -> 21,28
        { map = "168822788", path = "top", gather = true, fight = false },       -- 21,28 -> 21,27
        { map = "168822786", path = "right", gather = true, fight = false },     -- 21,27 -> 22,27
        { map = "169082882", path = "right", gather = true, fight = false },     -- 22,27 -> 23,27
        { map = "169083906", path = "right", gather = true, fight = false },     -- 23,27 -> 24,27
        { map = "169084930", path = "bottom", gather = true, fight = false },    -- 24,27 -> 24,28
        { map = "169084932", path = "top", gather = true, fight = false },       -- 24,28 -> 24,27
        { map = "169084930", path = "top", gather = true, fight = false },       -- 24,27 -> 24,26
        { map = "169084928", path = "left", gather = true, fight = false },      -- 24,26 -> 23,26
        { map = "169083904", path = "right", gather = true, fight = false },     -- 23,26 -> 24,26
        { map = "169084928", path = "right", gather = true, fight = false },     -- 24,26 -> 25,26
        { map = "168558592", path = "right", gather = true, fight = false },     -- 25,26 -> 26,26
        { map = "168559616", path = "left", gather = true, fight = false },      -- 26,26 -> 25,26
        { map = "168558592", path = "bottom", gather = true, fight = false },    -- 25,26 -> 25,27
        { map = "168558594", path = "right", gather = true, fight = false },     -- 25,27 -> 26,27
        { map = "168559618", path = "right", gather = true, fight = false },     -- 26,27 -> 27,27
        { map = "168560642", path = "bottom", gather = true, fight = false },    -- 27,27 -> 27,28
        { map = "168560644", path = "left", gather = true, fight = false },      -- 27,28 -> 26,28
        { map = "168559620", path = "bottom", gather = true, fight = false },    -- 26,28 -> 26,29
        { map = "168559622", path = "left", gather = true, fight = false },      -- 26,29 -> 25,29
        { map = "168558598", path = "bottom", gather = true, fight = false },    -- 25,29 -> 25,30
        { map = "168558600", path = "right", gather = true, fight = false },     -- 25,30 -> 26,30
        { map = "168559624", path = "right", gather = true, fight = false },     -- 26,30 -> 27,30
        { map = "168560648", path = "top", gather = true, fight = false },       -- 27,30 -> 27,29
        { map = "168560646", path = "bottom", gather = true, fight = false },    -- 27,29 -> 27,30
        { map = "168560648", path = "bottom", gather = true, fight = false },    -- 27,30 -> 27,31
        { map = "168560650", path = "left", gather = true, fight = false },      -- 27,31 -> 26,31
        { map = "168559626", path = "left", gather = true, fight = false },      -- 26,31 -> 25,31
        { map = "168558602", path = "bottom", gather = true, fight = false },    -- 25,31 -> 25,32
        { map = "168558604", path = "right", gather = true, fight = false },     -- 25,32 -> 26,32
        { map = "168559628", path = "right", gather = true, fight = false },     -- 26,32 -> 27,32
        { map = "168560652", path = "bottom", gather = true, fight = false },    -- 27,32 -> 27,33
        { map = "168560654", path = "left", gather = true, fight = false },      -- 27,33 -> 26,33
        { map = "168559630", path = "bottom", gather = true, fight = false },    -- 26,33 -> 26,34
        { map = "168559632", path = "right", gather = true, fight = false },     -- 26,34 -> 27,34
        { map = "168560656", path = "left", gather = true, fight = false },      -- 27,34 -> 26,34
        { map = "168559632", path = "left", gather = true, fight = false },      -- 26,34 -> 25,34
        { map = "168558608", path = "top", gather = true, fight = false },       -- 25,34 -> 25,33
        { map = "168558606", path = "left", gather = true, fight = false },      -- 25,33 -> 24,33
        { map = "169084942", path = "bottom", gather = true, fight = false },    -- 24,33 -> 24,34
        { map = "169084944", path = "left", gather = true, fight = false },      -- 24,34 -> 23,34
        { map = "169083920", path = "left", gather = true, fight = false },      -- 23,34 -> 22,34
        { map = "169082896", path = "top", gather = true, fight = false },       -- 22,34 -> 22,33
        { map = "169082894", path = "right", gather = true, fight = false },     -- 22,33 -> 23,33
        { map = "169083918", path = "top", gather = true, fight = false },       -- 23,33 -> 23,32
        { map = "169083916", path = "left", gather = true, fight = false },      -- 23,32 -> 22,32
        { map = "169082892", path = "top", gather = true, fight = false },       -- 22,32 -> 22,31
        { map = "169082890", path = "right", gather = true, fight = false },     -- 22,31 -> 23,31
        { map = "169083914", path = "right", gather = true, fight = false },     -- 23,31 -> 24,31
        { map = "169084938", path = "top", gather = true, fight = false },       -- 24,31 -> 24,30
        { map = "169084936", path = "top", gather = true, fight = false },       -- 24,30 -> 24,29
        { map = "169084934", path = "left", gather = true, fight = false },      -- 24,29 -> 23,29
        { map = "169083910", path = "bottom", gather = true, fight = false },    -- 23,29 -> 23,30
        { map = "169083912", path = "left", gather = true, fight = false },      -- 23,30 -> 22,30
        { map = "169082888", path = "left", gather = true, fight = false },      -- 22,30 -> 21,30
        { map = "168822792", path = "bottom", gather = true, fight = false },    -- 21,30 -> 21,31
        { map = "168822794", path = "top", gather = true, fight = false },       -- 21,31 -> 21,30
        { map = "168822792", path = "top", gather = true, fight = false },       -- 21,30 -> 21,29
        { map = "168822790", path = "left", gather = true, fight = false },      -- 21,29 -> 20,29
        { map = "168821766", path = "left", gather = true, fight = false },      -- 20,29 -> 19,29
        { map = "168820742", path = "bottom", gather = true, fight = false },    -- 19,29 -> 19,30
        { map = "168820744", path = "right", gather = true, fight = false },     -- 19,30 -> 20,30
        { map = "168821768", path = "bottom", gather = true, fight = false },    -- 20,30 -> 20,31
        -- FIX: cell 252 auto est bloqué sur cette map, on force cell 336 (bordure gauche)
        { map = "168821770", path = "left", gather = true, fight = false, cell = "336" }, -- 20,31 -> 19,31
        { map = "168820746", path = "right", gather = true, fight = false },     -- 19,31 -> 20,31
        { map = "168821770", path = "bottom", gather = true, fight = false },    -- 20,31 -> 20,32
        { map = "168821772", path = "left", gather = true, fight = false },      -- 20,32 -> 19,32
        { map = "168820748", path = "bottom", gather = true, fight = false },    -- 19,32 -> 19,33
        { map = "168820750", path = "right", gather = true, fight = false },     -- 19,33 -> 20,33
        { map = "168821774", path = "right", gather = true, fight = false },     -- 20,33 -> 21,33
        { map = "168822798", path = "bottom", gather = true, fight = false },    -- 21,33 -> 21,34
        { map = "168822800", path = "left", gather = true, fight = false },      -- 21,34 -> 20,34
        { map = "168821776", path = "left", gather = true, fight = false },      -- 20,34 -> 19,34
        { map = "168820752", path = "right", gather = true, fight = false },     -- 19,34 -> 20,34
        { map = "168821776", path = "top", gather = true, fight = false },       -- 20,34 -> 20,33
        { map = "168821774", path = "right", gather = true, fight = false },     -- 20,33 -> 21,33
        { map = "168822798", path = "top", gather = true, fight = false },       -- 21,33 -> 21,32
        -- Retour via building 21,32 -> 21,26 puis walk vers 23,26
        { map = "168822796", path = "21,26", gather = true, fight = false },     -- 21,32 -> 21,26 (building)
        { map = "168822784", path = "bottom", gather = true, fight = false },    -- 21,26 -> 21,27
        { map = "168822786", path = "right", gather = true, fight = false },     -- 21,27 -> 22,27
        { map = "169082882", path = "top", gather = true, fight = false },       -- 22,27 -> 22,26
        { map = "169082880", path = "right", gather = true, fight = false },     -- 22,26 -> 23,26
        }
end

function bank()
        return {
        { map = "23,24", npcBank = true },
        }
end

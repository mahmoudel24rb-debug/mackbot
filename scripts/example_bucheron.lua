-- Example farming script — Wood cutter (Frêne/Chêne)
-- Compatible with Jitsuri script format
-- Edit the map coordinates below to match your farming route

ELEMENTS_TO_GATHER = {114, 115}   -- Frêne (114), Chêne (115)
MAX_PODS = 90                      -- Bank at 90% pods

function move()
    return {
        -- Example route: Astrub forest area
        -- Replace these coords with your actual farming route
        { map = "4,-19", path = "4,-18", gather = true, fight = false },
        { map = "4,-18", path = "5,-18", gather = true, fight = false },
        { map = "5,-18", path = "5,-19", gather = true, fight = false },
        { map = "5,-19", path = "4,-19", gather = true, fight = false },
    }
end

function bank()
    return {
        -- { map = "4,-17", npcBank = true }
    }
end

function phenix()
    return {}
end

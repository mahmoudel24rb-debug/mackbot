-- Example farming script — Wood cutter (Frêne/Chêne)
-- Compatible with Jitsuri script format

ELEMENTS_TO_GATHER = {114, 115}   -- Frêne (114), Chêne (115)
MAX_PODS = 90                      -- Bank at 90% pods

function move()
    return {
        -- Fill in with your map IDs and directions
        -- { map = "169083904", path = "right", gather = true,  fight = false },
        -- { map = "169082880", path = "bottom", gather = true, fight = false },
    }
end

function bank()
    return {
        -- { map = "192415750", npcBank = true }
    }
end

function phenix()
    return {}
end

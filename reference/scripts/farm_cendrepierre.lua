-- =============================================
-- FARM CENDREPIERRE - Galerie d'Ereboria
-- =============================================
-- 20 cendrepierres sur ~12 maps
-- Place toi dans la mine avant de lancer le script
-- Le bot boucle en continu dans la galerie
-- =============================================

ELEMENTS_TO_GATHER = {400}  -- Cendrepierre
MAX_PODS = 95

function move()
    return {
        -- === DEPART : (33,-70) → direction sud ===
        { map = "223085059", path = "223216131", gather = true, fight = false },   -- (33,-70)
        { map = "223216131", path = "223216129", gather = true, fight = false },   -- (32,-70)
        { map = "223216129", path = "223216385", gather = true, fight = false },   -- (32,-71)
        { map = "223216385", path = "223085313", gather = true, fight = false },   -- (32,-72)
        { map = "223085313", path = "223086337", gather = true, fight = false },   -- (33,-72)
        { map = "223086337", path = "223086086", gather = true, fight = false },   -- (34,-72) → escalier vers (34,-69)

        -- === REMONTEE EST : (34,-69) → boucle nord-est ===
        { map = "223086086", path = "223086088", gather = true, fight = false },   -- (34,-69)
        { map = "223086088", path = "223087112", gather = true, fight = false },   -- (34,-68) → est
        { map = "223087112", path = "223087114", gather = true, fight = false },   -- (35,-68)
        { map = "223087114", path = "223086090", gather = true, fight = false },   -- (35,-67)
        { map = "223086090", path = "223086088", gather = true, fight = false },   -- (34,-67) → retour (34,-68)

        -- === (34,-68) 2eme passage → direction ouest ===
        { map = "223086088", path = "223085064", gather = true, fight = false },   -- (34,-68) → ouest
        { map = "223085064", path = "223216136", gather = true, fight = false },   -- (33,-68)
        { map = "223216136", path = "223216134", gather = true, fight = false },   -- (32,-68)

        -- === BRANCHE (32,-69) / (33,-69) ===
        { map = "223216134", path = "223085062", gather = true, fight = false },   -- (32,-69)
        { map = "223085062", path = "223216134", gather = true, fight = false },   -- (33,-69) → retour (32,-69)
        { map = "223216134", path = "223216136", gather = true, fight = false },   -- (32,-69) → retour (32,-68)

        -- === DIRECTION OUEST → escalier → retour start ===
        { map = "223216136", path = "223217160", gather = true, fight = false },   -- (32,-68) → ouest
        { map = "223217160", path = "223217153", gather = true, fight = false },   -- (31,-68) → escalier vers (31,-71)
        { map = "223217153", path = "223216129", gather = true, fight = false },   -- (31,-71) → est
        { map = "223216129", path = "223216131", gather = true, fight = false },   -- (32,-71) → nord
        { map = "223216131", path = "223085059", gather = true, fight = false },   -- (32,-70) → retour start
        -- BOUCLE : retour a map 1
    }
end

function bank()
    return {}
end

function phenix()
    return {}
end

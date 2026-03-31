--------------------------------
-- DJ BLOP avec gestion de phase 
--------------------------------

-- PHASE/CONFIG
phase = "donjon"           -- "donjon" / "bossindigo" / "bossgriotte" / "bossreinette" / "bosscoco" / "bossmulti"
bossColor = "indigo"       -- "indigo" | "griotte" | "reinette" | "coco" 
multi = false              -- true = enchaîne Multicolore après un boss couleur / false = sortie
MAX_PODS = 90

-- Npc id de base (1er PNJ sur la map)
BOSS_NPC_ID = -20000
POST_BOSS_NPC_ID = -20000

--------------------------------
--  Décision après un boss couleur
--------------------------------
function postBoss()
    if multi then
        printMessage("[Blop] Multicolore activé -> phase = bossmulti", "yellow")
        phase = "bossmulti"
        talkNpc(POST_BOSS_NPC_ID)
        delay(400)
        replyNpc(0) -- 1ère réponse = aller au Multicolore 
        delay(600)
    else
        printMessage("[Blop] Multicolore désactivé -> retour à l'entrée du donjon", "yellow")
        talkNpc(POST_BOSS_NPC_ID)
        delay(400)
        replyNpc(1) -- 2e réponse = quitter 
        delay(600)
        phase = "donjon"
    end
end

--------------------------------
--  Phase Multicolore
--------------------------------
function bossMulti()
    printMessage("[Blop] Phase Multicolore lancée", "purple")
    return {
        
        { map = "mapid_debut_multicolor", fight = true },
        

        { map = "7,-43", custom = endMultiPhase },
    }
end

function endMultiPhase()
    printMessage("[Blop] Boss Multicolore terminé → retour en phase donjon", "green")
            delay(600)

    phase = "donjon"
end

--------------------------------
-- Dispatch principal
--------------------------------
function move()
    if phase == "donjon" then
        return cheminDonjon()
    elseif phase == "bossindigo" then
        return cheminBossIndigo()
    elseif phase == "bossgriotte" then
        return cheminBossGriotte()
    elseif phase == "bossreinette" then
        return cheminBossReinette()
    elseif phase == "bosscoco" then
        return cheminBossCoco()
    elseif phase == "bossmulti" then
        return bossMulti()
    else
        printMessage("[Blop] Phase inconnue : " .. tostring(phase), "red")
        stopScript()
    end
end

--------------------------------
-- Chemin : partie donjon
--------------------------------
function cheminDonjon()
    return {
        -- entrée du donjon 
        { map = "166985728", custom = interactWithEntryNPC },
        { map = "166986752", fight = true },
        { map = "166987776", fight = true },
        { map = "166988800", fight = true },
        { map = "166989824", fight = true },
        -- salle PNJ du choix de couleur
        { map = "166990848", custom = chooseBossRoom  },
    }
end

--------------------------------
-- Chemins : boss des 4 couleurs
--------------------------------
function cheminBossIndigo()
    return {
        { map = "166985730", fight = true },
        { map = "152835072", custom = postBoss },
         { map = "166990848", fight = true },
        { map = "166989826", custom = postBoss },
    }
end

function cheminBossGriotte()
    return {
       { map = "166985730", fight = true },
        { map = "152835072", custom = postBoss },
         { map = "166990848", fight = true },
        { map = "166989826", custom = postBoss },
    }
end


function cheminBossReinette()
    return {
        { map = "166985730", fight = true },
        { map = "152835072", custom = postBoss },
         { map = "166990848", fight = true },
        { map = "166989826", custom = postBoss },
    }
end

function cheminBossCoco()
    return {
        { map = "166985730", fight = true },
        { map = "152835072", custom = postBoss },
         { map = "166990848", fight = true },
        { map = "166989826", custom = postBoss },
    }
end

--------------------------------
-- PNJ : choix de la couleur 
-- Ordre PNJ: 1 indigo, 2 griotte, 3 reinette, 4 coco
--------------------------------
function chooseBossRoom()
    local raw = tostring(bossColor or "reinette")
    local color = string.lower(raw)
    -- Si erreur --> reinette par default
    color = color:gsub("^boss", "")

    printMessage("[Blop] Choix de couleur -> " .. color, "yellow")

    local colorReply = {
        indigo   = 0, -- 1ère réponse
        griotte  = 1, -- 2ème
        reinette = 2, -- 3ème 
        coco     = 3, -- 4ème
    }

    local replyIndex = colorReply[color]
    if replyIndex == nil then
        printMessage("[Blop] Couleur inconnue (" .. raw .. "), utilisation de 'reinette' par défaut.", "red")
        color = "reinette"
        replyIndex = colorReply[color]
    end

    
    talkNpc(BOSS_NPC_ID)
    
    delay(800)
    replyNpc(0)
    delay(200)
    replyNpc(0)

    delay(200)

    
    delay(1000)
    phase = "boss" .. color
    printMessage("[Blop] Passage en phase : " .. phase, "green")
    
end

--------------------------------
-- PNJ entrée / sortie
--------------------------------
function interactWithEntryNPC()
    delay(600)
    talkNpc(-20000)
    delay(400)
    replyNpc(0)
    delay(400)
    replyNpc(0)
    delay(400)
    replyNpcAndChangeMap(0)
end

function interactWithExitNPC()
    talkNpc(-20000)
    delay(400)
    replyNpcAndChangeMap(0)
end

--------------------------------
-- Banque
--------------------------------
function bank()
    return { { map = "192415750", npcBank = true } }
end

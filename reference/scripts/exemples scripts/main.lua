-- ===============================================================================
-- 🏰 PHOSSILE SCRIPT - MAIN LOADER
-- Version 3.10 - Système modulaire en 6 parties
-- ===============================================================================

-- ===============================================================================
-- 📁 CHARGEMENT DES MODULES
-- ===============================================================================

console.print("🏰 Démarrage script Donjon Phossile v3.10 (SYSTÈME MODULAIRE)", console.blue)

-- Chargement des modules avec gestion d'erreur
local function loadModule(moduleName)
    local success, result = pcall(function()
        return dofile(moduleName .. ".lua")
    end)
    
    if success then
        console.print("✅ Module " .. moduleName .. " chargé avec succès", console.green)
        return result
    else
        console.print("❌ ERREUR chargement module " .. moduleName .. ": " .. tostring(result), console.red)
        return nil
    end
end

-- Ordre de chargement des modules (IMPORTANT)
console.print("📦 === CHARGEMENT MODULES ===", console.blue)

-- 1. Configuration et constantes (premier)
loadModule("config")

-- 2. Utilitaires et API de base
loadModule("utils")

-- 3. Stratégies de combat
loadModule("combat_strategies")

-- 4. Stratégie post-Phossile et placement
loadModule("post_phossile")

-- 5. Système de combat principal (dernier)
loadModule("fight_core")

console.print("✅ Tous les modules chargés", console.green)

-- ===============================================================================
-- 🚀 FONCTION PRINCIPALE OBLIGATOIRE DOFUS
-- ===============================================================================

function move()
    local path = {}
    
    -- Map d'entrée
    path[#path+1] = {
        map = tostring(mapEntreeDonjon),
        custom = function()
            console.print("🚪 Entrée donjon - Récupération puis NPC", console.yellow)
            
            -- EXÉCUTER CODE.LUA AVANT INTERACTION NPC
            console.print("🍞 === EXÉCUTION CODE.LUA AVANT NPC D'ENTRÉE ===", console.blue)
            
            -- Récupération automatique avant entrée donjon
            local recoverySuccess = performInitialRecovery()
            
            if recoverySuccess then
                console.print("✅ Récupération pré-donjon réussie", console.green)
            else
                console.print("⚠️ Récupération pré-donjon échouée", console.orange)
            end
            
            -- Montée des caractéristiques selon settings.lua
            console.print("📊 === MONTÉE DES CARACTÉRISTIQUES ===", console.blue)
            upStats()
            console.print("✅ Caractéristiques mises à jour", console.green)
            
            -- Équipement automatique selon settings.lua  
            console.print("⚔️ === ÉQUIPEMENT AUTOMATIQUE ===", console.blue)
            stuff()
            console.print("✅ Équipement optimisé", console.green)
            
            -- Délai pour s'assurer que les objets sont bien consommés
            global.sleep(500)
            
            -- Maintenant parler au NPC d'entrée
            console.print("🚪 Dialogue avec NPC d'entrée...", console.yellow)
            
            if npc.exists(npcId) then
                npc.talk(npcId)
                global.sleep(0)
                npc.reply(-1)
                global.sleep(0)
                npc.reply(-1)
                global.sleep(0)
            end
            
            return true
        end
    }
    
    -- Maps de combat
    for i, mapId in ipairs(fightMaps) do
        path[#path+1] = {
            map = tostring(mapId),
            custom = function()
                console.print("⚔️ Salle " .. i .. " - Combat", console.red)
                
                map.fight(1)
                
                return true
            end
        }
    end
    
    -- Map de sortie
    path[#path+1] = {
        map = tostring(mapExitDonjon),
        custom = function()
            console.print("🚪 Sortie donjon - Parler au NPC", console.yellow)
            
            if npc.exists(npcExitId) then
                npc.talk(npcExitId)
                global.sleep(0)
                npc.reply(-1)
                global.sleep(0)
                npc.reply(-1)
                global.sleep(0)
            end
            
            return true
        end
    }
    
    -- Retour à la map d'entrée donjon
    path[#path+1] = { map = "129238529", door = 275 } -- [-2,-3]
    
    return path
end

-- ===============================================================================
-- 🏦 FONCTIONS OBLIGATOIRES DOFUS (même si vides)
-- ===============================================================================

function bank()
    return {}
end

function phenix()
    return {}
end

-- ===============================================================================
-- 🔥 INITIALISATION DU COMBAT
-- ===============================================================================

-- Callback de début de combat
function map_fight_start()
    console.print("🔥 Combat démarré !", console.red)
    initAdvancedAI()
end

console.print("🎯 === PHOSSILE SCRIPT MODULAIRE PRÊT ===", console.green)
console.print("📋 Modules chargés: config + utils + combat_strategies + post_phossile + fight_core", console.cyan)

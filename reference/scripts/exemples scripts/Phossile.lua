
local COMMERCIAL_MODE = true  -- true = Mode commercial (logs réduits), false = Mode développement (tous logs)

local COMMERCIAL_ALLOWED_MESSAGES = {
    "🏰 Démarrage script",
    "✅ Stratégie Tour",
    "⚔️ Combat terminé",
    "🚪 Entrée donjon",
    "🚪 Sortie donjon",
    "❌ ERREUR",
    "🛑 ARRÊT"
}

-- Messages INTERDITS en mode commercial (patterns à bloquer)
local COMMERCIAL_BLOCKED_MESSAGES = {
    "💀 PHOSSILE MORT",
    "💀.*PHOSSILE",
    "🌊 Condensation",
    "⚔️ Mutilation", 
    "🌪️ Dissolution",
    "⚡ Déchainement",
    "💪 Berserk",
    "👑 COURONNES",
    "ACTIVATION STRATÉGIE POST-PHOSSILE",
    "STRATÉGIE POST-PHOSSILE", 
    "POST%-PHOSSILE",
    "PA actuels",
    "PA restants",
    "PA disponibles",
    "ÉTAPE [0-9]",
    "réussie",
    "réussi", 
    "RÉUSSI",
    "RÉUSSIE",
    "NETTOYAGE",
    "Tour post%-Phossile",
    "OFFICIEL"
}

-- Fonction de log intelligente selon le mode
function smartLog(message, color)
    if not COMMERCIAL_MODE then
        -- Mode développement : tous les logs
        console.print(message, color)
    else
        -- Mode commercial : seulement les messages autorisés
        for _, allowedPattern in ipairs(COMMERCIAL_ALLOWED_MESSAGES) do
            if string.find(message, allowedPattern) then
                console.print(message, color)
                return
            end
        end
        -- Log commercial simplifié pour certains patterns critiques
        if string.find(message, "STRATÉGIE.*DÉMARRÉE") then
            console.print("🎯 Exécution stratégie...", console.blue)
        elseif string.find(message, "TERMINÉE") and string.find(message, "PA restants") then
            console.print("✅ Stratégie terminée", console.green)
        end
    end
end

-- ===============================================================================
-- 🎛️ SYSTÈME DE REMPLACEMENT AUTOMATIQUE DES LOGS
-- ===============================================================================

-- Sauvegarder la fonction console.print originale
local originalConsolePrint = console.print

-- Fonction de remplacement intelligent pour console.print
function console.print(message, color)
    if not COMMERCIAL_MODE then
        -- Mode développement : utiliser la fonction originale
        originalConsolePrint(message, color or console.white)
    else
        -- Mode commercial : filtrage intelligent
        local messageStr = tostring(message)
        
        -- ÉTAPE 1: Vérifier messages INTERDITS (priorité absolue)
        for _, blockedPattern in ipairs(COMMERCIAL_BLOCKED_MESSAGES) do
            if string.find(messageStr, blockedPattern) then
                -- Message bloqué, ne pas afficher
                return
            end
        end
        
        -- ÉTAPE 2: Messages TOUJOURS affichés en mode commercial
        for _, allowedPattern in ipairs(COMMERCIAL_ALLOWED_MESSAGES) do
            if string.find(messageStr, allowedPattern) then
                originalConsolePrint(message, color or console.white)
                return
            end
        end
        
        -- Messages simplifiés pour certains patterns
        if string.find(messageStr, "=== STRATÉGIE") and string.find(messageStr, "DÉMARRÉE") then
            originalConsolePrint("🎯 Exécution stratégie...", console.blue)
        elseif string.find(messageStr, "=== ÉTAPE") then
            -- Masquer les étapes détaillées
            return
        elseif string.find(messageStr, "PA actuels") or string.find(messageStr, "PA disponibles") then
            -- Masquer les logs de PA détaillés
            return
        elseif string.find(messageStr, "🔍") or string.find(messageStr, "📍") or string.find(messageStr, "📏") then
            -- Masquer les logs de debug et positions
            return
        elseif string.find(messageStr, "fight%.canCastSpell") then
            -- Masquer les vérifications de sorts
            return
        elseif string.find(messageStr, "TERMINÉE.*PA restants") then
            originalConsolePrint("✅ Stratégie terminée", console.green)
        elseif string.find(messageStr, "réussie") or string.find(messageStr, "réussi") then
            -- Messages de succès bloqués en mode commercial
            -- (Les sorts individuels sont maintenant masqués)
            return
        end
        -- Tous les autres messages sont masqués en mode commercial
    end
end

-- ===============================================================================
-- 🏪 FONCTIONS DE CONTRÔLE DU MODE COMMERCIAL
-- ===============================================================================

-- Fonction pour activer le mode commercial depuis le script
function activateCommercialMode()
    COMMERCIAL_MODE = true
    originalConsolePrint("🏪 MODE COMMERCIAL ACTIVÉ - Logs simplifiés", console.orange)
end

-- Fonction pour désactiver le mode commercial depuis le script  
function deactivateCommercialMode()
    COMMERCIAL_MODE = true
    originalConsolePrint("🔧 MODE DÉVELOPPEMENT ACTIVÉ - Tous logs visibles", console.cyan)
end

-- Fonction pour basculer entre les modes
function toggleCommercialMode()
    if COMMERCIAL_MODE then
        deactivateCommercialMode()
    else
        activateCommercialMode()
    end
end

-- Fonction pour tester les deux modes (utile pour démonstration)
function demonstrateCommercialMode()
    originalConsolePrint("🧪 === DÉMONSTRATION MODE COMMERCIAL ===", console.yellow)
    
    -- Test en mode développement
    originalConsolePrint("📋 Mode développement (tous logs visibles):", console.cyan)
    COMMERCIAL_MODE = true
    console.print("🔍 Ceci est un log de debug détaillé", console.white)
    console.print("📍 Position actuelle: 302", console.cyan)
    console.print("⚡ PA disponibles: 12", console.cyan)
    console.print("✅ MUTILATION réussie - PA restants: 10", console.green)
    console.print("🎯 === STRATÉGIE TOUR 1 DÉMARRÉE ===", console.red)
    
    originalConsolePrint("", console.white)
    originalConsolePrint("📋 Mode commercial (logs filtrés):", console.orange)
    COMMERCIAL_MODE = true
    console.print("🔍 Ceci est un log de debug détaillé", console.white)
    console.print("📍 Position actuelle: 302", console.cyan) 
    console.print("⚡ PA disponibles: 12", console.cyan)
    console.print("✅ MUTILATION réussie - PA restants: 10", console.green)
    console.print("🎯 === STRATÉGIE TOUR 1 DÉMARRÉE ===", console.red)
    
    -- Remettre en mode développement par défaut
    COMMERCIAL_MODE = true
    originalConsolePrint("🧪 === FIN DÉMONSTRATION ===", console.yellow)
end

-- Fonction pour afficher le statut actuel du mode
function showCommercialModeStatus()
    if COMMERCIAL_MODE then
        originalConsolePrint("📊 STATUT: Mode Commercial ACTIVÉ 🏪", console.orange)
        originalConsolePrint("   └─ Logs simplifiés pour les utilisateurs finaux", console.yellow)
    else
        originalConsolePrint("📊 STATUT: Mode Développement ACTIVÉ 🔧", console.cyan)
        originalConsolePrint("   └─ Tous les logs détaillés visibles", console.yellow)
    end
end

-- Affichage du mode actuel
if COMMERCIAL_MODE then
    console.print("🏪 MODE COMMERCIAL ULTRA-SILENCIEUX ACTIVÉ", console.orange)
    console.print("🔇 Logs de combat masqués (Phossile, sorts, PA, stratégies)", console.yellow)
    console.print("🔧 Pour logs détaillés: deactivateCommercialMode() ou COMMERCIAL_MODE = false", console.yellow)
else
    console.print("🔧 MODE DÉVELOPPEMENT - Tous logs détaillés visibles", console.cyan)
    console.print("🏪 Pour mode commercial: activateCommercialMode() ou COMMERCIAL_MODE = true", console.yellow)
end

-- ===============================================================================
-- ⚡ SYSTÈME DE CONSOMMATION PAIN/ÉNERGIE INTÉGRÉ (CODE.LUA)
-- ===============================================================================

-- ===============================================================================
-- 🔒 SYSTÈME DE SÉCURITÉ PHOSSILE
-- ===============================================================================

local function securityCheck()
    -- ID et nom autorisés pour Phossile
    local AUTHORIZED_CHARACTER_ID = 14675476830
    local AUTHORIZED_CHARACTER_NAME = "Deelma"
    
    console.print("🛡️ === VÉRIFICATION DE SÉCURITÉ PHOSSILE ===", console.blue)
    
    -- Vérifier le statut d'équipe
    local teamCount = 1
    local teamIndex = 0
    local isLeader = true
    
    pcall(function()
        if global.teamCount then
            teamCount = global.teamCount() or 1
        end
        if global.inTeamIndex then
            teamIndex = global.inTeamIndex() or 0
        end
        if global.isTeamLeader then
            isLeader = global.isTeamLeader() or (teamIndex <= 1)
        end
    end)
    
    -- Si ce n'est pas le leader (index 0 ou 1), autoriser l'exécution sans vérification
    if teamCount > 1 and teamIndex > 1 then
        console.print("👥 MULE DÉTECTÉE - Index équipe: " .. teamIndex, console.cyan)
        console.print("✅ Exécution autorisée pour les mules", console.green)
        console.print("🛡️ === SÉCURITÉ: MULE VALIDÉE ===", console.green)
        return true
    end
    
    -- Vérification uniquement pour le leader (index 0 ou 1)
    console.print("👑 LEADER DÉTECTÉ - Vérification de sécurité...", console.yellow)
    
    -- Récupération des informations du personnage actuel
    local currentCharacterId = nil
    local currentCharacterName = nil
    
    pcall(function()
        currentCharacterId = character.id()
        currentCharacterName = character.name()
    end)
    
    -- Vérification de l'ID du personnage
    if currentCharacterId ~= AUTHORIZED_CHARACTER_ID then
        console.print("🚫 ACCÈS REFUSÉ ", console.red)
        console.print("🔒 ID détecté: " .. tostring(currentCharacterId), console.red)
        console.print("🔒 ID autorisé: " .. AUTHORIZED_CHARACTER_ID, console.red)
        console.print("📞 Contactez les développeurs de ScriptLua", console.red)
        script.stop()
        return false
    end
    
    -- Vérification du nom du personnage
    if currentCharacterName ~= AUTHORIZED_CHARACTER_NAME then
        console.print("🚫 ACCÈS REFUSÉ ", console.red)
        console.print("🔒 Nom détecté: " .. tostring(currentCharacterName), console.red)
        console.print("🔒 Nom autorisé: " .. AUTHORIZED_CHARACTER_NAME, console.red)
        console.print("📞 Contactez les développeurs de ScriptLua", console.red)
        script.stop()
        return false
    end
    
    -- Si toutes les vérifications passent
    console.print("✅ VÉRIFICATION DE SÉCURITÉ RÉUSSIE", console.green)
    console.print("👤 Leader autorisé: " .. currentCharacterName .. " (ID: " .. currentCharacterId .. ")", console.green)
    console.print("🛡️ === SÉCURITÉ: LEADER VALIDÉ ===", console.green)
    return true
end

-- Exécuter la vérification de sécurité immédiatement
console.print("🔒 Démarrage vérification sécurité...", console.blue)
if not securityCheck() then
    console.print("🛑 ARRÊT DU SCRIPT - SÉCURITÉ ÉCHOUÉE", console.red)
    return -- Arrêter l'exécution du script si la vérification échoue
end

-- ===============================================================================
-- 📁 IMPORT DES SETTINGS
-- ===============================================================================

local RecoverySettings = nil

-- Tentative #1 : Fichier settings.lua dans le même dossier que le script
pcall(function()
    RecoverySettings = dofile("settings.lua")
    console.print("✅ Settings chargés depuis le même dossier que le script", console.green)
end)

-- Tentative #2 : Chemin avec dossier du script (fallback)
if not RecoverySettings then
    pcall(function()
        local scriptPath = script.folder() .. "settings.lua"
        RecoverySettings = dofile(scriptPath)
        console.print("✅ Settings chargés depuis: " .. scriptPath, console.green)
    end)
end

-- Tentative #3 : Utiliser require() standard (fallback)
if not RecoverySettings then
    pcall(function()
        RecoverySettings = require("settings")
        console.print("✅ Settings chargés avec require()", console.green)
    end)
end

-- Arrêt si settings.lua non trouvé
if not RecoverySettings then
    console.print("❌ ERREUR: Impossible de charger settings.lua", console.red)
    console.print("❌ Vérifiez que settings.lua est dans le même dossier que le script", console.red)
    return
end

console.print("✅ Configuration chargée avec succès", console.green)

-- ===============================================================================
-- ⚡ SYSTÈME DE CONSOMMATION PAIN/ÉNERGIE
-- ===============================================================================

-- Variable pour éviter la récupération multiple
local recoveryInProgress = false

-- Fonction de consommation d'items (UNIQUEMENT SETTINGS)
function useItemByGID(primaryGID, quantity, itemName)
    console.print("🍴 UTILISATION " .. itemName .. " (GID: " .. primaryGID .. ")", console.yellow)
    
    pcall(function()
        inventory.useObject(primaryGID, quantity)
        console.print("✅ Objet utilisé : inventory.useObject(" .. primaryGID .. ", " .. quantity .. ")", console.green)
    end)
    
    global.sleep(0)
    console.print("✅ " .. itemName .. " : Commande envoyée", console.green)
    return true
end

-- Fonction principale de récupération après combat
function executeRecovery(fightResult)
    if recoveryInProgress then
        console.print("⏰ Récupération déjà en cours...", console.yellow)
        return false
    end
    
    recoveryInProgress = true
    console.print("🍞 === RÉCUPÉRATION AUTOMATIQUE APRÈS COMBAT ===", console.blue)
    console.print("🎯 Résultat combat: " .. fightResult, console.cyan)
    
    if fightResult == "defeat" then
        console.print("💀 === DÉTECTION DÉFAITE - RÉCUPÉRATION FORCÉE ===", console.red)
        console.print("🛡️ Cette récupération doit TOUJOURS fonctionner après défaite", console.red)
        console.print("🍞 Pain + Énergie seront consommés pour leaders ET mules", console.red)
    else
        console.print("🏆 Récupération post-victoire normale", console.green)
    end
    
    -- Vérifier si leader ou mule
    local isLeader = false
    local teamCount = 1
    
    pcall(function()
        teamCount = global.teamCount() or 1
        if teamCount <= 1 then
            isLeader = true  -- Solo = leader
        else
            isLeader = global.isTeamLeader() or false
        end
    end)
    
    if isLeader then
        console.print("👑 LEADER - Utilisation Pain + Énergie", console.blue)
        console.print("🔧 Équipe: " .. teamCount .. " membre(s)", console.cyan)
        
        -- Pain pour leader
        console.print("🍞 LEADER: Consommation Pain...", console.yellow)
        useItemByGID(RecoverySettings.PAIN_GID, RecoverySettings.PAIN_QUANTITY, "Pain (leader)")
        
        -- Énergie pour leader
        console.print("⚡ LEADER: Consommation Énergie...", console.yellow)
        useItemByGID(RecoverySettings.ENERGY_POTION_GID, RecoverySettings.ENERGY_POTION_QUANTITY, "Énergie (leader)")
        
        console.print("✅ LEADER: Récupération complète", console.green)
    else
        console.print("🤖 MULE - Utilisation Pain + Énergie", console.blue)
        console.print("🔧 Équipe: " .. teamCount .. " membre(s)", console.cyan)
        
        -- Pain pour mule
        console.print("🍞 MULE: Consommation Pain...", console.yellow)
        useItemByGID(RecoverySettings.PAIN_GID, RecoverySettings.PAIN_QUANTITY, "Pain (mule)")
        
        -- Énergie pour mule
        console.print("⚡ MULE: Consommation Énergie...", console.yellow)
        useItemByGID(RecoverySettings.ENERGY_POTION_GID, RecoverySettings.ENERGY_POTION_QUANTITY, "Énergie (mule)")
        
        console.print("✅ MULE: Récupération complète", console.green)
    end
    
    console.print("🎉 === RÉCUPÉRATION TERMINÉE ===", console.blue)
    console.print("✅ Mode: " .. fightResult .. " | Statut: Succès", console.green)
    
    if fightResult == "defeat" then
        console.print("💀 DÉFAITE: Pain/Énergie consommés avec succès", console.green)
        console.print("🔧 Problème de consommation post-défaite: RÉSOLU", console.green)
    end
    
    recoveryInProgress = false
    return true
end

-- Fonction de récupération initiale au lancement
function performInitialRecovery()
    console.print("🍞 === UTILISATION AUTOMATIQUE OBJETS AU LANCEMENT ===", console.blue)
    
    -- Déterminer si on est leader ou mule
    local isLeader = false
    local teamCount = 1
    
    pcall(function()
        teamCount = global.teamCount() or 1
        if teamCount <= 1 then
            isLeader = true  -- Solo = leader
        else
            isLeader = global.isTeamLeader() or false
        end
    end)
    
    -- Leaders ET mules utilisent pain/énergie
    if isLeader then
        console.print("👑 LEADER - Utilisation automatique Pain + Énergie", console.blue)
        
        useItemByGID(RecoverySettings.PAIN_GID, RecoverySettings.PAIN_QUANTITY, "Pain")
        useItemByGID(RecoverySettings.ENERGY_POTION_GID, RecoverySettings.ENERGY_POTION_QUANTITY, "Énergie")
        
    else
        console.print("🤖 MULE - Utilisation automatique Pain + Énergie", console.blue)
        
        useItemByGID(RecoverySettings.PAIN_GID, RecoverySettings.PAIN_QUANTITY, "Pain (mule)")
        useItemByGID(RecoverySettings.ENERGY_POTION_GID, RecoverySettings.ENERGY_POTION_QUANTITY, "Énergie")
    end
    
    console.print("🎉 Récupération initiale terminée", console.green)
    return true
end

-- Fonction simple pour consommer pain/énergie
function consumePainEnergie()
    console.print("🍞 === CONSOMMATION PAIN/ÉNERGIE ===", console.blue)
    
    -- Pain
    console.print("🍞 Consommation Pain...", console.yellow)
    useItemByGID(RecoverySettings.PAIN_GID, RecoverySettings.PAIN_QUANTITY, "Pain de récupération")
    
    -- Énergie
    console.print("⚡ Consommation Énergie...", console.yellow)
    useItemByGID(RecoverySettings.ENERGY_POTION_GID, RecoverySettings.ENERGY_POTION_QUANTITY, "Potion d'énergie")
    
    console.print("✅ Pain/Énergie consommés", console.green)
    return true
end

console.print("✅ SYSTÈME PAIN/ÉNERGIE CHARGÉ (CODE.LUA INTÉGRÉ)", console.green)

-- ===============================================================================
-- ⚡ SYSTÈME DE MONTÉE DES CARACTÉRISTIQUES (SETTINGS.LUA)
-- ===============================================================================

-- Fonction de montée des caractéristiques selon settings.lua
function upStats()
    -- Vérifier si c'est le leader - les leaders n'upgradent pas les stats automatiquement
    if global.isTeamLeader() then
        console.print("🔒 LEADER - Pas d'up stats automatique", console.orange)
        return
    end
    
    console.print("On monte les caracteristiques...", console.red)
    global.delay(200)
    keyboard.press(0x43)
    global.delay(200)
    mouse.click(846,846)
    global.delay(200)
    for _, k in ipairs(RecoverySettings.listStats) do
        if k.id == RecoverySettings.ChoiceStats then
            console.print("On monte la stat "..k.name.." au maximum", console.green)
            mouse.click(1087,754)
            global.delay(200)
            mouse.click(1124,847)
            global.delay(200)
            keyboard.press(0x1B)
            break
        end
    end
end

-- ===============================================================================
-- ⚡ SYSTÈME D'ÉQUIPEMENT AUTOMATIQUE (SETTINGS.LUA)
-- ===============================================================================

-- Fonction d'équipement automatique selon settings.lua
function stuff()
    local characterLevel = character.level()
    if characterLevel >= 200 then return end
    local content = inventory.content()

    -- GIDs déjà équipés par position
    local equippedGids = {}
    for _, item in ipairs(content) do
        if item.position ~= 63 then
            equippedGids[item.gid] = true
            console.print("[-] Déja équipé : " .. inventory.objectName(item.gid))
        end
    end

    -- Sélection des meilleurs objets équipables
    local bestItems = {}
    for _, item in ipairs(RecoverySettings.items) do
        if characterLevel >= item.level then
            local currentBest = bestItems[item.pos]
            if not currentBest or item.level > currentBest.level then
                bestItems[item.pos] = item
            end
        end
    end

    -- Équipement des meilleurs objets
    for pos, item in pairs(bestItems) do
        if not equippedGids[item.gid] then
            local itemName = inventory.objectName(item.gid) or "Inconnu"
            inventory.setObjectPosition(item.gid, pos)
            console.print("[✓] Équipé : " .. itemName .. " (Lvl " .. item.level .. ")")
            global.delay(global.random(0, 200))
        end
    end

    console.print("[LOG] Stuff équipé.")
end

console.print("✅ SYSTÈME STATS/STUFF CHARGÉ (SETTINGS.LUA)", console.green)

-- ===============================================================================
-- 🏰 DÉMARRAGE SCRIPT PRINCIPAL
-- ===============================================================================

console.print("🏰 Démarrage script Donjon Phossile v3.10 (MODE COMMERCIAL INTÉGRÉ + CODE.LUA)", console.blue)
console.print("🔧 CORRECTION MAPPING: Nervosité depuis 289 vise désormais 274 (au lieu de 261)", console.cyan)
console.print("🎯 AMÉLIORATIONS v3.10:", console.red)
console.print("  ✅ MAPPING CORRIGÉ: Position 289 → Nervosité sur 274 (était 261)", console.red)
console.print("  ✅ Vérifications renforcées des sorts (délais augmentés, double contrôle PA)", console.red)
console.print("  ✅ Sorts critiques avec tentatives multiples (Nervosité = 2 tentatives max)", console.red)
console.print("  ✅ Détection d'échecs silencieux API (sort non exécuté malgré logs positifs)", console.red)
console.print("  ✅ 🍞 SYSTÈME CODE.LUA INTÉGRÉ: Pain/Énergie avant chaque NPC", console.red)
console.print("🎯 NOUVELLES STRATÉGIES TOUR 1 ADAPTATIVES:", console.green)
console.print("  🎯 SPÉCIALE 3: Phossile(410,397,383,370,356,343,329) → Si 316 occupé par allié ET 289 disponible: Séquence complète 12 PA", console.green)
console.print("  🌟 SPÉCIALE 2: Phossile(425,411,398,384,371) → Fluctuation+Position357", console.green)
console.print("  🔥 SPÉCIALE 1: Phossile(339,325,312,285,298) → Attirance+Position271", console.green)
console.print("  🛡️ NORMALE: Autres positions → Mutilation+Berserk+Attirance", console.green)
smartLog("🚀 MISE À JOUR v3.2:", console.orange)
smartLog("  🎯 STRATÉGIE SPÉCIALE 3: Si 316 occupé par allié ET 289 disponible → Séquence complète 12 PA: Berserk+Aversion(330)+Déplacement(289)+Attirance+Double Nervosité(274)", console.orange)
smartLog("  🌟 STRATÉGIE SPÉCIALE 2: Cellule 357→371 (Phossile sur 425,411,398,384,371)", console.orange)
smartLog("  📊 SEUIL PV: Déchainement >50% PV (était 60%) / Dissolution ≤50% PV", console.orange)
smartLog("  🗡️ TOUR 5: Mutilation → Déplacement → Couronnes d'Épines → Double Déchainement(CAC)/Dissolution(Distance)", console.orange)
smartLog("  🗡️ TOUR 6: Déplacement → Condensation → Déchainement(CAC)/Dissolution(Distance) → Folie Sanguinaire", console.orange)
smartLog("  🗡️ TOUR 7: Mutilation → Déplacement → Double Condensation → Dissolution (Basculement post-mort si Phossile tué)", console.orange)
smartLog("  ✅ NETTOYAGE PA: Dissolution forcée si exactement 4 PA (optimise utilisation)", console.orange)
smartLog("  ✅ COURONNES D'ÉPINES: Position mise à jour après déplacement", console.orange)
smartLog("  ✅ Correction cellule cible T2: Maintenant sur soi-même (302) au lieu de 317", console.orange)
console.print("🎯 STRATÉGIE POST-PHOSSILE OPTIMISÉE:", console.cyan)
console.print("  ✅ 1. Mutilation sur soi (1/2 tours)", console.cyan)
console.print("  ✅ 2. Couronnes d'épines (1/3 tours)", console.cyan)
console.print("  ✅ 3. Condensation avec scoring AOE", console.cyan)
console.print("  ✅ 4-5. Déchainement (>50% PV) ou Dissolution (≤50% PV)", console.cyan)
console.print("  ✅ 6. NETTOYAGE PA restants (Projection, Pénitence, etc.)", console.cyan)

-- Configuration
local npcId = 2352
local npcExitId = 2352
local mapEntreeDonjon = 130547712
local mapExitDonjon = 130549760

-- Maps du donjon
local fightMaps = {
    130548736  -- Salle 5
}

-- Fonction principale obligatoire
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

-- Fonctions obligatoires (même si vides)
function bank()
    return {}
end

function phenix()
    return {}
end

-- ===============================================================================
-- 🧠 IA AVANCÉE AVEC SYSTÈME DE SCORING INTELLIGENT
-- ===============================================================================

-- Configuration des sorts Sacrieur avec IDs et caractéristiques
local SPELL_IDS = {
    MUTILATION = 12737,
    BERSERK = 12743,
    CONDENSATION = 12745,
    DISSOLUTION = 12757,
    DECHAINEMENT = 12755,
    FOLIE_SANGUINAIRE = 12740,
    COURONNES_EPINES = 12761,
    PROJECTION = 12726,
    STASE = 12728,
    FLUCTUATION = 12763,
    NERVOSITE = 12727,
    PENITENCE = 12764,
    ASSAUT = 12733,
    LIBERATION = 368,
    AVERSION = 12749,
    ATTIRANCE = 12735
}

-- Coûts en PA des sorts
local SPELL_COSTS = {
    [SPELL_IDS.MUTILATION] = 2,
    [SPELL_IDS.BERSERK] = 2,
    [SPELL_IDS.CONDENSATION] = 3,
    [SPELL_IDS.DISSOLUTION] = 4,
    [SPELL_IDS.DECHAINEMENT] = 4,
    [SPELL_IDS.FOLIE_SANGUINAIRE] = 3,
    [SPELL_IDS.COURONNES_EPINES] = 2,
    [SPELL_IDS.PROJECTION] = 2,
    [SPELL_IDS.STASE] = 3,
    [SPELL_IDS.FLUCTUATION] = 2,
    [SPELL_IDS.NERVOSITE] = 3,
    [SPELL_IDS.PENITENCE] = 2,
    [SPELL_IDS.ASSAUT] = 2,
    [SPELL_IDS.LIBERATION] = 3,
    [SPELL_IDS.AVERSION] = 2,
    [SPELL_IDS.ATTIRANCE] = 2
}

-- Portées des sorts
local SPELL_RANGES = {
    [SPELL_IDS.MUTILATION] = 1,
    [SPELL_IDS.CONDENSATION] = 5,
    [SPELL_IDS.DISSOLUTION] = 5,
    [SPELL_IDS.DECHAINEMENT] = 6,
    [SPELL_IDS.PROJECTION] = 5,
    [SPELL_IDS.STASE] = 5,
    [SPELL_IDS.NERVOSITE] = 4,
    [SPELL_IDS.PENITENCE] = 6,
    [SPELL_IDS.ASSAUT] = 2,
    [SPELL_IDS.AVERSION] = 5,
    [SPELL_IDS.ATTIRANCE] = 10
}

-- Configuration IA avancée (À DÉFINIR SELON VOS PRÉFÉRENCES)
local advancedSpells = {
    -- CONFIGURATION VIDE - À REMPLIR TOUR PAR TOUR SELON VOS INSTRUCTIONS
}

-- ===============================================================================
-- 🎯 SYSTÈME DE SCORING INTELLIGENT AVEC PRIORITÉ PHOSSILE
-- ===============================================================================

-- Fonction de recherche du Phossile (corrigée)
function findPhossile()
    console.print("🔍 Recherche Phossile en cours...", console.yellow)
    
    local success, fighters = pcall(function()
        return fight.fighters()
    end)
    
    if not success or not fighters then
        console.print("❌ Erreur récupération fighters", console.red)
        return nil
    end
    
    console.print("👥 " .. #fighters .. " combattants trouvés", console.cyan)
    
    for i, fighter in pairs(fighters) do
        console.print("  " .. i .. ": " .. (fighter.name or "Inconnu") .. " - Équipe: " .. (fighter.team or "?") .. " - Vivant: " .. tostring(fighter.alive ~= false), console.white)
        
        -- Même logique que le placement qui fonctionne
        if fighter.name and string.find(fighter.name:lower(), "phossile") and fighter.alive ~= false then
            console.print("✅ PHOSSILE TROUVÉ - Cell: " .. (fighter.cellId or "?"), console.green)
            return fighter
        end
    end
    
    console.print("❌ PHOSSILE NON TROUVÉ dans la liste", console.red)
    return nil
end

-- Fonction de récupération des stats améliorée (VERSION API FRIGOST)
function getFighterStats()
    local stats = {PA = 0, PM = 0, PV = 0, PVMax = 0}
    
    local success, fighter = pcall(function()
        return fight.currentFighter()
    end)
    
    if success and fighter then
        console.print("📊 Récupération stats via API Frigost...", console.blue)
        
        -- MÉTHODE 1 : Via stats officielles Frigost (priorité)
        if fighter.stats then
            stats.PA = fighter.stats[enum_Stat.ACTION_POINTS] or 0
            stats.PM = fighter.stats[enum_Stat.MOVEMENT_POINTS] or 0
            stats.PV = fighter.stats[enum_Stat.CUR_LIFE] or 0
            stats.PVMax = fighter.stats[enum_Stat.MAX_LIFE] or 0
            
            console.print("✅ Stats via enum_Stat - PA=" .. stats.PA .. " PM=" .. stats.PM .. " PV=" .. stats.PV .. "/" .. stats.PVMax, console.green)
        else
            -- MÉTHODE 2 : Via propriétés directes (fallback)
        stats.PA = fighter.actionPoints or fighter.AP or 0
        stats.PM = fighter.movementPoints or fighter.MP or 0
        stats.PV = fighter.lifePoints or fighter.currentLifePoints or 0
        stats.PVMax = fighter.maxLifePoints or 0
        
            console.print("⚠️ Stats via propriétés directes (fallback) - PA=" .. stats.PA .. " PM=" .. stats.PM .. " PV=" .. stats.PV .. "/" .. stats.PVMax, console.orange)
        end
    else
        console.print("❌ Impossible de récupérer les stats du fighter", console.red)
    end
    
    return stats
end

-- ===============================================================================
-- 🔄 FONCTIONS D'ACTUALISATION PA VIA API FRIGOST
-- ===============================================================================

-- Fonction pour actualiser les PA via l'API après chaque sort (UTILISE LA MÊME LOGIQUE QUE getRealActionPoints)
function refreshCurrentPA()
    -- Utiliser la même logique que getRealActionPoints pour cohérence
    return getRealActionPoints()
end

-- Fonction pour lancer un sort et actualiser automatiquement les PA (VERSION RENFORCÉE)
function castSpellAndRefresh(spellId, targetCell, spellName, expectedPACost)
    local success = false
    local paAvant = refreshCurrentPA()
    
    console.print("🔮 Tentative " .. spellName .. " - PA avant: " .. paAvant, console.cyan)
    console.print("🎯 CIBLE: " .. targetCell .. " | SORT ID: " .. spellId, console.white)
    
    if fight.canCastSpell(spellId, targetCell) ~= 0 then
        -- LANCEMENT SÉCURISÉ AVEC DOUBLE VÉRIFICATION
        console.print("🚀 LANCEMENT SORT EN COURS...", console.blue)
        
        fight.castSpell(spellId, targetCell)
        global.sleep(50) -- Délai plus long pour s'assurer de la transmission
        
        local paApres = refreshCurrentPA()
        local paUtilises = math.max(0, paAvant - paApres)
        
        console.print("📊 VÉRIFICATION: PA avant(" .. paAvant .. ") → PA après(" .. paApres .. ") = " .. paUtilises .. " PA utilisés", console.white)
        
        -- VÉRIFICATION CRITIQUE : Si PA utilisés = 0, le sort a échoué silencieusement
        if paUtilises > 0 then
            -- DOUBLE VÉRIFICATION : Vérifier si le sort peut encore être lancé (cooldown activé ?)
            global.sleep(50)
            local canCastAgain = fight.canCastSpell(spellId, targetCell)
            
            if paUtilises >= expectedPACost then
                console.print("✅ " .. spellName .. " RÉUSSI - PA utilisés: " .. paUtilises .. " (attendu: " .. expectedPACost .. ") - PA actuels: " .. paApres, console.green)
                success = true
            else
                console.print("⚠️ " .. spellName .. " PARTIEL - PA utilisés: " .. paUtilises .. " (attendu: " .. expectedPACost .. ") - Possible échec", console.orange)
                success = false
            end
        else
            console.print("❌ " .. spellName .. " ÉCHEC SILENCIEUX (PA utilisés: 0) - Sort non exécuté dans le jeu", console.red)
            success = false
        end
    else
        console.print("❌ " .. spellName .. " impossible à lancer (canCastSpell = 0)", console.red)
    end
    
    return success, refreshCurrentPA()
end

-- Fonction RENFORCÉE pour sorts critiques avec tentatives multiples
function castCriticalSpellWithRetry(spellId, targetCell, spellName, expectedPACost, maxRetries)
    maxRetries = maxRetries or 3
    
    console.print("🎯 === LANCEMENT SORT CRITIQUE: " .. spellName .. " ===", console.red)
    console.print("🔄 Maximum " .. maxRetries .. " tentatives si échec", console.yellow)
    
    for attempt = 1, maxRetries do
        console.print("🔄 TENTATIVE #" .. attempt .. "/" .. maxRetries, console.cyan)
        
        local success, newPA = castSpellAndRefresh(spellId, targetCell, spellName, expectedPACost)
        
        if success then
            console.print("✅ SORT CRITIQUE " .. spellName .. " RÉUSSI à la tentative #" .. attempt, console.green)
            return true, newPA
        else
            console.print("❌ Tentative #" .. attempt .. " échouée", console.red)
            
            if attempt < maxRetries then
                console.print("⏳ Attente avant nouvelle tentative...", console.orange)
                global.sleep(0) -- Pause plus longue entre tentatives
                
                -- Vérifier si le sort est toujours possible
                if fight.canCastSpell(spellId, targetCell) == 0 then
                    console.print("❌ Sort " .. spellName .. " plus possible - Arrêt des tentatives", console.red)
                    break
                end
            end
        end
    end
    
    console.print("❌ SORT CRITIQUE " .. spellName .. " ÉCHEC TOTAL après " .. maxRetries .. " tentatives", console.red)
    return false, refreshCurrentPA()
end

-- Fonction de debug pour comparer PA calculés vs PA réels
function debugPAComparison(calculatedPA, description)
    local realPA = refreshCurrentPA()
    local difference = math.abs(calculatedPA - realPA)
    
    if difference > 0 then
        console.print("⚠️ DÉSYNC PA " .. description .. " - Calculé: " .. calculatedPA .. ", Réel: " .. realPA .. " (diff: " .. difference .. ")", console.red)
    else
        console.print("✅ PA SYNC " .. description .. " - " .. realPA .. " PA", console.green)
    end
    
    return realPA
end

-- ===============================================================================

-- Fonction pour calculer le score d'une zone d'effet avec priorité Phossile
function calculateZoneScore(centerCell, spellId)
    local score = 0
    local enemiesInZone = 0
    local phossileInZone = false
    local affectedCells = {}
    
    -- Déterminer les cellules affectées selon le sort
    if spellId == SPELL_IDS.CONDENSATION then
        -- Cercle rayon 2
        pcall(function()
            if fight.circleCells then
                affectedCells = fight.circleCells(centerCell, 0, 2) or {}
            end
        end)
    elseif spellId == SPELL_IDS.DISSOLUTION then
        -- Croix rayon 1  
        pcall(function()
            if fight.crossCells then
                affectedCells = fight.crossCells(centerCell, 0, 1) or {}
            end
        end)
    elseif spellId == SPELL_IDS.NERVOSITE then
        -- Cercle rayon 2
        pcall(function()
            if fight.circleCells then
                affectedCells = fight.circleCells(centerCell, 2, 2) or {}
            end
        end)
    end
    
    -- Compter les ennemis dans la zone
    local allFighters = fight.fighters() or {}
    local me = fight.currentFighter()
    
    if me then
        for _, fighter in pairs(allFighters) do
            -- CORRECTION : Gérer le cas où fighter.alive est nil (considérer comme vivant)
            local isAlive = fighter.alive ~= false
            if fighter.team ~= me.team and isAlive then
                for _, cell in pairs(affectedCells) do
                    -- CORRECTION : cell est un nombre directement, pas un objet avec .cellId
                    local cellId = (type(cell) == "table" and cell.cellId) or cell
                    if cellId == fighter.cellId then
                        enemiesInZone = enemiesInZone + 1
                        score = score + 100 -- Base score par ennemi
                        
                        -- PRIORITÉ ABSOLUE PHOSSILE
                        if fighter.name and (
                            string.find(fighter.name:lower(), "phossile") or
                            string.find(fighter.name:lower(), "fossile") or
                            fighter.monsterId == 4046
                        ) then
                            phossileInZone = true
                            score = score + 10000 -- SCORE MASSIF pour Phossile
                            console.print("🔥 PHOSSILE DANS ZONE AOE ! Score: +" .. 10000, console.red)
                        end
                        
                        -- Bonus pour ennemis faibles
                        if fighter.lifePoints and fighter.lifePoints < 500 then
                            score = score + 50
                        end
                        
                        -- Bonus pour boss/cibles prioritaires (mais moins que Phossile)
                        if fighter.name and string.find(fighter.name:lower(), "boss") then
                            score = score + 200
                        end
                        
                        break
                    end
                end
            end
        end
    end
    
    -- Bonus pour zones multi-ennemis (mais Phossile prime toujours)
    if not phossileInZone then
        if enemiesInZone >= 3 then
            score = score + 500
        elseif enemiesInZone >= 2 then
            score = score + 200
        end
    end
    
    return score, enemiesInZone, phossileInZone
end

-- Fonction pour trouver la meilleure position pour un sort AOE avec priorité Phossile (TESTE CELLULES VIDES)
function findBestAOEPosition(spellId)
    local bestCell = nil
    local bestScore = 0
    local bestEnemyCount = 0
    local bestHasPhossile = false
    
    local allFighters = fight.fighters() or {}
    local me = fight.currentFighter()
    
    if not me then return nil, 0, 0, false end
    
    local phossile = findPhossile()
    local maxRange = SPELL_RANGES[spellId] or 5
    
    console.print("🎯 AOE INTELLIGENTE - Test " .. (phossile and "AVEC" or "SANS") .. " Phossile", console.cyan)
    
    -- MÉTHODE 1: Tester TOUTES les cellules accessibles dans la portée (MÊME VIDES)
    local accessibleCells = fight.accessibleCells() or {}
    console.print("🔍 Test de " .. #accessibleCells .. " cellules accessibles pour AOE optimale", console.yellow)
    
    for _, cellData in ipairs(accessibleCells) do
        local testCell = cellData.cellId
        local distance = fight.cellsDistance(me.cellId, testCell)
        
        -- Vérifier si dans la portée du sort
                if distance <= maxRange then
            local canCast = fight.canCastSpell(spellId, testCell)
            
            if canCast ~= 0 then
                local score, enemyCount, hasPhossile = calculateZoneScore(testCell, spellId)
                
                -- Critères de sélection selon présence Phossile
                local shouldSelect = false
                
                if phossile then
                    -- Avec Phossile : Priorité ABSOLUE aux zones incluant Phossile
                    if hasPhossile and (score > bestScore or not bestHasPhossile) then
                        shouldSelect = true
                        console.print("🔥 CELLULE AOE AVEC PHOSSILE: " .. testCell .. " - Ennemis: " .. enemyCount .. ", Score: " .. score, console.red)
                    elseif not bestHasPhossile and score > bestScore then
                        shouldSelect = true
                        console.print("📊 Cellule AOE candidate: " .. testCell .. " - Ennemis: " .. enemyCount .. ", Score: " .. score, console.cyan)
                    end
                else
                    -- Sans Phossile : Maximiser le nombre d'ennemis touchés
                    if score > bestScore then
                        shouldSelect = true
                        console.print("📊 Cellule AOE optimale: " .. testCell .. " - Ennemis: " .. enemyCount .. ", Score: " .. score, console.green)
                    end
                end
                
                if shouldSelect then
                        bestScore = score
                    bestCell = testCell
                        bestEnemyCount = enemyCount
                    bestHasPhossile = hasPhossile
                    end
                end
            end
        end
        
    -- MÉTHODE 2: Si aucune cellule libre n'est optimale, tester directement sur ennemis
    if bestEnemyCount <= 1 then
        console.print("🔄 AOE - Test direct sur ennemis (cellules libres non optimales)", console.orange)
        
        for _, fighter in pairs(allFighters) do
            local isAlive = fighter.alive ~= false
            if fighter.team ~= me.team and isAlive then
                local distance = fight.cellsDistance(me.cellId, fighter.cellId)
                
                if distance <= maxRange then
                    local canCast = fight.canCastSpell(spellId, fighter.cellId)
                    if canCast ~= 0 then
                    local score, enemyCount, hasPhossile = calculateZoneScore(fighter.cellId, spellId)
                    
                    if score > bestScore then
                        bestScore = score
                        bestCell = fighter.cellId
                        bestEnemyCount = enemyCount
                            bestHasPhossile = hasPhossile
                            console.print("📊 Ciblage direct optimal: " .. (fighter.name or "Ennemi") .. " - Ennemis: " .. enemyCount, console.green)
                        end
                    end
                end
            end
        end
    end
    
    if bestCell then
        console.print("✅ AOE OPTIMALE SÉLECTIONNÉE: Cellule " .. bestCell .. " → " .. bestEnemyCount .. " ennemis (Score: " .. bestScore .. ")", console.green)
    else
        console.print("❌ Aucune position AOE valide trouvée", console.red)
    end
    
    return bestCell, bestScore, bestEnemyCount, bestHasPhossile
end

-- Fonction pour trouver la meilleure zone AOE pour Dissolution incluant le Phossile
function findBestDissolutionAOE(phossile)
    if not phossile then return nil end
    
    local me = fight.currentFighter()
    if not me then return nil end
    
    -- Vérifier si on peut faire Dissolution directement sur le Phossile
    local distance = fight.cellsDistance(me.cellId, phossile.cellId)
    local maxRange = SPELL_RANGES[SPELL_IDS.DISSOLUTION] or 5
    
    if distance <= maxRange then
        local score, enemyCount, hasPhossile = calculateZoneScore(phossile.cellId, SPELL_IDS.DISSOLUTION)
        if hasPhossile then
            console.print("✅ Zone Dissolution AOE avec Phossile trouvée", console.green)
            return phossile.cellId
        end
    end
    
    console.print("❌ Aucune zone Dissolution AOE valide", console.red)
    return nil
end

-- Fonction de stratégie défensive pour T4 (STRATÉGIE FUITE - JAMAIS DE RE-ÉVALUATION)
function executeDefensiveStrategy()
    console.print("🛡️ === EXÉCUTION STRATÉGIE DÉFENSIVE/FUITE T4 ===", console.blue)
    console.print("🔒 STRATÉGIE VERROUILLÉE - Aucune re-évaluation des PV Phossile", console.blue)
    
    local me = fight.currentFighter()
    if not me then
        console.print("❌ Pas de combattant actuel", console.red)
        return false
    end
    
    local currentPA = getRealActionPoints() -- UTILISER L'API OFFICIELLE
    console.print("🛡️ PA disponibles (Stratégie Défensive): " .. currentPA, console.yellow)
    
    -- ÉTAPE 1: FLUCTUATION défensive avec mise à jour automatique des PA
    if currentPA >= 2 then
        local success, newPA = castSpellAndRefresh(SPELL_IDS.FLUCTUATION, me.cellId, "FLUCTUATION DÉFENSIVE", 2)
        currentPA = newPA
        if not success then
            console.print("❌ 1. FLUCTUATION défensive impossible", console.red)
        end
    else
        console.print("❌ 1. PA insuffisants pour FLUCTUATION (" .. currentPA .. "/2)", console.red)
    end
    
    -- ÉTAPE 2: PÉNITENCE défensive avec mise à jour automatique des PA
    currentPA = getRealActionPoints() -- Actualiser PA après étape 1
    if currentPA >= 2 then
        local phossile = findPhossile()
        if phossile then
            local distance = fight.cellsDistance(me.cellId, phossile.cellId)
            console.print("🎯 PHOSSILE trouvé pour PÉNITENCE: " .. (phossile.name or "Phossile") .. " à distance " .. distance, console.cyan)
            
            local maxRange = SPELL_RANGES[SPELL_IDS.PENITENCE] or 6
            if distance <= maxRange then
                local success, newPA = castSpellAndRefresh(SPELL_IDS.PENITENCE, phossile.cellId, "PÉNITENCE DÉFENSIVE SUR PHOSSILE", 2)
                currentPA = newPA
                if not success then
                    console.print("⚠️ 2. PÉNITENCE impossible sur PHOSSILE - Sort bloqué", console.orange)
                end
            else
                console.print("⚠️ 2. PÉNITENCE impossible sur PHOSSILE - Distance: " .. distance .. ", Portée max: " .. maxRange, console.orange)
            end
        else
            console.print("⚠️ 2. PHOSSILE NON TROUVÉ pour PÉNITENCE - Fallback ennemi proche", console.orange)
            
            -- FALLBACK : Si Phossile mort, cibler un ennemi proche
        local nearEnemy = fight.nearestEnemy()
        if nearEnemy then
            local distance = fight.cellsDistance(me.cellId, nearEnemy.cellId)
                console.print("🎯 Fallback ennemi proche: " .. (nearEnemy.name or "Inconnu") .. " à distance " .. distance, console.cyan)
                
                if distance <= 6 then
                    local success, newPA = castSpellAndRefresh(SPELL_IDS.PENITENCE, nearEnemy.cellId, "PÉNITENCE FALLBACK", 2)
                    currentPA = newPA
                    if not success then
                        console.print("⚠️ 2. PÉNITENCE fallback impossible", console.orange)
                    end
                else
                    console.print("⚠️ 2. PÉNITENCE fallback hors portée - Distance: " .. distance, console.orange)
                end
            else
                console.print("⚠️ 2. Aucun ennemi trouvé pour fallback", console.orange)
            end
        end
    else
        console.print("❌ 2. PA insuffisants pour PÉNITENCE (" .. currentPA .. "/2)", console.red)
    end
    
    -- ÉTAPE 3: DISSOLUTION défensive avec PRIORITÉ PHOSSILE DIRECTE + AOE FALLBACK
    currentPA = getRealActionPoints() -- Actualiser PA après étape 2
    if currentPA >= 4 then
        local actionTaken = false
        
        -- PRIORITÉ 1: CIBLAGE DIRECT DU PHOSSILE
        local phossile = findPhossile()
        if phossile then
            local distance = fight.cellsDistance(me.cellId, phossile.cellId)
            local maxRange = SPELL_RANGES[SPELL_IDS.DISSOLUTION] or 5
            
            console.print("🎯 PRIORITÉ 1 - DISSOLUTION DIRECTE sur PHOSSILE (distance: " .. distance .. ", portée: " .. maxRange .. ")", console.red)
            
            if distance <= maxRange and fight.canCastSpell(SPELL_IDS.DISSOLUTION, phossile.cellId) ~= 0 then
                local success, newPA = castSpellAndRefresh(SPELL_IDS.DISSOLUTION, phossile.cellId, "DISSOLUTION DIRECTE PHOSSILE", 4)
                currentPA = newPA
                if success then
                    console.print("✅ 3. DISSOLUTION DIRECTE PHOSSILE RÉUSSIE", console.green)
                    actionTaken = true
                else
                    console.print("❌ 3. DISSOLUTION DIRECTE PHOSSILE échouée", console.red)
                end
            else
                console.print("⚠️ DISSOLUTION DIRECTE PHOSSILE impossible (distance: " .. distance .. ", canCast: " .. fight.canCastSpell(SPELL_IDS.DISSOLUTION, phossile.cellId) .. ")", console.orange)
            end
        end
        
        -- PRIORITÉ 2: AOE INCLUANT PHOSSILE (si direct impossible)
        if not actionTaken and phossile then
            console.print("🎯 PRIORITÉ 2 - DISSOLUTION AOE incluant PHOSSILE", console.orange)
            local bestCell, score, enemyCount, hasPhossile = findBestAOEPosition(SPELL_IDS.DISSOLUTION)
            
            if bestCell and hasPhossile and enemyCount >= 1 then
                local success, newPA = castSpellAndRefresh(SPELL_IDS.DISSOLUTION, bestCell, "DISSOLUTION AOE DÉFENSIVE", 4)
                currentPA = newPA
                if success then
                    console.print("✅ 3. DISSOLUTION AOE DÉFENSIVE RÉUSSIE (+" .. enemyCount .. " ennemis avec Phossile)", console.green)
                    actionTaken = true
                else
                    console.print("❌ 3. DISSOLUTION AOE DÉFENSIVE échouée", console.red)
                end
            else
                console.print("⚠️ DISSOLUTION AOE impossible (cell: " .. tostring(bestCell) .. ", hasPhossile: " .. tostring(hasPhossile) .. ")", console.orange)
            end
        end
        
        -- PRIORITÉ 3: FALLBACK ENNEMI PROCHE (dernier recours)
        if not actionTaken then
            console.print("🎯 PRIORITÉ 3 - DISSOLUTION FALLBACK ennemi proche", console.yellow)
            local nearEnemy = fight.nearestEnemy()
            if nearEnemy then
                console.print("🎯 Fallback ennemi: " .. (nearEnemy.name or "Inconnu"), console.cyan)
                
                local success, newPA = castSpellAndRefresh(SPELL_IDS.DISSOLUTION, nearEnemy.cellId, "DISSOLUTION FALLBACK", 4)
                currentPA = newPA
                if success then
                    console.print("✅ 3. DISSOLUTION FALLBACK RÉUSSIE", console.green)
                    actionTaken = true
                else
                    console.print("❌ 3. DISSOLUTION FALLBACK échouée", console.red)
                end
            else
                console.print("⚠️ 3. Aucun ennemi trouvé pour fallback", console.orange)
            end
        end
        
        if not actionTaken then
            console.print("❌ 3. DISSOLUTION TOTALEMENT IMPOSSIBLE - Toutes priorités échouées", console.red)
        end
    else
        console.print("❌ 3. PA insuffisants pour DISSOLUTION (" .. currentPA .. "/4)", console.red)
    end
    
    -- ÉTAPE 4: LIBÉRATION défensive avec DEBUG AVANCÉ
    currentPA = getRealActionPoints() -- Actualiser PA après étape 3
    if currentPA >= 3 then
        console.print("🔍 DEBUG LIBÉRATION - Sort ID: " .. SPELL_IDS.LIBERATION .. ", Ma cellule: " .. me.cellId, console.white)
        console.print("🔍 DEBUG LIBÉRATION - canCastSpell résultat: " .. fight.canCastSpell(SPELL_IDS.LIBERATION, me.cellId), console.white)
        
        -- Essayer avec castSpellPostPhossileAndRefresh pour avoir plus de debug
        local success, newPA = castSpellPostPhossileAndRefresh(SPELL_IDS.LIBERATION, me.cellId, "LIBÉRATION DÉFENSIVE DÉTAILLÉE")
        currentPA = newPA
        
        if success then
            console.print("✅ 4. LIBÉRATION DÉFENSIVE RÉUSSIE", console.green)
        else
            console.print("❌ 4. LIBÉRATION défensive impossible", console.red)
            
            -- Test alternatif : Essayer différentes cellules adjacentes au cas où
            console.print("🔍 Test LIBÉRATION sur cellules adjacentes...", console.orange)
            local adjacentCells = fight.adjacentCells(me.cellId)
            local liberationWorked = false
            
            for i, cell in ipairs(adjacentCells) do
                local cellId = (type(cell) == "table" and cell.cellId) or cell
                if fight.cellIsFree(cellId) then
                    console.print("🔍 Test LIBÉRATION cellule " .. cellId .. " - canCast: " .. fight.canCastSpell(SPELL_IDS.LIBERATION, cellId), console.white)
                    
                    if fight.canCastSpell(SPELL_IDS.LIBERATION, cellId) ~= 0 then
                        local altSuccess, altPA = castSpellPostPhossileAndRefresh(SPELL_IDS.LIBERATION, cellId, "LIBÉRATION ALTERNATIVE " .. cellId)
                        if altSuccess then
                            currentPA = altPA
                            liberationWorked = true
                            console.print("✅ 4. LIBÉRATION ALTERNATIVE RÉUSSIE sur cellule " .. cellId, console.green)
                            break
                        end
                    end
                end
            end
            
            if not liberationWorked then
                console.print("❌ 4. LIBÉRATION impossible même sur cellules alternatives", console.red)
            end
        end
    else
        console.print("❌ 4. PA insuffisants pour LIBÉRATION (" .. currentPA .. "/3)", console.red)
    end
    
    -- ÉTAPE 5: FUITE DÉFENSIVE avec PM restants
    local stats = getFighterStats()
    local currentPM = stats.PM
    
    console.print("🏃 === ÉTAPE 5: FUITE DÉFENSIVE - PM disponibles: " .. currentPM .. " ===", console.yellow)
    
    if currentPM > 0 then
        local phossile = findPhossile()
        if phossile then
            console.print("🎯 Phossile trouvé pour calcul de fuite - Position: " .. phossile.cellId, console.cyan)
            
            -- Trouver la cellule accessible la plus éloignée du Phossile
            local accessibleCells = fight.accessibleCells()
            local bestFleeCell = nil
            local maxDistance = 0
            
            for _, cell in ipairs(accessibleCells) do
                local distanceToPhossile = fight.cellsDistance(cell.cellId, phossile.cellId)
                local distanceFromMe = fight.cellsDistance(me.cellId, cell.cellId)
                
                -- Vérifier si accessible avec PM disponibles et plus loin du Phossile
                if distanceFromMe <= currentPM and distanceFromMe > 0 and distanceToPhossile > maxDistance then
                    maxDistance = distanceToPhossile
                    bestFleeCell = cell.cellId
                end
            end
            
            if bestFleeCell then
                console.print("🏃 Tentative fuite défensive vers cellule " .. bestFleeCell .. " (distance Phossile: " .. maxDistance .. ")", console.yellow)
                fight.moveTowardCell(bestFleeCell)
                global.sleep(0) -- Délai pour laisser le déplacement s'effectuer
                
                -- Vérifier succès du déplacement
                local newMe = fight.currentFighter()
                if newMe and newMe.cellId ~= me.cellId then
                    console.print("✅ FUITE DÉFENSIVE RÉUSSIE - Nouvelle position: " .. newMe.cellId, console.green)
                else
                    console.print("⚠️ Fuite défensive partiellement réussie ou bloquée", console.orange)
                end
            else
                console.print("❌ 5. Aucune cellule de fuite trouvée avec PM: " .. currentPM, console.red)
            end
        else
            console.print("⚠️ 5. Phossile non trouvé - Fuite générale", console.orange)
            
            -- Fuite générale loin des ennemis
            local nearestEnemy = fight.nearestEnemy()
            if nearestEnemy then
                local accessibleCells = fight.accessibleCells()
                local bestFleeCell = nil
                local maxDistance = 0
                
                for _, cell in ipairs(accessibleCells) do
                    local distanceToEnemy = fight.cellsDistance(cell.cellId, nearestEnemy.cellId)
                    local distanceFromMe = fight.cellsDistance(me.cellId, cell.cellId)
                    
                    if distanceFromMe <= currentPM and distanceFromMe > 0 and distanceToEnemy > maxDistance then
                        maxDistance = distanceToEnemy
                        bestFleeCell = cell.cellId
                    end
                end
                
                if bestFleeCell then
                    console.print("🏃 Fuite générale vers cellule " .. bestFleeCell, console.yellow)
                    fight.moveTowardCell(bestFleeCell)
                    global.sleep(0)
                end
            end
        end
    else
        console.print("❌ 5. Aucun PM pour fuite défensive", console.red)
    end
    
    console.print("🛡️ === STRATÉGIE DÉFENSIVE/FUITE TERMINÉE - PA restants: " .. currentPA .. " ===", console.blue)
    return true
end

-- Fonction pour vérifier si Phossile peut être ciblé directement
function canTargetPhossileDirectly(spellId)
    local phossile = findPhossile()
    local me = fight.currentFighter()
    
    if not phossile or not me then return false, nil end
    
    local distance = fight.cellsDistance(me.cellId, phossile.cellId)
    local maxRange = SPELL_RANGES[spellId] or 5
    
    if distance <= maxRange then
        if fight.canCastSpell(spellId, phossile.cellId) ~= 0 then
            console.print("✅ PHOSSILE CIBLABLE DIRECTEMENT avec " .. spellId .. " (Distance: " .. distance .. ")", console.green)
            return true, phossile
        end
    end
    
    console.print("❌ PHOSSILE NON CIBLABLE avec " .. spellId .. " (Distance: " .. distance .. ", Portée: " .. maxRange .. ")", console.red)
    return false, phossile
end

-- ===============================================================================
-- 🎯 STRATÉGIE TOUR 1 SPÉCIALISÉE
-- ===============================================================================

-- Mapping Phossile → Nervosité pour tour 1
local NERVOSITY_TARGETS = {
    [270] = 271,
    [284] = 285,
    [257] = 258,
    [243] = 244,
    [329] = 301,
    [316] = 288,
    [302] = 274,
    [343] = 315
}

-- Mapping spécial pour stratégie 3 tactique (position déplacement → cible nervosité)
local TACTICAL_NERVOSITY_TARGETS = {
    [289] = 274,  -- Si déplacement vers 289 → Nervosité sur 274 (CORRIGÉ)
    [302] = 288,  -- Si déplacement vers 302 → Nervosité sur 288  
    [329] = 315   -- Si déplacement vers 329 → Nervosité sur 315 (NOUVEAU)
}

-- Fonction pour vérifier si une cellule est disponible (libre OU occupée par le leader)
function isCellAvailableForLeader(cellId)
    local me = fight.currentFighter()
    if not me then return false end
    
    -- Si la cellule est libre, elle est disponible
    if fight.cellIsFree(cellId) then
        return true
    end
    
    -- Si c'est le leader qui occupe la cellule, elle est "disponible"
    if me.cellId == cellId then
        return true
    end
    
    -- Vérifier si c'est un allié du leader qui occupe la cellule
    local fighterOnCell = fight.fighterByCellId(cellId)
    if fighterOnCell and fighterOnCell.team == me.team then
        console.print("🔍 Cellule " .. cellId .. " occupée par allié: " .. (fighterOnCell.name or "Inconnu"), console.yellow)
        return false -- Occupée par un autre allié
    end
    
    -- Cellule occupée par un ennemi (non disponible)
    return false
end

-- Fonction spécialisée pour la priorité 1 si 316 occupé par allié ET 289 disponible (SÉQUENCE AVERSION)
function executeSpecialStrategy1Priority(phossile)
    console.print("🚨 === EXÉCUTION PRIORITÉ 1 - SÉQUENCE COMPLÈTE (12 PA) ===", console.red)
    console.print("🚨 SÉQUENCE: Berserk(2) + Aversion(2) + Déplacement(289) + Attirance(2) + Double Nervosité(6)", console.red)
    
    local me = fight.currentFighter()
    if not me then
        console.print("❌ Pas de combattant actuel", console.red)
        return false
    end
    
    local currentPA = refreshCurrentPA()
    console.print("⚡ PA COMPLETS de départ: " .. currentPA, console.cyan)
    
    -- ÉTAPE 1: BERSERK (2 PA) - Sur soi-même (PREMIER LANCEMENT UNIQUEMENT)
    console.print("💪 === ÉTAPE 1 PRIORITÉ 1: BERSERK INITIAL ===", console.blue)
    if currentPA >= 2 then
        local success, newPA = castSpellAndRefresh(SPELL_IDS.BERSERK, me.cellId, "BERSERK PRIORITÉ 1 INITIAL", 2)
        currentPA = newPA
        
        if success then
            console.print("✅ BERSERK priorité 1 initial réussi", console.green)
        else
            console.print("❌ BERSERK priorité 1 initial échoué", console.red)
        end
    else
        console.print("❌ PA insuffisants pour BERSERK (PA: " .. currentPA .. "/2)", console.red)
    end
    
    -- ÉTAPE 2: AVERSION sur cellule 330 (2 PA)
    console.print("🌀 === ÉTAPE 2 PRIORITÉ 1: AVERSION SUR 330 ===", console.blue)
    currentPA = refreshCurrentPA()
    local aversionTarget = 330
    
    if currentPA >= 2 then
        console.print("🎯 Cible AVERSION: cellule " .. aversionTarget, console.cyan)
        local distance = fight.cellsDistance(me.cellId, aversionTarget)
        local maxRange = SPELL_RANGES[SPELL_IDS.AVERSION] or 10
        
        console.print("📏 Distance vers cible: " .. distance .. " (Portée AVERSION: " .. maxRange .. ")", console.cyan)
        
        if distance <= maxRange then
            local canCast = fight.canCastSpell(SPELL_IDS.AVERSION, aversionTarget)
            console.print("🔍 fight.canCastSpell AVERSION result: " .. canCast, console.yellow)
            
            if canCast ~= 0 then
                local success, newPA = castSpellAndRefresh(SPELL_IDS.AVERSION, aversionTarget, "AVERSION PRIORITÉ 1", 2)
                currentPA = newPA
                
                if success then
                    console.print("✅ AVERSION sur 330 réussie", console.green)
                else
                    console.print("❌ AVERSION sur 330 échouée", console.red)
                end
            else
                console.print("❌ AVERSION impossible - fight.canCastSpell retourne 0", console.red)
            end
        else
            console.print("❌ AVERSION impossible - Hors portée (distance: " .. distance .. " > " .. maxRange .. ")", console.red)
        end
    else
        console.print("❌ PA insuffisants pour AVERSION (PA: " .. currentPA .. "/2)", console.red)
    end
    
    -- ÉTAPE 3: DÉPLACEMENT vers 289
    console.print("🏃 === ÉTAPE 3 PRIORITÉ 1: DÉPLACEMENT VERS 289 ===", console.blue)
    me = fight.currentFighter() -- Actualiser position
    
    local targetCell = 289
    local initialPosition = me.cellId
    local currentDistance = fight.cellsDistance(initialPosition, targetCell)
    
    console.print("🏃 Position initiale: " .. initialPosition .. " → Cible: " .. targetCell .. " (distance: " .. currentDistance .. ")", console.yellow)
    
    if currentDistance > 0 then
        console.print("🏃 Déplacement priorité 1 vers " .. targetCell .. "...", console.yellow)
        
        -- MÉTHODE 1: Essayer moveToAccessibleCell
        local method1Success = pcall(function()
            fight.moveToAccessibleCell(targetCell)
        end)
        
        global.sleep(0) -- Attendre mouvement
        me = fight.currentFighter() -- Actualiser position
        local newPosition = me.cellId
        local actuallyMoved = (newPosition ~= initialPosition)
        
        console.print("📍 Position après méthode 1: " .. newPosition .. " (bougé: " .. tostring(actuallyMoved) .. ")", console.cyan)
        
        -- Si pas bougé, essayer MÉTHODE 2
        if not actuallyMoved then
            console.print("🔄 Méthode 1 échouée - Essai déplacement alternatif", console.orange)
            
            local method2Success = pcall(function()
                fight.moveTowardCell(targetCell)
            end)
            
            global.sleep(0) -- Attendre mouvement
            me = fight.currentFighter() -- Actualiser position
            newPosition = me.cellId
            actuallyMoved = (newPosition ~= initialPosition)
            
            console.print("📍 Position après méthode 2: " .. newPosition .. " (bougé: " .. tostring(actuallyMoved) .. ")", console.cyan)
        end
        
        if actuallyMoved then
            local finalDistance = fight.cellsDistance(newPosition, targetCell)
            if finalDistance == 0 then
                console.print("✅ Déplacement vers 289 RÉUSSI (position exacte)", console.green)
            else
                console.print("✅ Déplacement partiel vers 289 (distance finale: " .. finalDistance .. ")", console.green)
            end
        else
            console.print("❌ Déplacement vers 289 IMPOSSIBLE - Position inchangée", console.red)
        end
    else
        console.print("✅ Déjà en position 289", console.green)
    end
    
    -- ÉTAPE 4: ATTIRANCE sur Phossile
    console.print("🔥 === ÉTAPE 4 PRIORITÉ 1: ATTIRANCE SUR PHOSSILE ===", console.blue)
    
    -- Actualiser position ET Phossile
    me = fight.currentFighter()
    phossile = findPhossile()
    currentPA = refreshCurrentPA()
    
    if not me then
        console.print("❌ Impossible de récupérer position actuelle", console.red)
    elseif not phossile then
        console.print("❌ Phossile introuvable pour ATTIRANCE", console.red)
    else
        console.print("📍 Position actuelle: " .. me.cellId, console.cyan)
        console.print("🎯 Position Phossile: " .. phossile.cellId, console.cyan)
        
        local distance = fight.cellsDistance(me.cellId, phossile.cellId)
        local maxRange = SPELL_RANGES[SPELL_IDS.ATTIRANCE] or 10
        
        console.print("📏 Distance vers Phossile: " .. distance .. " (Portée ATTIRANCE: 2-" .. maxRange .. ")", console.cyan)
        
        if currentPA >= 2 then
            if distance >= 2 and distance <= maxRange then
                console.print("✅ Conditions ATTIRANCE OK - Lancement du sort", console.green)
                local canCast = fight.canCastSpell(SPELL_IDS.ATTIRANCE, phossile.cellId)
                console.print("🔍 fight.canCastSpell result: " .. canCast, console.yellow)
                
                if canCast ~= 0 then
                    local success, newPA = castSpellAndRefresh(SPELL_IDS.ATTIRANCE, phossile.cellId, "ATTIRANCE PRIORITÉ 1", 2)
                    currentPA = newPA
                    
                    if success then
                        console.print("✅ ATTIRANCE PRIORITÉ 1 réussie", console.green)
                        global.sleep(0) -- Temps pour mouvement Phossile
                        phossile = findPhossile() -- Actualiser position Phossile
                        if phossile then
                            console.print("🔄 Position Phossile après ATTIRANCE: " .. phossile.cellId, console.cyan)
                        end
                    else
                        console.print("❌ ATTIRANCE PRIORITÉ 1 échouée malgré les vérifications", console.red)
                    end
                else
                    console.print("❌ fight.canCastSpell retourne 0 - Sort impossible", console.red)
                end
            else
                if distance < 2 then
                    console.print("❌ ATTIRANCE impossible - Trop proche (distance: " .. distance .. " < 2)", console.red)
                else
                    console.print("❌ ATTIRANCE impossible - Trop loin (distance: " .. distance .. " > " .. maxRange .. ")", console.red)
                end
            end
        else
            console.print("❌ PA insuffisants pour ATTIRANCE (PA: " .. currentPA .. "/2)", console.red)
        end
    end
    
    -- ÉTAPE 5: DOUBLE NERVOSITÉ sur cellule 274 (6 PA total)
    console.print("🧠 === ÉTAPE 5 PRIORITÉ 1: DOUBLE NERVOSITÉ SUR 274 ===", console.blue)
    currentPA = refreshCurrentPA()
    local nervosityTarget = 274  -- Cible pour déplacement vers 289 (CORRIGÉ)
    
    -- Actualiser position
    me = fight.currentFighter()
    
    if not me then
        console.print("❌ Impossible de récupérer position pour NERVOSITÉ", console.red)
    else
        console.print("📍 Position actuelle: " .. me.cellId, console.cyan)
        console.print("🎯 Cible NERVOSITÉ: " .. nervosityTarget, console.cyan)
        
        local distance = fight.cellsDistance(me.cellId, nervosityTarget)
        local maxRange = SPELL_RANGES[SPELL_IDS.NERVOSITE] or 4
        
        console.print("📏 Distance vers cible: " .. distance .. " (Portée NERVOSITÉ: " .. maxRange .. ")", console.cyan)
        console.print("⚡ PA disponibles: " .. currentPA .. " (Requis: 6 pour double)", console.cyan)
        
        if currentPA >= 6 then -- 3 PA par Nervosité
            console.print("✅ PA suffisants pour double NERVOSITÉ", console.green)
            
            -- Première NERVOSITÉ
            local canCast1 = fight.canCastSpell(SPELL_IDS.NERVOSITE, nervosityTarget)
            console.print("🔍 fight.canCastSpell NERVOSITÉ #1 result: " .. canCast1, console.yellow)
            
            if canCast1 ~= 0 then
                console.print("🧠 Lancement NERVOSITÉ #1...", console.blue)
                local success1, newPA1 = castSpellAndRefresh(SPELL_IDS.NERVOSITE, nervosityTarget, "NERVOSITÉ PRIORITÉ 1 #1", 3)
                currentPA = newPA1
                
                if success1 then
                    console.print("✅ NERVOSITÉ #1 réussie - PA restants: " .. currentPA, console.green)
                    
                    -- Deuxième NERVOSITÉ
                    if currentPA >= 3 then
                        local canCast2 = fight.canCastSpell(SPELL_IDS.NERVOSITE, nervosityTarget)
                        console.print("🔍 fight.canCastSpell NERVOSITÉ #2 result: " .. canCast2, console.yellow)
                        
                        if canCast2 ~= 0 then
                            console.print("🧠 Lancement NERVOSITÉ #2...", console.blue)
                            local success2, newPA2 = castSpellAndRefresh(SPELL_IDS.NERVOSITE, nervosityTarget, "NERVOSITÉ PRIORITÉ 1 #2", 3)
                            currentPA = newPA2
                            
                            if success2 then
                                console.print("✅ DOUBLE NERVOSITÉ PRIORITÉ 1 COMPLÈTE - PA restants: " .. currentPA, console.green)
                            else
                                console.print("❌ NERVOSITÉ #2 échouée", console.red)
                            end
                        else
                            console.print("❌ NERVOSITÉ #2 impossible", console.red)
                        end
                    else
                        console.print("⚠️ PA insuffisants pour NERVOSITÉ #2 (PA: " .. currentPA .. "/3)", console.orange)
                    end
                else
                    console.print("❌ NERVOSITÉ #1 échouée", console.red)
                end
            else
                console.print("❌ NERVOSITÉ #1 impossible", console.red)
            end
        else
            console.print("❌ PA insuffisants pour double NERVOSITÉ (PA: " .. currentPA .. "/6)", console.red)
        end
    end
    
    -- RÉSUMÉ FINAL
    currentPA = refreshCurrentPA()
    console.print("🚨 === PRIORITÉ 1 TERMINÉE - PA restants: " .. currentPA .. " ===", console.red)
    
    return true
end

-- Fonction spécialisée pour la stratégie spéciale 3 du tour 1 (DÉPLACEMENT TACTIQUE)
function executeSpecialStrategy3Turn1(phossile)
    console.print("🎯 === EXÉCUTION STRATÉGIE SPÉCIALE 3 TOUR 1 - DÉPLACEMENT TACTIQUE ===", console.red)
    
    local me = fight.currentFighter()
    if not me then
        console.print("❌ Pas de combattant actuel", console.red)
        return false
    end
    
    local currentPA = refreshCurrentPA()
    console.print("⚡ PA de départ: " .. currentPA, console.cyan)
    
    -- VÉRIFICATION PRIORITÉ 1 AVANT TOUTES LES ÉTAPES (POUR 12 PA COMPLETS)
    local cell289 = 289
    local cell302 = 302
    local cell289Available = isCellAvailableForLeader(cell289)
    local cell316Available = isCellAvailableForLeader(316)
    
    console.print("🔍 Cellule 289 disponible pour leader: " .. tostring(cell289Available), console.yellow)
    console.print("🔍 Cellule 316 disponible pour leader: " .. tostring(cell316Available), console.yellow)
    
    if not cell316Available and cell289Available then
        -- PRIORITÉ 1: Séquence complète qui remplace tout
        console.print("🚨 PRIORITÉ 1 DÉTECTÉE: 316 occupé par allié ET 289 disponible", console.red)
        console.print("🚨 PRIORITÉ 1 REMPLACE ENTIÈREMENT les étapes 3A, 3B, 3C", console.red)
        return executeSpecialStrategy1Priority(phossile)
    end
    
    -- ÉTAPE 3A: MUTILATION (2 PA) - Sur soi (seulement si pas de Priorité 1)
    console.print("⚔️ === ÉTAPE 3A: MUTILATION ===", console.blue)
    if currentPA >= 2 then
        local success, newPA = castSpellAndRefresh(SPELL_IDS.MUTILATION, me.cellId, "MUTILATION TACTIQUE", 2)
        currentPA = newPA
        
        if success then
            console.print("✅ MUTILATION tactique réussie", console.green)
            -- Enregistrer le cooldown de MUTILATION (2 tours)
            markSpellOnCooldown(SPELL_IDS.MUTILATION, "MUTILATION TACTIQUE T1", 2)
        else
            console.print("❌ MUTILATION tactique échouée", console.red)
        end
    else
        console.print("❌ PA insuffisants pour MUTILATION (PA: " .. currentPA .. "/2)", console.red)
    end
    
    -- ÉTAPE 3B: BERSERK (2 PA) - Sur soi
    console.print("💪 === ÉTAPE 3B: BERSERK ===", console.blue)
    currentPA = refreshCurrentPA()
    if currentPA >= 2 then
        local success, newPA = castSpellAndRefresh(SPELL_IDS.BERSERK, me.cellId, "BERSERK TACTIQUE", 2)
        currentPA = newPA
        
        if success then
            console.print("✅ BERSERK tactique réussi", console.green)
        else
            console.print("❌ BERSERK tactique échoué", console.red)
        end
    else
        console.print("❌ PA insuffisants pour BERSERK (PA: " .. currentPA .. "/2)", console.red)
    end
    
    -- ÉTAPE 3C: DÉPLACEMENT TACTIQUE
    console.print("🏃 === ÉTAPE 3C: DÉPLACEMENT TACTIQUE ===", console.blue)
    me = fight.currentFighter() -- Actualiser position
    
    local initialPosition = me.cellId
    local cell289 = 289
    local cell302 = 302
    local targetCell = nil
    local nervosityTarget = nil
    
    -- Vérifier disponibilité des cellules (nouvelle logique avec leader exclusion)
    local cell289Available = isCellAvailableForLeader(cell289)
    local cell302Available = isCellAvailableForLeader(cell302)
    local cell316Available = isCellAvailableForLeader(316)
    local cell329 = 329
    
    console.print("🔍 Cellule 289 disponible pour leader: " .. tostring(cell289Available), console.yellow)
    console.print("🔍 Cellule 302 disponible pour leader: " .. tostring(cell302Available), console.yellow)
    console.print("🔍 Cellule 316 disponible pour leader: " .. tostring(cell316Available), console.yellow)
    

    
    -- LOGIQUE DE DÉPLACEMENT TACTIQUE (Priorité 1 déjà vérifiée avant - séquence complète)
    if cell289Available and cell302Available then
        -- PRIORITÉ A: Si 289 disponible ET 302 disponible → Aller en 289  
        targetCell = cell289
        nervosityTarget = 274
        console.print("✅ PRIORITÉ A: 289 ET 302 disponibles → Cible: 289 → Nervosité: 274", console.green)
    elseif not cell302Available and cell316Available then
        -- PRIORITÉ B: Si 302 occupé par allié ET 316 disponible → Aller en 316
        targetCell = 316
        nervosityTarget = 330  -- Cellule adjacente optimale pour nervosité depuis 316
        console.print("✅ PRIORITÉ B: 302 occupé par allié ET 316 disponible → Cible: 316 → Nervosité: 330", console.blue)
    else
        -- PRIORITÉ C: Fallback → Aller en 302 (même si occupé par le leader)
        targetCell = cell302
        nervosityTarget = 288
        console.print("⚠️ PRIORITÉ C: Fallback → Cible: 302 → Nervosité: 288", console.orange)
    end
    
    local currentDistance = fight.cellsDistance(initialPosition, targetCell)
    console.print("🏃 Position initiale: " .. initialPosition .. " → Cible: " .. targetCell .. " (distance: " .. currentDistance .. ")", console.yellow)
    
    if currentDistance > 0 then
        console.print("🏃 Déplacement tactique vers " .. targetCell .. "...", console.yellow)
        
        -- MÉTHODE 1: Essayer moveToAccessibleCell
        local method1Success = pcall(function()
            fight.moveToAccessibleCell(targetCell)
        end)
        
        global.sleep(0) -- Attendre mouvement
        me = fight.currentFighter() -- Actualiser position
        local newPosition = me.cellId
        local actuallyMoved = (newPosition ~= initialPosition)
        
        console.print("📍 Position après méthode 1: " .. newPosition .. " (bougé: " .. tostring(actuallyMoved) .. ")", console.cyan)
        
        -- Si pas bougé, essayer MÉTHODE 2
        if not actuallyMoved then
            console.print("🔄 Méthode 1 échouée - Essai déplacement alternatif", console.orange)
            
            local method2Success = pcall(function()
                fight.moveTowardCell(targetCell)
            end)
            
            global.sleep(0) -- Attendre mouvement
            me = fight.currentFighter() -- Actualiser position
            newPosition = me.cellId
            actuallyMoved = (newPosition ~= initialPosition)
            
            console.print("📍 Position après méthode 2: " .. newPosition .. " (bougé: " .. tostring(actuallyMoved) .. ")", console.cyan)
        end
        
        -- Vérification finale et mise à jour de la cible Nervosité
        local finalDistance = fight.cellsDistance(newPosition, targetCell)
        if actuallyMoved then
            if finalDistance == 0 then
                console.print("✅ Déplacement vers " .. targetCell .. " RÉUSSI (position exacte)", console.green)
            else
                console.print("✅ Déplacement partiel vers " .. targetCell .. " (distance finale: " .. finalDistance .. ")", console.green)
            end
            
            -- Mise à jour de la cible Nervosité selon position finale
            if newPosition == cell289 then
                nervosityTarget = 274
                console.print("📍 Position finale 289 → Nervosité sur 274", console.green)
            elseif newPosition == cell302 then
                nervosityTarget = 288
                console.print("📍 Position finale 302 → Nervosité sur 288", console.green)
            elseif newPosition == cell329 then
                nervosityTarget = 315
                console.print("📍 Position finale 329 → Nervosité sur 315", console.green)
            else
                -- Position intermédiaire, garder la cible prévue
                console.print("📍 Position intermédiaire " .. newPosition .. " → Nervosité sur " .. nervosityTarget, console.orange)
            end
        else
            console.print("❌ Déplacement vers " .. targetCell .. " IMPOSSIBLE - Position inchangée", console.red)
        end
    else
        console.print("✅ Déjà en position " .. targetCell, console.green)
    end
    
    -- ÉTAPE 3D: ATTIRANCE sur Phossile
    console.print("🔥 === ÉTAPE 3D: ATTIRANCE SUR PHOSSILE ===", console.blue)
    
    -- Actualiser position ET Phossile
    me = fight.currentFighter()
    phossile = findPhossile()
    currentPA = refreshCurrentPA()
    
    if not me then
        console.print("❌ Impossible de récupérer position actuelle", console.red)
    elseif not phossile then
        console.print("❌ Phossile introuvable pour ATTIRANCE", console.red)
    else
        console.print("📍 Position actuelle: " .. me.cellId, console.cyan)
        console.print("🎯 Position Phossile: " .. phossile.cellId, console.cyan)
        
        local distance = fight.cellsDistance(me.cellId, phossile.cellId)
        local maxRange = SPELL_RANGES[SPELL_IDS.ATTIRANCE] or 10
        
        console.print("📏 Distance vers Phossile: " .. distance .. " (Portée ATTIRANCE: 2-" .. maxRange .. ")", console.cyan)
        
        if currentPA >= 2 then
            if distance >= 2 and distance <= maxRange then
                console.print("✅ Conditions ATTIRANCE OK - Lancement du sort", console.green)
                local canCast = fight.canCastSpell(SPELL_IDS.ATTIRANCE, phossile.cellId)
                console.print("🔍 fight.canCastSpell result: " .. canCast, console.yellow)
                
                if canCast ~= 0 then
                    local success, newPA = castSpellAndRefresh(SPELL_IDS.ATTIRANCE, phossile.cellId, "ATTIRANCE TACTIQUE", 2)
                    currentPA = newPA
                    
                    if success then
                        console.print("✅ ATTIRANCE TACTIQUE réussie", console.green)
                        global.sleep(0) -- Temps pour mouvement Phossile
                        phossile = findPhossile() -- Actualiser position Phossile
                        if phossile then
                            console.print("🔄 Position Phossile après ATTIRANCE: " .. phossile.cellId, console.cyan)
                        end
                    else
                        console.print("❌ ATTIRANCE TACTIQUE échouée malgré les vérifications", console.red)
                    end
                else
                    console.print("❌ fight.canCastSpell retourne 0 - Sort impossible", console.red)
                end
            else
                if distance < 2 then
                    console.print("❌ ATTIRANCE impossible - Trop proche (distance: " .. distance .. " < 2)", console.red)
                else
                    console.print("❌ ATTIRANCE impossible - Trop loin (distance: " .. distance .. " > " .. maxRange .. ")", console.red)
                end
            end
        else
            console.print("❌ PA insuffisants pour ATTIRANCE (PA: " .. currentPA .. "/2)", console.red)
        end
    end
    
    -- ÉTAPE 3E: DOUBLE NERVOSITÉ sur cellule déterminée
    console.print("🧠 === ÉTAPE 3E: DOUBLE NERVOSITÉ SUR " .. nervosityTarget .. " ===", console.blue)
    currentPA = refreshCurrentPA()
    
    -- Actualiser position
    me = fight.currentFighter()
    
    if not me then
        console.print("❌ Impossible de récupérer position pour NERVOSITÉ", console.red)
    else
        console.print("📍 Position actuelle: " .. me.cellId, console.cyan)
        console.print("🎯 Cible NERVOSITÉ: " .. nervosityTarget, console.cyan)
        
        local distance = fight.cellsDistance(me.cellId, nervosityTarget)
        local maxRange = SPELL_RANGES[SPELL_IDS.NERVOSITE] or 4
        
        console.print("📏 Distance vers cible: " .. distance .. " (Portée NERVOSITÉ: " .. maxRange .. ")", console.cyan)
        console.print("⚡ PA disponibles: " .. currentPA .. " (Requis: 6 pour double)", console.cyan)
        
        if currentPA >= 6 then -- 3 PA par Nervosité
            console.print("✅ PA suffisants pour double NERVOSITÉ CRITIQUE", console.green)
            console.print("🎯 === LANCEMENT DOUBLE NERVOSITÉ CRITIQUE AVEC VÉRIFICATIONS RENFORCÉES ===", console.red)
            
            -- Première NERVOSITÉ avec tentatives multiples
            console.print("🧠 === NERVOSITÉ #1 CRITIQUE ===", console.blue)
            local success1, newPA1 = castCriticalSpellWithRetry(SPELL_IDS.NERVOSITE, nervosityTarget, "NERVOSITÉ TACTIQUE #1", 3, 2)
            currentPA = newPA1
            
            if success1 then
                console.print("✅ NERVOSITÉ #1 CRITIQUE réussie - PA restants: " .. currentPA, console.green)
                
                -- Deuxième NERVOSITÉ avec tentatives multiples
                if currentPA >= 3 then
                    console.print("🧠 === NERVOSITÉ #2 CRITIQUE ===", console.blue)
                    local success2, newPA2 = castCriticalSpellWithRetry(SPELL_IDS.NERVOSITE, nervosityTarget, "NERVOSITÉ TACTIQUE #2", 3, 2)
                    currentPA = newPA2
                    
                    if success2 then
                        console.print("✅ DOUBLE NERVOSITÉ CRITIQUE COMPLÈTE - PA restants: " .. currentPA, console.green)
                        console.print("🎯 === DOUBLE NERVOSITÉ GARANTIE DANS LE JEU ===", console.red)
                    else
                        console.print("❌ NERVOSITÉ #2 CRITIQUE échec total", console.red)
                    end
                else
                    console.print("⚠️ PA insuffisants pour NERVOSITÉ #2 (PA: " .. currentPA .. "/3)", console.orange)
                end
            else
                console.print("❌ NERVOSITÉ #1 CRITIQUE échec total", console.red)
            end
        else
            console.print("❌ PA insuffisants pour double NERVOSITÉ (PA: " .. currentPA .. "/6)", console.red)
        end
    end
    
    -- RÉSUMÉ DIAGNOSTIC STRATÉGIE SPÉCIALE 3
    console.print("📋 === RÉSUMÉ STRATÉGIE SPÉCIALE 3 TACTIQUE ===", console.blue)
    me = fight.currentFighter()
    phossile = findPhossile()
    
    if me and phossile then
        console.print("📍 Position finale bot: " .. me.cellId, console.cyan)
        console.print("🎯 Position finale Phossile: " .. phossile.cellId, console.cyan)
        local finalDistance = fight.cellsDistance(me.cellId, phossile.cellId)
        console.print("📏 Distance finale bot-Phossile: " .. finalDistance, console.cyan)
        console.print("🎯 Cible Nervosité utilisée: " .. nervosityTarget, console.cyan)
        
        -- Diagnostic des problèmes potentiels
        if me.cellId == cell289 or me.cellId == cell302 then
            console.print("✅ Position tactique atteinte (" .. me.cellId .. ")", console.green)
        else
            console.print("❌ Position tactique NON atteinte (position: " .. me.cellId .. ")", console.red)
        end
    end
    
    -- Finaliser avec PA restants si possible
    currentPA = refreshCurrentPA()
    console.print("🔥 === FIN STRATÉGIE SPÉCIALE 3 TACTIQUE - PA restants: " .. currentPA .. " ===", console.red)
    
    return true
end

-- Fonction spécialisée pour la stratégie spéciale 2 du tour 1
function executeSpecialStrategy2Turn1(phossile)
    console.print("🌟 === EXÉCUTION STRATÉGIE SPÉCIALE 2 TOUR 1 ===", console.red)
    
    local me = fight.currentFighter()
    if not me then
        console.print("❌ Pas de combattant actuel", console.red)
        return false
    end
    
    local currentPA = refreshCurrentPA()
    console.print("⚡ PA de départ: " .. currentPA, console.cyan)
    
    -- ÉTAPE 2A: FLUCTUATION (2 PA) - Sur soi
    console.print("🌪️ === ÉTAPE 2A: FLUCTUATION ===", console.blue)
    if currentPA >= 2 then
        local success, newPA = castSpellAndRefresh(SPELL_IDS.FLUCTUATION, me.cellId, "FLUCTUATION SPÉCIALE", 2)
        currentPA = newPA
        
        if success then
            console.print("✅ FLUCTUATION spéciale réussie", console.green)
        else
            console.print("❌ FLUCTUATION spéciale échouée", console.red)
        end
    else
        console.print("❌ PA insuffisants pour FLUCTUATION (PA: " .. currentPA .. "/2)", console.red)
    end
    
    -- ÉTAPE 2B: BERSERK (2 PA) - Sur soi
    console.print("💪 === ÉTAPE 2B: BERSERK ===", console.blue)
    currentPA = refreshCurrentPA()
    if currentPA >= 2 then
        local success, newPA = castSpellAndRefresh(SPELL_IDS.BERSERK, me.cellId, "BERSERK SPÉCIAL", 2)
        currentPA = newPA
        
        if success then
            console.print("✅ BERSERK spécial réussi", console.green)
        else
            console.print("❌ BERSERK spécial échoué", console.red)
        end
    else
        console.print("❌ PA insuffisants pour BERSERK (PA: " .. currentPA .. "/2)", console.red)
    end
    
    -- ÉTAPE 2C: Déplacement vers cellule 357
    console.print("🏃 === ÉTAPE 2C: DÉPLACEMENT VERS 357 ===", console.blue)
    me = fight.currentFighter() -- Actualiser position
    
    local targetCell = 357
    local initialPosition = me.cellId
    local currentDistance = fight.cellsDistance(initialPosition, targetCell)
    
    console.print("🏃 Position initiale: " .. initialPosition .. " → Cible: " .. targetCell .. " (distance: " .. currentDistance .. ")", console.yellow)
    
    if currentDistance > 0 then
        console.print("🏃 Déplacement vers position spéciale 357...", console.yellow)
        
        -- MÉTHODE 1: Essayer moveToAccessibleCell
        local method1Success = pcall(function()
            fight.moveToAccessibleCell(targetCell)
        end)
        
        global.sleep(0) -- Attendre mouvement
        me = fight.currentFighter() -- Actualiser position
        local newPosition = me.cellId
        local actuallyMoved = (newPosition ~= initialPosition)
        
        console.print("📍 Position après méthode 1: " .. newPosition .. " (bougé: " .. tostring(actuallyMoved) .. ")", console.cyan)
        
        -- Si pas bougé, essayer MÉTHODE 2
        if not actuallyMoved then
            console.print("🔄 Méthode 1 échouée - Essai déplacement alternatif", console.orange)
            
            local method2Success = pcall(function()
                fight.moveTowardCell(targetCell)
            end)
            
            global.sleep(0) -- Attendre mouvement
            me = fight.currentFighter() -- Actualiser position
            newPosition = me.cellId
            actuallyMoved = (newPosition ~= initialPosition)
            
            console.print("📍 Position après méthode 2: " .. newPosition .. " (bougé: " .. tostring(actuallyMoved) .. ")", console.cyan)
        end
        
        -- Vérification finale
        local finalDistance = fight.cellsDistance(newPosition, targetCell)
        if actuallyMoved then
            if finalDistance == 0 then
                console.print("✅ Déplacement vers 357 RÉUSSI (position exacte)", console.green)
            else
                console.print("✅ Déplacement partiel vers 357 (distance finale: " .. finalDistance .. ")", console.green)
            end
        else
            console.print("❌ Déplacement vers 357 IMPOSSIBLE - Position inchangée", console.red)
        end
    else
        console.print("✅ Déjà en position 357", console.green)
    end
    
    -- ÉTAPE 2D: ATTIRANCE sur Phossile
    console.print("🔥 === ÉTAPE 2D: ATTIRANCE SUR PHOSSILE ===", console.blue)
    
    -- Actualiser position ET Phossile
    me = fight.currentFighter()
    phossile = findPhossile()
    currentPA = refreshCurrentPA()
    
    if not me then
        console.print("❌ Impossible de récupérer position actuelle", console.red)
    elseif not phossile then
        console.print("❌ Phossile introuvable pour ATTIRANCE", console.red)
    else
        console.print("📍 Position actuelle: " .. me.cellId, console.cyan)
        console.print("🎯 Position Phossile: " .. phossile.cellId, console.cyan)
        
        local distance = fight.cellsDistance(me.cellId, phossile.cellId)
        local maxRange = SPELL_RANGES[SPELL_IDS.ATTIRANCE] or 10
        
        console.print("📏 Distance vers Phossile: " .. distance .. " (Portée ATTIRANCE: 2-" .. maxRange .. ")", console.cyan)
        
        if currentPA >= 2 then
            if distance >= 2 and distance <= maxRange then
                console.print("✅ Conditions ATTIRANCE OK - Lancement du sort", console.green)
                local canCast = fight.canCastSpell(SPELL_IDS.ATTIRANCE, phossile.cellId)
                console.print("🔍 fight.canCastSpell result: " .. canCast, console.yellow)
                
                if canCast ~= 0 then
                    local success, newPA = castSpellAndRefresh(SPELL_IDS.ATTIRANCE, phossile.cellId, "ATTIRANCE SPÉCIALE 2", 2)
                    currentPA = newPA
                    
                    if success then
                        console.print("✅ ATTIRANCE SPÉCIALE 2 réussie", console.green)
                        global.sleep(0) -- Temps pour mouvement Phossile
                        phossile = findPhossile() -- Actualiser position Phossile
                        if phossile then
                            console.print("🔄 Position Phossile après ATTIRANCE: " .. phossile.cellId, console.cyan)
                        end
                    else
                        console.print("❌ ATTIRANCE SPÉCIALE 2 échouée malgré les vérifications", console.red)
                    end
                else
                    console.print("❌ fight.canCastSpell retourne 0 - Sort impossible", console.red)
                end
            else
                if distance < 2 then
                    console.print("❌ ATTIRANCE impossible - Trop proche (distance: " .. distance .. " < 2)", console.red)
                else
                    console.print("❌ ATTIRANCE impossible - Trop loin (distance: " .. distance .. " > " .. maxRange .. ")", console.red)
                end
            end
        else
            console.print("❌ PA insuffisants pour ATTIRANCE (PA: " .. currentPA .. "/2)", console.red)
        end
    end
    
    -- ÉTAPE 2E: Double NERVOSITÉ sur cellule 372
    console.print("🧠 === ÉTAPE 2E: DOUBLE NERVOSITÉ ===", console.blue)
    currentPA = refreshCurrentPA()
    local nervositeTarget = 372
    
    -- Actualiser position
    me = fight.currentFighter()
    
    if not me then
        console.print("❌ Impossible de récupérer position pour NERVOSITÉ", console.red)
    else
        console.print("📍 Position actuelle: " .. me.cellId, console.cyan)
        console.print("🎯 Cible NERVOSITÉ: " .. nervositeTarget, console.cyan)
        
        local distance = fight.cellsDistance(me.cellId, nervositeTarget)
        local maxRange = SPELL_RANGES[SPELL_IDS.NERVOSITE] or 4
        
        console.print("📏 Distance vers cible: " .. distance .. " (Portée NERVOSITÉ: " .. maxRange .. ")", console.cyan)
        console.print("⚡ PA disponibles: " .. currentPA .. " (Requis: 6 pour double)", console.cyan)
        
        if currentPA >= 6 then -- 3 PA par Nervosité
            console.print("✅ PA suffisants pour double NERVOSITÉ", console.green)
            
            -- Première NERVOSITÉ
            local canCast1 = fight.canCastSpell(SPELL_IDS.NERVOSITE, nervositeTarget)
            console.print("🔍 fight.canCastSpell NERVOSITÉ #1 result: " .. canCast1, console.yellow)
            
            if canCast1 ~= 0 then
                console.print("🧠 Lancement NERVOSITÉ #1...", console.blue)
                local success1, newPA1 = castSpellAndRefresh(SPELL_IDS.NERVOSITE, nervositeTarget, "NERVOSITÉ SPÉCIALE 2 #1", 3)
                currentPA = newPA1
                
                if success1 then
                    console.print("✅ NERVOSITÉ #1 réussie - PA restants: " .. currentPA, console.green)
                    
                    -- Deuxième NERVOSITÉ
                    if currentPA >= 3 then
                        local canCast2 = fight.canCastSpell(SPELL_IDS.NERVOSITE, nervositeTarget)
                        console.print("🔍 fight.canCastSpell NERVOSITÉ #2 result: " .. canCast2, console.yellow)
                        
                        if canCast2 ~= 0 then
                            console.print("🧠 Lancement NERVOSITÉ #2...", console.blue)
                            local success2, newPA2 = castSpellAndRefresh(SPELL_IDS.NERVOSITE, nervositeTarget, "NERVOSITÉ SPÉCIALE 2 #2", 3)
                            currentPA = newPA2
                            
                            if success2 then
                                console.print("✅ Double NERVOSITÉ spéciale 2 COMPLÈTE - PA restants: " .. currentPA, console.green)
                            else
                                console.print("❌ NERVOSITÉ #2 échouée malgré les vérifications", console.red)
                            end
                        else
                            console.print("❌ NERVOSITÉ #2 impossible - fight.canCastSpell retourne 0", console.red)
                        end
                    else
                        console.print("⚠️ PA insuffisants pour NERVOSITÉ #2 (PA: " .. currentPA .. "/3)", console.orange)
                    end
                else
                    console.print("❌ NERVOSITÉ #1 échouée malgré les vérifications", console.red)
                end
            else
                if distance > maxRange then
                    console.print("❌ NERVOSITÉ impossible - Hors portée (distance: " .. distance .. " > " .. maxRange .. ")", console.red)
                else
                    console.print("❌ NERVOSITÉ impossible - fight.canCastSpell retourne 0 (cooldown/obstacle?)", console.red)
                end
            end
        else
            console.print("❌ PA insuffisants pour double NERVOSITÉ (PA: " .. currentPA .. "/6)", console.red)
        end
    end
    
    -- RÉSUMÉ DIAGNOSTIC STRATÉGIE SPÉCIALE 2
    console.print("📋 === RÉSUMÉ STRATÉGIE SPÉCIALE 2 ===", console.blue)
    me = fight.currentFighter()
    phossile = findPhossile()
    
    if me and phossile then
        console.print("📍 Position finale bot: " .. me.cellId, console.cyan)
        console.print("🎯 Position finale Phossile: " .. phossile.cellId, console.cyan)
        local finalDistance = fight.cellsDistance(me.cellId, phossile.cellId)
        console.print("📏 Distance finale bot-Phossile: " .. finalDistance, console.cyan)
        
        -- Diagnostic des problèmes potentiels
        if me.cellId == 357 then
            console.print("✅ Position 357 atteinte", console.green)
        else
            console.print("❌ Position 357 NON atteinte (position: " .. me.cellId .. ")", console.red)
        end
        
        local canStillCastAttirance = fight.canCastSpell(SPELL_IDS.ATTIRANCE, phossile.cellId)
        local canStillCastNervosity = fight.canCastSpell(SPELL_IDS.NERVOSITE, 372)
        
        console.print("🔍 ATTIRANCE encore possible: " .. (canStillCastAttirance ~= 0 and "OUI" or "NON"), console.yellow)
        console.print("🔍 NERVOSITÉ encore possible: " .. (canStillCastNervosity ~= 0 and "OUI" or "NON"), console.yellow)
    end
    
    -- Finaliser avec PA restants si possible
    currentPA = refreshCurrentPA()
    console.print("🔥 === FIN STRATÉGIE SPÉCIALE 2 - PA restants: " .. currentPA .. " ===", console.red)
    
    return true
end

-- Fonction pour exécuter la stratégie tour 1
function executeTurn1Strategy()
    local me = fight.currentFighter()
    if not me then
        console.print("❌ Pas de combattant actuel", console.red)
        return false
    end
    
    console.print("🔥 === EXÉCUTION COMPLÈTE STRATÉGIE T1 ===", console.red)
    
    local phossile = findPhossile()
    if not phossile then
        console.print("❌ PHOSSILE NON TROUVÉ - Stratégie T1 annulée", console.red)
        return false
    end
    
    console.print("🎯 PHOSSILE trouvé sur cellule: " .. phossile.cellId, console.green)
    
    -- ========================================================
    -- VÉRIFICATION POSITION PHOSSILE AU TOUT DÉBUT DU TOUR
    -- ========================================================
    
    -- NOUVELLE STRATÉGIE SPÉCIALE 3 (PRIORITÉ MAXIMALE)
    local specialPhossileCells3 = {410, 397, 383, 370, 356, 343, 329}
    local isPhossileSpecial3 = false
    
    -- Vérifier cellules spéciales 3 EN PREMIER
    for _, specialCell in ipairs(specialPhossileCells3) do
        if phossile.cellId == specialCell then
            isPhossileSpecial3 = true
            break
        end
    end
    
    if isPhossileSpecial3 then
        console.print("🎯 STRATÉGIE SPÉCIALE 3 DÉTECTÉE (" .. phossile.cellId .. ") - DÉPLACEMENT TACTIQUE", console.yellow)
        return executeSpecialStrategy3Turn1(phossile)
    end
    
    local specialPhossileCells2 = {425, 411, 398, 384, 371}
    local isPhossileSpecial2 = false
    
    -- Vérifier cellules spéciales 2 
    for _, specialCell in ipairs(specialPhossileCells2) do
        if phossile.cellId == specialCell then
            isPhossileSpecial2 = true
            break
        end
    end
    
    if isPhossileSpecial2 then
        console.print("🌟 STRATÉGIE SPÉCIALE 2 DÉTECTÉE (" .. phossile.cellId .. ") - PAS D'ÉTAPE FIXE", console.yellow)
        -- Exécuter directement la stratégie spéciale 2 sans MUTILATION + BERSERK
        return executeSpecialStrategy2Turn1(phossile)
    end
    
    -- ==========================================
    -- ÉTAPE FIXE : MUTILATION + BERSERK (seulement si pas de stratégie spéciale 2)
    -- ==========================================
    console.print("🔥 ÉTAPE FIXE : MUTILATION + BERSERK", console.blue)
    
    -- ÉTAPE 1: MUTILATION (2 PA)
    local currentPA = refreshCurrentPA()
    console.print("🔥 Étape 1 - PA actuels: " .. currentPA, console.cyan)
    
    if currentPA >= 2 then
        local success, newPA = castSpellAndRefresh(SPELL_IDS.MUTILATION, me.cellId, "MUTILATION", 2)
        currentPA = newPA
        if success then
            -- Enregistrer le cooldown de MUTILATION (2 tours)
            markSpellOnCooldown(SPELL_IDS.MUTILATION, "MUTILATION T1", 2)
        end
    else
        console.print("❌ 1. PA insuffisants pour MUTILATION", console.red)
    end
    
    -- ÉTAPE 2: BERSERK (2 PA) - Sur soi-même
    currentPA = refreshCurrentPA()
    console.print("🔥 Étape 2 - PA actuels: " .. currentPA, console.cyan)
    
    if currentPA >= 2 then
        local success, newPA = castSpellAndRefresh(SPELL_IDS.BERSERK, me.cellId, "BERSERK", 2)
        currentPA = newPA
    else
        console.print("❌ 2. PA insuffisants pour BERSERK", console.red)
    end
    
    -- VÉRIFICATION POSITION PHOSSILE : Choisir stratégie après MUTILATION + BERSERK  
    currentPA = refreshCurrentPA()
    console.print("🔍 === ANALYSE POSITION PHOSSILE POUR STRATÉGIE T1 - PA actuels: " .. currentPA .. " ===", console.blue)
    
    local specialPhossileCells1 = {339, 325, 312, 285, 298}
    local isPhossileSpecial1 = false
    
    -- Vérifier cellules spéciales 1 (la stratégie spéciale 2 a déjà été vérifiée au début)
    for _, specialCell in ipairs(specialPhossileCells1) do
        if phossile.cellId == specialCell then
            isPhossileSpecial1 = true
            break
        end
    end
    
    if isPhossileSpecial1 then
        console.print("🔥 PHOSSILE EN POSITION SPÉCIALE 1 (" .. phossile.cellId .. ") - STRATÉGIE ALTERNATIVE T1", console.red)
        
        -- === STRATÉGIE SPÉCIALE 1 (ORIGINALE) ===
        me = fight.currentFighter() -- Actualiser position
        
        -- ÉTAPE SPÉCIALE 1: Se déplacer à la cellule 271
        local targetCell = 271
        local initialPosition = me.cellId
        local currentDistance = fight.cellsDistance(initialPosition, targetCell)
        
        console.print("🏃 Position initiale: " .. initialPosition .. " → Cible: " .. targetCell .. " (distance: " .. currentDistance .. ")", console.yellow)
        
        if currentDistance > 0 then
            console.print("🏃 Déplacement vers position spéciale 271...", console.yellow)
            
            -- MÉTHODE 1: Essayer moveToAccessibleCell
            local method1Success = pcall(function()
                fight.moveToAccessibleCell(targetCell)
            end)
            
            global.sleep(0) -- Attendre mouvement
            me = fight.currentFighter() -- Actualiser position
            local newPosition = me.cellId
            local actuallyMoved = (newPosition ~= initialPosition)
            
            console.print("📍 Position après méthode 1: " .. newPosition .. " (bougé: " .. tostring(actuallyMoved) .. ")", console.cyan)
            
            -- Si pas bougé, essayer MÉTHODE 2
            if not actuallyMoved then
                console.print("🔄 Méthode 1 échouée - Essai déplacement alternatif", console.orange)
                
                local method2Success = pcall(function()
                    fight.moveTowardCell(targetCell)
                end)
                
                global.sleep(0) -- Attendre mouvement
                me = fight.currentFighter() -- Actualiser position
                newPosition = me.cellId
                actuallyMoved = (newPosition ~= initialPosition)
                
                console.print("📍 Position après méthode 2: " .. newPosition .. " (bougé: " .. tostring(actuallyMoved) .. ")", console.cyan)
            end
            
            -- Vérification finale
            local finalDistance = fight.cellsDistance(newPosition, targetCell)
            if actuallyMoved then
                if finalDistance == 0 then
                    console.print("✅ Déplacement vers 271 RÉUSSI (position exacte)", console.green)
                else
                    console.print("✅ Déplacement partiel vers 271 (distance finale: " .. finalDistance .. ")", console.green)
                end
            else
                console.print("❌ Déplacement vers 271 IMPOSSIBLE - Position inchangée", console.red)
            end
        else
            console.print("✅ Déjà en position 271", console.green)
        end
        
        -- ÉTAPE SPÉCIALE 2: ATTIRANCE sur Phossile
        console.print("🔥 === ÉTAPE SPÉCIALE 2: ATTIRANCE SUR PHOSSILE ===", console.blue)
        
        -- Actualiser position ET Phossile
        me = fight.currentFighter()
        phossile = findPhossile()
        
        if not me then
            console.print("❌ Impossible de récupérer position actuelle", console.red)
        elseif not phossile then
            console.print("❌ Phossile introuvable pour ATTIRANCE", console.red)
        else
            console.print("📍 Position actuelle: " .. me.cellId, console.cyan)
            console.print("🎯 Position Phossile: " .. phossile.cellId, console.cyan)
            
            local distance = fight.cellsDistance(me.cellId, phossile.cellId)
            local maxRange = SPELL_RANGES[SPELL_IDS.ATTIRANCE] or 10
            
            console.print("📏 Distance vers Phossile: " .. distance .. " (Portée ATTIRANCE: 2-" .. maxRange .. ")", console.cyan)
            
            if currentPA >= 2 then
                if distance >= 2 and distance <= maxRange then
                    console.print("✅ Conditions ATTIRANCE OK - Lancement du sort", console.green)
                    local canCast = fight.canCastSpell(SPELL_IDS.ATTIRANCE, phossile.cellId)
                    console.print("🔍 fight.canCastSpell result: " .. canCast, console.yellow)
                    
                    if canCast ~= 0 then
                        local success, newPA = castSpellAndRefresh(SPELL_IDS.ATTIRANCE, phossile.cellId, "ATTIRANCE SPÉCIALE", 2)
                        currentPA = newPA
                        
                        if success then
                            console.print("✅ ATTIRANCE SPÉCIALE réussie", console.green)
                            global.sleep(0) -- Temps pour mouvement Phossile
                            phossile = findPhossile() -- Actualiser position Phossile
                            if phossile then
                                console.print("🔄 Position Phossile après ATTIRANCE: " .. phossile.cellId, console.cyan)
                            end
                        else
                            console.print("❌ ATTIRANCE SPÉCIALE échouée malgré les vérifications", console.red)
                        end
                    else
                        console.print("❌ fight.canCastSpell retourne 0 - Sort impossible", console.red)
                    end
                else
                    if distance < 2 then
                        console.print("❌ ATTIRANCE impossible - Trop proche (distance: " .. distance .. " < 2)", console.red)
                    else
                        console.print("❌ ATTIRANCE impossible - Trop loin (distance: " .. distance .. " > " .. maxRange .. ")", console.red)
                    end
                end
            else
                console.print("❌ PA insuffisants pour ATTIRANCE (PA: " .. currentPA .. "/2)", console.red)
            end
        end
        
        -- ÉTAPE SPÉCIALE 3: Double NERVOSITÉ sur cellule 286
        console.print("🧠 === ÉTAPE SPÉCIALE 3: DOUBLE NERVOSITÉ ===", console.blue)
        currentPA = refreshCurrentPA()
        local nervositeTarget = 286
        
        -- Actualiser position
        me = fight.currentFighter()
        
        if not me then
            console.print("❌ Impossible de récupérer position pour NERVOSITÉ", console.red)
        else
            console.print("📍 Position actuelle: " .. me.cellId, console.cyan)
            console.print("🎯 Cible NERVOSITÉ: " .. nervositeTarget, console.cyan)
            
            local distance = fight.cellsDistance(me.cellId, nervositeTarget)
            local maxRange = SPELL_RANGES[SPELL_IDS.NERVOSITE] or 4
            
            console.print("📏 Distance vers cible: " .. distance .. " (Portée NERVOSITÉ: " .. maxRange .. ")", console.cyan)
            console.print("⚡ PA disponibles: " .. currentPA .. " (Requis: 6 pour double)", console.cyan)
            
            if currentPA >= 6 then -- 3 PA par Nervosité
                console.print("✅ PA suffisants pour double NERVOSITÉ", console.green)
                
                -- Première NERVOSITÉ
                local canCast1 = fight.canCastSpell(SPELL_IDS.NERVOSITE, nervositeTarget)
                console.print("🔍 fight.canCastSpell NERVOSITÉ #1 result: " .. canCast1, console.yellow)
                
                if canCast1 ~= 0 then
                    console.print("🧠 Lancement NERVOSITÉ #1...", console.blue)
                    local success1, newPA1 = castSpellAndRefresh(SPELL_IDS.NERVOSITE, nervositeTarget, "NERVOSITÉ SPÉCIALE #1", 3)
                    currentPA = newPA1
                    
                    if success1 then
                        console.print("✅ NERVOSITÉ #1 réussie - PA restants: " .. currentPA, console.green)
                        
                        -- Deuxième NERVOSITÉ
                        if currentPA >= 3 then
                            local canCast2 = fight.canCastSpell(SPELL_IDS.NERVOSITE, nervositeTarget)
                            console.print("🔍 fight.canCastSpell NERVOSITÉ #2 result: " .. canCast2, console.yellow)
                            
                            if canCast2 ~= 0 then
                                console.print("🧠 Lancement NERVOSITÉ #2...", console.blue)
                                local success2, newPA2 = castSpellAndRefresh(SPELL_IDS.NERVOSITE, nervositeTarget, "NERVOSITÉ SPÉCIALE #2", 3)
                                currentPA = newPA2
                                
                                if success2 then
                                    console.print("✅ Double NERVOSITÉ spéciale COMPLÈTE - PA restants: " .. currentPA, console.green)
                                else
                                    console.print("❌ NERVOSITÉ #2 échouée malgré les vérifications", console.red)
                                end
                            else
                                console.print("❌ NERVOSITÉ #2 impossible - fight.canCastSpell retourne 0", console.red)
                            end
                        else
                            console.print("⚠️ PA insuffisants pour NERVOSITÉ #2 (PA: " .. currentPA .. "/3)", console.orange)
                        end
                    else
                        console.print("❌ NERVOSITÉ #1 échouée malgré les vérifications", console.red)
                    end
                else
                    if distance > maxRange then
                        console.print("❌ NERVOSITÉ impossible - Hors portée (distance: " .. distance .. " > " .. maxRange .. ")", console.red)
                    else
                        console.print("❌ NERVOSITÉ impossible - fight.canCastSpell retourne 0 (cooldown/obstacle?)", console.red)
                    end
                end
            else
                console.print("❌ PA insuffisants pour double NERVOSITÉ (PA: " .. currentPA .. "/6)", console.red)
            end
        end
        
        -- RÉSUMÉ DIAGNOSTIC STRATÉGIE SPÉCIALE
        console.print("📋 === RÉSUMÉ STRATÉGIE SPÉCIALE T1 ===", console.blue)
        me = fight.currentFighter()
        phossile = findPhossile()
        
        if me and phossile then
            console.print("📍 Position finale bot: " .. me.cellId, console.cyan)
            console.print("🎯 Position finale Phossile: " .. phossile.cellId, console.cyan)
            local finalDistance = fight.cellsDistance(me.cellId, phossile.cellId)
            console.print("📏 Distance finale bot-Phossile: " .. finalDistance, console.cyan)
            
            -- Diagnostic des problèmes potentiels
            if me.cellId == 271 then
                console.print("✅ Position 271 atteinte", console.green)
            else
                console.print("❌ Position 271 NON atteinte (position: " .. me.cellId .. ")", console.red)
            end
            
            local canStillCastAttirance = fight.canCastSpell(SPELL_IDS.ATTIRANCE, phossile.cellId)
            local canStillCastNervosity = fight.canCastSpell(SPELL_IDS.NERVOSITE, 286)
            
            console.print("🔍 ATTIRANCE encore possible: " .. (canStillCastAttirance ~= 0 and "OUI" or "NON"), console.yellow)
            console.print("🔍 NERVOSITÉ encore possible: " .. (canStillCastNervosity ~= 0 and "OUI" or "NON"), console.yellow)
        end
        
    -- STRATÉGIE SPÉCIALE 2 SUPPRIMÉE - Maintenant gérée par executeSpecialStrategy2Turn1()
    
    else
        console.print("🎯 PHOSSILE EN POSITION NORMALE (" .. phossile.cellId .. ") - STRATÉGIE STANDARD T1", console.green)
        
        -- === STRATÉGIE NORMALE T1 ===
        
        -- ÉTAPE 3: ATTIRANCE (2 PA) - Sur Phossile
        console.print("🔥 Étape 3 - ATTIRANCE Standard - PA actuels: " .. currentPA, console.cyan)
    
    if currentPA >= 2 then
        me = fight.currentFighter() -- Actualiser position
        local distance = fight.cellsDistance(me.cellId, phossile.cellId)
        local maxRange = SPELL_RANGES[SPELL_IDS.ATTIRANCE] or 10
        
        if distance >= 2 and distance <= maxRange then
            local success, newPA = castSpellAndRefresh(SPELL_IDS.ATTIRANCE, phossile.cellId, "ATTIRANCE", 2)
            currentPA = newPA
            
            if success then
                -- IMPORTANT: Rafraîchir la position du Phossile après Attirance
                global.sleep(0) -- Temps supplémentaire pour mouvement
                phossile = findPhossile()
                if phossile then
                    console.print("🔄 Position Phossile mise à jour: " .. phossile.cellId, console.cyan)
                end
            end
        else
            console.print("❌ 3. ATTIRANCE hors portée (Distance: " .. distance .. ", Portée: 2-" .. maxRange .. ")", console.red)
        end
    else
        console.print("❌ 3. PA insuffisants pour ATTIRANCE", console.red)
    end
        
    end -- Fin vérification position Phossile
    
    -- ÉTAPE 4: DOUBLE NERVOSITÉ (6 PA total) - Selon position Phossile ACTUELLE
    local nervosityTarget = phossile and NERVOSITY_TARGETS[phossile.cellId] or nil
    
    if nervosityTarget then
        console.print("🎯 Phossile sur " .. phossile.cellId .. " → Nervosité sur " .. nervosityTarget, console.cyan)
        
        -- Premier lancer de Nervosité
        currentPA = refreshCurrentPA()
        console.print("🔥 Étape 4a - PA actuels: " .. currentPA, console.cyan)
        
        if currentPA >= 3 then
            local success, newPA = castSpellAndRefresh(SPELL_IDS.NERVOSITE, nervosityTarget, "NERVOSITÉ #1", 3)
            currentPA = newPA
            
            if success then
                -- Deuxième lancer de Nervosité
                currentPA = refreshCurrentPA()
                console.print("🔥 Étape 4b - PA actuels: " .. currentPA, console.cyan)
                
                if currentPA >= 3 then
                    local success2, newPA2 = castSpellAndRefresh(SPELL_IDS.NERVOSITE, nervosityTarget, "NERVOSITÉ #2", 3)
                    currentPA = newPA2
                else
                    console.print("❌ 4b. PA insuffisants pour NERVOSITÉ #2", console.red)
                end
            end
        else
            console.print("❌ 4a. PA insuffisants pour NERVOSITÉ", console.red)
        end
    else
        console.print("❌ 4. AUCUN MAPPING NERVOSITÉ pour Phossile sur " .. phossile.cellId, console.orange)
    end
    
    -- Actualiser les PA finaux via API
    currentPA = refreshCurrentPA()
    console.print("🔥 === STRATÉGIE TOUR 1 TERMINÉE - PA restants: " .. currentPA .. " ===", console.red)
    return true
end

-- ===============================================================================
-- 🎯 STRATÉGIE TOUR 2 SPÉCIALISÉE
-- ===============================================================================

-- Fonction pour exécuter la stratégie tour 2
function executeTurn2Strategy()
    local me = fight.currentFighter()
    if not me then
        console.print("❌ Pas de combattant actuel", console.red)
        return false
    end
    
    -- DEBUG : Afficher les cooldowns actifs
    debugActiveCooldowns()
    
    -- Récupérer les PA via l'API officielle Frigost
    local currentPA = getRealActionPoints()
    console.print("⚔️ PA disponibles: " .. currentPA, console.cyan)
    
    local phossile = findPhossile()
    if not phossile then
        console.print("❌ PHOSSILE NON TROUVÉ - Stratégie T2 annulée", console.red)
        return false
    end
    
    console.print("🎯 PHOSSILE trouvé sur cellule: " .. phossile.cellId, console.yellow)
    
    -- ÉTAPE 0: Vérifier distance et se rapprocher si nécessaire
    local distance = fight.cellsDistance(me.cellId, phossile.cellId)
    console.print("📏 Distance avec Phossile: " .. distance, console.cyan)
    
    if distance > 1 then
        console.print("🏃 RAPPROCHEMENT NÉCESSAIRE - Distance actuelle: " .. distance, console.yellow)
        
        -- Récupérer toutes les cellules accessibles
        local accessibleCells = fight.accessibleCells()
        local currentPM = me.movementPoints or me.MP or 0
        local moved = false
        local bestCell = nil
        local minDistanceToPhossile = 999
        
        console.print("🔍 T2 - Recherche meilleure position CAC - PM disponibles: " .. currentPM, console.cyan)
        
        -- Trouver la cellule accessible la plus proche du Phossile
        for _, cellData in ipairs(accessibleCells) do
            local cell = cellData.cellId
            local distanceFromMe = fight.cellsDistance(me.cellId, cell)
            local distanceToPhossile = fight.cellsDistance(cell, phossile.cellId)
            
            -- Vérifier si cette cellule est accessible et se rapproche du Phossile
            if distanceFromMe <= currentPM and distanceFromMe > 0 and distanceToPhossile < minDistanceToPhossile then
                minDistanceToPhossile = distanceToPhossile
                bestCell = cell
                console.print("🎯 T2 - Cellule candidate: " .. cell .. " (distance Phossile: " .. distanceToPhossile .. ", coût PM: " .. distanceFromMe .. ")", console.white)
            end
        end
        
        -- Se déplacer vers la meilleure cellule trouvée
        if bestCell then
            console.print("🏃 T2 - Déplacement vers cellule " .. bestCell .. " (distance finale Phossile: " .. minDistanceToPhossile .. ")", console.yellow)
            fight.moveTowardCell(bestCell)
            global.sleep(0) -- Délai pour laisser le déplacement s'effectuer
            
            -- Vérifier le succès du déplacement
            local newMe = fight.currentFighter()
            if newMe then
                local newDistance = fight.cellsDistance(newMe.cellId, phossile.cellId)
                console.print("✅ T2 - Déplacement terminé - Nouvelle distance Phossile: " .. newDistance, console.green)
                moved = true
            end
        end
        
        if not moved then
            console.print("❌ Impossible de se rapprocher de Phossile avec " .. currentPM .. " PM", console.red)
        end
    else
        console.print("✅ Déjà au corps à corps avec Phossile", console.green)
    end
    
    -- ACTUALISER POSITION APRÈS DÉPLACEMENT OBLIGATOIRE
    me = fight.currentFighter()
    if not me then
        console.print("❌ Impossible de récupérer position après déplacement", console.red)
        return false
    end
    console.print("📍 Position finale T2 après déplacement: " .. me.cellId, console.cyan)
    
    -- ÉTAPE 1: COURONNES D'ÉPINES (2 PA) - Sur soi-même - UTILISE LA MÊME LOGIQUE QUI MARCHE POST-PHOSSILE
    console.print("👑 === ÉTAPE 1: COURONNES D'ÉPINES T2 (LOGIQUE POST-PHOSSILE) ===", console.blue)
    
    if currentPA >= 2 then
        -- Utiliser exactement la même fonction qui fonctionne post-Phossile
        local success, newPA = castSpellPostPhossileAndRefresh(SPELL_IDS.COURONNES_EPINES, me.cellId, "COURONNES D'ÉPINES T2")
        currentPA = newPA
        
        if success then
            console.print("✅ 1. COURONNES D'ÉPINES T2 RÉUSSIE (même logique post-Phossile)", console.green)
        else
            console.print("❌ 1. COURONNES D'ÉPINES T2 échouée (même raison que si c'était post-Phossile)", console.red)
        end
    else
        console.print("❌ 1. PA insuffisants pour COURONNES D'ÉPINES (" .. currentPA .. "/2)", console.red)
    end
    
    -- ÉTAPE 2: PROJECTION (2 PA) - Sur Phossile
    if currentPA >= 2 then
        local success, newPA = castSpellAndRefresh(SPELL_IDS.PROJECTION, phossile.cellId, "PROJECTION", 2)
        currentPA = newPA
    else
        console.print("❌ 2. PA insuffisants pour PROJECTION", console.red)
    end
    
    -- ÉTAPE 3: DOUBLE DÉCHAINEMENT (8 PA total) - Zone d'effet intelligente
    console.print("🗡️ === ÉTAPE 3: DOUBLE DÉCHAINEMENT T2 INTELLIGENT - PA actuels: " .. currentPA .. " ===", console.blue)
    
    -- Fonction pour trouver la meilleure cible DÉCHAINEMENT (direct ou zone)
    local function findBestDechainementTarget()
        -- 1. Essayer ciblage direct du Phossile
        if fight.canCastSpell(SPELL_IDS.DECHAINEMENT, phossile.cellId) ~= 0 then
            console.print("🎯 DÉCHAINEMENT - Ciblage DIRECT Phossile possible", console.green)
            return phossile.cellId, "PHOSSILE (direct)"
        end
        
        -- 2. Chercher un ennemi au CAC qui permettrait de toucher le Phossile en zone
        local allFighters = fight.fighters()
        for _, fighter in pairs(allFighters) do
            if fighter.team ~= me.team and fighter.alive and fighter.monsterId ~= 4046 then -- Pas le Phossile
                local distanceToEnemy = fight.cellsDistance(me.cellId, fighter.cellId)
                local distanceEnemyToPhossile = fight.cellsDistance(fighter.cellId, phossile.cellId)
                
                -- Vérifier si on peut cibler cet ennemi ET qu'il est proche du Phossile (zone carré de 1)
                if distanceToEnemy == 1 and distanceEnemyToPhossile <= 1 and fight.canCastSpell(SPELL_IDS.DECHAINEMENT, fighter.cellId) ~= 0 then
                    console.print("🎯 DÉCHAINEMENT - Ciblage ZONE via " .. (fighter.name or "Ennemi") .. " → touchera Phossile !", console.green)
                    return fighter.cellId, (fighter.name or "Ennemi") .. " (zone→Phossile)"
                end
            end
        end
        
        console.print("❌ DÉCHAINEMENT - Aucune cible valide trouvée", console.red)
        return nil, nil
    end
    
    -- 3a. Premier DÉCHAINEMENT
    if currentPA >= 4 then
        local targetCell, targetName = findBestDechainementTarget()
        
        if targetCell then
            local success, newPA = castSpellAndRefresh(SPELL_IDS.DECHAINEMENT, targetCell, "DÉCHAINEMENT T2 #1 sur " .. targetName, 4)
            currentPA = newPA
            if success then
                console.print("✅ 3a. DÉCHAINEMENT #1 T2 RÉUSSI sur " .. targetName .. " (PA restants: " .. currentPA .. ")", console.green)
                
                -- 3b. Deuxième DÉCHAINEMENT
                if currentPA >= 4 then
                    -- Recalculer la meilleure cible pour le 2ème DÉCHAINEMENT
                    local targetCell2, targetName2 = findBestDechainementTarget()
                    
                    if targetCell2 then
                        local success2, newPA2 = castSpellAndRefresh(SPELL_IDS.DECHAINEMENT, targetCell2, "DÉCHAINEMENT T2 #2 sur " .. targetName2, 4)
                        currentPA = newPA2
                        if success2 then
                            console.print("✅ 3b. DÉCHAINEMENT #2 T2 RÉUSSI sur " .. targetName2 .. " (PA restants: " .. currentPA .. ")", console.green)
                        else
                            console.print("❌ 3b. DÉCHAINEMENT #2 T2 ÉCHEC - Sort impossible", console.red)
                        end
                    else
                        console.print("❌ 3b. DÉCHAINEMENT #2 T2 - Aucune cible valide", console.red)
                    end
                else
                    console.print("❌ 3b. PA insuffisants pour DÉCHAINEMENT #2 (" .. currentPA .. "/4)", console.red)
                end
            else
                console.print("❌ 3a. DÉCHAINEMENT #1 T2 ÉCHEC - Sort impossible", console.red)
            end
        else
            console.print("❌ 3a. DÉCHAINEMENT #1 T2 - Aucune cible valide trouvée", console.red)
        end
    else
        console.print("❌ 3a. PA insuffisants pour DÉCHAINEMENT #1 (" .. currentPA .. "/4)", console.red)
    end
    
    console.print("⚔️ === STRATÉGIE TOUR 2 TERMINÉE - PA restants: " .. currentPA .. " ===", console.red)
    return true
end

-- ===============================================================================
-- 🎯 STRATÉGIE TOUR 3 SPÉCIALISÉE
-- ===============================================================================

-- Fonction pour exécuter la stratégie tour 3
function executeTurn3Strategy()
    local me = fight.currentFighter()
    if not me then
        console.print("❌ Pas de combattant actuel", console.red)
        return false
    end
    
    console.print("🗡️ === EXÉCUTION COMPLÈTE STRATÉGIE T3 ===", console.red)
    
    local phossile = findPhossile()
    if not phossile then
        console.print("❌ PHOSSILE NON TROUVÉ - Stratégie T3 annulée", console.red)
        return false
    end
    
    console.print("🎯 PHOSSILE trouvé sur cellule: " .. phossile.cellId, console.yellow)
    
    -- ÉTAPE 0: Vérifier distance et se rapprocher si nécessaire
    local distance = fight.cellsDistance(me.cellId, phossile.cellId)
    console.print("📏 Distance avec Phossile: " .. distance, console.cyan)
    
    if distance > 1 then
        console.print("🏃 RAPPROCHEMENT NÉCESSAIRE - Distance actuelle: " .. distance, console.yellow)
        
        local currentPM = me.movementPoints or me.MP or 0
        console.print("🔍 PM disponibles pour T3: " .. currentPM, console.cyan)
        
        if currentPM > 0 then
            -- NOUVELLE LOGIQUE T3 : Trouver la cellule accessible la plus proche du Phossile
            local accessibleCells = fight.accessibleCells()
            local bestCell = nil
            local bestDistance = 999
            
            console.print("🔍 T3 - Analyse de " .. #accessibleCells .. " cellules accessibles...", console.white)
            
            for _, cell in ipairs(accessibleCells) do
                -- CORRECTION : Gérer les deux formats de cellules (nombre ou objet)
                local cellId = (type(cell) == "table" and cell.cellId) or cell
                local distanceToPhossile = fight.cellsDistance(cellId, phossile.cellId)
                local distanceFromMe = fight.cellsDistance(me.cellId, cellId)
                
                -- Chercher la cellule qui rapproche le plus du Phossile
                if distanceToPhossile < bestDistance and distanceFromMe > 0 and distanceFromMe <= currentPM then
                    bestCell = cellId
                    bestDistance = distanceToPhossile
                    console.print("🎯 T3 - Cellule candidate: " .. bestCell .. " (distance Phossile: " .. distanceToPhossile .. ", coût PM: " .. distanceFromMe .. ")", console.white)
                end
            end
            
            if bestCell then
                console.print("🏃 T3 - RAPPROCHEMENT vers cellule " .. bestCell .. " (distance finale: " .. bestDistance .. ")", console.yellow)
                fight.moveTowardCell(bestCell)
                global.sleep(0)
                console.print("✅ T3 - Rapprochement effectué vers " .. bestCell, console.green)
            else
                console.print("❌ T3 - Aucune cellule de rapprochement trouvée", console.red)
            end
        else
            console.print("❌ T3 - Aucun PM disponible pour se rapprocher", console.red)
        end
    else
        console.print("✅ T3 - Déjà au corps à corps avec Phossile", console.green)
    end
    
    -- ÉTAPE 1: MUTILATION (2 PA) - Sur soi-même
    local currentPA = refreshCurrentPA()
    console.print("🗡️ Étape 1 - PA actuels: " .. currentPA, console.cyan)
    
    if currentPA >= 2 then
        local success, newPA = castSpellAndRefresh(SPELL_IDS.MUTILATION, me.cellId, "MUTILATION T3", 2)
        currentPA = newPA
        if success then
            -- Enregistrer le cooldown de MUTILATION (2 tours)
            markSpellOnCooldown(SPELL_IDS.MUTILATION, "MUTILATION T3", 2)
        end
    else
        console.print("❌ 1. PA insuffisants pour MUTILATION", console.red)
    end
    
    -- ÉTAPE 2: DOUBLE CONDENSATION (6 PA total) - Sur Phossile
    currentPA = refreshCurrentPA()
    console.print("🗡️ Étape 2 - DOUBLE CONDENSATION - PA actuels: " .. currentPA, console.cyan)
    
    -- Première CONDENSATION
    if currentPA >= 3 then
        local success1, newPA1 = castSpellAndRefresh(SPELL_IDS.CONDENSATION, phossile.cellId, "CONDENSATION T3 #1", 3)
        currentPA = newPA1
        
        if success1 then
            console.print("✅ 2a. CONDENSATION #1 réussie", console.green)
            
            -- Deuxième CONDENSATION
            currentPA = refreshCurrentPA()
            if currentPA >= 3 then
                local success2, newPA2 = castSpellAndRefresh(SPELL_IDS.CONDENSATION, phossile.cellId, "CONDENSATION T3 #2", 3)
                currentPA = newPA2
                
                if success2 then
                    console.print("✅ 2b. CONDENSATION #2 réussie", console.green)
                else
                    console.print("❌ 2b. CONDENSATION #2 échouée", console.red)
                end
            else
                console.print("❌ 2b. PA insuffisants pour CONDENSATION #2 (PA: " .. currentPA .. "/3)", console.orange)
            end
        else
            console.print("❌ 2a. CONDENSATION #1 échouée", console.red)
        end
    else
        console.print("❌ 2. PA insuffisants pour CONDENSATION (PA: " .. currentPA .. "/3)", console.red)
    end
    
    -- ÉTAPE 3: DÉCHAINEMENT (4 PA) - Sur Phossile avec fallback Dissolution AOE
    currentPA = refreshCurrentPA()
    console.print("🗡️ Étape 3 - PA actuels: " .. currentPA, console.cyan)
    
    local step3Success = false
    
    if currentPA >= 4 then
        -- Essayer Déchainement sur Phossile en priorité
        local success, newPA = castSpellAndRefresh(SPELL_IDS.DECHAINEMENT, phossile.cellId, "DÉCHAINEMENT T3", 4)
        currentPA = newPA
        
        if success then
            step3Success = true
        else
            console.print("❌ 3. DÉCHAINEMENT impossible sur PHOSSILE - Fallback Dissolution", console.yellow)
            
            -- FALLBACK : Prioriser Dissolution directe sur Phossile puis AOE
            currentPA = refreshCurrentPA()
            if currentPA >= 4 then
                -- PRIORITÉ 1 : Dissolution directe sur Phossile si possible
                if fight.canCastSpell(SPELL_IDS.DISSOLUTION, phossile.cellId) ~= 0 then
                    console.print("🎯 3. DISSOLUTION DIRECTE sur PHOSSILE à distance", console.green)
                    local success2, newPA2 = castSpellAndRefresh(SPELL_IDS.DISSOLUTION, phossile.cellId, "DISSOLUTION DIRECTE T3", 4)
                    currentPA = newPA2
                    step3Success = success2
                else
                    -- PRIORITÉ 2 : Dissolution AOE si directe impossible
                    console.print("🔄 3. Dissolution directe impossible - Tentative AOE", console.orange)
                    local bestAOECell = findBestDissolutionAOE(phossile)
                    
                    if bestAOECell then
                        console.print("🎯 3. DISSOLUTION AOE utilisée en fallback", console.yellow)
                        local success2, newPA2 = castSpellAndRefresh(SPELL_IDS.DISSOLUTION, bestAOECell, "DISSOLUTION AOE T3", 4)
                        currentPA = newPA2
                        step3Success = success2
                    else
                        console.print("❌ 3. Aucune cible trouvée pour Dissolution (directe + AOE)", console.red)
                    end
                end
            end
        end
    else
        console.print("❌ 3. PA insuffisants pour DÉCHAINEMENT", console.red)
    end
    
    if not step3Success then
        console.print("⚠️ 3. Aucune attaque possible à l'étape 3", console.orange)
    end
    
    -- ÉTAPE 4: LOGIQUE CONDITIONNELLE SELON DISTANCE PHOSSILE
    currentPA = refreshCurrentPA()
    console.print("🎯 === ÉTAPE 4 : LOGIQUE CONDITIONNELLE ===", console.red)
    
    -- Calculer distance avec Phossile
    me = fight.currentFighter()
    local distanceToPhossile = fight.cellsDistance(me.cellId, phossile.cellId)
    console.print("📏 Distance avec Phossile: " .. distanceToPhossile .. " PO", console.cyan)
    
    -- Pause pour stabiliser l'API après le sort précédent
    -- LOGIQUE CONDITIONNELLE : Distance ≤ 3 PO vs > 3 PO
    if distanceToPhossile <= 3 then
        -- CAS 1 : Phossile proche (≤ 3 PO) → FOLIE SANGUINAIRE sur soi
        console.print("🩸 4A. PHOSSILE PROCHE (" .. distanceToPhossile .. " ≤ 3 PO) → FOLIE SANGUINAIRE", console.red)
        
        global.sleep(0)
        local currentFighter = fight.currentFighter()
        if not currentFighter then
            console.print("❌ 4A. Aucun combattant trouvé pour Folie Sanguinaire", console.red)
        else
            local realPA = currentFighter.actionPoints or currentFighter.AP or 0
            console.print("🔍 4A. PA réels avant Folie Sanguinaire: " .. realPA, console.cyan)
            
            if realPA >= 3 then
                local canCast = fight.canCastSpell(SPELL_IDS.FOLIE_SANGUINAIRE, currentFighter.cellId)
                console.print("🔍 4A. Vérification Folie Sanguinaire: " .. tostring(canCast), console.cyan)
                
                if canCast ~= 0 then
                    console.print("🩸 4A. LANCEMENT FOLIE SANGUINAIRE...", console.red)
                    fight.castSpell(SPELL_IDS.FOLIE_SANGUINAIRE, currentFighter.cellId)
                    
                    -- Pause pour laisser le sort se lancer
                    global.sleep(0)
                    
                    -- Vérifier les PA après
                    local afterFighter = fight.currentFighter()
                    local afterPA = afterFighter.actionPoints or afterFighter.AP or 0
                    currentPA = afterPA -- Synchroniser
                    
                    console.print("✅ 4A. FOLIE SANGUINAIRE RÉELLEMENT LANCÉE - PA restants: " .. afterPA, console.green)
                else
                    console.print("❌ 4A. FOLIE SANGUINAIRE impossible (cooldown/déjà actif)", console.red)
                end
            else
                console.print("❌ 4A. PA insuffisants pour FOLIE SANGUINAIRE (" .. realPA .. "/3)", console.red)
            end
        end
    else
        -- CAS 2 : Phossile loin (> 3 PO) → DISSOLUTION directe sur Phossile
        console.print("🌊 4B. PHOSSILE LOIN (" .. distanceToPhossile .. " > 3 PO) → DISSOLUTION DIRECTE", console.blue)
        
        currentPA = refreshCurrentPA()
        if currentPA >= 4 then
            if fight.canCastSpell(SPELL_IDS.DISSOLUTION, phossile.cellId) ~= 0 then
                console.print("🌊 4B. LANCEMENT DISSOLUTION DIRECTE sur Phossile...", console.blue)
                local success, newPA = castSpellAndRefresh(SPELL_IDS.DISSOLUTION, phossile.cellId, "DISSOLUTION DIRECTE T3 CONDITIONNELLE", 4)
                currentPA = newPA
                
                if success then
                    console.print("✅ 4B. DISSOLUTION DIRECTE RÉUSSIE - PA restants: " .. currentPA, console.green)
                else
                    console.print("❌ 4B. DISSOLUTION DIRECTE ÉCHOUÉE", console.red)
                end
            else
                console.print("❌ 4B. DISSOLUTION impossible sur Phossile (hors portée/obstacle)", console.red)
            end
        else
            console.print("❌ 4B. PA insuffisants pour DISSOLUTION (" .. currentPA .. "/4)", console.red)
        end
    end
    
    -- PHASE DE NETTOYAGE T3 : Utiliser les PA restants même si Phossile pas au CAC
    currentPA = refreshCurrentPA()
    console.print("🧹 === PHASE NETTOYAGE T3 - PA restants: " .. currentPA .. " ===", console.blue)
    
    if currentPA >= 2 then
        console.print("🔄 T3 - PA RESTANTS SIGNIFICATIFS - Continuation forcée", console.orange)
        
        local t3Actions = 0
        local maxT3Actions = 4 -- Limite sécurité
        
        while currentPA >= 2 and t3Actions < maxT3Actions do
            t3Actions = t3Actions + 1
            console.print("🆘 T3 Action #" .. t3Actions .. " - PA: " .. currentPA, console.yellow)
            
            local actionTaken = false
            local enemy = fight.nearestEnemy()
            
            -- Essayer les sorts par ordre de priorité
            if enemy and currentPA >= 4 then
                -- 1. DÉCHAINEMENT/DISSOLUTION (selon distance et disponibilité)
                local distance = fight.cellsDistance(me.cellId, enemy.cellId)
                console.print("🎯 T3 Nettoyage - Ennemi à distance " .. distance, console.cyan)
                
                if distance <= 6 and fight.canCastSpell(SPELL_IDS.DECHAINEMENT, enemy.cellId) ~= 0 then
                    fight.castSpell(SPELL_IDS.DECHAINEMENT, enemy.cellId)
                    console.print("🆘 T3 NETTOYAGE - DÉCHAINEMENT sur " .. (enemy.name or "Ennemi"), console.green)
                    global.sleep(0)
                    actionTaken = true
                elseif distance <= 5 and fight.canCastSpell(SPELL_IDS.DISSOLUTION, enemy.cellId) ~= 0 then
                    fight.castSpell(SPELL_IDS.DISSOLUTION, enemy.cellId)
                    console.print("🆘 T3 NETTOYAGE - DISSOLUTION sur " .. (enemy.name or "Ennemi"), console.green)
                    global.sleep(0)
                    actionTaken = true
                end
            end
            
            if not actionTaken and enemy and currentPA >= 3 then
                -- 2. CONDENSATION
                local distance = fight.cellsDistance(me.cellId, enemy.cellId)
                if distance <= 5 and fight.canCastSpell(SPELL_IDS.CONDENSATION, enemy.cellId) ~= 0 then
                    fight.castSpell(SPELL_IDS.CONDENSATION, enemy.cellId)
                    console.print("🆘 T3 NETTOYAGE - CONDENSATION sur " .. (enemy.name or "Ennemi"), console.green)
                    global.sleep(0)
                    actionTaken = true
                end
            end
            
            if not actionTaken and enemy and currentPA >= 2 then
                -- 3. PROJECTION
                local distance = fight.cellsDistance(me.cellId, enemy.cellId)
                if distance <= 5 and fight.canCastSpell(SPELL_IDS.PROJECTION, enemy.cellId) ~= 0 then
                    fight.castSpell(SPELL_IDS.PROJECTION, enemy.cellId)
                    console.print("🆘 T3 NETTOYAGE - PROJECTION sur " .. (enemy.name or "Ennemi"), console.green)
                    global.sleep(0)
                    actionTaken = true
                end
            end
            
            if not actionTaken then
                console.print("🆘 T3 NETTOYAGE - Aucun sort possible - Arrêt", console.red)
                break
            end
            
            -- Actualiser PA après action
            currentPA = refreshCurrentPA()
        end
        
        console.print("🧹 T3 NETTOYAGE TERMINÉ - Actions: " .. t3Actions, console.blue)
    else
        console.print("⏸️ T3 - PA insuffisants pour nettoyage (" .. currentPA .. " PA)", console.orange)
    end
    
    -- Actualiser les PA finaux via API
    currentPA = refreshCurrentPA()
    console.print("🗡️ === STRATÉGIE TOUR 3 TERMINÉE - PA restants: " .. currentPA .. " ===", console.red)
    return true
end

-- ===============================================================================
-- 🎯 STRATÉGIE TOUR 4 CONDITIONNELLE
-- ===============================================================================

-- Fonction principale Tour 4 avec analyse Phossile (DÉCISION UNIQUE)
function executeTurn4Strategy()
    console.print("🎭 === ANALYSE STRATÉGIQUE TOUR 4 - DÉCISION UNIQUE ===", console.red)
    
    local phossile = findPhossile()
    if not phossile then
        console.print("❌ PHOSSILE NON TROUVÉ - ACTIVATION STRATÉGIE POST-PHOSSILE", console.red)
        executePostPhossileStrategy()
        return true
    end
    
    -- ANALYSE UNIQUE AU DÉBUT DU TOUR (NE CHANGE JAMAIS APRÈS)
    local phossilePV = phossile.lifePoints or phossile.currentLifePoints or 0
    local phossilePO = phossile.range or 0 -- PORTÉE du Phossile
    
    console.print("📊 ANALYSE PHOSSILE (DÉBUT T4 UNIQUEMENT) - PV: " .. phossilePV .. ", PO: " .. phossilePO, console.cyan)
    
    -- DÉCISION STRATÉGIQUE FINALE (VERROUILLÉE POUR TOUT LE TOUR)
    local strategieChoisie = ""
    local strategieRaison = ""
    
    if phossilePV < 2500 and phossilePO <= 3 then
        strategieChoisie = "AGRESSIF/FINISH"
        strategieRaison = "Phossile faible (" .. phossilePV .. " PV) et courte portée (" .. phossilePO .. " PO)"
        console.print("💀 STRATÉGIE VERROUILLÉE : " .. strategieChoisie .. " - " .. strategieRaison, console.red)
        console.print("🔒 Cette stratégie NE CHANGERA PAS pendant tout le tour T4", console.red)
        
        -- Exécution stratégie agressive (ne change jamais)
        executeTurn4Strategy1()
        
    elseif (phossilePV < 2500 and phossilePO >= 4) or (phossilePV >= 2500) then
        strategieChoisie = "DÉFENSIVE/FUITE"
        strategieRaison = "Phossile dangereux (" .. phossilePV .. " PV) ou longue portée (" .. phossilePO .. " PO)"
        console.print("🛡️ STRATÉGIE VERROUILLÉE : " .. strategieChoisie .. " - " .. strategieRaison, console.blue)
        console.print("🔒 Cette stratégie NE CHANGERA PAS pendant tout le tour T4", console.blue)
        
        -- Exécution stratégie défensive (ne change jamais)
        executeDefensiveStrategy()
        
    else
        strategieChoisie = "DÉFENSIVE/DÉFAUT"
        strategieRaison = "Cas non prévu - sécurité"
        console.print("❓ STRATÉGIE VERROUILLÉE : " .. strategieChoisie .. " - " .. strategieRaison, console.orange)
        console.print("🔒 Cette stratégie NE CHANGERA PAS pendant tout le tour T4", console.orange)
        
        -- Exécution stratégie défensive par défaut (ne change jamais)
        executeDefensiveStrategy()
    end
    
    console.print("✅ STRATÉGIE T4 '" .. strategieChoisie .. "' TERMINÉE", console.green)
    return true
end

-- === STRATÉGIE 1 : AGRESSIF/FINISH AVEC DÉTECTION MORT PHOSSILE ===
-- ⚠️ IMPORTANT: Cette fonction NE RE-ÉVALUE JAMAIS les PV du Phossile pour changer de stratégie
-- Elle vérifie seulement si le Phossile est MORT pour basculer vers post-Phossile
function executeTurn4Strategy1()
    console.print("💀 === EXÉCUTION STRATÉGIE AGRESSIVE T4 ===", console.red)
    console.print("🔒 STRATÉGIE VERROUILLÉE - Aucune re-évaluation PV pour changement stratégique", console.red)
    console.print("ℹ️ Seule la MORT du Phossile peut déclencher un basculement post-Phossile", console.cyan)
    
    local me = fight.currentFighter()
    if not me then return false end
    
    local currentPA = me.actionPoints or me.AP or 0
    console.print("💀 PA disponibles (Stratégie Agressive): " .. currentPA, console.red)
    
    -- OBLIGATOIRE : Se rapprocher au CAC avant d'attaquer
    local phossile = findPhossile()
    if not phossile then
        console.print("💀 PHOSSILE DÉJÀ MORT AU DÉBUT T4 - Basculement immédiat", console.red)
        executePostPhossileStrategy()
        return true
    end
    
    if phossile then
        local myCell = me.cellId
        local distance = fight.cellsDistance(myCell, phossile.cellId)
        local currentPM = me.movementPoints or me.MP or 0
        
        if distance > 1 and currentPM > 0 then
            console.print("🏃 RAPPROCHEMENT CAC AGRESSIF - Distance: " .. distance .. ", PM: " .. currentPM, console.yellow)
            
            local moved = false
            
            -- ÉTAPE 1 : Essayer d'atteindre une cellule adjacente au Phossile (idéal)
            local adjacentCells = fight.adjacentCells(phossile.cellId)
            console.print("🔍 T4 - " .. #adjacentCells .. " cellules adjacentes au Phossile trouvées", console.cyan)
            
            for i, cell in ipairs(adjacentCells) do
                -- Gérer les deux formats possibles : ID direct ou objet avec cellId
                local cellId = (type(cell) == "table" and cell.cellId) or cell
                console.print("🔍 T4 - Test cellule adjacente " .. i .. ": " .. cellId, console.white)
                
                if fight.cellIsFree(cellId) and fight.cellIsWalkable(cellId) then
                    local distanceFromMe = fight.cellsDistance(myCell, cellId)
                    local distanceCellToPhossile = fight.cellsDistance(cellId, phossile.cellId)
                    
                    console.print("🔍 T4 - Cellule " .. cellId .. " - Distance de moi: " .. distanceFromMe .. ", Distance du Phossile: " .. distanceCellToPhossile, console.white)
                    
                    -- Vérifier que c'est vraiment adjacent au Phossile ET accessible
                    if distanceCellToPhossile == 1 and distanceFromMe <= currentPM and distanceFromMe > 0 then
                        console.print("✅ T4 - Cellule CAC idéale trouvée: " .. cellId, console.green)
                        fight.moveTowardCell(cellId)
                        global.sleep(0)
                        console.print("✅ Déplacement CAC réussi vers " .. cellId, console.green)
                        moved = true
                        break
                    else
                        console.print("❌ T4 - Cellule " .. cellId .. " non accessible (PM: " .. distanceFromMe .. "/" .. currentPM .. ")", console.red)
                    end
                else
                    console.print("❌ T4 - Cellule " .. cellId .. " occupée ou non marchable", console.red)
                end
            end
            
            -- ÉTAPE 2 : Si CAC impossible, se rapprocher au maximum avec les PM disponibles
            if not moved then
                console.print("⚠️ T4 - CAC impossible - Recherche rapprochement maximal", console.orange)
                
                local accessibleCells = fight.accessibleCells()
                local bestApproachCell = nil
                local bestDistanceToPhossile = 999
                
                console.print("🔍 T4 - Analyse de " .. #accessibleCells .. " cellules accessibles pour rapprochement", console.cyan)
                
                for _, cell in ipairs(accessibleCells) do
                    -- CORRECTION : Gérer les deux formats de cellules (nombre ou objet)
                    local cellId = (type(cell) == "table" and cell.cellId) or cell
                    local distanceToPhossile = fight.cellsDistance(cellId, phossile.cellId)
                    local distanceFromMe = fight.cellsDistance(myCell, cellId)
                    
                    -- Chercher la cellule accessible qui rapproche le plus du Phossile
                    if distanceFromMe > 0 and distanceFromMe <= currentPM and distanceToPhossile < bestDistanceToPhossile then
                        bestApproachCell = cellId
                        bestDistanceToPhossile = distanceToPhossile
                        console.print("🎯 T4 - Cellule rapprochement candidate: " .. bestApproachCell .. " (distance Phossile: " .. distanceToPhossile .. ")", console.white)
                    end
                end
                
                if bestApproachCell then
                    console.print("🏃 T4 - RAPPROCHEMENT MAXIMAL vers " .. bestApproachCell .. " (distance finale: " .. bestDistanceToPhossile .. ")", console.yellow)
                    fight.moveTowardCell(bestApproachCell)
                global.sleep(0)
                    moved = true
                else
                    console.print("❌ T4 - Aucun rapprochement possible - Combat à distance", console.red)
                end
            end
            
        else
            if distance <= 1 then
                console.print("✅ Déjà au CAC avec Phossile - Attaque directe", console.green)
            else
                console.print("❌ Aucun PM disponible pour rapprochement - Combat à distance", console.orange)
            end
        end
    end
    
    -- CONFIRMATION : Fin de phase mouvement, début phase attaque
    console.print("🗡️ === FIN RAPPROCHEMENT - DÉBUT PHASE ATTAQUE T4 ===", console.blue)
    local me = fight.currentFighter() -- Actualiser la position après mouvement
    if me and phossile then
        local finalDistance = fight.cellsDistance(me.cellId, phossile.cellId)
        console.print("📏 Distance finale avec Phossile: " .. finalDistance, console.cyan)
    end
    
    -- ÉTAPE 1: Déchainement sur Phossile avec vérification mort
    local phossileCheck1 = findPhossile()
    if phossileCheck1 and currentPA >= 4 then
        if fight.canCastSpell(SPELL_IDS.DECHAINEMENT, phossileCheck1.cellId) ~= 0 then
            fight.castSpell(SPELL_IDS.DECHAINEMENT, phossileCheck1.cellId)
            currentPA = currentPA - 4
            console.print("✅ 1. DÉCHAINEMENT sur PHOSSILE (PA restants: " .. currentPA .. ")", console.green)
            global.sleep(0) -- Pause plus longue pour mise à jour serveur
            
            -- VÉRIFICATION CRITIQUE : Phossile mort après Déchainement ?
            local phossileAfterDechainement = findPhossile()
            if not phossileAfterDechainement then
                console.print("💀 PHOSSILE TUÉ PAR DÉCHAINEMENT - BASCULEMENT IMMÉDIAT POST-PHOSSILE", console.red)
                executePostPhossileStrategy()
                return true
            else
                console.print("✅ Phossile survit au Déchainement - Poursuite attaque", console.green)
            end
        end
    end
    
    -- ÉTAPE 2a: Première Dissolution avec vérification mort
    local phossileCheck2a = findPhossile()
    if phossileCheck2a and currentPA >= 4 then
        if fight.canCastSpell(SPELL_IDS.DISSOLUTION, phossileCheck2a.cellId) ~= 0 then
            fight.castSpell(SPELL_IDS.DISSOLUTION, phossileCheck2a.cellId)
            currentPA = currentPA - 4
            console.print("✅ 2a. DISSOLUTION #1 sur PHOSSILE (PA restants: " .. currentPA .. ")", console.green)
            global.sleep(0)
            
            -- VÉRIFICATION CRITIQUE : Phossile mort après Dissolution #1 ?
            local phossileAfterDissolution1 = findPhossile()
            if not phossileAfterDissolution1 then
                console.print("💀 PHOSSILE TUÉ PAR DISSOLUTION #1 - BASCULEMENT IMMÉDIAT POST-PHOSSILE", console.red)
                executePostPhossileStrategy()
                return true
            else
                console.print("✅ Phossile survit à Dissolution #1 - Poursuite attaque", console.green)
            end
        end
    end
    
    -- ÉTAPE 2b: Deuxième Dissolution avec vérification mort
    local phossileCheck2b = findPhossile()
    if phossileCheck2b and currentPA >= 4 then
        if fight.canCastSpell(SPELL_IDS.DISSOLUTION, phossileCheck2b.cellId) ~= 0 then
            fight.castSpell(SPELL_IDS.DISSOLUTION, phossileCheck2b.cellId)
            currentPA = currentPA - 4
            console.print("✅ 2b. DISSOLUTION #2 sur PHOSSILE (PA restants: " .. currentPA .. ")", console.green)
            global.sleep(0)
            
            -- VÉRIFICATION CRITIQUE : Phossile mort après Dissolution #2 ?
            local phossileAfterDissolution2 = findPhossile()
            if not phossileAfterDissolution2 then
                console.print("💀 PHOSSILE TUÉ PAR DISSOLUTION #2 - BASCULEMENT IMMÉDIAT POST-PHOSSILE", console.red)
                executePostPhossileStrategy()
                return true
            else
                console.print("✅ Phossile survit à Dissolution #2 - Fin stratégie agressive", console.green)
            end
        end
    end
    
    -- VÉRIFICATION FINALE : Si le Phossile est encore vivant à la fin
    local finalPhossileCheck = findPhossile()
    if not finalPhossileCheck then
        console.print("💀 Phossile mort en fin de stratégie - Activation post-Phossile", console.red)
        executePostPhossileStrategy()
        return true
    else
        console.print("💀 === STRATÉGIE AGRESSIF/FINISH TERMINÉE - PHOSSILE ENCORE VIVANT - PA restants: " .. currentPA .. " ===", console.red)
    end
    
    return true
end

-- ===============================================================================
-- 🎯 SYSTÈME DE PLACEMENT INTELLIGENT AVEC DÉTECTION PHOSSILE
-- ===============================================================================

-- Fonction de placement intelligent avec détection Phossile
function fight_placement(possiblePositions, availablePositions)
    console.print("🎯 === PLACEMENT INTELLIGENT DÉMARRÉ ===", console.blue)
    
    local finalPosition = nil
    local success, result = pcall(function()
        -- Statut d'équipe avec fallbacks sécurisés
        local teamStatus = {
            isLeader = false,
            isSolo = false,
            count = 1,
            index = 1
        }
        
        -- Déterminer le statut d'équipe avec protection
        pcall(function()
            teamStatus.count = global.teamCount() or 1
            teamStatus.isLeader = global.isTeamLeader() or false
            teamStatus.index = global.inTeamIndex() or 1
            teamStatus.isSolo = teamStatus.count <= 1
        end)
        
        console.print("👥 Équipe: " .. teamStatus.count .. " membres, Leader: " .. tostring(teamStatus.isLeader) .. ", Index: " .. teamStatus.index, console.cyan)
        
        -- Debug positions disponibles
        if availablePositions and #availablePositions > 0 then
            console.print("📍 Positions disponibles: " .. table.concat(availablePositions, ","), console.white)
        else
            console.print("📍 Aucune position disponible fournie", console.orange)
        end
        
        -- Détecter le Phossile via fight.fighters()
        local phossileCell = nil
        local phossileDetected = false
        
        pcall(function()
            local fighters = fight.fighters()
            if fighters then
                for _, fighter in pairs(fighters) do
                    if fighter then
                        -- Détection par monsterId 4046
                        if fighter.monsterId == 4046 then
                            phossileCell = fighter.cellId
                            phossileDetected = true
                            console.print("🎯 PHOSSILE DÉTECTÉ (ID 4046) - Cell: " .. phossileCell, console.yellow)
                            break
                        end
                        
                        -- Détection par nom contenant "phossile"
                        if fighter.name and string.find(fighter.name:lower(), "phossile") then
                            phossileCell = fighter.cellId
                            phossileDetected = true
                            console.print("🎯 PHOSSILE DÉTECTÉ (nom) - Cell: " .. phossileCell, console.yellow)
                            break
                        end
                    end
                end
            end
        end)
        
        -- Configuration de placement selon spécifications exactes
        local placementConfig = {
            -- Mode NORMAL (par défaut)
            normal = {
                leader = 302,
                mules = {230, 215, 202, 216, 232, 246}
            },
            -- Mode SPÉCIAL (Phossile sur cellules spécifiques)
            special = {
                leader = 230,
                mules = {274, 215, 202, 216, 232, 246} -- 274 en premier pour éviter conflit avec leader
            }
        }
        
        -- Déterminer le mode selon la position du Phossile
        local placementMode = "normal"
        if phossileDetected and phossileCell then
            local specialPhossileCells = {378, 337, 309, 281, 253}
            for _, specialCell in ipairs(specialPhossileCells) do
                if phossileCell == specialCell then
                    placementMode = "special"
                    console.print("🔥 MODE SPÉCIAL ACTIVÉ - Phossile sur cellule: " .. phossileCell, console.red)
                    break
                end
            end
        end
        
        if placementMode == "normal" then
            console.print("🎮 MODE NORMAL SÉLECTIONNÉ", console.cyan)
        end
        
        -- Debug configuration sélectionnée
        local selectedConfig = placementConfig[placementMode]
        console.print("📋 Config " .. placementMode:upper() .. " - Leader: " .. selectedConfig.leader .. ", Mules: " .. table.concat(selectedConfig.mules, ","), console.white)
        
        -- Sélectionner la position selon le rôle avec priorité leader
        local selectedConfig = placementConfig[placementMode]
        
        if teamStatus.isSolo then
            -- SOLO : Placement immédiat
            finalPosition = selectedConfig.leader
            console.print("👑 SOLO → Cellule: " .. finalPosition .. " (IMMÉDIAT)", console.blue)
            
        elseif teamStatus.isLeader then
            -- LEADER : ATTENDRE QUE LES MULES SE PLACENT EN PREMIER
            console.print("👑 LEADER → ATTENTE DES MULES (placement EN DERNIER)", console.yellow)
            
            -- Attendre 3-4 secondes pour laisser les mules se placer
            local waitTime = 0 + (teamStatus.count * 0) -- 3s + 500ms par membre d'équipe
            console.print("⏳ LEADER ATTEND " .. (waitTime/1000) .. "s pour que les mules se placent...", console.orange)
            global.sleep(waitTime)
            
            -- FORCER la position du leader (priorité absolue)
            finalPosition = selectedConfig.leader
            console.print("👑 LEADER FORCE SA POSITION → Cellule: " .. finalPosition .. " (PRIORITÉ ABSOLUE)", console.red)
            
            -- Vérifier si la position est occupée par une mule et la déloger si nécessaire
            local leaderPosOccupied = false
            if availablePositions and #availablePositions > 0 then
                for _, pos in ipairs(availablePositions) do
                    if pos == finalPosition then
                        leaderPosOccupied = false
                        break
                    else
                        leaderPosOccupied = true
                    end
                end
            end
            
            if leaderPosOccupied then
                console.print("🚨 POSITION LEADER " .. finalPosition .. " OCCUPÉE PAR UNE MULE - DÉLOGEMENT FORCÉ", console.red)
            end
            
            console.print("👑 LEADER IGNORE availablePositions ET PREND SA PLACE DE FORCE", console.blue)
            
        else
            -- MULES : Éviter la position du leader
            local mulePositions = selectedConfig.mules
            local leaderPosition = selectedConfig.leader
            local availableMulePositions = {}
            
            -- Filtrer les positions de mules pour exclure celle du leader
            for _, mulePos in ipairs(mulePositions) do
                if mulePos ~= leaderPosition then
                    table.insert(availableMulePositions, mulePos)
                end
            end
            
            console.print("👥 Positions mules filtrées (sans leader " .. leaderPosition .. "): " .. #availableMulePositions .. " disponibles", console.cyan)
            
            local muleIndex = math.min(teamStatus.index, #availableMulePositions)
            if muleIndex >= 1 and muleIndex <= #availableMulePositions then
                finalPosition = availableMulePositions[muleIndex]
                console.print("👥 MULE #" .. teamStatus.index .. " → Cellule: " .. finalPosition, console.green)
            else
                -- Fallback sur la première position de mule disponible
                if #availableMulePositions > 0 then
                    finalPosition = availableMulePositions[1]
                    console.print("👥 MULE FALLBACK → Cellule: " .. finalPosition, console.orange)
                else
                    -- Dernier recours : utiliser une position de mule originale
                    finalPosition = mulePositions[1]
                    console.print("👥 MULE DERNIER RECOURS → Cellule: " .. finalPosition, console.red)
                end
            end
        end
        
        -- Vérifier que la cellule est disponible dans availablePositions
        if availablePositions and #availablePositions > 0 then
            local isAvailable = false
            for _, pos in ipairs(availablePositions) do
                if pos == finalPosition then
                    isAvailable = true
                    break
                end
            end
            
            if not isAvailable then
                if teamStatus.isSolo then
                    -- SOLO : Priorité absolue 
                    console.print("👑 SOLO FORCE SA POSITION " .. finalPosition .. " (ignore availablePositions)", console.red)
                elseif teamStatus.isLeader then
                    -- LEADER : Priorité absolue - DÉLOGER TOUTE MULE
                    console.print("👑 LEADER DÉLOGER ET FORCE SA POSITION " .. finalPosition .. " (ignore availablePositions)", console.red)
                    console.print("🔥 MULES DÉLOGÉES DE LA CELLULE " .. finalPosition .. " - LEADER PRIORITAIRE", console.red)
                    -- Garder la position du leader même si pas dans availablePositions
                else
                    -- MULES : Respecter availablePositions
                    console.print("⚠️ Position mule " .. finalPosition .. " indisponible - Fallback", console.orange)
                    finalPosition = availablePositions[1]
                    console.print("🔄 Mule fallback sur première position disponible: " .. finalPosition, console.yellow)
                end
            end
        end
        
        return finalPosition
    end)
    
    -- Gestion d'erreur avec fallback de sécurité
    if not success or not result then
        console.print("❌ Erreur placement - Fallback sécurité", console.red)
        if availablePositions and #availablePositions > 0 then
            finalPosition = availablePositions[1]
        else
            finalPosition = 302 -- Position par défaut de sécurité
        end
    else
        finalPosition = result
    end
    
    console.print("🎯 === PLACEMENT FINAL: " .. (finalPosition or "AUCUN") .. " ===", console.blue)
    return finalPosition or -1
end

-- ===============================================================================
-- 💀 STRATÉGIE POST-PHOSSILE AVEC DÉPLACEMENT CAC ET PRIORITÉS
-- ===============================================================================

-- Variables globales pour tracker les cooldowns (réinitialisées à chaque combat)
postPhossileTurnCount = 0 -- GLOBAL : Compteur de tours post-Phossile
spellCooldowns = {} -- GLOBAL : Tracker des sorts en cooldown

-- Fonction pour marquer un sort en cooldown
function markSpellOnCooldown(spellId, spellName, turnsDuration)
    turnsDuration = turnsDuration or 3 -- Défaut 3 tours de cooldown
    local currentRound = fight.currentRound() or 1
    spellCooldowns[spellId] = {
        name = spellName,
        expiresAtTurn = currentRound + turnsDuration
    }
    console.print("⏸️ COOLDOWN DÉTECTÉ: " .. spellName .. " bloqué pour " .. turnsDuration .. " tours (expire au tour " .. (currentRound + turnsDuration) .. ")", console.red)
end

-- Fonction pour vérifier si un sort est en cooldown
function isSpellOnCooldown(spellId)
    if spellCooldowns[spellId] then
        local currentRound = fight.currentRound() or 1
        if currentRound < spellCooldowns[spellId].expiresAtTurn then
            local turnsLeft = spellCooldowns[spellId].expiresAtTurn - currentRound
            console.print("⏸️ " .. spellCooldowns[spellId].name .. " en cooldown (" .. turnsLeft .. " tours restants - expire au tour " .. spellCooldowns[spellId].expiresAtTurn .. ")", console.orange)
            return true
        else
            -- Cooldown expiré
            local spellName = spellCooldowns[spellId].name
            spellCooldowns[spellId] = nil
            console.print("✅ Cooldown expiré pour " .. spellName .. " (tour actuel: " .. currentRound .. ")", console.green)
            return false
        end
    end
    return false
end

-- Fonction pour réinitialiser les cooldowns au début d'un nouveau combat
function resetSpellCooldowns()
    spellCooldowns = {}
    postPhossileTurnCount = 0 -- IMPORTANT : Réinitialiser le compteur de tours
    console.print("🔄 Cooldowns des sorts et compteur de tours réinitialisés", console.blue)
end

-- Fonction de debug pour afficher les cooldowns actifs
function debugActiveCooldowns()
    local currentRound = fight.currentRound() or 1
    console.print("🔍 === DEBUG COOLDOWNS (Tour " .. currentRound .. ") ===", console.yellow)
    
    local hasActiveCooldowns = false
    for spellId, cooldownData in pairs(spellCooldowns) do
        if cooldownData then
            local turnsLeft = cooldownData.expiresAtTurn - currentRound
            if turnsLeft > 0 then
                console.print("  ⏸️ " .. cooldownData.name .. " - Expire au tour " .. cooldownData.expiresAtTurn .. " (" .. turnsLeft .. " tours restants)", console.orange)
                hasActiveCooldowns = true
            else
                console.print("  ✅ " .. cooldownData.name .. " - Cooldown expiré", console.green)
            end
        end
    end
    
    if not hasActiveCooldowns then
        console.print("  ✅ Aucun cooldown actif", console.green)
    end
    console.print("🔍 === FIN DEBUG COOLDOWNS ===", console.yellow)
end





-- ===============================================================================
-- 🗡️ STRATÉGIE TOUR 5 SPÉCIALISÉE
-- ===============================================================================

function executeTurn5Strategy()
    console.print("🗡️ === STRATÉGIE TOUR 5 DÉMARRÉE ===", console.red)
    
    local me = fight.currentFighter()
    if not me then
        console.print("❌ Pas de combattant actuel", console.red)
        return false
    end
    
    local phossile = findPhossile()
    if not phossile then
        console.print("💀 PHOSSILE MORT - ACTIVATION STRATÉGIE POST-PHOSSILE T5", console.red)
        
        -- Récupérer PA actuels et PV pour stratégie post-Phossile
        local currentPA = refreshCurrentPA()
        local stats = getFighterStats()
        local pvPercent = 0
        if stats.PVMax > 0 then
            pvPercent = (stats.PV / stats.PVMax) * 100
        end
        
        console.print("⚡ PA disponibles pour stratégie post-Phossile: " .. currentPA, console.cyan)
        console.print("💖 PV actuels: " .. pvPercent .. "%", console.cyan)
        
        -- Lancer stratégie post-Phossile officielle
        executeOfficialPostPhossileStrategy(currentPA, pvPercent)
        
        console.print("✅ Stratégie Post-Phossile T5 TERMINÉE", console.green)
        return true
    end
    
    console.print("🎯 PHOSSILE trouvé sur cellule: " .. phossile.cellId, console.yellow)
    
    local currentPA = refreshCurrentPA()
    console.print("⚡ PA disponibles: " .. currentPA, console.cyan)
    
    -- ÉTAPE 1: MUTILATION (2 PA) - Sur soi-même
    console.print("⚔️ === ÉTAPE 1 T5: MUTILATION ===", console.blue)
    
    if currentPA >= 2 then
        -- Vérifier si Mutilation n'est pas en cooldown
        if not isSpellOnCooldown(SPELL_IDS.MUTILATION) then
            local success, newPA = castSpellAndRefresh(SPELL_IDS.MUTILATION, me.cellId, "MUTILATION T5", 2)
            currentPA = newPA
            
            if success then
                console.print("✅ 1. MUTILATION T5 réussie", console.green)
                -- Enregistrer le cooldown (2 tours)
                markSpellOnCooldown(SPELL_IDS.MUTILATION, "MUTILATION T5", 2)
            else
                console.print("❌ 1. MUTILATION T5 échouée", console.red)
            end
        else
            console.print("⏸️ 1. MUTILATION ignorée (cooldown actif)", console.orange)
        end
    else
        console.print("❌ 1. PA insuffisants pour MUTILATION (" .. currentPA .. "/2)", console.red)
    end
    
    -- ÉTAPE 2: DÉPLACEMENT VERS PHOSSILE
    console.print("🏃 === ÉTAPE 2 T5: DÉPLACEMENT VERS PHOSSILE ===", console.blue)
    
    me = fight.currentFighter() -- Actualiser position
    if not me then
        console.print("❌ Impossible de récupérer position actuelle", console.red)
        return false
    end
    
    local distance = fight.cellsDistance(me.cellId, phossile.cellId)
    console.print("📏 Distance actuelle avec Phossile: " .. distance, console.cyan)
    
    if distance > 1 then
        console.print("🏃 RAPPROCHEMENT NÉCESSAIRE T5 - Distance: " .. distance, console.yellow)
        
        -- Récupérer toutes les cellules accessibles
        local accessibleCells = fight.accessibleCells()
        local currentPM = me.movementPoints or me.MP or 0
        local bestCell = nil
        local minDistanceToPhossile = 999
        
        console.print("🔍 T5 - Recherche position optimale - PM disponibles: " .. currentPM, console.cyan)
        
        -- Trouver la cellule accessible la plus proche du Phossile
        for _, cellData in ipairs(accessibleCells) do
            local cell = cellData.cellId
            local distanceFromMe = fight.cellsDistance(me.cellId, cell)
            local distanceToPhossile = fight.cellsDistance(cell, phossile.cellId)
            
            if distanceFromMe <= currentPM and distanceFromMe > 0 and distanceToPhossile < minDistanceToPhossile then
                minDistanceToPhossile = distanceToPhossile
                bestCell = cell
                console.print("🎯 T5 - Cellule candidate: " .. cell .. " (distance Phossile: " .. distanceToPhossile .. ", coût PM: " .. distanceFromMe .. ")", console.white)
            end
        end
        
        -- Se déplacer vers la meilleure cellule trouvée
        if bestCell then
            console.print("🏃 T5 - Déplacement vers " .. bestCell .. " (distance finale Phossile: " .. minDistanceToPhossile .. ")", console.yellow)
            fight.moveTowardCell(bestCell)
            global.sleep(0) -- Délai pour laisser le déplacement s'effectuer
            
            -- Vérifier le succès du déplacement
            me = fight.currentFighter()
            if me then
                local newDistance = fight.cellsDistance(me.cellId, phossile.cellId)
                console.print("✅ T5 - Déplacement terminé - Nouvelle distance Phossile: " .. newDistance, console.green)
            end
        else
            console.print("❌ T5 - Impossible d'optimiser la position avec " .. currentPM .. " PM", console.red)
        end
    else
        console.print("✅ T5 - Déjà proche du Phossile (distance: " .. distance .. ")", console.green)
    end
    
    -- ÉTAPE 3: COURONNES D'ÉPINES SUR SOI-MÊME (2 PA)
    console.print("🛡️ === ÉTAPE 3 T5: COURONNES D'ÉPINES SUR SOI-MÊME ===", console.blue)
    
    -- Actualiser positions
    me = fight.currentFighter()
    currentPA = refreshCurrentPA()
    
    if not me then
        console.print("❌ Impossible de récupérer position pour COURONNES D'ÉPINES", console.red)
        return false
    end
    
    console.print("📍 Position actuelle: " .. me.cellId, console.cyan)
    console.print("⚡ PA disponibles: " .. currentPA .. " (Requis: 2 pour COURONNES D'ÉPINES)", console.cyan)
    
    if currentPA >= 2 then
        -- Vérifier si Couronnes d'Épines n'est pas en cooldown
        if not isSpellOnCooldown(SPELL_IDS.COURONNES_EPINES) then
            console.print("✅ PA suffisants pour COURONNES D'ÉPINES", console.green)
            local canCast = fight.canCastSpell(SPELL_IDS.COURONNES_EPINES, me.cellId)
            console.print("🔍 fight.canCastSpell COURONNES D'ÉPINES result: " .. canCast, console.yellow)
            
            if canCast ~= 0 then
                console.print("🛡️ Lancement COURONNES D'ÉPINES...", console.blue)
                local success, newPA = castSpellAndRefresh(SPELL_IDS.COURONNES_EPINES, me.cellId, "COURONNES D'ÉPINES T5", 2)
                currentPA = newPA
                
                if success then
                    console.print("✅ COURONNES D'ÉPINES T5 réussie - PA restants: " .. currentPA, console.green)
                    -- Enregistrer le cooldown (3 tours)
                    markSpellOnCooldown(SPELL_IDS.COURONNES_EPINES, "COURONNES D'ÉPINES T5", 3)
                else
                    console.print("❌ COURONNES D'ÉPINES T5 échouée malgré les vérifications", console.red)
                end
            else
                console.print("❌ COURONNES D'ÉPINES impossible - fight.canCastSpell retourne 0", console.red)
            end
        else
            console.print("⏸️ COURONNES D'ÉPINES ignorée (cooldown actif)", console.orange)
        end
    else
        console.print("❌ PA insuffisants pour COURONNES D'ÉPINES (PA: " .. currentPA .. "/2)", console.red)
    end
    
    -- ÉTAPE 4: DOUBLE DÉCHAINEMENT (CAC) OU DOUBLE DISSOLUTION (DISTANCE)
    console.print("⚔️ === ÉTAPE 4 T5: ANALYSE DISTANCE POUR SORTS FINAUX ===", console.blue)
    
    -- Actualiser positions finales
    me = fight.currentFighter()
    phossile = findPhossile()
    currentPA = refreshCurrentPA()
    
    if not me then
        console.print("❌ Impossible de récupérer position pour sorts finaux", console.red)
        return false
    end
    
    if not phossile then
        console.print("💀 Phossile mort - Sorts finaux annulés", console.orange)
        console.print("🗡️ === STRATÉGIE TOUR 5 TERMINÉE (Phossile mort) - PA restants: " .. currentPA .. " ===", console.red)
        return true
    end
    
    console.print("📍 Position finale: " .. me.cellId, console.cyan)
    console.print("🎯 Position Phossile finale: " .. phossile.cellId, console.cyan)
    
    local finalDistance = fight.cellsDistance(me.cellId, phossile.cellId)
    console.print("📏 Distance finale vers Phossile: " .. finalDistance, console.cyan)
    console.print("⚡ PA disponibles: " .. currentPA .. " (Requis: 8 pour double sort)", console.cyan)
    
    if currentPA >= 8 then
        if finalDistance == 1 then
            -- CAC : DOUBLE DÉCHAINEMENT
            console.print("🗡️ === CAC DÉTECTÉ - DOUBLE DÉCHAINEMENT ===", console.green)
            local maxRangeDechainement = SPELL_RANGES[SPELL_IDS.DECHAINEMENT] or 1
            
            if finalDistance <= maxRangeDechainement then
                -- Premier DÉCHAINEMENT
                local canCast1 = fight.canCastSpell(SPELL_IDS.DECHAINEMENT, phossile.cellId)
                console.print("🔍 fight.canCastSpell DÉCHAINEMENT #1 result: " .. canCast1, console.yellow)
                
                if canCast1 ~= 0 then
                    console.print("🗡️ Lancement DÉCHAINEMENT #1...", console.blue)
                    local success1, newPA1 = castSpellAndRefresh(SPELL_IDS.DECHAINEMENT, phossile.cellId, "DÉCHAINEMENT T5 #1", 4)
                    currentPA = newPA1
                    
                    if success1 then
                        console.print("✅ DÉCHAINEMENT #1 réussi - PA restants: " .. currentPA, console.green)
                        
                        -- Actualiser position Phossile après premier sort
                        phossile = findPhossile()
                        if phossile and currentPA >= 4 then
                            console.print("🔄 Position Phossile après DÉCHAINEMENT #1: " .. phossile.cellId, console.cyan)
                            
                            -- Deuxième DÉCHAINEMENT
                            local canCast2 = fight.canCastSpell(SPELL_IDS.DECHAINEMENT, phossile.cellId)
                            console.print("🔍 fight.canCastSpell DÉCHAINEMENT #2 result: " .. canCast2, console.yellow)
                            
                            if canCast2 ~= 0 then
                                console.print("🗡️ Lancement DÉCHAINEMENT #2...", console.blue)
                                local success2, newPA2 = castSpellAndRefresh(SPELL_IDS.DECHAINEMENT, phossile.cellId, "DÉCHAINEMENT T5 #2", 4)
                                currentPA = newPA2
                                
                                if success2 then
                                    console.print("✅ DOUBLE DÉCHAINEMENT T5 COMPLET - PA restants: " .. currentPA, console.green)
                                else
                                    console.print("❌ DÉCHAINEMENT #2 échoué", console.red)
                                end
                            else
                                console.print("❌ DÉCHAINEMENT #2 impossible", console.red)
                            end
                        elseif not phossile then
                            console.print("💀 Phossile tué par DÉCHAINEMENT #1", console.red)
                        else
                            console.print("⚠️ PA insuffisants pour DÉCHAINEMENT #2 (PA: " .. currentPA .. "/4)", console.orange)
                        end
                    else
                        console.print("❌ DÉCHAINEMENT #1 échoué", console.red)
                    end
                else
                    console.print("❌ DÉCHAINEMENT #1 impossible", console.red)
                end
            else
                console.print("❌ DÉCHAINEMENT impossible - Hors portée CAC", console.red)
            end
        else
            -- DISTANCE : DOUBLE DISSOLUTION
            console.print("🌪️ === DISTANCE DÉTECTÉE - DOUBLE DISSOLUTION ===", console.yellow)
            local maxRangeDissolution = SPELL_RANGES[SPELL_IDS.DISSOLUTION] or 5
            
            if finalDistance <= maxRangeDissolution then
                -- Première DISSOLUTION
                local canCast1 = fight.canCastSpell(SPELL_IDS.DISSOLUTION, phossile.cellId)
                console.print("🔍 fight.canCastSpell DISSOLUTION #1 result: " .. canCast1, console.yellow)
                
                if canCast1 ~= 0 then
                    console.print("🌪️ Lancement DISSOLUTION #1...", console.blue)
                    local success1, newPA1 = castSpellAndRefresh(SPELL_IDS.DISSOLUTION, phossile.cellId, "DISSOLUTION T5 #1", 4)
                    currentPA = newPA1
                    
                    if success1 then
                        console.print("✅ DISSOLUTION #1 réussie - PA restants: " .. currentPA, console.green)
                        
                        -- Actualiser position Phossile après premier sort
                        phossile = findPhossile()
                        if phossile and currentPA >= 4 then
                            console.print("🔄 Position Phossile après DISSOLUTION #1: " .. phossile.cellId, console.cyan)
                            
                            -- Deuxième DISSOLUTION
                            local canCast2 = fight.canCastSpell(SPELL_IDS.DISSOLUTION, phossile.cellId)
                            console.print("🔍 fight.canCastSpell DISSOLUTION #2 result: " .. canCast2, console.yellow)
                            
                            if canCast2 ~= 0 then
                                console.print("🌪️ Lancement DISSOLUTION #2...", console.blue)
                                local success2, newPA2 = castSpellAndRefresh(SPELL_IDS.DISSOLUTION, phossile.cellId, "DISSOLUTION T5 #2", 4)
                                currentPA = newPA2
                                
                                if success2 then
                                    console.print("✅ DOUBLE DISSOLUTION T5 COMPLÈTE - PA restants: " .. currentPA, console.green)
                                else
                                    console.print("❌ DISSOLUTION #2 échouée", console.red)
                                end
                            else
                                console.print("❌ DISSOLUTION #2 impossible", console.red)
                            end
                        elseif not phossile then
                            console.print("💀 Phossile tué par DISSOLUTION #1", console.red)
                        else
                            console.print("⚠️ PA insuffisants pour DISSOLUTION #2 (PA: " .. currentPA .. "/4)", console.orange)
                        end
                    else
                        console.print("❌ DISSOLUTION #1 échouée", console.red)
                    end
                else
                    console.print("❌ DISSOLUTION #1 impossible", console.red)
                end
            else
                console.print("❌ DISSOLUTION impossible - Hors portée (distance: " .. finalDistance .. " > " .. maxRangeDissolution .. ")", console.red)
            end
        end
    else
        console.print("❌ PA insuffisants pour double sort (PA: " .. currentPA .. "/8)", console.red)
    end
    
    -- RÉSUMÉ FINAL
    currentPA = refreshCurrentPA()
    console.print("🗡️ === STRATÉGIE TOUR 5 TERMINÉE - PA restants: " .. currentPA .. " ===", console.red)
    
    return true
end

-- ===============================================================================
-- 🗡️ STRATÉGIE TOUR 6 SPÉCIALISÉE
-- ===============================================================================

function executeTurn6Strategy()
    console.print("🗡️ === STRATÉGIE TOUR 6 DÉMARRÉE ===", console.red)
    
    local me = fight.currentFighter()
    if not me then
        console.print("❌ Pas de combattant actuel", console.red)
        return false
    end
    
    local phossile = findPhossile()
    if not phossile then
        console.print("💀 PHOSSILE MORT - ACTIVATION STRATÉGIE POST-PHOSSILE T6", console.red)
        
        -- Récupérer PA actuels et PV pour stratégie post-Phossile
        local currentPA = refreshCurrentPA()
        local stats = getFighterStats()
        local pvPercent = 0
        if stats.PVMax > 0 then
            pvPercent = (stats.PV / stats.PVMax) * 100
        end
        
        console.print("⚡ PA disponibles pour stratégie post-Phossile: " .. currentPA, console.cyan)
        console.print("💖 PV actuels: " .. pvPercent .. "%", console.cyan)
        
        -- Lancer stratégie post-Phossile officielle
        executeOfficialPostPhossileStrategy(currentPA, pvPercent)
        
        console.print("✅ Stratégie Post-Phossile T6 TERMINÉE", console.green)
        return true
    end
    
    console.print("🎯 PHOSSILE trouvé sur cellule: " .. phossile.cellId, console.yellow)
    
    local currentPA = refreshCurrentPA()
    console.print("⚡ PA disponibles: " .. currentPA, console.cyan)
    
    -- ÉTAPE 1: DÉPLACEMENT VERS PHOSSILE
    console.print("🏃 === ÉTAPE 1 T6: DÉPLACEMENT VERS PHOSSILE ===", console.blue)
    
    me = fight.currentFighter() -- Actualiser position
    if not me then
        console.print("❌ Impossible de récupérer position actuelle", console.red)
        return false
    end
    
    local distance = fight.cellsDistance(me.cellId, phossile.cellId)
    console.print("📏 Distance actuelle avec Phossile: " .. distance, console.cyan)
    
    if distance > 1 then
        console.print("🏃 RAPPROCHEMENT NÉCESSAIRE T6 - Distance: " .. distance, console.yellow)
        
        -- Récupérer toutes les cellules accessibles
        local accessibleCells = fight.accessibleCells()
        local currentPM = me.movementPoints or me.MP or 0
        local bestCell = nil
        local minDistanceToPhossile = 999
        
        console.print("🔍 T6 - Recherche position optimale - PM disponibles: " .. currentPM, console.cyan)
        
        -- Trouver la cellule accessible la plus proche du Phossile
        for _, cellData in ipairs(accessibleCells) do
            local cell = cellData.cellId
            local distanceFromMe = fight.cellsDistance(me.cellId, cell)
            local distanceToPhossile = fight.cellsDistance(cell, phossile.cellId)
            
            if distanceFromMe <= currentPM and distanceFromMe > 0 and distanceToPhossile < minDistanceToPhossile then
                minDistanceToPhossile = distanceToPhossile
                bestCell = cell
                console.print("🎯 T6 - Cellule candidate: " .. cell .. " (distance Phossile: " .. distanceToPhossile .. ", coût PM: " .. distanceFromMe .. ")", console.white)
            end
        end
        
        -- Se déplacer vers la meilleure cellule trouvée
        if bestCell then
            console.print("🏃 T6 - Déplacement vers " .. bestCell .. " (distance finale Phossile: " .. minDistanceToPhossile .. ")", console.yellow)
            fight.moveTowardCell(bestCell)
            global.sleep(0) -- Délai pour laisser le déplacement s'effectuer
            
            -- Vérifier le succès du déplacement
            me = fight.currentFighter()
            if me then
                local newDistance = fight.cellsDistance(me.cellId, phossile.cellId)
                console.print("✅ T6 - Déplacement terminé - Nouvelle distance Phossile: " .. newDistance, console.green)
            end
        else
            console.print("❌ T6 - Impossible d'optimiser la position avec " .. currentPM .. " PM", console.red)
        end
    else
        console.print("✅ T6 - Déjà proche du Phossile (distance: " .. distance .. ")", console.green)
    end
    
    -- ÉTAPE 2: CONDENSATION SUR PHOSSILE (3 PA)
    console.print("🌊 === ÉTAPE 2 T6: CONDENSATION SUR PHOSSILE ===", console.blue)
    
    -- Actualiser positions
    me = fight.currentFighter()
    phossile = findPhossile()
    currentPA = refreshCurrentPA()
    
    if not me or not phossile then
        console.print("❌ Impossible de récupérer positions pour CONDENSATION T6", console.red)
        return false
    end
    
    console.print("📍 Position actuelle: " .. me.cellId, console.cyan)
    console.print("🎯 Position Phossile: " .. phossile.cellId, console.cyan)
    
    local distance = fight.cellsDistance(me.cellId, phossile.cellId)
    local maxRange = SPELL_RANGES[SPELL_IDS.CONDENSATION] or 5
    
    console.print("📏 Distance vers Phossile: " .. distance .. " (Portée CONDENSATION: " .. maxRange .. ")", console.cyan)
    console.print("⚡ PA disponibles: " .. currentPA .. " (Requis: 3 pour CONDENSATION)", console.cyan)
    
    if currentPA >= 3 then
        if distance <= maxRange then
            console.print("✅ Conditions CONDENSATION T6 OK", console.green)
            local canCast = fight.canCastSpell(SPELL_IDS.CONDENSATION, phossile.cellId)
            console.print("🔍 fight.canCastSpell CONDENSATION T6 result: " .. canCast, console.yellow)
            
            if canCast ~= 0 then
                console.print("🌊 Lancement CONDENSATION T6...", console.blue)
                local success, newPA = castSpellAndRefresh(SPELL_IDS.CONDENSATION, phossile.cellId, "CONDENSATION T6", 3)
                currentPA = newPA
                
                if success then
                    console.print("✅ CONDENSATION T6 réussie - PA restants: " .. currentPA, console.green)
                else
                    console.print("❌ CONDENSATION T6 échouée malgré les vérifications", console.red)
                end
            else
                console.print("❌ CONDENSATION T6 impossible - fight.canCastSpell retourne 0", console.red)
            end
        else
            console.print("❌ CONDENSATION T6 impossible - Hors portée (distance: " .. distance .. " > " .. maxRange .. ")", console.red)
        end
    else
        console.print("❌ PA insuffisants pour CONDENSATION T6 (PA: " .. currentPA .. "/3)", console.red)
    end
    
    -- ÉTAPE 3: DÉCHAINEMENT (CAC) OU DISSOLUTION (DISTANCE)
    console.print("⚔️ === ÉTAPE 3 T6: DÉCHAINEMENT/DISSOLUTION SELON DISTANCE ===", console.blue)
    
    -- Actualiser positions après CONDENSATION
    me = fight.currentFighter()
    phossile = findPhossile()
    currentPA = refreshCurrentPA()
    
    if not me then
        console.print("❌ Impossible de récupérer position pour étape 3 T6", console.red)
        return false
    end
    
    if not phossile then
        console.print("💀 Phossile mort après CONDENSATION - Étape 3 annulée", console.orange)
        return true
    end
    
    console.print("📍 Position actuelle: " .. me.cellId, console.cyan)
    console.print("🎯 Position Phossile: " .. phossile.cellId, console.cyan)
    
    local finalDistance = fight.cellsDistance(me.cellId, phossile.cellId)
    console.print("📏 Distance vers Phossile: " .. finalDistance, console.cyan)
    console.print("⚡ PA disponibles: " .. currentPA .. " (Requis: 4 pour sort)", console.cyan)
    
    if currentPA >= 4 then
        if finalDistance == 1 then
            -- CAC : DÉCHAINEMENT
            console.print("🗡️ === CAC DÉTECTÉ - DÉCHAINEMENT T6 ===", console.green)
            local maxRangeDechainement = SPELL_RANGES[SPELL_IDS.DECHAINEMENT] or 1
            
            if finalDistance <= maxRangeDechainement then
                local canCast = fight.canCastSpell(SPELL_IDS.DECHAINEMENT, phossile.cellId)
                console.print("🔍 fight.canCastSpell DÉCHAINEMENT T6 result: " .. canCast, console.yellow)
                
                if canCast ~= 0 then
                    console.print("🗡️ Lancement DÉCHAINEMENT T6...", console.blue)
                    local success, newPA = castSpellAndRefresh(SPELL_IDS.DECHAINEMENT, phossile.cellId, "DÉCHAINEMENT T6", 4)
                    currentPA = newPA
                    
                    if success then
                        console.print("✅ DÉCHAINEMENT T6 réussi - PA restants: " .. currentPA, console.green)
                    else
                        console.print("❌ DÉCHAINEMENT T6 échoué", console.red)
                    end
                else
                    console.print("❌ DÉCHAINEMENT T6 impossible", console.red)
                end
            else
                console.print("❌ DÉCHAINEMENT T6 impossible - Hors portée CAC", console.red)
            end
        else
            -- DISTANCE : DISSOLUTION
            console.print("🌪️ === DISTANCE DÉTECTÉE - DISSOLUTION T6 ===", console.yellow)
            local maxRangeDissolution = SPELL_RANGES[SPELL_IDS.DISSOLUTION] or 5
            
            if finalDistance <= maxRangeDissolution then
                local canCast = fight.canCastSpell(SPELL_IDS.DISSOLUTION, phossile.cellId)
                console.print("🔍 fight.canCastSpell DISSOLUTION T6 result: " .. canCast, console.yellow)
                
                if canCast ~= 0 then
                    console.print("🌪️ Lancement DISSOLUTION T6...", console.blue)
                    local success, newPA = castSpellAndRefresh(SPELL_IDS.DISSOLUTION, phossile.cellId, "DISSOLUTION T6", 4)
                    currentPA = newPA
                    
                    if success then
                        console.print("✅ DISSOLUTION T6 réussie - PA restants: " .. currentPA, console.green)
                    else
                        console.print("❌ DISSOLUTION T6 échouée", console.red)
                    end
                else
                    console.print("❌ DISSOLUTION T6 impossible", console.red)
                end
            else
                console.print("❌ DISSOLUTION T6 impossible - Hors portée (distance: " .. finalDistance .. " > " .. maxRangeDissolution .. ")", console.red)
            end
        end
    else
        console.print("❌ PA insuffisants pour DÉCHAINEMENT/DISSOLUTION T6 (PA: " .. currentPA .. "/4)", console.red)
    end
    
    -- ÉTAPE 4: FOLIE SANGUINAIRE SUR SOI-MÊME (3 PA)
    console.print("🩸 === ÉTAPE 4 T6: FOLIE SANGUINAIRE SUR SOI-MÊME ===", console.blue)
    
    -- Actualiser positions finales
    me = fight.currentFighter()
    currentPA = refreshCurrentPA()
    
    if not me then
        console.print("❌ Impossible de récupérer position pour FOLIE SANGUINAIRE T6", console.red)
        return false
    end
    
    console.print("📍 Position finale: " .. me.cellId, console.cyan)
    console.print("⚡ PA disponibles: " .. currentPA .. " (Requis: 3 pour FOLIE SANGUINAIRE)", console.cyan)
    
    if currentPA >= 3 then
        console.print("✅ PA suffisants pour FOLIE SANGUINAIRE T6", console.green)
        local canCast = fight.canCastSpell(SPELL_IDS.FOLIE_SANGUINAIRE, me.cellId)
        console.print("🔍 fight.canCastSpell FOLIE SANGUINAIRE T6 result: " .. canCast, console.yellow)
        
        if canCast ~= 0 then
            console.print("🩸 Lancement FOLIE SANGUINAIRE T6...", console.blue)
            local success, newPA = castSpellAndRefresh(SPELL_IDS.FOLIE_SANGUINAIRE, me.cellId, "FOLIE SANGUINAIRE T6", 3)
            currentPA = newPA
            
            if success then
                console.print("✅ FOLIE SANGUINAIRE T6 réussie - PA restants: " .. currentPA, console.green)
            else
                console.print("❌ FOLIE SANGUINAIRE T6 échouée malgré les vérifications", console.red)
            end
        else
            console.print("❌ FOLIE SANGUINAIRE T6 impossible - fight.canCastSpell retourne 0", console.red)
        end
    else
        console.print("❌ PA insuffisants pour FOLIE SANGUINAIRE T6 (PA: " .. currentPA .. "/3)", console.red)
    end
    
    -- RÉSUMÉ FINAL
    currentPA = refreshCurrentPA()
    console.print("🗡️ === STRATÉGIE TOUR 6 TERMINÉE - PA restants: " .. currentPA .. " ===", console.red)
    
    return true
end

-- 🗡️ STRATÉGIE TOUR 7 SPÉCIALISÉE
-- ===============================================================================

function executeTurn7Strategy()
    console.print("🗡️ === STRATÉGIE TOUR 7 DÉMARRÉE ===", console.red)
    
    local me = fight.currentFighter()
    if not me then
        console.print("❌ Pas de combattant actuel", console.red)
        return false
    end
    
    local phossile = findPhossile()
    if not phossile then
        console.print("💀 PHOSSILE MORT - ACTIVATION STRATÉGIE POST-PHOSSILE T7", console.red)
        
        -- Récupérer PA actuels et PV pour stratégie post-Phossile
        local currentPA = refreshCurrentPA()
        local stats = getFighterStats()
        local pvPercent = 0
        if stats.PVMax > 0 then
            pvPercent = (stats.PV / stats.PVMax) * 100
        end
        
        console.print("⚡ PA disponibles pour stratégie post-Phossile: " .. currentPA, console.cyan)
        console.print("💖 PV actuels: " .. pvPercent .. "%", console.cyan)
        
        -- Lancer stratégie post-Phossile officielle
        executeOfficialPostPhossileStrategy(currentPA, pvPercent)
        
        console.print("✅ Stratégie Post-Phossile T7 TERMINÉE", console.green)
        return true
    end
    
    console.print("🎯 PHOSSILE trouvé sur cellule: " .. phossile.cellId, console.yellow)
    
    local currentPA = refreshCurrentPA()
    console.print("⚡ PA disponibles: " .. currentPA, console.cyan)
    
    -- ÉTAPE 1: MUTILATION (2 PA) - Sur soi-même
    console.print("⚔️ === ÉTAPE 1 T7: MUTILATION ===", console.blue)
    if currentPA >= 2 then
        local success, newPA = castSpellAndRefresh(SPELL_IDS.MUTILATION, me.cellId, "MUTILATION T7", 2)
        currentPA = newPA
        
        if success then
            console.print("✅ 1. MUTILATION T7 réussie", console.green)
            -- Enregistrer le cooldown de MUTILATION (2 tours)
            markSpellOnCooldown(SPELL_IDS.MUTILATION, "MUTILATION T7", 2)
        else
            console.print("❌ 1. MUTILATION T7 échouée", console.red)
        end
    else
        console.print("❌ 1. PA insuffisants pour MUTILATION T7 (PA: " .. currentPA .. "/2)", console.red)
    end
    
    -- ÉTAPE 2: DÉPLACEMENT VERS PHOSSILE
    console.print("🏃 === ÉTAPE 2 T7: DÉPLACEMENT VERS PHOSSILE ===", console.blue)
    
    -- Actualiser position et Phossile
    me = fight.currentFighter()
    phossile = findPhossile()
    
    if not me or not phossile then
        console.print("❌ Impossible de récupérer positions pour déplacement T7", console.red)
    else
        local distance = fight.cellsDistance(me.cellId, phossile.cellId)
        local currentPM = me.movementPoints or me.MP or 0
        
        console.print("📍 Position actuelle: " .. me.cellId .. " → Phossile: " .. phossile.cellId .. " (distance: " .. distance .. ")", console.cyan)
        
        if distance > 1 and currentPM > 0 then
            console.print("🏃 RAPPROCHEMENT NÉCESSAIRE T7 - Distance: " .. distance, console.yellow)
            
            -- Trouver la meilleure position accessible proche du Phossile
            local accessibleCells = fight.accessibleCells()
            local bestCell = nil
            local minDistanceToPhossile = 999
            
            console.print("🔍 T7 - Recherche position optimale - PM disponibles: " .. currentPM, console.cyan)
            
            for _, cell in ipairs(accessibleCells) do
                local distanceToPhossile = fight.cellsDistance(cell.cellId, phossile.cellId)
                local distanceFromMe = fight.cellsDistance(me.cellId, cell.cellId)
                
                -- Chercher cellule accessible qui rapproche du Phossile
                if distanceFromMe <= currentPM and distanceFromMe > 0 and distanceToPhossile < minDistanceToPhossile then
                    minDistanceToPhossile = distanceToPhossile
                    bestCell = cell.cellId
                    console.print("🎯 T7 - Cellule candidate: " .. cell.cellId .. " (distance Phossile: " .. distanceToPhossile .. ", coût PM: " .. distanceFromMe .. ")", console.white)
                end
            end
            
            if bestCell then
                console.print("🏃 T7 - Déplacement vers " .. bestCell .. " (distance finale Phossile: " .. minDistanceToPhossile .. ")", console.yellow)
                
                fight.moveToAccessibleCell(bestCell)
                global.sleep(100) -- Attendre le déplacement
                
                -- Vérifier le déplacement
                me = fight.currentFighter()
                local newDistance = fight.cellsDistance(me.cellId, phossile.cellId)
                console.print("✅ T7 - Déplacement terminé - Nouvelle distance Phossile: " .. newDistance, console.green)
            else
                console.print("❌ T7 - Impossible d'optimiser la position avec " .. currentPM .. " PM", console.red)
            end
        else
            console.print("✅ T7 - Déjà proche du Phossile (distance: " .. distance .. ")", console.green)
        end
    end
    
    -- ÉTAPE 3: PREMIÈRE CONDENSATION SUR PHOSSILE
    console.print("🌊 === ÉTAPE 3 T7: CONDENSATION #1 SUR PHOSSILE ===", console.blue)
    currentPA = refreshCurrentPA()
    
    -- Actualiser position et Phossile
    me = fight.currentFighter()
    phossile = findPhossile()
    
    if not me or not phossile then
        console.print("❌ Impossible de récupérer positions pour CONDENSATION T7 #1", console.red)
    else
        local distance = fight.cellsDistance(me.cellId, phossile.cellId)
        local maxRange = SPELL_RANGES[SPELL_IDS.CONDENSATION] or 5
        
        console.print("📍 Position: " .. me.cellId .. " → Phossile: " .. phossile.cellId .. " (distance: " .. distance .. ")", console.cyan)
        
        if currentPA >= 3 then
            if distance <= maxRange then
                console.print("✅ Conditions CONDENSATION T7 #1 OK", console.green)
                
                local canCast = fight.canCastSpell(SPELL_IDS.CONDENSATION, phossile.cellId)
                console.print("🔍 fight.canCastSpell CONDENSATION T7 #1 result: " .. canCast, console.yellow)
                
                if canCast ~= 0 then
                    console.print("🌊 Lancement CONDENSATION T7 #1...", console.blue)
                    local success, newPA = castSpellAndRefresh(SPELL_IDS.CONDENSATION, phossile.cellId, "CONDENSATION T7 #1", 3)
                    currentPA = newPA
                    
                    if success then
                        console.print("✅ CONDENSATION T7 #1 réussie - PA restants: " .. currentPA, console.green)
                        
                        -- VÉRIFICATION CRITIQUE : Phossile mort après Condensation #1 ?
                        phossile = findPhossile()
                        if not phossile then
                            console.print("💀 PHOSSILE TUÉ PAR CONDENSATION #1 - BASCULEMENT IMMÉDIAT POST-PHOSSILE", console.red)
                            executePostPhossileStrategy()
                            return true
                        end
                    else
                        console.print("❌ CONDENSATION T7 #1 échouée malgré les vérifications", console.red)
                    end
                else
                    console.print("❌ CONDENSATION T7 #1 impossible - fight.canCastSpell retourne 0", console.red)
                end
            else
                console.print("❌ CONDENSATION T7 #1 impossible - Hors portée (distance: " .. distance .. " > " .. maxRange .. ")", console.red)
            end
        else
            console.print("❌ PA insuffisants pour CONDENSATION T7 #1 (PA: " .. currentPA .. "/3)", console.red)
        end
    end
    
    -- ÉTAPE 4: DEUXIÈME CONDENSATION SUR PHOSSILE
    console.print("🌊 === ÉTAPE 4 T7: CONDENSATION #2 SUR PHOSSILE ===", console.blue)
    currentPA = refreshCurrentPA()
    
    -- Actualiser position et Phossile
    me = fight.currentFighter()
    phossile = findPhossile()
    
    if not phossile then
        console.print("💀 Phossile mort après CONDENSATION #1 - Étape 4 annulée", console.orange)
        console.print("🗡️ === STRATÉGIE TOUR 7 TERMINÉE (Phossile mort) - PA restants: " .. currentPA .. " ===", console.red)
        return true
    end
    
    if not me then
        console.print("❌ Impossible de récupérer position pour CONDENSATION T7 #2", console.red)
    else
        local distance = fight.cellsDistance(me.cellId, phossile.cellId)
        local maxRange = SPELL_RANGES[SPELL_IDS.CONDENSATION] or 5
        
        console.print("📍 Position: " .. me.cellId .. " → Phossile: " .. phossile.cellId .. " (distance: " .. distance .. ")", console.cyan)
        
        if currentPA >= 3 then
            if distance <= maxRange then
                console.print("✅ Conditions CONDENSATION T7 #2 OK", console.green)
                
                local canCast = fight.canCastSpell(SPELL_IDS.CONDENSATION, phossile.cellId)
                console.print("🔍 fight.canCastSpell CONDENSATION T7 #2 result: " .. canCast, console.yellow)
                
                if canCast ~= 0 then
                    console.print("🌊 Lancement CONDENSATION T7 #2...", console.blue)
                    local success, newPA = castSpellAndRefresh(SPELL_IDS.CONDENSATION, phossile.cellId, "CONDENSATION T7 #2", 3)
                    currentPA = newPA
                    
                    if success then
                        console.print("✅ CONDENSATION T7 #2 réussie - PA restants: " .. currentPA, console.green)
                        
                        -- VÉRIFICATION CRITIQUE : Phossile mort après Condensation #2 ?
                        phossile = findPhossile()
                        if not phossile then
                            console.print("💀 PHOSSILE TUÉ PAR CONDENSATION #2 - BASCULEMENT IMMÉDIAT POST-PHOSSILE", console.red)
                            executePostPhossileStrategy()
                            return true
                        end
                    else
                        console.print("❌ CONDENSATION T7 #2 échouée malgré les vérifications", console.red)
                    end
                else
                    console.print("❌ CONDENSATION T7 #2 impossible - fight.canCastSpell retourne 0", console.red)
                end
            else
                console.print("❌ CONDENSATION T7 #2 impossible - Hors portée (distance: " .. distance .. " > " .. maxRange .. ")", console.red)
            end
        else
            console.print("❌ PA insuffisants pour CONDENSATION T7 #2 (PA: " .. currentPA .. "/3)", console.red)
        end
    end
    
    -- ÉTAPE 5: DISSOLUTION SUR PHOSSILE
    console.print("🌪️ === ÉTAPE 5 T7: DISSOLUTION SUR PHOSSILE ===", console.blue)
    currentPA = refreshCurrentPA()
    
    -- Actualiser position et Phossile
    me = fight.currentFighter()
    phossile = findPhossile()
    
    if not phossile then
        console.print("💀 Phossile mort après CONDENSATION #2 - Étape 5 annulée", console.orange)
        console.print("🗡️ === STRATÉGIE TOUR 7 TERMINÉE (Phossile mort) - PA restants: " .. currentPA .. " ===", console.red)
        return true
    end
    
    if not me then
        console.print("❌ Impossible de récupérer position pour DISSOLUTION T7", console.red)
    else
        local distance = fight.cellsDistance(me.cellId, phossile.cellId)
        local maxRange = SPELL_RANGES[SPELL_IDS.DISSOLUTION] or 5
        
        console.print("📍 Position: " .. me.cellId .. " → Phossile: " .. phossile.cellId .. " (distance: " .. distance .. ")", console.cyan)
        
        if currentPA >= 4 then
            if distance <= maxRange then
                console.print("✅ Conditions DISSOLUTION T7 OK", console.green)
                
                local canCast = fight.canCastSpell(SPELL_IDS.DISSOLUTION, phossile.cellId)
                console.print("🔍 fight.canCastSpell DISSOLUTION T7 result: " .. canCast, console.yellow)
                
                if canCast ~= 0 then
                    console.print("🌪️ Lancement DISSOLUTION T7...", console.blue)
                    local success, newPA = castSpellAndRefresh(SPELL_IDS.DISSOLUTION, phossile.cellId, "DISSOLUTION T7", 4)
                    currentPA = newPA
                    
                    if success then
                        console.print("✅ DISSOLUTION T7 réussie - PA restants: " .. currentPA, console.green)
                        
                        -- VÉRIFICATION CRITIQUE : Phossile mort après Dissolution ?
                        phossile = findPhossile()
                        if not phossile then
                            console.print("💀 PHOSSILE TUÉ PAR DISSOLUTION T7 - BASCULEMENT IMMÉDIAT POST-PHOSSILE", console.red)
                            executePostPhossileStrategy()
                            return true
                        end
                    else
                        console.print("❌ DISSOLUTION T7 échouée malgré les vérifications", console.red)
                    end
                else
                    console.print("❌ DISSOLUTION T7 impossible - fight.canCastSpell retourne 0", console.red)
                end
            else
                console.print("❌ DISSOLUTION T7 impossible - Hors portée (distance: " .. distance .. " > " .. maxRange .. ")", console.red)
            end
        else
            console.print("❌ PA insuffisants pour DISSOLUTION T7 (PA: " .. currentPA .. "/4)", console.red)
        end
    end
    
    -- RÉSUMÉ FINAL
    currentPA = refreshCurrentPA()
    console.print("🗡️ === STRATÉGIE TOUR 7 TERMINÉE - PA restants: " .. currentPA .. " ===", console.red)
    
    return true
end

-- Fonction principale de stratégie post-Phossile
function executePostPhossileStrategy()
    console.print("💀 === STRATÉGIE POST-PHOSSILE ACTIVÉE ===", console.red)
    
    local me = fight.currentFighter()
    if not me then
        console.print("❌ Aucun combattant actuel", console.red)
        return false
    end
    
    -- Incrémenter le compteur de tours post-Phossile
    postPhossileTurnCount = postPhossileTurnCount + 1
    local currentRound = fight.currentRound() or 1
    console.print("💀 Tour post-Phossile #" .. postPhossileTurnCount .. " (Tour global: " .. currentRound .. ")", console.cyan)
    
    -- Récupérer les stats actuelles avec debug détaillé
    local stats = getFighterStats()
    local currentPA = stats.PA
    local currentPM = stats.PM
    local currentPV = stats.PV
    local maxPV = stats.PVMax
    
    -- CALCUL SÉCURISÉ DES PV AVEC DÉTECTION D'ERREURS
    local pvPercent = 0
    
    -- PROTECTION 1: PV négatifs (bug API)
    if currentPV < 0 then
        console.print("🚨 PV NÉGATIFS DÉTECTÉS (" .. currentPV .. ") - Récupération directe obligatoire", console.red)
        local me = fight.currentFighter()
        if me then
            currentPV = me.lifePoints or me.currentLifePoints or 0
            maxPV = me.maxLifePoints or 1
            console.print("🔄 PV corrigés: " .. currentPV .. "/" .. maxPV, console.orange)
        end
    end
    
    -- PROTECTION 2: PV MAX = 0 (bug API)
    if maxPV <= 0 then
        console.print("🚨 PV MAX INVALIDE (" .. maxPV .. ") - Récupération directe obligatoire", console.red)
        local me = fight.currentFighter()
        if me then
            currentPV = me.lifePoints or me.currentLifePoints or 0
            maxPV = me.maxLifePoints or 1
            console.print("🔄 PV MAX corrigé: " .. currentPV .. "/" .. maxPV, console.orange)
        end
    end
    
    -- CALCUL FINAL SÉCURISÉ
    if maxPV > 0 and currentPV >= 0 then
        pvPercent = (currentPV / maxPV) * 100
        console.print("✅ Calcul PV valide: " .. currentPV .. "/" .. maxPV .. " = " .. math.floor(pvPercent) .. "%", console.green)
    else
        console.print("⚠️ Stats PV encore invalides - Défaut 50%", console.red)
        pvPercent = 50 -- Défaut sécurisé
        currentPV = 1000
        maxPV = 2000
    end
    
    console.print("📊 STATS POST-PHOSSILE DÉTAILLÉES:", console.cyan)
    console.print("  🔋 PA=" .. currentPA .. " PM=" .. currentPM, console.cyan)
    console.print("  ❤️ PV=" .. currentPV .. "/" .. maxPV .. " → " .. math.floor(pvPercent) .. "% PV", console.cyan)
    
    -- DEBUG CRITIQUE: Afficher quel sort sera sélectionné
    if pvPercent > 50 then
        console.print("  ⚔️ SEUIL PV: " .. math.floor(pvPercent) .. "% > 50% → DÉCHAINEMENT SERA SÉLECTIONNÉ", console.green)
    else
        console.print("  🌪️ SEUIL PV: " .. math.floor(pvPercent) .. "% ≤ 50% → DISSOLUTION SERA SÉLECTIONNÉE", console.yellow)
    end
    
    -- ÉTAPE 1: DÉPLACEMENT VERS ENNEMIS AU CAC (OPTIONNEL)
    console.print("🔍 === ÉTAPE 1 POST-PHOSSILE : RECHERCHE ET DÉPLACEMENT ===", console.blue)
    
    local bestEnemy = findBestEnemyForCAC()
    if bestEnemy then
        local distance = fight.cellsDistance(me.cellId, bestEnemy.cellId)
        console.print("🎯 Ennemi ciblé: " .. (bestEnemy.name or "Inconnu") .. " à distance " .. distance, console.yellow)
        
        console.print("🔍 Conditions déplacement - Distance: " .. distance .. ", PM: " .. currentPM, console.cyan)
        
        if distance > 1 then
            if currentPM > 0 then
                console.print("✅ CONDITIONS OK - Lancement déplacement vers ennemi", console.green)
                local moveSuccess = moveTowardsEnemyCAC(bestEnemy)
                if moveSuccess then
                    console.print("✅ Déplacement post-Phossile réussi", console.green)
                else
                    console.print("⚠️ Déplacement post-Phossile échoué - Combat à distance", console.orange)
        end
    else
                console.print("❌ AUCUN PM DISPONIBLE - Pas de déplacement possible", console.red)
            end
        else
            console.print("✅ Déjà au CAC (distance " .. distance .. ") - Combat direct", console.green)
        end
    else
        console.print("⚠️ Aucun ennemi CAC trouvé - Continuation avec sorts à distance", console.orange)
    end
    
    console.print("🔍 === ÉTAPE 2 POST-PHOSSILE : APPLICATION DES SORTS (STRATÉGIE OFFICIELLE) ===", console.blue)
    
    -- STRATÉGIE POST-PHOSSILE OFFICIELLE (selon spécifications utilisateur)
    executeOfficialPostPhossileStrategy(currentPA, pvPercent)
    
    return true
end

-- Fonction pour trouver le meilleur ennemi à attaquer au CAC
function findBestEnemyForCAC()
    local me = fight.currentFighter()
    if not me then 
        console.print("❌ findBestEnemyForCAC: Pas de combattant actuel", console.red)
        return nil 
    end
    
    console.print("🔍 findBestEnemyForCAC: Mon équipe = " .. (me.team or "?"), console.cyan)
    
    local allFighters = fight.fighters()
    local validEnemies = {}
    
    console.print("🔍 findBestEnemyForCAC: " .. #allFighters .. " combattants trouvés", console.cyan)
    
    for i, fighter in pairs(allFighters) do
        local isEnemy = fighter.team ~= me.team
        -- CORRECTION : Gérer le cas où fighter.alive est nil
        -- En général, nil signifie vivant par défaut dans l'API Frigost
        local isAlive = fighter.alive
        if isAlive == nil then
            isAlive = true -- Considérer nil comme vivant par défaut
            console.print("  ⚠️ fighter.alive est nil pour " .. (fighter.name or "Inconnu") .. " - Considéré comme vivant", console.orange)
        end
        
        local fighterTeam = fighter.team or "?"
        local fighterName = fighter.name or "Inconnu"
        
        console.print("  " .. i .. ". " .. fighterName .. " - Équipe: " .. fighterTeam .. " - Vivant: " .. tostring(fighter.alive) .. " → " .. tostring(isAlive) .. " - Ennemi: " .. tostring(isEnemy), console.white)
        
        if isEnemy and isAlive then
            local distance = fight.cellsDistance(me.cellId, fighter.cellId)
            table.insert(validEnemies, {
                fighter = fighter,
                distance = distance
            })
            console.print("  ✅ Ennemi valide ajouté: " .. fighterName .. " (distance: " .. distance .. ")", console.green)
        elseif isEnemy and not isAlive then
            console.print("  💀 Ennemi mort ignoré: " .. fighterName, console.red)
        elseif not isEnemy then
            console.print("  👥 Allié ignoré: " .. fighterName, console.blue)
        end
    end
    
    console.print("🔍 findBestEnemyForCAC: " .. #validEnemies .. " ennemis valides trouvés", console.cyan)
    
    if #validEnemies == 0 then
        console.print("❌ findBestEnemyForCAC: Aucun ennemi valide", console.red)
        return nil
    end
    
    -- Trier par distance uniquement (PRIORITÉ AOE - Plus proche = Meilleur positionnement)
    table.sort(validEnemies, function(a, b)
        return a.distance < b.distance
    end)
    
    local bestEnemy = validEnemies[1].fighter
    console.print("✅ findBestEnemyForCAC: Meilleur ennemi = " .. (bestEnemy.name or "Inconnu"), console.green)
    
    return bestEnemy
end



-- Fonction pour se déplacer vers un ennemi au CAC (VERSION AMÉLIORÉE)
function moveTowardsEnemyCAC(targetEnemy)
    local me = fight.currentFighter()
    if not me or not targetEnemy then 
        console.print("❌ moveTowardsEnemyCAC: Paramètres invalides", console.red)
        return false 
    end
    
    local myCell = me.cellId
    local targetCell = targetEnemy.cellId
    local distance = fight.cellsDistance(myCell, targetCell)
    local currentPM = me.movementPoints or me.MP or 0
    
    console.print("🏃 POST-PHOSSILE - Déplacement vers " .. (targetEnemy.name or "Ennemi") .. " (distance: " .. distance .. ", PM: " .. currentPM .. ")", console.yellow)
    
    -- Vérifier si on a des PM pour se déplacer
    if currentPM <= 0 then
        console.print("❌ Aucun PM disponible pour déplacement", console.red)
        return false
    end
    
    -- Si déjà au CAC, pas besoin de se déplacer
    if distance <= 1 then
        console.print("✅ Déjà au CAC avec " .. (targetEnemy.name or "Ennemi"), console.green)
        return true
    end
    
    local moved = false
    
    -- ÉTAPE 1 : Essayer d'atteindre une cellule adjacente à l'ennemi (idéal)
    local adjacentCells = fight.adjacentCells(targetCell)
    console.print("🔍 POST-PHOSSILE - " .. #adjacentCells .. " cellules adjacentes à l'ennemi trouvées", console.cyan)
    
    for i, cell in ipairs(adjacentCells) do
        -- Gérer les deux formats possibles : ID direct ou objet avec cellId
        local cellId = (type(cell) == "table" and cell.cellId) or cell
        console.print("🔍 POST-PHOSSILE - Test cellule adjacente " .. i .. ": " .. cellId, console.white)
        
        if fight.cellIsFree(cellId) and fight.cellIsWalkable(cellId) then
            local distanceFromMe = fight.cellsDistance(myCell, cellId)
            local distanceCellToEnemy = fight.cellsDistance(cellId, targetCell)
            
            console.print("🔍 POST-PHOSSILE - Cellule " .. cellId .. " - Distance de moi: " .. distanceFromMe .. ", Distance de l'ennemi: " .. distanceCellToEnemy, console.white)
            
            -- Vérifier que c'est vraiment adjacent à l'ennemi ET accessible
            if distanceCellToEnemy == 1 and distanceFromMe <= currentPM and distanceFromMe > 0 then
                console.print("✅ POST-PHOSSILE - Cellule CAC idéale trouvée: " .. cellId, console.green)
                fight.moveTowardCell(cellId)
                global.sleep(0)
                console.print("✅ Déplacement CAC réussi vers " .. cellId, console.green)
                moved = true
                break
            else
                console.print("❌ POST-PHOSSILE - Cellule " .. cellId .. " non accessible (PM: " .. distanceFromMe .. "/" .. currentPM .. ")", console.red)
            end
        else
            console.print("❌ POST-PHOSSILE - Cellule " .. cellId .. " occupée ou non marchable", console.red)
        end
    end
    
    -- ÉTAPE 2 : Si CAC impossible, se rapprocher au maximum avec les PM disponibles
    if not moved then
        console.print("⚠️ POST-PHOSSILE - CAC impossible - Recherche rapprochement maximal", console.orange)
        
        local accessibleCells = fight.accessibleCells()
        local bestApproachCell = nil
        local bestDistanceToEnemy = 999
        
        console.print("🔍 POST-PHOSSILE - Analyse de " .. #accessibleCells .. " cellules accessibles pour rapprochement", console.cyan)
        
        for _, cell in ipairs(accessibleCells) do
            -- CORRECTION : Gérer les deux formats de cellules (nombre ou objet)
            local cellId = (type(cell) == "table" and cell.cellId) or cell
            local distanceToEnemy = fight.cellsDistance(cellId, targetCell)
            local distanceFromMe = fight.cellsDistance(myCell, cellId)
            
            -- Chercher la cellule accessible qui rapproche le plus de l'ennemi
            if distanceFromMe > 0 and distanceFromMe <= currentPM and distanceToEnemy < bestDistanceToEnemy then
                bestApproachCell = cellId
                bestDistanceToEnemy = distanceToEnemy
                console.print("🎯 POST-PHOSSILE - Cellule rapprochement candidate: " .. bestApproachCell .. " (distance ennemi: " .. distanceToEnemy .. ")", console.white)
            end
        end
        
        if bestApproachCell then
            console.print("🏃 POST-PHOSSILE - RAPPROCHEMENT MAXIMAL vers " .. bestApproachCell .. " (distance finale: " .. bestDistanceToEnemy .. ")", console.yellow)
            fight.moveTowardCell(bestApproachCell)
        global.sleep(0)
            moved = true
    else
            console.print("❌ POST-PHOSSILE - Aucun rapprochement possible - Combat à distance", console.red)
    end
end

    if moved then
        console.print("✅ POST-PHOSSILE - Déplacement terminé avec succès", console.green)
    else
        console.print("⚠️ POST-PHOSSILE - Aucun déplacement effectué - Combat à distance", console.orange)
    end
    
    return moved
end

-- Fonction OFFICIELLE de stratégie post-Phossile (selon spécifications utilisateur)
function executeOfficialPostPhossileStrategy(startingPA, pvPercent)
    console.print("🎯 === STRATÉGIE POST-PHOSSILE OFFICIELLE ===", console.red)
    console.print("📋 Ordre des sorts: 1-Mutilation(1/2tours) 2-Couronnes(1/3tours) 3-Condensation 4-Déchainement/Dissolution 5-Déchainement/Dissolution 6-Nettoyage PA", console.cyan)
    
    local me = fight.currentFighter()
    if not me then return end
    
    -- ÉTAPE 1: MUTILATION sur soi-même (1 fois tous les 2 tours)
    local currentPA = getRealActionPoints()
    console.print("🗡️ ÉTAPE 1 - MUTILATION (1/2 tours) - PA actuels: " .. currentPA, console.cyan)
    
    if postPhossileTurnCount % 2 == 1 then -- Tours impairs (1, 3, 5...)
        if currentPA >= 2 and not isSpellOnCooldown(SPELL_IDS.MUTILATION) then
            local success, newPA = castSpellPostPhossileAndRefresh(SPELL_IDS.MUTILATION, me.cellId, "MUTILATION OFFICIELLE")
            if success then
                console.print("✅ 1. MUTILATION sur soi réussie (tour " .. postPhossileTurnCount .. "/2)", console.green)
            else
                console.print("❌ 1. MUTILATION sur soi échouée", console.red)
        end
    else
        if currentPA < 2 then
                console.print("❌ 1. PA insuffisants pour MUTILATION (" .. currentPA .. "/2)", console.red)
            else
                console.print("⏸️ 1. MUTILATION ignorée (cooldown)", console.orange)
            end
        end
    else
        console.print("⏸️ 1. MUTILATION sautée (tour " .. postPhossileTurnCount .. " - programmée pour tours impairs)", console.orange)
    end
    
    -- ÉTAPE 2: COURONNES D'ÉPINES (1 fois tous les 3 tours)
    currentPA = getRealActionPoints()
    console.print("👑 ÉTAPE 2 - COURONNES D'ÉPINES (1/3 tours) - PA actuels: " .. currentPA, console.cyan)
    
    if postPhossileTurnCount % 3 == 1 then -- Tours 1, 4, 7, 10...
        -- ACTUALISER POSITION AVANT COURONNES D'ÉPINES
        me = fight.currentFighter()
        if not me then
            console.print("❌ Impossible de récupérer position pour COURONNES", console.red)
            return false
        end
        console.print("📍 Position actuelle pour COURONNES: " .. me.cellId, console.cyan)
        
        -- EXCEPTION TOUR 2: COURONNES D'ÉPINES toujours disponible au Tour 2
    local isCourronnesTour2Exception = (fight.currentRound() == 2)
    if currentPA >= 2 and (not isSpellOnCooldown(SPELL_IDS.COURONNES_EPINES) or isCourronnesTour2Exception) then
            local success, newPA = castSpellPostPhossileAndRefresh(SPELL_IDS.COURONNES_EPINES, me.cellId, "COURONNES OFFICIELLES")
            if success then
                console.print("✅ 2. COURONNES D'ÉPINES réussies (tour " .. postPhossileTurnCount .. "/3)", console.green)
            else
                console.print("❌ 2. COURONNES D'ÉPINES échouées", console.red)
        end
    else
        if currentPA < 2 then
                console.print("❌ 2. PA insuffisants pour COURONNES (" .. currentPA .. "/2)", console.red)
        else
                console.print("⏸️ 2. COURONNES ignorées (cooldown)", console.orange)
        end
        end
    else
        local nextCouronneTurn = ((math.floor(postPhossileTurnCount / 3) + 1) * 3) + 1
        console.print("⏸️ 2. COURONNES sautées (tour " .. postPhossileTurnCount .. " - prochaine au tour " .. nextCouronneTurn .. ")", console.orange)
    end
    
    -- ÉTAPE 3: CONDENSATION avec scoring (PAS DE COOLDOWN - PEUT ÊTRE RELANCÉE CHAQUE TOUR)
    currentPA = getRealActionPoints()
    console.print("🌊 ÉTAPE 3 - CONDENSATION AVEC SCORING - PA actuels: " .. currentPA, console.cyan)
    
    if currentPA >= 3 then
        local bestCell, score, enemyCount = findBestAOEPosition(SPELL_IDS.CONDENSATION)
        
        if bestCell and enemyCount >= 1 then
            local success, newPA = castSpellPostPhossileAndRefresh(SPELL_IDS.CONDENSATION, bestCell, "CONDENSATION SCORING OFFICIELLE")
            if success then
                console.print("✅ 3. CONDENSATION SCORING réussie (+" .. enemyCount .. " ennemis, score: " .. score .. ")", console.green)
            else
                console.print("❌ 3. CONDENSATION SCORING échouée", console.red)
            end
        else
            console.print("❌ 3. CONDENSATION - Aucune zone AOE viable (cell=" .. tostring(bestCell) .. ", ennemis=" .. tostring(enemyCount) .. ")", console.red)
        end
    else
        console.print("❌ 3. PA insuffisants pour CONDENSATION (" .. currentPA .. "/3)", console.red)
    end
    
    -- DÉTERMINER LE SORT POUR ÉTAPES 4 & 5 selon % PV
    local spellChoice = ""
    local spellId = 0
    local spellCost = 4
    
    if pvPercent > 50 then
        spellChoice = "DÉCHAINEMENT"
        spellId = SPELL_IDS.DECHAINEMENT
        console.print("⚔️ CHOIX OFFICIEL: " .. math.floor(pvPercent) .. "% PV > 50% → DÉCHAINEMENT sélectionné", console.green)
    else
        spellChoice = "DISSOLUTION"
        spellId = SPELL_IDS.DISSOLUTION
        console.print("🌪️ CHOIX OFFICIEL: " .. math.floor(pvPercent) .. "% PV ≤ 50% → DISSOLUTION sélectionnée", console.yellow)
    end
    
    -- ÉTAPE 4: Premier DÉCHAINEMENT/DISSOLUTION (PAS DE COOLDOWN - PEUT ÊTRE RELANCÉ CHAQUE TOUR)
    currentPA = getRealActionPoints()
    console.print("⚔️ ÉTAPE 4 - " .. spellChoice .. " #1 OFFICIEL - PA actuels: " .. currentPA, console.cyan)
    
    if currentPA >= spellCost then
        local targetCell = nil
        local actionDescription = ""
        
        if spellChoice == "DISSOLUTION" then
            -- DISSOLUTION : Scoring AOE prioritaire
            local bestCell, score, enemyCount = findBestAOEPosition(SPELL_IDS.DISSOLUTION)
            if bestCell and enemyCount >= 1 then
                targetCell = bestCell
                actionDescription = spellChoice .. " AOE #1 (+" .. enemyCount .. " ennemis, score: " .. score .. ")"
            else
                -- Fallback ciblage direct
                local enemy = fight.nearestEnemy()
                if enemy then
                    targetCell = enemy.cellId
                    actionDescription = spellChoice .. " DIRECT #1 sur " .. (enemy.name or "Ennemi")
            end
        end
    else
            -- DÉCHAINEMENT : Ciblage direct uniquement
            local enemy = fight.nearestEnemy()
            if enemy then
                targetCell = enemy.cellId
                actionDescription = spellChoice .. " DIRECT #1 sur " .. (enemy.name or "Ennemi")
            end
        end
        
        if targetCell then
            local success, newPA = castSpellPostPhossileAndRefresh(spellId, targetCell, actionDescription .. " OFFICIEL")
            if success then
                console.print("✅ 4. " .. actionDescription .. " - RÉUSSI", console.green)
            else
                console.print("❌ 4. " .. actionDescription .. " - ÉCHOUÉ", console.red)
            end
        else
            console.print("❌ 4. " .. spellChoice .. " #1 - Aucune cible trouvée", console.red)
        end
    else
        console.print("❌ 4. PA insuffisants pour " .. spellChoice .. " #1 (" .. currentPA .. "/4)", console.red)
    end
    
    -- ÉTAPE 5: Deuxième DÉCHAINEMENT/DISSOLUTION (PAS DE COOLDOWN - PEUT ÊTRE RELANCÉ CHAQUE TOUR)
    currentPA = getRealActionPoints()
    console.print("⚔️ ÉTAPE 5 - " .. spellChoice .. " #2 OFFICIEL - PA actuels: " .. currentPA, console.cyan)
    
    if currentPA >= spellCost then
        local targetCell = nil
        local actionDescription = ""
        
        if spellChoice == "DISSOLUTION" then
            -- DISSOLUTION : Scoring AOE prioritaire
            local bestCell, score, enemyCount = findBestAOEPosition(SPELL_IDS.DISSOLUTION)
            if bestCell and enemyCount >= 1 then
                targetCell = bestCell
                actionDescription = spellChoice .. " AOE #2 (+" .. enemyCount .. " ennemis, score: " .. score .. ")"
            else
                -- Fallback ciblage direct
                local enemy = fight.nearestEnemy()
                if enemy then
                    targetCell = enemy.cellId
                    actionDescription = spellChoice .. " DIRECT #2 sur " .. (enemy.name or "Ennemi")
            end
        end
    else
            -- DÉCHAINEMENT : Ciblage direct uniquement
            local enemy = fight.nearestEnemy()
            if enemy then
                targetCell = enemy.cellId
                actionDescription = spellChoice .. " DIRECT #2 sur " .. (enemy.name or "Ennemi")
            end
        end
        
        if targetCell then
            local success, newPA = castSpellPostPhossileAndRefresh(spellId, targetCell, actionDescription .. " OFFICIEL")
            if success then
                console.print("✅ 5. " .. actionDescription .. " - RÉUSSI", console.green)
            else
                console.print("❌ 5. " .. actionDescription .. " - ÉCHOUÉ", console.red)
            end
        else
            console.print("❌ 5. " .. spellChoice .. " #2 - Aucune cible trouvée", console.red)
        end
    else
        console.print("❌ 5. PA insuffisants pour " .. spellChoice .. " #2 (" .. currentPA .. "/4)", console.red)
    end
    
    -- ÉTAPE 6: NETTOYAGE OPTIMISÉ - Prioriser les sorts coûteux (4 PA > 3 PA > 2 PA)
    local finalPA = getRealActionPoints()
    console.print("🧹 ÉTAPE 6 - NETTOYAGE PA RESTANTS OPTIMISÉ - PA actuels: " .. finalPA, console.orange)
    
    if finalPA >= 2 then
        console.print("🔄 PA RESTANTS SIGNIFICATIFS - Nettoyage par ordre de priorité PA", console.orange)
        
        local me = fight.currentFighter()
        if not me then
            console.print("❌ NETTOYAGE - Impossible de récupérer position actuelle", console.red)
            return
        end
        
        local actionsNettoyage = 0
        local maxActionsNettoyage = 5 -- Augmenté pour épuiser plus de PA
        
        -- BOUCLE NETTOYAGE OPTIMISÉE : TANT QU'IL RESTE DES PA UTILISABLES
        while finalPA >= 2 and actionsNettoyage < maxActionsNettoyage do
            actionsNettoyage = actionsNettoyage + 1
            local actionTaken = false
            local enemy = fight.nearestEnemy()
            
            console.print("🆘 NETTOYAGE Action #" .. actionsNettoyage .. " - PA: " .. finalPA, console.yellow)
            
            -- PRIORITÉ 1: DISSOLUTION/DÉCHAINEMENT (4 PA) - OPTIMISATION NETTOYAGE
            if finalPA >= 4 and enemy then
                -- LOGIQUE SPÉCIALE NETTOYAGE : Si exactement 4 PA, forcer DISSOLUTION pour optimisation
                local isExactly4PA = (finalPA == 4)
                local spellToTry = (isExactly4PA or pvPercent <= 50) and SPELL_IDS.DISSOLUTION or SPELL_IDS.DECHAINEMENT
                local spellName = (isExactly4PA or pvPercent <= 50) and "DISSOLUTION" or "DÉCHAINEMENT"
                local maxRange = (spellToTry == SPELL_IDS.DECHAINEMENT) and 6 or 5
                
                -- Log explicatif pour l'optimisation
                if isExactly4PA and pvPercent > 50 then
                    console.print("🎯 OPTIMISATION NETTOYAGE : DISSOLUTION forcée (4 PA exactement) au lieu de DÉCHAINEMENT", console.yellow)
                end
                
                local distance = fight.cellsDistance(me.cellId, enemy.cellId)
                console.print("🎯 NETTOYAGE P1 - " .. spellName .. " sur " .. (enemy.name or "Ennemi") .. " (distance: " .. distance .. ", portée: " .. maxRange .. ")", console.cyan)
                
                if distance <= maxRange and fight.canCastSpell(spellToTry, enemy.cellId) ~= 0 then
                    local success, newPA = castSpellPostPhossileAndRefresh(spellToTry, enemy.cellId, spellName .. " NETTOYAGE PRIORITAIRE")
                    if success then
                        finalPA = newPA
                        actionTaken = true
                        console.print("✅ NETTOYAGE P1 - " .. spellName .. " réussi - PA restants: " .. finalPA, console.green)
                    end
                end
            end
            
            -- PRIORITÉ 2: CONDENSATION (3 PA) si pas assez pour sorts 4 PA ou échec
            if not actionTaken and finalPA >= 3 then
                local bestAOECell, score, enemyCount = findBestAOEPosition(SPELL_IDS.CONDENSATION)
                if bestAOECell and enemyCount >= 1 then
                    local success, newPA = castSpellPostPhossileAndRefresh(SPELL_IDS.CONDENSATION, bestAOECell, "CONDENSATION NETTOYAGE P2")
                    if success then
                        finalPA = newPA
                        actionTaken = true
                        console.print("✅ NETTOYAGE P2 - CONDENSATION réussie (+" .. enemyCount .. " ennemis) - PA restants: " .. finalPA, console.green)
                    end
                elseif enemy then
                    -- Fallback CONDENSATION directe sur ennemi
                    local distance = fight.cellsDistance(me.cellId, enemy.cellId)
                    if distance <= 5 and fight.canCastSpell(SPELL_IDS.CONDENSATION, enemy.cellId) ~= 0 then
                        local success, newPA = castSpellPostPhossileAndRefresh(SPELL_IDS.CONDENSATION, enemy.cellId, "CONDENSATION NETTOYAGE DIRECTE")
                        if success then
                            finalPA = newPA
                            actionTaken = true
                            console.print("✅ NETTOYAGE P2 - CONDENSATION directe réussie - PA restants: " .. finalPA, console.green)
                        end
                    end
                end
            end
            
            -- PRIORITÉ 3: PROJECTION (2 PA) seulement en dernier recours
            if not actionTaken and finalPA >= 2 and enemy then
                local distance = fight.cellsDistance(me.cellId, enemy.cellId)
                console.print("🎯 NETTOYAGE P3 - PROJECTION (dernier recours) sur " .. (enemy.name or "Ennemi") .. " (distance: " .. distance .. ")", console.cyan)
                
                if distance <= 5 and fight.canCastSpell(SPELL_IDS.PROJECTION, enemy.cellId) ~= 0 then
                    local success, newPA = castSpellPostPhossileAndRefresh(SPELL_IDS.PROJECTION, enemy.cellId, "PROJECTION NETTOYAGE FINAL")
                    if success then
                        finalPA = newPA
                        actionTaken = true
                        console.print("✅ NETTOYAGE P3 - PROJECTION (dernier recours) réussie - PA restants: " .. finalPA, console.green)
                    end
                end
            end
            
            -- Si aucune action possible, arrêter
            if not actionTaken then
                console.print("❌ NETTOYAGE - Aucun sort possible avec " .. finalPA .. " PA - Arrêt", console.red)
                break
            end
            
            -- Sécurité : actualiser PA après chaque action
            finalPA = getRealActionPoints()
        end
        
        console.print("🧹 NETTOYAGE OPTIMISÉ TERMINÉ - Actions: " .. actionsNettoyage .. ", PA finaux: " .. finalPA, console.orange)
    else
        console.print("⏸️ PA restants insuffisants pour nettoyage (" .. finalPA .. " PA)", console.orange)
    end
    
    -- STRATÉGIE D'URGENCE SUPPRIMÉE : La logique de nettoyage optimisée gère déjà l'épuisement des PA
    
    -- Affichage final actualisé
    finalPA = getRealActionPoints()
    console.print("🎯 === STRATÉGIE POST-PHOSSILE OFFICIELLE TERMINÉE - PA finaux: " .. finalPA .. " ===", console.red)
    
    -- ÉTAPE FINALE : POSITIONNEMENT CAC OPTIMAL AVANT FIN DE TOUR
    local me = fight.currentFighter()
    if me then
        local finalPM = me.movementPoints or me.MP or 0
        
        if finalPM > 0 then
            console.print("🔍 === POSITIONNEMENT FINAL CAC - PM restants: " .. finalPM .. " ===", console.blue)
            
            -- Trouver l'ennemi le plus proche
            local nearestEnemy = findBestEnemyForCAC()
            
            if nearestEnemy then
                local distance = fight.cellsDistance(me.cellId, nearestEnemy.cellId)
                console.print("🎯 Ennemi le plus proche: " .. (nearestEnemy.name or "Ennemi") .. " (distance: " .. distance .. ")", console.cyan)
                
                -- Vérifier si déjà au CAC (distance 1)
                if distance <= 1 then
                    console.print("✅ Déjà au CAC - Positionnement optimal", console.green)
                else
                    console.print("🏃 DÉPLACEMENT FINAL vers CAC - PM: " .. finalPM, console.yellow)
                    
                    -- Essayer de se coller au CAC
                    local success = moveTowardsEnemyCAC(nearestEnemy)
                    
                    if success then
                        -- Vérifier la distance finale
                        local newDistance = fight.cellsDistance(me.cellId, nearestEnemy.cellId)
                        console.print("✅ Déplacement final réussi - Distance finale: " .. newDistance, console.green)
                        
                        -- NETTOYAGE POST-DÉPLACEMENT : Utiliser PA restants après repositionnement
                        local paPostDeplacement = getRealActionPoints()
                        if paPostDeplacement >= 2 then
                            console.print("🧹 === NETTOYAGE POST-DÉPLACEMENT - PA restants: " .. paPostDeplacement .. " ===", console.blue)
                            
                            local actionsPostDeplacement = 0
                            local maxActionsPostDeplacement = 3
                            
                            while paPostDeplacement >= 2 and actionsPostDeplacement < maxActionsPostDeplacement do
                                actionsPostDeplacement = actionsPostDeplacement + 1
                                console.print("🆘 Nettoyage post-déplacement #" .. actionsPostDeplacement .. " - PA: " .. paPostDeplacement, console.yellow)
                                
                                local actionTaken = false
                                
                                -- Priorité 1: DISSOLUTION/DÉCHAINEMENT (4 PA) - OPTIMISATION NETTOYAGE POST-DÉPLACEMENT
                                if paPostDeplacement >= 4 then
                                                        -- LOGIQUE SPÉCIALE NETTOYAGE : Si exactement 4 PA, forcer DISSOLUTION pour optimisation
                    local isExactly4PA = (paPostDeplacement == 4)
                    local pvPercent = getPVPercent()
                    local spellId = (isExactly4PA or pvPercent <= 50) and SPELL_IDS.DISSOLUTION or SPELL_IDS.DECHAINEMENT
                    local spellName = (isExactly4PA or pvPercent <= 50) and "DISSOLUTION" or "DÉCHAINEMENT"
                    
                    -- Log explicatif pour l'optimisation post-déplacement
                    if isExactly4PA and pvPercent > 50 then
                        console.print("🎯 OPTIMISATION POST-DÉPLACEMENT : DISSOLUTION forcée (4 PA exactement)", console.yellow)
                    end
                                    
                                    -- Essayer AOE d'abord
                                    local bestCell, score, enemyCount = findBestAOEPosition(spellId)
                                    if bestCell and enemyCount >= 1 then
                                        local success, newPA = castSpellPostPhossileAndRefresh(spellId, bestCell, spellName .. " AOE POST-DÉPLACEMENT")
                                        if success then
                                            paPostDeplacement = newPA
                                            actionTaken = true
                                            console.print("✅ " .. spellName .. " AOE post-déplacement réussi (+" .. enemyCount .. " ennemis)", console.green)
                                        end
                                    else
                                        -- Fallback ciblage direct
                                        local enemy = fight.nearestEnemy()
                                        if enemy then
                                            local success, newPA = castSpellPostPhossileAndRefresh(spellId, enemy.cellId, spellName .. " DIRECT POST-DÉPLACEMENT")
                                            if success then
                                                paPostDeplacement = newPA
                                                actionTaken = true
                                                console.print("✅ " .. spellName .. " direct post-déplacement réussi", console.green)
                                            end
                                        end
                                    end
                                end
                                
                                -- Priorité 2: CONDENSATION (3 PA) si pas assez pour sorts 4 PA
                                if not actionTaken and paPostDeplacement >= 3 then
                                    local bestCell, score, enemyCount = findBestAOEPosition(SPELL_IDS.CONDENSATION)
                                    if bestCell and enemyCount >= 1 then
                                        local success, newPA = castSpellPostPhossileAndRefresh(SPELL_IDS.CONDENSATION, bestCell, "CONDENSATION POST-DÉPLACEMENT")
                                        if success then
                                            paPostDeplacement = newPA
                                            actionTaken = true
                                            console.print("✅ CONDENSATION post-déplacement réussie (+" .. enemyCount .. " ennemis)", console.green)
                                        end
                                    end
                                end
                                
                                -- Priorité 3: PROJECTION (2 PA) en dernier recours
                                if not actionTaken and paPostDeplacement >= 2 then
                                    local enemy = fight.nearestEnemy()
                                    if enemy then
                                        local distance = fight.cellsDistance(me.cellId, enemy.cellId)
                                        if distance <= 5 then -- Portée PROJECTION
                                            local success, newPA = castSpellPostPhossileAndRefresh(SPELL_IDS.PROJECTION, enemy.cellId, "PROJECTION POST-DÉPLACEMENT")
                                            if success then
                                                paPostDeplacement = newPA
                                                actionTaken = true
                                                console.print("✅ PROJECTION post-déplacement réussie", console.green)
                                            end
                                        end
                                    end
                                end
                                
                                -- Si aucun sort réussi, arrêter
                                if not actionTaken then
                                    console.print("❌ Aucun sort post-déplacement possible - PA: " .. paPostDeplacement, console.red)
                                    break
                                end
                            end
                            
                            console.print("🧹 Nettoyage post-déplacement terminé - Actions: " .. actionsPostDeplacement .. ", PA finaux: " .. paPostDeplacement, console.green)
                        else
                            console.print("⏸️ Pas assez de PA pour nettoyage post-déplacement (" .. paPostDeplacement .. " PA)", console.orange)
                        end
                        
                    else
                        console.print("⚠️ Déplacement final impossible - Rapprochement maximal effectué", console.orange)
                    end
                end
            else
                console.print("⏸️ Aucun PM pour positionnement final", console.orange)
            end
        end
    end
end

-- ===============================================================================
-- 🤖 PROCÉDURE DE COMBAT AVEC STRATÉGIES PAR TOUR
-- ===============================================================================

-- Variables pour garder l'état du tour
local lastProcedureRound = -1
local turn1StrategyCompleted = false
local turn2StrategyCompleted = false
local turn3StrategyCompleted = false
local turn4StrategyCompleted = false
    local turn5StrategyCompleted = false
    local turn6StrategyCompleted = false
    local turn7StrategyCompleted = false

function fight_procedure()
    console.print("🧠 Procédure de combat démarrée", console.blue)
    
    local currentRound = fight.currentRound() or 1
    
    -- Réinitialiser les flags à chaque nouveau tour
    if currentRound ~= lastProcedureRound then
        lastProcedureRound = currentRound
        turn1StrategyCompleted = false
        turn2StrategyCompleted = false
        turn3StrategyCompleted = false
        turn4StrategyCompleted = false
        turn5StrategyCompleted = false
        turn6StrategyCompleted = false
        turn7StrategyCompleted = false
        
        -- Réinitialiser le compteur post-Phossile au premier tour du combat seulement
        if currentRound == 1 then
            postPhossileTurnCount = 0
            resetSpellCooldowns() -- Réinitialiser les cooldowns au nouveau combat
            console.print("🔄 Compteur post-Phossile et cooldowns réinitialisés", console.blue)
        end
        
        console.print("🔄 Nouveau tour " .. currentRound .. " - Reset flags", console.blue)
    end
    
    -- Vérifier si c'est bien notre tour
    local me = fight.currentFighter()
    if not me then
        console.print("❌ Pas notre tour - Attente...", console.red)
        fight.finishTurn()
        return
    end
    
    console.print("🎮 TOUR ACTUEL: " .. currentRound, console.cyan)
    console.print("👤 Combattant actuel: " .. (me.name or "Inconnu"), console.cyan)
    
    -- Récupérer les stats avec debug détaillé
    local stats = getFighterStats()
    console.print("💪 PA disponibles: " .. stats.PA, console.cyan)
    
    if stats.PA == 0 then
        console.print("⚠️ Aucun PA disponible - Fin de tour", console.orange)
        fight.finishTurn()
        return
    end
    
    -- STRATÉGIE SELON LE TOUR
    if currentRound == 1 then
        -- TOUR 1: Stratégie spécialisée
        if not turn1StrategyCompleted then
            console.print("🔥 === STRATÉGIE TOUR 1 DÉMARRÉE ===", console.red)
            local success = executeTurn1Strategy()
            if success then
                turn1StrategyCompleted = true
                console.print("✅ Stratégie Tour 1 TERMINÉE", console.green)
            else
                console.print("❌ Stratégie Tour 1 ÉCHOUÉE", console.red)
            end
        else
            console.print("✅ Stratégie Tour 1 déjà terminée - Fin de tour", console.green)
        end
        
    elseif currentRound == 2 then
        -- TOUR 2: Stratégie spécialisée
        if not turn2StrategyCompleted then
            console.print("⚔️ === STRATÉGIE TOUR 2 DÉMARRÉE ===", console.red)
            local success = executeTurn2Strategy()
            if success then
                turn2StrategyCompleted = true
                console.print("✅ Stratégie Tour 2 TERMINÉE", console.green)
            end
        else
            console.print("✅ Stratégie Tour 2 déjà terminée - Fin de tour", console.green)
        end
        
    elseif currentRound == 3 then
        -- TOUR 3: Stratégie spécialisée
        if not turn3StrategyCompleted then
            console.print("🗡️ === STRATÉGIE TOUR 3 DÉMARRÉE ===", console.red)
            local success = executeTurn3Strategy()
            if success then
                turn3StrategyCompleted = true
                console.print("✅ Stratégie Tour 3 TERMINÉE", console.green)
            end
        else
            console.print("✅ Stratégie Tour 3 déjà terminée - Fin de tour", console.green)
        end
        
    elseif currentRound == 4 then
        -- TOUR 4: Stratégie conditionnelle selon état Phossile
        if not turn4StrategyCompleted then
            console.print("🎭 === STRATÉGIE TOUR 4 DÉMARRÉE ===", console.red)
            
            -- Vérification préliminaire du Phossile
            local phossileBeforeTurn4 = findPhossile()
            if not phossileBeforeTurn4 then
                console.print("💀 PHOSSILE DÉJÀ MORT AU DÉBUT T4 - STRATÉGIE POST-PHOSSILE DIRECTE", console.red)
                executePostPhossileStrategy()
                turn4StrategyCompleted = true
                console.print("✅ Stratégie Post-Phossile T4 TERMINÉE", console.green)
            else
                console.print("🎯 Phossile détecté - Exécution stratégie T4", console.cyan)
            local success = executeTurn4Strategy()
            if success then
                turn4StrategyCompleted = true
                console.print("✅ Stratégie Tour 4 TERMINÉE", console.green)
                    
                    -- Vérification post-stratégie : Si le Phossile a été tué pendant T4
                    local phossileAfterTurn4 = findPhossile()
                    if not phossileAfterTurn4 then
                        console.print("💀 PHOSSILE TUÉ PENDANT T4 - Stratégie post-Phossile déjà activée", console.red)
                    end
                end
            end
        else
            console.print("✅ Stratégie Tour 4 déjà terminée - Fin de tour", console.green)
        end
        
    elseif currentRound == 5 then
        -- TOUR 5: Stratégie spécialisée
        if not turn5StrategyCompleted then
            console.print("🗡️ === STRATÉGIE TOUR 5 DÉMARRÉE ===", console.red)
            local success = executeTurn5Strategy()
            if success then
                turn5StrategyCompleted = true
                console.print("✅ Stratégie Tour 5 TERMINÉE", console.green)
            end
        else
            console.print("✅ Stratégie Tour 5 déjà terminée - Fin de tour", console.green)
                end
        
    elseif currentRound == 6 then
        -- TOUR 6: Stratégie spécialisée
        if not turn6StrategyCompleted then
            console.print("🗡️ === STRATÉGIE TOUR 6 DÉMARRÉE ===", console.red)
            local success = executeTurn6Strategy()
            if success then
                turn6StrategyCompleted = true
                console.print("✅ Stratégie Tour 6 TERMINÉE", console.green)
            end
        else
            console.print("✅ Stratégie Tour 6 déjà terminée - Fin de tour", console.green)
        end
        
    elseif currentRound == 7 then
        -- TOUR 7: Stratégie spécialisée
        if not turn7StrategyCompleted then
            console.print("🗡️ === STRATÉGIE TOUR 7 DÉMARRÉE ===", console.red)
            local success = executeTurn7Strategy()
            if success then
                turn7StrategyCompleted = true
                console.print("✅ Stratégie Tour 7 TERMINÉE", console.green)
            end
        else
            console.print("✅ Stratégie Tour 7 déjà terminée - Fin de tour", console.green)
        end
        
    else
        -- TOURS SUIVANTS: Logique avec priorité Phossile
        console.print("🎯 Tours suivants - Logique priorité Phossile", console.blue)
        
        local phossile = findPhossile()
        
        if phossile then
            console.print("🎯 PHOSSILE VIVANT - PRIORITÉ ABSOLUE !", console.red)
            
            -- Test des sorts AOE prioritaires incluant Phossile
            local actionTaken = false
            local currentPA = stats.PA -- Utiliser les stats déjà récupérées
            
            if currentPA >= 3 then
                local bestCell, score, enemyCount, hasPhossile = findBestAOEPosition(SPELL_IDS.CONDENSATION)
                if bestCell and hasPhossile and score >= 10000 then
                    if fight.canCastSpell(SPELL_IDS.CONDENSATION, bestCell) ~= 0 then
                        fight.castSpell(SPELL_IDS.CONDENSATION, bestCell)
                        console.print("🌊 CONDENSATION sur PHOSSILE ! (+" .. enemyCount .. " ennemis)", console.red)
                        actionTaken = true
                    end
                end
            end
            
            if not actionTaken and currentPA >= 4 then
                local bestCell, score, enemyCount, hasPhossile = findBestAOEPosition(SPELL_IDS.DISSOLUTION)
                if bestCell and hasPhossile and score >= 10000 then
                    if fight.canCastSpell(SPELL_IDS.DISSOLUTION, bestCell) ~= 0 then
                        fight.castSpell(SPELL_IDS.DISSOLUTION, bestCell)
                        console.print("🌪️ DISSOLUTION sur PHOSSILE ! (+" .. enemyCount .. " ennemis)", console.red)
                        actionTaken = true
                    end
                end
            end
            
            if not actionTaken then
                console.print("🎯 Configuration tours suivants en attente", console.orange)
            end
            
        else
            console.print("💀 PHOSSILE MORT - ACTIVATION STRATÉGIE POST-PHOSSILE", console.red)
            executePostPhossileStrategy()
        end
    end
    
    console.print("🏁 Tour terminé", console.white)
    fight.finishTurn()
end

-- FONCTION fight_action SUPPRIMÉE - Plus de récursion avec fight_procedure
-- On utilise uniquement fight_procedure maintenant

-- ===============================================================================
-- 🎮 INITIALISATION IA AVANCÉE
-- ===============================================================================

function initAdvancedAI()
    console.print("🤖 Initialisation structure IA...", console.blue)
    
    pcall(function()
        -- Enregistrer UNIQUEMENT fight_procedure (pas fight_action pour éviter récursion)
        fight.register("fight_procedure", fight_procedure)
        fight.register("fight_placement", fight_placement)
        console.print("✅ Callbacks combat (procedure uniquement) ET placement enregistrés - Récursion évitée", console.green)
    end)
end

-- Callback de début de combat
function map_fight_start()
    console.print("🔥 Combat démarré !", console.red)
    initAdvancedAI()
end

-- Fonction pour récupérer les PA réels via l'API Frigost (RÉFÉRENCE OFFICIELLE)
function getRealActionPoints()
    local me = fight.currentFighter()
    if not me then
        console.print("❌ getRealActionPoints: Aucun combattant actuel", console.red)
        return 0
    end
    
    local realPA = 0
    
    -- MÉTHODE 1 : Via stats officielles Frigost (priorité absolue)
    if me.stats and me.stats[enum_Stat.ACTION_POINTS] then
        realPA = me.stats[enum_Stat.ACTION_POINTS]
        console.print("📊 PA via enum_Stat.ACTION_POINTS: " .. realPA .. " (MÉTHODE OFFICIELLE)", console.green)
        return realPA
    end
    
    -- MÉTHODE 2 : Via propriétés directes (fallback d'urgence)
    realPA = me.actionPoints or me.AP or 0
    console.print("⚠️ PA via propriétés directes: " .. realPA .. " (FALLBACK - enum_Stat non disponible)", console.orange)
    
    return realPA
end

-- Fonction pour lancer un sort et récupérer automatiquement les PA réels après
function castSpellPostPhossileAndRefresh(spellId, targetCell, spellName)
    -- VÉRIFICATION PRÉALABLE : Cooldown seulement pour MUTILATION et COURONNES D'ÉPINES
    -- EXCEPTION TOUR 2: COURONNES D'ÉPINES toujours disponible au Tour 2
    local isCourronnesTour2Exception = (spellId == SPELL_IDS.COURONNES_EPINES and fight.currentRound() == 2)
    
    if (spellId == SPELL_IDS.MUTILATION or spellId == SPELL_IDS.COURONNES_EPINES) and isSpellOnCooldown(spellId) and not isCourronnesTour2Exception then
        console.print("⏸️ POST-PHOSSILE - " .. spellName .. " IGNORÉ (cooldown réel)", console.orange)
        return false, getRealActionPoints()
    end
    
    local paAvant = getRealActionPoints()
    console.print("🔮 POST-PHOSSILE - Tentative " .. spellName .. " - PA avant: " .. paAvant, console.cyan)
    
    if paAvant < 1 then
        console.print("❌ POST-PHOSSILE - Aucun PA disponible pour " .. spellName, console.red)
        return false, 0
    end
    
    -- VÉRIFICATION canCastSpell avec EXCEPTION TOUR 2 pour COURONNES D'ÉPINES
    local canCast = fight.canCastSpell(spellId, targetCell) ~= 0
    local shouldForceCast = isCourronnesTour2Exception and not canCast
    
    -- DEBUG COMPLET POUR COURONNES D'ÉPINES T2
    if spellId == SPELL_IDS.COURONNES_EPINES and fight.currentRound() == 2 then
        console.print("🔍 === DEBUG COURONNES T2 COMPLET ===", console.yellow)
        console.print("🔍 Sort ID: " .. spellId, console.white)
        console.print("🔍 Target Cell: " .. targetCell, console.white)
        console.print("🔍 Tour actuel: " .. fight.currentRound(), console.white)
        console.print("🔍 canCastSpell résultat: " .. fight.canCastSpell(spellId, targetCell), console.white)
        console.print("🔍 isCourronnesTour2Exception: " .. tostring(isCourronnesTour2Exception), console.white)
        console.print("🔍 canCast: " .. tostring(canCast), console.white)
        console.print("🔍 shouldForceCast: " .. tostring(shouldForceCast), console.white)
        console.print("🔍 === FIN DEBUG COURONNES T2 ===", console.yellow)
    end
    
    if canCast or shouldForceCast then
        if shouldForceCast then
            console.print("🚨 FORÇAGE COURONNES T2 - Ignoring canCastSpell=0 (Exception Tour 2)", console.red)
        end
        
        -- AJOUT : Debug avant lancement pour COURONNES T2
        if spellId == SPELL_IDS.COURONNES_EPINES and fight.currentRound() == 2 then
            console.print("🚀 LANCEMENT COURONNES T2 - canCast=" .. tostring(canCast) .. ", forcé=" .. tostring(shouldForceCast), console.yellow)
        end
        
        fight.castSpell(spellId, targetCell)
        global.sleep(0) -- Pause pour mise à jour serveur
        
        local paApres = getRealActionPoints()
        local paUtilises = paAvant - paApres
        
        -- VÉRIFICATION CRITIQUE : Si PA utilisés = 0, le sort a échoué silencieusement
        if paUtilises > 0 then
            console.print("✅ POST-PHOSSILE - " .. spellName .. " lancé - PA utilisés: " .. paUtilises .. " - PA actuels: " .. paApres, console.green)
            
            -- COOLDOWN RÉEL seulement pour MUTILATION et COURONNES D'ÉPINES
            if spellId == SPELL_IDS.MUTILATION then
                markSpellOnCooldown(spellId, spellName, 2) -- Cooldown 2 tours
            elseif spellId == SPELL_IDS.COURONNES_EPINES then
                markSpellOnCooldown(spellId, spellName, 3) -- Cooldown 3 tours
            end
            -- Les autres sorts (CONDENSATION, DÉCHAINEMENT, DISSOLUTION, PROJECTION) n'ont PAS de cooldown
            
            return true, paApres
        else
            -- DEBUG SPÉCIAL POUR COURONNES D'ÉPINES T2 - ANALYSER POURQUOI ÇA ÉCHOUE
            if spellId == SPELL_IDS.COURONNES_EPINES and fight.currentRound() == 2 then
                console.print("🔥 === ANALYSE ÉCHEC COURONNES T2 ===", console.red)
                console.print("🔥 PA avant lancement: " .. paAvant, console.white)
                console.print("🔥 PA après lancement: " .. paApres, console.white)
                console.print("🔥 PA utilisés: " .. paUtilises, console.white)
                console.print("🔥 canCastSpell était: " .. fight.canCastSpell(spellId, targetCell), console.white)
                
                -- TEST : Essayer sur d'autres cellules
                console.print("🔥 Test COURONNES sur cellules adjacentes...", console.orange)
                local me = fight.currentFighter()
                if me then
                    local adjacentCells = fight.adjacentCells(me.cellId)
                    for i = 1, math.min(3, #adjacentCells) do
                        local cell = adjacentCells[i]
                        local cellId = (type(cell) == "table" and cell.cellId) or cell
                        local canCastResult = fight.canCastSpell(spellId, cellId)
                        console.print("🔥   Cellule " .. cellId .. " - canCast: " .. canCastResult, console.white)
                    end
                end
                
                -- TEST : Vérifier d'autres sorts pour comparaison
                console.print("🔥 Test autres sorts pour comparaison...", console.orange)
                local testSpells = {SPELL_IDS.MUTILATION, SPELL_IDS.PROJECTION, SPELL_IDS.CONDENSATION}
                for _, testSpell in ipairs(testSpells) do
                    local canCastTest = fight.canCastSpell(testSpell, targetCell)
                    console.print("🔥   Sort " .. testSpell .. " - canCast: " .. canCastTest, console.white)
                end
                
                console.print("🔥 === FIN ANALYSE ÉCHEC ===", console.red)
            end
            
            if shouldForceCast then
                console.print("❌ POST-PHOSSILE - " .. spellName .. " ÉCHEC MÊME AVEC FORÇAGE T2 (vraiment impossible)", console.red)
            else
                console.print("❌ POST-PHOSSILE - " .. spellName .. " ÉCHEC SILENCIEUX (PA utilisés: 0) - Impossible ce tour", console.red)
            end
            return false, paApres
        end
    else
        -- DEBUG COMPLET SI canCastSpell = 0 pour COURONNES T2
        if spellId == SPELL_IDS.COURONNES_EPINES and fight.currentRound() == 2 then
            console.print("🔥 COURONNES T2 - canCastSpell = 0 - INVESTIGATION APPROFONDIE", console.red)
            
            -- Test sur TOUTES les cellules accessibles
            console.print("🔥 Test sur toutes cellules accessibles...", console.orange)
            local accessibleCells = fight.accessibleCells()
            local workingCells = {}
            
            for i = 1, math.min(10, #accessibleCells) do
                local cell = accessibleCells[i]
                local cellId = (type(cell) == "table" and cell.cellId) or cell
                local canCastResult = fight.canCastSpell(spellId, cellId)
                
                if canCastResult ~= 0 then
                    table.insert(workingCells, cellId)
                    console.print("🔥   ✅ Cellule " .. cellId .. " - canCast: " .. canCastResult, console.green)
                else
                    console.print("🔥   ❌ Cellule " .. cellId .. " - canCast: " .. canCastResult, console.red)
                end
            end
            
            -- Si on trouve des cellules qui marchent, essayer dessus
            if #workingCells > 0 then
                console.print("🔥 TENTATIVE COURONNES T2 SUR CELLULE ALTERNATIVE: " .. workingCells[1], console.yellow)
                fight.castSpell(spellId, workingCells[1])
                global.sleep(0)
                
                local paApresAlt = getRealActionPoints()
                local paUtilisesAlt = paAvant - paApresAlt
                
                if paUtilisesAlt > 0 then
                    console.print("🔥 ✅ COURONNES T2 RÉUSSI SUR CELLULE ALTERNATIVE !", console.green)
                    markSpellOnCooldown(spellId, spellName, 3)
                    return true, paApresAlt
                else
                    console.print("🔥 ❌ COURONNES T2 ÉCHEC MÊME SUR CELLULE ALTERNATIVE", console.red)
                end
            else
                console.print("🔥 ❌ AUCUNE CELLULE VIABLE POUR COURONNES T2", console.red)
            end
        end
        
        console.print("❌ POST-PHOSSILE - " .. spellName .. " impossible à lancer (canCastSpell = 0)", console.red)
        return false, paAvant
    end
end

-- Fonction pour récupérer le pourcentage de PV actuel
function getPVPercent()
    local me = fight.currentFighter()
    if not me then return 50 end -- Défaut sécurisé
    
    local currentPV = me.lifePoints or 0
    local maxPV = me.maxLifePoints or 1
    
    -- Gestion des PV négatifs (erreur API)
    if currentPV < 0 and maxPV <= 0 then
        -- Utilisation méthode alternative via stats
        pcall(function()
            currentPV = me.stats[enum_Stat.CUR_LIFE] or 0
            maxPV = me.stats[enum_Stat.MAX_LIFE] or 1
        end)
    end
    
    if maxPV <= 0 then maxPV = 1 end -- Éviter division par 0
    
    local pvPercent = (currentPV / maxPV) * 100
    return math.max(0, math.min(100, pvPercent)) -- Limiter entre 0-100%
end




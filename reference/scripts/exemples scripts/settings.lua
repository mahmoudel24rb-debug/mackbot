-- =============================================
-- SETTINGS PHOSSILE - CONFIGURATION OBJETS RÉCUPÉRATION
-- =============================================
-- Fichier de configuration pour les GIDs des objets utilisés après combat
-- 
-- USAGE APRÈS COMBAT :
-- - Leader : Pain (toujours) + Potion énergie (si défaite uniquement)
-- - Mules : Potion énergie (si défaite uniquement)
--
-- Pour trouver les GIDs de vos objets :
-- 1. Aller sur https://www.ankabot.dev/Identifiants/DofusPC/Objets.txt
-- 2. Chercher votre item et vous trouverez le GID 

-- =============================================

local Settings = {}

-- =============================================
-- CONFIGURATION DES OBJETS DE RÉCUPÉRATION
-- =============================================

-- Pain pour récupération de vitalité (LEADER UNIQUEMENT)
Settings.PAIN_GID = 17189  -- Pain spécifique utilisateur
Settings.PAIN_QUANTITY = 30  -- Quantité à utiliser (CORRIGÉE)

-- Potion d'énergie pour récupération après défaite
Settings.ENERGY_POTION_GID = 17195  -- Potion d'énergie spécifique utilisateur
Settings.ENERGY_POTION_QUANTITY = 30 -- Quantité à utiliser (CORRIGÉE)

-- =============================================
-- CONFIGURATION AVANCÉE DE RÉCUPÉRATION
-- =============================================

-- Seuils de déclenchement
Settings.MIN_VITALITY_PERCENT = 100  -- En dessous de ce %, le leader utilise du pain
Settings.MIN_ENERGY_PERCENT = 100   -- En dessous de ce %, utiliser potion énergie après défaite

-- Maps de téléportation
Settings.DUNGEON_ENTRANCE_MAP = 130285568  -- Map d'entrée du donjon Phossile
Settings.RECOVERY_CELL = 477  -- Cellule de récupération à l'entrée

-- Délais de sécurité
Settings.RECOVERY_DELAY = 1000  -- Délai après utilisation d'objet (ms)
Settings.TELEPORT_DELAY = 1000  -- Délai après téléportation (ms)

-- Options de debug
Settings.ENABLE_RECOVERY_LOGS = true  -- Afficher les logs de récupération

-- =============================================
-- CONFIGURATION DES STATISTIQUES
-- =============================================

Settings.listStats = {
    { id = 1, coords = 262, name = "VIE"},
    { id = 2, coords = 356, name = "TERRE"},
    { id = 3, coords = 453, name = "FEU"},
    { id = 4, coords = 553, name = "EAU"},
    { id = 5, coords = 648, name = "AIR"},
    { id = 6, coords = 746, name = "SAGESSE"}
}

-- Choix de la statistique à monter (1=VIE, 2=TERRE, 3=FEU, 4=EAU, 5=AIR, 6=SAGESSE)
Settings.ChoiceStats = 6  -- Par défaut : SAGESSE

Nobstant_1 = 11622
Nobstant_2 = 11622

-- =============================================
-- OBJETS ALTERNATIFS (FALLBACK)
-- =============================================

-- Si les objets principaux ne sont pas disponibles
Settings.FALLBACK_PAIN_GIDS = {
    524,   -- Pain de Campagne (standard)
    527,   -- Pain d'Amakna
    523,   -- Pain de Bonta
    525,   -- Pain de Brakmar
    8219   -- Pain du Boulanger
}

Settings.FALLBACK_ENERGY_GIDS = {
    1558,  -- Potion d'Énergie Majeure (standard)
    1557,  -- Potion d'Énergie Moyenne
    1556,  -- Potion d'Énergie Mineure
    17198, -- Autre variante possible
    1559   -- Potion d'Énergie Supérieure
}

-- =============================================
-- CONFIGURATION DU STUFF SAGESSE
-- =============================================

Settings.items = {
    -- Amulettes
    {gid = 1489,  pos = 0,  level = 28},   -- La Broche Hète
    {gid = 2390,  pos = 0,  level = 49},   -- Amulette de la Boule
    {gid = 8268,  pos = 0,  level = 111},  -- Collier du Minotoror

    -- Armes
    {gid = 835,   pos = 1,  level = 27},   -- Baguette de Sagesse
    {gid = 8274,   pos = 1,  level = 109},   -- Hache du minotoror
    {gid = 8850,  pos = 1,  level = 144},  -- Racine de Floribonde

    -- Anneaux 1
    {gid = 732,   pos = 2,  level = 26},   -- Alliance de Silimelle
    {gid = 11622, pos = 2,  level = 146},  -- Anneau Nobstant

    -- Ceintures
    {gid = 1487,  pos = 3,  level = 48},   -- l'Adelus
    {gid = 8282,  pos = 3,  level = 109},   -- Ceinture de minotoror


    -- Anneaux 2
    {gid = 11622, pos = 4,  level = 146},  -- Anneau Nobstant
    {gid = 732, pos = 4, level = 26}, -- Anneau de ilimelle
    

    -- Bottes

    {gid = 1665,  pos = 5,  level = 50},   -- Bottes d'Apprentissage
    {gid = 8276, pos = 5,  level = 113},  -- Bottes de minotoror

    -- Coiffes
    {gid = 712,   pos = 6,  level = 32},   -- Gulliver
    {gid = 6481,  pos = 6,  level = 120},  -- Dora Bora

    -- Capes
    {gid = 6927,  pos = 7,  level = 37 },  -- Cape prespic
    {gid = 8279,  pos = 7,  level = 107},  -- Cape du Minotoror

    -- Dofus
    {gid = 972,   pos = 9,  level = 60},   -- Dofus Cawotte

    -- Boucliers
    {gid = 18676, pos = 15, level = 1},    -- Bouclier du Bûcheron

    -- Familier
    {gid = 1711,  pos = 8,  level = 1},    -- Chienchien
}

-- =============================================
-- FONCTION D'EXPORT
-- =============================================

return Settings 
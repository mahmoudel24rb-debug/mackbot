/**
 * Global application state.
 * Updated by WebSocket messages, read by UI components.
 */
window.store = {
    // Connection
    connected: false,
    wsConnected: false,

    // Character
    character: {
        name: null,
        level: null,
        server: null,
        hp: 0, maxHp: 0,
        mp: 0, maxMp: 0,
        ap: 0, maxAp: 0,
        kamas: 0,
    },

    // Map
    map: {
        id: null,
        x: null,
        y: null,
        cellId: null,
        resources: [],
        entities: {},
        walkable: new Set(),
        specialCells: {},
        mapChangeData: {},
    },

    // Bot
    bot: {
        scriptRunning: false,
        inFight: false,
        isBusy: false,
        busyReason: null,
    },

    // Stats
    stats: {
        monstersOnMap: 0,
        npcsOnMap: 0,
        playersOnMap: 0,
        resourcesOnMap: 0,
    },

    // Logs
    logs: [],
    maxLogs: 500,

    // Active tab
    activeTab: 'dashboard',

    // -- Methods --

    addLog(text, level = 'info') {
        const ts = new Date().toLocaleTimeString('fr-FR', { hour12: false });
        this.logs.push({ text, level, ts });
        if (this.logs.length > this.maxLogs) {
            this.logs = this.logs.slice(-400);
        }
        if (window.logsComponent) window.logsComponent.refresh();
    },

    updateFromStatus(status) {
        if (!status) return;

        this.character.name = status.character || null;
        this.character.level = status.level || null;
        this.character.hp = status.hp || 0;
        this.character.maxHp = status.max_hp || 0;
        this.character.mp = status.mp || 0;
        this.character.maxMp = status.max_mp || 0;
        this.character.ap = status.ap || 0;
        this.character.maxAp = status.max_ap || 0;
        this.character.kamas = status.kamas || 0;
        this.map.id = status.map_id || null;
        this.map.x = status.map_x;
        this.map.y = status.map_y;
        this.map.cellId = status.cell_id;
        this.bot.scriptRunning = status.script_running || false;
        this.bot.inFight = status.in_fight || false;
        this.bot.isBusy = status.is_busy || false;
        this.bot.busyReason = status.busy_reason || null;
        this.stats.monstersOnMap = status.monster_count || 0;
        this.stats.npcsOnMap = status.npc_count || 0;
        this.stats.playersOnMap = status.player_count || 0;
        this.stats.resourcesOnMap = status.resources_on_map || 0;
        this.connected = status.connected || false;

        if (window.dashboardComponent) window.dashboardComponent.refresh();
    },
};

/**
 * MackBot Web UI — Main application entry point.
 */
(function() {
    'use strict';

    const ws = window.botWS;
    const store = window.store;

    // -- Tab routing --
    const pageTitles = {
        dashboard: 'Accueil',
        harvest: 'Recolte',
        map: 'Carte',
        sniffer: 'Sniffer',
        settings: 'Parametres',
    };

    function switchTab(tabId) {
        if (['fight', 'market', 'fm'].includes(tabId)) return;

        store.activeTab = tabId;

        document.querySelectorAll('.nav-item').forEach(btn => {
            btn.classList.toggle('active', btn.dataset.tab === tabId);
        });

        document.querySelectorAll('.tab-content').forEach(section => {
            section.classList.toggle('active', section.id === `tab-${tabId}`);
        });

        const title = pageTitles[tabId] || tabId;
        document.getElementById('pageTitle').textContent = title;
    }

    document.querySelectorAll('.nav-item').forEach(btn => {
        btn.addEventListener('click', () => {
            if (!btn.classList.contains('disabled')) {
                switchTab(btn.dataset.tab);
            }
        });
    });

    // -- WebSocket handlers --

    ws.onStatusChange = (connected) => {
        store.wsConnected = connected;
        const el = document.getElementById('wsStatus');
        if (connected) {
            el.textContent = '\u25CF WS connecte';
            el.style.color = 'var(--text-green)';
        } else {
            el.textContent = '\u25CF WS deconnecte';
            el.style.color = 'var(--border)';
        }
    };

    ws.on('Status', (payload) => {
        store.updateFromStatus(payload);
        updateSidebarStatus();
    });

    ws.on('CharacterData', (payload) => {
        store.character.name = payload.name;
        store.character.level = payload.level;
        store.character.server = payload.server;
        store.connected = true;
        updateSidebarStatus();
        store.addLog(`Connecte: ${payload.name} Nv.${payload.level}`, 'success');
    });

    ws.on('MapInformation', (payload) => {
        store.map.id = payload.map_id;
        store.map.x = payload.x;
        store.map.y = payload.y;
        store.addLog(
            `Map changee -> ${payload.map_id} (${payload.x},${payload.y})`,
            'nav'
        );
    });

    ws.on('Log', (payload) => {
        store.addLog(payload.message, payload.type || 'info');
    });

    // -- Sidebar status update --

    function updateSidebarStatus() {
        const { character, connected } = store;

        const dot = document.getElementById('statusDot');
        const statusText = document.getElementById('statusText');

        if (!connected) {
            dot.classList.remove('connected');
            dot.classList.add('disconnected');
            statusText.textContent = 'En attente de connexion...';
            statusText.style.color = 'var(--text-dim)';
        } else if (!character.name) {
            dot.classList.add('connected');
            dot.classList.remove('disconnected');
            statusText.textContent = 'Connecte (chargement...)';
            statusText.style.color = 'var(--text-yellow)';
        } else {
            dot.classList.add('connected');
            dot.classList.remove('disconnected');
            statusText.textContent = 'Connecte';
            statusText.style.color = 'var(--text-green)';
        }

        const name = character.name || (connected ? 'Chargement...' : 'Deconnecte');
        const displayName = character.level ? `${name}  Nv.${character.level}` : name;
        const initial = character.name ? character.name[0].toUpperCase() : '?';

        document.getElementById('charName').textContent = displayName;
        document.getElementById('charServer').textContent = character.server || '';
        document.getElementById('charAvatar').textContent = initial;

        document.getElementById('topbarName').textContent = displayName;
        document.getElementById('topbarServer').textContent = character.server || '';
        document.getElementById('topbarAvatar').textContent = initial;
    }

    // -- Init --
    switchTab('dashboard');
    ws.connect();

})();

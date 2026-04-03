/**
 * Settings tab — proxy, directories, anti-detection config.
 */
(function() {
    'use strict';

    const ws = window.botWS;

    const FIELDS = [
        { section: 'Reseau / Proxy', items: [
            { key: 'PROXY_PORT', label: 'Proxy port', type: 'number' },
            { key: 'SERVER_HOSTNAME', label: 'Serveur hostname', type: 'text' },
            { key: 'SERVER_PORT', label: 'Serveur port', type: 'number' },
            { key: 'FAKE_LAUNCHER_PORT', label: 'Fake Launcher port', type: 'number' },
        ]},
        { section: 'Dossiers', items: [
            { key: 'SCRIPTS_DIR', label: 'Dossier scripts', type: 'text' },
            { key: 'ROUTES_DIR', label: 'Dossier routes', type: 'text' },
        ]},
        { section: 'Anti-detection', items: [
            { key: 'ACTION_DELAY_MIN', label: 'Delai min (s)', type: 'number', step: '0.1' },
            { key: 'ACTION_DELAY_MAX', label: 'Delai max (s)', type: 'number', step: '0.1' },
            { key: 'MAP_CHANGE_DELAY', label: 'Delai map change (s)', type: 'number', step: '0.1' },
        ]},
    ];

    function init() {
        const el = document.getElementById('tab-settings');
        if (!el) return;

        let html = '';

        for (const section of FIELDS) {
            html += `<div class="card">
                <div class="card-title">${section.section}</div>`;
            for (const item of section.items) {
                const step = item.step ? `step="${item.step}"` : '';
                html += `
                <div class="stat-row" style="margin-bottom:6px;">
                    <span class="stat-label">${item.label}:</span>
                    <input type="${item.type}" id="setting-${item.key}" ${step}
                           style="width:220px;"
                           value="">
                </div>`;
            }
            html += '</div>';
        }

        html += `
        <div style="display:flex;align-items:center;gap:12px;margin-top:8px;">
            <button class="btn btn-green" onclick="window.settingsComponent.save()">Enregistrer</button>
            <span id="settings-feedback" style="font-size:11px;color:var(--text-green);opacity:0;transition:opacity 0.3s;"></span>
        </div>
        <div style="font-size:10px;color:var(--text-dim);margin-top:8px;">
            Les modifications prennent effet au prochain demarrage.
        </div>
        `;

        el.innerHTML = html;

        // Request current settings
        ws.send('getSettings', {});
    }

    function loadSettings(settings) {
        for (const section of FIELDS) {
            for (const item of section.items) {
                const input = document.getElementById(`setting-${item.key}`);
                if (input && settings[item.key] !== undefined) {
                    input.value = settings[item.key];
                }
            }
        }
    }

    function save() {
        const settings = {};
        for (const section of FIELDS) {
            for (const item of section.items) {
                const input = document.getElementById(`setting-${item.key}`);
                if (input) {
                    settings[item.key] = input.value;
                }
            }
        }
        ws.send('saveSettings', { settings });

        // Feedback
        const fb = document.getElementById('settings-feedback');
        if (fb) {
            fb.textContent = '\u2713 Sauvegarde';
            fb.style.opacity = '1';
            setTimeout(() => { fb.style.opacity = '0'; }, 3000);
        }
    }

    // -- WebSocket --
    ws.on('CurrentSettings', (payload) => loadSettings(payload));

    // -- Public API --
    window.settingsComponent = { init, save };

    document.addEventListener('DOMContentLoaded', init);
})();

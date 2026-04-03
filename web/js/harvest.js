/**
 * Harvest tab — script selection, start/stop, session timer, stats, logs.
 */
(function() {
    'use strict';

    const ws = window.botWS;
    const store = window.store;

    let selectedScript = null;
    let running = false;
    let timerInterval = null;
    let sessionStart = null;
    let totalHarvests = 0;
    let harvestCounts = {};  // name -> count

    function init() {
        const el = document.getElementById('tab-harvest');
        if (!el) return;

        el.innerHTML = `
        <!-- Header -->
        <div style="display:flex;align-items:center;justify-content:space-between;margin-bottom:12px;">
            <div style="display:flex;align-items:center;gap:12px;">
                <span style="font-size:16px;font-weight:600;">Recolte automatique</span>
                <span id="harvest-timer" style="font-size:12px;color:var(--text-dim);font-family:var(--font-mono);"></span>
            </div>
            <div style="display:flex;gap:8px;">
                <button class="btn btn-red" id="harvest-stop-btn" disabled onclick="window.harvestComponent.stop()">
                    &#9632; Arreter
                </button>
                <button class="btn btn-green" id="harvest-start-btn" onclick="window.harvestComponent.start()">
                    &#9654; Demarrer
                </button>
            </div>
        </div>

        <div class="grid-1-2">
            <!-- Left: Scripts + Stats -->
            <div>
                <!-- Scripts -->
                <div class="card">
                    <div style="display:flex;justify-content:space-between;align-items:center;margin-bottom:8px;">
                        <div class="card-title" style="margin:0;">Scripts</div>
                        <button class="btn btn-secondary" onclick="window.harvestComponent.refreshScripts()" style="padding:4px 10px;font-size:10px;">&#8635; Actualiser</button>
                    </div>
                    <div id="harvest-script-list" style="max-height:200px;overflow-y:auto;"></div>
                </div>

                <!-- Stats -->
                <div class="card">
                    <div class="card-title">Session</div>
                    <div id="harvest-stats" style="font-size:12px;">
                        <div class="stat-row"><span class="stat-label">Total:</span><span class="stat-value" id="harvest-total">0</span></div>
                        <div class="stat-row"><span class="stat-label">Recoltes/h:</span><span class="stat-value" id="harvest-rate">&mdash;</span></div>
                    </div>
                    <div id="harvest-counts" style="margin-top:8px;font-size:11px;"></div>
                </div>
            </div>

            <!-- Right: Logs -->
            <div class="card" style="display:flex;flex-direction:column;">
                <div style="display:flex;justify-content:space-between;align-items:center;margin-bottom:8px;">
                    <div class="card-title" style="margin:0;">Logs</div>
                    <button class="btn btn-secondary" onclick="window.harvestComponent.clearLogs()" style="padding:4px 10px;font-size:10px;">Vider</button>
                </div>
                <div id="harvest-logs" class="log-area" style="flex:1;min-height:300px;"></div>
            </div>
        </div>
        `;

        // Request script list
        ws.send('getScripts', {});
    }

    // -- Scripts --
    function renderScripts(scripts) {
        const el = document.getElementById('harvest-script-list');
        if (!el) return;

        if (!scripts || scripts.length === 0) {
            el.innerHTML = '<div style="color:var(--text-dim);font-size:11px;padding:8px;">Aucun script trouve</div>';
            return;
        }

        el.innerHTML = scripts.map((path, i) => {
            const name = path.split(/[/\\]/).pop();
            const isSelected = path === selectedScript;
            return `<div class="harvest-script-item${isSelected ? ' selected' : ''}"
                         data-path="${path}" data-idx="${i}"
                         onclick="window.harvestComponent.selectScript('${path.replace(/\\/g, '\\\\')}')"
                         style="padding:6px 8px;cursor:pointer;border-radius:6px;font-size:11px;font-family:var(--font-mono);
                                color:${isSelected ? 'var(--accent)' : 'var(--text-dim)'};
                                background:${isSelected ? 'var(--accent-bg)' : 'transparent'};
                                margin-bottom:2px;">
                        ${name}
                    </div>`;
        }).join('');
    }

    function selectScript(path) {
        selectedScript = path;
        ws.send('getScripts', {});  // Re-render
        addLog(`Script selectionne: ${path.split(/[/\\]/).pop()}`, 'debug');
    }

    // -- Start / Stop --
    function start() {
        if (!selectedScript) {
            addLog('Selectionnez un script d\'abord', 'warning');
            return;
        }
        ws.send('loadLua', { path: selectedScript });
        running = true;
        sessionStart = Date.now();
        totalHarvests = 0;
        harvestCounts = {};
        updateButtons();
        startTimer();
        addLog(`Demarrage: ${selectedScript.split(/[/\\]/).pop()}`, 'success');
    }

    function stop() {
        ws.send('stopLua', {});
        running = false;
        updateButtons();
        stopTimer();
        addLog('Script arrete', 'warning');
    }

    function updateButtons() {
        const startBtn = document.getElementById('harvest-start-btn');
        const stopBtn = document.getElementById('harvest-stop-btn');
        if (startBtn) startBtn.disabled = running;
        if (stopBtn) stopBtn.disabled = !running;
    }

    // -- Timer --
    function startTimer() {
        stopTimer();
        timerInterval = setInterval(updateTimer, 1000);
        updateTimer();
    }

    function stopTimer() {
        if (timerInterval) {
            clearInterval(timerInterval);
            timerInterval = null;
        }
    }

    function updateTimer() {
        if (!sessionStart) return;
        const el = document.getElementById('harvest-timer');
        if (!el) return;
        const elapsed = Math.floor((Date.now() - sessionStart) / 1000);
        const h = Math.floor(elapsed / 3600);
        const m = Math.floor((elapsed % 3600) / 60);
        const s = elapsed % 60;
        el.textContent = `${h.toString().padStart(2,'0')}:${m.toString().padStart(2,'0')}:${s.toString().padStart(2,'0')}`;

        // Update rate
        updateRate(elapsed);
    }

    // -- Stats --
    function recordHarvest(name) {
        totalHarvests++;
        const key = name || 'Ressource';
        harvestCounts[key] = (harvestCounts[key] || 0) + 1;
        updateStats();
    }

    function updateStats() {
        const totalEl = document.getElementById('harvest-total');
        if (totalEl) totalEl.textContent = totalHarvests;

        const countsEl = document.getElementById('harvest-counts');
        if (countsEl) {
            countsEl.innerHTML = Object.entries(harvestCounts)
                .map(([name, count]) =>
                    `<div class="stat-row"><span class="stat-label" style="width:auto;margin-right:8px;">${name}:</span><span class="stat-value" style="color:var(--text-cyan);">${count}</span></div>`
                ).join('');
        }
    }

    function updateRate(elapsedSec) {
        const rateEl = document.getElementById('harvest-rate');
        if (!rateEl) return;
        if (elapsedSec > 0 && totalHarvests > 0) {
            const perHour = Math.round(totalHarvests / elapsedSec * 3600);
            rateEl.textContent = `~${perHour}/h`;
        } else {
            rateEl.textContent = '\u2014';
        }
    }

    // -- Logs --
    function addLog(text, level) {
        const el = document.getElementById('harvest-logs');
        if (!el) return;
        const ts = new Date().toLocaleTimeString('fr-FR', { hour12: false });
        const colors = {
            info: 'var(--text)', success: 'var(--text-green)', warning: 'var(--text-yellow)',
            error: 'var(--text-red)', debug: 'var(--text-dim)', gather: 'var(--text-cyan)',
            nav: 'var(--text-blue)',
        };
        const div = document.createElement('div');
        div.className = 'log-entry';
        div.style.color = colors[level] || colors.info;
        div.textContent = `[${ts}] ${text}`;
        el.appendChild(div);
        el.scrollTop = el.scrollHeight;

        // Trim
        while (el.children.length > 500) el.removeChild(el.firstChild);
    }

    function clearLogs() {
        const el = document.getElementById('harvest-logs');
        if (el) el.innerHTML = '';
    }

    // -- WebSocket handlers --
    ws.on('ScriptList', (payload) => {
        renderScripts(payload.scripts || []);
    });

    ws.on('GatheringStats', (payload) => {
        recordHarvest(payload.gatherableName);
        addLog(`Recolte: ${payload.gatherableName || 'Ressource'}`, 'gather');
    });

    ws.on('ServiceState', (payload) => {
        if (payload.service === 'gathering') {
            running = payload.status;
            updateButtons();
            if (running && !timerInterval) {
                sessionStart = Date.now();
                startTimer();
            } else if (!running) {
                stopTimer();
            }
        }
    });

    ws.on('Log', (payload) => {
        addLog(payload.message, payload.type || 'info');
    });

    // -- Public API --
    window.harvestComponent = {
        init,
        start,
        stop,
        selectScript,
        refreshScripts: () => ws.send('getScripts', {}),
        clearLogs,
    };

    document.addEventListener('DOMContentLoaded', init);

})();

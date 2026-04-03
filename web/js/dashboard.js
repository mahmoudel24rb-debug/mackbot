/**
 * Dashboard component — character info, map position, bot status.
 */
window.dashboardComponent = {

    init() {
        const el = document.getElementById('dashboard-content');
        if (!el) return;

        el.innerHTML = `
        <div class="grid-2">
            <!-- Character card -->
            <div class="card">
                <div class="card-title">Personnage</div>

                <!-- HP bar -->
                <div class="stat-row" style="gap:8px; margin-bottom:4px;">
                    <span class="stat-label" style="width:30px;">HP</span>
                    <div class="progress-bar" style="flex:1;">
                        <div class="progress-fill hp" id="dash-hp-fill" style="width:0%"></div>
                    </div>
                    <span id="dash-hp-pct" style="width:40px;text-align:right;font-size:10px;color:var(--text-dim);">0%</span>
                </div>

                <!-- MP bar -->
                <div class="stat-row" style="gap:8px; margin-bottom:4px;">
                    <span class="stat-label" style="width:30px;">MP</span>
                    <div class="progress-bar" style="flex:1;">
                        <div class="progress-fill mp" id="dash-mp-fill" style="width:0%"></div>
                    </div>
                    <span id="dash-mp-text" style="width:40px;text-align:right;font-size:10px;color:var(--text-dim);">0/0</span>
                </div>

                <!-- AP bar -->
                <div class="stat-row" style="gap:8px; margin-bottom:8px;">
                    <span class="stat-label" style="width:30px;">PA</span>
                    <div class="progress-bar" style="flex:1;">
                        <div class="progress-fill ap" id="dash-ap-fill" style="width:0%"></div>
                    </div>
                    <span id="dash-ap-text" style="width:40px;text-align:right;font-size:10px;color:var(--text-dim);">0/0</span>
                </div>

                <div class="stat-row"><span class="stat-label">Nom:</span><span class="stat-value" id="dash-name">&mdash;</span></div>
                <div class="stat-row"><span class="stat-label">Niveau:</span><span class="stat-value" id="dash-level">&mdash;</span></div>
                <div class="stat-row"><span class="stat-label">HP:</span><span class="stat-value" id="dash-hp-text">&mdash;</span></div>
                <div class="stat-row"><span class="stat-label">MP:</span><span class="stat-value" id="dash-mp-val">&mdash;</span></div>
                <div class="stat-row"><span class="stat-label">PA:</span><span class="stat-value" id="dash-ap-val">&mdash;</span></div>
                <div class="stat-row"><span class="stat-label">Kamas:</span><span class="stat-value" id="dash-kamas">&mdash;</span></div>
            </div>

            <!-- Map card -->
            <div class="card">
                <div class="card-title">Carte & Position</div>
                <div class="stat-row"><span class="stat-label">Map ID:</span><span class="stat-value" id="dash-map-id">&mdash;</span></div>
                <div class="stat-row"><span class="stat-label">Coordonnees:</span><span class="stat-value" id="dash-coords">&mdash;</span></div>
                <div class="stat-row"><span class="stat-label">Cellule:</span><span class="stat-value" id="dash-cell">&mdash;</span></div>
                <div class="stat-row"><span class="stat-label">Ressources dispo:</span><span class="stat-value" id="dash-resources">0</span></div>

                <div style="height:1px;background:var(--border);margin:10px 0;"></div>
                <div style="font-size:11px;color:var(--text-dim);margin-bottom:6px;">Entites sur la carte</div>
                <div style="display:flex;gap:20px;">
                    <div><span id="dash-monsters" style="font-size:16px;font-weight:700;color:var(--text-red);">0</span> <span style="font-size:10px;color:var(--text-dim);">Monstres</span></div>
                    <div><span id="dash-npcs" style="font-size:16px;font-weight:700;color:var(--text-purple);">0</span> <span style="font-size:10px;color:var(--text-dim);">PNJ</span></div>
                    <div><span id="dash-players" style="font-size:16px;font-weight:700;color:var(--text-blue);">0</span> <span style="font-size:10px;color:var(--text-dim);">Joueurs</span></div>
                </div>
            </div>
        </div>

        <!-- Bot status card -->
        <div class="card">
            <div class="card-title">Statut bot</div>
            <div id="dash-bot-status" style="font-size:14px;font-weight:600;color:var(--text-dim);">Inactif</div>
            <div id="dash-bot-detail" style="font-size:11px;color:var(--text-dim);margin-top:2px;"></div>
        </div>
        `;
    },

    refresh() {
        const s = window.store;
        const c = s.character;

        // HP bar
        const hpRatio = c.maxHp > 0 ? c.hp / c.maxHp : 0;
        const hpPct = Math.round(hpRatio * 100);
        let hpColor = 'var(--text-green)';
        if (hpRatio < 0.2) hpColor = 'var(--text-red)';
        else if (hpRatio < 0.5) hpColor = 'var(--text-yellow)';

        this._set('dash-hp-fill', null, { width: hpPct + '%', background: hpColor });
        this._setText('dash-hp-pct', hpPct + '%');

        // MP bar
        const mpRatio = c.maxMp > 0 ? c.mp / c.maxMp : 0;
        this._set('dash-mp-fill', null, { width: Math.round(mpRatio * 100) + '%' });
        this._setText('dash-mp-text', `${c.mp}/${c.maxMp}`);

        // AP bar
        const apRatio = c.maxAp > 0 ? c.ap / c.maxAp : 0;
        this._set('dash-ap-fill', null, { width: Math.round(apRatio * 100) + '%' });
        this._setText('dash-ap-text', `${c.ap}/${c.maxAp}`);

        // Text fields
        this._setText('dash-name', c.name || '\u2014');
        this._setText('dash-level', c.level || '\u2014');
        this._setText('dash-hp-text', c.maxHp > 0 ? `${c.hp} / ${c.maxHp}` : '\u2014');
        this._setText('dash-mp-val', c.maxMp > 0 ? `${c.mp} / ${c.maxMp}` : '\u2014');
        this._setText('dash-ap-val', c.maxAp > 0 ? `${c.ap} / ${c.maxAp}` : '\u2014');
        this._setText('dash-kamas', c.kamas > 0 ? c.kamas.toLocaleString('fr-FR') : '\u2014');

        // Map
        this._setText('dash-map-id', s.map.id || '\u2014');
        this._setText('dash-coords', s.map.x != null ? `(${s.map.x}, ${s.map.y})` : '\u2014');
        this._setText('dash-cell', s.map.cellId != null ? s.map.cellId : '\u2014');
        this._setText('dash-resources', s.stats.resourcesOnMap);

        // Entities
        this._setText('dash-monsters', s.stats.monstersOnMap);
        this._setText('dash-npcs', s.stats.npcsOnMap);
        this._setText('dash-players', s.stats.playersOnMap);

        // Bot status
        const statusEl = document.getElementById('dash-bot-status');
        const detailEl = document.getElementById('dash-bot-detail');
        if (statusEl) {
            if (s.bot.inFight) {
                statusEl.textContent = '\u2694 En combat';
                statusEl.style.color = 'var(--text-red)';
            } else if (s.bot.scriptRunning) {
                statusEl.textContent = '\u25B6 Script actif';
                statusEl.style.color = 'var(--text-green)';
            } else if (s.bot.isBusy) {
                statusEl.textContent = '\u23F3 ' + (s.bot.busyReason || 'occupe');
                statusEl.style.color = 'var(--text-yellow)';
            } else {
                statusEl.textContent = 'Inactif';
                statusEl.style.color = 'var(--text-dim)';
            }
        }
        if (detailEl) {
            detailEl.textContent = '';
        }
    },

    _setText(id, text) {
        const el = document.getElementById(id);
        if (el) el.textContent = text;
    },

    _set(id, text, styles) {
        const el = document.getElementById(id);
        if (!el) return;
        if (text !== null && text !== undefined) el.textContent = text;
        if (styles) Object.assign(el.style, styles);
    }
};

document.addEventListener('DOMContentLoaded', () => {
    window.dashboardComponent.init();
});

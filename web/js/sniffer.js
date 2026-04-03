/**
 * Sniffer tab — live traffic capture + code matching.
 */
(function() {
    'use strict';

    const ws = window.botWS;
    let sniffing = false;
    let packetCount = 0;
    let newCodes = 0;
    let dirFilter = 'Both';  // Both | C2S | S2C

    function init() {
        const el = document.getElementById('tab-sniffer');
        if (!el) return;

        el.innerHTML = `
        <!-- Header -->
        <div class="card" style="padding:10px 16px;margin-bottom:8px;">
            <div style="display:flex;align-items:center;gap:8px;flex-wrap:wrap;">
                <span style="font-size:14px;font-weight:700;color:var(--accent);">Sniffer / Matching</span>
                <span id="sniff-status" style="font-size:10px;color:var(--text-dim);">Idle</span>
                <div style="flex:1;"></div>
                <button class="btn btn-green" id="sniff-start-btn" onclick="window.snifferComponent.toggleSniff()">Start Sniffing</button>
                <button class="btn btn-secondary" onclick="window.snifferComponent.clearTraffic()">Clear</button>
                <button class="btn btn-green" onclick="window.snifferComponent.saveMatching()">Save Matching</button>
                <div id="sniff-filter" style="display:flex;gap:2px;">
                    <button class="btn btn-secondary sniff-dir active" data-dir="Both" onclick="window.snifferComponent.setFilter('Both')">Both</button>
                    <button class="btn btn-secondary sniff-dir" data-dir="C2S" onclick="window.snifferComponent.setFilter('C2S')">C2S</button>
                    <button class="btn btn-secondary sniff-dir" data-dir="S2C" onclick="window.snifferComponent.setFilter('S2C')">S2C</button>
                </div>
            </div>
        </div>

        <div class="grid-1-2">
            <!-- Left: Matching -->
            <div class="card" style="display:flex;flex-direction:column;">
                <div class="card-title">Code Matching</div>
                <div id="sniff-matching" class="log-area" style="flex:1;min-height:300px;font-size:10px;"></div>
            </div>
            <!-- Right: Traffic -->
            <div class="card" style="display:flex;flex-direction:column;">
                <div class="card-title">Live Traffic</div>
                <div id="sniff-traffic" class="log-area" style="flex:1;min-height:300px;font-size:10px;"></div>
            </div>
        </div>

        <!-- Stats -->
        <div class="card" style="padding:6px 16px;margin-top:8px;">
            <span id="sniff-stats" style="font-size:10px;color:var(--text-dim);">Packets: 0 | Codes: 0 | New: 0</span>
        </div>
        `;
    }

    function toggleSniff() {
        if (sniffing) {
            sniffing = false;
            ws.send('stopSniff', {});
            document.getElementById('sniff-start-btn').textContent = 'Start Sniffing';
            document.getElementById('sniff-start-btn').className = 'btn btn-green';
            document.getElementById('sniff-status').textContent = 'Stopped';
            document.getElementById('sniff-status').style.color = 'var(--text-dim)';
        } else {
            sniffing = true;
            ws.send('startSniff', {});
            document.getElementById('sniff-start-btn').textContent = 'Stop Sniffing';
            document.getElementById('sniff-start-btn').className = 'btn btn-red';
            document.getElementById('sniff-status').textContent = 'Sniffing...';
            document.getElementById('sniff-status').style.color = 'var(--text-green)';
        }
    }

    function setFilter(dir) {
        dirFilter = dir;
        document.querySelectorAll('.sniff-dir').forEach(btn => {
            btn.classList.toggle('active', btn.dataset.dir === dir);
            if (btn.dataset.dir === dir) {
                btn.style.background = 'var(--accent-dim)';
                btn.style.color = 'var(--bg)';
            } else {
                btn.style.background = '';
                btn.style.color = '';
            }
        });
    }

    function addTraffic(code, name, direction, size) {
        packetCount++;
        if (dirFilter === 'C2S' && direction !== 'c2s') { updateStats(); return; }
        if (dirFilter === 'S2C' && direction !== 's2c') { updateStats(); return; }

        const el = document.getElementById('sniff-traffic');
        if (!el) return;

        const arrow = direction === 'c2s' ? '>>>' : '<<<';
        const color = direction === 'c2s' ? 'var(--text-blue)' : 'var(--text-green)';
        const div = document.createElement('div');
        div.className = 'log-entry';
        div.style.color = color;
        div.textContent = `${arrow} ${code} (${name}) [${size}B]`;
        el.appendChild(div);
        el.scrollTop = el.scrollHeight;

        while (el.children.length > 500) el.removeChild(el.firstChild);
        updateStats();
    }

    function addMatch(code, name, isNew) {
        if (isNew) newCodes++;
        const el = document.getElementById('sniff-matching');
        if (!el) return;

        const div = document.createElement('div');
        div.className = 'log-entry';
        div.style.color = isNew ? 'var(--text-yellow)' : 'var(--text-dim)';
        div.textContent = `${isNew ? 'NEW ' : '    '}${code} -> ${name}`;
        el.appendChild(div);
        el.scrollTop = el.scrollHeight;
        updateStats();
    }

    function refreshMatching(codes) {
        const el = document.getElementById('sniff-matching');
        if (!el) return;
        el.innerHTML = '';
        const sorted = Object.entries(codes).sort((a, b) => a[1].localeCompare(b[1]));
        for (const [code, name] of sorted) {
            const div = document.createElement('div');
            div.className = 'log-entry';
            div.style.color = 'var(--text-dim)';
            div.textContent = `  ${code} -> ${name}`;
            el.appendChild(div);
        }
    }

    function clearTraffic() {
        const el = document.getElementById('sniff-traffic');
        if (el) el.innerHTML = '';
        packetCount = 0;
        updateStats();
    }

    function saveMatching() {
        ws.send('saveMatching', {});
        const status = document.getElementById('sniff-status');
        if (status) {
            status.textContent = 'Matching saved!';
            status.style.color = 'var(--text-green)';
        }
    }

    function updateStats() {
        const el = document.getElementById('sniff-stats');
        if (!el) return;
        const matchEl = document.getElementById('sniff-matching');
        const codeCount = matchEl ? matchEl.children.length : 0;
        el.textContent = `Packets: ${packetCount} | Codes: ${codeCount} | New: ${newCodes}`;
    }

    // -- WebSocket handlers --
    ws.on('SniffTraffic', (p) => addTraffic(p.code, p.name, p.direction, p.size));
    ws.on('SniffMatch', (p) => addMatch(p.code, p.name, p.isNew || p.is_new));
    ws.on('SniffMatchingRefresh', (p) => refreshMatching(p));

    // -- Public API --
    window.snifferComponent = { init, toggleSniff, setFilter, clearTraffic, saveMatching };

    document.addEventListener('DOMContentLoaded', init);
})();

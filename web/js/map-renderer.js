/**
 * Map Renderer — Canvas HTML5 isometric grid (Jitsuri-style).
 * 560 cells (14 cols x 40 rows), diamond layout.
 */
(function() {
    'use strict';

    const MAP_CONFIG = {
        MAP_WIDTH: 14,
        MAP_HEIGHT: 40,
        CELL_WIDTH: 86,
        CELL_HEIGHT: 43,
        get CELL_HALF_WIDTH() { return this.CELL_WIDTH / 2; },
        get CELL_HALF_HEIGHT() { return this.CELL_HEIGHT / 2; },
        get WIDTH() { return this.MAP_WIDTH * this.CELL_WIDTH + this.CELL_HALF_WIDTH; },
        get HEIGHT() { return this.MAP_HEIGHT * this.CELL_HALF_HEIGHT + this.CELL_HALF_HEIGHT; },
    };

    let canvas, ctx;
    let cellMap = new Map();           // cellId -> { cellNumber, mov, los, mapChangeData }
    let entities = [];                 // [{ id, cellId, type, name, level }]
    let characterCellId = null;
    let hoveredCell = null;
    let displayCellIds = true;

    // Assets
    const assets = {};
    let assetsLoaded = 0;
    const ASSET_LIST = ['grayCell', 'purpleCell', 'areaUnitHigh'];

    function loadAssets(callback) {
        ASSET_LIST.forEach(name => {
            const img = new Image();
            img.onload = () => {
                assetsLoaded++;
                if (assetsLoaded === ASSET_LIST.length) callback();
            };
            img.onerror = () => {
                console.warn(`[MAP] Failed to load ${name}.png, using fallback`);
                assetsLoaded++;
                if (assetsLoaded === ASSET_LIST.length) callback();
            };
            img.src = `assets/${name}.png`;
            assets[name] = img;
        });
    }

    // -- Cell position (from Jitsuri) --
    function cellPosition(cellId) {
        const row = Math.floor(cellId / MAP_CONFIG.MAP_WIDTH);
        const col = cellId % MAP_CONFIG.MAP_WIDTH;
        const x = col * MAP_CONFIG.CELL_WIDTH + (row % 2) * MAP_CONFIG.CELL_HALF_WIDTH;
        const y = row * MAP_CONFIG.CELL_HALF_HEIGHT;
        return { x, y, row, col };
    }

    // -- Hit test: mouse -> cellId --
    function getCellFromMouse(mouseX, mouseY) {
        const rect = canvas.getBoundingClientRect();
        const scaleX = canvas.width / rect.width;
        const scaleY = canvas.height / rect.height;
        const x = (mouseX - rect.left) * scaleX;
        const y = (mouseY - rect.top) * scaleY;

        // Try nearby rows (isometric overlap)
        const approxRow = Math.floor(y / MAP_CONFIG.CELL_HALF_HEIGHT);
        let bestCell = null;
        let bestDist = Infinity;

        for (let dr = -1; dr <= 1; dr++) {
            const row = approxRow + dr;
            if (row < 0 || row >= MAP_CONFIG.MAP_HEIGHT) continue;
            const offset = (row % 2) * MAP_CONFIG.CELL_HALF_WIDTH;
            const col = Math.floor((x - offset) / MAP_CONFIG.CELL_WIDTH);
            if (col < 0 || col >= MAP_CONFIG.MAP_WIDTH) continue;

            const cellId = row * MAP_CONFIG.MAP_WIDTH + col;
            const pos = cellPosition(cellId);
            const cx = pos.x + MAP_CONFIG.CELL_HALF_WIDTH;
            const cy = pos.y + MAP_CONFIG.CELL_HALF_HEIGHT;
            const dist = Math.abs(x - cx) / MAP_CONFIG.CELL_HALF_WIDTH +
                         Math.abs(y - cy) / MAP_CONFIG.CELL_HALF_HEIGHT;
            if (dist < 1.0 && dist < bestDist) {
                bestDist = dist;
                bestCell = cellId;
            }
        }
        return bestCell;
    }

    // -- Draw diamond shape --
    function drawDiamond(x, y, w, h, fill, stroke) {
        const hw = w / 2, hh = h / 2;
        ctx.beginPath();
        ctx.moveTo(x + hw, y);
        ctx.lineTo(x + w, y + hh);
        ctx.lineTo(x + hw, y + h);
        ctx.lineTo(x, y + hh);
        ctx.closePath();
        if (fill) { ctx.fillStyle = fill; ctx.fill(); }
        if (stroke) { ctx.strokeStyle = stroke; ctx.lineWidth = 0.5; ctx.stroke(); }
    }

    // -- Main render --
    function render() {
        if (!ctx) return;
        const W = MAP_CONFIG.WIDTH;
        const H = MAP_CONFIG.HEIGHT;
        ctx.clearRect(0, 0, W, H);

        // Background
        ctx.fillStyle = '#0a0c12';
        ctx.fillRect(0, 0, W, H);

        // Build entity lookup
        const entityMap = new Map();
        for (const e of entities) {
            if (e.cellId != null) entityMap.set(e.cellId, e);
        }

        // Draw cells
        for (let cellId = 0; cellId < 560; cellId++) {
            const pos = cellPosition(cellId);
            const cell = cellMap.get(cellId);
            const isWalkable = cell ? cell.mov : true;
            const isLos = cell ? cell.los : true;
            const mapChange = cell ? cell.mapChangeData : 0;
            const isHovered = hoveredCell === cellId;

            const x = pos.x;
            const y = pos.y;
            const cw = MAP_CONFIG.CELL_WIDTH;
            const ch = MAP_CONFIG.CELL_HEIGHT;

            if (isHovered && isWalkable) {
                // Orange hover
                drawDiamond(x, y, cw, ch, 'rgba(251, 146, 60, 0.6)', '#fb923c');
            } else if (mapChange > 0) {
                // Map change cell (blue)
                drawDiamond(x, y, cw, ch, 'rgba(29, 78, 216, 0.4)', '#1d4ed8');
            } else if (isWalkable) {
                // Walkable - use asset or fallback color
                const img = (pos.row % 2 === 0) ? assets.grayCell : assets.purpleCell;
                if (img && img.complete && img.naturalWidth > 0) {
                    ctx.drawImage(img, x, y, cw, ch);
                } else {
                    const fill = (pos.row % 2 === 0) ? '#1a2e2e' : '#1e3434';
                    drawDiamond(x, y, cw, ch, fill, '#0f1219');
                }
            } else {
                // Wall/obstacle
                if (!isLos && assets.areaUnitHigh && assets.areaUnitHigh.complete && assets.areaUnitHigh.naturalWidth > 0) {
                    const wallH = ch * 2;
                    ctx.drawImage(assets.areaUnitHigh, x, y - ch * 0.6, cw, wallH);
                } else {
                    drawDiamond(x, y, cw, ch, '#0c0e14', '#080a10');
                }
            }

            // Entities
            const entity = entityMap.get(cellId);
            if (cellId === characterCellId) {
                // Player (yellow circle)
                drawCircle(x + cw/2, y + ch/2, 8, '#facc15', '#fde047');
            } else if (entity) {
                const type = entity.type;
                if (type === 'monster') {
                    drawCircle(x + cw/2, y + ch/2, 6, '#ef4444');
                } else if (type === 'npc') {
                    drawCircle(x + cw/2, y + ch/2, 6, '#fdffa7');
                } else if (type === 'player') {
                    drawCircle(x + cw/2, y + ch/2, 6, '#38bdf8');
                } else {
                    drawCircle(x + cw/2, y + ch/2, 5, '#a78bfa');
                }
            }

            // Resources
            const store = window.store;
            if (store && store.map.resources) {
                for (const r of store.map.resources) {
                    if (r.cellId === cellId && r.available) {
                        drawCircle(x + cw/2, y + ch/2, 6, '#22c55e');
                    }
                }
            }

            // Cell ID text
            if (displayCellIds && isWalkable) {
                ctx.fillStyle = mapChange > 0 ? '#4a6a9a' : '#3a4a5a';
                ctx.font = '7px Consolas';
                ctx.textAlign = 'center';
                ctx.textBaseline = 'middle';
                ctx.fillText(cellId, x + cw/2, y + ch/2);
            }
        }
    }

    function drawCircle(cx, cy, r, fill, stroke) {
        ctx.beginPath();
        ctx.arc(cx, cy, r, 0, Math.PI * 2);
        ctx.fillStyle = fill;
        ctx.fill();
        if (stroke) {
            ctx.strokeStyle = stroke;
            ctx.lineWidth = 1.5;
            ctx.stroke();
        }
    }

    // -- Resize --
    function resizeCanvas() {
        const container = document.getElementById('map-container');
        if (!container || !canvas) return;

        const containerW = container.clientWidth - 16;
        const containerH = container.clientHeight - 16;
        const scaleW = containerW / MAP_CONFIG.WIDTH;
        const scaleH = containerH / MAP_CONFIG.HEIGHT;
        const scale = Math.min(scaleW, scaleH, 1);

        canvas.style.width = (MAP_CONFIG.WIDTH * scale) + 'px';
        canvas.style.height = (MAP_CONFIG.HEIGHT * scale) + 'px';
        render();
    }

    // -- Legend --
    function renderLegend() {
        const el = document.getElementById('map-legend');
        if (!el) return;
        const items = [
            { color: '#1a2e2e', label: 'Passable' },
            { color: '#0c0e14', label: 'Mur/Obstacle' },
            { color: '#1d4ed8', label: 'Changement carte' },
            { color: '#facc15', label: 'Personnage' },
            { color: '#ef4444', label: 'Monstre' },
            { color: '#fdffa7', label: 'PNJ' },
            { color: '#22c55e', label: 'Ressource' },
        ];
        el.innerHTML = items.map(i =>
            `<div style="display:flex;align-items:center;gap:4px;">
                <div style="width:10px;height:10px;border-radius:2px;background:${i.color};"></div>
                <span style="font-size:10px;color:var(--text-dim);">${i.label}</span>
            </div>`
        ).join('');
    }

    // -- Cell info on hover --
    function updateCellInfo(cellId) {
        const el = document.getElementById('map-cell-info');
        if (!el) return;
        if (cellId === null) {
            el.textContent = '';
            return;
        }
        const cell = cellMap.get(cellId);
        const walkable = cell ? cell.mov : true;
        const mc = cell ? cell.mapChangeData : 0;
        let info = `Cell: ${cellId}`;
        if (walkable) info += ' | Passable';
        else info += ' | Bloque';
        if (mc > 0) info += ' | Transition';
        el.textContent = info;
    }

    // -- Init --
    function init() {
        canvas = document.getElementById('map-canvas');
        if (!canvas) return;
        ctx = canvas.getContext('2d');

        canvas.width = MAP_CONFIG.WIDTH;
        canvas.height = MAP_CONFIG.HEIGHT;

        // Mouse events
        canvas.addEventListener('mousemove', (e) => {
            const cellId = getCellFromMouse(e.clientX, e.clientY);
            if (cellId !== hoveredCell) {
                hoveredCell = cellId;
                updateCellInfo(cellId);
                render();
            }
        });

        canvas.addEventListener('mouseleave', () => {
            hoveredCell = null;
            updateCellInfo(null);
            render();
        });

        canvas.addEventListener('click', (e) => {
            const cellId = getCellFromMouse(e.clientX, e.clientY);
            if (cellId !== null) {
                const cell = cellMap.get(cellId);
                if (cell && cell.mov) {
                    window.botWS.send('moveTo', { cellId: cellId });
                    window.store.addLog(`Move vers cell ${cellId}`, 'nav');
                }
            }
        });

        window.addEventListener('resize', resizeCanvas);

        // Load assets then render
        loadAssets(() => {
            renderLegend();
            resizeCanvas();
            render();
        });

        // Fallback render if assets fail
        setTimeout(() => {
            renderLegend();
            resizeCanvas();
            render();
        }, 1000);
    }

    // -- WebSocket handlers --
    const ws = window.botWS;

    ws.on('MapCellData', (payload) => {
        if (payload.cells) {
            cellMap = new Map(payload.cells.map(c => [c.cellNumber, c]));
        }
        render();
    });

    ws.on('MapEntities', (payload) => {
        entities = payload.entities || [];
        characterCellId = payload.characterCellId;
        render();
    });

    ws.on('MapInformation', () => {
        // Map changed, will get new MapCellData soon
    });

    ws.on('Status', (payload) => {
        const newCell = payload.cell_id;
        if (newCell !== characterCellId) {
            characterCellId = newCell;
            render();
        }
        // Update coords badge
        const badge = document.getElementById('map-coords-badge');
        if (badge && payload.map_x != null) {
            badge.textContent = `(${payload.map_x}, ${payload.map_y})`;
        }
    });

    // Expose for tab switching
    window.mapRenderer = {
        init,
        render,
        resize: resizeCanvas,
    };

    document.addEventListener('DOMContentLoaded', init);

})();

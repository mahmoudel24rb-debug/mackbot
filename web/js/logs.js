/**
 * Logs component — displays timestamped, color-coded log entries.
 */
window.logsComponent = {

    _container: null,

    init() {
        this._container = document.getElementById('logs-container');
    },

    refresh() {
        if (!this._container) this.init();
        if (!this._container) return;

        const store = window.store;
        const logs = store.logs;

        // Only render new entries (avoid full rebuild)
        const rendered = this._container.childElementCount;
        const toRender = logs.slice(rendered);

        for (const entry of toRender) {
            const div = document.createElement('div');
            div.className = 'log-entry';
            div.style.color = this._color(entry.level);
            div.textContent = `[${entry.ts}] ${entry.text}`;
            this._container.appendChild(div);
        }

        // Auto-scroll
        this._container.scrollTop = this._container.scrollHeight;

        // Trim DOM if too many
        while (this._container.children.length > 500) {
            this._container.removeChild(this._container.firstChild);
        }
    },

    clear() {
        if (this._container) this._container.innerHTML = '';
        window.store.logs = [];
    },

    _color(level) {
        const colors = {
            info:    'var(--text)',
            success: 'var(--text-green)',
            warning: 'var(--text-yellow)',
            error:   'var(--text-red)',
            debug:   'var(--text-dim)',
            gather:  'var(--text-cyan)',
            nav:     'var(--text-blue)',
            fight:   'var(--text-red)',
        };
        return colors[level] || colors.info;
    }
};

document.addEventListener('DOMContentLoaded', () => {
    window.logsComponent.init();
});

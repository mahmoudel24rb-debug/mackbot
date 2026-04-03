/**
 * WebSocket client with auto-reconnection.
 * Connects to the Python backend on ws://localhost:7777
 */
class BotWebSocket {
    constructor(url = 'ws://localhost:7777') {
        this.url = url;
        this.ws = null;
        this.listeners = {};
        this.reconnectDelay = 1000;
        this.maxReconnectDelay = 10000;
        this.currentDelay = this.reconnectDelay;
        this.shouldReconnect = true;
        this.onStatusChange = null;
    }

    connect() {
        try {
            this.ws = new WebSocket(this.url);

            this.ws.onopen = () => {
                console.log('[WS] Connected');
                this.currentDelay = this.reconnectDelay;
                if (this.onStatusChange) this.onStatusChange(true);
            };

            this.ws.onclose = () => {
                console.log('[WS] Disconnected');
                if (this.onStatusChange) this.onStatusChange(false);
                if (this.shouldReconnect) this._scheduleReconnect();
            };

            this.ws.onerror = (err) => {
                console.error('[WS] Error:', err);
            };

            this.ws.onmessage = (event) => {
                try {
                    const msg = JSON.parse(event.data);
                    const { type, payload, processId } = msg;
                    this._dispatch(type, payload, processId);
                } catch (e) {
                    console.error('[WS] Parse error:', e);
                }
            };
        } catch (e) {
            console.error('[WS] Connection failed:', e);
            if (this.shouldReconnect) this._scheduleReconnect();
        }
    }

    _scheduleReconnect() {
        setTimeout(() => {
            console.log(`[WS] Reconnecting in ${this.currentDelay}ms...`);
            this.connect();
            this.currentDelay = Math.min(
                this.currentDelay * 1.3,
                this.maxReconnectDelay
            );
        }, this.currentDelay);
    }

    on(type, callback) {
        if (!this.listeners[type]) this.listeners[type] = [];
        this.listeners[type].push(callback);
    }

    _dispatch(type, payload, processId) {
        const handlers = this.listeners[type];
        if (handlers) {
            handlers.forEach(fn => {
                try { fn(payload, processId); }
                catch (e) { console.error(`[WS] Handler error for ${type}:`, e); }
            });
        }
    }

    send(action, data = {}) {
        if (this.ws && this.ws.readyState === WebSocket.OPEN) {
            this.ws.send(JSON.stringify({ action, data }));
        } else {
            console.warn('[WS] Cannot send, not connected');
        }
    }

    disconnect() {
        this.shouldReconnect = false;
        if (this.ws) this.ws.close();
    }
}

// Global instance
window.botWS = new BotWebSocket();

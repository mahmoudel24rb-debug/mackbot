"""
Quick test for WebSocket server connection.
Run while app.py is running in another terminal.

Usage: python tools/test_ws.py
"""
import asyncio
import json
import sys
import os
sys.path.insert(0, os.path.dirname(os.path.dirname(__file__)))

async def test():
    import websockets
    uri = "ws://localhost:7777"
    print(f"Connecting to {uri}...")
    try:
        async with websockets.connect(uri) as ws:
            print("Connected!")
            # Listen for 10 seconds
            for _ in range(10):
                try:
                    msg = await asyncio.wait_for(ws.recv(), timeout=2.0)
                    data = json.loads(msg)
                    payload_preview = json.dumps(data.get("payload", {}))[:100]
                    print(f"  [{data['type']}] {payload_preview}")
                except asyncio.TimeoutError:
                    print("  (waiting...)")
            print("Test OK!")
    except ConnectionRefusedError:
        print("ERROR: Cannot connect. Is app.py running?")
    except Exception as e:
        print(f"ERROR: {e}")

if __name__ == "__main__":
    asyncio.run(test())

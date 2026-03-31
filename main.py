"""
Dofus 3 MITM Proxy Sniffer - Point d'entrée.

Usage:
    python main.py
    python main.py --port 5555 --server-host 1.2.3.4 --server-port 5555
    python main.py --quiet
"""

import argparse
import asyncio
import re
import subprocess
import sys

from utils import logger
import config


def resolve_login_server():
    """Resolve the real login server IP via nslookup (bypasses hosts file)."""
    hostname = config.SERVER_HOSTNAME
    try:
        result = subprocess.run(
            ["nslookup", hostname],
            capture_output=True, text=True, timeout=10
        )
        # Parse IPs from nslookup output (skip the DNS server's own IP)
        lines = result.stdout.split("\n")
        in_answer = False
        ips = []
        for line in lines:
            # After "Nom :" or "Name:" we're in the answer section
            if "nom :" in line.lower() or "name:" in line.lower():
                in_answer = True
                continue
            if in_answer:
                match = re.search(r"(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})", line)
                if match:
                    ips.append(match.group(1))
        if ips:
            return ips[0], ips
    except Exception as e:
        logger.error(f"DNS resolution failed: {e}")
    return None, []


def main():
    parser = argparse.ArgumentParser(
        description="Dofus 3 MITM Proxy Sniffer",
        formatter_class=argparse.RawDescriptionHelpFormatter,
        epilog="""
Exemples:
  python main.py                                    # Auto-resolve login server
  python main.py --server-host 1.2.3.4              # Manual IP override
  python main.py --quiet                            # Moins de détails
        """,
    )

    parser.add_argument(
        "--host", default=config.PROXY_HOST,
        help=f"IP d'écoute du proxy (défaut: {config.PROXY_HOST})"
    )
    parser.add_argument(
        "--port", type=int, default=config.PROXY_PORT,
        help=f"Port d'écoute du proxy (défaut: {config.PROXY_PORT})"
    )
    parser.add_argument(
        "--server-host", default=None,
        help="IP du serveur login (défaut: auto-resolve via DNS)"
    )
    parser.add_argument(
        "--server-port", type=int, default=config.SERVER_PORT,
        help=f"Port du serveur Dofus cible (défaut: {config.SERVER_PORT})"
    )
    parser.add_argument(
        "--quiet", "-q", action="store_true",
        help="Mode silencieux (moins de détails)"
    )
    parser.add_argument(
        "--hex-dump-size", type=int, default=config.HEX_DUMP_MAX_BYTES,
        help=f"Taille max du hex dump (défaut: {config.HEX_DUMP_MAX_BYTES})"
    )

    args = parser.parse_args()

    # Apply config overrides
    if args.quiet:
        config.VERBOSE = False
    config.HEX_DUMP_MAX_BYTES = args.hex_dump_size

    # Display banner
    logger.banner()

    # Resolve login server IP
    server_host = args.server_host
    if not server_host:
        logger.info(f"Resolving login server: {config.SERVER_HOSTNAME} ...")
        ip, all_ips = resolve_login_server()
        if ip:
            server_host = ip
            logger.info(f"  -> {', '.join(all_ips)}")
            logger.info(f"  Using: {ip}")
        else:
            logger.error("Cannot resolve login server IP!")
            logger.error(f"  Hostname: {config.SERVER_HOSTNAME}")
            logger.error("  Use --server-host <IP> to specify manually")
            sys.exit(1)
    print()

    logger.info(f"Configuration:")
    logger.info(f"  Proxy:   {args.host}:{args.port}")
    logger.info(f"  Login:   {server_host}:{args.server_port}")
    logger.info(f"  Verbose: {'oui' if config.VERBOSE else 'non'}")
    print()

    logger.info("En attente de connexion du client Dofus...")
    logger.info(f"  hosts: {config.SERVER_HOSTNAME} -> 127.0.0.1")
    logger.info(f"  proxy: 127.0.0.1:{args.port} -> {server_host}:{args.server_port}")
    print()

    # Import here to avoid circular imports
    from proxy.mitm_proxy import run_proxy

    try:
        asyncio.run(run_proxy(
            listen_host=args.host,
            listen_port=args.port,
            server_host=server_host,
            server_port=args.server_port,
        ))
    except KeyboardInterrupt:
        print()
        logger.info("Proxy arrêté (Ctrl+C)")
    except OSError as e:
        logger.error(f"Impossible de démarrer le proxy: {e}")
        sys.exit(1)


if __name__ == "__main__":
    main()

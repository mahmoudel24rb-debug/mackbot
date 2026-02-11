"""
Dofus 3 MITM Proxy Sniffer - Point d'entrée.

Usage:
    python main.py
    python main.py --port 5555 --server-host 127.0.0.1 --server-port 5556
    python main.py --quiet
"""

import argparse
import asyncio
import sys

from utils import logger
import config


def main():
    parser = argparse.ArgumentParser(
        description="Dofus 3 MITM Proxy Sniffer",
        formatter_class=argparse.RawDescriptionHelpFormatter,
        epilog="""
Exemples:
  python main.py                                    # Valeurs par défaut
  python main.py --server-host 192.168.1.100        # Serveur sur le LAN
  python main.py --port 443 --server-port 5555      # Ports personnalisés
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
        "--server-host", default=config.SERVER_HOST,
        help=f"IP du serveur Dofus cible (défaut: {config.SERVER_HOST})"
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

    logger.info(f"Configuration:")
    logger.info(f"  Proxy:   {args.host}:{args.port}")
    logger.info(f"  Serveur: {args.server_host}:{args.server_port}")
    logger.info(f"  Verbose: {'oui' if config.VERBOSE else 'non'}")
    print()

    logger.info("Pour utiliser le sniffer:")
    logger.info("  1. Lance ton serveur Dofus privé")
    logger.info(f"  2. Configure le client Dofus pour se connecter à {args.host}:{args.port}")
    logger.info("  3. Les paquets seront affichés ici en temps réel")
    print()

    # Import here to avoid circular imports
    from proxy.mitm_proxy import run_proxy

    try:
        asyncio.run(run_proxy(
            listen_host=args.host,
            listen_port=args.port,
            server_host=args.server_host,
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

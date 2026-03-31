"""
Protocol matching system.

Maps 3-letter protobuf type_url codes (e.g. "ipx") to stable message names
(e.g. "MapMovementRequest"). Codes rotate every 2-3 weeks; names are stable.

Usage:
    from protocol.matching import Matching
    m = Matching("data/matching.json")
    code = m.get_code("MapMovementRequest")   # "ipx" (or None if unknown)
    name = m.get_name("ipx")                  # "MapMovementRequest"
"""

import json
import os
import time


class Matching:
    """
    Bidirectional mapping between 3-letter codes and stable message names.

    Loaded from data/matching.json at startup; updated by AutoMatcher at runtime.
    """

    def __init__(self, filepath=None):
        self._filepath = filepath or "data/matching.json"
        self._code_to_name = {}   # "ipx" -> "MapMovementRequest"
        self._name_to_code = {}   # "MapMovementRequest" -> "ipx"
        self._version = "initial"
        self.load()

    # ------------------------------------------------------------------
    # Loading / saving
    # ------------------------------------------------------------------

    def load(self, filepath=None):
        """Load matching from JSON file. Resets if older than 21 days (code rotation)."""
        path = filepath or self._filepath
        if not os.path.exists(path):
            return
        try:
            with open(path, "r", encoding="utf-8") as f:
                data = json.load(f)
            self._version = data.get("version", "unknown")

            # Phase 3.1: Check freshness — codes rotate every 2-3 weeks
            if self._version and self._version != "initial":
                try:
                    import datetime
                    version_date = datetime.date.fromisoformat(self._version)
                    age_days = (datetime.date.today() - version_date).days
                    if age_days > 21:
                        print(f"[Matching] WARNING: matching.json is {age_days} days old — "
                              f"codes likely rotated. Resetting to empty.")
                        self._code_to_name = {}
                        self._name_to_code = {}
                        self.save()
                        return
                except (ValueError, TypeError):
                    pass

            codes = data.get("codes", {})
            self._code_to_name = dict(codes)
            self._name_to_code = {v: k for k, v in codes.items()}
        except (json.JSONDecodeError, OSError) as e:
            print(f"[Matching] Failed to load {path}: {e}")

    def save(self, filepath=None):
        """Save current matching to JSON file."""
        path = filepath or self._filepath
        os.makedirs(os.path.dirname(path) if os.path.dirname(path) else ".", exist_ok=True)
        data = {
            "version": time.strftime("%Y-%m-%d"),
            "codes": dict(self._code_to_name),
        }
        with open(path, "w", encoding="utf-8") as f:
            json.dump(data, f, indent=4)
        self._version = data["version"]

    # ------------------------------------------------------------------
    # Lookup
    # ------------------------------------------------------------------

    def get_code(self, message_name):
        """
        Get the current 3-letter code for a stable message name.

        Returns None if not yet discovered.
        """
        return self._name_to_code.get(message_name)

    def get_name(self, type_code):
        """
        Get the stable message name for a 3-letter type_url code.

        Returns the code itself if unknown (so logs still show something).
        """
        return self._code_to_name.get(type_code, type_code)

    def is_known(self, type_code):
        """Return True if this code is already matched."""
        return type_code in self._code_to_name

    # ------------------------------------------------------------------
    # Learning
    # ------------------------------------------------------------------

    def add(self, type_code, message_name, save=True):
        """
        Add or update a code↔name mapping.

        Called by AutoMatcher when it identifies a new code.
        """
        old_name = self._code_to_name.get(type_code)
        old_code = self._name_to_code.get(message_name)

        if old_name == message_name:
            return False  # no change

        # Remove stale reverse entry if code was previously mapped differently
        if old_name and old_name in self._name_to_code:
            del self._name_to_code[old_name]
        # Remove stale forward entry if name was previously mapped to a different code
        if old_code and old_code in self._code_to_name:
            del self._code_to_name[old_code]

        self._code_to_name[type_code] = message_name
        self._name_to_code[message_name] = type_code

        if save:
            self.save()
        return True

    def remove_code(self, type_code):
        """Remove a code (called when we detect it no longer exists)."""
        name = self._code_to_name.pop(type_code, None)
        if name:
            self._name_to_code.pop(name, None)
            self.save()

    # ------------------------------------------------------------------
    # Introspection
    # ------------------------------------------------------------------

    def all_codes(self):
        """Return dict of all known code->name mappings."""
        return dict(self._code_to_name)

    def missing(self, message_names):
        """Return list of message names that have no code yet."""
        return [n for n in message_names if n not in self._name_to_code]

    def __len__(self):
        return len(self._code_to_name)

    def __repr__(self):
        return f"Matching(v={self._version}, {len(self)} codes)"

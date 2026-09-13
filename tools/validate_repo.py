#!/usr/bin/env python3
"""Fast dependency-free repository checks that can run without a Unity license."""
from pathlib import Path
import json
import sys

ROOT = Path(__file__).resolve().parents[1]
required = [
    ROOT / "README.md",
    ROOT / "docs" / "20_AI_BUILD_BRIEF.md",
    ROOT / "docs" / "29_UNITY_PLAYTEST_CHECKLIST.md",
    ROOT / "docs" / "30_PLAYTEST_TELEMETRY.md",
    ROOT / "Game" / "ProjectSettings" / "ProjectVersion.txt",
    ROOT / "Game" / "Packages" / "manifest.json",
    ROOT / "Game" / "Assets" / "_MagnetHavoc" / "Resources" / "game_tuning.json",
    ROOT / "Game" / "Assets" / "_MagnetHavoc" / "Scripts" / "Bootstrap" / "PrototypeBootstrap.cs",
    ROOT / "Game" / "Assets" / "_MagnetHavoc" / "Scripts" / "Core" / "TuningProvider.cs",
    ROOT / "Game" / "Assets" / "_MagnetHavoc" / "Scripts" / "Core" / "TuningRules.cs",
    ROOT / "Game" / "Assets" / "_MagnetHavoc" / "Scripts" / "Diagnostics" / "PrototypeTelemetry.cs",
    ROOT / "Game" / "Assets" / "_MagnetHavoc" / "Scripts" / "Gameplay" / "MagnetAbility.cs",
    ROOT / "Game" / "Assets" / "_MagnetHavoc" / "Scripts" / "Gameplay" / "MatchManager.cs",
    ROOT / "Game" / "Assets" / "_MagnetHavoc" / "Scripts" / "Simulation" / "MatchStatsModel.cs",
]

errors = []
for path in required:
    if not path.exists():
        errors.append(f"missing required file: {path.relative_to(ROOT)}")

for path in ROOT.rglob("*.json"):
    try:
        json.loads(path.read_text(encoding="utf-8"))
    except Exception as exc:
        errors.append(f"invalid JSON: {path.relative_to(ROOT)}: {exc}")

for path in ROOT.rglob("*.cs"):
    text = path.read_text(encoding="utf-8")
    if text.count("{") != text.count("}"):
        errors.append(f"brace mismatch: {path.relative_to(ROOT)}")
    if "TODO: ship later" in text:
        errors.append(f"blocked TODO marker: {path.relative_to(ROOT)}")

if errors:
    print("Repository validation failed:")
    for error in errors:
        print(f" - {error}")
    sys.exit(1)

print(f"Repository validation passed. Checked {sum(1 for _ in ROOT.rglob('*.cs'))} C# files.")

# Magnet Havoc

**PUSH. PULL. THROW EVERYTHING.**

Magnet Havoc is a competitive mobile physics-action arena game. Players sprint, dash, attract objects, repel rivals, steal the Flux Core, and knock each other out in short chaotic matches.

## Current status

🟡 **Pre-alpha / local vertical slice under active development**

The `dev/vertical-slice` branch contains the playable-source implementation plus the first recovery/readability pass. It remains a draft PR until the Unity/editor and physical-device playtest gate passes.

The product specification lives in [`docs/`](docs/00_README.md). The Unity project lives in [`Game/`](Game/).

## Technical baseline

- Unity **6000.3.24f1 / Unity 6.3 LTS**
- C#
- iOS + Android target, landscape
- 4-player local/bot prototype first; 8-player online target
- Physics/knockback combat; no HP and no jump in MVP
- Cosmetic-first monetization; no paid ranked power

## Current prototype

- 1 human + 3 bots
- Core Rush objective mode
- movement, deceleration and dash
- tap Push / hold Pull
- Flux resource
- magnetic props and player knockback
- Core possession/drop/scoring, Overload, timer, winner and rematch
- knockouts, safe respawns and short respawn protection
- automatic Core recovery after falling out of the arena
- bot edge-safety steering
- readable HUD/world markers for Core, dash and respawn protection
- desktop + mobile touch input
- automated repository checks, C# syntax parsing and deterministic rule tests
- Unity EditMode tests plus prepared PlayMode regression tests

## Start here

1. Read [`docs/20_AI_BUILD_BRIEF.md`](docs/20_AI_BUILD_BRIEF.md).
2. Read [`DEVELOPMENT_PLAN.md`](DEVELOPMENT_PLAN.md).
3. Read [`docs/24_DECISIONS_AND_OPEN_QUESTIONS.md`](docs/24_DECISIONS_AND_OPEN_QUESTIONS.md) before changing core mechanics.
4. Install Unity 6000.3.24f1.
5. Open the `Game` folder as a Unity project.
6. Press Play in any empty scene: the prototype bootstraps itself at runtime.
7. Optional: use **Tools → Magnet Havoc → Create Prototype Scene** to create a saved build scene.

### Editor controls

- WASD / arrows — move
- Space — dash
- Left mouse — Push
- Right mouse or Left Shift — Pull
- R — restart after a match

Mobile touch controls are also implemented: left-side drag moves, the lower-right magnet region uses tap=Push / hold=Pull, and the smaller lower-right region dashes.

## Development principle

Before expanding content or live-service systems, prove this loop is fun:

> move → fight for the Core → pull something useful → push something dangerous → knock a rival out → score → rematch

A green CI build proves source health, not game feel. Networking stays gated behind the Unity/device human-fun test.

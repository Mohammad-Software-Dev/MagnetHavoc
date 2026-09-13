# Magnet Havoc

**PUSH. PULL. THROW EVERYTHING.**

Magnet Havoc is a competitive mobile physics-action arena game. Players sprint, dash, attract objects, repel rivals, steal the Flux Core, and knock each other out in short chaotic matches.

## Current status

🟡 **Playable prototype / vertical-slice development**

The product specification lives in [`docs/`](docs/00_README.md). The Unity project lives in [`Game/`](Game/).

## Technical baseline

- Unity **6000.3.24f1 / Unity 6.3 LTS**
- C#
- iOS + Android target, landscape
- 4-player local/bot prototype first; 8-player online target
- Physics/knockback combat; no HP and no jump in MVP
- Cosmetic-first monetization; no paid ranked power

## Start here

1. Read [`docs/20_AI_BUILD_BRIEF.md`](docs/20_AI_BUILD_BRIEF.md).
2. Install Unity 6000.3.24f1.
3. Open the `Game` folder as a Unity project.
4. Press Play in any empty scene: the prototype bootstraps itself at runtime.
5. Optional: use **Tools → Magnet Havoc → Create Prototype Scene** to create a saved build scene.

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

See [`DEVELOPMENT_PLAN.md`](DEVELOPMENT_PLAN.md) for execution stages.

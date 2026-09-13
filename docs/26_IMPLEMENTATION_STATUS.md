# Implementation Status

## Current branch milestone

The first playable local prototype is now represented in source code under `Game/`.

Implemented:
- Unity 6.3 LTS project baseline.
- Runtime bootstrap that can create the prototype in an otherwise empty scene.
- One human player and three bots.
- Rigidbody movement, dash, knockback, and respawn.
- Shared magnet control: tap Push, hold Pull.
- Flux resource with costs and regeneration.
- Magnetic props and Flux Core.
- Core pickup/drop, score accumulation, Overload scoring, match timer, winner, rematch.
- Primitive arena, rotating hazard, cover, and throwable crates.
- Procedural robot-like visual parts and simple movement/magnet animation.
- Follow camera, HUD, desktop controls, mobile touch input.
- Procedurally generated placeholder audio feedback.
- Pure scoring/Flux simulation models plus Unity EditMode tests.
- Dependency-free repository validation workflow.

Not yet implemented:
- Real human-vs-human networking.
- Production art, character rigs, authored animations, production sound/music.
- Accounts, persistence, ranked matchmaking, parties, friends, crews.
- Economy/store/pass or live ops.
- Production analytics, remote configuration, moderation, anti-cheat service.
- Device-lab performance validation.

## Validation requirement

The next meaningful decision is empirical: open the Unity project, run the prototype, and tune until movement + Push/Pull + Core competition is genuinely fun. Do not treat placeholder numeric values as final.

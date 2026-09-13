# Implementation Status

## Current branch milestone

The local vertical-slice source now includes the first Stage 2 recovery/readability pass under `Game/`.

Implemented:
- Unity 6.3 LTS project baseline.
- Runtime bootstrap that can create the prototype in an otherwise empty scene.
- One human player and three bots.
- Rigidbody movement, explicit acceleration/deceleration, dash, knockback, knockout, and respawn.
- Tunable dash resistance to magnetic knockback.
- Short post-respawn magnetic protection.
- Safe-spawn selection that favors distance from active opponents.
- Shared magnet control: tap Push, hold Pull.
- Flux resource with costs and regeneration.
- Magnetic props and Flux Core.
- Core pickup/drop, score accumulation, Overload scoring, match timer, winner, rematch.
- Automatic Flux Core recovery if the objective falls out of the arena.
- Data-driven Core carry offsets/drop behavior.
- Duplicate magnetic-force application on Core carriers removed.
- Primitive arena, rotating hazard, cover, and throwable crates.
- Bots that contest the objective and bias toward arena safety near edges.
- Procedural robot-like visual parts and simple movement/magnet animation.
- World markers for Core ownership and respawn protection.
- Follow camera and HUD with Core, dash, shield, score, Flux, and match state.
- Desktop controls and mobile touch input.
- Procedurally generated placeholder audio feedback.
- Pure scoring/Flux/match-clock simulation models plus Unity EditMode tests.
- Prepared Unity PlayMode tests for bootstrap, Core recovery, and rematch reset behavior.
- Repository validation, Roslyn syntax parsing, and deterministic .NET smoke tests in CI.

Not yet implemented:
- Real human-vs-human networking.
- Production art, character rigs, authored animations, production sound/music.
- Accounts, persistence, ranked matchmaking, parties, friends, crews.
- Economy/store/pass or live ops.
- Production analytics, remote configuration, moderation, anti-cheat service.
- Device-lab performance validation.

## Validation requirement

The next product decision remains empirical: open the Unity project, run the prototype on desktop and phone, and tune until movement + Push/Pull + Core competition is genuinely fun. The new safety/recovery values are still hypotheses, not final balance.

Do not treat a green source/CI pipeline as proof that the physics feel is good. Networking remains gated behind the local fun test.

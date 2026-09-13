# Magnet Havoc Development Plan

## Product goal

Ship a polished mobile competitive physics game whose first five seconds are understandable and whose first match creates a strong urge to rematch.

## Stage 0 — repository + technical baseline

**Status: complete in the vertical-slice branch.**

- Pin Unity 6.3 LTS.
- Establish runtime architecture and tuning model.
- Separate simulation, input, gameplay, and presentation.
- Add repository validation, Roslyn syntax validation, deterministic .NET smoke tests, and Unity test assemblies.

## Stage 1 — local fun prototype

**Status: source implementation complete; Unity/device verification still required.**

Implemented in source:
- One runtime-generated arena.
- Human player + three bots.
- Responsive movement, deceleration, and dash.
- Tap Push and hold Pull.
- Magnetic crates and Flux Core.
- Player knockback, arena knockouts, safe respawn selection, and short respawn protection.
- Core possession, scoring, timer, winner, Overload, and rematch.
- Automatic Core recovery if the objective falls off the arena.
- Basic character motion, ownership/protection markers, camera, HUD, audio feedback, and mobile touch input.

Verification gate:
- Open successfully in Unity 6000.3.24f1 with zero C# errors.
- Run EditMode and PlayMode tests in Unity.
- Verify desktop controls and at least one physical phone.
- Record feel/tuning problems before locking balance values.

## Stage 2 — feel and balance

**Status: initial safety/readability pass implemented; subjective tuning remains blocked on playtest.**

Implemented before playtest:
- Safer bot edge steering.
- Safe respawn selection.
- Spawn protection.
- Tunable dash knockback resistance.
- Deterministic match clock/Overload model.
- Core recovery and data-driven carry/drop tuning.
- Clearer HUD and world state markers.
- Duplicate magnetic-force bug on Core carriers fixed.

Playtest work still required:
- Tune acceleration, deceleration, dash cadence, force curves, Flux costs, pickup rules, and Core break thresholds.
- Evaluate directional aim assistance versus omnidirectional field behavior.
- Improve hit/force feedback based on observed confusion.
- Test alternate arena geometry and edge-recovery options only if needed.
- Record observations in `docs/24_DECISIONS_AND_OPEN_QUESTIONS.md`.

Gate: four humans willingly rematch for roughly 15 minutes in an ugly build.

## Stage 3 — online multiplayer

Begin only after the local fun gate passes.

- Introduce a networking adapter abstraction.
- Server-authoritative match rules and critical physics.
- Client prediction for local movement; remote interpolation.
- Region-aware matchmaking and reconnect handling.
- Artificial latency/packet-loss tests.
- Anti-cheat validation for force, position, score, and cooldowns.

Gate: 8-player sessions remain understandable and responsive under realistic mobile latency.

## Stage 4 — product vertical slice

- Replace primitives with first-pass Mag character art.
- Production HUD, FTUE, results/rematch flow.
- 2–3 arenas and Core Rush + Knockout.
- Parties/private rooms.
- Account/profile persistence.
- Basic progression and cosmetic inventory.

## Stage 5 — alpha / soft launch

- Ranked ladder and MMR.
- Analytics/telemetry and remote tuning.
- Economy, cosmetic shop, season pass.
- Friend challenges, crews, moderation/reporting.
- Crash/performance/device matrix.
- Soft-launch KPIs: tutorial completion, D1/D7 retention, matches/session, rematch rate, rage-quit rate, queue time, payer conversion.

## Stage 6 — launch/live ops

- Additional modes (Magnetball, Team Core, Hot Core, King of the Platform).
- Seasonal content and events.
- Creator/share hooks, spectator/replay where justified.
- Continuous balance, anti-cheat, and performance work.

## Non-negotiable engineering rules

- Simulation is not presentation.
- Gameplay tuning is data-driven.
- No backend SDK calls inside core gameplay code.
- Network only authoritative gameplay objects; decorative debris stays local.
- No purchased competitive power.
- Avoid feature work that does not improve fun, readability, fairness, retention, or production feasibility.

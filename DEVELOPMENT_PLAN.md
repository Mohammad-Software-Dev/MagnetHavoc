# Magnet Havoc Development Plan

## Product goal

Ship a polished mobile competitive physics game whose first five seconds are understandable and whose first match creates a strong urge to rematch.

## Stage 0 — repository + technical baseline

**Status: complete in this branch.**

- Pin Unity 6.3 LTS.
- Establish runtime architecture and tuning model.
- Separate simulation, input, gameplay, and presentation.
- Add repository validation and Unity edit-mode tests.

## Stage 1 — local fun prototype

**Current stage.**

Exit criteria:
- One arena runs from a clean Unity project.
- Human player + three bots.
- Responsive movement and dash.
- Tap Push and hold Pull.
- Magnetic crates and a magnetic Flux Core.
- Player knockback and arena knockouts.
- Core possession, scoring, timer, winner, and restart.
- Basic animation, camera, VFX/audio feedback, and mobile touch input.
- Stable 60 FPS target on representative mid-range mobile hardware.

Do not add accounts, shops, crews, or networking until human playtests say this loop is fun.

## Stage 2 — feel and balance

- Tune acceleration, dash, force curves, Flux costs, pickup rules, and Core break thresholds.
- Improve aim assistance and target selection.
- Add clearer hit/force feedback.
- Test alternate arena geometry and safe recovery options.
- Record playtest observations in `docs/24_DECISIONS_AND_OPEN_QUESTIONS.md`.

Gate: four humans willingly rematch for at least 15 minutes in an ugly build.

## Stage 3 — online multiplayer

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

# Prototype Playtest Telemetry

Magnet Havoc records a small local match summary at the end of every completed prototype match. This is intentionally local-only: there is no analytics SDK, account identifier, network upload, advertising identifier, or external service dependency.

## Output

At runtime the game appends one JSON object per completed match to:

`Application.persistentDataPath/magnet_havoc_playtests.jsonl`

The Unity Console logs the resolved platform-specific path after a summary is written.

JSON Lines (`.jsonl`) is used so a playtest session can append matches safely and the resulting file can be processed by scripts, spreadsheets, notebooks, or a future telemetry uploader without changing the gameplay model.

## Match fields

Each line contains:

- `utc` — UTC timestamp at match completion.
- `appVersion` — Unity application version.
- `unityVersion` — editor/runtime version.
- `tuningVersion` — balance/config revision from `Resources/game_tuning.json`.
- `matchIndex` — sequential match number in the current app session.
- `durationSeconds` — actual elapsed play time, including Sudden Death.
- `winnerId` — winning prototype player ID.
- `suddenDeath` — whether the match ended after a regulation tie.
- `players` — per-player summaries.

Per-player fields:

- `playerId`
- `score`
- `pushes` — successful Push activations that consumed Flux; not hit count.
- `dashes`
- `corePickups`
- `coreDrops`
- `knockouts` — credited KOs caused by a recent magnetic hit.
- `eliminations` — times the player fell out / was eliminated.
- `possessionSeconds`
- `pullSeconds` — time spent sustaining a successful charged Pull state; not target-hit time.

## Knockout attribution

When a player receives magnetic force from another player, the instigator is remembered for `KnockoutCreditWindowSeconds`. If the victim is eliminated within that window, the recent instigator receives knockout credit. Falls without a valid recent instigator count only as an elimination for the victim.

This is a prototype attribution heuristic. Playtest whether the default window feels representative before treating KO statistics as competitive truth.

## Tuning workflow

Gameplay tuning is loaded from:

`Game/Assets/_MagnetHavoc/Resources/game_tuning.json`

Every meaningful balance revision must change `TuningVersion`. The runtime validates the configuration before using it; invalid JSON/config values fall back to compiled defaults.

When comparing playtests, group results by `tuningVersion`. Do not aggregate across incompatible force, movement, scoring, arena, or Core rules.

## Questions the current data can answer

- Are matches reaching the score target or timing out?
- How often is Sudden Death happening?
- Is one player/bot holding the Core for most of the match?
- Are Push, Pull, and Dash actually being used?
- Is knockout frequency high enough to create action without constant frustration?
- Does a tuning revision increase/decrease possession dominance or elimination rate?
- Are rematches producing different winners and strategies? (Session-level rematch intent still requires observation until explicit UX telemetry is added.)

## What this does not measure yet

- Push hits versus misses.
- Which object a Pull affected.
- Core steals distinguished from ordinary pickups.
- Cause of self-KOs versus environmental hazards.
- Frame-time percentiles or device thermals.
- Human identity or account data.
- Network latency, because online play is not implemented yet.

Add fields only when they answer a concrete design question. Avoid physics-tick event spam.

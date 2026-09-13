# Stage 2 Feel Pass

This pass improves failure recovery and match readability without locking subjective feel values before human playtesting.

## Implemented in source

- Flux Core automatically resets if it falls below the arena.
- Respawns choose the safest available spawn instead of always returning to one fixed corner.
- Short spawn protection prevents immediate magnetic chain-knockouts after respawn.
- Dash provides tunable resistance to magnetic knockback while active.
- Bots bias back toward the arena center near dangerous edges and avoid wasting dashes at the boundary.
- Core carrier/drop offsets and launch speed are data-driven.
- Held Core colliders no longer cause duplicate magnetic force on the carrier.
- HUD exposes dash readiness, spawn protection, Core ownership, and clearer scoreboard ownership markers.
- Player visuals expose spawn protection and Core ownership with simple prototype markers.
- Match timer / Overload timing is extracted into a Unity-independent deterministic model and tested in CI.
- Unity PlayMode regression tests are prepared for bootstrap, Core recovery, and rematch reset behavior.

## Still intentionally unresolved

These remain playtest questions, not design facts:

- Exact spawn-protection duration.
- Exact dash knockback resistance.
- Whether Push/Pull should remain omnidirectional or gain stronger directional targeting.
- Whether edge recovery is needed.
- Core carry movement penalty and break threshold.
- Bot difficulty and aggression.
- Final arena safe radius.

Do not move to production networking because these systems exist. Move to networking only after Unity/device playtests confirm the local loop is fun and understandable.

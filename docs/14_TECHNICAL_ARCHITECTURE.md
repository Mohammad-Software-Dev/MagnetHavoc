# Technical Architecture

## Preferred implementation direction

Recommended client engine: **Unity, using the latest stable LTS available when implementation begins, C#, and a mobile-appropriate render pipeline such as URP.** This is a preference, not a hard dependency if the implementation team has stronger expertise elsewhere.

Why Unity is a sensible default:
- Strong iOS/Android tooling.
- Mature 3D physics/animation pipeline.
- Broad networking/service ecosystem.
- Good profiling/device support.
- Large hiring/plugin ecosystem.

## Architectural principles

1. Gameplay rules are data-driven where practical.
2. Authoritative simulation logic is separated from presentation.
3. Networking is behind interfaces so provider changes do not infect gameplay code.
4. Cosmetics never alter authoritative combat stats.
5. Systems are testable without full online services.
6. Prototype code may be fast; production code must remove hidden dependencies before soft launch.

## Suggested client modules

- `Core` — boot, service locator/dependency injection, build config.
- `Input` — mobile/control abstraction.
- `Player` — movement, dash, state machine, visuals.
- `Magnet` — Push/Pull queries, force requests, Flux resource.
- `PhysicsGameplay` — mass classes, impacts, knockback, elimination.
- `Objective` — Core ownership/scoring/reset.
- `Modes` — rulesets and win conditions.
- `Arena` — spawn points, hazards, prop lifecycle.
- `Networking` — transport/session/snapshot adapters.
- `Backend` — auth/profile/inventory/rank/config adapters.
- `UI` — lobby, HUD, results, store.
- `Social` — party/friend abstractions.
- `Analytics` — typed event pipeline.
- `Audio` / `VFX` — presentation services.
- `Bots` — AI input generation.
- `Tests` — deterministic rules/unit/play tests.

## State model

Player high-level states:
- SpawnProtected.
- Active.
- Staggered.
- Tumbled.
- Eliminated.
- Respawning.

Avoid dozens of overlapping booleans. Use explicit state transitions plus orthogonal resources (Flux, dash cooldown, Core possession).

## Data-driven configuration

Use ScriptableObjects or equivalent authoring assets for:
- Movement tuning.
- Magnet tuning.
- Prop classes.
- Mode rules.
- Arena config.
- Cosmetic definitions.
- VFX/audio references.

Export or mirror critical server-authoritative values to a versioned shared config format so client/server cannot silently disagree.

## Performance targets

Primary target:
- 60 FPS on mid-range supported devices during ordinary play.
- Graceful 30 FPS quality tier on lower-end supported devices if necessary.

Budgets must be profiled on real devices.

Guidelines:
- Pool dynamic objects/VFX.
- Cap authoritative networked rigidbodies.
- Keep collision meshes simple.
- Avoid GC allocations in per-frame gameplay loops.
- Batch/static combine environment where appropriate.
- Quality tiers for shadows, post-processing, particles, resolution scale.

## Build environments

At minimum:
- Local/dev.
- Internal QA.
- Staging.
- Production.

Separate backend keys, analytics streams, and economy catalogs by environment.

## Save data

Server-authoritative:
- Account identity.
- Inventory/entitlements.
- Currencies.
- Rank/MMR where appropriate.
- Pass progression.
- Purchases.

Client-local/cache:
- Control layout.
- Graphics/audio settings.
- Tutorial hint state cache.
- Non-sensitive downloaded config cache.

Never trust local storage for paid entitlements or rank.

## Versioning

Every match session should know:
- Client build version.
- Content/config version.
- Protocol version.
- Playlist/ruleset version.

Incompatible clients should fail clearly before entering a match.

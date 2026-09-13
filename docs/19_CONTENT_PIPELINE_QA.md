# Content Pipeline, QA, and Release Discipline

## Content authoring

Prefer data/config assets over hardcoded scene logic.

Arena content should be assembled from validated prefabs/components:
- Spawn point.
- Core spawn.
- Kill volume.
- Magnetic prop spawn.
- Moving platform.
- Conveyor.
- Hazard controller.
- Cover blocker.
- Bot navigation hint.

Each component exposes only necessary tuning values.

## Arena validation tool

Create an editor validation pass that checks:
- Enough spawn points for mode.
- No spawn inside kill volume.
- Core spawn exists.
- Kill volumes are below/around playable geometry.
- Networked prop count below budget.
- Hazard IDs unique.
- Missing references.
- Camera bounds set.

## Cosmetic pipeline

Every cosmetic definition needs:
- Stable ID.
- Display localization key.
- Rarity/tag.
- Asset reference.
- Compatible rig/body slot.
- Thumbnail/preview data.
- Store availability metadata.

Automated validation should reject duplicate IDs and missing assets.

## Test layers

### Unit tests
- Flux spend/regeneration.
- Score rules.
- Match timer/overtime.
- Reward idempotency helper logic.
- Data parsing/versioning.

### Simulation tests
- Push/Pull force bounds.
- Core reset.
- Respawn safety scoring.
- Moving platform/hazard timing.

### Network tests
- Packet delay/loss simulation.
- Reconciliation under latency.
- Client disconnect/reconnect.
- Host/server failure behavior.
- Duplicate result submission.

### Device tests
- Low/mid/high Android target samples.
- Current supported iPhones/iPads.
- Thermal throttling over repeated matches.
- Background/resume.
- Network handoff where feasible.

## Regression checklist

Every release candidate:
- Tutorial complete.
- Queue works.
- Match start/end.
- Core scoring.
- Knockout/respawn.
- Rank/reward application.
- Party invite.
- Purchase flow in sandbox.
- Restore purchases where relevant.
- Reporting/blocking.
- Settings persist.
- Localization fallback.
- Analytics receives expected events.
- Crash reporting symbols uploaded.

## Performance QA

Measure:
- Frame-time percentiles, not only average FPS.
- Memory peak and leaks across 10+ matches.
- Network bandwidth per client.
- Server CPU per match.
- Number of networked rigidbodies.
- Physics step cost.
- VFX overdraw.

## Release safety

Use staged rollout where platform supports it. Maintain server compatibility window or explicit minimum-client gating. Every remote config event needs a fallback. Economy grants and purchases need auditability.

# MVP Roadmap and Acceptance Criteria

## Philosophy

Build risk in the correct order. The biggest risk is not the store, clan system, or cosmetics. The biggest risk is whether moving and magnetically throwing friends around is fun.

## Phase 0 — Greybox toy

Build locally with no networking.

Required:
- One capsule/robot controller.
- Movement.
- Dash.
- Tap Push.
- Hold Pull.
- Flux meter.
- One Core object.
- Light and medium physics props.
- Small greybox arena with edges.
- Local split testing or dummy targets/bots.

Exit criteria:
- Movement feels responsive.
- Push direction is readable.
- Pull feels controllable.
- Launching a prop into a target is satisfying.
- No major physics instability.

Do not build account systems yet.

## Phase 1 — Local Core Rush

Add:
- Core possession/scoring.
- Respawn.
- Match timer/win state.
- Basic bot.
- One arena hazard.
- HUD.

Exit criteria:
- New tester understands objective quickly.
- 4-player-equivalent sessions create repeated contests.
- Testers ask for rematches.

## Phase 2 — Online 4-player prototype

Add:
- Authoritative server/session.
- Prediction/interpolation.
- 4 remote players.
- Matchmaking/dev lobby.
- Server-owned Core and critical props.
- Basic reconnect.
- Telemetry.

Exit criteria:
- Playable at realistic regional latency.
- No frequent divergent physics outcomes.
- Local movement remains responsive.
- Match result cannot be forged by client.

## Phase 3 — 8-player vertical slice

Add:
- 8 players.
- Production-quality Skyforge map.
- First polished Mag character.
- Core VFX/audio.
- Result/rematch flow.
- Friends/party minimal flow.
- 3–5 cosmetics.
- Device quality tiers.

Exit criteria:
- Stable performance on target mid-range devices.
- User test confirms visual clarity.
- Social rematches work reliably.
- Crash/network failure rates acceptable for closed test.

## Phase 4 — Closed alpha

Add:
- Account/profile persistence.
- Basic rank/MMR.
- Inventory.
- Store sandbox.
- Second map.
- Reporting/blocking.
- Remote config.
- Operational dashboards.

Goals:
- Retention measurement.
- Queue/match quality.
- Economy test without aggressive monetization.

## Phase 5 — Soft launch candidate

Add only after metrics justify:
- Havoc Pass.
- More cosmetics.
- Additional casual mode.
- Crew prototype if social metrics support it.
- Localization.
- Purchase validation.
- Full support/ops processes.

## Feature priority rubric

When deciding whether to build something, score it against:
1. Improves core fun?
2. Improves clarity?
3. Improves social replay?
4. Improves retention without coercion?
5. Reuses existing systems?
6. Can be measured?
7. Does not create disproportionate production/ops cost?

Low-scoring features go to backlog.

## Definition of “MVP”

MVP is **not** a commercially complete game. It is the smallest online product capable of proving that real players enjoy the core magnetic competition enough to return and rematch.

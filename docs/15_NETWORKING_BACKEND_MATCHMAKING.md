# Networking, Backend, and Matchmaking

## Networking philosophy

Ranked multiplayer must be **server authoritative** for movement validation, objective ownership, scoring, eliminations, and gameplay-critical physics. Client responsiveness is maintained through prediction and interpolation, not by trusting the client.

## Simulation target

Starting technical target:
- Server simulation: ~30 Hz.
- Client render: 60 FPS where device permits.
- Snapshot/interpolation cadence can be lower than render rate if quality remains good.

Final values depend on provider, physics implementation, bandwidth, and testing.

## Authority split

Server owns:
- Player canonical position/velocity.
- Dash eligibility/cooldown.
- Flux usage.
- Push/Pull hit validation.
- Core state and score.
- Critical props.
- Hazards affecting outcomes.
- Knockouts/respawns.
- Match timer/winner.

Client owns presentation:
- Camera.
- Local input sampling.
- Predicted local movement.
- Cosmetic-only VFX/debris.
- UI animation.
- Non-authoritative hit emphasis.

## Physics networking

Do not attempt to synchronize unlimited free rigidbodies.

Classify objects:
- Critical networked rigidbodies: Core + intentionally limited props.
- Constrained networked movers: platforms/hazards with deterministic paths.
- Decorative client-local physics: fragments/debris.

Suggested cap for early arena: roughly 8–12 free networked gameplay props, then profile.

## Client prediction

Local player movement and dash should be predicted immediately. Server corrections should be reconciled smoothly except when large cheating/latency divergence requires a visible correction.

Push/Pull presentation can start instantly, while authoritative results arrive from server.

## Lag handling

- Region selection prioritizes latency.
- Show connection quality when materially poor.
- Do not grant large lag-compensation advantages for knockback.
- For ambiguous contests over the Core, server timestamp/order is authoritative.
- Reconnect to an active match when feasible within a short window.

## Matchmaking inputs

Primary:
- Latency/region.
- Hidden MMR.
- Playlist.
- Party size.
- Client/protocol compatibility.

Secondary:
- Queue time expansion.
- New-player protection.

Never use spending as a matchmaking variable.

## Bot fill policy

Casual queues may use bots to reduce wait times, especially for new users or low population. Ranked bot policy should be conservative and transparent in design. See `17_AI_BOTS_FAIR_PLAY_SAFETY.md`.

## Backend service boundaries

Define provider-independent interfaces for:
- Authentication.
- Player profile.
- Friends/party integration.
- Matchmaking/lobbies.
- Dedicated session allocation if used.
- Inventory/economy.
- Leaderboards/rank.
- Remote config.
- Cloud save.
- Analytics.
- Crash reporting.

Potential providers can be selected later. Do not couple gameplay domain objects directly to SDK types.

## Match lifecycle

1. Client authenticates.
2. Client checks config/content version.
3. Player/party selects playlist.
4. Matchmaker groups compatible players.
5. Session allocation returns endpoint/token.
6. Clients connect and complete ready handshake.
7. Server loads authoritative ruleset/arena seed.
8. Match runs.
9. Server finalizes signed result.
10. Backend applies rank/rewards idempotently.
11. Clients receive result and rematch options.

## Idempotency

Reward and purchase operations need idempotency keys. Reconnecting or repeating a request must not duplicate rewards.

## Security basics

- TLS for service traffic.
- Short-lived session tokens.
- Server validation of purchases.
- No secret keys in client build.
- Rate limiting for sensitive endpoints.
- Input sanity checks.
- Audit logs for economy/rank mutations.

## Service degradation

If social/store services are degraded but gameplay is healthy, allow appropriate unaffected modes. If authoritative match result storage is unavailable, do not pretend ranked rewards were committed; queue/retry safely or disable affected playlist.

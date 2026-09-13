# Bots, Fair Play, Anti-Cheat, and Player Safety

## Bots

Bots serve three purposes:
- Tutorial opponents.
- Offline/internal testing.
- Casual queue fill when population is insufficient.

Bots should not be used to secretly manipulate outcomes or simulate fake social relationships.

## Bot behavior layers

Perception:
- Core position/holder.
- Nearby rivals.
- Edges/hazards.
- Useful props.
- Safe routes.

Decision:
- Contest Core.
- Pursue leader.
- Retreat from edge.
- Use Pull on objective/prop.
- Push when predicted value exceeds threshold.
- Dash for escape/intercept.

Execution:
- Convert decision into the same input abstraction used by human players where possible.

## Difficulty

Do not create difficulty by giving bots impossible reaction times or hidden information. Scale:
- Reaction delay.
- Prediction accuracy.
- Risk tolerance.
- Target selection quality.
- Movement precision.

## Bot disclosure

Use internally clear bot identifiers. Product presentation should not intentionally deceive users into believing bots are real humans. Exact UI treatment can be decided later.

## Anti-cheat

Server-authoritative checks:
- Maximum acceleration/speed.
- Dash cooldown.
- Flux costs/regeneration.
- Push/Pull rate and range.
- Impossible position changes.
- Core ownership.
- Reward/rank writes.

Additional measures:
- Signed session tokens.
- Build/protocol checks.
- Rate limits.
- Anomaly detection.
- Purchase receipt validation.

Do not make client obfuscation the primary defense.

## AFK/leavers

Casual:
- Reconnect window.
- Bot takeover may be considered after timeout.

Ranked:
- Reconnect window.
- Repeated intentional leaving can trigger queue cooldowns.
- Avoid punishing server-caused disconnects when detectable.

## Reporting

Initial categories:
- Cheating.
- Offensive name.
- Harassment through available communication.
- Intentional team griefing.

Provide mute/block immediately where applicable.

## Player-name safety

- Profanity filtering.
- Length/character limits.
- Report flow.
- Reserved impersonation terms.
- Server-side enforcement.

## Age and communication

Because the game may appeal to younger players, default to conservative communication features. Open voice/text should not be added casually; they require privacy, moderation, parental-control, and regional compliance review.

## Competitive integrity

- No paid combat stats.
- No hidden stat boosts based on loss streaks.
- No fabricated ranked opponents presented as humans.
- Clear season/rank rules.
- Consistent authoritative physics within a ranked playlist.

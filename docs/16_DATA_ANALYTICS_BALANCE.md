# Data, Analytics, KPIs, and Balance

## Principle

Telemetry should answer design questions, not merely generate dashboards. Every event must have a reason to exist.

## Core funnel events

- `app_open`
- `install_attribution_available`
- `ftue_start`
- `ftue_step_complete`
- `ftue_complete`
- `lobby_view`
- `queue_start`
- `queue_cancel`
- `match_found`
- `match_start`
- `match_end`
- `result_view`
- `rematch_offer`
- `rematch_accept`
- `session_end`

## Match summary telemetry

Prefer summary counters over event spam for every physics tick.

Per player/match:
- Mode/map/version.
- Placement/team result.
- Score.
- Core possession seconds.
- Core steals.
- Push attempts/hits.
- Pull active seconds / successful acquisitions.
- Prop hits caused.
- Knockouts caused.
- Falls/self-KOs.
- Times eliminated.
- Dash count.
- Average/95th percentile ping.
- Disconnect/reconnect.
- Device performance bucket.

## Social/economy events

- Friend request sent/accepted.
- Party invite sent/accepted.
- Match with friend.
- Crew join/leave.
- Store impression.
- Cosmetic preview/equip.
- Purchase initiated/completed/failed.
- Pass progression.
- Currency earn/spend with reason code.

## Prototype success metrics

Before commercial KPIs, measure fun:
- Rematch acceptance after close matches.
- Matches per playtest session.
- Voluntary session continuation after first match.
- Frequency of vocal/laughter reactions in observed tests.
- Player comprehension of Push/Pull without explanation.

## Soft-launch KPI hypotheses

These are directional goals, not guarantees:
- D1 retention: aim for 35%+ as an early healthy signal.
- D7: aim for 10–15%+ depending on acquisition mix/market.
- D30: aim for 4–7%+.
- Matches per active day: 5+ desirable.
- Median queue time: keep short enough that users do not abandon; target depends on region/population.
- Crash-free sessions: 99%+ target, improving toward higher production standards.

Do not optimize one KPI while damaging trust or match quality.

## Balance dashboards

Track by mode/map/MMR cohort:
- Win/placement distribution.
- Spawn advantage.
- Core first-capture vs final win correlation.
- Knockout frequency.
- Average possession streak.
- Match duration.
- Comeback rate.
- Overtime frequency.
- Prop usage effectiveness.
- Gadget usage/win rate if gadgets are added.

## Experimentation

A/B tests can cover:
- FTUE sequence.
- UI layout.
- Result-screen rematch prominence.
- Reward pacing.
- Casual mode rotation.

Avoid A/B testing core ranked physics across players in the same competitive population unless isolated by version/queue, because differing physical rules destroy fairness.

## Tuning workflow

1. State hypothesis.
2. Identify metric and qualitative evidence.
3. Change one primary variable or a coherent bundle.
4. Run internal playtest.
5. Run controlled external cohort if appropriate.
6. Review both telemetry and player feedback.
7. Document decision in `24_DECISIONS_AND_OPEN_QUESTIONS.md`.

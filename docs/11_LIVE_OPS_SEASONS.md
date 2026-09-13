# Live Operations and Seasons

## Purpose

Live operations should refresh reasons to play without constantly rewriting the game. Core movement and magnet mechanics remain stable; seasons rotate goals, cosmetics, map variants, and limited modes.

## Season cadence

Starting recommendation: 6–8 week seasons.

A season can contain:
- Ranked reset/placement period.
- Havoc Pass.
- Cosmetic theme.
- One arena variant or new arena when production allows.
- One temporary rule modifier or mode.
- Weekly challenges.
- Crew event.

## Weekly cadence example

Week 1: season launch + ranked placement.
Week 2: new cosmetic set + casual modifier.
Week 3: Crew challenge.
Week 4: Magnetball weekend.
Week 5: map hazard remix.
Week 6: final ranked push + double cosmetic XP event.

Do not overload every week with bespoke assets.

## Remote configuration

The following should be remotely configurable where safe:
- Mode rotation.
- Score targets.
- Match timer.
- Prop spawn weights.
- Challenge definitions.
- Reward amounts.
- Store rotations.
- Event start/end times.
- Matchmaking thresholds within bounded limits.

Physics fundamentals should not change mid-ranked-season without clear patch communication.

## Event design

Good events remix familiar systems:
- Infinite Flux casual weekend.
- Heavy Props mode.
- Low Gravity Knockout.
- Tiny Arena.
- Double Core.
- Crew Core Relay.

Avoid events requiring entirely separate control schemes unless they graduate into permanent modes.

## Content calendar fields

Maintain data for:
- Event ID.
- Start/end UTC.
- Eligible regions/build versions.
- Mode/config reference.
- Reward table.
- Store assets.
- Localization keys.
- QA status.
- Rollback plan.

## Operational safety

Every live event should have:
- Kill switch.
- Version compatibility rules.
- Fallback default playlist.
- Server-side validation of reward claims.
- Monitoring dashboard.

## Patch philosophy

Balance patches should explain intent. Avoid changing multiple core systems simultaneously unless necessary, because it becomes difficult to attribute retention/match-quality effects.

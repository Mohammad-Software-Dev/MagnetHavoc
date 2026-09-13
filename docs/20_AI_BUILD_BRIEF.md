# AI Build Brief — Read This Before Implementing

## Mission

Build **Magnet Havoc**, a landscape mobile 3D competitive physics arena game. The first objective is not a commercial live service. It is a fun, robust playable prototype proving that movement + magnetic Push/Pull + short objective competition creates repeated rematches.

## Source-of-truth priority

If documents appear to conflict, use this priority:
1. `24_DECISIONS_AND_OPEN_QUESTIONS.md`
2. `01_GAME_VISION.md`
3. `02_CORE_GAMEPLAY.md`
4. System-specific specification file.
5. Example numeric configs.

Numeric values are hypotheses unless explicitly marked locked.

## Do not overbuild

Before implementing accounts, store, battle pass, crews, or advanced cosmetics, produce a playable local build containing:
- One arena.
- One player body.
- Movement.
- Dash.
- Tap Push.
- Hold Pull.
- Flux resource.
- Core objective.
- A few magnetic props.
- Knockout and respawn.
- Match timer and winner.

Then add networking.

## Preferred stack

Default recommendation:
- Unity latest stable LTS at implementation time.
- C#.
- Mobile-friendly render pipeline.
- Server-authoritative multiplayer provider selected through an adapter/interface layer.

Do not hard-wire gameplay code to a specific backend SDK.

## Coding rules

- Separate simulation from presentation.
- Use explicit state machines for player/match states.
- Put tuning values in data/config assets.
- No magic numbers in core gameplay code when a named config field is appropriate.
- Keep authoritative server validation for all competitive outcomes.
- Pool transient VFX/props where appropriate.
- Keep gameplay collision simple.
- Write tests for score, Flux, cooldowns, state transitions, and result application.
- Log failures with actionable context; avoid silent catches.

## Prototype scene acceptance

A developer must be able to launch one scene and immediately:
1. Move with keyboard/gamepad in editor and mobile joystick on device.
2. Tap/press to Push.
3. Hold to Pull.
4. Pull a crate close.
5. Push-launch it.
6. Affect another player/bot.
7. Capture the Core.
8. Score while holding it.
9. Fall off and respawn.
10. Finish a timed match.

## Controls

Landscape mobile:
- Left virtual joystick = move.
- Magnet button tap = Push.
- Magnet button hold = Pull.
- Dash button = directional dash.
- No jump button in MVP.

Editor fallback:
- WASD movement.
- Mouse/keyboard bindings clearly documented.

## Networking requirements

When online implementation begins:
- Authoritative server owns match rules and critical physics.
- Local movement uses client prediction.
- Remote entities interpolate.
- Decorative debris remains local-only.
- Limit free networked rigidbodies.
- Test with artificial latency and packet loss.

## UX requirements

- Core holder unmistakable.
- Push/Pull feedback immediate.
- Result screen appears quickly.
- Rematch is prominent.
- No store surfaces in the critical first prototype loop.

## Performance requirements

- Target 60 FPS on representative mid-range phone.
- Avoid per-frame allocations in hot gameplay paths.
- Provide quality scaling.
- Profile physics and VFX on device.

## What not to invent without updating docs

Do not casually add:
- HP/damage combat.
- Weapons/guns.
- Character classes with unique stats.
- Manual jump.
- Paid power.
- Long battle royale maps.
- Crafting/equipment stats.
- More than the necessary control buttons.

If a change seems necessary, record the rationale in `24_DECISIONS_AND_OPEN_QUESTIONS.md` and ensure it supports the product pillars.

## Definition of success for the AI-built first version

A human tester should be able to play a short match, understand why they won/lost, experience at least one satisfying magnetic interaction, and want to restart quickly. Visual polish is secondary to control feel and understandable physics.

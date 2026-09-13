# Controls, Camera, and Game Feel

## Control philosophy

The game must be playable comfortably on a phone with two thumbs. Controls should support mastery without requiring shooter-level precision.

### Left thumb — movement joystick
- Floating or fixed virtual joystick; test both.
- 360-degree movement.
- Short acceleration curve rather than instant maximum speed.
- Strong deceleration when releasing input so positioning remains controllable.
- Character faces movement direction unless actively magnet-targeting.

### Right thumb — Magnet button
One button exposes both polarities:
- **Tap:** Push / Repel Pulse.
- **Hold:** Pull / Attraction Field.
- Visual state changes immediately so the player knows whether the game interpreted a tap or hold.
- Hold threshold starting hypothesis: 160–200 ms.

This must be tested heavily. If tap-vs-hold causes misfires, split Push and Pull into two adjacent buttons before adding complexity elsewhere.

### Right thumb — Dash button
- Directional dash based on movement input.
- If no movement input exists, dash in facing direction.
- Cooldown starting hypothesis: 2.5 seconds.
- Dash is mobility, not a damaging attack by default.
- Very short knockback resistance window may be tested, but no long invulnerability.

### No mandatory jump button in MVP
Vertical traversal comes from ramps, bounce surfaces, moving platforms, and knockback. Removing manual jump frees the right thumb and reduces accidental inputs. If prototype testing proves jump is essential, add it deliberately rather than by default.

## Camera

Recommended view: elevated third-person / isometric chase camera.
- Landscape orientation.
- Player occupies roughly lower-middle screen region.
- Camera rotates minimally or not at all in initial prototype; stable orientation reduces cognitive load.
- Dynamic zoom can widen slightly when several threats approach or player speed spikes.
- Camera must never hide an arena edge directly behind UI.
- Offscreen Core indicator appears when necessary.

### Camera priorities
1. Local player readability.
2. Core location.
3. Nearest dangerous rivals/props.
4. Arena edge awareness.
5. Spectacle.

Spectacle must never defeat gameplay readability.

## Target movement feel

The avatar should feel like a compact athletic machine, not a frictionless puck.
- Responsive acceleration.
- Small body lean into turns.
- Foot/hover effects sell speed.
- Dash has anticipatory squash/charge of ~80–120 ms, then fast impulse.
- Impacts temporarily exaggerate body pose while keeping recovery short.

Starting movement hypotheses:
- Base speed: tune around crossing the main arena in 6–8 seconds.
- Acceleration to near-full speed: ~0.25–0.4s.
- Turning responsiveness: high at low speed, slightly weighted at high speed.
- Dash distance: about 2–2.5 player body lengths.

## Haptics

Use haptics sparingly and meaningfully:
- Light tick when Pull acquires a valid target.
- Medium pulse when a Push connects.
- Stronger pulse on a knockout caused by the player.
- Short celebratory pattern on victory.
- Respect device settings and provide a toggle.

## Hit stop and time effects

Very brief local hit-stop or animation freeze can sell a major impact, but multiplayer simulation must not pause. Use rendering/audio tricks only. Major knockout impacts can use 40–70 ms visual emphasis without affecting authoritative simulation.

## Targeting assistance

The game should feel skillful but mobile-friendly.
- Pull field uses a broad forward cone with soft target weighting.
- Core receives slight priority over ordinary props when near the center of the cone.
- Do not auto-target players through walls.
- Push is primarily area/arc based rather than lock-on.
- Aim direction comes from facing/movement unless a later control scheme introduces right-stick aim.

## Edge recovery

Falling should feel dramatic but not cheap.
- Small ledge lips may prevent accidental micro-slips.
- Dash can rescue near-edge situations if used before crossing the kill plane.
- Some maps can include bounce nets or magnetic rails in casual modes.
- Ranked maps should have consistent, learnable boundaries.

## Game-feel acceptance criteria

Before online networking is treated as complete, local play should satisfy:
- Input-to-visible-response feels immediate at 60 FPS.
- Dash direction matches player expectation at least 95% of the time in internal testing.
- Push impact direction is obvious from animation/VFX.
- Pull acquisition is visually unambiguous.
- Recovery from ordinary hits rarely removes control for more than ~0.8s.
- Players can distinguish player bodies, Core, dangerous props, and hazards at phone screen size.

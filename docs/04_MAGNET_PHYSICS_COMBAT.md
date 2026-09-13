# Magnet, Physics, and Combat System

## Design goal

Magnetism is both the combat language and the toy. It should create predictable cause-and-effect with enough physical variation to produce surprising stories.

## Flux resource

Players use a regenerating **Flux meter** to prevent endless spam.

Starting hypothesis:
- Capacity: 100.
- Push cost: 25.
- Pull drain: 18 per second.
- Regeneration: 25 per second after a 0.6s delay from last magnetic use.
- Dash does not consume Flux in MVP; it uses its own cooldown.

Display Flux as a compact ring or bar near the magnet button, not a large HUD element.

## Push

Tap Magnet to emit a short repulsion pulse.

Initial properties:
- Forward-biased arc or short-radius hemisphere.
- Effective range: roughly 3–4 meters in game scale.
- Strongest near the player; falls off with distance.
- Affects magnetic props, Core, and players at different multipliers.
- Brief cooldown prevents animation/input ambiguity even if Flux remains.

Suggested force priority:
1. Core — highly responsive, readable.
2. Light props — dramatic travel.
3. Medium props — useful but slower.
4. Players — meaningful displacement, lower than loose objects.
5. Heavy props — barely move or require repeated force.

## Pull

Hold Magnet to project an attraction field.
- Cone/range starting point: about 6 meters.
- Field ramps over ~0.15s to avoid jarring target changes.
- Pull line/particles show active influence.
- Multiple eligible objects can be affected, but assistance weighting should make the intended target legible.
- Walls block influence unless a map element is explicitly designed to transmit it.

### Mag-lock
A light/medium prop pulled very close can enter a short **mag-lock** position in front of the player. A subsequent Push launches it. This is the simplest form of improvised weapon handling.

Constraints:
- Only one locked prop at a time.
- Locking a prop slightly slows movement.
- Heavy props cannot lock.
- Network state is explicit: free, influenced, locked, launched.

## Core behavior

The Flux Core has special rules:
- Highest magnetic responsiveness.
- When captured, it becomes possession state rather than ordinary rigidbody carry.
- Possession can be broken by knockback above a threshold.
- Core cannot be mag-locked and launched as a lethal weapon in MVP.
- It uses an obvious glow/trail and an offscreen arrow.

## Player-to-player magnetic force

Players contain metal, so they respond to Pull and Push but not identically to props.
- Player force multiplier starts around 0.5–0.65 of a light prop.
- Opponent control should bend rather than vanish under weak Pull.
- Strong external velocity can cause stagger/tumble.
- Repeated hits should not create infinite stun chains; use diminishing crowd-control resistance or short post-recovery resistance.

## Impact and knockback model

No health bar is required for base modes. Calculate impact outcome from:
- Source force.
- Relative velocity.
- Target mass class.
- Whether target is grounded/airborne.
- Surface friction and slope.
- Temporary resistance states.

Outcome bands:
- **Nudge:** visual response, no control loss.
- **Hit:** meaningful displacement, tiny reaction animation.
- **Stagger:** stronger movement interruption, ~0.25–0.45s.
- **Tumble:** dramatic impact, ~0.55–0.9s before useful control returns.
- **Knockout:** target crosses arena elimination volume or hits a designated knockout hazard.

## Physics object classes

### Light
Cans, balls, small crates, panels. Fast, throwable, plentiful.

### Medium
Large crates, bumpers, battery packs. Slower but strong on impact.

### Heavy
Machines, gates, anchored obstacles. Usually move only along constrained paths.

### Decorative
Bolts, fragments, sparks. Client-local only; never influence authoritative outcomes.

## Predictability rules

- Critical gameplay objects use stable mass/drag values from data assets.
- Do not allow decorative debris to change player movement.
- Networked physics count per arena should be intentionally capped.
- Major collision shapes stay simple.
- Arena geometry should avoid tiny snag points.
- Effects may exaggerate impacts visually, but authoritative motion remains learnable.

## Friendly interaction

Team modes need reduced frustration:
- Teammate Push force: 20–35% of enemy force or disabled in ranked team modes.
- Teammate Pull can assist but should not allow griefing into hazards.
- Core interaction remains shared by team.

## Combo opportunities

The system should naturally support:
- Pull prop → Push prop into rival.
- Push rival → hazard.
- Pull Core → dash away.
- Pull rival slightly → their own momentum carries them off route.
- Push Core out of leader possession → teammate captures.
- Dash behind cover → rival Push hits prop instead.

## Tuning principle

If a magnet interaction looks fun but feels random, improve telegraphing and physical constraints before reducing spectacle. The ideal is **surprising outcome, understandable cause**.

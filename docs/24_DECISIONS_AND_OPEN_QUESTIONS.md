# Decisions and Open Questions

This document records what is currently decided versus intentionally unresolved. Update it whenever a major design or technical decision changes.

## Current decisions

### Product
- Name: **Magnet Havoc** (working name; legal clearance pending).
- Genre: competitive mobile physics-action arena / party brawler.
- Platform: iOS + Android first.
- Orientation: landscape.
- Match size target: 8 players; prototype begins with 4.
- Primary mode: Core Rush.
- Match target: roughly 90–150 seconds.
- Main fantasy: manipulate magnetic forces, objectives, props, and rivals.
- Combat model: positional/knockback; no base HP system.
- Monetization: cosmetic-first; no paid ranked power.

### Controls
- Left joystick: movement.
- Magnet tap: Push.
- Magnet hold: Pull.
- Separate Dash button.
- No manual Jump button in MVP.

### World
- Futuristic sport in Gridspire.
- Competitors called Mags.
- Central objective called Flux Core.
- League called Havoc League.

### Technical
- Preferred engine direction: Unity 6.3 LTS for the current production baseline.
- Ranked simulation server-authoritative.
- Client prediction for local movement.
- Critical physics objects capped and networked; decorative debris local-only.
- Simulation, gameplay, input, and presentation remain separated.
- Core match timing/Overload, Flux, and scoring rules have Unity-independent models for automated testing.
- Prototype tuning revisions are tagged with `TuningVersion` and match summaries retain that version for comparison.

### Prototype safety/recovery choices
These are implemented as tunable hypotheses, not final balance:
- Flux Core automatically resets if it falls below the arena.
- Respawns choose a safer spawn rather than always returning to a fixed point.
- Respawns receive a short protection window against magnetic impulses.
- Dash can reduce magnetic knockback while active.
- Bots bias toward arena center before reaching dangerous edges.
- A held Core should not cause duplicate magnetic force on its carrier.

## Open questions to resolve through prototyping

### Control scheme
- Does tap-vs-hold reliably distinguish Push and Pull under stress?
- Would two polarity buttons improve mastery enough to justify extra UI?
- Should magnetism remain broadly omnidirectional or gain directional targeting/aim assist?

### Movement
- Is no-jump design sufficiently expressive?
- Is current dash knockback resistance helpful or frustrating?
- Should players have an edge-grab/recovery mechanic?
- Does stronger release deceleration improve mobile precision?

### Core Rush
- Exact score rate and target.
- Does Core Rush need an explicit score target at all, or should the timer be the primary match end condition?
- How much movement penalty should Core possession apply?
- What force threshold breaks possession?
- Best Overload trigger.
- Is automatic proximity pickup sufficiently readable and fair?

### Headless scoring signal — not a final balance decision
A deterministic CI stress harness ran 5,000 symmetric four-player Core Rush matches using tuning `prototype-2026-09-13-a`.

Results:
- Player-ID win shares: 23.96%–26.02%.
- No match termination failures.
- Average duration: 120.0 seconds; P95: 120.0 seconds.
- Sudden Death: 0.2%.
- Score-target finishes: **0/5,000** at the current 100-point target.
- Average winner score: 42.7.
- Average credited knockouts: 9.76 per match under the simulator's abstract pressure model.

This harness deliberately abstracts away real Unity physics, positioning, map geometry, touch control quality, bot tactics, and human skill. It is evidence that **100 points is still an unvalidated hypothesis**, not evidence that the correct target is 43. During real Unity/human playtesting, record winner scores and whether early target finishes improve the experience. Valid outcomes include lowering the target, increasing score rate, timer-only scoring, or retaining 100 as a rare blowout/mercy condition.

### Physics
- Best player magnetic force multiplier.
- Whether mag-locking props is essential or can be simplified.
- Maximum useful networked prop count.
- Ragdoll versus authored tumble balance.
- Whether Push/Pull should be blocked by level geometry or intentionally affect through thin cover.

### Camera
- Fully fixed world orientation versus mild auto-rotation.
- Dynamic zoom strength.

### Ranked
- FFA Core Rush versus team-first ranked launch.
- Exact season length.
- Rating model.

### Business
- Whether rewarded ads are needed at all.
- Pass cadence and pricing.
- Store rotation cadence.

### Social
- Crew launch timing.
- Whether private matches ship before ranked.
- Spectator/replay priority.

## Decision rule

Prototype uncertain items cheaply. Do not resolve questions by adding systems. Prefer the simplest choice that preserves fun, clarity, fairness, and production feasibility. A green CI build is not evidence that a physics value feels good; subjective values only become decisions after playtest evidence.

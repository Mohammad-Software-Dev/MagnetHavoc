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
- Preferred engine direction: Unity latest stable LTS at implementation time.
- Ranked simulation server-authoritative.
- Client prediction for local movement.
- Critical physics objects capped and networked; decorative debris local-only.

## Open questions to resolve through prototyping

### Control scheme
- Does tap-vs-hold reliably distinguish Push and Pull under stress?
- Would two polarity buttons improve mastery enough to justify extra UI?
- Should facing follow movement only, or should magnet use introduce aim assist/secondary aim?

### Movement
- Is no-jump design sufficiently expressive?
- Does dash need brief knockback resistance?
- Should players have an edge-grab recovery mechanic?

### Core Rush
- Exact score rate and target.
- How much movement penalty should Core possession apply?
- What force threshold breaks possession?
- Best Overload trigger.

### Physics
- Best player magnetic force multiplier.
- Whether mag-locking props is essential or can be simplified.
- Maximum useful networked prop count.
- Ragdoll versus authored tumble balance.

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

Prototype uncertain items cheaply. Do not resolve questions by adding systems. Prefer the simplest choice that preserves fun, clarity, fairness, and production feasibility.

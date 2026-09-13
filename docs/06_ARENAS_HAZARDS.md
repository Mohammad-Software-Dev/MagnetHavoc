# Arenas and Hazards

## Level design goals

A Magnet Havoc arena is a compact sports stage, not an exploration map. Every route should create interaction within seconds.

Core principles:
- Core and rivals are easy to locate.
- Edges create tension but do not dominate every encounter.
- Props create tactical opportunities.
- Cover interrupts Push/Pull sightlines.
- Multiple routes prevent one defensible corner from becoming unbeatable.
- Hazards are telegraphed and learnable.
- Spawn locations are safe enough to re-enter play.

## Recommended scale

Target crossing time from one side of the main playable area to the other: approximately 6–8 seconds at base run speed. This keeps players connected to the action.

## Standard arena components

### Central objective zone
Open enough for contesting, with 2–4 exit routes.

### Edge risk zones
Areas where a well-timed Push can produce a knockout. Avoid long stretches where any minor nudge kills.

### Cover
Low walls, energy shields, machinery, or pillars that block magnetic influence and create positioning decisions.

### Props
A deliberate set of light and medium magnetic objects. Their spawn points and reset behavior must be predictable.

### Movement features
Ramps, conveyors, bounce pads, rotating platforms, lifts, rails.

### Hazards
Telegraphed machinery, shock fields, crushers, vents, or moving walls. Most hazards should displace rather than instantly eliminate.

## Hazard telegraphing

Every active hazard should have:
- Pre-activation visual cue.
- Audio cue.
- Consistent timing.
- Distinct danger color/material language.
- Clear recovery or avoidance route.

Avoid surprise damage from offscreen events.

## Launch arena concepts

### 1. Skyforge Station — first production map
A bright floating industrial platform built around a central Core pedestal.

Layout:
- Circular/hexagonal central platform.
- Four side wings connected by short bridges.
- Two moving conveyor lanes.
- Four light-crate spawn points.
- Two medium bumpers on rails.
- Outer fall zones.

Overload event:
- Side conveyors accelerate and two bridge sections slide inward/outward.

Purpose:
- Cleanest teaching arena.
- Symmetrical enough for balance.
- Great prototype map.

### 2. Scrapworks Yard
Denser map with stacked junk, cranes, and magnetic cargo.

Signature interactions:
- Hanging magnet crane periodically moves a heavy object.
- More throwable props.
- Fewer lethal edges; more collision chaos.

Overload:
- Crusher lanes activate on a predictable cycle.

### 3. Flux Foundry
A high-tech energy plant with polarity rails.

Signature interactions:
- Certain floor rails periodically Pull or Push magnetic objects.
- Players can intentionally use rail timing to accelerate the Core or a prop.

Overload:
- One polarity rail activates at a time, signaled several seconds early.

### 4. Neon Rooftops — later
Connected rooftop platforms, bounce billboards, wind vents, and narrow traversal lanes. More advanced and less suitable for first-time players.

## Spawn rules

- Never spawn within immediate Push range of a rival if alternatives exist.
- Use multiple spawn nodes and score them for danger.
- 1.5s spawn protection starting hypothesis.
- Spawn protection ends early if the player uses Push/Pull or captures the Core.
- Camera must face toward the main action on respawn.

## Anti-camping rules

If a location allows a Core holder to defend indefinitely:
- Add a second entry angle.
- Reduce cover.
- Add a temporary environmental pressure.
- Make Core possession increase local visual signature.
- Prefer map fixes over arbitrary debuffs.

## Map data

Each arena should define data-driven:
- Spawn points.
- Core spawn/reset point.
- Kill volumes.
- Prop spawn tables.
- Hazard sequences.
- Overload event options.
- Camera bounds.
- Navigation markers for bots.

## Level review checklist

- Can a new player identify the center in two seconds?
- Can the Core reach every safe zone?
- Is there at least one escape route from each major pocket?
- Do props create choices rather than clutter?
- Are dangerous edges visually distinct?
- Are spawn-to-action times short?
- Does the map remain readable on a small phone?
- Are networked dynamic objects intentionally limited?

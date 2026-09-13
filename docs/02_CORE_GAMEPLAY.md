# Core Gameplay

## Primary mode: Core Rush

Core Rush is the launch-defining mode. Eight players enter a compact arena containing a highly magnetic **Flux Core**. Holding the Core generates score over time. Players use movement, dash, Pull, and Push to take possession, protect themselves, disrupt rivals, and manipulate metal props. First to the score target wins; if nobody reaches it before the match timer expires, the highest score wins.

Recommended initial tuning hypothesis:
- Players: 8 in full version; prototype begins at 4.
- Match limit: 120 seconds.
- Win target: 100 Energy.
- Core scoring: 2 Energy per second while controlled.
- Respawn after falling/KO: 3 seconds.
- Overtime: if top scores are tied at time expiry, score generation pauses for everyone except the current Core holder; first score change resolves the match.

These numbers are starting values, not sacred constants.

## Moment-to-moment loop

1. **Read** — identify Core position, rival trajectories, nearby props, hazards, and edges.
2. **Move** — run and dash into a favorable angle.
3. **Manipulate** — Pull the Core/prop/rival or Push threats away.
4. **Contest** — steal possession, block a path, launch a prop, cause a fall, or force a rival to waste mobility.
5. **Score** — hold the Core while relocating to safer or more advantageous ground.
6. **React** — arena hazards and player pressure change the situation quickly.
7. **Recover** — after losing the Core or being knocked out, re-enter quickly rather than waiting.
8. **Climax** — final 20–30 seconds increase urgency through score pressure and mild arena escalation.

## Sample match story

The match opens with the Core on a central pedestal. All eight players rush inward. Player A uses Pull a moment before the group arrives, bringing the Core into possession. Player B dashes across the expected escape line and taps Push, knocking A sideways. The Core breaks free.

Player C has been pulling a light metal crate toward themselves. As B reaches the Core, C lines up and uses Push. The crate crosses the arena, hits B, and knocks both B and the Core toward an edge. Player D pulls the Core out of the air before it falls and runs up a ramp.

Thirty seconds later, D leads. The arena enters Overload: side conveyors activate, making the safest route less stable. Two players chase D from behind. D pulls a hanging metal bumper into their path, forcing one rival to dash early. The other rival times a Push perfectly and launches D off the platform. The Core drops near center.

At 10 seconds remaining, Player A trails the leader by four points. A steals the Core with a long Pull, dashes behind a barrier, and survives a final Push by using the barrier as cover. The last scoring tick gives A a two-point lead at zero. Result screen: **VICTORY — +18 rating — REMATCH?**

## Possession rules

The Core is an objective, not a conventional carried item.
- When a player successfully Pulls the Core into close capture range, it magnetically orbits/attaches behind the player.
- While held, the player scores and has a subtle visible Core trail.
- Core holder movement speed should be reduced slightly (starting hypothesis: 5–8%) to make pursuit possible.
- A sufficiently strong Push or impact breaks possession.
- Falling, being knocked out, or entering certain hazards drops the Core.
- The Core cannot be permanently hidden behind geometry; maps must preserve contestability.

## Combat without health bars

The base game does not need conventional HP. Pressure comes from position, knockback, temporary stagger, hazards, and falling.

A hit can:
- Add velocity / knockback.
- Interrupt a Pull channel.
- Break Core possession above a threshold.
- Cause a brief tumble when force exceeds a threshold.
- Push a player into an environmental hazard.

Removing HP keeps the screen clean and makes every interaction spatial.

## Skill expression

Skill should come from:
- Positioning relative to edges and cover.
- Predicting rival movement.
- Timing Push against dashes or airborne players.
- Using Pull to steal the Core or redirect props.
- Combining movement with arena geometry.
- Knowing when to pursue the leader versus disrupt another threat.
- Conserving Flux rather than spamming magnetic actions.
- Recovering from knockback efficiently.

## Anti-frustration rules

- Spawn protection prevents immediate chain knockouts.
- Hard crowd control is short.
- Players should regain meaningful input quickly after impacts.
- No single ordinary action should guarantee a knockout from center arena.
- Camera and VFX must clearly communicate why a player was moved.
- Respawns are fast.
- Last place should still have opportunities to affect the match.

## Match phases

### Opening: 0–20s
Fast race to the Core. Minimal hazards. Teaches the arena naturally.

### Midgame: 20–90s
Full interaction. Props respawn/rotate. Players establish rivalries and begin targeting the leader.

### Overload: final 30s or leader reaches 75 Energy
One arena modifier activates: conveyors accelerate, side platforms shift, selected bumpers wake up, or a low-risk hazard becomes active. The modifier increases interaction density without making outcomes random.

### Result
Maximum delay from victory to actionable result screen: about 2 seconds. Show placement, rating change, one reward summary, and a large Rematch button.

## Core gameplay test

The project should not progress beyond prototype until testers answer “yes” to most of these:
- Is moving fun with no objective present?
- Is pushing a friend satisfying?
- Is pulling/launching a prop understandable?
- Do players laugh or react vocally to knockouts?
- Do players understand who currently holds the Core?
- Do they voluntarily request another round?

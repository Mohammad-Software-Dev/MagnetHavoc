# Game Modes

## Mode philosophy

All modes should reuse the same movement, magnet, physics, and arena systems. New modes are valuable when they create new decisions without requiring a new combat game.

## 1. Core Rush — primary launch mode

**Format:** 8-player free-for-all.

A Flux Core spawns at center. Holding it generates Energy. First to target score wins; otherwise highest score at time expiry.

Why it matters:
- Teaches Pull and Push naturally.
- Generates pursuit and comeback opportunities.
- Creates a visible leader without requiring kills.
- Every player can interact with the same objective.

MVP must prove this mode first.

## 2. Team Core — expansion

**Format:** 4v4.

Team score increases while any teammate controls the Core. Passing is implicit: a teammate can Pull the dropped Core or receive it after controlled movement.

Design goals:
- Protect the carrier.
- Create body-blocking and rescue plays.
- Make friend parties meaningful.

Avoid overly long defensive stalemates. The Core should periodically destabilize safe camping positions.

## 3. Knockout

**Format:** 8-player last survivor or three-stock survival.

No objective Core. The arena slowly becomes more dangerous. Players use Push, Pull, props, and hazards to eliminate rivals.

Variants:
- One life, very short round.
- Three lives, first to last survivor.
- Score by knockouts over 90 seconds.

This is likely the easiest mode to understand in marketing clips.

## 4. Magnetball

**Format:** 3v3.

A large magnetic ball must be moved into the opposing goal. Players cannot simply carry it; movement comes primarily from Pull/Push.

This mode emphasizes:
- Angles.
- Passing.
- Defensive repulsion.
- Team positioning.
- Rebounds and bank shots.

Potential to become a highly competitive mode, but should launch only after base netcode/physics is stable.

## 5. Hot Core

**Format:** 8-player party mode.

One player controls an unstable Core with a visible countdown. The player must transfer or eject it before detonation. Explosion causes major knockback rather than permanent elimination.

The loop creates social blame, chasing, and funny last-second transfers.

## 6. King of the Platform

A capture zone moves between safe areas. Players score while occupying it. Magnetic props and knockback make control unstable.

Use as an event mode; avoid too much passive standing.

## 7. Cargo Heist — future experiment

**Format:** teams.

Multiple magnetic cargo cells must be pulled from center lanes back to team extraction zones. Opponents can steal dropped cargo. This introduces macro decisions without weapons.

## Private match modifiers

For friend lobbies, allow harmless rule mutations:
- Low gravity.
- Maximum Push force.
- Tiny arena.
- Giant props.
- Infinite Flux.
- One-hit tumble.
- Core speed boost.

These are excellent for social play and content creation but should be separated from ranked balance.

## Ranked mode policy

At any one time, ranked should emphasize a small number of well-balanced formats. Do not fragment matchmaking across six queues. Recommended early structure:
- Ranked Core Rush or Team Core as the main queue.
- Casual rotating playlist for party modes.
- Private/custom games for experimentation.

## Mode acceptance criteria

A mode should be promoted from event to permanent only if it:
- Has healthy queue population.
- Creates distinct decisions from Core Rush.
- Does not require confusing exceptions to base controls.
- Maintains acceptable match duration.
- Supports spectatable, readable outcomes.

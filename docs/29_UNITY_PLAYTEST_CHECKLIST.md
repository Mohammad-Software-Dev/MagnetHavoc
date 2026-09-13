# Unity + Device Playtest Checklist

Use this checklist on PR #1 before marking it ready to merge.

## Editor compile gate

- Open `Game/` in Unity 6000.3.24f1.
- Allow package import/domain reload to finish.
- Confirm zero C# compiler errors.
- Run all EditMode tests.
- Run all PlayMode tests.

## Desktop interaction gate

- Start from an empty scene and enter Play mode.
- Confirm arena, four players, Flux Core, MatchManager, camera and HUD appear automatically.
- Move with WASD/arrows.
- Verify stopping feels controlled rather than slippery.
- Dash forward and while changing direction.
- Push nearby bots and crates.
- Pull loose crates and the loose Core.
- Verify a Core carrier is not hit twice by one magnetic event.
- Verify dash knockback resistance is noticeable but not immunity.
- Verify spawn protection prevents immediate magnetic re-knockout.
- Knock the Core off the arena and confirm it resets automatically.
- Knock a player out and confirm a safe respawn is chosen.
- Hold the Core and verify score accumulation.
- Reach Overload and verify x2 scoring.
- Finish by score target and by timer in separate runs.
- Use Rematch and confirm transient state resets.

## Mobile gate

Test on at least one physical Android device before networking becomes priority.

- Landscape orientation usable without hand obstruction.
- Left-side drag movement is stable while simultaneously using magnet/dash controls.
- Quick tap consistently produces Push.
- Intentional hold consistently produces Pull.
- Dash region is reachable without accidental magnet activation.
- HUD does not collide with notches/safe areas.
- Sustained play does not show obvious frame pacing, heat or memory problems.

## Human fun gate

Run at least one session where people other than the developer play.

Capture:
- time to understand objective
- accidental Push/Pull inputs
- camera confusion
- perceived unfair knockouts
- whether Core ownership is visually obvious
- whether respawn protection is noticeable/annoying
- whether players understand Overload
- spontaneous laughter/reactions
- voluntary rematches
- which moments players talk about afterward

Pass condition: four humans are willing to keep rematching for roughly 15 minutes in the ugly prototype. If they are not, fix the local game before adding networking.

# Unity + Device Playtest Checklist

Use this checklist on PR #1 before marking it ready to merge.

## Automated checks already available

- Repository/static validation must pass.
- Roslyn syntax parsing must pass across all Unity C# source.
- Deterministic .NET rule tests must pass.
- The headless Core Rush stress runner must complete 5,000 matches without termination/fairness failures.
- `.github/workflows/unity-editor-tests.yml` is prepared to run Unity EditMode + PlayMode tests through GameCI when `UNITY_LICENSE` is configured in GitHub Actions secrets.

Current infrastructure note: on 2026-09-13 the Unity CI workflow detected that `UNITY_LICENSE` was **not configured**, so the engine test job skipped. A skipped engine job is not a pass.

## Editor compile gate

- Open `Game/` in Unity 6000.3.24f1.
- Allow package import/domain reload to finish.
- Confirm zero C# compiler errors.
- Confirm `Resources/game_tuning.json` loads and reports a valid `TuningVersion`.
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
- Finish by score target and by timer in separate runs if practically achievable.
- Use Rematch and confirm transient state resets.
- Confirm completed matches append valid JSONL summaries to `Application.persistentDataPath/magnet_havoc_playtests.jsonl`.

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

Capture per tuning version:
- time to understand objective
- accidental Push/Pull inputs
- camera confusion
- perceived unfair knockouts
- whether Core ownership is visually obvious
- whether respawn protection is noticeable/annoying
- whether players understand Overload
- winner score and whether the match ended by score target or timer
- spontaneous laughter/reactions
- voluntary rematches
- which moments players talk about afterward

Headless scoring signal for `prototype-2026-09-13-a`: 0/5,000 simulated matches reached the current 100-point target and the average winner score was 42.7. This is only a playtest question because the simulator abstracts away real Unity physics and human skill.

Pass condition: four humans are willing to keep rematching for roughly 15 minutes in the ugly prototype. If they are not, fix the local game before adding networking.

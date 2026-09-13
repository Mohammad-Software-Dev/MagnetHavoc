# Testing Strategy

Magnet Havoc is physics-heavy, so testing is layered. Automated tests can protect syntax and deterministic rules, but they cannot determine whether movement and knockback feel fun.

## Gate 1 — repository validation

`python tools/validate_repo.py`

Runs without Unity and checks required files, JSON validity, and basic C# structural sanity. This runs on every `main` and `dev/**` push and on pull requests.

## Gate 2 — full C# syntax parsing

`dotnet run --project tools/CSharpSyntaxCheck/CSharpSyntaxCheck.csproj --configuration Release -- Game`

Roslyn parses every C# source file in the Unity project. This catches real C# syntax errors without pretending to provide Unity API/type resolution.

## Gate 3 — deterministic simulation smoke tests

`dotnet run --project tools/SimulationSmoke/SimulationSmoke.csproj --configuration Release`

This compiles the Unity-independent production source files directly and tests Flux spending/clamping, score behavior, and tuning invariants. The same source files are used by the Unity project; there is no copied implementation.

## Gate 4 — Unity EditMode tests

`Game/Assets/_MagnetHavoc/Tests/EditMode/SimulationTests.cs`

Run in Unity Test Runner once the project is opened in the pinned editor. The project explicitly depends on the Unity Test Framework. Expand this layer for cooldowns, match transitions, target selection helpers, score multipliers, and save-data migrations.

## Gate 5 — Unity PlayMode tests

To add after the first editor run:
- Bootstrap creates four players, Core, arena, MatchManager, camera, and HUD.
- Core can be acquired and dropped.
- Knockout below kill plane triggers respawn.
- Timer ends match and rematch resets state.
- Bot match can run for several minutes without exceptions.

## Gate 6 — device tests

Required before online work:
- Android mid-range reference device.
- iPhone reference device.
- 60 FPS frame pacing under representative physics load.
- Touch regions work across common aspect ratios and safe areas.
- Thermal/memory check over repeated matches.

## Gate 7 — multiplayer simulation

When networking lands:
- 50/100/150/250 ms latency profiles.
- 1–5% packet loss.
- disconnect/reconnect.
- server rejection of impossible force, movement, score, or cooldown state.
- 8-player soak tests.

## Human fun test

The prototype does not pass Stage 1 because tests are green. It passes when four humans voluntarily rematch for ~15 minutes and can explain why they won or lost. Track confusion, accidental input, dead time, knockback readability, comeback frequency, and rematch intent.

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

This compiles the Unity-independent production source files directly and tests Flux spending/clamping, score behavior, match-clock/Overload behavior, and tuning invariants. The same source files are used by the Unity project; there is no copied implementation.

## Gate 4 — Unity EditMode tests

`Game/Assets/_MagnetHavoc/Tests/EditMode/SimulationTests.cs`

Run in Unity Test Runner once the project is opened in the pinned editor. The project explicitly depends on the Unity Test Framework. Current coverage includes Flux, score, match-clock and safety-tuning invariants.

## Gate 5 — Unity PlayMode tests

`Game/Assets/_MagnetHavoc/Tests/PlayMode/PrototypePlayModeTests.cs`

Prepared regression tests cover:
- Bootstrap creates MatchManager, Core and four active players.
- Core resets after falling out of the arena.
- Restart restores a knocked-out player and match state.

These tests require the real Unity engine and are intentionally not claimed as executed by the non-Unity GitHub workflows.

Additional PlayMode coverage to add after the first editor run:
- Push spends Flux and moves an eligible target.
- Pull drains Flux while held.
- Carrier drops Core after threshold force.
- Timer ends match and rematch resets all transient state.
- Bot match can run for several minutes without exceptions.

## Gate 6 — device tests

Required before online work:
- Android mid-range reference device.
- At least one modern iPhone if available.
- 60 FPS target under normal play.
- Repeated Push/Pull does not create escalating allocations or uncontrolled rigidbody counts.
- Touch regions work across common aspect ratios and safe areas.
- Thermal/memory check over repeated matches.

## Gate 7 — human fun test

This is a product gate, not an automated test.

- Four humans can understand the objective without a long explanation.
- Push/Pull reads clearly under pressure.
- Players blame understandable mistakes rather than controls/camera.
- Knockouts feel funny/competitive rather than arbitrary.
- Players voluntarily rematch for roughly 15 minutes in an ugly build.

Networking should not become the priority until this gate is satisfied.

## Gate 8 — multiplayer simulation

When networking lands:
- 50/100/150/250 ms latency profiles.
- 1–5% packet loss.
- Reconciliation stress while dashing and being pushed.
- Core ownership/score authority divergence checks.
- Reconnect/disconnect cases.
- Server validation of cooldowns, Flux, motion and force limits.

# Magnet Havoc — Game Design & Build Repository

This folder is the source of truth for **Magnet Havoc**, a competitive mobile physics-action party game built around magnetic attraction, repulsion, knockback, movement, short matches, and social rivalry.

This repository is designed to be readable by both humans and AI implementation agents. Start with this README, then read `01_GAME_VISION.md`, `02_CORE_GAMEPLAY.md`, and `20_AI_BUILD_BRIEF.md` before implementing anything.

## One-line pitch

**Magnet Havoc is an 8-player mobile arena brawler where players run, dash, pull objects toward themselves, blast rivals away, and fight over objectives in chaotic 2-minute matches.**

## Non-negotiable pillars

1. **Movement feels good.** Running, dashing, impacts, recovery, and knockback must feel responsive and playful.
2. **One mechanic creates many stories.** Push/pull magnetism must interact with players, the objective, props, and hazards.
3. **Readable in five seconds.** A viewer should understand a gameplay clip without explanation.
4. **Short competitive sessions.** Most matches last 90–150 seconds and immediately offer a rematch.
5. **Fair competition.** Monetization is cosmetic-first. Ranked power is not sold.
6. **Social by design.** Rematches, friend challenges, parties, crews, team modes, and shareable moments are core—not afterthoughts.
7. **Small-team scope.** The initial game must prove fun with one arena, one character body, one objective, and four players before content expansion.

## Recommended reading order

- `01_GAME_VISION.md` — fantasy, audience, pillars, differentiation.
- `02_CORE_GAMEPLAY.md` — moment-to-moment loop and a sample match.
- `03_CONTROLS_CAMERA_GAMEFEEL.md` — mobile controls, camera, responsiveness.
- `04_MAGNET_PHYSICS_COMBAT.md` — push/pull rules, forces, knockback, recovery.
- `05_GAME_MODES.md` — Core Rush and expansion modes.
- `06_ARENAS_HAZARDS.md` — level design, map rules, launch arenas.
- `07_CHARACTERS_COSMETICS.md` — player avatars and customization.
- `08_PROGRESSION_RANKED_REWARDS.md` — progression and ranked structure.
- `09_SOCIAL_PARTIES_CREWS.md` — friends, rematches, parties, crews.
- `10_ECONOMY_MONETIZATION.md` — currencies, cosmetics, pass, fair-play constraints.
- `11_LIVE_OPS_SEASONS.md` — events, seasons, content cadence.
- `12_ART_ANIMATION_VFX_AUDIO.md` — visual language, animation, VFX, sound.
- `13_UI_UX_ONBOARDING.md` — menus, HUD, FTUE, result flow.
- `14_TECHNICAL_ARCHITECTURE.md` — project architecture and performance targets.
- `15_NETWORKING_BACKEND_MATCHMAKING.md` — authoritative multiplayer and services.
- `16_DATA_ANALYTICS_BALANCE.md` — telemetry, KPIs, experiments, tuning.
- `17_AI_BOTS_FAIR_PLAY_SAFETY.md` — bots, anti-cheat, moderation.
- `18_MVP_ROADMAP_ACCEPTANCE.md` — implementation phases and exit criteria.
- `19_CONTENT_PIPELINE_QA.md` — authoring pipeline, testing, release checklist.
- `20_AI_BUILD_BRIEF.md` — instructions for an AI model implementing the game.
- `21_GAME_DATA_SCHEMAS.md` — suggested configuration/data shapes.
- `22_MARKETING_POSITIONING.md` — audience, hooks, store presentation, creative angles.
- `23_LORE_WORLD.md` — light worldbuilding and terminology.
- `24_DECISIONS_AND_OPEN_QUESTIONS.md` — explicit decisions and items intentionally left flexible.
- `26_IMPLEMENTATION_STATUS.md` — what currently exists in source.
- `27_TESTING_STRATEGY.md` — automated, Unity, device, and human test gates.
- `28_STAGE2_FEEL_PASS.md` — first recovery/readability implementation pass.
- `29_UNITY_PLAYTEST_CHECKLIST.md` — exact editor/device/human verification checklist for the current draft PR.
- `30_PLAYTEST_TELEMETRY.md` — local match summaries, tuning versions, and prototype balance-data workflow.

## Product status

**Pre-alpha / local vertical slice.** The current implementation lives on the `dev/vertical-slice` branch and remains unmerged until Unity/editor and device playtest gates pass. Placeholder numeric values remain hypotheses until tested.

## Golden rule for implementation

If a feature makes the game more complex but does not improve **movement, magnetic interaction, competition, social replayability, or spectator clarity**, defer it.

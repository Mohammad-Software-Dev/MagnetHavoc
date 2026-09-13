# UI, UX, and Onboarding

## UX principle

The game should feel fast before and after the match. Menus must not become more complex than the core gameplay.

## Main navigation

Recommended bottom/top-level destinations:
- Play.
- Locker.
- Pass/Events.
- Social.
- Store.

Profile/rank remains accessible from the lobby header.

## Lobby

Primary screen should show:
- Current Mag in 3D.
- Large Play button.
- Selected playlist.
- Party slots.
- Rank/season status.
- One active event/promo panel, not a wall of banners.

## Match HUD

Essential elements only:
- Score / leading score.
- Match timer.
- Core holder indicator.
- Flux meter integrated near Magnet control.
- Dash cooldown.
- Small placement/rank indicator if useful.
- Offscreen Core arrow.

Avoid mini-map in MVP unless testing proves it necessary.

## First-time user experience

Goal: get player moving within seconds.

### Tutorial 1 — Move
Small safe arena. “Move to the Core.” No text wall.

### Tutorial 2 — Pull
Core is out of reach. Hold Magnet to Pull it.

### Tutorial 3 — Push
A training bot blocks path. Tap Magnet to Push it away.

### Tutorial 4 — Dash
Cross a short closing gap or avoid a slow hazard.

### Tutorial 5 — One mini match
Player plus simple bots. Score a small target such as 20 Energy.

Total tutorial target: 2–4 minutes. Skip/replay options after essential safety/account steps.

## Result screen

Within ~2 seconds after match end show:
- Placement / victory.
- Score.
- Rank change if ranked.
- One concise reward summary.
- **Rematch** as prominent action.
- Add-friend shortcuts for notable rivals.
- Continue button.

Detailed stats can live one tap deeper.

## Feedback hierarchy

High priority:
- You gained/lost Core.
- You caused a knockout.
- You were knocked out.
- Final 10 seconds.
- Victory/defeat.

Medium:
- Push/Pull connected.
- Dash ready.
- Challenge progress.

Low:
- Currency drip.
- Cosmetic notifications.

Do not let low-priority economy feedback obscure a live match.

## Accessibility and device UX

- Adjustable control size/position.
- Left-handed layout option if feasible.
- UI safe areas for notches.
- Text scaling within reason.
- 16px+ equivalent touch text size.
- Buttons sized for thumbs.
- Reduced motion / screen shake.
- Haptic toggle.

## Error states

Networking errors should be specific and recoverable:
- Queue canceled.
- Connection lost.
- Reconnecting.
- Match version mismatch.
- Maintenance.

Never trap player on an indefinite spinner.

# Game Data Schemas

This file proposes implementation-neutral data shapes. Names may change, but stable IDs and versioning are important.

## Player tuning

```json
{
  "configId": "player_default_v1",
  "moveSpeed": 6.5,
  "acceleration": 22.0,
  "deceleration": 28.0,
  "turnRate": 720.0,
  "dashSpeed": 13.0,
  "dashDuration": 0.18,
  "dashCooldown": 2.5,
  "spawnProtectionSeconds": 1.5
}
```

Values are examples only.

## Magnet tuning

```json
{
  "configId": "magnet_default_v1",
  "fluxCapacity": 100,
  "pushFluxCost": 25,
  "pushCooldown": 0.75,
  "pushRange": 3.5,
  "pullFluxPerSecond": 18,
  "pullRange": 6.0,
  "fluxRegenPerSecond": 25,
  "fluxRegenDelay": 0.6,
  "tapHoldThreshold": 0.18
}
```

## Physics prop

```json
{
  "propId": "crate_light_a",
  "massClass": "light",
  "networkCritical": true,
  "magnetMultiplier": 1.0,
  "impactMultiplier": 1.0,
  "respawnPolicy": "after_destroy_or_15s",
  "prefabRef": "..."
}
```

## Core Rush ruleset

```json
{
  "rulesetId": "core_rush_v1",
  "maxPlayers": 8,
  "matchSeconds": 120,
  "scoreToWin": 100,
  "scorePerSecond": 2,
  "respawnSeconds": 3,
  "overloadTriggerSecondsRemaining": 30,
  "overloadTriggerLeaderScore": 75
}
```

## Arena

```json
{
  "arenaId": "skyforge_v1",
  "supportedModes": ["core_rush"],
  "spawnSet": "skyforge_spawns_a",
  "coreSpawnId": "center_core",
  "propTableId": "skyforge_props_v1",
  "hazardSequenceId": "skyforge_hazards_v1",
  "cameraProfileId": "arena_standard"
}
```

## Cosmetic

```json
{
  "itemId": "skin_factory_blue_01",
  "itemType": "body_skin",
  "rarity": "common",
  "assetRef": "...",
  "displayNameKey": "item.skin.factory_blue.name",
  "descriptionKey": "item.skin.factory_blue.desc",
  "competitiveStats": null
}
```

`competitiveStats` should remain null for cosmetics.

## Match result

```json
{
  "matchId": "uuid",
  "rulesetVersion": "core_rush_v1",
  "serverBuild": "...",
  "startedAtUtc": "...",
  "endedAtUtc": "...",
  "players": [
    {
      "playerId": "...",
      "placement": 1,
      "score": 100,
      "coreHoldSeconds": 38.4,
      "knockouts": 3,
      "disconnectState": "completed"
    }
  ]
}
```

## ID rules

- IDs are stable, lowercase snake_case where convenient.
- Display names are localization keys, never IDs.
- Removing content should not recycle IDs.
- Server and client configs carry explicit version identifiers.

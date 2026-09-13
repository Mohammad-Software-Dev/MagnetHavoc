using System;

namespace MagnetHavoc
{
    [Serializable]
    public sealed class GameTuning
    {
        public float MoveSpeed = 7.5f;
        public float MoveAcceleration = 34f;
        public float MoveDeceleration = 46f;
        public float RotationSpeedDegrees = 720f;
        public float DashSpeed = 15f;
        public float DashDuration = 0.18f;
        public float DashCooldown = 2.4f;
        public float DashKnockbackMultiplier = 0.55f;

        public float MagnetRange = 7.25f;
        public float PushImpulse = 9.5f;
        public float PullAcceleration = 24f;
        public float PlayerForceMultiplier = 0.82f;
        public float CoreForceMultiplier = 1.2f;
        public float PushLift = 0.16f;
        public float PushFluxCost = 18f;
        public float PullFluxPerSecond = 25f;
        public float FluxMax = 100f;
        public float FluxRegenPerSecond = 17f;
        public float MagnetHoldThreshold = 0.18f;

        public float MatchDurationSeconds = 120f;
        public float ScoreTarget = 100f;
        public float ScorePerSecond = 1f;
        public float OverloadStartSeconds = 30f;
        public float OverloadScoreMultiplier = 2f;

        public float CorePickupRadius = 1.15f;
        public float CorePickupLockout = 0.4f;
        public float CoreCarrierBreakImpulse = 6.8f;
        public float CoreCarrierMoveMultiplier = 0.92f;
        public float CoreCarryHeight = 1.35f;
        public float CoreCarryForward = 0.65f;
        public float CoreDropSpeed = 3.2f;
        public float CoreResetY = -3.5f;

        public float KnockoutY = -4.5f;
        public float RespawnDelay = 1.35f;
        public float SpawnProtectionSeconds = 0.85f;

        public float ArenaSafeRadius = 7.15f;
        public float BotEdgeAvoidRadius = 6.65f;

        public static GameTuning CreateDefault() => new GameTuning();
    }
}

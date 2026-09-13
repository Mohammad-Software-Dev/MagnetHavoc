namespace MagnetHavoc
{
    public static class TuningRules
    {
        public static bool IsValid(GameTuning t)
        {
            if (t == null) return false;
            return !string.IsNullOrWhiteSpace(t.TuningVersion)
                && t.MoveSpeed > 0f
                && t.MoveAcceleration > 0f
                && t.MoveDeceleration > 0f
                && t.RotationSpeedDegrees > 0f
                && t.DashSpeed > 0f
                && t.DashDuration > 0f
                && t.DashCooldown >= t.DashDuration
                && t.DashKnockbackMultiplier > 0f
                && t.DashKnockbackMultiplier <= 1f
                && t.MagnetRange > 0f
                && t.PushImpulse > 0f
                && t.PullAcceleration > 0f
                && t.PlayerForceMultiplier > 0f
                && t.CoreForceMultiplier > 0f
                && t.PushFluxCost >= 0f
                && t.PullFluxPerSecond >= 0f
                && t.FluxMax > 0f
                && t.PushFluxCost <= t.FluxMax
                && t.FluxRegenPerSecond >= 0f
                && t.MatchDurationSeconds > 0f
                && t.OverloadStartSeconds >= 0f
                && t.OverloadStartSeconds < t.MatchDurationSeconds
                && t.ScoreTarget > 0f
                && t.ScorePerSecond > 0f
                && t.OverloadScoreMultiplier >= 1f
                && t.CorePickupRadius > 0f
                && t.CorePickupRadius < t.MagnetRange
                && t.CorePickupLockout >= 0f
                && t.CoreCarrierBreakImpulse > 0f
                && t.CoreCarrierMoveMultiplier > 0f
                && t.CoreCarrierMoveMultiplier <= 1f
                && t.CoreResetY > t.KnockoutY
                && t.RespawnDelay >= 0f
                && t.SpawnProtectionSeconds >= 0f
                && t.KnockoutCreditWindowSeconds >= 0f
                && t.ArenaSafeRadius > 0f
                && t.BotEdgeAvoidRadius > 0f
                && t.BotEdgeAvoidRadius < t.ArenaSafeRadius;
        }
    }
}

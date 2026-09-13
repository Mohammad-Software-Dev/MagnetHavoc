using MagnetHavoc;
using MagnetHavoc.Simulation;

static class Smoke
{
    private static int _assertions;

    public static int Main()
    {
        FluxRules();
        ScoreRules();
        ClockRules();
        StatsRules();
        TuningSanity();
        Console.WriteLine($"Simulation smoke tests passed ({_assertions} assertions).\n");
        return 0;
    }

    private static void FluxRules()
    {
        FluxModel flux = new FluxModel(100f);
        Check(flux.TrySpend(18f), "push cost should be affordable from full Flux");
        CheckClose(82f, flux.Current, "spend subtracts exact amount");
        Check(!flux.TrySpend(90f), "overspend must fail");
        CheckClose(82f, flux.Current, "failed overspend must not mutate Flux");
        flux.Add(-200f);
        CheckClose(0f, flux.Current, "Flux clamps at zero");
        flux.Add(1000f);
        CheckClose(100f, flux.Current, "Flux clamps at max");
        flux.Reset();
        CheckClose(100f, flux.Current, "reset restores full Flux");
    }

    private static void ScoreRules()
    {
        MatchScoreModel score = new MatchScoreModel();
        score.Register(0);
        score.Register(1);
        score.AddScore(0, 4f);
        score.AddScore(1, 7f);
        score.AddScore(0, -999f);
        CheckClose(4f, score.GetScore(0), "negative scoring is ignored");
        CheckClose(7f, score.GetScore(1), "score accumulates");
        Check(score.GetLeaderId() == 1, "highest score leads");
        Check(score.GetUniqueLeaderId() == 1, "unique leader is reported");
        score.AddScore(0, 3f);
        Check(score.GetUniqueLeaderId() == -1, "top-score tie has no unique winner");
        score.AddScore(1, 0.25f);
        Check(score.GetUniqueLeaderId() == 1, "next scoring event resolves tie");
        score.Reset();
        CheckClose(0f, score.GetScore(0), "reset clears player zero");
        CheckClose(0f, score.GetScore(1), "reset clears player one");
        Check(score.GetUniqueLeaderId() == -1, "equal reset scores do not create arbitrary winner");
    }

    private static void ClockRules()
    {
        MatchClockModel clock = new MatchClockModel(120f, 30f);
        CheckClose(120f, clock.RemainingSeconds, "clock starts at duration");
        Check(!clock.IsOverload, "match does not start in overload");
        clock.Advance(89f);
        CheckClose(31f, clock.RemainingSeconds, "clock advances deterministically");
        Check(!clock.IsOverload, "31 seconds is outside overload");
        clock.Advance(1f);
        Check(clock.IsOverload, "30 seconds enters overload");
        clock.Advance(1000f);
        Check(clock.IsExpired, "clock expires at zero");
        Check(!clock.IsOverload, "expired clock is not overload");
        CheckClose(0f, clock.RemainingSeconds, "clock clamps at zero");
        clock.Reset();
        CheckClose(120f, clock.RemainingSeconds, "clock reset restores duration");
    }

    private static void StatsRules()
    {
        MatchStatsModel stats = new MatchStatsModel();
        stats.Register(0);
        stats.RecordPush(0);
        stats.RecordDash(0);
        stats.RecordCorePickup(0);
        stats.RecordCoreDrop(0);
        stats.RecordKnockout(0);
        stats.RecordElimination(0);
        stats.AddPossession(0, 3.5f);
        stats.AddPull(0, 1.25f);
        stats.AddPossession(0, -99f);

        PlayerMatchStats player = stats.Get(0);
        Check(player.Pushes == 1, "push count tracked");
        Check(player.Dashes == 1, "dash count tracked");
        Check(player.CorePickups == 1 && player.CoreDrops == 1, "Core transitions tracked");
        Check(player.Knockouts == 1 && player.Eliminations == 1, "combat results tracked");
        CheckClose(3.5f, player.PossessionSeconds, "possession time tracks only positive deltas");
        CheckClose(1.25f, player.PullSeconds, "Pull time accumulates");

        stats.Reset();
        PlayerMatchStats reset = stats.Get(0);
        Check(reset.Pushes == 0 && reset.Dashes == 0 && reset.Knockouts == 0, "stat reset clears counters");
        CheckClose(0f, reset.PossessionSeconds, "stat reset clears timers");
    }

    private static void TuningSanity()
    {
        GameTuning t = GameTuning.CreateDefault();
        Check(TuningRules.IsValid(t), "default tuning passes production validation");
        Check(t.MoveDeceleration > t.MoveAcceleration, "release deceleration exceeds acceleration");
        Check(t.DashCooldown > t.DashDuration, "dash cooldown exceeds active dash duration");
        Check(t.KnockoutCreditWindowSeconds >= 0f, "knockout attribution window valid");

        GameTuning invalid = GameTuning.CreateDefault();
        invalid.MatchDurationSeconds = 10f;
        invalid.OverloadStartSeconds = 15f;
        Check(!TuningRules.IsValid(invalid), "invalid overload timing is rejected");

        invalid = GameTuning.CreateDefault();
        invalid.DashKnockbackMultiplier = 2f;
        Check(!TuningRules.IsValid(invalid), "invalid dash resistance is rejected");
    }

    private static void Check(bool value, string message)
    {
        _assertions++;
        if (!value) throw new InvalidOperationException("FAILED: " + message);
    }

    private static void CheckClose(float expected, float actual, string message)
    {
        _assertions++;
        if (Math.Abs(expected - actual) > 0.0001f)
            throw new InvalidOperationException($"FAILED: {message}. Expected {expected}, got {actual}");
    }
}

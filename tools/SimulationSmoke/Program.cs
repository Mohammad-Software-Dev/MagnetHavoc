using MagnetHavoc;
using MagnetHavoc.Simulation;

static class Smoke
{
    private static int _assertions;

    public static int Main()
    {
        FluxRules();
        ScoreRules();
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
        score.Reset();
        CheckClose(0f, score.GetScore(0), "reset clears player zero");
        CheckClose(0f, score.GetScore(1), "reset clears player one");
    }

    private static void TuningSanity()
    {
        GameTuning t = GameTuning.CreateDefault();
        Check(t.MoveSpeed > 0f, "move speed positive");
        Check(t.DashCooldown > t.DashDuration, "dash cooldown exceeds active dash duration");
        Check(t.MagnetRange > t.CorePickupRadius, "magnet range exceeds pickup radius");
        Check(t.FluxMax > t.PushFluxCost, "a fresh player can Push");
        Check(t.MatchDurationSeconds > t.OverloadStartSeconds, "overload begins before match ends");
        Check(t.ScoreTarget > 0f && t.ScorePerSecond > 0f, "score rules positive");
        Check(t.RespawnDelay > 0f, "respawn delay positive");
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

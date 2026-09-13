using MagnetHavoc;
using MagnetHavoc.Simulation;

static class HeadlessMatchSim
{
    private const int PlayerCount = 4;
    private const int MatchCount = 5000;
    private const float Step = 0.1f;
    private const float MaxOvertimeSeconds = 90f;

    public static int Main()
    {
        GameTuning tuning = GameTuning.CreateDefault();
        if (!TuningRules.IsValid(tuning)) throw new InvalidOperationException("Default tuning is invalid.");

        int[] wins = new int[PlayerCount];
        int suddenDeaths = 0;
        int scoreTargetFinishes = 0;
        double totalDuration = 0d;
        double totalKnockouts = 0d;
        double totalWinnerScore = 0d;
        List<float> durations = new List<float>(MatchCount);

        for (int seed = 0; seed < MatchCount; seed++)
        {
            MatchResult result = RunMatch(tuning, seed);
            wins[result.WinnerId]++;
            if (result.SuddenDeath) suddenDeaths++;
            if (result.ReachedScoreTarget) scoreTargetFinishes++;
            totalDuration += result.Duration;
            totalKnockouts += result.TotalKnockouts;
            totalWinnerScore += result.WinnerScore;
            durations.Add(result.Duration);
        }

        durations.Sort();
        float p95 = durations[(int)Math.Floor((durations.Count - 1) * 0.95)];
        double averageDuration = totalDuration / MatchCount;
        double averageKnockouts = totalKnockouts / MatchCount;
        double averageWinnerScore = totalWinnerScore / MatchCount;
        double suddenDeathRate = 100d * suddenDeaths / MatchCount;
        double scoreTargetRate = 100d * scoreTargetFinishes / MatchCount;

        Console.WriteLine($"Headless Core Rush stress run: {MatchCount:N0} matches");
        Console.WriteLine($"Tuning: {tuning.TuningVersion}");
        Console.WriteLine($"Average duration: {averageDuration:0.0}s | P95: {p95:0.0}s | Sudden Death: {suddenDeathRate:0.0}%");
        Console.WriteLine($"Score-target finishes: {scoreTargetFinishes:N0} ({scoreTargetRate:0.00}%) | Average winner score: {averageWinnerScore:0.0}");
        Console.WriteLine($"Average credited KOs/match: {averageKnockouts:0.00}");

        for (int i = 0; i < PlayerCount; i++)
        {
            double share = 100d * wins[i] / MatchCount;
            Console.WriteLine($"P{i + 1} wins: {wins[i],4} ({share:0.00}%)");
            if (share < 20d || share > 30d)
                throw new InvalidOperationException($"Player-id fairness regression: P{i + 1} won {share:0.00}% of matches.");
        }

        if (p95 > tuning.MatchDurationSeconds + MaxOvertimeSeconds)
            throw new InvalidOperationException($"P95 match duration {p95:0.0}s exceeded the overtime safety bound.");

        Console.WriteLine("Headless Core Rush stress simulation passed.");
        return 0;
    }

    private static MatchResult RunMatch(GameTuning tuning, int seed)
    {
        Random random = new Random(unchecked(7919 * seed + 104729));
        MatchScoreModel scores = new MatchScoreModel();
        MatchStatsModel stats = new MatchStatsModel();
        MatchClockModel clock = new MatchClockModel(tuning.MatchDurationSeconds, tuning.OverloadStartSeconds);

        for (int i = 0; i < PlayerCount; i++)
        {
            scores.Register(i);
            stats.Register(i);
        }

        int holder = -1;
        bool suddenDeath = false;
        bool reachedScoreTarget = false;
        float elapsed = 0f;
        int winner = -1;

        while (elapsed <= tuning.MatchDurationSeconds + MaxOvertimeSeconds)
        {
            if (!suddenDeath) clock.Advance(Step);

            for (int player = 0; player < PlayerCount; player++)
            {
                if (Chance(random, 0.34f * Step)) stats.RecordPush(player);
                if (Chance(random, 0.18f * Step)) stats.RecordDash(player);
                if (Chance(random, 0.42f * Step)) stats.AddPull(player, Step);
            }

            if (holder < 0)
            {
                if (Chance(random, 1.65f * Step))
                {
                    holder = random.Next(PlayerCount);
                    stats.RecordCorePickup(holder);
                }
            }
            else
            {
                stats.AddPossession(holder, Step);
                float multiplier = suddenDeath || clock.IsOverload ? tuning.OverloadScoreMultiplier : 1f;
                float score = scores.AddScore(holder, tuning.ScorePerSecond * multiplier * Step);

                if (score >= tuning.ScoreTarget)
                {
                    winner = holder;
                    reachedScoreTarget = true;
                    break;
                }

                if (suddenDeath)
                {
                    int leader = scores.GetUniqueLeaderId();
                    if (leader >= 0)
                    {
                        winner = leader;
                        break;
                    }
                }

                // Pressure from the three non-carriers. This is deliberately symmetric by player id.
                if (Chance(random, 0.48f * Step))
                {
                    int carrier = holder;
                    int attacker = RandomOtherPlayer(random, carrier);
                    stats.RecordCoreDrop(carrier);
                    holder = -1;

                    // Some successful breaks become arena knockouts; this exercises stat integrity.
                    if (Chance(random, 0.22f))
                    {
                        stats.RecordElimination(carrier);
                        stats.RecordKnockout(attacker);
                    }
                }
            }

            if (!suddenDeath && clock.IsExpired)
            {
                int leader = scores.GetUniqueLeaderId();
                if (leader >= 0)
                {
                    winner = leader;
                    break;
                }
                suddenDeath = true;
            }

            elapsed += Step;
        }

        if (winner < 0)
            throw new InvalidOperationException($"Seed {seed} failed to terminate within overtime safety bound.");

        float possession = 0f;
        int totalKnockouts = 0;
        for (int i = 0; i < PlayerCount; i++)
        {
            PlayerMatchStats playerStats = stats.Get(i);
            possession += playerStats.PossessionSeconds;
            totalKnockouts += playerStats.Knockouts;
            if (playerStats.PossessionSeconds < 0f || playerStats.PullSeconds < 0f)
                throw new InvalidOperationException($"Seed {seed} produced negative time statistics.");
        }

        if (possession > elapsed + Step * 1.5f)
            throw new InvalidOperationException($"Seed {seed} recorded more possession time than match time.");

        return new MatchResult(winner, elapsed, suddenDeath, totalKnockouts, reachedScoreTarget, scores.GetScore(winner));
    }

    private static int RandomOtherPlayer(Random random, int excluded)
    {
        int value = random.Next(PlayerCount - 1);
        return value >= excluded ? value + 1 : value;
    }

    private static bool Chance(Random random, float probability)
    {
        return random.NextDouble() < Math.Clamp(probability, 0f, 1f);
    }

    private readonly record struct MatchResult(
        int WinnerId,
        float Duration,
        bool SuddenDeath,
        int TotalKnockouts,
        bool ReachedScoreTarget,
        float WinnerScore);
}

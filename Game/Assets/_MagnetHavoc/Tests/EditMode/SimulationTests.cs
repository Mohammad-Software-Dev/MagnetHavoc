using MagnetHavoc.Simulation;
using NUnit.Framework;

namespace MagnetHavoc.Tests
{
    public sealed class SimulationTests
    {
        [Test]
        public void FluxCannotOverspend()
        {
            FluxModel flux = new FluxModel(100f);
            Assert.IsTrue(flux.TrySpend(80f));
            Assert.IsFalse(flux.TrySpend(30f));
            Assert.AreEqual(20f, flux.Current, 0.001f);
        }

        [Test]
        public void FluxClampsOnRecharge()
        {
            FluxModel flux = new FluxModel(100f);
            flux.TrySpend(80f);
            flux.Add(999f);
            Assert.AreEqual(100f, flux.Current, 0.001f);
        }

        [Test]
        public void ScoreModelTracksLeaderAndReset()
        {
            MatchScoreModel scores = new MatchScoreModel();
            scores.Register(0);
            scores.Register(1);
            scores.AddScore(0, 10f);
            scores.AddScore(1, 12f);
            Assert.AreEqual(1, scores.GetLeaderId());
            Assert.AreEqual(1, scores.GetUniqueLeaderId());
            scores.Reset();
            Assert.AreEqual(0f, scores.GetScore(0), 0.001f);
            Assert.AreEqual(0f, scores.GetScore(1), 0.001f);
            Assert.AreEqual(-1, scores.GetUniqueLeaderId());
        }

        [Test]
        public void UniqueLeaderRejectsTopScoreTie()
        {
            MatchScoreModel scores = new MatchScoreModel();
            scores.Register(0);
            scores.Register(1);
            scores.Register(2);
            scores.AddScore(0, 10f);
            scores.AddScore(1, 10f);
            scores.AddScore(2, 3f);
            Assert.AreEqual(-1, scores.GetUniqueLeaderId());
            scores.AddScore(1, 0.5f);
            Assert.AreEqual(1, scores.GetUniqueLeaderId());
        }

        [Test]
        public void MatchClockEntersOverloadAndExpires()
        {
            MatchClockModel clock = new MatchClockModel(120f, 30f);
            clock.Advance(89f);
            Assert.IsFalse(clock.IsOverload);
            Assert.IsFalse(clock.IsExpired);

            clock.Advance(1f);
            Assert.IsTrue(clock.IsOverload);
            Assert.AreEqual(30f, clock.RemainingSeconds, 0.001f);

            clock.Advance(100f);
            Assert.IsTrue(clock.IsExpired);
            Assert.IsFalse(clock.IsOverload);
            Assert.AreEqual(0f, clock.RemainingSeconds, 0.001f);
        }

        [Test]
        public void DefaultTuningKeepsSafetyRadiiOrdered()
        {
            GameTuning tuning = GameTuning.CreateDefault();
            Assert.Greater(tuning.ArenaSafeRadius, tuning.BotEdgeAvoidRadius);
            Assert.Greater(tuning.SpawnProtectionSeconds, 0f);
            Assert.Greater(tuning.MoveDeceleration, tuning.MoveAcceleration);
            Assert.Greater(tuning.CoreResetY, tuning.KnockoutY);
            Assert.Greater(tuning.DashKnockbackMultiplier, 0f);
            Assert.LessOrEqual(tuning.DashKnockbackMultiplier, 1f);
        }
    }
}

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
            scores.Reset();
            Assert.AreEqual(0f, scores.GetScore(0), 0.001f);
            Assert.AreEqual(0f, scores.GetScore(1), 0.001f);
        }
    }
}

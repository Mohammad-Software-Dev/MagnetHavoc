using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace MagnetHavoc.Tests
{
    public sealed class PrototypePlayModeTests
    {
        [UnitySetUp]
        public IEnumerator SetUpPrototype()
        {
            if (Object.FindAnyObjectByType<PrototypeBootstrap>() == null)
            {
                GameObject root = new GameObject("MagnetHavoc_PlayModeTestRoot");
                root.AddComponent<PrototypeBootstrap>();
            }

            yield return null;
            yield return new WaitForFixedUpdate();

            Assert.IsNotNull(MatchManager.Instance, "Prototype must create a MatchManager.");
            Assert.IsNotNull(CoreObjective.Instance, "Prototype must create a Flux Core.");
            MatchManager.Instance.RestartMatch();
            yield return null;
        }

        [UnityTest]
        public IEnumerator BootstrapCreatesFourPlayerCoreRushLoop()
        {
            MatchManager match = MatchManager.Instance;
            Assert.AreEqual(MatchState.Playing, match.State);
            Assert.IsNotNull(match.Players);
            Assert.AreEqual(4, match.Players.Length);

            for (int i = 0; i < match.Players.Length; i++)
            {
                Assert.IsNotNull(match.Players[i]);
                Assert.IsTrue(match.Players[i].gameObject.activeInHierarchy);
            }

            Assert.IsTrue(CoreObjective.Instance.IsLoose);
            yield return null;
        }

        [UnityTest]
        public IEnumerator CoreResetsAfterFallingOutOfArena()
        {
            CoreObjective core = CoreObjective.Instance;
            core.Drop(new Vector3(0f, RuntimeContext.Tuning.CoreResetY - 2f, 0f), Vector3.zero);

            yield return new WaitForFixedUpdate();
            yield return null;

            Assert.Greater(core.transform.position.y, RuntimeContext.Tuning.CoreResetY);
            Assert.IsTrue(core.IsLoose);
        }

        [UnityTest]
        public IEnumerator RestartRestoresKnockedOutPlayerAndMatchState()
        {
            MatchManager match = MatchManager.Instance;
            PlayerController player = match.Players[0];
            match.OnPlayerKnockedOut(player);
            Assert.IsTrue(player.IsKnockedOut);

            match.RestartMatch();
            yield return null;

            Assert.AreEqual(MatchState.Playing, match.State);
            Assert.IsFalse(player.IsKnockedOut);
            Assert.IsTrue(player.gameObject.activeInHierarchy);
            Assert.AreEqual(0f, match.GetScore(player.PlayerId), 0.001f);
        }
    }
}

using System;
using System.IO;
using MagnetHavoc.Simulation;
using UnityEngine;

namespace MagnetHavoc
{
    public sealed class PrototypeTelemetry : MonoBehaviour
    {
        [Serializable]
        private sealed class PlayerSummary
        {
            public int playerId;
            public float score;
            public int pushes;
            public int dashes;
            public int corePickups;
            public int coreDrops;
            public int knockouts;
            public int eliminations;
            public float possessionSeconds;
            public float pullSeconds;
        }

        [Serializable]
        private sealed class MatchSummary
        {
            public string utc;
            public string appVersion;
            public string unityVersion;
            public int matchIndex;
            public int winnerId;
            public bool suddenDeath;
            public PlayerSummary[] players;
        }

        private MatchManager _match;
        private int _matchIndex = 1;

        public static string LogPath => Path.Combine(Application.persistentDataPath, "magnet_havoc_playtests.jsonl");

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AutoCreate()
        {
            if (Object.FindAnyObjectByType<PrototypeTelemetry>() != null) return;
            new GameObject("PrototypeTelemetry").AddComponent<PrototypeTelemetry>();
        }

        private void Update()
        {
            if (_match == null && MatchManager.Instance != null)
                Configure(MatchManager.Instance);
        }

        public void Configure(MatchManager match)
        {
            if (_match == match) return;
            Unsubscribe();
            _match = match;
            if (_match == null) return;
            _match.MatchFinished += OnMatchFinished;
            _match.MatchRestarted += OnMatchRestarted;
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        private void Unsubscribe()
        {
            if (_match == null) return;
            _match.MatchFinished -= OnMatchFinished;
            _match.MatchRestarted -= OnMatchRestarted;
        }

        private void OnMatchRestarted()
        {
            _matchIndex++;
        }

        private void OnMatchFinished(int winnerId)
        {
            if (_match == null || _match.Players == null) return;

            PlayerSummary[] players = new PlayerSummary[_match.Players.Length];
            for (int i = 0; i < _match.Players.Length; i++)
            {
                PlayerController player = _match.Players[i];
                PlayerMatchStats stats = _match.GetStats(player.PlayerId);
                players[i] = new PlayerSummary
                {
                    playerId = player.PlayerId,
                    score = _match.GetScore(player.PlayerId),
                    pushes = stats.Pushes,
                    dashes = stats.Dashes,
                    corePickups = stats.CorePickups,
                    coreDrops = stats.CoreDrops,
                    knockouts = stats.Knockouts,
                    eliminations = stats.Eliminations,
                    possessionSeconds = stats.PossessionSeconds,
                    pullSeconds = stats.PullSeconds
                };
            }

            MatchSummary summary = new MatchSummary
            {
                utc = DateTime.UtcNow.ToString("O"),
                appVersion = Application.version,
                unityVersion = Application.unityVersion,
                matchIndex = _matchIndex,
                winnerId = winnerId,
                suddenDeath = _match.EndedInSuddenDeath,
                players = players
            };

            try
            {
                File.AppendAllText(LogPath, JsonUtility.ToJson(summary) + Environment.NewLine);
                Debug.Log($"Magnet Havoc playtest summary written to {LogPath}");
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"Failed to write Magnet Havoc playtest telemetry: {exception.Message}");
            }
        }
    }
}

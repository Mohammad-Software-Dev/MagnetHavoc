using System.Collections;
using MagnetHavoc.Simulation;
using UnityEngine;

namespace MagnetHavoc
{
    public enum MatchState { Warmup, Playing, Ended }

    public sealed class MatchManager : MonoBehaviour
    {
        public static MatchManager Instance { get; private set; }

        private readonly MatchScoreModel _scores = new MatchScoreModel();
        private MatchClockModel _clock;
        private Vector3[] _spawns;
        private CoreObjective _core;

        public MatchState State { get; private set; } = MatchState.Warmup;
        public PlayerController[] Players { get; private set; }
        public float RemainingSeconds => _clock != null ? _clock.RemainingSeconds : 0f;
        public int WinnerId { get; private set; } = -1;
        public bool IsOverload => State == MatchState.Playing && _clock != null && _clock.IsOverload;

        private void Awake() => Instance = this;

        public void Configure(PlayerController[] players, Vector3[] spawns, CoreObjective core)
        {
            Players = players;
            _spawns = spawns;
            _core = core;
            for (int i = 0; i < players.Length; i++) _scores.Register(players[i].PlayerId);
            RestartMatch();
        }

        private void Update()
        {
            if (State != MatchState.Playing || _core == null || _clock == null) return;

            _clock.Advance(Time.deltaTime);
            if (_core.Holder != null)
            {
                float multiplier = IsOverload ? RuntimeContext.Tuning.OverloadScoreMultiplier : 1f;
                float score = _scores.AddScore(_core.Holder.PlayerId, RuntimeContext.Tuning.ScorePerSecond * multiplier * Time.deltaTime);
                if (score >= RuntimeContext.Tuning.ScoreTarget)
                {
                    Finish(_core.Holder.PlayerId);
                    return;
                }
            }

            if (_clock.IsExpired) Finish(_scores.GetLeaderId());
        }

        public float GetScore(int playerId) => _scores.GetScore(playerId);

        public PlayerController GetNearestOpponent(PlayerController owner)
        {
            if (Players == null) return null;
            PlayerController best = null;
            float bestSq = float.MaxValue;
            for (int i = 0; i < Players.Length; i++)
            {
                PlayerController candidate = Players[i];
                if (candidate == null || candidate == owner || candidate.IsKnockedOut || candidate.IsSpawnProtected) continue;
                float sq = (candidate.transform.position - owner.transform.position).sqrMagnitude;
                if (sq < bestSq)
                {
                    bestSq = sq;
                    best = candidate;
                }
            }
            return best;
        }

        public void OnPlayerKnockedOut(PlayerController player)
        {
            if (player == null || player.IsKnockedOut) return;
            Vector3 knockoutPosition = player.transform.position;
            player.MarkKnockedOut();
            GameAudio.Instance?.PlayKnockout(knockoutPosition);
            StartCoroutine(RespawnAfterDelay(player));
        }

        private IEnumerator RespawnAfterDelay(PlayerController player)
        {
            yield return new WaitForSeconds(RuntimeContext.Tuning.RespawnDelay);
            if (State == MatchState.Playing)
                player.Respawn(SelectSafestSpawn(player));
        }

        private Vector3 SelectSafestSpawn(PlayerController respawning)
        {
            if (_spawns == null || _spawns.Length == 0) return respawning.SpawnPoint;

            int preferred = Mathf.Abs(respawning.PlayerId) % _spawns.Length;
            int bestIndex = preferred;
            float bestSafety = -1f;

            for (int offset = 0; offset < _spawns.Length; offset++)
            {
                int spawnIndex = (preferred + offset) % _spawns.Length;
                Vector3 spawn = _spawns[spawnIndex];
                float nearestOpponentSq = float.MaxValue;

                if (Players != null)
                {
                    for (int i = 0; i < Players.Length; i++)
                    {
                        PlayerController candidate = Players[i];
                        if (candidate == null || candidate == respawning || candidate.IsKnockedOut || !candidate.gameObject.activeInHierarchy)
                            continue;

                        float sq = (candidate.transform.position - spawn).sqrMagnitude;
                        if (sq < nearestOpponentSq) nearestOpponentSq = sq;
                    }
                }

                if (nearestOpponentSq > bestSafety)
                {
                    bestSafety = nearestOpponentSq;
                    bestIndex = spawnIndex;
                }
            }

            return _spawns[bestIndex];
        }

        private void Finish(int winnerId)
        {
            State = MatchState.Ended;
            WinnerId = winnerId;
            GameAudio.Instance?.PlayWin(Vector3.zero);
        }

        public void RestartMatch()
        {
            StopAllCoroutines();
            _scores.Reset();
            _clock = new MatchClockModel(RuntimeContext.Tuning.MatchDurationSeconds, RuntimeContext.Tuning.OverloadStartSeconds);
            WinnerId = -1;
            State = MatchState.Playing;

            if (Players != null)
            {
                for (int i = 0; i < Players.Length; i++)
                {
                    if (Players[i] != null) Players[i].Respawn(_spawns[i % _spawns.Length]);
                }
            }
            _core?.ResetToSpawn();
        }
    }
}

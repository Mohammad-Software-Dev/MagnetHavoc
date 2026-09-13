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
        private Vector3[] _spawns;
        private CoreObjective _core;
        private float _remaining;

        public MatchState State { get; private set; } = MatchState.Warmup;
        public PlayerController[] Players { get; private set; }
        public float RemainingSeconds => _remaining;
        public int WinnerId { get; private set; } = -1;
        public bool IsOverload => State == MatchState.Playing && _remaining <= RuntimeContext.Tuning.OverloadStartSeconds;

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
            if (State != MatchState.Playing || _core == null) return;

            _remaining -= Time.deltaTime;
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

            if (_remaining <= 0f) Finish(_scores.GetLeaderId());
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
                if (candidate == null || candidate == owner || candidate.IsKnockedOut) continue;
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
            player.MarkKnockedOut();
            GameAudio.Instance?.PlayKnockout(player.SpawnPoint);
            StartCoroutine(RespawnAfterDelay(player));
        }

        private IEnumerator RespawnAfterDelay(PlayerController player)
        {
            yield return new WaitForSeconds(RuntimeContext.Tuning.RespawnDelay);
            if (State == MatchState.Playing)
            {
                Vector3 spawn = _spawns[player.PlayerId % _spawns.Length];
                player.Respawn(spawn);
            }
        }

        private void Finish(int winnerId)
        {
            State = MatchState.Ended;
            WinnerId = winnerId;
            _remaining = 0f;
            GameAudio.Instance?.PlayWin(Vector3.zero);
        }

        public void RestartMatch()
        {
            StopAllCoroutines();
            _scores.Reset();
            _remaining = RuntimeContext.Tuning.MatchDurationSeconds;
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

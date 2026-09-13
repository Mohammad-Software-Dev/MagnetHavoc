using System;
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
        private readonly MatchStatsModel _stats = new MatchStatsModel();
        private MatchClockModel _clock;
        private Vector3[] _spawns;
        private CoreObjective _core;
        private bool _suddenDeath;
        private float _elapsedSeconds;

        public MatchState State { get; private set; } = MatchState.Warmup;
        public PlayerController[] Players { get; private set; }
        public float RemainingSeconds => _clock != null ? _clock.RemainingSeconds : 0f;
        public float ElapsedSeconds => _elapsedSeconds;
        public int WinnerId { get; private set; } = -1;
        public bool IsSuddenDeath => State == MatchState.Playing && _suddenDeath;
        public bool IsOverload => State == MatchState.Playing && (_suddenDeath || (_clock != null && _clock.IsOverload));
        public bool EndedInSuddenDeath { get; private set; }

        public event Action<int> MatchFinished;
        public event Action MatchRestarted;

        private void Awake() => Instance = this;

        public void Configure(PlayerController[] players, Vector3[] spawns, CoreObjective core)
        {
            Players = players;
            _spawns = spawns;
            _core = core;
            for (int i = 0; i < players.Length; i++)
            {
                _scores.Register(players[i].PlayerId);
                _stats.Register(players[i].PlayerId);
            }
            RestartMatch();
        }

        private void Update()
        {
            if (State != MatchState.Playing || _core == null || _clock == null) return;

            _elapsedSeconds += Time.deltaTime;
            if (!_suddenDeath) _clock.Advance(Time.deltaTime);

            if (_core.Holder != null)
            {
                int holderId = _core.Holder.PlayerId;
                _stats.AddPossession(holderId, Time.deltaTime);

                float multiplier = IsOverload ? RuntimeContext.Tuning.OverloadScoreMultiplier : 1f;
                float score = _scores.AddScore(holderId, RuntimeContext.Tuning.ScorePerSecond * multiplier * Time.deltaTime);
                if (score >= RuntimeContext.Tuning.ScoreTarget)
                {
                    Finish(holderId);
                    return;
                }

                if (_suddenDeath)
                {
                    int suddenDeathLeader = _scores.GetUniqueLeaderId();
                    if (suddenDeathLeader >= 0)
                    {
                        Finish(suddenDeathLeader);
                        return;
                    }
                }
            }

            if (!_suddenDeath && _clock.IsExpired)
            {
                int leader = _scores.GetUniqueLeaderId();
                if (leader >= 0)
                    Finish(leader);
                else
                    _suddenDeath = true;
            }
        }

        public float GetScore(int playerId) => _scores.GetScore(playerId);
        public PlayerMatchStats GetStats(int playerId) => _stats.Get(playerId);
        public void RecordPush(int playerId) => _stats.RecordPush(playerId);
        public void RecordDash(int playerId) => _stats.RecordDash(playerId);
        public void RecordPull(int playerId, float seconds) => _stats.AddPull(playerId, seconds);
        public void RecordCorePickup(int playerId) => _stats.RecordCorePickup(playerId);
        public void RecordCoreDrop(int playerId) => _stats.RecordCoreDrop(playerId);

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

            int creditedInstigator = player.GetRecentInstigator(RuntimeContext.Tuning.KnockoutCreditWindowSeconds);
            _stats.RecordElimination(player.PlayerId);
            if (creditedInstigator >= 0 && creditedInstigator != player.PlayerId)
                _stats.RecordKnockout(creditedInstigator);

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
            EndedInSuddenDeath = _suddenDeath;
            State = MatchState.Ended;
            WinnerId = winnerId;
            _suddenDeath = false;
            GameAudio.Instance?.PlayWin(Vector3.zero);
            MatchFinished?.Invoke(winnerId);
        }

        public void RestartMatch()
        {
            StopAllCoroutines();
            _scores.Reset();
            _stats.Reset();
            _clock = new MatchClockModel(RuntimeContext.Tuning.MatchDurationSeconds, RuntimeContext.Tuning.OverloadStartSeconds);
            _elapsedSeconds = 0f;
            _suddenDeath = false;
            EndedInSuddenDeath = false;
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
            MatchRestarted?.Invoke();
        }
    }
}

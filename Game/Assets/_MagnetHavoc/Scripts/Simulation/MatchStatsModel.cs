using System.Collections.Generic;

namespace MagnetHavoc.Simulation
{
    public sealed class PlayerMatchStats
    {
        public int PlayerId { get; }
        public int Pushes { get; private set; }
        public int Dashes { get; private set; }
        public int CorePickups { get; private set; }
        public int CoreDrops { get; private set; }
        public int Knockouts { get; private set; }
        public int Eliminations { get; private set; }
        public float PossessionSeconds { get; private set; }
        public float PullSeconds { get; private set; }

        public PlayerMatchStats(int playerId)
        {
            PlayerId = playerId;
        }

        internal void RecordPush() => Pushes++;
        internal void RecordDash() => Dashes++;
        internal void RecordCorePickup() => CorePickups++;
        internal void RecordCoreDrop() => CoreDrops++;
        internal void RecordKnockout() => Knockouts++;
        internal void RecordElimination() => Eliminations++;
        internal void AddPossession(float seconds)
        {
            if (seconds > 0f) PossessionSeconds += seconds;
        }
        internal void AddPull(float seconds)
        {
            if (seconds > 0f) PullSeconds += seconds;
        }
    }

    public sealed class MatchStatsModel
    {
        private readonly Dictionary<int, PlayerMatchStats> _players = new Dictionary<int, PlayerMatchStats>();

        public void Register(int playerId)
        {
            if (!_players.ContainsKey(playerId))
                _players.Add(playerId, new PlayerMatchStats(playerId));
        }

        public PlayerMatchStats Get(int playerId)
        {
            Register(playerId);
            return _players[playerId];
        }

        public void RecordPush(int playerId) => Get(playerId).RecordPush();
        public void RecordDash(int playerId) => Get(playerId).RecordDash();
        public void RecordCorePickup(int playerId) => Get(playerId).RecordCorePickup();
        public void RecordCoreDrop(int playerId) => Get(playerId).RecordCoreDrop();
        public void RecordKnockout(int playerId) => Get(playerId).RecordKnockout();
        public void RecordElimination(int playerId) => Get(playerId).RecordElimination();
        public void AddPossession(int playerId, float seconds) => Get(playerId).AddPossession(seconds);
        public void AddPull(int playerId, float seconds) => Get(playerId).AddPull(seconds);

        public void Reset()
        {
            int[] ids = new int[_players.Count];
            _players.Keys.CopyTo(ids, 0);
            _players.Clear();
            for (int i = 0; i < ids.Length; i++) Register(ids[i]);
        }
    }
}

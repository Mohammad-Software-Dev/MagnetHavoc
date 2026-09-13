using System.Collections.Generic;

namespace MagnetHavoc.Simulation
{
    public sealed class MatchScoreModel
    {
        private readonly Dictionary<int, float> _scores = new Dictionary<int, float>();

        public void Register(int playerId)
        {
            if (!_scores.ContainsKey(playerId)) _scores.Add(playerId, 0f);
        }

        public float AddScore(int playerId, float amount)
        {
            Register(playerId);
            _scores[playerId] += amount > 0f ? amount : 0f;
            return _scores[playerId];
        }

        public float GetScore(int playerId)
        {
            return _scores.TryGetValue(playerId, out float score) ? score : 0f;
        }

        public int GetLeaderId()
        {
            int leader = -1;
            float best = float.MinValue;
            foreach (KeyValuePair<int, float> entry in _scores)
            {
                if (entry.Value > best)
                {
                    best = entry.Value;
                    leader = entry.Key;
                }
            }
            return leader;
        }

        public int GetUniqueLeaderId()
        {
            int leader = -1;
            float best = float.MinValue;
            bool tied = false;

            foreach (KeyValuePair<int, float> entry in _scores)
            {
                if (entry.Value > best)
                {
                    best = entry.Value;
                    leader = entry.Key;
                    tied = false;
                }
                else if (entry.Value == best)
                {
                    tied = true;
                }
            }

            return tied ? -1 : leader;
        }

        public void Reset()
        {
            int[] keys = new int[_scores.Count];
            _scores.Keys.CopyTo(keys, 0);
            for (int i = 0; i < keys.Length; i++) _scores[keys[i]] = 0f;
        }
    }
}

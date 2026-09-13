using System;

namespace MagnetHavoc.Simulation
{
    [Serializable]
    public sealed class MatchClockModel
    {
        private readonly float _duration;
        private readonly float _overloadThreshold;

        public MatchClockModel(float durationSeconds, float overloadThresholdSeconds)
        {
            _duration = Math.Max(0f, durationSeconds);
            _overloadThreshold = Math.Max(0f, overloadThresholdSeconds);
            Reset();
        }

        public float RemainingSeconds { get; private set; }
        public bool IsExpired => RemainingSeconds <= 0f;
        public bool IsOverload => !IsExpired && RemainingSeconds <= _overloadThreshold;

        public void Reset()
        {
            RemainingSeconds = _duration;
        }

        public void Advance(float deltaSeconds)
        {
            if (deltaSeconds <= 0f || IsExpired) return;
            RemainingSeconds = Math.Max(0f, RemainingSeconds - deltaSeconds);
        }
    }
}

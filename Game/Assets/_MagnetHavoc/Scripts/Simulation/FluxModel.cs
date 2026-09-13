namespace MagnetHavoc.Simulation
{
    public sealed class FluxModel
    {
        public float Current { get; private set; }
        public float Max { get; }

        public FluxModel(float max)
        {
            Max = max > 0f ? max : 1f;
            Current = Max;
        }

        public bool TrySpend(float amount)
        {
            if (amount <= 0f) return true;
            if (Current + 0.0001f < amount) return false;
            Current -= amount;
            if (Current < 0f) Current = 0f;
            return true;
        }

        public void Add(float amount)
        {
            Current += amount;
            if (Current < 0f) Current = 0f;
            if (Current > Max) Current = Max;
        }

        public void Reset() => Current = Max;
    }
}

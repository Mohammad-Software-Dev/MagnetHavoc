using MagnetHavoc.Simulation;
using UnityEngine;

namespace MagnetHavoc
{
    public sealed class FluxMeter : MonoBehaviour
    {
        private FluxModel _model;
        public float Current => _model != null ? _model.Current : 0f;
        public float Max => _model != null ? _model.Max : RuntimeContext.Tuning.FluxMax;
        public float Normalized => Max > 0f ? Current / Max : 0f;

        public void Configure()
        {
            _model = new FluxModel(RuntimeContext.Tuning.FluxMax);
        }

        private void Update()
        {
            if (_model != null) _model.Add(RuntimeContext.Tuning.FluxRegenPerSecond * Time.deltaTime);
        }

        public bool TrySpend(float amount) => _model != null && _model.TrySpend(amount);

        public bool TryDrain(float amount)
        {
            return _model != null && _model.TrySpend(amount);
        }

        public void ResetMeter() => _model?.Reset();
    }
}

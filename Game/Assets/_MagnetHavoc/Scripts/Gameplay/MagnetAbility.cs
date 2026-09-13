using System.Collections.Generic;
using UnityEngine;

namespace MagnetHavoc
{
    public sealed class MagnetAbility : MonoBehaviour
    {
        private readonly Collider[] _hits = new Collider[64];
        private readonly HashSet<Rigidbody> _seen = new HashSet<Rigidbody>();
        private PlayerController _owner;
        private FluxMeter _flux;
        private PlayerVisual _visual;
        private bool _pulling;

        public bool IsPulling => _pulling;

        public void Configure(PlayerController owner)
        {
            _owner = owner;
            _flux = owner.GetComponent<FluxMeter>();
            _visual = owner.GetComponent<PlayerVisual>();
        }

        public void ProcessCommand(PlayerCommand command)
        {
            if (_owner == null || _flux == null) return;
            if (command.PushPressed) Push();
            _pulling = command.PullHeld && _flux.Current > 0.1f;
            _visual?.SetMagnetState(_pulling, false);
        }

        private void FixedUpdate()
        {
            if (!_pulling || _owner == null || _flux == null) return;
            float cost = RuntimeContext.Tuning.PullFluxPerSecond * Time.fixedDeltaTime;
            if (!_flux.TryDrain(cost))
            {
                _pulling = false;
                _visual?.SetMagnetState(false, false);
                return;
            }

            MatchManager.Instance?.RecordPull(_owner.PlayerId, Time.fixedDeltaTime);
            ApplyField(false);
        }

        private void Push()
        {
            if (!_flux.TrySpend(RuntimeContext.Tuning.PushFluxCost)) return;
            MatchManager.Instance?.RecordPush(_owner.PlayerId);
            ApplyField(true);
            _visual?.SetMagnetState(false, true);
            GameAudio.Instance?.PlayPush(transform.position);
        }

        private void ApplyField(bool push)
        {
            _seen.Clear();
            float range = RuntimeContext.Tuning.MagnetRange;
            int count = Physics.OverlapSphereNonAlloc(transform.position, range, _hits, ~0, QueryTriggerInteraction.Ignore);

            for (int i = 0; i < count; i++)
            {
                Rigidbody body = _hits[i].attachedRigidbody;
                if (body == null || body == _owner.Body || !_seen.Add(body)) continue;

                Vector3 offset = body.worldCenterOfMass - transform.position;
                float distance = Mathf.Max(0.5f, offset.magnitude);
                if (distance > range) continue;
                float falloff = Mathf.Clamp01(1f - distance / range);
                falloff = 0.25f + falloff * 0.75f;

                CoreObjective core = body.GetComponent<CoreObjective>();
                PlayerController player = body.GetComponent<PlayerController>();

                // A held Core has its own collider/Rigidbody, but force belongs on the carrier.
                // Skip the Core body here; the carrier's player Rigidbody is processed once below.
                if (core != null && core.Holder != null) continue;

                Vector3 direction = push ? offset.normalized : -offset.normalized;
                if (player != null)
                {
                    bool carryingCore = CoreObjective.Instance != null && CoreObjective.Instance.Holder == player;
                    float carrierMultiplier = carryingCore ? RuntimeContext.Tuning.CoreForceMultiplier : 1f;
                    float strength = (push ? RuntimeContext.Tuning.PushImpulse : RuntimeContext.Tuning.PullAcceleration * Time.fixedDeltaTime)
                        * RuntimeContext.Tuning.PlayerForceMultiplier * carrierMultiplier * falloff;
                    player.ApplyExternalImpulse(WithLift(direction, push) * strength, _owner.PlayerId);
                }
                else
                {
                    float multiplier = core != null ? RuntimeContext.Tuning.CoreForceMultiplier : 1f;
                    if (push)
                        body.AddForce(WithLift(direction, true) * RuntimeContext.Tuning.PushImpulse * multiplier * falloff, ForceMode.Impulse);
                    else
                        body.AddForce(direction * RuntimeContext.Tuning.PullAcceleration * multiplier * falloff, ForceMode.Acceleration);
                }
            }
        }

        private static Vector3 WithLift(Vector3 direction, bool push)
        {
            if (!push) return direction;
            direction.y += RuntimeContext.Tuning.PushLift;
            return direction.normalized;
        }
    }
}

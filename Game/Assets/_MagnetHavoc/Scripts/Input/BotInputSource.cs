using UnityEngine;

namespace MagnetHavoc
{
    public sealed class BotInputSource : MonoBehaviour, IPlayerInputSource
    {
        private PlayerController _owner;
        private float _nextDash;
        private float _nextPush;

        public void Configure(PlayerController owner)
        {
            _owner = owner;
            _nextDash = Time.time + 1.1f + owner.PlayerId * 0.27f;
            _nextPush = Time.time + 0.4f + owner.PlayerId * 0.21f;
        }

        public PlayerCommand ReadCommand()
        {
            if (_owner == null || MatchManager.Instance == null || CoreObjective.Instance == null)
                return default;

            CoreObjective core = CoreObjective.Instance;
            Vector3 position = _owner.transform.position;
            Vector3 target;
            bool pull = false;
            bool push = false;

            if (core.Holder == _owner)
            {
                PlayerController threat = MatchManager.Instance.GetNearestOpponent(_owner);
                Vector3 away = threat != null ? position - threat.transform.position : position;
                if (away.sqrMagnitude < 0.1f) away = _owner.transform.forward;
                target = position + away.normalized * 5f;

                if (threat != null && Vector3.Distance(position, threat.transform.position) < 3.2f && Time.time >= _nextPush)
                {
                    push = true;
                    _nextPush = Time.time + 1.15f;
                }
            }
            else if (core.Holder != null)
            {
                target = core.Holder.transform.position;
                float distance = Vector3.Distance(position, target);
                if (distance < 3.4f && Time.time >= _nextPush)
                {
                    push = true;
                    _nextPush = Time.time + 1.0f;
                }
            }
            else
            {
                target = core.transform.position;
                float distance = Vector3.Distance(position, target);
                pull = distance > 1.7f && distance < RuntimeContext.Tuning.MagnetRange;
            }

            Vector3 desired = target - position;
            desired.y = 0f;
            if (desired.sqrMagnitude > 0.001f) desired.Normalize();

            // Prevent all bots from converging on exactly the same line.
            if (desired.sqrMagnitude > 0.001f && core.Holder != _owner)
            {
                float sign = (_owner.PlayerId & 1) == 0 ? 1f : -1f;
                Vector3 lateral = Vector3.Cross(Vector3.up, desired) * (0.12f * sign);
                desired = (desired + lateral).normalized;
            }

            Vector3 planarPosition = new Vector3(position.x, 0f, position.z);
            float radius = planarPosition.magnitude;
            bool nearEdge = radius > RuntimeContext.Tuning.BotEdgeAvoidRadius;
            if (nearEdge && planarPosition.sqrMagnitude > 0.01f)
            {
                float danger = Mathf.InverseLerp(RuntimeContext.Tuning.BotEdgeAvoidRadius, RuntimeContext.Tuning.ArenaSafeRadius, radius);
                Vector3 inward = -planarPosition.normalized;
                desired = Vector3.Slerp(desired, inward, Mathf.Clamp01(0.6f + danger * 0.4f)).normalized;
                pull = false;
            }

            bool dash = false;
            float targetDistance = Vector3.Distance(position, target);
            if (!nearEdge && targetDistance > 4.5f && Time.time >= _nextDash)
            {
                dash = true;
                _nextDash = Time.time + 2.8f + (_owner.PlayerId % 3) * 0.25f;
            }

            return new PlayerCommand
            {
                Move = new Vector2(desired.x, desired.z),
                DashPressed = dash,
                PushPressed = push,
                PullHeld = pull
            };
        }
    }
}

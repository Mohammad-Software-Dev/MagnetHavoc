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
            Vector3 target;
            bool pull = false;
            bool push = false;

            if (core.Holder == _owner)
            {
                PlayerController threat = MatchManager.Instance.GetNearestOpponent(_owner);
                Vector3 away = threat != null ? _owner.transform.position - threat.transform.position : _owner.transform.position;
                target = _owner.transform.position + away.normalized * 5f;
                if (threat != null && Vector3.Distance(_owner.transform.position, threat.transform.position) < 3.2f && Time.time >= _nextPush)
                {
                    push = true;
                    _nextPush = Time.time + 1.15f;
                }
            }
            else if (core.Holder != null)
            {
                target = core.Holder.transform.position;
                float distance = Vector3.Distance(_owner.transform.position, target);
                if (distance < 3.4f && Time.time >= _nextPush)
                {
                    push = true;
                    _nextPush = Time.time + 1.0f;
                }
            }
            else
            {
                target = core.transform.position;
                float distance = Vector3.Distance(_owner.transform.position, target);
                pull = distance > 1.7f && distance < RuntimeContext.Tuning.MagnetRange;
            }

            Vector3 delta = target - _owner.transform.position;
            Vector2 move = new Vector2(delta.x, delta.z).normalized;
            bool dash = false;
            if (delta.magnitude > 4.5f && Time.time >= _nextDash)
            {
                dash = true;
                _nextDash = Time.time + 2.8f + (_owner.PlayerId % 3) * 0.25f;
            }

            return new PlayerCommand { Move = move, DashPressed = dash, PushPressed = push, PullHeld = pull };
        }
    }
}

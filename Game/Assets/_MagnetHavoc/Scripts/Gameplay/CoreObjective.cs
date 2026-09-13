using UnityEngine;

namespace MagnetHavoc
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class CoreObjective : MonoBehaviour
    {
        public static CoreObjective Instance { get; private set; }

        private Rigidbody _body;
        private Vector3 _spawn;
        private float _pickupLockedUntil;

        public PlayerController Holder { get; private set; }
        public bool IsLoose => Holder == null;
        public float PickupLockRemaining => Mathf.Max(0f, _pickupLockedUntil - Time.time);

        private void Awake()
        {
            Instance = this;
            _body = GetComponent<Rigidbody>();
        }

        public void Configure(Vector3 spawn)
        {
            _spawn = spawn;
            ResetToSpawn();
        }

        private void FixedUpdate()
        {
            if (Holder != null)
            {
                Vector3 target = Holder.transform.position
                    + Vector3.up * RuntimeContext.Tuning.CoreCarryHeight
                    + Holder.transform.forward * RuntimeContext.Tuning.CoreCarryForward;
                _body.MovePosition(Vector3.Lerp(_body.position, target, 0.65f));
                _body.MoveRotation(Quaternion.RotateTowards(_body.rotation, Quaternion.identity, 360f * Time.fixedDeltaTime));
                return;
            }

            if (transform.position.y < RuntimeContext.Tuning.CoreResetY)
            {
                ResetToSpawn();
                return;
            }

            if (Time.time < _pickupLockedUntil || MatchManager.Instance == null || MatchManager.Instance.State != MatchState.Playing)
                return;

            PlayerController[] players = MatchManager.Instance.Players;
            float radiusSq = RuntimeContext.Tuning.CorePickupRadius * RuntimeContext.Tuning.CorePickupRadius;
            PlayerController closest = null;
            float closestSq = radiusSq;

            for (int i = 0; i < players.Length; i++)
            {
                PlayerController player = players[i];
                if (player == null || player.IsKnockedOut || !player.gameObject.activeInHierarchy) continue;
                float sq = (player.transform.position - transform.position).sqrMagnitude;
                if (sq < closestSq)
                {
                    closestSq = sq;
                    closest = player;
                }
            }

            if (closest != null) SetHolder(closest);
        }

        private void SetHolder(PlayerController player)
        {
            Holder = player;
            _body.isKinematic = true;
            _body.linearVelocity = Vector3.zero;
            _body.angularVelocity = Vector3.zero;
            GameAudio.Instance?.PlayPickup(transform.position);
        }

        public void NotifyCarrierHit(float impulseMagnitude, int instigatorId)
        {
            if (Holder == null || impulseMagnitude < RuntimeContext.Tuning.CoreCarrierBreakImpulse) return;

            PlayerController carrier = Holder;
            Vector3 velocity = carrier.Body.linearVelocity;
            Vector3 direction = velocity.sqrMagnitude > 0.1f ? velocity.normalized : carrier.transform.forward;
            Vector3 dropVelocity = velocity + direction * RuntimeContext.Tuning.CoreDropSpeed;
            Drop(carrier.transform.position + Vector3.up * 1.1f, dropVelocity);
        }

        public void Drop(Vector3 position, Vector3 velocity)
        {
            Holder = null;
            transform.position = position;
            transform.rotation = Quaternion.identity;
            _body.isKinematic = false;
            _body.linearVelocity = velocity;
            _body.angularVelocity = Vector3.zero;
            _pickupLockedUntil = Time.time + RuntimeContext.Tuning.CorePickupLockout;
        }

        public void ResetToSpawn()
        {
            Holder = null;
            transform.position = _spawn;
            transform.rotation = Quaternion.identity;
            _body.isKinematic = false;
            _body.linearVelocity = Vector3.zero;
            _body.angularVelocity = Vector3.zero;
            _pickupLockedUntil = Time.time + 0.35f;
        }
    }
}

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
                Vector3 target = Holder.transform.position + Vector3.up * 1.35f + Holder.transform.forward * 0.65f;
                transform.position = Vector3.Lerp(transform.position, target, 0.65f);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.identity, 360f * Time.fixedDeltaTime);
                return;
            }

            if (Time.time < _pickupLockedUntil || MatchManager.Instance == null) return;
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
            GameAudio.Instance?.PlayPickup(transform.position);
        }

        public void NotifyCarrierHit(float impulseMagnitude, int instigatorId)
        {
            if (Holder == null || impulseMagnitude < RuntimeContext.Tuning.CoreCarrierBreakImpulse) return;
            Vector3 direction = Holder.Body.linearVelocity.sqrMagnitude > 0.1f ? Holder.Body.linearVelocity.normalized : Holder.transform.forward;
            Drop(Holder.transform.position + Vector3.up * 1.1f, direction * 3f);
        }

        public void Drop(Vector3 position, Vector3 velocity)
        {
            Holder = null;
            transform.position = position;
            _body.isKinematic = false;
            _body.linearVelocity = velocity;
            _pickupLockedUntil = Time.time + RuntimeContext.Tuning.CorePickupLockout;
        }

        public void ResetToSpawn()
        {
            Holder = null;
            transform.position = _spawn;
            _body.isKinematic = false;
            _body.linearVelocity = Vector3.zero;
            _body.angularVelocity = Vector3.zero;
            _pickupLockedUntil = Time.time + 0.35f;
        }
    }
}

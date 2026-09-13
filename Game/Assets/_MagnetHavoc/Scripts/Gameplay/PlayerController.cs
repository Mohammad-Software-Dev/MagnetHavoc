using UnityEngine;

namespace MagnetHavoc
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class PlayerController : MonoBehaviour
    {
        private Rigidbody _body;
        private FluxMeter _flux;
        private MagnetAbility _magnet;
        private IPlayerInputSource _input;
        private PlayerCommand _command;
        private float _dashRemaining;
        private float _dashCooldownRemaining;
        private float _spawnProtectionRemaining;
        private Vector3 _dashDirection;
        private int _lastExternalInstigatorId = -1;
        private float _lastExternalHitTime = -999f;
        private bool _configured;

        public int PlayerId { get; private set; }
        public bool IsLocal { get; private set; }
        public bool IsKnockedOut { get; private set; }
        public Vector3 SpawnPoint { get; private set; }
        public FluxMeter Flux => _flux;
        public Rigidbody Body => _body;
        public PlayerCommand CurrentCommand => _command;
        public float DashCooldownRemaining => Mathf.Max(0f, _dashCooldownRemaining);
        public float SpawnProtectionRemaining => Mathf.Max(0f, _spawnProtectionRemaining);
        public bool IsDashing => _dashRemaining > 0f;
        public bool IsSpawnProtected => _spawnProtectionRemaining > 0f;

        public void Configure(int playerId, bool isLocal, Vector3 spawnPoint, IPlayerInputSource input)
        {
            PlayerId = playerId;
            IsLocal = isLocal;
            SpawnPoint = spawnPoint;
            _input = input;
            _body = GetComponent<Rigidbody>();
            _flux = GetComponent<FluxMeter>();
            _magnet = GetComponent<MagnetAbility>();
            _configured = true;
        }

        private void Update()
        {
            if (_dashCooldownRemaining > 0f) _dashCooldownRemaining -= Time.deltaTime;
            if (_spawnProtectionRemaining > 0f) _spawnProtectionRemaining -= Time.deltaTime;

            if (!_configured || IsKnockedOut || MatchManager.Instance == null || MatchManager.Instance.State != MatchState.Playing)
            {
                _command = default;
                _magnet?.ProcessCommand(default);
                return;
            }

            _command = _input != null ? _input.ReadCommand() : default;
            if (_command.DashPressed) TryStartDash();
            _magnet?.ProcessCommand(_command);
        }

        private void FixedUpdate()
        {
            if (!_configured || IsKnockedOut) return;

            if (transform.position.y < RuntimeContext.Tuning.KnockoutY)
            {
                MatchManager.Instance?.OnPlayerKnockedOut(this);
                return;
            }

            Vector3 planar = new Vector3(_body.linearVelocity.x, 0f, _body.linearVelocity.z);
            Vector3 desiredDirection = new Vector3(_command.Move.x, 0f, _command.Move.y);
            if (desiredDirection.sqrMagnitude > 1f) desiredDirection.Normalize();

            float carrierMultiplier = CoreObjective.Instance != null && CoreObjective.Instance.Holder == this
                ? RuntimeContext.Tuning.CoreCarrierMoveMultiplier
                : 1f;

            if (_dashRemaining > 0f)
            {
                _dashRemaining -= Time.fixedDeltaTime;
                Vector3 dashVelocity = _dashDirection * RuntimeContext.Tuning.DashSpeed;
                _body.linearVelocity = new Vector3(dashVelocity.x, _body.linearVelocity.y, dashVelocity.z);
            }
            else
            {
                Vector3 target = desiredDirection * RuntimeContext.Tuning.MoveSpeed * carrierMultiplier;
                float acceleration = desiredDirection.sqrMagnitude > 0.001f
                    ? RuntimeContext.Tuning.MoveAcceleration
                    : RuntimeContext.Tuning.MoveDeceleration;
                Vector3 next = Vector3.MoveTowards(planar, target, acceleration * Time.fixedDeltaTime);
                _body.linearVelocity = new Vector3(next.x, _body.linearVelocity.y, next.z);
            }

            if (desiredDirection.sqrMagnitude > 0.02f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(desiredDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, RuntimeContext.Tuning.RotationSpeedDegrees * Time.fixedDeltaTime);
            }
        }

        private void TryStartDash()
        {
            if (_dashCooldownRemaining > 0f || _dashRemaining > 0f) return;
            Vector3 direction = new Vector3(_command.Move.x, 0f, _command.Move.y);
            if (direction.sqrMagnitude < 0.05f) direction = transform.forward;
            _dashDirection = direction.normalized;
            _dashRemaining = RuntimeContext.Tuning.DashDuration;
            _dashCooldownRemaining = RuntimeContext.Tuning.DashCooldown;
            MatchManager.Instance?.RecordDash(PlayerId);
            GameAudio.Instance?.PlayDash(transform.position);
        }

        public void ApplyExternalImpulse(Vector3 impulse, int instigatorId)
        {
            if (IsKnockedOut || IsSpawnProtected) return;

            if (instigatorId >= 0 && instigatorId != PlayerId)
            {
                _lastExternalInstigatorId = instigatorId;
                _lastExternalHitTime = Time.time;
            }

            float resistance = IsDashing ? RuntimeContext.Tuning.DashKnockbackMultiplier : 1f;
            Vector3 applied = impulse * resistance;
            _body.AddForce(applied, ForceMode.VelocityChange);

            if (CoreObjective.Instance != null && CoreObjective.Instance.Holder == this)
                CoreObjective.Instance.NotifyCarrierHit(applied.magnitude, instigatorId);
        }

        public int GetRecentInstigator(float windowSeconds)
        {
            if (_lastExternalInstigatorId < 0 || windowSeconds < 0f) return -1;
            return Time.time - _lastExternalHitTime <= windowSeconds ? _lastExternalInstigatorId : -1;
        }

        public void MarkKnockedOut()
        {
            if (IsKnockedOut) return;
            IsKnockedOut = true;
            if (CoreObjective.Instance != null && CoreObjective.Instance.Holder == this)
                CoreObjective.Instance.Drop(transform.position + Vector3.up, Vector3.zero);
            gameObject.SetActive(false);
        }

        public void Respawn(Vector3 position)
        {
            gameObject.SetActive(true);
            IsKnockedOut = false;
            transform.position = position;
            transform.rotation = Quaternion.identity;
            _body.isKinematic = false;
            _body.linearVelocity = Vector3.zero;
            _body.angularVelocity = Vector3.zero;
            _dashRemaining = 0f;
            _dashCooldownRemaining = 0f;
            _spawnProtectionRemaining = RuntimeContext.Tuning.SpawnProtectionSeconds;
            _lastExternalInstigatorId = -1;
            _lastExternalHitTime = -999f;
            _flux?.ResetMeter();
        }
    }
}

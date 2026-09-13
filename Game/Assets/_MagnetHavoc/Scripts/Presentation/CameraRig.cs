using UnityEngine;

namespace MagnetHavoc
{
    public sealed class CameraRig : MonoBehaviour
    {
        private Transform _target;
        private Vector3 _velocity;
        private readonly Vector3 _offset = new Vector3(0f, 12.5f, -10.5f);

        public void Configure(Transform target) => _target = target;

        private void LateUpdate()
        {
            if (_target == null) return;
            Vector3 focus = _target.position;
            if (CoreObjective.Instance != null)
            {
                Vector3 towardCore = CoreObjective.Instance.transform.position - focus;
                focus += Vector3.ClampMagnitude(towardCore, 3.2f) * 0.22f;
            }
            Vector3 desired = focus + _offset;
            transform.position = Vector3.SmoothDamp(transform.position, desired, ref _velocity, 0.16f);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(focus + Vector3.up * 0.7f - transform.position), Time.deltaTime * 8f);
        }
    }
}

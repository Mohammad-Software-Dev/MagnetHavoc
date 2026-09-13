using UnityEngine;

namespace MagnetHavoc
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class RotatingHazard : MonoBehaviour
    {
        public float DegreesPerSecond = 72f;
        private Rigidbody _body;

        private void Awake()
        {
            _body = GetComponent<Rigidbody>();
            _body.isKinematic = true;
        }

        private void FixedUpdate()
        {
            Quaternion rotation = Quaternion.Euler(0f, DegreesPerSecond * Time.fixedDeltaTime, 0f) * _body.rotation;
            _body.MoveRotation(rotation);
        }
    }
}

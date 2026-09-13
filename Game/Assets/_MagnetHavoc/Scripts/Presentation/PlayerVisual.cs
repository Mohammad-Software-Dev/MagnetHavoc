using UnityEngine;

namespace MagnetHavoc
{
    public sealed class PlayerVisual : MonoBehaviour
    {
        private Transform _visualRoot;
        private Transform _leftArm;
        private Transform _rightArm;
        private float _pulse;

        public void Configure(Color color)
        {
            Renderer physicsRenderer = GetComponent<Renderer>();
            if (physicsRenderer != null) physicsRenderer.enabled = false;

            GameObject visualRootObject = new GameObject("VisualRoot");
            _visualRoot = visualRootObject.transform;
            _visualRoot.SetParent(transform, false);

            CreatePart(PrimitiveType.Capsule, "Body", new Vector3(0f, 0f, 0f), new Vector3(0.9f, 1f, 0.9f), color);
            Transform head = CreatePart(PrimitiveType.Sphere, "Head", new Vector3(0f, 0.72f, 0.02f), new Vector3(0.62f, 0.42f, 0.62f), color * 1.1f);
            _leftArm = CreatePart(PrimitiveType.Cube, "LeftMagnet", new Vector3(-0.58f, 0.1f, 0.08f), new Vector3(0.28f, 0.28f, 0.52f), Color.cyan);
            _rightArm = CreatePart(PrimitiveType.Cube, "RightMagnet", new Vector3(0.58f, 0.1f, 0.08f), new Vector3(0.28f, 0.28f, 0.52f), Color.magenta);
            head.localRotation = Quaternion.Euler(-8f, 0f, 0f);
        }

        private Transform CreatePart(PrimitiveType type, string partName, Vector3 localPosition, Vector3 localScale, Color color)
        {
            GameObject part = GameObject.CreatePrimitive(type);
            part.name = partName;
            part.transform.SetParent(_visualRoot, false);
            part.transform.localPosition = localPosition;
            part.transform.localScale = localScale;
            Collider collider = part.GetComponent<Collider>();
            if (collider != null) Destroy(collider);
            Renderer renderer = part.GetComponent<Renderer>();
            if (renderer != null) renderer.material = PrototypeBootstrap.CreateMaterial(color, 0.45f);
            return part.transform;
        }

        public void SetMagnetState(bool pulling, bool pushed)
        {
            if (pulling) _pulse = Mathf.Max(_pulse, 0.45f);
            if (pushed) _pulse = 1f;
        }

        private void LateUpdate()
        {
            if (_visualRoot == null) return;

            Rigidbody body = GetComponent<Rigidbody>();
            float lean = 0f;
            if (body != null)
            {
                Vector3 localVelocity = transform.InverseTransformDirection(body.linearVelocity);
                lean = Mathf.Clamp(-localVelocity.z * 1.1f, -10f, 10f);
            }

            _visualRoot.localRotation = Quaternion.Lerp(_visualRoot.localRotation, Quaternion.Euler(lean, 0f, 0f), Time.deltaTime * 10f);
            _pulse = Mathf.MoveTowards(_pulse, 0f, Time.deltaTime * 2.8f);
            float armAngle = Mathf.Sin(Time.time * 18f) * 8f * _pulse;
            if (_leftArm != null) _leftArm.localRotation = Quaternion.Euler(0f, -armAngle, -15f * _pulse);
            if (_rightArm != null) _rightArm.localRotation = Quaternion.Euler(0f, armAngle, 15f * _pulse);
        }
    }
}

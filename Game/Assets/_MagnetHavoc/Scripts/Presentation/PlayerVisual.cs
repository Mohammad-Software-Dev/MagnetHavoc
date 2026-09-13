using UnityEngine;

namespace MagnetHavoc
{
    public sealed class PlayerVisual : MonoBehaviour
    {
        private Transform _visualRoot;
        private Transform _leftArm;
        private Transform _rightArm;
        private Transform _shieldMarker;
        private Transform _coreMarker;
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
            _shieldMarker = CreatePart(PrimitiveType.Cylinder, "SpawnShieldMarker", new Vector3(0f, -0.94f, 0f), new Vector3(1.05f, 0.025f, 1.05f), Color.cyan);
            _coreMarker = CreatePart(PrimitiveType.Cylinder, "CoreHolderMarker", new Vector3(0f, 1.32f, 0f), new Vector3(0.38f, 0.035f, 0.38f), new Color(0.75f, 0.2f, 1f));
            head.localRotation = Quaternion.Euler(-8f, 0f, 0f);
            _shieldMarker.gameObject.SetActive(false);
            _coreMarker.gameObject.SetActive(false);
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

            PlayerController player = GetComponent<PlayerController>();
            if (player != null && _shieldMarker != null)
            {
                bool active = player.IsSpawnProtected;
                if (_shieldMarker.gameObject.activeSelf != active) _shieldMarker.gameObject.SetActive(active);
                if (active)
                {
                    float pulseScale = 1f + Mathf.Sin(Time.time * 9f) * 0.08f;
                    _shieldMarker.localScale = new Vector3(1.05f * pulseScale, 0.025f, 1.05f * pulseScale);
                    _shieldMarker.Rotate(0f, 100f * Time.deltaTime, 0f, Space.Self);
                }
            }

            if (player != null && _coreMarker != null)
            {
                bool holdingCore = CoreObjective.Instance != null && CoreObjective.Instance.Holder == player;
                if (_coreMarker.gameObject.activeSelf != holdingCore) _coreMarker.gameObject.SetActive(holdingCore);
                if (holdingCore) _coreMarker.Rotate(0f, 140f * Time.deltaTime, 0f, Space.Self);
            }
        }
    }
}

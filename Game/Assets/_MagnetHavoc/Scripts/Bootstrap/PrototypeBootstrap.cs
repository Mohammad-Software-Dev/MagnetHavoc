using UnityEngine;

namespace MagnetHavoc
{
    public sealed class PrototypeBootstrap : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AutoBoot()
        {
            if (FindAnyObjectByType<PrototypeBootstrap>() != null) return;
            GameObject root = new GameObject("MagnetHavoc_Prototype");
            root.AddComponent<PrototypeBootstrap>();
        }

        private void Start()
        {
            RuntimeContext.Tuning = GameTuning.CreateDefault();
            Physics.gravity = new Vector3(0f, -18f, 0f);
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;

            if (FindAnyObjectByType<GameAudio>() == null) new GameObject("Audio").AddComponent<GameAudio>();
            CreateLighting();
            CreateArena();

            Vector3[] spawns =
            {
                new Vector3(-5.4f, 1.2f, -5.4f),
                new Vector3(5.4f, 1.2f, -5.4f),
                new Vector3(-5.4f, 1.2f, 5.4f),
                new Vector3(5.4f, 1.2f, 5.4f)
            };

            PlayerController[] players = new PlayerController[4];
            Color[] colors = { new Color(0.1f, 0.85f, 1f), new Color(1f, 0.25f, 0.35f), new Color(1f, 0.75f, 0.1f), new Color(0.45f, 1f, 0.35f) };
            for (int i = 0; i < players.Length; i++) players[i] = CreatePlayer(i, i == 0, spawns[i], colors[i]);

            CoreObjective core = CreateCore(Vector3.up * 1.2f);
            MatchManager match = new GameObject("MatchManager").AddComponent<MatchManager>();
            match.Configure(players, spawns, core);

            ConfigureCamera(players[0]);
            PrototypeHud hud = new GameObject("PrototypeHUD").AddComponent<PrototypeHud>();
            hud.Configure(players[0]);
        }

        private static PlayerController CreatePlayer(int id, bool local, Vector3 position, Color color)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            go.name = local ? "Player_Local" : $"Bot_{id}";
            go.transform.position = position;
            go.transform.localScale = new Vector3(0.9f, 1f, 0.9f);

            Rigidbody body = go.AddComponent<Rigidbody>();
            body.mass = 1.2f;
            body.linearDamping = 4f;
            body.angularDamping = 8f;
            body.freezeRotation = true;
            body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            FluxMeter flux = go.AddComponent<FluxMeter>();
            flux.Configure();
            PlayerController player = go.AddComponent<PlayerController>();
            MagnetAbility magnet = go.AddComponent<MagnetAbility>();
            PlayerVisual visual = go.AddComponent<PlayerVisual>();

            IPlayerInputSource input;
            if (local)
            {
                input = go.AddComponent<HumanInputSource>();
            }
            else
            {
                BotInputSource bot = go.AddComponent<BotInputSource>();
                input = bot;
            }

            player.Configure(id, local, position, input);
            magnet.Configure(player);
            visual.Configure(color);
            if (input is BotInputSource botInput) botInput.Configure(player);
            return player;
        }

        private static CoreObjective CreateCore(Vector3 position)
        {
            GameObject core = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            core.name = "FluxCore";
            core.transform.position = position;
            core.transform.localScale = Vector3.one * 0.85f;
            Renderer renderer = core.GetComponent<Renderer>();
            renderer.material = CreateMaterial(new Color(0.75f, 0.2f, 1f), 0.9f);
            Rigidbody body = core.AddComponent<Rigidbody>();
            body.mass = 0.8f;
            body.linearDamping = 1.2f;
            body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            CoreObjective objective = core.AddComponent<CoreObjective>();
            objective.Configure(position);
            return objective;
        }

        private static void CreateArena()
        {
            GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.name = "ArenaFloor";
            floor.transform.position = new Vector3(0f, -0.5f, 0f);
            floor.transform.localScale = new Vector3(17f, 1f, 17f);
            floor.GetComponent<Renderer>().material = CreateMaterial(new Color(0.08f, 0.11f, 0.16f), 0.05f);

            GameObject center = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            center.name = "CorePad";
            center.transform.position = new Vector3(0f, 0.05f, 0f);
            center.transform.localScale = new Vector3(2.5f, 0.12f, 2.5f);
            center.GetComponent<Renderer>().material = CreateMaterial(new Color(0.2f, 0.1f, 0.32f), 0.55f);

            for (int i = 0; i < 8; i++)
            {
                float angle = i * Mathf.PI * 2f / 8f;
                Vector3 pos = new Vector3(Mathf.Cos(angle), 0.65f, Mathf.Sin(angle)) * 4.2f;
                GameObject crate = GameObject.CreatePrimitive(PrimitiveType.Cube);
                crate.name = $"MagneticCrate_{i}";
                crate.transform.position = pos;
                crate.transform.localScale = Vector3.one * (i % 3 == 0 ? 1.25f : 0.9f);
                crate.GetComponent<Renderer>().material = CreateMaterial(new Color(0.22f, 0.28f, 0.34f), 0.35f);
                Rigidbody rb = crate.AddComponent<Rigidbody>();
                rb.mass = i % 3 == 0 ? 2.5f : 1.3f;
                rb.linearDamping = 0.7f;
                rb.angularDamping = 1.5f;
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            }

            GameObject spinner = GameObject.CreatePrimitive(PrimitiveType.Cube);
            spinner.name = "FluxSpinner";
            spinner.transform.position = new Vector3(0f, 0.75f, 0f);
            spinner.transform.localScale = new Vector3(7f, 0.35f, 0.45f);
            spinner.GetComponent<Renderer>().material = CreateMaterial(new Color(0.1f, 0.7f, 0.8f), 0.65f);
            spinner.AddComponent<Rigidbody>();
            spinner.AddComponent<RotatingHazard>();

            // Edge bumpers create readable cover while leaving open knockout lanes.
            for (int i = 0; i < 4; i++)
            {
                Vector3 pos = i < 2 ? new Vector3(i == 0 ? -6.3f : 6.3f, 0.55f, 0f) : new Vector3(0f, 0.55f, i == 2 ? -6.3f : 6.3f);
                GameObject cover = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cover.name = $"Cover_{i}";
                cover.transform.position = pos;
                cover.transform.localScale = i < 2 ? new Vector3(1f, 1.1f, 3.2f) : new Vector3(3.2f, 1.1f, 1f);
                cover.GetComponent<Renderer>().material = CreateMaterial(new Color(0.14f, 0.18f, 0.24f), 0.15f);
            }
        }

        private static void CreateLighting()
        {
            if (FindAnyObjectByType<Light>() != null) return;
            GameObject lightObject = new GameObject("Sun");
            Light light = lightObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.2f;
            light.color = new Color(0.9f, 0.95f, 1f);
            lightObject.transform.rotation = Quaternion.Euler(52f, -35f, 0f);
            RenderSettings.ambientLight = new Color(0.28f, 0.32f, 0.42f);
        }

        private static void ConfigureCamera(PlayerController target)
        {
            Camera camera = Camera.main;
            if (camera == null)
            {
                GameObject cameraObject = new GameObject("Main Camera");
                cameraObject.tag = "MainCamera";
                camera = cameraObject.AddComponent<Camera>();
                cameraObject.AddComponent<AudioListener>();
            }
            camera.fieldOfView = 58f;
            camera.nearClipPlane = 0.15f;
            camera.farClipPlane = 150f;
            CameraRig rig = camera.GetComponent<CameraRig>();
            if (rig == null) rig = camera.gameObject.AddComponent<CameraRig>();
            camera.transform.position = target.transform.position + new Vector3(0f, 12.5f, -10.5f);
            rig.Configure(target.transform);
        }

        public static Material CreateMaterial(Color color, float emission)
        {
            Shader shader = Shader.Find("Standard");
            if (shader == null) shader = Shader.Find("Universal Render Pipeline/Lit");
            Material material = new Material(shader);
            material.color = color;
            if (material.HasProperty("_Metallic")) material.SetFloat("_Metallic", 0.55f);
            if (material.HasProperty("_Smoothness")) material.SetFloat("_Smoothness", 0.65f);
            if (emission > 0f && material.HasProperty("_EmissionColor"))
            {
                material.EnableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", color * emission);
            }
            return material;
        }
    }
}

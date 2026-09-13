using UnityEngine;

namespace MagnetHavoc
{
    public sealed class PrototypeHud : MonoBehaviour
    {
        private PlayerController _local;
        private GUIStyle _title;
        private GUIStyle _label;
        private GUIStyle _small;

        public void Configure(PlayerController local) => _local = local;

        private void EnsureStyles()
        {
            if (_title != null) return;
            _title = new GUIStyle(GUI.skin.label) { fontSize = 26, fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter };
            _label = new GUIStyle(GUI.skin.label) { fontSize = 18, fontStyle = FontStyle.Bold };
            _small = new GUIStyle(GUI.skin.label) { fontSize = 14 };
        }

        private void OnGUI()
        {
            EnsureStyles();
            MatchManager match = MatchManager.Instance;
            if (match == null || _local == null) return;

            float scale = Mathf.Clamp(Screen.height / 720f, 0.75f, 1.5f);
            Matrix4x4 old = GUI.matrix;
            GUI.matrix = Matrix4x4.Scale(new Vector3(scale, scale, 1f));
            float width = Screen.width / scale;
            float height = Screen.height / scale;

            string overload = match.IsOverload ? "  OVERLOAD x2" : string.Empty;
            GUI.Label(new Rect(width * 0.5f - 220f, 12f, 440f, 40f), $"{Mathf.CeilToInt(match.RemainingSeconds)}s{overload}", _title);
            GUI.Label(new Rect(18f, 16f, 300f, 30f), $"YOU  {match.GetScore(_local.PlayerId):0}", _label);
            GUI.Label(new Rect(18f, 48f, 300f, 30f), $"FLUX  {_local.Flux.Current:0}/{_local.Flux.Max:0}", _label);

            string dash = _local.DashCooldownRemaining <= 0.01f ? "READY" : $"{_local.DashCooldownRemaining:0.0}s";
            GUI.Label(new Rect(18f, 80f, 300f, 26f), $"DASH  {dash}", _small);
            if (_local.IsSpawnProtected)
                GUI.Label(new Rect(18f, 104f, 300f, 26f), $"RESPAWN SHIELD  {_local.SpawnProtectionRemaining:0.0}s", _small);

            CoreObjective core = CoreObjective.Instance;
            string coreText = "CORE  LOOSE";
            if (core != null && core.Holder != null)
                coreText = core.Holder == _local ? "CORE  YOU" : $"CORE  P{core.Holder.PlayerId + 1}";
            GUI.Label(new Rect(width * 0.5f - 120f, 50f, 240f, 30f), coreText, _label);

            for (int i = 0; i < match.Players.Length; i++)
            {
                PlayerController player = match.Players[i];
                bool hasCore = core != null && core.Holder == player;
                string marker = hasCore ? " *" : string.Empty;
                string ko = player.IsKnockedOut ? " KO" : string.Empty;
                GUI.Label(new Rect(width - 190f, 12f + i * 25f, 170f, 24f), $"P{player.PlayerId + 1}: {match.GetScore(player.PlayerId):0}{marker}{ko}", _label);
            }

            GUI.Label(new Rect(18f, height - 42f, 500f, 30f), "WASD • Space dash • LMB Push • RMB/Shift Pull", GUI.skin.label);
            DrawTouchHints(width, height);

            if (match.State == MatchState.Ended)
            {
                GUI.Box(new Rect(width * 0.5f - 180f, height * 0.5f - 90f, 360f, 180f), string.Empty);
                string result = match.WinnerId == _local.PlayerId ? "VICTORY!" : $"P{match.WinnerId + 1} WINS";
                GUI.Label(new Rect(width * 0.5f - 160f, height * 0.5f - 60f, 320f, 50f), result, _title);
                if (GUI.Button(new Rect(width * 0.5f - 90f, height * 0.5f + 15f, 180f, 52f), "REMATCH") || Input.GetKeyDown(KeyCode.R))
                    match.RestartMatch();
            }

            GUI.matrix = old;
        }

        private static void DrawTouchHints(float width, float height)
        {
            if (!Application.isMobilePlatform) return;
            GUI.Box(new Rect(28f, height - 210f, 180f, 180f), "MOVE");
            GUI.Box(new Rect(width - 180f, height - 150f, 130f, 120f), "TAP PUSH\nHOLD PULL");
            GUI.Box(new Rect(width - 300f, height - 105f, 80f, 75f), "DASH");
        }
    }
}

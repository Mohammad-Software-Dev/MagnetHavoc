using UnityEngine;

namespace MagnetHavoc
{
    public sealed class PrototypeHud : MonoBehaviour
    {
        private PlayerController _local;
        private GUIStyle _title;
        private GUIStyle _label;

        public void Configure(PlayerController local) => _local = local;

        private void EnsureStyles()
        {
            if (_title != null) return;
            _title = new GUIStyle(GUI.skin.label) { fontSize = 26, fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter };
            _label = new GUIStyle(GUI.skin.label) { fontSize = 18, fontStyle = FontStyle.Bold };
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

            string overload = match.IsOverload ? "  ⚡ OVERLOAD x2" : string.Empty;
            GUI.Label(new Rect(width * 0.5f - 200f, 12f, 400f, 40f), $"{Mathf.CeilToInt(match.RemainingSeconds)}s{overload}", _title);
            GUI.Label(new Rect(18f, 16f, 280f, 30f), $"YOU  {match.GetScore(_local.PlayerId):0}", _label);
            GUI.Label(new Rect(18f, 48f, 280f, 30f), $"FLUX  {_local.Flux.Current:0}/{_local.Flux.Max:0}", _label);

            for (int i = 0; i < match.Players.Length; i++)
            {
                PlayerController player = match.Players[i];
                GUI.Label(new Rect(width - 170f, 12f + i * 25f, 150f, 24f), $"P{player.PlayerId + 1}: {match.GetScore(player.PlayerId):0}", _label);
            }

            GUI.Label(new Rect(18f, height - 42f, 400f, 30f), "WASD • Space dash • LMB Push • RMB/Shift Pull", GUI.skin.label);
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

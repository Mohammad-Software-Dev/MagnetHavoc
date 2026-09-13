using UnityEngine;

namespace MagnetHavoc
{
    public static class TuningProvider
    {
        private const string ResourceName = "game_tuning";

        public static GameTuning Load()
        {
            GameTuning fallback = GameTuning.CreateDefault();
            TextAsset asset = Resources.Load<TextAsset>(ResourceName);
            if (asset == null || string.IsNullOrWhiteSpace(asset.text))
                return fallback;

            try
            {
                GameTuning candidate = JsonUtility.FromJson<GameTuning>(asset.text);
                if (TuningRules.IsValid(candidate))
                    return candidate;

                Debug.LogWarning("Magnet Havoc tuning resource is invalid. Falling back to compiled defaults.");
            }
            catch (System.Exception exception)
            {
                Debug.LogWarning($"Failed to parse Magnet Havoc tuning resource: {exception.Message}. Falling back to defaults.");
            }

            return fallback;
        }
    }
}

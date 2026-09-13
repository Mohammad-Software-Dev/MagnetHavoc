#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace MagnetHavoc.Editor
{
    public static class PrototypeSceneSetup
    {
        private const string SceneFolder = "Assets/_MagnetHavoc/Scenes";
        private const string ScenePath = SceneFolder + "/Prototype.unity";

        [MenuItem("Tools/Magnet Havoc/Create Prototype Scene")]
        public static void CreatePrototypeScene()
        {
            if (!Directory.Exists(SceneFolder)) Directory.CreateDirectory(SceneFolder);
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            EditorSceneManager.SaveScene(scene, ScenePath);
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
            AssetDatabase.Refresh();
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<SceneAsset>(ScenePath);
        }
    }
}
#endif

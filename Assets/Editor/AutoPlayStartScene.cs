#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// Ensures Play mode always starts from the StartScene
/// by setting EditorSceneManager.playModeStartScene on load.
/// </summary>
[InitializeOnLoad]
public static class AutoPlayStartScene
{
    private const string StartScenePath = "Assets/Scenes/StartScene.unity";

    static AutoPlayStartScene()
    {
        var startSceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(StartScenePath);
        if (startSceneAsset == null)
        {
            Debug.LogWarning($"[EDITOR] Could not find StartScene at '{StartScenePath}'. Play mode start scene not set.");
            return;
        }

        if (EditorSceneManager.playModeStartScene != startSceneAsset)
        {
            EditorSceneManager.playModeStartScene = startSceneAsset;
            Debug.Log("[EDITOR] Play mode will always begin from StartScene.");
        }
    }
}
#endif


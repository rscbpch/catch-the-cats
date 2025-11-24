using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string startSceneName = "StartScene";
    [SerializeField] private string introductionSceneName = "IntroductionScene";
    [SerializeField] private string gameSceneName = "GameScene";

    public void LoadStartScene()
    {
        LoadScene(startSceneName);
    }

    public void LoadIntroductionScene()
    {
        LoadScene(introductionSceneName);
    }

    public void LoadGameScene()
    {
        LoadScene(gameSceneName);
    }

    private void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("[SCENE] Scene name is empty, aborting load.");
            return;
        }

        Debug.Log($"[SCENE] Loading scene: {sceneName}");
        Time.timeScale = 1f; 
        SceneManager.LoadScene(sceneName);
    }
}


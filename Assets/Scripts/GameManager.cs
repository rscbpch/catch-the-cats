using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("Game Settings")]
    public float gameDuration = 30f;
    [SerializeField] private string winSceneName = "WinScene";
    [SerializeField] private string loseSceneName = "LoseScene";
    
    [Header("Game State")]
    public int missedCats = 0;
    public int maxMisses = 3;
    public bool gameOver = false;
    public bool gameWon = false;
    
    private float timeRemaining;
    private bool gameStarted = false;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        timeRemaining = gameDuration;
        gameStarted = true;
    }
    
    private void Update()
    {
        if (gameStarted && !gameOver && !gameWon)
        {
            timeRemaining -= Time.deltaTime;
            
            if (timeRemaining <= 0)
            {
                WinGame();
            }
        }
    }
    
    public void MissCat()
    {
        if (gameOver || gameWon) return;
        
        missedCats++;
        UIManager.Instance?.UpdateMisses(missedCats);
        
        if (missedCats >= maxMisses)
        {
            GameOver("You missed too many cats!");
        }
    }
    
    public void CatchBomb()
    {
        if (gameOver || gameWon) return;
        
        GameOver("You caught a bomb!");
    }
    
    public void GameOver(string reason)
    {
        if (gameOver) return;
        
        gameOver = true;
        UIManager.Instance?.ShowGameOver(reason);
        
        if (!LoadSceneIfConfigured(loseSceneName))
        {
            Time.timeScale = 0f;
        }
    }
    
    public void WinGame()
    {
        if (gameWon) return;
        
        gameWon = true;
        UIManager.Instance?.ShowWin();
        
        if (!LoadSceneIfConfigured(winSceneName))
        {
            Time.timeScale = 0f;
        }
    }
    
    public float GetTimeRemaining()
    {
        return timeRemaining;
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private bool LoadSceneIfConfigured(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            return false;
        }

        Debug.Log($"[GAME] Loading scene '{sceneName}'");
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
        return true;
    }
}
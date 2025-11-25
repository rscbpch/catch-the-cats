using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("Game Settings")]
    public float gameDuration = 15f;
    [SerializeField] private string winSceneName = "WinScene";

    [Header("Lose Scenes")]
    [SerializeField] private string catLoseSceneName = "CatLoseScene";
    [SerializeField] private string boomLoseSceneName = "BoomLoseScene";

    [Header("Game State")]
    public int missedCats = 0;
    public int maxMisses = 3;
    public bool gameOver = false;
    public bool gameWon = false;
    
    private float timeRemaining;
    private bool gameStarted = false;
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
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
            if (timeRemaining <= 0) WinGame();
        }
    }
    
    public void MissCat()
    {
        if (gameOver || gameWon) return;

        missedCats++;
        UIManager.Instance?.UpdateMisses(missedCats);

        if (missedCats >= maxMisses)
        {
            GameOver(catLoseSceneName);
        }
    }

    public void CatchBomb()
    {
        if (gameOver || gameWon) return;

        GameOver(boomLoseSceneName);
    }

    public void GameOver(string loseSceneToLoad)
    {
        if (gameOver) return;

        gameOver = true;

        UIManager.Instance?.ShowGameOver("Game Over!");

        Time.timeScale = 1f;
        SceneManager.LoadScene(loseSceneToLoad);
    }

    public void WinGame()
    {
        if (gameWon) return;

        gameWon = true;
        UIManager.Instance?.ShowWin();

        Time.timeScale = 1f;
        SceneManager.LoadScene(winSceneName);
    }
    
    public float GetTimeRemaining() => timeRemaining;

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
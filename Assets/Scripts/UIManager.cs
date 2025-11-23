using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    
    [Header("UI Elements")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI missesText;
    public GameObject gameOverPanel;
    public GameObject winPanel;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    
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
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
        if (restartButton != null) restartButton.onClick.AddListener(RestartGame);
    }
    
    private void Update()
    {
        if (GameManager.Instance != null && timerText != null)
        {
            float timeRemaining = GameManager.Instance.GetTimeRemaining();
            timerText.text = $"Time: {Mathf.Ceil(timeRemaining)}s";
        }
    }
    
    public void UpdateMisses(int misses)
    {
        if (missesText != null)
        {
            missesText.text = $"Misses: {misses}/3";
        }
    }
    
    public void ShowGameOver(string reason)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            if (gameOverText != null)
            {
                gameOverText.text = reason;
            }
        }
    }
    
    public void ShowWin()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }
    }
    
    private void RestartGame()
    {
        GameManager.Instance?.RestartGame();
    }
}
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [Header("Falling Settings")]
    public float fallSpeed = 5f;
    public float destroyY = -6f;
    
    [Header("Object Type")]
    public bool isBomb = false;
    
    private void Update()
    {
        if (GameManager.Instance != null && (GameManager.Instance.gameOver || GameManager.Instance.gameWon))
        {
            return;
        }
        
        // Move downward
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
        
        // Destroy if below screen
        if (transform.position.y < destroyY)
        {
            OnMissed();
        }
    }
    
    public void OnCaught()
    {
        if (isBomb)
        {
            GameManager.Instance?.CatchBomb();
        }
        // If it's a cat, just destroy it (successful catch)
        
        Destroy(gameObject);
    }
    
    private void OnMissed()
    {
        if (!isBomb)
        {
            GameManager.Instance?.MissCat();
        }
        
        Destroy(gameObject);
    }
}
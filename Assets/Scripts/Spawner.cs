using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] catPrefabs; // Array of 5 cat prefabs
    public GameObject bombPrefab;
    public float spawnIntervalMin = 0.5f;
    public float spawnIntervalMax = 2f;
    public float spawnXMin = -7f;
    public float spawnXMax = 7f;
    public float spawnY = 6f;
    
    [Header("Parent Transform")]
    public RectTransform parentRectTransform; // Assign MainPanel here (use RectTransform instead of Transform)
    
    [Header("Spawn Weights")]
    [Range(0, 100)]
    public int bombSpawnChance = 10; // 10% chance to spawn bomb
    
    [Header("Rendering")]
    public int fallingObjectSortingOrder = -1; // Lower than box (box should be 0 or higher)
    
    private void Start()
    {
        StartCoroutine(SpawnObjects());
    }
    
    private IEnumerator SpawnObjects()
    {
        while (true)
        {
            if (GameManager.Instance != null && 
                !GameManager.Instance.gameOver && 
                !GameManager.Instance.gameWon)
            {
                SpawnRandomObject();
            }
            
            float waitTime = Random.Range(spawnIntervalMin, spawnIntervalMax);
            yield return new WaitForSeconds(waitTime);
        }
    }
    
    private void SpawnRandomObject()
    {
        if (parentRectTransform == null)
        {
            Debug.LogWarning("Parent RectTransform is not assigned!");
            return;
        }
        
        // Get the size of the MainPanel
        float panelWidth = parentRectTransform.rect.width;
        float panelHeight = parentRectTransform.rect.height;
        
        // Calculate spawn position in local coordinates
        // X: from left edge to right edge of panel
        float randomX = Random.Range(-panelWidth / 2f, panelWidth / 2f);
        // Y: at the top of the panel
        float spawnY = panelHeight / 2f;
        
        Vector3 localSpawnPosition = new Vector3(randomX, spawnY, 0);
        
        // Decide if spawning bomb or cat
        bool spawnBomb = Random.Range(0, 100) < bombSpawnChance;
        
        GameObject objectToSpawn;
        
        if (spawnBomb && bombPrefab != null)
        {
            objectToSpawn = Instantiate(bombPrefab, parentRectTransform);
            SetSortingOrder(objectToSpawn);
            RectTransform rectTransform = objectToSpawn.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.localPosition = localSpawnPosition;
            }
            else
            {
                // If it's not a UI element, convert to world position
                objectToSpawn.transform.localPosition = localSpawnPosition;
            }
        }
        else if (catPrefabs != null && catPrefabs.Length > 0)
        {
            // Randomly select a cat type
            int randomCatIndex = Random.Range(0, catPrefabs.Length);
            objectToSpawn = Instantiate(catPrefabs[randomCatIndex], parentRectTransform);
            SetSortingOrder(objectToSpawn);
            RectTransform rectTransform = objectToSpawn.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.localPosition = localSpawnPosition;
            }
            else
            {
                // If it's not a UI element, convert to world position
                objectToSpawn.transform.localPosition = localSpawnPosition;
            }
        }
        else
        {
            return; // No prefabs assigned
        }
    }
    
    private void SetSortingOrder(GameObject obj)
    {
        // Set sorting order for SpriteRenderer
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingOrder = fallingObjectSortingOrder;
        }
        
        // If using UI Image, set sibling index (lower index = renders behind)
        // Objects spawned later will have higher index, so they render on top
        // But we want them behind the box, so we can set a lower index
        RectTransform rectTransform = obj.GetComponent<RectTransform>();
        if (rectTransform != null && parentRectTransform != null)
        {
            // Set sibling index to be before the box
            // Find box in hierarchy and place before it
            Transform boxTransform = parentRectTransform.Find("box");
            if (boxTransform != null)
            {
                int boxIndex = boxTransform.GetSiblingIndex();
                obj.transform.SetSiblingIndex(boxIndex);
            }
        }
    }
}
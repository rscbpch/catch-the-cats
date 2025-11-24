using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] catPrefabs; 
    public GameObject bombPrefab;
    public float spawnIntervalMin = 0.5f;
    public float spawnIntervalMax = 2f;
    public float spawnXMin = -7f;
    public float spawnXMax = 7f;
    public float spawnY = 6f;
    
    [Header("Parent Transform")]
    public RectTransform parentRectTransform;
    
    [Header("Spawn Weights")]
    [Range(0, 100)]
    public int bombSpawnChance = 30; 
    
    [Header("Rendering")]
    public int fallingObjectSortingOrder = -1; 
    
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
        
        float panelWidth = parentRectTransform.rect.width;
        float panelHeight = parentRectTransform.rect.height;
        
        float randomX = Random.Range(-panelWidth / 2f, panelWidth / 2f);
        float spawnY = panelHeight / 2f;
        
        Vector3 localSpawnPosition = new Vector3(randomX, spawnY, 0);
        
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
                objectToSpawn.transform.localPosition = localSpawnPosition;
            }
        }
        else if (catPrefabs != null && catPrefabs.Length > 0)
        {
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
                objectToSpawn.transform.localPosition = localSpawnPosition;
            }
        }
        else
        {
            return; 
        }
    }
    
    private void SetSortingOrder(GameObject obj)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingOrder = fallingObjectSortingOrder;
        }

        RectTransform rectTransform = obj.GetComponent<RectTransform>();
        if (rectTransform != null && parentRectTransform != null)
        {
            Transform boxTransform = parentRectTransform.Find("box");
            if (boxTransform != null)
            {
                int boxIndex = boxTransform.GetSiblingIndex();
                obj.transform.SetSiblingIndex(boxIndex);
            }
        }
    }
}
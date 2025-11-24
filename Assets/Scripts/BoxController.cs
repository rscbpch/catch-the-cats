using UnityEngine;

public class BoxController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float mouseFollowSpeed = 15f;      
    public float boundaryLeft = -8f;
    public float boundaryRight = 8f;

    private Camera mainCamera;
    private float lockedYPosition;

    private void Start()
    {
        mainCamera = Camera.main;
        lockedYPosition = transform.position.y;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingOrder = 1;
        }
    }

    private void Update()
    {
        float targetX = transform.position.x;

        if (mainCamera != null)
        {
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            targetX = Mathf.Lerp(targetX, mousePos.x, mouseFollowSpeed * Time.deltaTime);
        }

        targetX = Mathf.Clamp(targetX, boundaryLeft, boundaryRight);
        transform.position = new Vector3(targetX, lockedYPosition, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        FallingObject fallingObject = other.GetComponent<FallingObject>();
        if (fallingObject != null)
        {
            fallingObject.OnCaught();
        }
    }
}
using UnityEngine;

public class BoxController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float boundaryLeft = -8f;
    public float boundaryRight = 8f;
    
    private Camera mainCamera;
    private bool isDragging = false;
    private Vector3 offset;
    private float lockedYPosition; // Store the Y position to lock
    
    private void Start()
    {
        mainCamera = Camera.main;
        // Lock the Y position to the current position
        lockedYPosition = transform.position.y;
        
        // Ensure box renders on top of falling objects
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingOrder = 1; // Higher than falling objects
        }
    }
    
    private void Update()
    {
        HandleInput();
    }
    
    private void HandleInput()
    {
        // Mouse/Touch input
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            
            // Check if clicking on the box
            Collider2D hit = Physics2D.OverlapPoint(mousePos);
            if (hit != null && hit.gameObject == gameObject)
            {
                isDragging = true;
                // Only calculate offset for X axis
                offset.x = transform.position.x - mousePos.x;
            }
        }
        
        if (Input.GetMouseButton(0) && isDragging)
        {
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            
            // Only use X from mouse, keep Y locked
            Vector3 targetPos = new Vector3(mousePos.x + offset.x, lockedYPosition, 0);
            
            // Clamp to boundaries (only X)
            targetPos.x = Mathf.Clamp(targetPos.x, boundaryLeft, boundaryRight);
            transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
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
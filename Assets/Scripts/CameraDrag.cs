using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    private Vector3 touchStart; // Where the drag started
    private bool isDragging = false;
    public float dragSpeed = 0.1f; // Adjust this to control drag sensitivity
    public float minX = -10f; // Optional: Minimum X boundary
    public float maxX = 10f;  // Optional: Maximum X boundary
    public float minY = -5f;  // Optional: Minimum Y boundary
    public float maxY = 5f;   // Optional: Maximum Y boundary
    public GameObject room;
    public GameObject garage;

    // Zoom settings
    public float zoomSpeed = 100.0f; // Speed of zooming with scroll wheel
    public float pinchZoomSpeed = 5f; // Speed of zooming with pinch
    public float minZoom = 2f; // Minimum orthographic size (zoomed in)
    public float maxZoom = 10f; // Maximum orthographic size (zoomed out)
    private Vector2[] touchPositions; // For pinch-to-zoom
    private float initialPinchDistance;
    private float originalZoom;

    void Start()
    {
        // Ensure the camera is orthographic for 2D
        Camera.main.orthographic = true;
        touchPositions = new Vector2[2];
        originalZoom = Camera.main.orthographicSize;
    }

    void Update()
    {
        if (room.activeSelf || garage.activeSelf)
        {
            // Handle dragging (mouse)
            if (Input.GetMouseButtonDown(0))
            {
                touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                isDragging = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }
            else if (Input.GetMouseButton(0) && isDragging)
            {
                Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                MoveCamera(direction);
            }

            // Handle dragging (touch)
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    touchStart = Camera.main.ScreenToWorldPoint(touch.position);
                    isDragging = true;
                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    isDragging = false;
                }
                else if (touch.phase == TouchPhase.Moved && isDragging)
                {
                    Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(touch.position);
                    MoveCamera(direction);
                }
            }

            // Handle zoom with scroll wheel (desktop)
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0f)
            {
                ZoomCamera(scroll * zoomSpeed); // Negative to match typical scroll direction
            }

            // Handle pinch-to-zoom (mobile)
            if (Input.touchCount == 2)
            {
                Touch touch0 = Input.GetTouch(0);
                Touch touch1 = Input.GetTouch(1);

                if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
                {
                    touchPositions[0] = touch0.position;
                    touchPositions[1] = touch1.position;
                    initialPinchDistance = Vector2.Distance(touchPositions[0], touchPositions[1]);
                }
                else if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
                {
                    Vector2 currentTouch0 = touch0.position;
                    Vector2 currentTouch1 = touch1.position;
                    float currentDistance = Vector2.Distance(currentTouch0, currentTouch1);
                    float pinchDelta = currentDistance - initialPinchDistance;
                    ZoomCamera(-pinchDelta * pinchZoomSpeed); // Negative to match pinch intuition
                    initialPinchDistance = currentDistance; // Update for smooth zooming
                }
            }
        }
        else
        {
            transform.position = new Vector3(0, 0, -10);
            Camera.main.orthographicSize = originalZoom;
        }
    }

    void MoveCamera(Vector3 direction)
    {
        // Calculate new position
        Vector3 newPosition = transform.position + direction * dragSpeed;

        // Clamp the camera position to boundaries
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
        newPosition.z = transform.position.z; // Keep Z constant for 2D

        // Apply the new position
        transform.position = newPosition;
    }

    void ZoomCamera(float delta)
    {
        // Adjust orthographic size based on zoom input
        float newSize = Camera.main.orthographicSize - delta;
        Camera.main.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
    }
}
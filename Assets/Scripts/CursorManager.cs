using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D defaultCursor;      // Default cursor texture
    [SerializeField] private Texture2D hoverCursor;        // Cursor texture for hovering over buttons or specific objects
    [SerializeField] private Vector2 cursorHotspot = new Vector2(0, 0); // Hotspot for the cursor
    [SerializeField] private string hoverTag = "Hoverable"; // Tag for GameObjects that trigger the hover cursor
    [SerializeField] private LayerMask hoverLayer;         // Optional: Layer for hoverable GameObjects

    private bool isHovering = false;

    void Start()
    {
        // Set the default cursor when the game starts
        Cursor.SetCursor(defaultCursor, cursorHotspot, CursorMode.Auto);
    }

    void Update()
    {
        // Check if the mouse is over a button or a specific GameObject
        bool currentlyHovering = IsPointerOverButton() || IsPointerOverHoverableObject();

        // Switch cursor texture when hovering state changes
        if (currentlyHovering && !isHovering)
        {
            Cursor.SetCursor(hoverCursor, cursorHotspot, CursorMode.Auto);
            isHovering = true;
        }
        else if (!currentlyHovering && isHovering)
        {
            Cursor.SetCursor(defaultCursor, cursorHotspot, CursorMode.Auto);
            isHovering = false;
        }
    }

    // Check if the pointer is over a UI Button
    private bool IsPointerOverButton()
    {
        if (EventSystem.current == null || !EventSystem.current.IsPointerOverGameObject())
            return false;

        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var raycastResults = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        foreach (var result in raycastResults)
        {
            if (result.gameObject.GetComponent<Button>() != null)
            {
                return true;
            }
        }

        return false;
    }

    // Check if the pointer is over a specific GameObject
    private bool IsPointerOverHoverableObject()
    {
        // Skip if the pointer is over UI to avoid conflicts
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return false;

        // Cast a ray from the camera through the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, hoverLayer))
        {
            // Check if the hit object has the specified tag
            if (hit.collider.CompareTag(hoverTag))
            {
                return true;
            }
        }

        return false;
    }
}
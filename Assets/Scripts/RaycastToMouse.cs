using UnityEngine;

public class RaycastToMouse : MonoBehaviour
{
    private Camera mainCamera;

    private Interactable highlightedObject = null;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        TrackMouseAndHighlight();

        // Perform raycast on left mouse click
        if (Input.GetMouseButtonDown(0))
        {
            RaycastFromCameraToMouse();
        }
    }

    private void RaycastFromCameraToMouse()
    {
        // Get the mouse position in screen space
        Vector3 mousePosition = Input.mousePosition;

        // Create a ray from the camera to the mouse position
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);

        // Perform the raycast
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            // Check if the collider hit belongs to interactables
            Interactable interactable = hitInfo.collider.gameObject.GetComponent<Interactable>();
            if (interactable != null)
            {
                // Debug.Log($"Raycast hit {gameObject.name} at position {hitInfo.point}");
                interactable.RaycastHit(hitInfo.point);
            }
        }
    }
    private void TrackMouseAndHighlight()
    {
        // Get the mouse position in screen space
        Vector3 mousePosition = Input.mousePosition;

        // Create a ray from the camera to the mouse position
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);

        // Perform the raycast
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Interactable interactable = hitInfo.collider.gameObject.GetComponent<Interactable>();

            if (interactable != null)
            {
                // If a new object is hit, highlight it
                if (highlightedObject != interactable)
                {
                    // Remove highlight from the previously highlighted object
                    if (highlightedObject != null)
                    {
                        highlightedObject.SetHighlight(false);
                    }

                    // Highlight the current object
                    highlightedObject = interactable;
                    highlightedObject.SetHighlight(true);
                }
            }
            else
            {
                // Remove highlight if no interactable object is hit
                RemoveHighlight();
            }
        }
        else
        {
            // Remove highlight if nothing is hit
            RemoveHighlight();
        }
    }

    private void RemoveHighlight()
    {
        if (highlightedObject != null)
        {
            highlightedObject.SetHighlight(false);
            highlightedObject = null;
        }
    }
}
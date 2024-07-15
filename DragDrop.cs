using UnityEngine;

public class DragRigidbody : MonoBehaviour
{
    private Camera cam;
    private Rigidbody selectedRigidbody;
    private Vector3 offset;
    private float zDistanceToCamera;

    // Sensitivity for dragging speed
    public float dragSpeed = 10.0f;

    // Sensitivity for rotation speed
    public float rotationSpeed = 5.0f;

    // Layer mask to filter draggable objects
    public LayerMask draggableLayerMask;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        // Check if R key is held down for rotation control
        bool isRotating = Input.GetKey(KeyCode.R);

        if (Input.GetMouseButtonDown(0))
        {
            // Raycast to check if we hit a draggable object
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, draggableLayerMask))
            {
                if (hit.rigidbody != null)
                {
                    selectedRigidbody = hit.rigidbody;
                    zDistanceToCamera = hit.distance; // Use hit distance instead of transform.z difference
                    offset = selectedRigidbody.transform.position - GetMouseWorldPosition();
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            selectedRigidbody = null;
        }

        if (selectedRigidbody != null)
        {
            // Calculate the target position
            Vector3 targetPosition = GetMouseWorldPosition() + offset;

            // Calculate force direction for movement
            Vector3 forceDirection = (targetPosition - selectedRigidbody.position) * dragSpeed;

            // Apply force to move the Rigidbody
            selectedRigidbody.velocity = forceDirection;

            // Rotation control
            if (isRotating)
            {
                float rotateAmount = Input.GetAxis("Mouse X") * rotationSpeed;
                selectedRigidbody.transform.Rotate(Vector3.up, rotateAmount);
            }else{
                selectedRigidbody.transform.Rotate(Vector3.up,0f    );
            }
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, zDistanceToCamera);
        return cam.ScreenToWorldPoint(mouseScreenPosition);
    }
}

using UnityEngine;

public class PickupSystem : MonoBehaviour
{
    [Header("Pickup Settings")]
    public float pickupRange = 3f;
    public float holdDistance = 2f;
    public float moveSmoothness = 10f;
    public float minHoldDistance = 0.5f;
    public float maxHoldDistance = 5f;
    public float scrollSensitivity = 1f;

    [Header("Rotation Settings")]
    public float rotationSpeed = 5f;

    private Camera playerCam;
    private GameObject heldObject;
    private Rigidbody heldRb;

    [HideInInspector] public bool isInspecting = false;

    void Start()
    {
        playerCam = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject == null)
                TryPickup();
            else
                DropObject();
        }

        if (heldObject != null)
        {
            HandleScrollInput();
            MoveHeldObject();

            if (Input.GetMouseButton(1)) // Right mouse button
            {
                isInspecting = true;
                RotateHeldObject();
            }
            else if (isInspecting)
            {
                isInspecting = false;
                heldRb.angularVelocity = Vector3.zero;
            }
        }
    }

    void HandleScrollInput()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            holdDistance += scroll * scrollSensitivity;
            holdDistance = Mathf.Clamp(holdDistance, minHoldDistance, maxHoldDistance);
        }
    }

    void TryPickup()
    {
        Ray ray = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
        {
            ConductiveItem item = hit.collider.GetComponent<ConductiveItem>();
            if (item != null)
            {
                heldObject = hit.collider.gameObject;
                heldRb = heldObject.GetComponent<Rigidbody>();

                if (heldRb != null)
                {
                    heldRb.useGravity = false;
                    heldRb.linearDamping = 10f;
                    heldRb.angularDamping = 10f;
                }
            }
        }
    }

    void MoveHeldObject()
    {
        Vector3 targetPos = playerCam.transform.position + playerCam.transform.forward * holdDistance;
        Vector3 direction = targetPos - heldObject.transform.position;
        heldRb.linearVelocity = direction * moveSmoothness;
    }

    void RotateHeldObject()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        heldObject.transform.Rotate(playerCam.transform.up, -mouseX, Space.World);
        heldObject.transform.Rotate(playerCam.transform.right, mouseY, Space.World);
    }

    void DropObject()
    {
        if (heldRb != null)
        {
            heldRb.useGravity = true;
            heldRb.linearDamping = 0f;
            heldRb.angularDamping = 0.05f;
        }

        heldObject = null;
        heldRb = null;
        isInspecting = false;
    }
}

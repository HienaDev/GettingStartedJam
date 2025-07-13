using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 6f;
    public float gravity = -9.81f;

    [Header("Mouse Look Settings")]
    public Transform cameraTransform;
    public float mouseSensitivity = 100f;
    public float maxLookAngle = 90f;

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation = 0f;

    [SerializeField] private PickupSystem pickupSystem; // Drag your PickupSystem here in the Inspector


    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;  // Hide and lock cursor
    }

    void Update()
    {
        LookAround();
        Move();
    }

 
    void LookAround()
    {
        if (pickupSystem != null && pickupSystem.isInspecting)
            return; // Don't rotate camera while inspecting

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxLookAngle, maxLookAngle);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }


    void Move()
    {

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move.normalized * walkSpeed * Time.deltaTime);


        controller.Move(velocity * Time.deltaTime);
    }
}

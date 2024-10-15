using UnityEngine;

public class SpectatorCamera : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float sprintSpeed = 15f; // Movement speed
    public float mouseSensitivity = 100f; // Mouse sensitivity for looking around
    public float maxLookAngle = 85f;     // Max angle to prevent flipping
    public Rigidbody rb;

    private float yaw = 0f;
    private float pitch = 0f;
    private float currentSpeed;

    private void Start()
    {
        // Lock cursor to the screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Handle rotation using the mouse
        //HandleRotation();
        HandleMovement();
    }

    private void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

        // Apply rotation
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    private void HandleMovement()
    {
        // Get input for movement (WASD or Arrow keys)
        float moveX = Input.GetAxis("Horizontal");  // A/D or Left/Right arrow keys
        float moveZ = Input.GetAxis("Vertical");    // W/S or Up/Down arrow keys
        float moveY = 0f;

        if (Input.GetKey(KeyCode.Space))  // Move up
            moveY = 1f;
        else if (Input.GetKey(KeyCode.LeftControl))  // Move down
            moveY = -1f;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed;
        }
        else
        {
            currentSpeed = moveSpeed;
        }

        // Create movement vector based on camera direction
        Vector3 move = transform.right * moveX + transform.forward * moveZ + transform.up * moveY;

        // Set the Rigidbody velocity to move the camera
        rb.velocity = move * currentSpeed;
    }
}


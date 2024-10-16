using UnityEngine;

public class SpectatorCamera : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float sprintSpeed = 15f; // Movement speed


    public Rigidbody rb;
    private float currentSpeed;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleMovement();
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


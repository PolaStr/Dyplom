using Cinemachine;
using UnityEngine;

public class SpectatorCamera : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float sprintSpeed = 15f; // Movement speed

    private float targetZoom; // Target zoom value
    private float zoomSens = 10.0f; // Sensitivity of zoom
    private float zoomSmoothSpeed = 5.0f; // Speed of smoothing
    private float minZoom = 0.0f; // Minimum zoom limit
    private float maxZoom = 15.0f; // Maximum zoom limit


    public Rigidbody rb;
    private float currentSpeed;
    private bool canMove = true;
    private bool isOrbital = true;

    [SerializeField] private CinemachineVirtualCamera orbitalCam;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        targetZoom = orbitalCam.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_FollowOffset.y;
    }

    private void Update()
    {
        if (canMove)
        {
            CameraMovement();
        }
        if (canMove && isOrbital)
        {
            OrbitalMovement();
            WheelZoom();
        }
    }

    private void CameraMovement()
    {
        // Get input for movement (WASD or Arrow keys)
        float moveX = Input.GetAxis("Horizontal");  // A/D or Left/Right arrow keys
        float moveZ = Input.GetAxis("Vertical");    // W/S or Up/Down arrow keys
        float moveY = 0f;

        if (Input.GetKey(KeyCode.Space))
        {
            moveY = 1f;
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            moveY = -1f;
        }


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

        rb.velocity = move * currentSpeed;
    }

    private void OnDisable()
    {
        rb.velocity = Vector3.zero;
    }

    private void OrbitalMovement()
    {
        // Get input for movement (WASD or Arrow keys)
        float moveXOrb = Input.GetAxis("Horizontal");  // A/D or Left/Right arrow keys
        float moveZOrb = Input.GetAxis("Vertical");    // W/S or Up/Down arrow keys
        float moveYOrb = 0f;

        if (Input.GetKey(KeyCode.Space))
        {
            moveYOrb = 1f;
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            moveYOrb = -1f;
        }


        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed;
        }
        else
        {
            currentSpeed = moveSpeed;
        }

        // Create movement vector based on camera direction
        Vector3 moveOrb = transform.right * moveXOrb + transform.forward * moveZOrb + Vector3.up * moveYOrb;

        rb.velocity = moveOrb * currentSpeed;


        if (Input.GetKey(KeyCode.Q))
        {
            orbitalCam.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_XAxis.Value += 100f * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.E))
        {
            orbitalCam.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_XAxis.Value -= 100f * Time.deltaTime;
        }
    }

    private void WheelZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            // Adjust target zoom based on scroll input
            targetZoom -= scrollInput * zoomSens;
            targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom); // Clamp the target zoom
        }

        // Smoothly interpolate the current zoom towards the target zoom
        float currentZoom = orbitalCam.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_FollowOffset.y;
        currentZoom = Mathf.Lerp(currentZoom, targetZoom, Time.deltaTime * zoomSmoothSpeed);

        // Apply the smoothed zoom
        orbitalCam.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_FollowOffset.y = currentZoom;
    }
}


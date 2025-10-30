using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [Header("References")]
    public Transform player;          // Player reference
    public Transform cameraTarget;    // Child "CameraPosition" on Player

    [Header("Movement Settings")]
    public float moveSpeed = 3f;      // Camera movement speed
    public float rotateSpeed = 5f;    // Camera rotation speed
    public float attachDistance = 0.05f; // Distance before snapping

    [Header("Player Control Settings")]
    public float walkSpeed = 3f;          // How fast player moves forward/backward
    public float rotationSensitivity = 60f; // How fast player rotates with A/D

    private bool isMoving = false;
    private bool isAttached = false;

    void Start()
    {
        // Auto-find player and camera position
        if (player == null)
        {
            GameObject pObj = GameObject.FindGameObjectWithTag("Player");
            if (pObj != null)
                player = pObj.transform;
            else
                Debug.LogWarning("CameraFollowPlayer: No object with tag 'Player' found.");
        }

        if (cameraTarget == null && player != null)
        {
            Transform t = player.Find("CameraPosition");
            if (t != null)
                cameraTarget = t;
            else
                Debug.LogWarning("CameraFollowPlayer: No child named 'CameraPosition' found on Player.");
        }
    }

    void Update()
    {
        // When player presses Left Click, start moving to player's view
        if (Input.GetButtonDown("Fire1") && !isAttached)
        {
            isMoving = true;
        }

        // Once attached, allow player movement & rotation
        if (isAttached)
        {
            HandleRotation();
            HandleMovement();
        }
    }

    void LateUpdate()
    {
        if (!isMoving || cameraTarget == null) return;

        if (!isAttached)
        {
            // Smoothly move and rotate toward player head
            transform.position = Vector3.Lerp(transform.position, cameraTarget.position, Time.deltaTime * moveSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, cameraTarget.rotation, Time.deltaTime * rotateSpeed);

            // Snap when close enough
            if (Vector3.Distance(transform.position, cameraTarget.position) < attachDistance)
            {
                isAttached = true;
                transform.position = cameraTarget.position;
                transform.rotation = cameraTarget.rotation;
                isMoving = false;
            }
        }
    }

    void HandleRotation()
    {
        float horizontal = 0f;

        if (Input.GetKey(KeyCode.A))
            horizontal = -1f;
        else if (Input.GetKey(KeyCode.D))
            horizontal = 1f;

        if (horizontal != 0f)
        {
            float rotationAmount = horizontal * rotationSensitivity * Time.deltaTime;
            player.Rotate(Vector3.up, rotationAmount);
        }

        // Keep camera locked on player's head
        transform.position = cameraTarget.position;
        transform.rotation = cameraTarget.rotation;
    }

    void HandleMovement()
    {
        float vertical = 0f;

        if (Input.GetKey(KeyCode.W))
            vertical = 1f;
        else if (Input.GetKey(KeyCode.S))
            vertical = -1f;

        // Move player forward/backward smoothly
        if (vertical != 0f)
        {
            Vector3 moveDir = player.forward * vertical * walkSpeed * Time.deltaTime;
            player.position += moveDir;
        }

        // Keep camera attached
        transform.position = cameraTarget.position;
        transform.rotation = cameraTarget.rotation;
    }
}

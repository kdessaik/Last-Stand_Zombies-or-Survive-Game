using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject bulletPrefab;   // Drag your bullet prefab here
    public Transform firePoint;       // The point from where bullets will be shot
    public float bulletForce = 20f;   // Speed of bullet

    [Header("Animation")]
    public Animator animator; // Drag Player's Animator here

    [Header("Movement Settings")]
    public float rotationSpeed = 100f; // Speed of turning

    void Update()
    {
        HandleRotation();
        HandleShooting();
    }

    void HandleRotation()
    {
        float horizontal = 0f;

        if (Input.GetKey(KeyCode.D))
        {
            horizontal = 1f; // Turn right
        }
        else if (Input.GetKey(KeyCode.A))
        {
            horizontal = -1f; // Turn left
        }

        // Apply rotation if any key is pressed
        if (horizontal != 0f)
        {
            transform.Rotate(Vector3.up * horizontal * rotationSpeed * Time.deltaTime);
        }
    }

    void HandleShooting()
    {
        if (Input.GetButtonDown("Fire1")) // Left Click / Ctrl
        {
            Shoot();

            // Trigger shooting animation
            if (animator != null)
            {
                animator.SetTrigger("Shoot");
            }
        }
    }

    void Shoot()
    {
        // Create a bullet at the firePoint position and rotation
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Add force to bullet so it moves forward
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);
    }
}

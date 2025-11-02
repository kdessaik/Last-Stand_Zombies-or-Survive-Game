using UnityEngine;
using System.Collections;

public class PlayerShootingWithAudio : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletForce = 20f;

    [Header("Animation")]
    public Animator animator;

    [Header("Movement Settings")]
    public float rotationSpeed = 100f;
    public float moveSpeed = 5f;

    [Header("Muzzle Flash Settings")]
    public float flashDuration = 0.05f;
    public float flashLightIntensity = 5f;
    public float flashRange = 2f;
    public ParticleSystem flashParticles;

    [Header("Audio Clips")]
    public AudioClip startClip;        // Plays at start
    public AudioClip shootClip;        // Gunshot
    public AudioClip enemyHitClip;     // Enemy takes damage
    public AudioClip enemyDeathClip;   // Enemy dies
    public AudioClip loseClip;         // Player loses
    public AudioClip winClip;          // Player wins

    private AudioSource audioSource;
    private Rigidbody rb;
    private Light muzzleLight;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Play game start sound
        if (startClip != null)
            audioSource.PlayOneShot(startClip);

        // Create a light for muzzle flash
        GameObject flashLightObject = new GameObject("MuzzleFlashLight");
        flashLightObject.transform.SetParent(firePoint);
        flashLightObject.transform.localPosition = Vector3.zero;
        muzzleLight = flashLightObject.AddComponent<Light>();
        muzzleLight.color = Color.yellow;
        muzzleLight.range = flashRange;
        muzzleLight.intensity = 0f;
        muzzleLight.enabled = false;
    }

    void Update()
    {
        HandleRotation();
        HandleMovement();
        HandleShooting();
    }

    void HandleRotation()
    {
        float horizontal = 0f;
        if (Input.GetKey(KeyCode.D)) horizontal = 1f;
        else if (Input.GetKey(KeyCode.A)) horizontal = -1f;

        if (horizontal != 0f)
        {
            Quaternion turn = Quaternion.Euler(0f, horizontal * rotationSpeed * Time.deltaTime, 0f);
            transform.rotation *= turn;
        }
    }

    void HandleMovement()
    {
        float vertical = 0f;
        if (Input.GetKey(KeyCode.W)) vertical = 1f;
        else if (Input.GetKey(KeyCode.S)) vertical = -1f;

        Vector3 moveDirection = transform.forward * vertical * moveSpeed * Time.deltaTime;
        transform.position += moveDirection;
    }

    void HandleShooting()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();

            if (animator != null)
                animator.SetTrigger("Shoot");

            if (shootClip != null)
                audioSource.PlayOneShot(shootClip);

            StartCoroutine(DoMuzzleFlash());
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(100);

        Vector3 shootDir = (targetPoint - firePoint.position).normalized;
        bullet.transform.forward = shootDir;

        Rigidbody rbBullet = bullet.GetComponent<Rigidbody>();
        if (rbBullet != null)
            rbBullet.AddForce(shootDir * bulletForce, ForceMode.Impulse);

        // Assign bullet sound behavior
        BulletSound bulletSound = bullet.AddComponent<BulletSound>();
        bulletSound.enemyHitClip = enemyHitClip;
        bulletSound.enemyDeathClip = enemyDeathClip;
    }

    IEnumerator DoMuzzleFlash()
    {
        muzzleLight.enabled = true;
        muzzleLight.intensity = flashLightIntensity;

        if (flashParticles != null)
            flashParticles.Play();

        yield return new WaitForSeconds(flashDuration);
        muzzleLight.enabled = false;
    }

    // Called externally when losing/winning
    public void PlayLoseSound()
    {
        if (loseClip != null)
            audioSource.PlayOneShot(loseClip);
    }

    public void PlayWinSound()
    {
        if (winClip != null)
            audioSource.PlayOneShot(winClip);
    }
}

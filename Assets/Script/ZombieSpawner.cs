using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;   // Drag your zombie prefab here
    public Transform spawnPoint;      // The single spawn location
    public Transform player;          // Reference to the player
    public float spawnInterval = 5f;  // Time between each spawn
    public int maxZombies = 20;       // Limit number of zombies
    public float baseSpeed = 2f;      // Starting zombie speed
    public float speedIncrease = 0.5f; // Speed boost after every 5 zombies

    private float timer = 0f;
    private int spawnedCount = 0;
    private float currentSpeed;

    void Start()
    {
        currentSpeed = baseSpeed;
    }

    void Update()
    {
        if (spawnedCount >= maxZombies)
            return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnZombie();
            timer = 0f;
        }
    }

    void SpawnZombie()
    {
        if (spawnPoint == null || zombiePrefab == null || player == null)
        {
            Debug.LogWarning("Spawner not configured properly!");
            return;
        }

        // Create the zombie
        GameObject zombie = Instantiate(zombiePrefab, spawnPoint.position, spawnPoint.rotation);

        // Give zombie the player target and speed
        ZombieFollow followScript = zombie.GetComponent<ZombieFollow>();
        if (followScript != null)
        {
            followScript.SetTarget(player);
            followScript.IncreaseSpeed(currentSpeed);
        }

        spawnedCount++;

        // Every 5 zombies, make next ones faster
        if (spawnedCount % 5 == 0)
        {
            currentSpeed += speedIncrease;
            Debug.Log($"Speed increased to {currentSpeed} after {spawnedCount} zombies!");
        }
    }
}

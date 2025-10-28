using UnityEngine;

public class ZombieMovement : MonoBehaviour
{
    public float speed = 0.5f;            // Speed of zombie
    private Transform player;           // Reference to player

    void Start()
    {
        // Find the player in the scene using the Player tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("No player found! Please assign the Player tag to your player object.");
        }
    }

    void Update()
    {
        if (player == null) return;

        // Move towards player position
        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            speed * Time.deltaTime
        );

        // Rotate to face the player
        Vector3 direction = player.position - transform.position;
        direction.y = 0; // keep rotation horizontal
        if (direction.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}

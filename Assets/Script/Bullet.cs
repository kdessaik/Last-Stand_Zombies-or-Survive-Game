using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bodyDamage = 1;         // Damage for body hits
    public float lifeTime = 3f;        // Time before bullet destroys itself

    void Start()
    {
        // Destroy bullet automatically after a few seconds
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if we hit a zombie
        ZombieHealth zombie = collision.gameObject.GetComponent<ZombieHealth>();

        if (zombie != null)
        {
            // Check if hit the head (the head must have a collider tagged "Head")
            if (collision.collider.CompareTag("Head"))
            {
                zombie.Headshot();
            }
            else
            {
                zombie.TakeDamage(bodyDamage);
            }
        }

        // Destroy bullet after hitting something
        Destroy(gameObject);
    }
}

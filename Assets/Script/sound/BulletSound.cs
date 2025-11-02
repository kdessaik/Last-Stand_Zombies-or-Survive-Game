using UnityEngine;

public class BulletSound : MonoBehaviour
{
    public AudioClip enemyHitClip;
    public AudioClip enemyDeathClip;

    void OnCollisionEnter(Collision collision)
    {
        ZombieHealth zombie = collision.gameObject.GetComponent<ZombieHealth>();

        if (zombie != null)
        {
            AudioSource.PlayClipAtPoint(enemyHitClip, transform.position);

            if (collision.collider.CompareTag("Head"))
            {
                zombie.Headshot();
                AudioSource.PlayClipAtPoint(enemyDeathClip, transform.position);
            }
            else
            {
                zombie.TakeDamage(1);
            }
        }

        Destroy(gameObject);
    }
}

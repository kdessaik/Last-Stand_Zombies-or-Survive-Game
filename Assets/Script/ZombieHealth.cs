using UnityEngine;


public class ZombieHealth : MonoBehaviour
{
    public int health = 2;
    public GameObject deathEffect;
    private bool isDead = false;
    private Animator animator;


    void Start()
    {
        animator = GetComponent<Animator>();
    }


    public void TakeDamage(int damage)
    {
        if (isDead) return;


        health -= damage;
        if (health <= 0)
        {
            Die();
        }
        else if (animator != null)
            animator.SetTrigger("Hit");
    }


    public void Headshot()
    {
        if (isDead) return;
        health = 0;
        Die();
    }


    void Die()
    {
        isDead = true;
        if (animator != null)
            animator.SetTrigger("Die");


        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);


        // Notify GameManager
        if (GameManager.Instance != null)
            GameManager.Instance.RegisterKill();


        Destroy(gameObject, 2f);
    }
}
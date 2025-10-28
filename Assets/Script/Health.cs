using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 50;
    public int current;
    public Animator animator;
    public bool dead = false;

    void Start()
    {
        current = maxHealth;
        if (!animator) animator = GetComponentInChildren<Animator>();
    }

    public void TakeDamage(int amount)
    {
        if (dead) return;
        current -= amount;
        if (animator) animator.SetTrigger("HitTrigger");
        if (current <= 0) Die();
    }

    void Die()
    {
        dead = true;
        if (animator) animator.SetBool("IsDead", true);
        // disable navmesh agent
        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent) agent.enabled = false;
        // disable collider or set to ragdoll - optional
        Destroy(gameObject, 5f); // cleanup
    }
}

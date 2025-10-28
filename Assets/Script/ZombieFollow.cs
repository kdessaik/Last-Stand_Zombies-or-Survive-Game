using UnityEngine;
using UnityEngine.AI;

public class ZombieFollow : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 20f;
    public float attackRange = 1.5f;
    public float attackRate = 1f;
    public int damage = 10;
    public float moveSpeed = 2f;

    private NavMeshAgent agent;
    private Animator animator;
    private float nextAttackTime = 0f;
    private bool isDead = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }

        agent.speed = moveSpeed;
        agent.updateRotation = true;
        agent.updatePosition = true;
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            agent.isStopped = true;
            AttackPlayer();
        }
        else if (distance <= detectionRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            if (animator != null)
                animator.SetBool("isWalking", true);
        }
        else
        {
            agent.isStopped = true;
            if (animator != null)
                animator.SetBool("isWalking", false);
        }

        // Re-path if stuck
        if (!agent.pathPending && !agent.isStopped && agent.remainingDistance > 0 && agent.velocity.magnitude < 0.1f)
        {
            agent.SetDestination(player.position);
        }
    }

    void AttackPlayer()
    {
        if (animator != null)
        {
            animator.SetBool("isWalking", false);
            animator.SetTrigger("Attack");
        }

        if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackRate;
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
                playerHealth.TakeDamage(damage);
        }
    }

    public void Die()
    {
        isDead = true;
        agent.isStopped = true;
        if (animator != null)
            animator.SetTrigger("Die");
        Destroy(gameObject, 3f);
    }

    public void SetTarget(Transform target)
    {
        player = target;
    }

    public void IncreaseSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
        if (agent != null)
            agent.speed = moveSpeed;
    }
}

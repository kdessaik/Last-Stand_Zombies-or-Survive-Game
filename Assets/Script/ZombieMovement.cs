using UnityEngine;
using UnityEngine.AI; // Needed for pathfinding

public class ZombieAI : MonoBehaviour
{
    public Transform player;         // Assign your player here
    public float attackRange = 2f;   // How close before attacking
    public float attackCooldown = 1.5f; // Time between attacks
    public int damage = 10;          // Damage dealt to player

    private NavMeshAgent agent;      // Handles movement
    private Animator animator;       // Controls walking/attack animations
    private float lastAttackTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        // Move towards player
        agent.SetDestination(player.position);

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            // Stop moving and attack
            agent.isStopped = true;
            animator.SetBool("isWalking", false);

            if (Time.time > lastAttackTime + attackCooldown)
            {
                animator.SetTrigger("Attack"); // Play attack animation
                lastAttackTime = Time.time;

                
            }
        }
        else
        {
            // Keep walking
            agent.isStopped = false;
            animator.SetBool("isWalking", true);
        }
    }
}

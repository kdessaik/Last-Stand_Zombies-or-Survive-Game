using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ZombieAI : MonoBehaviour
{
    public enum State { Patrol, Chase, Attack, Idle }
    public State state = State.Patrol;
    public Transform[] patrolPoints;
    private int currentPatrol = 0;
    NavMeshAgent agent;
    Transform target;
    public float chaseDistance = 10f;
    public float attackDistance = 1.8f;
    public float attackCooldown = 1.5f;
    float lastAttackTime;
    Animator anim;
    Health healthScript;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        healthScript = GetComponent<Health>();
        target = GameObject.FindWithTag("Player")?.transform;
        if (patrolPoints.Length > 0)
            agent.SetDestination(patrolPoints[0].position);
    }

    void Update()
    {
        if (healthScript != null && healthScript.dead) return;

        float distToPlayer = target ? Vector3.Distance(transform.position, target.position) : Mathf.Infinity;

        if (distToPlayer <= attackDistance)
        {
            state = State.Attack;
        }
        else if (distToPlayer <= chaseDistance)
        {
            state = State.Chase;
        }
        else
        {
            state = State.Patrol;
        }

        switch (state)
        {
            case State.Patrol:
                Patrol();
                break;
            case State.Chase:
                Chase();
                break;
            case State.Attack:
                Attack();
                break;
        }

        float speed = agent.velocity.magnitude;
        if (anim) anim.SetFloat("Speed", speed);
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;
        if (!agent.hasPath || agent.remainingDistance < 0.3f)
        {
            currentPatrol = (currentPatrol + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentPatrol].position);
        }
    }

    void Chase()
    {
        if (!target) return;
        agent.SetDestination(target.position);
    }

    void Attack()
    {
        agent.isStopped = true;
        if (anim) anim.SetBool("IsAttacking", true);

        if (Time.time - lastAttackTime > attackCooldown)
        {
            lastAttackTime = Time.time;
            // Damage when animation event triggers or do a sphere check here:
            TryDealDamage();
        }
    }

    void TryDealDamage()
    {
        if (!target) return;
        float d = Vector3.Distance(transform.position, target.position);
        if (d <= attackDistance + 0.5f)
        {
            var dmg = target.GetComponent<IDamageable>();
            if (dmg != null) dmg.TakeDamage(10);
        }
    }

    public void OnEndAttack() // call from animation event at attack end
    {
        agent.isStopped = false;
        if (anim) anim.SetBool("IsAttacking", false);
    }
}

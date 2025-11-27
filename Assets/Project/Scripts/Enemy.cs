using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Vision Settings")]
    public float sightRange = 12f;
    public float attackRange = 3f;
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [Header("AI Settings")]
    public float walkPointRange = 100f;
    public float timeBetweenAttacks = 2f;
    public int attackDamage = 10;

    public Animator animator;

    private NavMeshAgent agent;
    private Transform player;
    private Vector3 walkPoint;
    private bool walkPointSet;
    private bool alreadyAttacked;

    private bool playerInSightRange;
    private bool playerInAttackRange;

    private float patrolTurnTimer = 0f;
    public float patrolTurnInterval = 5f;

    private bool waitingAfterTurn = false;
    public float waitAfterTurn = 1.5f; // how long he waits before new walkpoint
    private float waitTimer = 0f;



    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, targetMask);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, targetMask);

        // Choose state
        if (!playerInSightRange && !playerInAttackRange)
            Patroling();
        else if (playerInSightRange && !playerInAttackRange)
            ChasePlayer();
        else if (playerInAttackRange)
            AttackPlayer();

        // Update animation states
        animator.SetBool("isWalking", agent.velocity.magnitude > 0.1f && !playerInSightRange);
        animator.SetBool("isRunning", agent.velocity.magnitude > 0.1f && playerInSightRange && !playerInAttackRange);
    }


    void Patroling()
    {
        agent.isStopped = false;


        // 1. HANDLE WAITING AFTER TURN

        if (waitingAfterTurn)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitAfterTurn)
            {
                waitingAfterTurn = false;
                waitTimer = 0f;
                walkPointSet = false; // NOW search a new walk point
            }
            return; // stop patrolling until waiting is done
        }


        // 2. TIMER TO ROTATE EVERY X SECONDS

        patrolTurnTimer += Time.deltaTime;
        if (patrolTurnTimer >= patrolTurnInterval)
        {
            transform.Rotate(0, 180f, 0);   // Turn around
            patrolTurnTimer = 0f;

            // Start waiting phase
            waitingAfterTurn = true;
            return;
        }


        // 3. NORMAL PATROLLING BEHAVIOR
 
        if (!walkPointSet) SearchWalkPoint();
        if (walkPointSet) agent.SetDestination(walkPoint);

        if (Vector3.Distance(transform.position, walkPoint) < 1f)
            walkPointSet = false;
    }

    void SearchWalkPoint()
    {
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, obstacleMask))
            walkPointSet = true;
    }

    void ChasePlayer()
    {
        agent.isStopped = false;  // <-- important

        if (player != null)
            agent.SetDestination(player.position);
    }

    void AttackPlayer()
    {
        agent.isStopped = true;
        transform.LookAt(player);
        animator.SetBool("isAttacking", true);

        if (!alreadyAttacked)
        {
            // damage
            Player p = player.GetComponent<Player>();
            if (p != null)
                p.TakeDamageBlood(attackDamage);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    void ResetAttack()
    {
        animator.SetBool("isAttacking", false);
        agent.isStopped = false;
        alreadyAttacked = false;
    }
}

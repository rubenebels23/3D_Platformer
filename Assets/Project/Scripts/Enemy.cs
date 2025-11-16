using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    #region Vision Settings
    public float viewRadius = 10f;
    [Range(0, 360)] public float viewAngle = 120f;
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public List<Transform> visibleTargets = new List<Transform>();
    #endregion

    #region AI Settings
    public float sightRange = 12f;
    public float attackRange = 3f;
    public float walkPointRange = 100f;
    public float timeBetweenAttacks = 2f;
    public int attackDamage = 10; // damage per hit
    #endregion

    private NavMeshAgent agent;
    private Transform player;

    private Vector3 walkPoint;
    private bool walkPointSet;
    private bool alreadyAttacked;

    private bool playerInSightRange;
    private bool playerInAttackRange;

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

        if (!playerInSightRange && !playerInAttackRange)
            Patroling();
        else if (playerInSightRange && !playerInAttackRange)
            ChasePlayer();
        else if (playerInAttackRange && playerInSightRange)
            AttackPlayer();
    }


    //  PATROL / CHASE / ATTACK 
    void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f)
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
        if (visibleTargets.Count > 0)
            agent.SetDestination(visibleTargets[0].position);
        else
            agent.SetDestination(player.position);
    }

    void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            Player p = player.GetComponent<Player>();
            if (p != null)
            {
                p.TakeDamageBlood(attackDamage);
            }

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    void ResetAttack()
    {
        alreadyAttacked = false;
    }


}

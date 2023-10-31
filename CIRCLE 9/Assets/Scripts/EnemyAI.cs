using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;

    [SerializeField] private LayerMask whatIsGround, whatIsPlayer;

    [SerializeField] private float health;
    [SerializeField] private float _chaseSpeed, _patrolSpeed;
    //Patroling
    private Vector3 walkPoint;
    private bool walkPointSet;
    [SerializeField] private float walkPointRange;

    private bool _enabled;
    private bool _shouldEnable;

    //States
    [SerializeField] private float sightRange, attackRange;
    [SerializeField] private bool playerInSightRange, playerInAttackRange;

    //Lerp when starting
    private float lerpedValue;
    private float duration = 3;
    private float totalScale = 1;
    private void Start()
    {
        player = GameObject.Find("PLAYER").transform;
        agent = GetComponent<NavMeshAgent>();
        _enabled = true;
        agent.enabled = true;
    }
    IEnumerator LerpValue(float start, float end)
    {
        float timeElapsed = 0;
        float reducedRange = Mathf.Abs(start - end);
        float reducedDuration = duration * (reducedRange / totalScale);

        while (timeElapsed < reducedDuration)
        {
            float t = timeElapsed / reducedDuration;

            lerpedValue = Mathf.Lerp(start, end, t);
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        lerpedValue = end;
        _shouldEnable = true;
    }
    private void Update()
    {
        EnableWhenNeeded();
        //Check for sight and attack range
        if (agent.enabled)
        {
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
            if (!playerInSightRange && !playerInAttackRange) Patroling();
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            if (playerInAttackRange) AttackPlayer();
        } else
        {
            //lerp above the ice
            //this.transform.position = new Vector3(this.transform.position.x, lerpedValue, this.transform.position.z);
        }
    }
    private void EnableWhenNeeded()
    {
        if (!_enabled &&  _shouldEnable)
        {
            this.gameObject.GetComponent<NavMeshAgent>().enabled = true;
            _enabled = true;
            StartCoroutine(LerpValue(lerpedValue, 1));
        }
    }
    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            agent.speed = _patrolSpeed;
            agent.SetDestination(walkPoint);
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.speed = _chaseSpeed;
        agent.SetDestination(player.position);
        agent.isStopped = false;
    }

    private void AttackPlayer()
    {
        //agent.isStopped = true;
        agent.velocity = Vector3.zero;
        //Debug.Log("attack animation");
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}


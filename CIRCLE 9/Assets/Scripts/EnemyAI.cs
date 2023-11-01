using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;

    [Header("Layers")]

    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsPlayer;

    [Header("Settings")]
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _chaseSpeed, _patrolSpeed;
    //Patroling
    private Vector3 walkPoint;
    private bool walkPointSet;

    [SerializeField] private float _patrolRange;

    private bool _enabled;
    private bool _shouldEnable;

    //States
    [SerializeField] private float sightRange, attackRange;
    [SerializeField] private float _ragDollTime = 10;
    [Header("Debug")]
    [SerializeField] private bool playerInSightRange;
    [SerializeField] private bool playerInAttackRange;

    //Lerp when starting
    private float lerpedValue;
    private float duration = 3;
    private float totalScale = 1;
    private bool isLerping;
    [NonSerialized] public bool isDead;
    private float _timer;

    private void Start()
    {
        player = GameObject.Find("PLAYER").transform;
        agent = GetComponent<NavMeshAgent>();
        _enabled = false;
        agent.enabled = false;
        lerpedValue = this.transform.position.y;
    }
    IEnumerator LerpValue(float start, float end)
    {
        float timeElapsed = 0;
        float reducedRange = Mathf.Abs(start - end);
        float reducedDuration = duration * (reducedRange / totalScale);
        reducedDuration += UnityEngine.Random.Range(0, 5);
        while (timeElapsed < reducedDuration && !isDead)
        {
            isLerping = true;
            float t = timeElapsed / reducedDuration;

            lerpedValue = Mathf.Lerp(start, end, t);
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        lerpedValue = end;
        isLerping = false;
        _shouldEnable = true;
    }
    private void EnableWhenNeeded()
    {
        if (!_enabled && _shouldEnable && !isDead)
        {
            this.gameObject.GetComponent<NavMeshAgent>().enabled = true;
            _enabled = true;
        }
    }

    public void SubmergeOutIce(float endY)
    {
        StartCoroutine(LerpValue(transform.position.y, endY));
        // massive band-aid fix, let's hope nobody sees this code lol
        //StartCoroutine(LerpValue(-1f, 1.05f));

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
        }
        else if (!agent.enabled & isLerping)
        {
            //lerp above the ice
            this.transform.position = new Vector3(this.transform.position.x, lerpedValue, this.transform.position.z);
        }
        if (isDead)
        {
            TakeDamage();
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
        float randomZ = UnityEngine.Random.Range(-_patrolRange, _patrolRange);
        float randomX = UnityEngine.Random.Range(-_patrolRange, _patrolRange);

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

    public void TakeDamage()
    {
        if (_timer <= _ragDollTime)
            _timer += Time.deltaTime;

        if (_timer > _ragDollTime) Invoke(nameof(DestroyEnemy), 0.5f);
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


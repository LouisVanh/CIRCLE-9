using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;

    [Header("Prefab")]
    [SerializeField] private GameObject _dropItem;

    [Header("Layers")]

    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsPlayer;

    [Header("Settings")]
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _chaseSpeed, _patrolSpeed;
    [SerializeField] private float _patrolRange;
    [SerializeField] private int _itemDropRate = 1;

    private bool _enabled;
    private bool _shouldEnable;

    //States
    [SerializeField] private float sightRange, attackRange;
    [SerializeField] private float _ragDollTime = 10;
    [Header("Debug")]
    [SerializeField] private bool playerInSightRange;
    private bool playerNotWayOutOfSightRange;
    [SerializeField] private bool playerInAttackRange;
    //Patroling
    [SerializeField] private Vector3 walkPoint;
    [SerializeField] private bool walkPointSet;

    [Header("Sounds")]
    private AudioSource _enemyAudioSource;
    [SerializeField] private AudioClip[] _deathSounds;
    [SerializeField] private AudioClip[] _onSightSounds;
    [SerializeField] private Audio _soundEffects;

    //Lerp when starting
    private float lerpedValue;
    private float duration = 3;
    private float totalScale = 1;
    private bool isLerping;
    [NonSerialized] public bool isDead;
    private float _timer;
    private bool _itemDropped = false;
    private Animator _animator;
    private int _maxSkullSpawnRate;
    private int _randomOnSightSound;
    private int _randomDeathSound;
    private bool _playOnSightOnce = false;
    private Collider _collider;

    private void Start()
    {
        player = GameObject.Find("PLAYER").transform;
        agent = GetComponent<NavMeshAgent>();
        _enabled = false;
        agent.enabled = false;
        lerpedValue = this.transform.position.y;
        _maxSkullSpawnRate = 1/*UnityEngine.Random.Range(0, _itemDropRate)*/;
        _randomDeathSound = UnityEngine.Random.Range(0, 3);
        _randomOnSightSound = UnityEngine.Random.Range(0, 6);
        _animator = GetComponentInChildren<Animator>();
        _enemyAudioSource = GetComponent<AudioSource>();
        _animator.SetInteger("AtackIndex", UnityEngine.Random.Range(0, 2));
        _collider = GetComponentInChildren<Collider>();
        _soundEffects = GameObject.Find("Music").GetComponent<Audio>();
        _enemyAudioSource.volume = _soundEffects._sfxVolume;
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
    }

    private void Update()
    {
        EnableWhenNeeded();
        //Check for sight and attack range
        if (agent.enabled)
        {
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerNotWayOutOfSightRange = Physics.CheckSphere(transform.position, sightRange * 2, whatIsPlayer);

            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
            if (!playerInSightRange && !playerInAttackRange && playerNotWayOutOfSightRange) Patroling();
            if (agent.velocity.magnitude > 0 && !playerInSightRange && !isDead) _animator.SetBool("Walk", true);
            if (agent.velocity.magnitude == 0 && !playerInSightRange && !isDead) _animator.SetBool("Walk", false);
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            if (playerInSightRange && !isDead)
            {
                if (!_playOnSightOnce)
                {
                    _enemyAudioSource.PlayOneShot(_onSightSounds[_randomOnSightSound]);
                    _playOnSightOnce = true;
                }
                _animator.SetBool("Run", true);
            }
            if (!playerInSightRange && !isDead)
            {
                _animator.SetBool("Run", false);
                _playOnSightOnce = false;

            }

            if (playerInAttackRange) AttackPlayer();
            if (!playerInAttackRange) _animator.SetBool("Atack", false);
            //if (playerInAttackRange && !isDead) _animator.SetBool("Atack", true);

        }
        else if (!agent.enabled & isLerping)
        {
            //lerp above the ice
            this.transform.position = new Vector3(this.transform.position.x, lerpedValue, this.transform.position.z);
        }

        if (isDead)
        {
            _animator.SetBool("Die", true);
            DropItem();
            DespawnAfterSeconds();
            _collider.enabled = false;
            //_enemyAudioSource.volume = Mathf.Lerp(_enemyAudioSource.volume, 0, 10 * Time.deltaTime); 
            //TODO: Lerp volume down to 0 after kill
        }
    }

    private void DropItem()
    {
        if (_itemDropped == false)
        {
            if (_maxSkullSpawnRate == 1)
            {
                GameObject pickup = Instantiate(_dropItem, this.transform.position, Quaternion.identity);
            }
            _itemDropped = true;
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
        Vector2 distanceToWalkPoint = new(transform.position.x - walkPoint.x, transform.position.z - walkPoint.z);

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1.5f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = UnityEngine.Random.Range(-_patrolRange, _patrolRange);
        float randomX = UnityEngine.Random.Range(-_patrolRange, _patrolRange);

        NavMeshHit hit;
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        for (int i = 0; i < 10; i++)
        {
            if (!NavMesh.SamplePosition(walkPoint, out hit, 2f, NavMesh.AllAreas))
            {
                randomX = UnityEngine.Random.Range(-_patrolRange, _patrolRange);
                randomZ = UnityEngine.Random.Range(-_patrolRange, _patrolRange);
                walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
                continue;
            }
            NavMeshPath navpath = new NavMeshPath();
            if (NavMesh.CalculatePath(transform.position, hit.position, NavMesh.AllAreas, navpath)) // if theres a path
            {
                if (navpath.status == NavMeshPathStatus.PathPartial || navpath.status == NavMeshPathStatus.PathInvalid) //if its fucked
                    continue; // redo
            }

            walkPoint = hit.position;
            break;
        }  // find a position on the navmesh

        if (Physics.Raycast(walkPoint + new Vector3(0, 0.1f, 0), -transform.up, 2f, whatIsGround))
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
        _animator.SetBool("Atack", true);
        agent.velocity = Vector3.zero;
        //Debug.Log("attack animation");
    }

    public void DespawnAfterSeconds()
    {
        if (_timer <= _ragDollTime)
            _timer += Time.deltaTime;

        if (_timer > _ragDollTime) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
    public void PlayDeathSound()
    {
        _enemyAudioSource.PlayOneShot(_deathSounds[_randomDeathSound]);
    }
    public void PlayRandomAtackSound()
    {
        _enemyAudioSource.PlayOneShot(_onSightSounds[_randomOnSightSound]);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

}


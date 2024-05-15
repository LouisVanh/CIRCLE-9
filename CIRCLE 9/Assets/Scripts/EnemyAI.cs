using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Transform _player;
    private PlayerBehaviour _playerBehaviour;

    [Header("Prefab")]
    [SerializeField] private GameObject _dropItem;
    [SerializeField] private GameObject _dropShotgun;

    [Header("Layers")]

    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsPlayer;

    [Header("Settings")]
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _chaseSpeed, _patrolSpeed;
    [SerializeField] private float _patrolRange;
    [SerializeField] private int _itemDropRate;

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
    private int _randomSkin;
    private int _randomDeathSound;
    private bool _playOnSightOnce = false;
    private Collider _collider;
    private int _deathCounter =0;

    private bool _outOfCamera; // MAKE SURE THE SCENE CAMERA IS NOT POINTING AT THE SCENE!!!!!!!!!!!!


    private void Start()
    {

        _player = GameObject.Find("PLAYER").transform;
        _playerBehaviour = GameObject.Find("PLAYER").GetComponent<PlayerBehaviour>();
        _agent = GetComponent<NavMeshAgent>();
        _enabled = false;
        _agent.enabled = false;
        lerpedValue = this.transform.position.y;
        _maxSkullSpawnRate = UnityEngine.Random.Range(1, _itemDropRate);
        _randomDeathSound = UnityEngine.Random.Range(0, 3);
        _randomOnSightSound = UnityEngine.Random.Range(0, 6);
        _randomSkin = UnityEngine.Random.Range(0, 3);

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
        EnableWhenNeeded();
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
        //Check for sight and attack range
        if (_agent.enabled)
        {
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerNotWayOutOfSightRange = Physics.CheckSphere(transform.position, sightRange * 2, whatIsPlayer);

            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
            if (!playerInSightRange && !playerInAttackRange && playerNotWayOutOfSightRange) Patroling();
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            if (playerInAttackRange) AttackPlayer();
            Animations();
        }
        else if (!_agent.enabled & isLerping)
        {
            //lerp above the ice
            this.transform.position = new Vector3(this.transform.position.x, lerpedValue, this.transform.position.z);
        }
        DespawnAfterSeconds();
    }

    public void OnDeath()
    {
        if (isDead)
        {
            if(_deathCounter ==0)
            {
                _playerBehaviour.AmountOfKills++;
                _deathCounter= 1;
                DropShotGun();
            }          
            _animator.SetBool("Die", true);
            DropItem();
            //DespawnAfterSeconds();
            //_collider.enabled = false;
            //_enemyAudioSource.volume = Mathf.Lerp(_enemyAudioSource.volume, 0, 10 * Time.deltaTime); 
            //TODO: Lerp volume down to 0 after kill
        }
    }
    private void DropShotGun()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(1))
        {// main scene 

            if (_playerBehaviour.AmountOfKills == 10)
            {
                GameObject shotgun = Instantiate(_dropShotgun, this.transform.position, Quaternion.identity);
            }
        }
    }

    private void Animations()
    {
        if (_agent.velocity.magnitude > 0 && !playerInSightRange && !isDead) _animator.SetBool("Walk", true);
        if (_agent.velocity.magnitude == 0 && !playerInSightRange && !isDead) _animator.SetBool("Walk", false);
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
        if (!playerInAttackRange) _animator.SetBool("Atack", false);
    }

    private void DropItem()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(1))
        {// main scene (else only spawn after the animation with satan

            if (_itemDropped == false)
            {
                if (_maxSkullSpawnRate == 1)
                {
                    GameObject pickup = Instantiate(_dropItem, this.transform.position, Quaternion.identity);
                }
                _itemDropped = true;
            }
        }
    }

    private void Patroling()
    {
        if (!walkPointSet && !_outOfCamera) SearchWalkPoint();

        if (walkPointSet)
        {
            _agent.speed = _patrolSpeed;
            _agent.SetDestination(walkPoint);
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
                if (navpath.status == NavMeshPathStatus.PathPartial || navpath.status == NavMeshPathStatus.PathInvalid) //if there's no valid path
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
        _agent.speed = _chaseSpeed;
        _agent.SetDestination(_player.position);
        _agent.isStopped = false;
    }

    private void AttackPlayer()
    {
        _animator.SetBool("Atack", true);
        _agent.velocity = Vector3.zero;
    }

    public void DespawnAfterSeconds()
    {
        if (isDead)
        {
            if (_timer <= _ragDollTime)
                _timer += Time.deltaTime;

            if(_timer > _ragDollTime - 1) _collider.enabled = false; // fall thru floor
            if (_timer > _ragDollTime) Invoke(nameof(DestroyEnemy), 0.5f);
        }
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
    public void DamagePlayer()
    {
        if (playerInAttackRange)
        {
            _playerBehaviour.AddHealth(-15);
        }
    }

    public void OnBecameInvisible()
    {
        _outOfCamera = true;
    }
    public void OnBecameVisible()
    {
        _outOfCamera = false;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyWave : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _timeBetweenWaves = 30;
    [SerializeField] private int _amountZombiesPerWave = 50;
    [SerializeField] private int _maxAmountOfWaves = 5;

    [Header("Debug")]
    [SerializeField] private float _timer;
    [SerializeField] private int _waveCount;

    [Header("MapSize")]
    [SerializeField] private int LeftBottomX;
    [SerializeField] private int RightBottomX;
    [SerializeField] private int LeftTopZ;
    [SerializeField] private int RightTopZ;

    [Header("EnemyPrefab")]
    [SerializeField] private GameObject _enemy;
    [SerializeField] private GameObject _enemy1;
    [SerializeField] private GameObject _enemy2;


    private void Start()
    {
        _waveCount = 1;
        if(SceneManager.GetActiveScene()==SceneManager.GetSceneByBuildIndex(1)) // main scene (else only spawn after the animation with satan
        _timer = 25;
        else { _timer = 15; }
    }
    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > _timeBetweenWaves && _waveCount < _maxAmountOfWaves)
        {
            SpawnWave(_waveCount * _amountZombiesPerWave);
            _waveCount++;
            _timer = 0;
        }

    }
    private void SpawnWave(int nEnemies)
    {
        for (int i = 0; i < nEnemies; i++)
        {
            int randomX = Random.Range(LeftBottomX, RightBottomX);
            int randomZ = Random.Range(LeftTopZ, RightTopZ);

            Vector3 spawnPos = new Vector3(randomX, -1, randomZ);
            NavMeshHit hit;
            for (int j = 0; j < 10; j++)
            {
                if (!NavMesh.SamplePosition(new Vector3(spawnPos.x, 0, spawnPos.z), out hit, 2f, NavMesh.AllAreas)) // find position
                {
                    randomX = Random.Range(LeftBottomX, RightBottomX);
                    randomZ = Random.Range(LeftTopZ, RightTopZ);

                    spawnPos = new Vector3(randomX, -1, randomZ);
                    continue;
                }

                spawnPos = new Vector3(hit.position.x, -1, hit.position.z);

                if (Physics.CheckBox(spawnPos, Vector3.up * 30, Quaternion.identity, 1 << 8)) // check if it collides with objects above (inside a model: 8)
                {
                    continue;
                }

                Vector3 randomRot = new(Random.Range(-30, 30), Random.Range(-30, 30), Random.Range(-30, 30));
                var randomSkin = Random.Range(0, 3);
                if (randomSkin == 1)
                {
                    var enemy = Instantiate(_enemy, spawnPos, Quaternion.Euler(randomRot));
                    enemy.GetComponent<EnemyAI>().SubmergeOutIce(hit.position.y + 0.8f); // O.8f = enemy offset to spawn on feet
                }

                if (randomSkin == 2)
                {
                    var enemy = Instantiate(_enemy1, spawnPos, Quaternion.Euler(randomRot));
                    enemy.GetComponent<EnemyAI>().SubmergeOutIce(hit.position.y + 0.8f); // O.8f = enemy offset to spawn on feet
                }

                if (randomSkin == 3)
                {
                    var enemy = Instantiate(_enemy2, spawnPos, Quaternion.Euler(randomRot));
                    enemy.GetComponent<EnemyAI>().SubmergeOutIce(hit.position.y + 0.8f); // O.8f = enemy offset to spawn on feet
                }

                break;
            }
        }
    }
}

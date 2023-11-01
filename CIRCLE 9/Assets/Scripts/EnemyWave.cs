using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWave : MonoBehaviour
{
    [SerializeField] private float _timer;
    [SerializeField] private int _waveCount;
    [SerializeField] private int LeftBottomX;
    [SerializeField] private int RightBottomX;
    [SerializeField] private int LeftTopZ;
    [SerializeField] private int RightTopZ;
    [SerializeField] private GameObject _enemy;

    private void Start()
    {
        _waveCount = 1;
        _timer = 25;
    }
    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > 30)
        {
            SpawnWave(_waveCount * 100);
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
            while (!NavMesh.SamplePosition(new Vector3(spawnPos.x, 0, spawnPos.z), out hit, 2f, NavMesh.AllAreas))  // find a position on the navmesh
            {
                randomX = Random.Range(LeftBottomX, RightBottomX);
                randomZ = Random.Range(LeftTopZ, RightTopZ);

                spawnPos = new Vector3(randomX, -1, randomZ);
            }
            spawnPos = new Vector3(hit.position.x, -1, hit.position.z);
            var enemy = Instantiate(_enemy, spawnPos, Quaternion.identity);
            enemy.GetComponent<EnemyAI>().SubmergeOutIce(hit.position.y + 0.8f); // O.8f = enemy offset to spawn on feet
        }
    }
}

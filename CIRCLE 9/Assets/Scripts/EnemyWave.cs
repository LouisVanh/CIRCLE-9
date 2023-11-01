using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            var enemy = Instantiate(_enemy, spawnPos, Quaternion.identity);
            //_enemy.GetComponent<EnemyAI>().SubmergeOutIce();
            enemy.GetComponent<EnemyAI>().SubmergeOutIce();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SatanMinions : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject Satan;
    [Header("EnemyPrefab")]
    [SerializeField] private GameObject _enemy;
    [SerializeField] private GameObject _enemy1;
    [SerializeField] private GameObject _enemy2;

    [Header("Debug")]
    [SerializeField] private float _timer;
    [SerializeField] private float _timeBetweenWaves;
    [SerializeField] private float radius;
    private float posY;

    void Start()
    {
        Satan = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if(_timer > _timeBetweenWaves)
        {
            SpawnMinionsAroundHim(10);
            _timer = 0;
        }
    }

    private void SpawnMinionsAroundHim(int n)
    {
        Vector3 center = Satan.transform.position;
        for (int i = 0; i < n; i++)
        {
            //var fullCircle = 2 * Mathf.PI;
            var fullCircle = 360;
            float angle = (i * (fullCircle / (n)) );
            float rx = Mathf.Cos(angle);
            float rz = Mathf.Sin(angle);

            Vector3 spawnPos = new Vector3(center.x + radius * rx, -20, center.z + radius * rz);

            //NavMeshHit hit;
            //if (!NavMesh.SamplePosition(new Vector3(spawnPos.x, 0, spawnPos.z), out hit, 2f, NavMesh.AllAreas)) // find position
            //{
            //    randomX = Mathf.Cos(fullCircle/ i);
            //    randomZ = Mathf.Sin(fullCircle / i);

            //    spawnPos = new Vector3(center.x + radius * randomX, -1, center.z + radius * randomZ);
            //    continue;
            //}
            //spawnPos = new Vector3(hit.position.x, -1, hit.position.z);


            Ray ray = new Ray(spawnPos, Vector3.up);
            if(Physics.Raycast(ray, out RaycastHit hit, 50))
            {
                Debug.Log("RAYCASTING");
                posY = hit.point.y;
                spawnPos = new Vector3(hit.point.x, -5, hit.point.z);
            }

            var randomSkin = Random.Range(0, 3);
            if (randomSkin == 1)
            {
                var enemy = Instantiate(_enemy, spawnPos, Quaternion.identity);
                enemy.GetComponent<EnemyAI>().SubmergeOutIce(posY + 0.8f); // O.8f = enemy offset to spawn on feet
            }

            if (randomSkin == 2)
            {
                var enemy = Instantiate(_enemy1, spawnPos, Quaternion.identity);
                enemy.GetComponent<EnemyAI>().SubmergeOutIce(posY + 0.8f); // O.8f = enemy offset to spawn on feet
            }

            if (randomSkin == 3)
            {
                var enemy = Instantiate(_enemy2, spawnPos, Quaternion.identity);
                enemy.GetComponent<EnemyAI>().SubmergeOutIce(posY + 0.8f); // O.8f = enemy offset to spawn on feet
            }
        }
    }
}

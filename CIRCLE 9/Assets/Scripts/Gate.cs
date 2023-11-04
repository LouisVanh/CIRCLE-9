using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private PlayerBehaviour _player;
    [SerializeField] private int _amountOfSkulls = 5;
    [SerializeField] private GameObject _skull;
    [SerializeField] private Transform _pos1;
    [SerializeField] private Transform _pos2;
    [SerializeField] private Transform _pos3;
    [SerializeField] private Transform _pos4;
    [SerializeField] private Transform _pos5;
    [SerializeField] private Transform _pos6;
    [SerializeField] private List<Transform> _positions;



    void Start()
    {
        _positions.Add(_pos1);
        _positions.Add(_pos2);
        _positions.Add(_pos3);
        _positions.Add(_pos4);
        _positions.Add(_pos5);
        _positions.Add(_pos6);

    }

    public void SpawnSkullOnGatePosition(int i)
    {
        Instantiate(_skull, _positions[i].position, Quaternion.identity);
    }
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("PLAYER AT GATE");
            
        }
    }
}

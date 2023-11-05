using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] private PlayerBehaviour _player;
    private Vector3 _playerPos;
    private void Awake()
    {
        _player = GameObject.Find("PLAYER").GetComponent<PlayerBehaviour>();
        _playerPos = _player.transform.position;

    }
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _playerPos, 10 * Time.deltaTime);
        if (transform.position == _playerPos)
        {
            Destroy(gameObject);
        }
    }
}

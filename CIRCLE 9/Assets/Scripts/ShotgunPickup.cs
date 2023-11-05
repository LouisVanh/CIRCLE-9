using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunPickup : MonoBehaviour
{
    private PlayerBehaviour _player;
    [SerializeField] private Collider _collider;
    private float _timer = 0;

    private void Start()
    {
        _collider.isTrigger = false;
    }
    private void Update()
    {
        _timer += Time.deltaTime;
        if(_timer >= 1)
        {
            _collider.isTrigger = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            _player = GameObject.Find("PLAYER").GetComponent<PlayerBehaviour>();
            _player.ShotgunPickup();

            Destroy(gameObject);
        }
    }
}

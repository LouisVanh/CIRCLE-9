using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private PlayerBehaviour _player;
    bool _shouldPickup;
    private void Start()
    {
        //_player = GameObject.Find("PLAYER").GetComponent<PlayerBehaviour>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            _player.SkullPickup();
            Destroy(gameObject);
        }
    }
    //private void LateUpdate()
    //{
    //    if (_shouldPickup)
    //    {
    //        _player.SkullPickup();
    //        Destroy(gameObject);
    //    }
    //}
}

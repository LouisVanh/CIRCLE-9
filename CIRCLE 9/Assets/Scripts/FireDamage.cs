using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDamage : MonoBehaviour
{
    private PlayerBehaviour _player;
    private bool _isOnFire;
    void Start()
    {
        _player = GameObject.Find("PLAYER").GetComponent<PlayerBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isOnFire)
        {
            _player.AddHealth(-10 * Time.deltaTime);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            _isOnFire = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            _isOnFire = false;
        }
    }
}

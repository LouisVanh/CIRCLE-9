using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunPickup : MonoBehaviour
{
    [SerializeField] private PlayerBehaviour _player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            _player.ShotgunPickup();
            Destroy(gameObject);
        }
    }
}

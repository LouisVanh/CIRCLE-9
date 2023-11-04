using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunPickup : MonoBehaviour
{
    [SerializeField] private PlayerBehaviour _player;
    //[SerializeField] private SkullCountUI _skullCountUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            _player.ShotgunPickup();
            //_skullCountUI.ShouldGrow = true;
            Destroy(gameObject);
        }
    }
}

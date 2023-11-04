using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private PlayerBehaviour _player;
    private SkullCountUI _skullCountUI;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            _player = GameObject.Find("PLAYER").GetComponent<PlayerBehaviour>();
            _skullCountUI = GameObject.Find("SkullCounter").GetComponentInChildren<SkullCountUI>();
            _player.SkullPickup();
            _skullCountUI.ShouldGrow = true;

            Destroy(gameObject);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpew : MonoBehaviour
{
    private float _timer;

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= 5)
        {
            Destroy(gameObject);
        }
    }
}

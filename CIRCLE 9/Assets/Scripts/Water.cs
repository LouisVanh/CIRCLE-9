using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] private Material _waterTexture;
    void Update()
    {
        _waterTexture.mainTextureOffset += new Vector2(0, 0.1f) * Time.deltaTime;
    }
}

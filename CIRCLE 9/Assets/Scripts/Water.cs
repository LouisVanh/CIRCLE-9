using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Material _waterTexture;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _waterTexture.mainTextureOffset += new Vector2(0, 0.2f) * Time.deltaTime;
    }
}

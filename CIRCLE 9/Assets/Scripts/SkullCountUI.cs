using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkullCountUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _shownAmount;
    [SerializeField] private PlayerBehaviour _playerBehaviour;
    [SerializeField] private RawImage _image;
    private float size = 1;
    private bool _scale = false;
    public bool ShouldGrow;

    public void Update()
    {
        if (ShouldGrow) // set to true in script
        {
            float velocity = 0.0f;
            if (_scale == false)
            {
                size = Mathf.SmoothDamp(size, 2, ref velocity, 0.05f);
                if (size > 1.9f) _scale = true;
            }
            if (_scale == true)
            {
                size = Mathf.SmoothDamp(size, 1, ref velocity, 0.05f);
                if (size < 1.1f)
                {
                    _scale = false;
                    ShouldGrow = false;
                }
            }
            _image.gameObject.transform.localScale = new Vector3(size, size, size);
        }
        _shownAmount.SetText("x" + _playerBehaviour.SkullAmount.ToString());
    }
}

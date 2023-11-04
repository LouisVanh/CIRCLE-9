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
    private int _previousAmount = 0;
    private int _currentAmount = 0;
    private float size = 1;
    private bool _scale = false;
    public bool ShouldGrow;

    public void Update()
    {
        _currentAmount = _playerBehaviour.SkullAmount;
        if (ShouldGrow) // set to true in script
        {
            if (_previousAmount < _currentAmount && _scale == false)
            {
                size = Mathf.Lerp(1, 5, 5 * Time.deltaTime);
                if (size == 5) _scale = true;
            }
            if (_scale == true)
            {
                _previousAmount = _currentAmount;
                size = Mathf.Lerp(5, 1, 5 * Time.deltaTime);
                if (size == 1)
                {
                    _scale = false;
                    ShouldGrow = false;
                }
            }
            _image.gameObject.transform.localScale = new Vector3(size, size, size);
        }
        //TODO: WHY IS THIS NOT WORKING IM GONNA KILL MYSELF
        //picture needs to row in size and shrink again after a skull has been picked up

        _shownAmount.SetText("x" + _playerBehaviour.SkullAmount.ToString());
    }
}

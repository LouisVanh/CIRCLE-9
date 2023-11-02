using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkullCountUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _shownAmount;
    [SerializeField] private PlayerBehaviour _playerBehaviour;

    public void Update()
    {
        _shownAmount.SetText("x" + _playerBehaviour.SkullAmount.ToString());
    }
}

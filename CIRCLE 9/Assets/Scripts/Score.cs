using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    private int _score;
    [SerializeField] private PlayerBehaviour _playerBehaviour;
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    private void Start()
    {
        _textMeshPro = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        _score = _playerBehaviour.AmountOfKills * 100;
        _textMeshPro.SetText("Score:" + _score.ToString());

    }
}

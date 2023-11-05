using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public int gameScore;
    [SerializeField] private PlayerBehaviour _playerBehaviour;
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    private void Start()
    {
        _textMeshPro = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        gameScore = _playerBehaviour.AmountOfKills * 100;
        _textMeshPro.SetText("Score:" + gameScore.ToString());

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private RectTransform _healthBar;
    public float _health;
    public float _maxHealth;
    public float _width;
    public float _height;
    
    public void SetMaxHealth(float maxHealth)
    {
        _maxHealth= maxHealth;
    }

    public void SetHealth(float health)
    {
        _health= health;
        float newWidth = (_health / _maxHealth) * _width;

        _healthBar.sizeDelta = new Vector2 (newWidth, _height);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satan : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HealthBarUI _healthBar;
    [Header("Settings")]
    [SerializeField] private float _health;
    [SerializeField] private float _maxHealth;
    [SerializeField] private Gate _gate;
    [NonSerialized] public bool HasDied = false;
    public bool IsSpewingFire;
    void Start()
    {
        _healthBar = GameObject.Find("SatanHealthBar").GetComponent<HealthBarUI>();
        _healthBar.SetMaxHealth(_maxHealth);
    }
    void Update()
    {
        //if (!HasDied)
        //{
        //    CheckDeath();
        //}
    }

    private void CheckDeath()
    {
        if (_health <= 0)
        {
            HasDied = true;
        }
    }

    public void AddHealth(float healthChange)
    {
        _health += healthChange;
        _health = Mathf.Clamp(_health, 0, _maxHealth);
        _healthBar.SetHealth(_health);
        CheckDeath();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Satan : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HealthBarUI _healthBar;
    [Header("Settings")]
    [SerializeField] private float _health;
    [SerializeField] private float _maxHealth;
    [SerializeField] private Gate _gate;
    [NonSerialized] public bool HasDied = false;
    [SerializeField] GameObject _fireSpew;
    [SerializeField] GameObject _fireBall;
    [SerializeField] Transform _fireSpewPos;
    private float _fireBallTimer;
    private float _fireSpewTimer;
    public bool IsSpewingFire;
    void Start()
    {
        _healthBar = GameObject.Find("SatanHealthBar").GetComponent<HealthBarUI>();
        _healthBar.SetMaxHealth(_maxHealth);
        _fireSpewTimer = 19;
    }
    void Update()
    {
        _fireSpewTimer += Time.deltaTime;
        if (_fireSpewTimer >= 20)
        {
            //Vector3 pos = new Vector3(transform.position.x,transform.position.y + 13, 2);
            //Vector3 direction = new Vector3(-270, 0, 60);
            //Quaternion quatDirection = Quaternion.Euler(direction);
            GameObject fireSpew = Instantiate(_fireSpew, _fireSpewPos);
            _fireSpewTimer = 0;
        }


        _fireBallTimer += Time.deltaTime;
        if (_fireBallTimer >= 10)
        {
            GameObject fireball = Instantiate(_fireBall, _fireSpewPos);
            _fireBallTimer = 0;
        }

        if (!HasDied)
        {
            CheckDeath();
        }
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

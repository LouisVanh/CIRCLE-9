using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Shotgun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ParticleSystem m_MuzzleFlashParticle;
    [SerializeField] private AudioSource _gunShotAudioSource;
    [SerializeField] private AudioClip _gunShotSound;
    [SerializeField] private AudioClip _shellIceSound;
    [SerializeField] private AudioClip _shellRockSound;

    public Audio _sfxSettings;   
    [Header("Settings")]
    [SerializeField] private int _bulletKnockback = 100000;
    [SerializeField] private int _maxShootDistance = 15;

    //private bool _pickup = true;
    private ParticleSystem _vfx;
    private Animator m_Animator;
    [SerializeField] private float _shotGunSoundAmplify = 2f;
    [SerializeField] private int _maxShots = 2;
    [SerializeField] private int _amountOfBulletsShot = 1;

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        _sfxSettings = GameObject.Find("Music").GetComponent<Audio>();
        _gunShotAudioSource.volume = _sfxSettings._sfxVolume * _shotGunSoundAmplify;
    }
    private void Update()
    {
        RunningAnimation();
        if (Input.GetMouseButtonDown(0) && m_Animator.GetBool("Shoot") == false && _amountOfBulletsShot <= _maxShots)
        {
            _amountOfBulletsShot++;
            PlayAnimation();
            Shoot();
        }
        if (_amountOfBulletsShot > _maxShots || Input.GetKeyDown(KeyCode.R) && _amountOfBulletsShot > 1)
        {
            ReloadAnimationTrue();
        }
    }

    private void ReloadAnimationTrue()
    {
        m_Animator.SetBool("Reload", true);
        //m_Animator.SetBool("Shoot", false);
        //TODO: check if ground is ice / rock and play according shell sound, but model has to be done first
    }

    private void RunningAnimation()
    {
        if (GetComponentInParent<PlayerBehaviour>()._isSprinting)
        {
            m_Animator.SetBool("Run", true);

        }
        else
        {
            m_Animator.SetBool("Run", false);

        }
    }
    private void ReloadAnimationFalse()
    {
        _gunShotAudioSource.PlayOneShot(_shellIceSound);
        _amountOfBulletsShot = 1;
        m_Animator.SetBool("Reload", false);
    }
    private void ShootAnimationFalse()
    {
        m_Animator.SetBool("Shoot", false);
    }

    private void PlayAnimation()
    {
        m_Animator.SetBool("Shoot", true);
        m_MuzzleFlashParticle.Play();
    }

    private void Shoot()
    {
        _gunShotAudioSource.PlayOneShot(_gunShotSound);
        Ray ray1 = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        Ray ray2 = new Ray(Camera.main.transform.position, Camera.main.transform.forward + Camera.main.transform.right);
        Ray ray3 = new Ray(Camera.main.transform.position, Camera.main.transform.forward - Camera.main.transform.right);
        Ray ray4 = new Ray(Camera.main.transform.position, Camera.main.transform.forward + 0.5f* Camera.main.transform.right);
        Ray ray5 = new Ray(Camera.main.transform.position, Camera.main.transform.forward - 0.5f* Camera.main.transform.right);
        Ray ray6 = new Ray(Camera.main.transform.position, Camera.main.transform.forward + 0.25f * Camera.main.transform.right);
        Ray ray7 = new Ray(Camera.main.transform.position, Camera.main.transform.forward - 0.25f * Camera.main.transform.right);
        Ray ray8 = new Ray(Camera.main.transform.position, Camera.main.transform.forward + 0.125f * Camera.main.transform.right);
        Ray ray9 = new Ray(Camera.main.transform.position, Camera.main.transform.forward - 0.125f * Camera.main.transform.right);
        ShootAlongThisRay(ray1);
        ShootAlongThisRay(ray2);
        ShootAlongThisRay(ray3);
        ShootAlongThisRay(ray4);
        ShootAlongThisRay(ray5);
        ShootAlongThisRay(ray6);
        ShootAlongThisRay(ray7);
        ShootAlongThisRay(ray8);
        ShootAlongThisRay(ray9);
    }

    private void ShootAlongThisRay(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit, _maxShootDistance))
        {
            //Debug.DrawRay(Camera.main.transform.position, ray.direction, Color.red, 10);
            if (hit.transform.gameObject.layer == 7) // Enemy : 7
            {
                PlayVFXAtPoint(hit);

                if (hit.transform.gameObject.GetComponent<Animator>()) // turn off animations
                {
                    Debug.Log("Animator component detected");
                    hit.transform.gameObject.GetComponent<Animator>().enabled = false;
                }

                if (hit.transform.gameObject.GetComponent<Rigidbody>() == null) // add ragdoll
                {
                    var rb = hit.transform.gameObject.AddComponent<Rigidbody>();
                    hit.transform.gameObject.GetComponent<NavMeshAgent>().enabled = false;
                    hit.transform.gameObject.GetComponent<EnemyAI>().isDead = true;
                    var distanceBetweenEnemyAndPlayer = Vector3.Distance(Camera.main.transform.position, hit.point);
                    rb.AddForce(Camera.main.transform.forward.normalized * _bulletKnockback / distanceBetweenEnemyAndPlayer);
                }

            }
        }
    }

    private void PlayVFXAtPoint(RaycastHit hit)
    {
        _vfx = hit.transform.gameObject.GetComponentInChildren<ParticleSystem>();
        _vfx.transform.position = hit.point;
        _vfx.Play(true);
    }
}

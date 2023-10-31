using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Shotgun : MonoBehaviour
{
    [SerializeField] private int _bulletKnockback = 100000;
    private Animator m_Animator;
    //private bool _pickup = true;
    [SerializeField] private Transform PlayerHandPos;
    [SerializeField] private ParticleSystem m_MuzzleFire;
    [SerializeField] private AudioSource _gunAudioSource;
    [SerializeField] private AudioClip _gunShot;
    private ParticleSystem _vfx;

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlayAnimation();
            Shoot();
        }
    }

    private void PlayAnimation()
    {
        m_Animator.SetTrigger("Shoot");
        m_MuzzleFire.Play();
    }

    private void Shoot()
    {
        _gunAudioSource.PlayOneShot(_gunShot);
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
        if (Physics.Raycast(ray, out RaycastHit hit, 10))
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

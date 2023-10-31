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
    private void Start()
    {
        m_Animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_Animator.SetTrigger("Shoot");
            m_MuzzleFire.Play();
            Debug.Log("shoot");
            Shoot();
        }

        //transform.parent = PlayerHandPos;
    }

    private void Shoot()
    {
        _gunAudioSource.PlayOneShot(_gunShot);
        Ray ray1 = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        ShootAlongThisRay(ray1);
        Ray ray2 = new Ray(Camera.main.transform.position, Camera.main.transform.forward + Camera.main.transform.right);
        ShootAlongThisRay(ray2);
        Ray ray3 = new Ray(Camera.main.transform.position, Camera.main.transform.forward - Camera.main.transform.right);
        ShootAlongThisRay(ray3);
        Ray ray4 = new Ray(Camera.main.transform.position, Camera.main.transform.forward + 0.5f* Camera.main.transform.right);
        ShootAlongThisRay(ray4);
        Ray ray5 = new Ray(Camera.main.transform.position, Camera.main.transform.forward - 0.5f* Camera.main.transform.right);
        ShootAlongThisRay(ray5);
    }

    private void ShootAlongThisRay(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit, 10))
        {
            Debug.DrawRay(Camera.main.transform.position, ray.direction, Color.red, 10);
            Debug.Log("Raycasting");
            if (hit.transform.gameObject.layer == 7) // Enemy : 7
            {
                Debug.Log("Enemy detected");

                if (hit.transform.gameObject.GetComponent<Animator>())
                {
                    Debug.Log("Animator component detected");

                    hit.transform.gameObject.GetComponent<Animator>().enabled = false;
                }
                if (hit.transform.gameObject.GetComponent<Rigidbody>() == null)
                {
                    var rb = hit.transform.gameObject.AddComponent<Rigidbody>();
                    hit.transform.gameObject.GetComponent<NavMeshAgent>().enabled = false;
                    rb.AddForce(Camera.main.transform.forward.normalized * _bulletKnockback);
                }

            }
        }
    }
}

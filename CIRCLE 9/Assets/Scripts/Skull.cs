using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Skull : MonoBehaviour
{
    private Animator m_Animator;
    private ParticleSystem _vfx;
    [SerializeField] private int _smashKnockBack = 100000;
    [SerializeField] private int _maxSmashDistance = 3;
    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && m_Animator.GetBool("Smash") == false)
        {
            Debug.Log("smashing");
            m_Animator.SetBool("Smash", true);
        }

    }
    private void AnimationFalse()
    {
        m_Animator.SetBool("Smash", false );
    }

    private void Smash()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        Ray ray2 = new Ray(Camera.main.transform.position, Camera.main.transform.forward + Camera.main.transform.right);
        Ray ray3 = new Ray(Camera.main.transform.position, Camera.main.transform.forward - Camera.main.transform.right);
        SmashAlongThisRay(ray);
        SmashAlongThisRay(ray2);
        SmashAlongThisRay(ray3);

    }

    private void SmashAlongThisRay(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit, _maxSmashDistance))
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
                    rb.AddForce(Camera.main.transform.forward.normalized * _smashKnockBack / distanceBetweenEnemyAndPlayer);
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

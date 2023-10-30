using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
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
    }
}

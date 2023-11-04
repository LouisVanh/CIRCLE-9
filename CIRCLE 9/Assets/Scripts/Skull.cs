using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Skull : MonoBehaviour
{
    private Animator m_Animator;
    private ParticleSystem _vfx;
    [SerializeField] private int _smashKnockBack = 100000;
    [SerializeField] private int _maxSmashDistance = 3;
    private AudioSource _skullAudioSource;
    [SerializeField] private AudioClip _skullBashSound;
    [SerializeField] private AudioClip _skullWhooshSound;
    private PlayerBehaviour _player;
    private Audio _sfxSettings;

    // Start is called before the first frame update
    void Start()
    {
        _player = GetComponentInParent<PlayerBehaviour>();
        m_Animator = GetComponent<Animator>();
        _skullAudioSource = GetComponent<AudioSource>();
        _sfxSettings = GameObject.Find("Music").GetComponent<Audio>();
        _skullAudioSource.volume = _sfxSettings._sfxVolume * 4;
        //gameObject.transform.parent.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && m_Animator.GetBool("Smash") == false)
        {
            m_Animator.SetBool("Smash", true);
            _skullAudioSource.PlayOneShot(_skullWhooshSound, 0.4f);
        }   
    }
    private void AnimationFalse()
    {
        //called in animation
        m_Animator.SetBool("Smash", false );
    }

    private void Smash()
    {
        //called in animation
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
                _player.AmountOfKills++;
                _skullAudioSource.PlayOneShot(_skullBashSound);
                PlayVFXAtPoint(hit);
                hit = TurnOffAnimation(hit);
                if (hit.transform.gameObject.GetComponent<Rigidbody>() == null) // add ragdoll
                {
                    var rb = hit.transform.gameObject.AddComponent<Rigidbody>();
                    rb.freezeRotation = true;
                    hit.transform.gameObject.GetComponent<NavMeshAgent>().enabled = false;
                    hit.transform.gameObject.GetComponent<EnemyAI>().isDead = true;
                    var distanceBetweenEnemyAndPlayer = Vector3.Distance(Camera.main.transform.position, hit.point);
                    rb.AddForce(Camera.main.transform.forward.normalized * _smashKnockBack / distanceBetweenEnemyAndPlayer);
                }

            }
        }
    }

    private static RaycastHit TurnOffAnimation(RaycastHit hit)
    {
        if (hit.transform.gameObject.GetComponent<Animator>()) // turn off animations
        {
            Debug.Log("Animator component detected");
            hit.transform.gameObject.GetComponent<Animator>().enabled = false;
        }

        return hit;
    }

    private void PlayVFXAtPoint(RaycastHit hit)
    {
        _vfx = hit.transform.gameObject.GetComponentInChildren<ParticleSystem>();
        _vfx.transform.position = hit.point;
        _vfx.Play(true);
    }

}

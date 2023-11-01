using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [Header("Audio Source On Player")]
    [SerializeField] private AudioSource _footstepsAudioSource;

    [Header("Footstep Sounds")]
    [SerializeField] private AudioClip _footstep1;
    [SerializeField] private AudioClip _footstep2;
    [SerializeField] private AudioClip _footstep3;
    //[SerializeField] private AudioClip _footstep4;
    //[SerializeField] private AudioClip _footstep5;

    [Header("Player Reference")]
    [SerializeField] private PlayerBehaviour _player;

    [Header("Settings")]
    [SerializeField] private float _delayBetweenSteps;
    private List<AudioClip> _footsteps;

    void Start()
    {
        _footsteps = new List<AudioClip>();
        _footsteps.Add(_footstep1);
        _footsteps.Add(_footstep2);
        _footsteps.Add(_footstep3);
        //_footsteps.Add(_footstep4);
        //_footsteps.Add(_footstep5);
        InvokeRepeating(nameof(PlayFootsteps), 1, _delayBetweenSteps);
    }

    // Update is called once per frame
    public void PlayFootsteps()
    {
        if (_player._isMoving)
        {
            var soundToBePlayed = _footsteps[UnityEngine.Random.Range(0, _footsteps.Count)];
            //Debug.Log(soundToBePlayed);
            _footstepsAudioSource.PlayOneShot(soundToBePlayed);
        }
    }
}
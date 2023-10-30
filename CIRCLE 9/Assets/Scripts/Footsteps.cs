using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [SerializeField] private AudioSource _footstepsAudioSource;
    [SerializeField] private List<AudioClip> _footsteps;
    [SerializeField] private AudioClip _footstep1;
    [SerializeField] private AudioClip _footstep2;
    [SerializeField] private AudioClip _footstep3;
    [SerializeField] private AudioClip _footstep4;
    [SerializeField] private AudioClip _footstep5;
    [SerializeField] private PlayerBehaviour _player;
    [SerializeField] private float _delayBetweenSteps;

    void Start()
    {
        _footsteps.Add(_footstep1);
        _footsteps.Add(_footstep2);
        _footsteps.Add(_footstep3);
        _footsteps.Add(_footstep4);
        _footsteps.Add(_footstep5);
        InvokeRepeating(nameof(PlayFootsteps), 1, _delayBetweenSteps);
    }

    // Update is called once per frame
    public void PlayFootsteps()
    {
        if (_player._isMoving)
        {
            var soundToBePlayed = _footsteps[Random.Range(0, _footsteps.Count)];
            //Debug.Log(soundToBePlayed);
            _footstepsAudioSource.PlayOneShot(soundToBePlayed);
        }
    }
}
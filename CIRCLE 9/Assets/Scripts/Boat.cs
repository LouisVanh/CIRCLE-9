using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CameraFading;
using System;

public class Boat : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform _boat;
    [SerializeField] private Transform _introPlayerCam;
    [SerializeField] private GameObject _UI;

    private GameObject _enemyWaveSystem;
    private GameObject _playerControls;
    [SerializeField] private float _boatBobSpeed;
    [SerializeField] private float _boatSpeed;

    [SerializeField] private AudioClip _voiceLines;
    [SerializeField] private AudioSource _acheronAudioSource;
    [SerializeField] private Audio _gameAudio;

    private float _timer;
    [SerializeField] private float _voiceLineLength;
    private bool _canOnlySkipOnce = false;
    private float cameraHeight;

    void Start()
    {
        _enemyWaveSystem = GameObject.Find("EnemyWaveSystem");
        _playerControls = GameObject.Find("PLAYER");
        _UI = GameObject.Find("HealthCanvas");


        _enemyWaveSystem.SetActive(false);
        if(_playerControls)_playerControls.SetActive(false);
        _UI.SetActive(false);
        //_acheronAudioSource.PlayOneShot(_voiceLines);
        cameraHeight = _introPlayerCam.transform.position.y;

        _gameAudio = GameObject.Find("Music").GetComponent<Audio>();
        _acheronAudioSource.volume = _gameAudio._sfxVolume;
        _acheronAudioSource.clip = _voiceLines;
        _acheronAudioSource.Play();

        CameraFade.In(5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _canOnlySkipOnce == false) SkipCutscene();
        _boat.position += new Vector3(_boatSpeed * Time.deltaTime, 0, 0);
        _boat.position = new Vector3(_boat.position.x, Mathf.Sin(Time.timeSinceLevelLoad) * _boatBobSpeed , _boat.position.z);
        _introPlayerCam.position += new Vector3(_boatSpeed * Time.deltaTime, 0, 0);
        _introPlayerCam.position = new Vector3(_introPlayerCam.position.x, 2 + Mathf.Sin(Time.timeSinceLevelLoad + 0.1f ) * _boatBobSpeed , _introPlayerCam.position.z);
        _timer += Time.deltaTime;
        if(_timer > _voiceLineLength)
        {
            _timer = 0;
            SwitchCameras();
        }
    }

    public void SkipCutscene()
    {
        _canOnlySkipOnce = true;
        _gameAudio._gameHasBegun= true;
        _introPlayerCam.gameObject.SetActive(false);
        // code here to activate player
        _playerControls.SetActive(true);
        _enemyWaveSystem.SetActive(true);
        _UI.SetActive(true);
        _acheronAudioSource.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void SwitchCameras()
    {
        CameraFade.Out(() => // short notation for a callback after 4f seconds
        {
            _introPlayerCam.gameObject.SetActive(false);
            _gameAudio._gameHasBegun = true;
            // code here to activate player
            _playerControls.SetActive(true);
            _enemyWaveSystem.SetActive(true);
            _UI.SetActive(true);
            _acheronAudioSource.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }, 4f);
    }
}

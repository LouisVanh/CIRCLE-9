using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CameraFading;

public class Boat : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform _boat;
    [SerializeField] private Transform _introPlayerCam;
    private GameObject _enemyWaveSystem;
    private GameObject _playerControls;
    [SerializeField] private float _boatBobSpeed;
    [SerializeField] private float _boatSpeed;

    [SerializeField] private AudioClip _voiceLines;
    [SerializeField] private AudioSource _acheronAudioSource;

    private float _timer;
    [SerializeField] private float _voiceLineLength;


    void Start()
    {
        _enemyWaveSystem = GameObject.Find("EnemyWaveSystem");
        _playerControls = GameObject.Find("PLAYER");

        _enemyWaveSystem.SetActive(false);
        _playerControls.SetActive(false);
        _acheronAudioSource.PlayOneShot(_voiceLines);
    }

    // Update is called once per frame
    void Update()
    {
        _boat.position += new Vector3(_boatSpeed * Time.deltaTime, 0, 0);
        _boat.position = new Vector3(_boat.position.x, Mathf.Sin(Time.timeSinceLevelLoad) * _boatBobSpeed , _boat.position.z);
        _timer += Time.deltaTime;
        if(_timer > _voiceLineLength)
        {
            _timer = 0;
            SwitchCameras();
        }
    }

    private void SwitchCameras()
    {
        CameraFade.Out(() => // short notation for a callback after 2 seconds
        {
            _introPlayerCam.gameObject.SetActive(false);
            // code here to activate player
            _playerControls.SetActive(true);
            _enemyWaveSystem.SetActive(true);
        }, 4f);
    }
}
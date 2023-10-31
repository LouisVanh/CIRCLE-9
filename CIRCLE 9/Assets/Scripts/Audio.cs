using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Audio : MonoBehaviour
{
    public AudioSource audioSource;
    [SerializeField] private AudioClip _clipGame;
    [SerializeField] private AudioClip _clipMenu;
    [SerializeField] private Slider _volumeSlider;
    public static Audio instance;
    private bool _startedPlaying = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


    }
    void Start()
    {
        
        if (!PlayerPrefs.HasKey("volume"))
        {
            PlayerPrefs.SetFloat("volume", 0.9f);
        }
        Load();

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            audioSource.clip = _clipMenu;
        }
        else
        {
            audioSource.clip = _clipGame;
        }
        audioSource.Play();

        
    }
    private void Update()
    {
        Debug.Log(_volumeSlider.value);
        //audioSource.volume = _volumeSlider.value;
        
    }
    public void ChangeVolume()
    {
        audioSource.volume = _volumeSlider.value;
        Save();
    }
    public void Load()
    {
        if (_volumeSlider)
            _volumeSlider.value = PlayerPrefs.GetFloat("volume");
    }
    public void Save()
    {
        PlayerPrefs.SetFloat("volume", _volumeSlider.value);
    }
    public void StartPlayingGameMusic()
    {
        audioSource.clip = _clipGame;
        audioSource.Play();
    }
}

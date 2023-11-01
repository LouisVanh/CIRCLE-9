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
    [SerializeField] private Slider _sfxSlider;
    public float _sfxVolume = 0.025f;
    public static Audio instance;

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

        if (!PlayerPrefs.HasKey("sfxVolume"))
        {
            PlayerPrefs.SetFloat("sfxVolume", 0.9f);
        }
        LoadSFX();

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
    public void ChangeVolume()
    {
        audioSource.volume = _volumeSlider.value;
        Save();
    }
    public void ChangeVolumeSFX()
    {
        _sfxVolume= _sfxSlider.value;
        SaveSFX();
    }
    public void Load()
    {
        if (_volumeSlider)
            _volumeSlider.value = PlayerPrefs.GetFloat("volume");

    }
    public void LoadSFX()
    {
        if (_sfxSlider)
        _sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
            
    }
    public void Save()
    {
        PlayerPrefs.SetFloat("volume", _volumeSlider.value);

    }
    public void SaveSFX()
    {
        PlayerPrefs.SetFloat("sfxVolume", _sfxSlider.value);
    }
    public void StartPlayingGameMusic()
    {
        audioSource.clip = _clipGame;
        audioSource.Play();
    }
}

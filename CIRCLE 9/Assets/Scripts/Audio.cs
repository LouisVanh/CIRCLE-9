using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Audio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource audioSourceAmbiance;
    public AudioSource audioSourceAmbiance2;
    [SerializeField] private AudioClip _clipGame;
    [SerializeField] private AudioClip _clipMenu;
    [SerializeField] private AudioClip _clipGameAmbiance1;
    [SerializeField] private AudioClip _clipGameAmbiance2;
    [SerializeField] private AudioClip _clipStartCutscene;
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private Slider _sfxSlider;
    public float _sfxVolume = 0.025f;
    public static Audio Instance;
    public int _playAudioCounter = 0;
    public int _playAudioSceneCounter = 0;
    private bool _hasPressedPlay = false;   
    public bool _gameHasBegun = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        //_volumeSlider = GameObject.Find("MusicS").GetComponent<Slider>();
        //_sfxSlider = GameObject.Find("SFX").GetComponent<Slider>();

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
            audioSourceAmbiance.Stop();
        }
        else
        {

            audioSource.clip = _clipGame;
            audioSourceAmbiance.clip = _clipGameAmbiance1;
            audioSourceAmbiance2.clip = _clipGameAmbiance2;
            audioSourceAmbiance.Play();
        }
        audioSource.Play();

        
    }
    private void Update()
    {
        //Debug.Log($" HAS GAME BEGUN????{_gameHasBegun}");

        if (SceneManager.GetActiveScene().buildIndex !=0 && _playAudioSceneCounter ==0)
        {
            audioSource.clip = _clipStartCutscene;
            audioSource.Play();
            audioSourceAmbiance.Stop();
            audioSourceAmbiance2.Stop();
            _playAudioSceneCounter = 1;
           
        }
        if (_playAudioCounter == 0 && _gameHasBegun)
        {           
            StartPlayingGameMusic();
            _playAudioCounter = 1;
        }
    }
    public void ChangeVolume()
    {
        audioSource.volume = _volumeSlider.value;
        Save();
    }
    public void ChangeVolumeSFX()
    {
        _sfxVolume= _sfxSlider.value;
        audioSourceAmbiance.volume = _sfxSlider.value;
        audioSourceAmbiance2.volume = _sfxSlider.value;
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
        audioSourceAmbiance.clip = _clipGameAmbiance1;
        audioSourceAmbiance.Play();
        audioSourceAmbiance2.clip = _clipGameAmbiance2;
        audioSourceAmbiance2.Play();
    }
    public void HasPressedPlay()
    {
        _hasPressedPlay= true;
    }
}

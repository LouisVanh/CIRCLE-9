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
    [SerializeField] private AudioClip _clipSatanScene;
    [SerializeField] private AudioClip _clipWonGame;
    [SerializeField] private AudioClip _clipCutsceneAmbiance;
    [SerializeField] private AudioClip _clipDeathAmbiance;
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private Slider _sfxSlider;
    public float _sfxVolume = 0.025f;
    public static Audio Instance;
    public int _playAudioCounter = 0;
    public int _playAudioSceneCounter = 0; 
    public bool _gameHasBegun = false;
    public bool WonGame = false;
    public bool PlayingWonTheme = false;

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

        if (SceneManager.GetActiveScene().buildIndex !=0 && _playAudioSceneCounter ==0)
        {
            audioSource.clip = _clipStartCutscene;
            audioSource.Play();
            audioSourceAmbiance.Stop();
            audioSourceAmbiance.clip = _clipCutsceneAmbiance;
            audioSourceAmbiance.Play();
            audioSourceAmbiance2.Stop();
            _playAudioSceneCounter = 1;
           
        }
        if (SceneManager.GetActiveScene().buildIndex == 2 && _playAudioSceneCounter == 1 && _gameHasBegun)
        {
            audioSource.Stop();
            audioSource.clip = _clipSatanScene;
            audioSource.Play();
            _playAudioSceneCounter = 2;
            Debug.Log("PLAY SATAN THEME");

        }
        if (_playAudioCounter == 0 && _gameHasBegun)
        {           
            StartPlayingGameMusic();
            _playAudioCounter = 1;
        }

        if(SceneManager.GetActiveScene().buildIndex == 2 && _gameHasBegun && WonGame && !PlayingWonTheme)
        {
            audioSource.Stop();
            audioSource.clip = _clipWonGame;
            audioSource.Play();           
            Debug.Log("PLAY WON THEME");
            PlayingWonTheme= true;
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
    public void PlayerDied()
    {
        if(_playAudioCounter ==1)
        {
            audioSource.Stop();
            audioSourceAmbiance2.Stop();
            audioSourceAmbiance.Stop();
            audioSourceAmbiance.clip = _clipDeathAmbiance;
            audioSourceAmbiance.Play();
            _playAudioCounter = 2;
        }
        
    }
}

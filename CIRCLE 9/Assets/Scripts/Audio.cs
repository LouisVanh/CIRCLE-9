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
    // Start is called before the first frame update
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

    // Update is called once per frame
    public void ChangeVolume()
    {
        AudioListener.volume = _volumeSlider.value;
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
}

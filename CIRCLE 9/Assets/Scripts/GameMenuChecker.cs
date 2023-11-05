using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuChecker : MonoBehaviour
{
    [SerializeField] private PlayerBehaviour _player;
    [SerializeField] private Satan _satan;
    [SerializeField] private GameObject _deathMenu;
    [SerializeField] private GameObject _cutsceneMenu;
    [SerializeField] private GameObject _winMenu;
    [SerializeField] private Audio _gameAudio;
    //[SerializeField] private Boat _boat;
    // Start is called before the first frame update
    void Start()
    {
        if(_winMenu!= null)
        {
            _winMenu.SetActive(false);
        }
        _deathMenu.SetActive(false);
        if (_cutsceneMenu != null)
        {
            _cutsceneMenu.SetActive(true);
        }
        Time.timeScale= 1.0f;
        _gameAudio = GameObject.Find("Music").GetComponent<Audio>();
        //_player = GameObject.Find("PLAYER").GetComponent<PlayerBehaviour>() ;

    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerDeath();
        if (_satan != null)
        {
            CheckSatanDeath();
        }
        if (_cutsceneMenu != null)
        {
            if (_player.isActiveAndEnabled)
            {
                _cutsceneMenu.SetActive(false);
            }
            else
            {
                _cutsceneMenu.SetActive(true);
            }
        }
        
    }

    private void CheckSatanDeath()
    {
        if(_satan!= null)
        {
            if (_satan.HasDied)
            {
                Win();
                
            }
        }
    }

    private void Win()
    {
        _winMenu.SetActive(true);
        _gameAudio.WonGame = true;
        _player.WonGame = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void CheckPlayerDeath()
    {
        if(_player.HasDied)
        {
            GameOver();
            _gameAudio.PlayerDied();
        }
    }
    private void GameOver()
    {
        _deathMenu.SetActive(true);
        
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
        _player.HasDied= false;
        _gameAudio._gameHasBegun= false;
        _gameAudio._playAudioSceneCounter= 0;
        _gameAudio._playAudioCounter= 0;
        _gameAudio.PlayingWonTheme= false;
        _gameAudio.WonGame = false;
    }
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        _player.HasDied= false;
        Destroy(GameObject.Find("Music"));
        SceneManager.LoadScene(0);
        _gameAudio.PlayingWonTheme= false;
        _gameAudio.WonGame = false;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}

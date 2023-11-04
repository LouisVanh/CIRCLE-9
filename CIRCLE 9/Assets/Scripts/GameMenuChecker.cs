using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuChecker : MonoBehaviour
{
    [SerializeField] private PlayerBehaviour _player;
    [SerializeField] private GameObject _deathMenu;
    [SerializeField] private Audio _gameAudio;
    //[SerializeField] private Boat _boat;
    // Start is called before the first frame update
    void Start()
    {
        _deathMenu.SetActive(false);
        Time.timeScale= 1.0f;
        _gameAudio = GameObject.Find("Music").GetComponent<Audio>();
        //_player = GameObject.Find("PLAYER").GetComponent<PlayerBehaviour>() ;

    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerDeath();
    }
    private void CheckPlayerDeath()
    {
        if(_player.HasDied)
        {
            GameOver();
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
        _player.HasDied= false;
        _gameAudio._gameHasBegun= false;
        _gameAudio._playAudioSceneCounter= 0;
        _gameAudio._playAudioCounter= 0;
    }
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        _player.HasDied= false;
        Destroy(GameObject.Find("Music"));
        SceneManager.LoadScene(0);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}

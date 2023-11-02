using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuChecker : MonoBehaviour
{
    public PlayerBehaviour _player;
    [SerializeField] private GameObject _deathMenu;
    // Start is called before the first frame update
    void Start()
    {
        _deathMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerDeath();
    }
    private void CheckPlayerDeath()
    {
        if(_player._hasDied)
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
        _player._hasDied= false;
    }
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}

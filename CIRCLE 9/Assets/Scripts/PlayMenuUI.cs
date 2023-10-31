using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayMenuUI : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
        if(GameSettings.Instance)
        Destroy(GameSettings.Instance.gameObject);
    }
    public void Options()
    {
        // @Szilard, you're welcome
        //allOtherCanvas(es).gameObject.SetActive(false);
        //OptionsCanvas.gameObject.SetActive(true);
    }
}

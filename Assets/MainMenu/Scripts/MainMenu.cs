using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public CanvasGroup OptionPanel;
    public void PlayGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Option(){
        Time.timeScale = 0f;
        OptionPanel.alpha = 1;
        OptionPanel.blocksRaycasts = true;
    }

    public void Back(){
        Time.timeScale = 1f;
        OptionPanel.alpha = 0;
        OptionPanel.blocksRaycasts = false;
    }
    public void QuitGame(){
        Application.Quit();
    }
}

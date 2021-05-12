using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject optionMenu;
    //in game manager
    bool isGamePaused;


    private void Start()
    {
        isGamePaused = false;
        pauseMenu.SetActive(false);
        optionMenu.SetActive(false);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene("Nono's Scene");
        print("scene reloaded");
    }

    public void SetPause()
    {
        if (!isGamePaused)
        {
            isGamePaused = true;
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
        else
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
            isGamePaused = false;
        }
    }
    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        isGamePaused = false;
    }
    public void BackToMenu()
    {
        //SceneManager.LoadScene("MainMenu");
        print("Going Back To MainMenu . . .");
    }
    public void QuitApplication()
    {
        //Application.Quit();
        print("Quitting Game . . .");
    }
    public void OptionMenu()
    {
        pauseMenu.SetActive(false);
        optionMenu.SetActive(true);

        print("Opening Option Menu . . . ");
    }

    public void ExitOptions()
    {
        
        pauseMenu.SetActive(true);
        optionMenu.SetActive(false);
    }

}

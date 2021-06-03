using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public static ButtonManager instance { get; private set; }
    public GameObject pauseMenu;
    public GameObject optionMenu;
    public GameObject pause_button;
    public GameObject LoseMenu;
    public GameObject WinMenu;

    //in game manager
    public bool isGamePaused;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        isGamePaused = false;
        if(pauseMenu != null)
            pauseMenu.SetActive(false);
        if (optionMenu != null)
            optionMenu.SetActive(false);
        if (LoseMenu != null)
            LoseMenu.SetActive(false);
        if (WinMenu != null)
            WinMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene("Nono's Scene");
        print("scene reloaded");
    }

    public void SetPause()
    {
        AudioManager.instance.Play("click_sci_fi");
        if (!isGamePaused)
        {
            isGamePaused = true;
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            pause_button.SetActive(false);
        }
        else
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
            isGamePaused = false;
            pause_button.SetActive(true);
        }
    }
    public void BackToMenu()
    {   
        AudioManager.instance.Play("click_sci_fi");
        SceneManager.LoadScene("LevelMenu");
        print("Going Back To MainMenu . . ." + AudioManager.instance.musicVolume + " / " + AudioManager.instance.effectVolume);
    }
    public void QuitApplication()
    {
        AudioManager.instance.Play("click_sci_fi");
        Application.Quit();
        print("Quitting Game . . .");

    }
    public void OptionMenu()
    {
        pauseMenu.SetActive(false);
        optionMenu.SetActive(true);
        AudioManager.instance.Play("click_sci_fi");
        print("Opening Option Menu . . . ");
    }

    public void ExitOptions()
    {
        
        pauseMenu.SetActive(true);
        optionMenu.SetActive(false);
        AudioManager.instance.Play("click_sci_fi");
    }

    public void NextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex + 1 != 24)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            AudioManager.instance.ApplyChanges();
        }
            
        else
            BackToMainMenu();
        AudioManager.instance.Play("click_sci_fi");
    }

    public void RestartLevel()
    {
        print("reloading this level");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        AudioManager.instance.Play("click_sci_fi");
    }

    public void ShowWin()
    {
        optionMenu.SetActive(false);
        pauseMenu.SetActive(false);
        LoseMenu.SetActive(false);
        pause_button.SetActive(false);
        WinMenu.SetActive(true);

    }

    public void ShowLose()
    {
        optionMenu.SetActive(false);
        pauseMenu.SetActive(false);
        LoseMenu.SetActive(true);
        pause_button.SetActive(false);
        WinMenu.SetActive(false);
    }

    public void BackToMainMenu()
    {
        BackToMainMenu();
    }

    public void LoginGooglePlay()
    {
        PlayGames.instance.SignInOrOut();
        AudioManager.instance.Play("click_sci_fi");
    }

    public void ShowAchievements()
    {
        PlayGames.instance.ShowAchievements();
        AudioManager.instance.Play("click_sci_fi");
    }


}

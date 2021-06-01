using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Button[] buttons;
    public GameObject OptionMenu;
    private int DEBUG_nbOfButtonAccessible;
    void Start()
    {
        updateLevelAvailable();
        AudioManager.instance.Play("music_menu");
    }

    public void StartLevel(string name)
    {
        print("starting level called : " + name);
        SceneManager.LoadScene(name);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayerPrefs.SetInt("levelReached", 1);
            updateLevelAvailable();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlayerPrefs.SetInt("levelReached", 100);
            print("onkefnokjegrfanj");
            updateLevelAvailable();
        }
    }

    private void updateLevelAvailable()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int levelReached = PlayerPrefs.GetInt("levelReached", 1);

            if (levelReached <= 0)
                levelReached = 1;

            if (i + 1 > levelReached && buttons[i] != null)
                buttons[i].interactable = false;
            else
                buttons[i].interactable = true;

        }
        DEBUG_nbOfButtonAccessible = 0;
    }

    public void ExitOptions()
    {
        OptionMenu.SetActive(false);
    }
    public void OpenOptions()
    {
        OptionMenu.SetActive(true);
    }
}

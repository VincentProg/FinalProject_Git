using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Button[] buttons;
    private int DEBUG_nbOfButtonAccessible;
    void Start()
    {
        updateLevelAvailable();
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
            print("player pref reset");
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
                DEBUG_nbOfButtonAccessible++;

        }
        print("nb Of button: " + DEBUG_nbOfButtonAccessible);
        DEBUG_nbOfButtonAccessible = 0;
    }
}

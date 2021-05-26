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
        for (int i = 0; i < buttons.Length; i++)
        {
            int levelReached = PlayerPrefs.GetInt("levelReached", 1);

            if(i+1 > levelReached && buttons[i] != null)
                buttons[i].interactable = false;
            else
                DEBUG_nbOfButtonAccessible++;

        }
        print("nb Of button: " + DEBUG_nbOfButtonAccessible);
    }

    public void StartLevel(string name)
    {
        print("starting level called : " + name);
        SceneManager.LoadScene(name);
    }

    
}

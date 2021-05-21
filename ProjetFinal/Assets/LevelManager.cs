using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Button[] buttons;
    void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int levelReached = PlayerPrefs.GetInt("levelReached", 1);

            if(i+1 > levelReached)
                buttons[i].interactable = false;
        }
    }

    public void StartLevel(string name)
    {
        SceneManager.LoadScene(name);
    }
}

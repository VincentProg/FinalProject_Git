using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Button[] buttons;
    public GameObject OptionMenu;
    public GameObject welcomeMenu;
    public Animator animator;
    void Start()
    {
        updateLevelAvailable();
        AudioManager.instance.StopPlayAll();
        AudioManager.instance.Play("music_menu");
    }

    public void StartLevel(string name)
    {
        AudioManager.instance.Play("click_sci_fi");
        print("starting level called : " + name);
        SceneManager.LoadScene(name);
        Time.timeScale = 1;
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
    }

    public void ExitOptions()
    {
        OptionMenu.SetActive(false);
        AudioManager.instance.Play("click_sci_fi");
    }
    public void OpenOptions()
    {
        OptionMenu.SetActive(true);
        AudioManager.instance.Play("click_sci_fi");
    }

    public void Welcome()
    {
        //welcomeMenu.GetComponent<RectTransform>().anchorMin = new Vector2(1, 2);
        //animation vers le haut
        animator.SetBool("move", true);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int levelNumber;
    public bool isLastLevel = false;

    public GameObject endButtonUICowboy;
    public GameObject endButtonUISoldier;
    public Sprite endButtonImage;
    public Sprite endButtonImageLit;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ButtonManager.instance.SetPause();
        }
    }

    public void HighlightEnd()
    {
        endButtonUICowboy.GetComponent<Image>().sprite = endButtonImageLit;
        endButtonUISoldier.GetComponent<Image>().sprite = endButtonImageLit;
    }
    public void UnlitEnd()
    {
        endButtonUICowboy.GetComponent<Image>().sprite = endButtonImage;
        endButtonUISoldier.GetComponent<Image>().sprite = endButtonImage;
    }

}

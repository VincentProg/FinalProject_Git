using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueRobot : MonoBehaviour
{
    public static DialogueRobot instance;

    [HideInInspector]
    public GameObject popUpBox;
    private Animator anim;
    private Text Text;
    public bool isActive;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        popUpBox = transform.GetChild(0).gameObject;
        anim = popUpBox.GetComponent<Animator>();
        Text = popUpBox.transform.GetChild(1).GetComponent<Text>();

    }

    public void RobotSpeak(string text)
    {

        if (!isActive)
        {
            isActive = true;
            Text.text = "";
            popUpBox.SetActive(true);
            StartCoroutine(TypeSentence(text));
        }
    }


    IEnumerator TypeSentence(string sentence)
    {
        foreach (char letter in sentence.ToCharArray())
        {
            Text.text += letter;
            yield return null;
        }
    }

    public IEnumerator iDisappear()
    {
        anim.SetTrigger("Disappear");
        yield return new WaitForSeconds(0.2f);
        popUpBox.SetActive(false);
        isActive = false;
    }
}

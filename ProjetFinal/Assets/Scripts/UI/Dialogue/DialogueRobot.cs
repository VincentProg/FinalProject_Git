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

    public bool textEnded;

    public List<string> startSentences = new List<string>();

    int indexStart = 0;

    bool isFightStarted = false;

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
        print(startSentences.Count);
        if(startSentences.Count == 0)
        {
            isFightStarted = true;
            CombatSystem.instance.StartFight();
            print("salutation");
        }
    }

    private void Update()
    {
        if (!isFightStarted)
        {

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {

                if (isActive)
                {
                    StartCoroutine(iDisappear());
                    if(indexStart >= startSentences.Count)
                    {
                        isFightStarted = true;
                        CombatSystem.instance.StartFight();
                        print("wtf");
                    }
                    return;
                }
            }

            if (!isActive)
            {
                if (startSentences.Count > indexStart)
                {
                    RobotSpeak(startSentences[indexStart]);
                    indexStart++;
                }
            }
        }
    }

    public void RobotSpeak(string text)
    {

        if (!isActive)
        {
            isActive = true;
            textEnded = false;
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

        textEnded = true;
    }

    public IEnumerator iDisappear()
    {
        anim.SetTrigger("Disappear");
        yield return new WaitForSeconds(0.2f);
        popUpBox.SetActive(false);
        isActive = false;
    }

    private IEnumerator StartSpeak()
    {
        yield return new WaitForSeconds(1.5f);
        RobotSpeak(startSentences[0]);
    }
}

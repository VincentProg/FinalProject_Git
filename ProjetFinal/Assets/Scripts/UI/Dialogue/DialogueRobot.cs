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

    [SerializeField]
    Animator robotAnim;
    bool hasToDisappear = false;
    bool isRobotHere = false;

    public List<string> startSentences = new List<string>();

    int indexStart = 0;

    bool isFightStarted = false;

    public bool isTuto;
    IEnumerator routineRunning = null;

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
        if(startSentences.Count == 0)
        {
            isFightStarted = true;
            CombatSystem.instance.StartFight();
        }
    }

    private void Update()
    {
        if (!isTuto && !isFightStarted)
        {

            if ( Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && textEnded)
            {

                if (isActive)
                {
                    routineRunning = iDisappear();

                    StartCoroutine(routineRunning);

                    if (indexStart >= startSentences.Count)
                    {
                        isFightStarted = true;
                        CombatSystem.instance.StartFight();
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
            hasToDisappear = false;

            isActive = true;
            textEnded = false;
            Text.text = "";
            popUpBox.SetActive(true);
            StartCoroutine(TypeSentence(text));
            if (!isRobotHere)
            {
                robotAnim.SetTrigger("Appear");
                isRobotHere = true;
            }
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
        hasToDisappear = true;
        anim.SetTrigger("Disappear");
        yield return new WaitForSeconds(0.2f);
        popUpBox.SetActive(false);
        isActive = false;
        yield return new WaitForSeconds(2);
        if (hasToDisappear)
        {
            robotAnim.SetTrigger("Disappear");
            isRobotHere = false;
        }
    }
}

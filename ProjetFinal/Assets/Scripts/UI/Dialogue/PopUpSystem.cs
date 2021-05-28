using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpSystem : MonoBehaviour
{
    public static PopUpSystem instance;

    [HideInInspector]
    public GameObject popUpBox;
    private Animator anim;
    private Text popUpText;
    private bool isActive;

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
        popUpText = popUpBox.transform.GetChild(1).GetChild(0).GetComponent<Text>();

    }

    public void PopUp(string text, HeroController hero)
    {

        if (!isActive)
        {
            Cut();
            isActive = true;
            popUpText.text = "";
            popUpBox.SetActive(true);
            transform.position = hero.transform.position + new Vector3(0, 20);
            StartCoroutine(TypeSentence(text));
        }
    }


    IEnumerator TypeSentence(string sentence)
    {
        foreach(char letter in sentence.ToCharArray())
        {
            popUpText.text += letter;
            yield return null;
        }

        yield return new WaitForSeconds(1.5f + 0.05f* sentence.Length);
        StartCoroutine(iDisappear());
    }

    public void Cut()
    {
        isActive = false;
        StopAllCoroutines();
        popUpBox.SetActive(false);
    }

    public IEnumerator iDisappear()
    {
        anim.SetTrigger("Disappear");
        yield return new WaitForSeconds(0.2f);
        popUpBox.SetActive(false);
        isActive = false;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timeline : MonoBehaviour
{

    public static Timeline _instance { get; private set;}

    [SerializeField] private float enlargmentValue;

    public GameObject h1, h2;

    private Slider sh1, sh2;
    private Vector3 defSize;

    private void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        sh1 = h1.GetComponentInChildren<Slider>();
        sh2 = h2.GetComponentInChildren<Slider>();

        defSize = h1.GetComponent<RectTransform>().localScale;
        if (enlargmentValue == 0)
            enlargmentValue = 1.1f;

    }

    // Update is called once per frame
    void Update()
    {
        sh1.value = (float)CombatSystem.instance.heros[0].health / CombatSystem.instance.heros[0].stats.health;
        sh2.value = (float)CombatSystem.instance.heros[1].health / CombatSystem.instance.heros[1].stats.health;
    }

    public void Enlarge(GameObject HeroBoxUI)
    {
        HeroBoxUI.GetComponent<RectTransform>().localScale *= enlargmentValue;
        HeroBoxUI.GetComponentInChildren<Image>().color = Color.blue;
    }

    public void ResetHeroBoxToDefaultSize(GameObject HeroBoxUI)
    {
        HeroBoxUI.GetComponent<RectTransform>().localScale = defSize;
        HeroBoxUI.GetComponentInChildren<Image>().color = Color.white;
    }
}

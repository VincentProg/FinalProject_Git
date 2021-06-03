using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class updateHealth : MonoBehaviour
{
    public HeroController hero;
    private Image img;
    void Start()
    {
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        img.fillAmount = (float)hero.health / hero.stats.health;
        print(hero.name);
        if(hero.name == "Hero1")
        {
            if (img.fillAmount >= 0.68f)
                img.color = Color.green;
            else if (img.fillAmount < 0.35)
                img.color = Color.red;
            else
                img.color = Color.yellow;
        }
        else
        {
            if (img.fillAmount > 0.5f)
                img.color = Color.green;
            else
                img.color = Color.red;
        }
    }
}

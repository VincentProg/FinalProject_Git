using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Attack : MonoBehaviour
{
    public int index;
    [HideInInspector]
    public Attack attack;

    public HeroController hero;



    private bool isDisabled = false;
    private int cooldown;
    GameObject cooldownGO;
    private Text textCD;

    private bool isNbrPerTurn;
    private int nbrUsePerTurn;
    private Text Txt_NbrUse;

    private bool isNbrTotal;
    private int nbrUseTotal;

    Image image;

    [HideInInspector]
    public bool isClicked;



    public void UpdateUI()
    {
        image = GetComponent<Image>();
        image.sprite = attack.sprite;
        cooldownGO = transform.GetChild(0).gameObject;
        textCD = cooldownGO.transform.GetChild(0).GetComponent<Text>();
        cooldownGO.SetActive(false);

        nbrUsePerTurn = attack.nbrUsePerTurn;
        nbrUseTotal = attack.nbrUseTotal;
        if (nbrUsePerTurn > 0) isNbrPerTurn = true;
        if (nbrUseTotal > 0)
        {
            isNbrTotal = true;
            Txt_NbrUse = transform.GetChild(1).GetComponent<Text>();
            Txt_NbrUse.text = nbrUseTotal.ToString();

        }
        else transform.GetChild(1).gameObject.SetActive(false);


    }

    public void OnClick()
    {
       
        if(hero.PA == 0)
        {
            PopUpSystem.instance.PopUp("No more energy for this turn", hero);
        }
        if (!isClicked)
        {
            if (!isDisabled)
            {
                if (isNbrTotal)
                {
                    if (nbrUseTotal > 0)
                    {
                        if (isNbrPerTurn)
                        {

                            if (nbrUsePerTurn > 0)
                            {
                                TryShowAttack();
                            }
                            else
                            {
                                PopUpSystem.instance.PopUp("No more use possible for this turn", hero);
                            }
                        }
                        else
                        {
                            TryShowAttack();
                        }
                    }
                    else
                    {
                        PopUpSystem.instance.PopUp("No more utilisations left", hero);
                    }
                }
                else
                {
                    if (isNbrPerTurn)
                    {
                        if (nbrUsePerTurn > 0)
                        {
                            TryShowAttack();
                        }
                        else
                        {
                            PopUpSystem.instance.PopUp("No more use possible for this turn", hero);
                        }
                    }
                    else
                    {
                        TryShowAttack();
                    }
                }
            }
            else
            {
                PopUpSystem.instance.PopUp("I can't realize this attack yet", hero);
            }
        } else
        {
            isClicked = false;
            image.color = Color.white;
            hero.ShowMovements();
        }
    }

    private void TryShowAttack()
    {

        if (hero.PA >= attack.costPA)
        {    
           Hero_AttacksManager.instance.ShowAttackRange(this, attack);
           foreach(Transform button in hero.myCanvas.transform.GetChild(0)){
                if (button.GetComponent<UI_Attack>())
                {
                    button.GetComponent<Image>().color = Color.white;
                    button.GetComponent<UI_Attack>().isClicked = false;
                }
           }
           image.color = Color.grey;
           isClicked = true;
        }
        else PopUpSystem.instance.PopUp("Not enough energy", hero);
    }

    public void ActivateAttack()
    {
        cooldown = attack.cooldown;
        hero.PA -= attack.costPA;
        image.color = Color.white;
        if (cooldown > 0)
        {
            isDisabled = true;
            cooldownGO.SetActive(true);
            textCD.text = cooldown.ToString();
        }

        if (isNbrTotal)
        {
            nbrUseTotal--;
            Txt_NbrUse.text = nbrUseTotal.ToString();
        }

        if (isNbrPerTurn)
        {
            nbrUsePerTurn--;
        }

    }

    public void StartTurn()
    {
        if(cooldown > 0)
        {
            cooldown--;
            if(cooldown == 0)
            {
                isDisabled = false;
                cooldownGO.SetActive(false);
            }
            else textCD.text = cooldown.ToString();     
        }
        nbrUsePerTurn = attack.nbrUsePerTurn;
    }

}

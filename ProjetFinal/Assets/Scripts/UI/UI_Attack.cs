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

    [HideInInspector]
    public bool canBeClick = true;


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
            int rand = Random.Range(0, 10);
            switch (rand)
            {
                case 0:
                    PopUpSystem.instance.PopUp("Not enought ammo!", hero);
                    break;
                case 1:
                    PopUpSystem.instance.PopUp("I don’t do overtime.", hero);
                    break;
                case 2:
                    PopUpSystem.instance.PopUp("Reloading!", hero);
                    break;
                case 3:
                    PopUpSystem.instance.PopUp("Need to reload!", hero);

                    break;
                case 4:
                    PopUpSystem.instance.PopUp("Sorry, it's my break.", hero);
                    break;

            }
        }
        if (canBeClick)
        {
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
                                    PopUpSystem.instance.PopUp("Can't use this at the moment.", hero);
                                }
                            }
                            else
                            {
                                TryShowAttack();
                            }
                        }
                        else
                        {
                           
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
                    int rand = Random.Range(0, 5);
                    switch (rand)
                    {
                        case 0:
                            PopUpSystem.instance.PopUp("WAIT FOR THE F****** COOLDOWN!", hero);
                            break;
                        default:
                            PopUpSystem.instance.PopUp("Not ready yet.", hero);
                            break;
                    }
                }
            }
            else
            {
                isClicked = false;
                image.color = Color.white;
                hero.ShowMovements();
            }
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

        foreach (Transform button in hero.myCanvas.transform.GetChild(0))
        {
            if (button.GetComponent<UI_Attack>())
            {
                button.GetComponent<Image>().color = Color.grey;
                button.GetComponent<UI_Attack>().canBeClick = false;
            }
        }

    }

    public void StartTurn()
    {
        image.color = Color.white;
        isClicked = false;
        canBeClick = true;
        if (cooldown > 0)
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

    public void UpdateBtnClickable(bool makeThemClickable)
    {
        if (makeThemClickable)
        {
            if (canBeClick)
            {
                image.color = Color.white;
                isClicked = false;

            }
        } else
        {
            image.color = Color.grey;
            canBeClick = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Attack : MonoBehaviour
{
    public int index;
    [HideInInspector]
    public Attack attack;

    public HeroController hero;



    private bool isDisabled = false;
    private int cooldown;
    GameObject cooldownGO;
    private TextMeshProUGUI textCD;

    private bool isNbrPerTurn;
    private int nbrUsePerTurn;
    private TextMeshProUGUI Txt_NbrUse;

    private bool isNbrTotal;
    private int nbrUseTotal;





    public void UpdateUI()
    {
        GetComponent<Image>().sprite = attack.sprite;
        textCD = transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        cooldownGO = transform.GetChild(0).gameObject;
        cooldownGO.SetActive(false);
        transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = attack.costPA.ToString();

        nbrUsePerTurn = attack.nbrUsePerTurn;
        nbrUseTotal = attack.nbrUseTotal;
        if (nbrUsePerTurn > 0) isNbrPerTurn = true;
        if (nbrUseTotal > 0)
        {
            isNbrTotal = true;
            Txt_NbrUse = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            Txt_NbrUse.text = nbrUseTotal.ToString();

        }
        else transform.GetChild(2).gameObject.SetActive(false);
    }

    public void OnClick()
    {
        if(hero.PA == 0)
        {
            PopUpSystem.instance.PopUp("No more energy for this turn", hero);
        }

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
                        } else
                        {
                            PopUpSystem.instance.PopUp("No more use possible for this turn", hero);
                        }
                    } else
                    {
                        TryShowAttack();
                    }
                } else
                {
                    PopUpSystem.instance.PopUp("No more utilisations left", hero);
                }
            } else
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
                } else
                {
                    TryShowAttack();
                }
            } 
        }
        else
        {
            PopUpSystem.instance.PopUp("I can't realize this attack yet", hero);
        }
    }

    private void TryShowAttack()
    {

        if (hero.PA >= attack.costPA)
        {    
                Hero_AttacksManager.instance.ShowAttackRange(this, attack);
        }
        else PopUpSystem.instance.PopUp("Not enough energy", hero);
    }

    public void ActivateAttack()
    {
        cooldown = attack.cooldown;
        hero.PA -= attack.costPA;
        if(cooldown > 0)
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

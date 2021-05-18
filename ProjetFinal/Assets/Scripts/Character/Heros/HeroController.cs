using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HeroController : MonoBehaviour
{
    // STATISTIQUES
    public StatsHero stats;
    // ----
    [HideInInspector]
    public string nameHero;
    [HideInInspector]
    public int health, PM, PA;

    // ATTACKS
    public List<Attack> attacks = new List<Attack>();
    public GameObject myCanvas;
    TextMeshProUGUI PAtxt, PMtxt, PVtxt;
    

    // VARIABLES GRID
    [HideInInspector]
    public HexCell myTile;


    bool isMyTurn = false;


    // Start is called before the first frame update
    void Start()
    {
        

        PAtxt = myCanvas.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        PMtxt = myCanvas.transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        PVtxt = myCanvas.transform.GetChild(5).GetComponent<TextMeshProUGUI>();

        SetMyStats();
        SetUIAttacks();


        #region GET MY START TILE()
        // AJOUT TUILE DEPART
        RaycastHit2D hitStart = Physics2D.Raycast(transform.position, Vector2.zero, Mathf.Infinity);
        if (hitStart)
        {
            if (hitStart.transform.GetComponent<HexCell>())
            {
                myTile = hitStart.transform.GetComponent<HexCell>();
                myTile.hero = this;
            }
        }
        #endregion

    }

    // Update is called once per frame
    void Update()
    {

        if (isMyTurn)
        {

            if (PA > 0 && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                Vector3 touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

                Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
                RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);
                if (hitInformation)
                {
                    if (hitInformation.transform.GetComponent<HexCell>() != null)
                    {
                        HexCell tileTouched = hitInformation.transform.GetComponent<HexCell>();


                        switch (tileTouched.selectionType)
                        {
                            case HexCell.SELECTION_TYPE.MOVEMENT:
                                Move(tileTouched);
                                break;

                            case HexCell.SELECTION_TYPE.AIM:
                                Hero_AttacksManager.instance.ShowImpactRange(tileTouched);
                                break;
                            case HexCell.SELECTION_TYPE.AIM_IMPACT:
                                Hero_AttacksManager.instance.ShowImpactRange(tileTouched);
                                break;
                            case HexCell.SELECTION_TYPE.ORIGIN_AIM:
                                Hero_AttacksManager.instance.LaunchAttack();
                                break;
                            case HexCell.SELECTION_TYPE.ORIGIN_IMPACT:
                                Hero_AttacksManager.instance.LaunchAttack();
                                break;
                            default:
                                ShowMovements();
                                break;
                        }
                    }
                }
            }
        }
    }

    private void SetUIAttacks()
    {
        for (int i = 0; i < attacks.Count; i++)
        {
            if(myCanvas.transform.GetChild(0).transform.childCount - 1 < i)
            {
                print("NOT ENOUGH SLOTS");
                return;
            }
            UI_Attack UIAttack = myCanvas.transform.GetChild(0).transform.GetChild(i).GetComponent<UI_Attack>();
            UIAttack.attack = attacks[i];
            UIAttack.UpdateUI();

        }
    }

    private void SetMyStats()
    {
        GetComponent<SpriteRenderer>().sprite = stats.sprite;
        nameHero = stats.heroName;
        health = stats.health;
        PM = stats.PM;
        PA = stats.PA;
        SetUI_PA_PM();
        PVtxt.text = health.ToString();
    }

    public void StartTurn()
    {
        isMyTurn = true;
        //print("StartTurn");
        myCanvas.SetActive(true);
        SetUI_PA_PM();
        foreach (Transform UI_AttackBtn in myCanvas.transform.GetChild(0).transform)
        {
            UI_AttackBtn.GetComponent<UI_Attack>().StartTurn();
        }
        ShowMovements();

    }

    public void ShowMovements()
    {
        TilesManager.instance.ClearTiles(false);
  
        int rangePM;
        if (stats.isDofusPM) rangePM = PM;
        else rangePM = 1;

        if (PM >= 1)
        {
            foreach( HexCell tile in TilesManager.instance.GetRangeInRadius(myTile.coordinates, 1, rangePM, false, false))
            {
                tile.SelectCell(HexCell.SELECTION_TYPE.MOVEMENT);
            }
            
        }

    }

    public void EndTurn()
    {
        //print("endturn");
        isMyTurn = false;
        myCanvas.SetActive(false);
        PM = stats.PM;
        PA = stats.PA;
        CombatSystem.instance.NextTurn();
    }

    private void Move(HexCell tile)
    {
        PM -= TilesManager.instance.HeuristicDistance(myTile.coordinates, tile.coordinates);
        PA--;
        SetUI_PA_PM();
        
        myTile.hero = null;
        myTile = tile;
        myTile.hero = this;

        transform.position = myTile.transform.position;
        

        if (PA > 0)
        ShowMovements();
        else
        {
            TilesManager.instance.ClearTiles(false);
        }
    }

    public void SetUI_PA_PM()
    {
        PMtxt.text = PM.ToString();
        PAtxt.text = PA.ToString();
    }

    public void TakeDamages(int damages)
    {
        health -= damages;
        health = Mathf.Clamp(health, 0, stats.health);
        PVtxt.text = health.ToString();

        if(health == 0)
        {
            Death();
        }
    }

    private void Death()
    {
        print("Death");
    }
}

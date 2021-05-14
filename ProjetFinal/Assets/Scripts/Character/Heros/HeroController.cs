using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameObject UI_Attacks;
    public GameObject UI_AttackBTN;

    // VARIABLES GRID
    [HideInInspector]
    public HexCell myTile;

    private enum ACTION { NONE ,MOVE, ATTACK };
    private ACTION currentAction;

    bool isMyTurn = false;


    // Start is called before the first frame update
    void Start()
    {
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

        StartTurn(); // ------------- TO DELETE BECAUSE FIGHTMANAGER

        //foreach(Attack attack in attacks)
        //{
        //    GameObject newUIAttack = Instantiate(UI_AttackBTN, UI_Attacks.transform);
        //    newUIAttack.GetComponent<UI_Attack>().attack = attack;
        //    newUIAttack.GetComponent<UI_Attack>().hero = this;
        //}

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

                        if (tileTouched.isSelected)
                        {
                            switch (tileTouched.selectionType)
                            {
                                case HexCell.SELECTION_TYPE.MOVEMENT:
                                    Move(tileTouched);
                                    break;

                                case HexCell.SELECTION_TYPE.AIM:
                                    Hero_AttacksManager.instance.ShowImpactRange(tileTouched);
                                    break;
                                case HexCell.SELECTION_TYPE.IMPACT:

                                    break;
                                case HexCell.SELECTION_TYPE.AIM_IMPACT:

                                    break;
                            }

                        }
                        else
                        {
                            ShowMovements();
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
            if(UI_Attacks.transform.childCount - 1 < i)
            {
                print("NOT ENOUGH SLOTS");
                return;
            }
            UI_Attack UIAttack = UI_Attacks.transform.GetChild(i).GetComponent<UI_Attack>();
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
    }

    public void StartTurn()
    {
        isMyTurn = true;
        //PM = stats.PM;
        //PA = stats.PA;
        // Show my UI
        ShowMovements();

    }

    public void ShowMovements()
    {
        TilesManager.instance.ClearTiles();
        currentAction = ACTION.MOVE;

        if (PM >= 1)
        {
            foreach( HexCell tile in TilesManager.instance.GetMinMaxRange(myTile.coordinates, 0, PM)[1])
            {
                tile.SelectCell(HexCell.SELECTION_TYPE.MOVEMENT);
            }
            
        }

    }

    public void EndTurn()
    {
        isMyTurn = false;
        currentAction = ACTION.NONE;
        // BatlleManager.instance.NextTurn();
    }

    private void Move(HexCell tile)
    {
        PM -= TilesManager.instance.HeuristicDistance(myTile.coordinates, tile.coordinates);
        PA--;
        myTile.hero = null;
        myTile = tile;
        myTile.hero = this;

        transform.position = myTile.transform.position;
        

        if (PA > 0)
        ShowMovements();
        else
        {
            TilesManager.instance.ClearTiles();
        }
    }


    public void TakeDamages(int damages)
    {
        health -= damages;
        health = Mathf.Clamp(health, 0, stats.health);

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

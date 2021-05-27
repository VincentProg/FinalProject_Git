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

    public GameObject TXT_Damages;
    private bool canShowDamages = true;

    // ATTACKS
    public List<Attack> attacks = new List<Attack>();
    public GameObject myCanvas;
    private bool isMoving;

    public List<Grenade> grenades = new List<Grenade>();

    public HERO_TYPE heroType;

    public enum HERO_TYPE
    {
        SOLDIER,
        COWBOY
    }

    // VARIABLES GRID
    [HideInInspector]
    public HexCell myTile;

    public Color myTileColor;
    SpriteRenderer myHeroSprite;


    bool isMyTurn = false;
    int nbrTurnsToSkip = 0;

    // Start is called before the first frame update
    void Start()
    {

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
        myTile.myTileSprite.color = myTileColor;

        SetMyStats();
        SetUIAttacks();

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
                                Hero_AttacksManager.instance.LaunchAttack(this);
                                break;
                            case HexCell.SELECTION_TYPE.ORIGIN_IMPACT:
                                Hero_AttacksManager.instance.LaunchAttack(this);
                                break;
                            default:
                                ShowMovements();
                                break;
                        }
                    }
                }
            }

            else
            {
                if (PA > 0 && Input.GetMouseButtonDown(0))
                {
                    Vector3 touchPosWorld = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));

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
                                    Hero_AttacksManager.instance.LaunchAttack(this);
                                    break;
                                case HexCell.SELECTION_TYPE.ORIGIN_IMPACT:
                                    Hero_AttacksManager.instance.LaunchAttack(this);
                                    break;
                                default:
                                    ShowMovements();
                                    break;
                            }
                        }
                    }
                }
            }

            if (isMoving)
            {
                transform.position = Vector3.MoveTowards(transform.position, myTile.transform.position, 100f * Time.deltaTime);
                if (transform.position == myTile.transform.position)
                {
                    isMoving = false;
                    ArriveOnCell();
                }
            }
        }
    }
    private void SetUIAttacks()
    {
        for (int i = 0; i < attacks.Count; i++)
        {
            if(myCanvas.transform.GetChild(3).transform.childCount - 1 < i)
            {
                print("NOT ENOUGH SLOTS");
                return;
            }
            UI_Attack UIAttack = myCanvas.transform.GetChild(3).transform.GetChild(i).GetComponent<UI_Attack>();
            UIAttack.attack = attacks[i];
            UIAttack.UpdateUI();

        }
    }

    private void SetMyStats()
    {
        myHeroSprite = GetComponent<SpriteRenderer>();
        myHeroSprite.sprite = stats.sprite;
        myHeroSprite.sortingOrder = myTile.coordinates.Y - myTile.coordinates.X;
        nameHero = stats.heroName;
        health = stats.health;
        PM = stats.PM;
        PA = stats.PA;
    }

    public void StartTurn()
    {
        if(nbrTurnsToSkip > 0)
        {
            nbrTurnsToSkip--;
            EndTurn();
            return;
        }

        isMyTurn = true;
        //print("StartTurn");
        myCanvas.SetActive(true);
        foreach (Transform UI_AttackBtn in myCanvas.transform.GetChild(3).transform)
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
            foreach( HexCell tile in TilesManager.instance.GetRangeInRadius(myTile.coordinates, 1, rangePM, false, false, false))
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

        TilesManager.instance.ClearTiles(false);
        PopUpSystem.instance.Cut();

        for (int i = 0; i < grenades.Count; i++)
        {
            grenades[i].StartTurn();
        }

        

        CombatSystem.instance.NextTurn();
        return;
    }

    private void Move(HexCell tile)
    {
        if(heroType.Equals(HERO_TYPE.SOLDIER))
            AchievementsManager.CgkImpif4cQQEAIQDg_temp_ok = false;

        PM -= TilesManager.instance.HeuristicDistance(myTile.coordinates, tile.coordinates);
        PA--;


        myTile.hero = null;
        myTile.myTileSprite.color = TilesManager.instance.classicColor;
        myTile = tile;
        myTile.hero = this;
        myTile.myTileSprite.color = myTileColor;
        myHeroSprite.sortingOrder = myTile.coordinates.Y - myTile.coordinates.X;
        print("helo");



        isMoving = true;
    }
    
    private void ArriveOnCell() { 

        if (myTile.item != null)
        {
            myTile.ActionItem(true);
        }

        PopUpSystem.instance.Cut();

        if (PA > 0)
        ShowMovements();
        else
        {
            TilesManager.instance.ClearTiles(false);
        }
    }

    public void TakeDamages(int damages)
    {
        health -= damages;
        health = Mathf.Clamp(health, 0, stats.health);

        GameObject txt = Instantiate(TXT_Damages, transform.position, transform.rotation);
        txt.transform.GetChild(0).GetComponent<TextMeshPro>().text = damages.ToString();
        txt.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 10;
        Destroy(txt, 1);

        PopUpSystem.instance.PopUp("OUCH", this);

        if (health == 0)
        {
            Death();
        }
    }

    public void SkipTurns(int turnsToSkip)
    {
        nbrTurnsToSkip += turnsToSkip;
    }

    private void Death()
    {
        print("Death");
        CombatSystem.instance.state = CombatSystem.CombatState.Lose;
        ButtonManager.instance.ShowLose();
    }

}

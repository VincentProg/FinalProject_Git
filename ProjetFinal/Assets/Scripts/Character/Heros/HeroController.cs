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
    public HexCell myTile;

    private enum ACTION { NONE ,MOVE, ATTACK };
    private ACTION currentAction;


    // Start is called before the first frame update
    void Start()
    {
        SetMyStats();
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

        foreach(Attack attack in attacks)
        {
            GameObject newUIAttack = Instantiate(UI_AttackBTN, UI_Attacks.transform);
            newUIAttack.GetComponent<UI_Attack>().attack = attack;
            newUIAttack.GetComponent<UI_Attack>().hero = this;
        }

    }

    // Update is called once per frame
    void Update()
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

                    // CAS 1 : MA TUILE
                    if (tileTouched == myTile)
                    {
                        ShowMovements();
                    }

                    // CAS 2 : TUILE SELECTIONNEE
                    else if (tileTouched.isSelected)
                    {
                        switch (currentAction)
                        {
                            case ACTION.MOVE:
                                Move(tileTouched);
                                break;

                            case ACTION.ATTACK:

                                break;
                        }

                    }
                    else
                    {
                        // CAS 3 : TUILES NON SELECTIONNEE

                        ShowMovements();
                    }
                }
            }
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
        PM = stats.PM;
        PA = stats.PA;
        ShowMovements();

    }

    public void ShowMovements()
    {
        TilesManager.instance.ClearTiles();
        currentAction = ACTION.MOVE;

        if (PM >= 1)
        {
            TilesManager.instance.GetRange(myTile.coordinates, PM);
        }

    }

    public void EndTurn()
    {
        currentAction = ACTION.NONE;
        // BatlleManager.instance.NextTurn();
    }

    private void Move(HexCell tile)
    {
        myTile.hero = null;
        myTile = tile;
        myTile.hero = this;

        transform.position = myTile.transform.position;
        PM--; // ------------------------------------------------------ � modifier avec la distance s�parant
        PA--;

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

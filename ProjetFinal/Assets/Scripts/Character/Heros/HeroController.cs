using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    // STATISTIQUES
    public StatsHero stats;
    // ----
    public string nameHero;
    public int health;

    // ATTACKS
    public List<Attacks> attacks = new List<Attacks>();

    // VARIABLES GRID
    HexCell myTile;
    private bool isTilesArround_TurnedOn = false;


    // Start is called before the first frame update
    void Start()
    {
        setMyStats();
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

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
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
                        print("turn on");
                        TurnOn_TilesArround();
                    }

                    // CAS 2 : TUILE A PORTEE
                    else if (Vector2.Distance(gameObject.transform.position, tileTouched.transform.position) < 50)
                    {
                        if (isTilesArround_TurnedOn)
                        {
                            TurnOff_TilesArround();
                        }

                        if (tileTouched.canMoveHere)
                        {
                            transform.position = new Vector2(tileTouched.transform.position.x, tileTouched.transform.position.y);
                            myTile.hero = null;
                            tileTouched.hero = this;
                            myTile = tileTouched;

                        }

                    }
                    else
                    {
                        // CAS 3 : TUILES TROP LOIN
                        if (isTilesArround_TurnedOn)
                        {
                            TurnOff_TilesArround();
                        }
                        Debug.Log("Too far");
                    }
                }
            }
        }

        




    }

    private void setMyStats()
    {
        SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
        nameHero = stats.heroName;
        health = stats.health;
    }

    private void Attack(int id)
    {
        switch (attacks[id].type)
        {
            case Attacks.ATTACK_TYPE.MELEE:

                break;

            case Attacks.ATTACK_TYPE.SHOT:

                break;

            case Attacks.ATTACK_TYPE.RADIUS:

                break;
        }
    }



    private void TurnOn_TilesArround()
    {
        isTilesArround_TurnedOn = true;
        foreach (HexCoordinates arround in TilesManager.instance.tilesArround)
        {
            HexCoordinates tileCoordinates = new HexCoordinates(myTile.coordinates.X + arround.X, myTile.coordinates.Z + arround.Z);
            HexCell tileArround;
            TilesManager.instance.mapTiles.TryGetValue(tileCoordinates, out tileArround);

            if (tileArround != null)
            {
                tileArround.transform.GetComponent<SpriteRenderer>().color = TilesManager.instance.lightingColor;
            }
        }
    }
    private void TurnOff_TilesArround()
    {
        isTilesArround_TurnedOn = false;
        foreach (HexCoordinates arround in TilesManager.instance.tilesArround)
        {
            HexCoordinates tileCoordinates = new HexCoordinates(myTile.coordinates.X + arround.X, myTile.coordinates.Z + arround.Z);
            HexCell tileArround;
            TilesManager.instance.mapTiles.TryGetValue(tileCoordinates, out tileArround);

            if (tileArround != null)
            {
                tileArround.transform.GetComponent<SpriteRenderer>().color = TilesManager.instance.baseColor;
            }
        }
    }
}

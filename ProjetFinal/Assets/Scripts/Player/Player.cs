using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Tile myTile;
    Vector3 touchPosWorld;

    //Change me to change the touch phase used.
    TouchPhase touchPhase = TouchPhase.Ended;

    private bool isTilesArround_TurnedOn = false;

    private void Start()
    {
        // AJOUT TUILE DEPART
        RaycastHit2D hitStart = Physics2D.Raycast(transform.position, Vector2.zero, Mathf.Infinity);
        if (hitStart)
        {
            if (hitStart.transform.GetComponent<Tile>())
            {
                myTile = hitStart.transform.GetComponent<Tile>();
                myTile.player = this;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == touchPhase)
        {
            touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

            Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
            RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);
            if (hitInformation)
            {
                if(hitInformation.transform.GetComponent<Tile>() != null)
                {
                    Tile tileTouched = hitInformation.transform.GetComponent<Tile>();

                    // CAS 1 : MA TUILE
                    if(tileTouched == myTile)
                    {
                        print("turn on");
                        TurnOn_TilesArround();
                    }

                    // CAS 2 : TUILE A PORTEE
                    else if(Vector2.Distance(gameObject.transform.position, tileTouched.position) < 1.1f)
                    {
                        if (isTilesArround_TurnedOn)
                        {
                            TurnOff_TilesArround();
                        }

                        if (tileTouched.canMoveHere)
                        {
                            transform.position = new Vector2(tileTouched.position.x, tileTouched.position.y);
                            myTile.player = null;
                            tileTouched.player = this;
                            myTile = tileTouched;
                          
                        }
                        
                    } else
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

    private void TurnOn_TilesArround()
    {
        isTilesArround_TurnedOn = true;
        foreach (Vector2 arround in GridMap.instance.tilesArround)
        {
            Vector2 tilePosition = new Vector2(myTile.position.x + arround.x, myTile.position.y + arround.y);
            Tile tileArround;
            GridMap.instance.mapTiles.TryGetValue(tilePosition, out tileArround);

            if (tileArround != null)
            {
                tileArround.transform.GetComponent<SpriteRenderer>().color = GridMap.instance.lightingColor;
            }
            else
            {
                print("tile not found");
                print("tile position = " + tilePosition);
                print("my tile pos X + tile.x =  " + (myTile.position.x + arround.x));
            }
        }
    }
    private void TurnOff_TilesArround()
    {
        isTilesArround_TurnedOn = false;
        foreach (Vector2 arround in GridMap.instance.tilesArround)
        {
            Vector2 tilePosition = new Vector2(myTile.position.x + arround.x, myTile.position.y + arround.y);
            Tile tileArround;
            GridMap.instance.mapTiles.TryGetValue(tilePosition, out tileArround);

            if (tileArround != null)
            {
                tileArround.transform.GetComponent<SpriteRenderer>().color = GridMap.instance.baseColor;
            }
        }
    }
}

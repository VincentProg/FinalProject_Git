using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    GameObject _previousTile = null;
    Vector3 touchPosWorld;

    //Change me to change the touch phase used.
    TouchPhase touchPhase = TouchPhase.Ended;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == touchPhase)
        {
            touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

            Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
            RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);

            if (hitInformation.collider != null)
            {
                GameObject touchedObject = hitInformation.transform.gameObject;
                if(touchedObject.GetComponent<Tile>() != null)
                {
                    if(Vector2.Distance(gameObject.transform.position, touchedObject.transform.position) < 1.1)
                    {
                        if (touchedObject.GetComponent<Tile>().canMoveHere)
                        {
                            if(_previousTile != null) _previousTile.GetComponent<PolygonCollider2D>().enabled = true;
                            
                            GameObject tempTile;


                            gameObject.transform.position = new Vector2(touchedObject.transform.position.x, touchedObject.transform.position.y);
                            touchedObject.GetComponent<PolygonCollider2D>().enabled = false;

                            _previousTile = touchedObject;


                            foreach (Vector2 pos in MapGrid.mapTiles.Keys)
                            {
                                MapGrid.mapTiles.TryGetValue(pos, out tempTile);
                                tempTile.GetComponent<SpriteRenderer>().color = MapGrid.staticBaseColor;
                            }

                            RaycastHit2D[] hits = Physics2D.RaycastAll(touchedObject.transform.position, new Vector2(0, 1), Mathf.Infinity);
                            ColorDiagonal(hits);

                            hits = Physics2D.RaycastAll(touchedObject.transform.position, new Vector2(0, -1), Mathf.Infinity);
                            ColorDiagonal(hits);

                            hits = Physics2D.RaycastAll(touchedObject.transform.position, new Vector2(2/3, 1), Mathf.Infinity);
                            ColorDiagonal(hits);

                            hits = Physics2D.RaycastAll(touchedObject.transform.position, new Vector2(2/3, -1), Mathf.Infinity);
                            ColorDiagonal(hits);

                            hits = Physics2D.RaycastAll(touchedObject.transform.position, new Vector2(-2/3, 1), Mathf.Infinity);
                            ColorDiagonal(hits);
                            hits = Physics2D.RaycastAll(touchedObject.transform.position, new Vector2(-2/3, -1), Mathf.Infinity);
                            ColorDiagonal(hits);



                        }
                    } else
                    {
                        Debug.Log("Too far");
                    }
                }
            }
        }
    }

    void ColorDiagonal(RaycastHit2D[] hits)
    {
        GameObject tempTile;
        bool firstFound = false;
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null)
            {

                MapGrid.mapTiles.TryGetValue(hit.collider.gameObject.transform.position, out tempTile);
                tempTile.GetComponent<SpriteRenderer>().color = Color.blue;

                if (!firstFound)
                {
                    firstFound = true;
                    tempTile.GetComponent<SpriteRenderer>().color = Color.green;
                }
            }
        }
    }
}

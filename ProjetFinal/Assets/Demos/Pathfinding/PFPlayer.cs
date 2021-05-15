using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PFPlayer : MonoBehaviour
{
    public HexCell myTile;
    HexCell touchedtile;


    int index = 0;
    bool run = false;
    public float cooldown = 2;
    float lastUpdate;

    // Start is called before the first frame update
    void Start()
    {

        RaycastHit2D hitStart = Physics2D.Raycast(transform.position, Vector2.zero, Mathf.Infinity);
        if (hitStart)
        {
            if (hitStart.transform.GetComponent<HexCell>())
            {
                myTile = hitStart.transform.GetComponent<HexCell>();
            }
        }
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


                    if (Vector2.Distance(gameObject.transform.position, tileTouched.transform.position) < 500)
                    {
                            /*                            transform.position = new Vector2(tileTouched.transform.position.x, tileTouched.transform.position.y);
                                                        myTile.hero = null;
                                                        myTile = tileTouched;*/
                        tileTouched.gameObject.GetComponent<SpriteRenderer>().color = Color.black;

                        startPath(myTile, tileTouched);
                    }
                }
            }
        }
    }


    private void FixedUpdate()
    {
        if (run)
        {
            if (lastUpdate > cooldown)
            {
                lastUpdate = 0;
                if (index < 400)
                {
                    index++;
                    TilesManager.instance.GetPath(myTile.coordinates, touchedtile.coordinates, false, false, index);
                }
                else
                {
                    index = 0;
                    run = false;
                }
            }
            else
            {
                lastUpdate += Time.deltaTime;
            }
        }
    }


    public void startPath(HexCell mytile1, HexCell touchedtile1)
    {
        myTile = mytile1;
        touchedtile = touchedtile1;

        index = 0;
        run = true;
    }
}

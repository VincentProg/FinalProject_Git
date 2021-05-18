using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{

    public HexCell myTile;
    HexCell tileTouched;


    // Start is called before the first frame update
    void Start()
    {
        RaycastHit2D hitStart = Physics2D.Raycast(transform.position, Vector2.zero, Mathf.Infinity);
        if (hitStart)
        {
            if (hitStart.transform.GetComponent<HexCell>())
            {
                myTile = hitStart.transform.GetComponent<HexCell>();
                myTile.isHero = true;

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
                    tileTouched = hitInformation.transform.GetComponent<HexCell>();


                   /* if (Vector2.Distance(gameObject.transform.position, tileTouched.transform.position) < 500)
                    {*/
/*                        gameObject.transform.position = tileTouched.gameObject.transform.position;
                        RaycastHit2D hitStart = Physics2D.Raycast(transform.position, Vector2.zero, Mathf.Infinity);
                        if (hitStart)
                        {
                            if (hitStart.transform.GetComponent<HexCell>())
                            {
                                myTile = hitStart.transform.GetComponent<HexCell>();
                            }
                        }

                        List<List<HexCell>> result = TilesManager.instance.GetFOV(myTile, 50, true);

                        foreach (var item in result[0])
                        {
                            item.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                        }

                        foreach (var item in result[1])
                        {
                            item.gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
                        }*/
                    //}
                }
            }
        }
    }
}

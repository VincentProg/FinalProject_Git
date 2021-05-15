using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{

    public HexCell myTile;


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

        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
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

                        List<List<HexCell>> results = TilesManager.instance.GetMinMaxRange(myTile.coordinates, 0, 5);

                        foreach (HexCell item in results[0])
                        {
                            item.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                        }
                        foreach (HexCell item in results[1])
                        {
                            item.GetComponent<SpriteRenderer>().color = Color.green;
                        }
                    }
                }
            }
        }
    }
}

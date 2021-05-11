using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
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
                    if(Vector2.Distance(gameObject.transform.position, touchedObject.transform.position) < 1)
                    {
                        if (touchedObject.GetComponent<Tile>().canMoveHere)
                        {
                            gameObject.transform.position = new Vector2(touchedObject.transform.position.x, touchedObject.transform.position.y);
                        }
                    }
                }
            }
        }
    }
}

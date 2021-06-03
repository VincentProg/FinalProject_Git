using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTypeElement : MonoBehaviour
{

    HexCell myTile;
    public HexCell.TILE_TYPE type;



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
            }
        }
        #endregion

        myTile.UpdateTileDatas(type);
        transform.position = myTile.transform.position;
        //GetComponent<SpriteRenderer>().sortingOrder = myTile.coordinates.Y - myTile.coordinates.X;
        int order = 0;
        if(type != HexCell.TILE_TYPE.HOLE)
        order = -myTile.coordinates.X;
        else order = -myTile.coordinates.X - 1;

        GetComponent<SpriteRenderer>().sortingOrder = order;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public int damages;
    public int range;

    public HexCell myTile;


    public void Attack()
    {
        foreach(HexCell tile in TilesManager.instance.GetRange(myTile.coordinates, range, true, false)){
            
            if(tile.hero != null)
            {
                tile.hero.TakeDamages(damages);
            } else if (tile.enemy != null)
            {
                tile.enemy.TakeDamages(damages);
            }

        }

        Destroy(gameObject);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField]
    private int damages, range;
   
    public bool isFriendlyFire;

    public HexCell myTile;


    public void Attack()
    {
        foreach(HexCell tile in TilesManager.instance.GetRange(myTile.coordinates, range, true, false)){
            
            if(isFriendlyFire && tile.hero != null)
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

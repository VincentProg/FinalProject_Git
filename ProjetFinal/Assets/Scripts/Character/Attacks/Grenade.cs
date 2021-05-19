using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Grenade : MonoBehaviour
{

    public enum TYPE_GRENADE {  EXPLOSE, FLASH};
    public TYPE_GRENADE type;

    public int delay;
    public int range;
    public int damages;

    bool isFriendlyFire;

    [HideInInspector]
    public HexCell myTile;
    public HeroController hero;

    public void StartTurn()
    {
        delay--;
        if(delay <= 0)
        {
            Explode();
        }
    }


    private void Explode()
    {
        List<HexCell> listTiles = new List<HexCell>();
        listTiles = TilesManager.instance.GetRange(myTile.coordinates, range, true, false);

        switch (type)
        {

            case TYPE_GRENADE.EXPLOSE:

                foreach(HexCell tile in listTiles)
                {
                    if(!isFriendlyFire && tile.hero != null)
                    {
                        tile.hero.TakeDamages(damages);
                    } else if(tile.enemy != null)
                    {
                        tile.enemy.TakeDamages(damages);
                    }
                }

                break;

            case TYPE_GRENADE.FLASH:

                break;

        }

    }

    private void Death()
    {
        Destroy(gameObject);
    }
}

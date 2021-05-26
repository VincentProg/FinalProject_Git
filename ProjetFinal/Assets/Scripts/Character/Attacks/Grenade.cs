using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Grenade : MonoBehaviour
{

    public enum TYPE_GRENADE {EXPLOSE, FLASH};
    public TYPE_GRENADE type;

    public int delay = 1;
    public int range;
    public int damagesExplose;
    public int nbrTurnSkipFlash;

    public bool isFriendlyFire;

    [HideInInspector]
    public HexCell myTile;

    [HideInInspector]
    public HeroController hero;

    public void StartTurn()
    {
        delay--;
        if(delay < 0)
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
                    if(isFriendlyFire && tile.hero != null)
                    {
                        tile.hero.TakeDamages(damagesExplose);
                    } else if(tile.enemy != null)
                    {
                        tile.enemy.TakeDamages(damagesExplose);
                    }
                }

                break;

            case TYPE_GRENADE.FLASH:

                foreach (HexCell tile in listTiles)
                {
                    if (!isFriendlyFire && tile.hero != null)
                    {
                        tile.hero.SkipTurns(nbrTurnSkipFlash);
                    }
                    else if (tile.enemy != null)
                    {
                        tile.enemy.SkipTurns(nbrTurnSkipFlash);
                    }
                }

                break;

        }

        Death();

    }

    private void Death()
    {
        print(hero.gameObject.name);
        hero.grenades.Remove(this);
        Destroy(gameObject);
    }
}

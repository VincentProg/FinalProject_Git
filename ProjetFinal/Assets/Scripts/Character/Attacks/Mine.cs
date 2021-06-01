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
        GameObject particles = Instantiate(CombatSystem.instance.mineParticle, transform);
        particles.transform.SetParent(null);
        particles.transform.localScale = new Vector3(9, 9, 9);
        particles.transform.eulerAngles = new Vector3(-90f, 0, 0);

        foreach (HexCell tile in TilesManager.instance.GetRange(myTile.coordinates, range, true, false)){
            if(isFriendlyFire && tile.hero != null)
            {
                tile.hero.TakeDamages(damages, "mine", "explosion");
            } else if (tile.enemy != null)
            {
                tile.enemy.TakeDamages(damages, "mine", "explosion");
            }

        }

        Destroy(gameObject);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // STATISTIQUES
    public StatsEnemy stats;
    // ----
    public string nameHero;
    public int health;


    // VARIABLES GRID
    HexCell myTile;


    // Start is called before the first frame update
    void Start()
    {
        setMyStats();
        #region GET MY START TILE()
        // AJOUT TUILE DEPART
        RaycastHit2D hitStart = Physics2D.Raycast(transform.position, Vector2.zero, Mathf.Infinity);
        if (hitStart)
        {
            if (hitStart.transform.GetComponent<HexCell>())
            {
                myTile = hitStart.transform.GetComponent<HexCell>();
                myTile.enemy = this;
            }
        }
        #endregion

    }

    private void setMyStats()
    {
        SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
        nameHero = stats.enemyName;
        health = stats.health;
    }
}

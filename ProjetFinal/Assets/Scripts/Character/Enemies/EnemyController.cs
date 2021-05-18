using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private bool isFirstTurn = true;
    // STATISTIQUES
    public StatsEnemy stats;
    // ----
    private string nameEnemy;
    private int health, PM, PA, damages;

    private bool isActionDone = true;
  


    // VARIABLES GRID
    HexCell myTile;

    HeroController hero1;
    HeroController hero2;



   



    public void Initialize()
    {
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

        hero1 = CombatSystem.instance.heros[0];
        hero2 = CombatSystem.instance.heros[1];

        SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
        mySprite.sprite = stats.sprite;
        nameEnemy = stats.enemyName;
        health = stats.health;
        PM = stats.PM;
        PA = stats.PA;
        damages = stats.damages;

    }

    public void StartTurn()
    {
        if (isFirstTurn)
        {
            isFirstTurn = false;
            CombatSystem.instance.NextTurn();
            return;
        }

        while (PA > 0 && isActionDone)
        {
            CheckAction();          
        }

        EndTurn();
    }
    private void EndTurn()
    {
        PM = stats.PM;
        PA = stats.PA;
        CombatSystem.instance.NextTurn();
    }

    private void CheckAction()
    {
        int dist1 = TilesManager.instance.GetPath(myTile.coordinates, hero1.myTile.coordinates, false, false).Count;
        int dist2 = TilesManager.instance.GetPath(myTile.coordinates, hero2.myTile.coordinates, false, false).Count;
        HeroController hero = null;


        if (dist1 == 1 && dist2 == 1)
        {
            if (hero1.health < hero2.health)
            {
                hero = hero1;
            }
            else hero = hero2;

        } else if (dist1 == 1) hero = hero1;
          else if(dist2 == 1) hero = hero2;

        if(hero != null)
        {
            AttackCAC(hero);
        } else 
        {
            if (dist1 < dist2)
                hero = hero1;
            else if (dist1 == dist2)
            {
                if (hero1.health < hero2.health)
                {
                    hero = hero1;
                }
                else hero = hero2;
            }
            else hero = hero2;

            Move(hero);
        }
        

    }

    private void Move(HeroController hero)
    {

        if (PM > 0 && PA > 0)
        {
            List<HexCoordinates> path = new List<HexCoordinates>();
            path = TilesManager.instance.GetPath(myTile.coordinates, hero.myTile.coordinates, false, false);
            HexCell tile;
            TilesManager.instance.mapTiles.TryGetValue(path[path.Count - 1], out tile);

            myTile.enemy = null;
            myTile = tile;
            tile.enemy = this;
            transform.position = myTile.transform.position;

            PM -= 1;
            PA -= 1;
            isActionDone = true;

            
        }
    }

    private void AttackCAC(HeroController hero)
    {
        if (PA > 0)
        {
            hero.TakeDamages(damages);
            PA--;
            isActionDone = true;
        }
    }



    public void TakeDamages(int damages)
    {
        health -= damages;
        health = Mathf.Clamp(health, 0, stats.health);

        if (health == 0)
        {
            Death();
        }
    }

    private void Death()
    {
        CombatSystem.instance.enemies.Remove(this);
        Destroy(gameObject);
        print("Death");
    }
}

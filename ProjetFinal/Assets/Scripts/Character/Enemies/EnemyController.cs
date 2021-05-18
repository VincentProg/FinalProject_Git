using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public bool hasSpawned;
    private bool isFirstTurn = true;
    private int nbrTurnToSkip;
    // STATISTIQUES
    public StatsEnemy stats;
    // ----
    private string nameEnemy;
    private int health, PM, PA;

    private bool isActionDone = true;
  

    // VARIABLES GRID
    HexCell myTile;
    HexCell moveTo;

    HexCell targetCell;

    HeroController hero1;
    HeroController hero2;


    private void Start()
    {
        if (!hasSpawned)
        {
            Initialize();

        }
    }


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

        CombatSystem.instance.enemies.Add(this);
        hero1 = CombatSystem.instance.heros[0];
        hero2 = CombatSystem.instance.heros[1];

        SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
        mySprite.sprite = stats.sprite;
        nameEnemy = stats.enemyName;
        health = stats.health;
        PM = stats.PM;
        PA = stats.PA;

    }

    public void StartTurn()
    {
        if (nbrTurnToSkip > 0)
        {
            nbrTurnToSkip--;
            EndTurn();
            return;
        }

        if (isFirstTurn)
        {
            isFirstTurn = false;
            CombatSystem.instance.NextTurn();
            return;
        }

        while (PA > 0 && isActionDone)
        {
            isActionDone = false;
            CheckAction();
        }

        EndTurn();
    }
    private void EndTurn()
    {
        PM = stats.PM;
        PA = stats.PA;
        isActionDone = true;
        CombatSystem.instance.NextTurn();
    }

    private void CheckAction()
    {
        switch (stats.Type) {

            case StatsEnemy.ENEMY_TYPE.CAC:
                #region CAC_CheckAction
                int dist1 = TilesManager.instance.GetPath(myTile.coordinates, hero1.myTile.coordinates, false, false).Count;
                int dist2 = TilesManager.instance.GetPath(myTile.coordinates, hero2.myTile.coordinates, false, false).Count;
                HeroController hero = null;
                int distHero = 0;
                int distRange = stats.attacks[0].range + 1;

                if (dist1 <= distRange && dist2 <= distRange)
                {
                    if (hero1.health < hero2.health)
                    {
                        hero = hero1;
                        distHero = dist1;
                    }
                    else
                    {
                        hero = hero2;
                        distHero = dist2;
                    }

                }
                else if (dist1 <= distRange) { hero = hero1; distHero = dist1; }
                else if (dist2 <= distRange) { hero = hero2; distHero = dist2; }

                if (hero != null)
                {
                    AttackCAC(hero, distHero);
                }
                else
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

                    MoveCAC(hero);
                }
                #endregion
                break;

            case StatsEnemy.ENEMY_TYPE.DISTANCE:
                dist1 = TilesManager.instance.HeuristicDistance(myTile.coordinates, hero1.myTile.coordinates);
                dist2 = TilesManager.instance.HeuristicDistance(myTile.coordinates, hero2.myTile.coordinates);

                if(dist1 <= dist2)
                    targetCell = hero1.myTile;
                else
                    targetCell = hero2.myTile;


                // If player already in line of sight 
                List<List<HexCell>> diagonals = TilesManager.instance.GetDiagonals(myTile.coordinates, stats.attacks[0].range, true, false, TilesManager.instance.GetDirection(myTile.coordinates, targetCell.coordinates));
                List<List<HexCell>> fov = TilesManager.instance.GetFOV(myTile, TilesManager.instance.HeuristicDistance(myTile.coordinates, targetCell.coordinates) + 1, true);

                bool inRange = false;
                foreach (var diagonal in diagonals)
                {
                    foreach (var item in diagonal)
                    {
                        if (item.isPossessed())
                        {
                            inRange = true;
                            break;
                        }
                    }
                }

                // If too close
                if (TilesManager.instance.HeuristicDistance(myTile.coordinates, targetCell.coordinates) <= stats.distanceToStayAway && fov[0].Contains(targetCell))
                {
                    MoveAway();

                }
                else
                {
                    if (inRange)
                    {
                        AttackDistance();
                    }
                    else
                    {
                        MoveClosestDiag(fov[0]);
                    }
                }

                break;
        }
    }

    #region Distance Methods
    private void MoveClosestDiag(List<HexCell> fov)
    {
        // Try and get nearest diagonals
        moveTo = TilesManager.instance.GetClosestDiagonal(myTile.coordinates, targetCell.coordinates, stats.distanceToStayAway);
        if (moveTo)
        {
            Debug.Log("moveto " + moveTo.coordinates);

            if (fov.Contains(moveTo))
            {
                int direction = TilesManager.instance.GetDirection(myTile.coordinates, moveTo.coordinates);
                HexCoordinates newCoords = TilesManager.instance.GetNeighboor(myTile.coordinates, direction);
                Debug.Log("newCoords " + newCoords);
                Debug.Log("direction " + direction);

                TilesManager.instance.mapTiles.TryGetValue(newCoords, out moveTo);
                Debug.Log("movetonewCoords " + moveTo.coordinates);

                if (moveTo)
                    MoveDistance();
            }
            else
            {
                List<HexCoordinates> newCoords = TilesManager.instance.GetPath(myTile.coordinates, moveTo.coordinates, false, false);
                TilesManager.instance.mapTiles.TryGetValue(newCoords[newCoords.Count - 1], out moveTo);
                if (moveTo)
                    MoveDistance();
            }

        }
        else
        {
            List<HexCoordinates> newCoords = TilesManager.instance.GetPath(myTile.coordinates, targetCell.coordinates, false, false);
            if (newCoords.Count > 2)
            {
                TilesManager.instance.mapTiles.TryGetValue(newCoords[newCoords.Count - 1], out moveTo);

                if (moveTo)
                {
                    if (TilesManager.instance.HeuristicDistance(myTile.coordinates, targetCell.coordinates) <= stats.distanceToStayAway && fov.Contains(targetCell))
                    {
                        MoveAway();
                    }
                    else
                    {
                        MoveDistance();
                    }
                }
            }
        }
    }

    private void MoveAway()
    {

        int direct = TilesManager.instance.GetDirection(targetCell.coordinates, myTile.coordinates);

        List<HexCell> tiles = TilesManager.instance.GetImpactArc(myTile.coordinates, TilesManager.instance.GetNeighboor(myTile.coordinates, direct), false, false);
        moveTo = tiles[0];
        bool canMove = false;
        foreach (var item in tiles)
        {
            // Can move here
            if ((item.tileType.Equals(HexCell.TILE_TYPE.GROUND) || (item.tileType.Equals(HexCell.TILE_TYPE.HOLE) && stats.isFlying)) && !item.isPossessed() && !item.tileType.Equals(HexCell.TILE_TYPE.WALL))
            {
                moveTo = item;
                canMove = true;
                break;
            }
        }

        if (canMove)
        {
            MoveDistance();
        }
        else
        {
            tiles = TilesManager.instance.GetRadius(myTile.coordinates, 1, stats.isFlying, false);
            foreach (var item in tiles)
            {
                // Can move here
                if ((item.tileType.Equals(HexCell.TILE_TYPE.GROUND) || (item.tileType.Equals(HexCell.TILE_TYPE.HOLE) && stats.isFlying)) && !item.isPossessed() && !item.tileType.Equals(HexCell.TILE_TYPE.WALL))
                {
                    moveTo = item;
                    canMove = true;
                    break;

                }
            }
        }

        if (canMove)
        {
            MoveDistance();
        }
        else
        {
            Debug.Log("couldn't find path out of here");
        }
    }

    private void MoveDistance()
    {
        if (PM > 0 && PA > 0)
        {
            myTile.enemy = null;
            myTile = moveTo;
            myTile.enemy = this;
            transform.position = myTile.transform.position;

            if (myTile.item != null)
            {
                myTile.ActionItem();
            }

            PM -= 1;
            PA -= 1;
            isActionDone = true;
        }
    }

    private void AttackDistance()
    {
        Debug.Log("attack");
    }
    #endregion

    #region CAC Methods
    private void MoveCAC(HeroController hero)
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

            if (myTile.item != null)
            {
                myTile.ActionItem(true);
            }

            PM -= 1;
            PA -= 1;
            isActionDone = true;

            
        }
    }

    private void AttackCAC(HeroController hero, int distanceHero)
    {

        if (PA >= stats.attacks[0].costPA)
        {
            if (stats.attacks[0].range < 1)
            {
                hero.TakeDamages(stats.attacks[0].damages);  
            } else
            {
                List<HexCoordinates> path = new List<HexCoordinates>();
                path = TilesManager.instance.GetPath(myTile.coordinates, hero.myTile.coordinates, false, false);
                HexCell tileAimed;
                TilesManager.instance.mapTiles.TryGetValue(path[path.Count - 1], out tileAimed);
                foreach (HexCell tile in TilesManager.instance.GetRange(tileAimed.coordinates, stats.attacks[0].range, false, false))
                {
                    if(tile != myTile)
                    {
                        if (tile.hero)
                        {
                            tile.hero.TakeDamages(stats.attacks[0].damages);
                        } else if (tile.enemy)
                        {
                            tile.enemy.TakeDamages(stats.attacks[0].damages);
                        }
                    } 
                }
            }
            PA -= stats.attacks[0].costPA;
            isActionDone = true;
        }
    }
    #endregion


    public void TakeDamages(int damages)
    {
        health -= damages;
        health = Mathf.Clamp(health, 0, stats.health);

        if (health == 0)
        {
            Death();
        }
    }

    public void SkipTurns(int turnsToSkip)
    {
        nbrTurnToSkip += turnsToSkip;
    }

    private void Death()
    {
        CombatSystem.instance.enemies.Remove(this);
        Destroy(gameObject);
        print("Death");
    }
}

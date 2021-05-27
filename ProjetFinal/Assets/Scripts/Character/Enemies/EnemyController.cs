using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public bool hasSpawned;
    public bool SkipFirstTurn = true;
    private int nbrTurnToSkip;
    // STATISTIQUES
    public StatsEnemy stats;
    // ----
    private string nameEnemy;
    private int health, PM, PA;
    public GameObject TXT_Damages;

    private bool isMoving;
    private bool isActionDone = true;

    [SerializeField]
    Color myTileColor;

    SpriteRenderer myAlienSprite;

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

    private void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, myTile.transform.position, 50f*Time.deltaTime);
            if (transform.position == myTile.transform.position)
            {
                isMoving = false;
                ArriveOnCell();
            }
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
        myTile.myTileSprite.color = myTileColor;

        CombatSystem.instance.enemies.Add(this);
        hero1 = CombatSystem.instance.heros[0];
        hero2 = CombatSystem.instance.heros[1];

        myAlienSprite = GetComponent<SpriteRenderer>();
        myAlienSprite.sprite = stats.sprite;
        myAlienSprite.sortingOrder = myTile.coordinates.Y - myTile.coordinates.X;
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

        if (SkipFirstTurn)
        {
            SkipFirstTurn = false;
            CombatSystem.instance.NextTurn();

            return;
        }

        ContinueTurn();
    }

    public void ContinueTurn()
    {

        if (PA > 0 && isActionDone)
        {
            isActionDone = false;
            CheckAction();
        } else
        {
            EndTurn();
        }
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
                List<HexCoordinates> path1 = TilesManager.instance.GetPath(myTile.coordinates, hero1.myTile.coordinates, stats.isFlying, false);
                List<HexCoordinates> path2 = TilesManager.instance.GetPath(myTile.coordinates, hero2.myTile.coordinates, stats.isFlying, false);
                int dist1 = 100;
                int dist2 = 100;

                HeroController hero = null;

                if (path1.Count == 1)
                {
                    hero = hero1;
                }
                else if (path2.Count == 1)
                {
                    hero = hero2;
                }

                if(hero != null)
                {
                    AttackCAC(hero);
                    return;
                }
                

                if (!(path1[1].X == 0 && path1[1].Z == 0))
                {
                    dist1 = path1.Count;
                }
                if (!(path2[1].X == 0 && path2[1].Z == 0))
                {
                    dist2 = path2.Count;
                }
                if (dist1 == 100 && dist2 == 100)
                {
                    ContinueTurn();
                    return;
                }
                
                
                
                int distRange = stats.attacks[0].range;

                if (dist1 <= distRange && dist2 <= distRange)
                {
                    if (hero1.health < hero2.health)
                    {
                        hero = hero1;
                    }
                    else
                    {
                        hero = hero2;                    }

                }
                else if (dist1 <= distRange) hero = hero1;
                else if (dist2 <= distRange) hero = hero2;

                if (hero != null)
                {
                    AttackCAC(hero);
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
                #region DISTANCE.CheckAction
                dist1 = TilesManager.instance.HeuristicDistance(myTile.coordinates, hero1.myTile.coordinates);
                dist2 = TilesManager.instance.HeuristicDistance(myTile.coordinates, hero2.myTile.coordinates);

                if(dist1 <= dist2)
                    targetCell = hero1.myTile;
                else
                    targetCell = hero2.myTile;

                // If player already in line of sight 
                List<List<HexCell>> diagonals = TilesManager.instance.GetDiagonals(myTile.coordinates, 0, stats.attacks[0].range, true, false, true, TilesManager.instance.GetDirection(myTile.coordinates, targetCell.coordinates));
                List<List<HexCell>> fov = TilesManager.instance.GetFOV(myTile, TilesManager.instance.HeuristicDistance(myTile.coordinates, targetCell.coordinates) + 1, true);

                bool inRange = false;
                foreach (var diagonal in diagonals)
                {
                    foreach (var item in diagonal)
                    {
                        if (item.isHero())
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
                #endregion
                break;

            case StatsEnemy.ENEMY_TYPE.KAMIKAZE:
                #region KAMIKAZE_CheckAction
                dist1 = TilesManager.instance.HeuristicDistance(myTile.coordinates, hero1.myTile.coordinates);
                dist2 = TilesManager.instance.HeuristicDistance(myTile.coordinates, hero2.myTile.coordinates);

                if (dist1 == 1 || dist2 == 1)
                {
                    if (PA >= stats.attacks[0].costPA)
                        Death(true, "self", "explosion");
                    else ContinueTurn();

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
        }
    }

    #region Distance Methods
    private void MoveClosestDiag(List<HexCell> fov)
    {
        // Try and get nearest diagonals
        moveTo = TilesManager.instance.GetClosestDiagonal(myTile.coordinates, targetCell.coordinates, fov, stats.distanceToStayAway);
        if (moveTo)
        {
            if (!moveTo.isPossessed())
            {


            int direction = TilesManager.instance.GetDirection(myTile.coordinates, moveTo.coordinates);
            HexCoordinates newCoords = TilesManager.instance.GetNeighboor(myTile.coordinates, direction);

            TilesManager.instance.mapTiles.TryGetValue(newCoords, out moveTo);
            if (moveTo)
                MoveDistance();
            }
            else
            {
                MovePath(fov);
            }
        }
        else
        {
            MovePath(fov);
        }
    }

    private void MovePath(List<HexCell> fov)
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
            tiles = TilesManager.instance.GetRadius(myTile.coordinates, 1, stats.isFlying, false, false);
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
            int distance = TilesManager.instance.HeuristicDistance(myTile.coordinates, targetCell.coordinates);
            if (distance <= stats.attacks[0].range && TilesManager.instance.GetFOV(myTile, distance, true)[0].Contains(targetCell))
            {
                AttackDistance();

            }
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
                myTile.ActionItem(false);
            }

            PM -= 1;
            PA -= 1;
            isActionDone = true;
        }
    }

    private void AttackDistance()
    {
    }
    #endregion

    #region CAC Methods
    private void MoveCAC(HeroController hero)
    {

        if (PM > 0 && PA > 0)
        {
            List<HexCoordinates> path = new List<HexCoordinates>();
            path = TilesManager.instance.GetPath(myTile.coordinates, hero.myTile.coordinates, stats.isFlying, false);

            if (path.Count > 1)
            {
                HexCell tile;
                TilesManager.instance.mapTiles.TryGetValue(path[path.Count - 1], out tile);

                if (tile)
                {
                    if (!(path[1].X == 0 && path[1].Z == 0))
                    {

                        tile.enemy = this;
                        myTile.myTileSprite.color = TilesManager.instance.classicColor;
                        myTile.enemy = null;
                        myTile = tile;
                        myTile.myTileSprite.color = myTileColor;
                        myAlienSprite.sortingOrder = myTile.coordinates.Y - myTile.coordinates.X;

                        isMoving = true;

                        return;
                        
                    }
                }
            }


            if (hero == hero1)
                hero = hero2;
            else
                hero = hero1;
            path = TilesManager.instance.GetPath(myTile.coordinates, hero.myTile.coordinates, stats.isFlying, false);
            if (path.Count > 1)
            {
                HexCell tile;
                TilesManager.instance.mapTiles.TryGetValue(path[path.Count - 1], out tile);
                if (tile)
                {
                    if (!(path[1].X == 0 && path[1].Z == 0))
                    {
                        tile.enemy = this;
                        myTile.myTileSprite.color = TilesManager.instance.classicColor;
                        myTile.enemy = null;
                        myTile = tile;
                        myTile.myTileSprite.color = myTileColor;
                        myAlienSprite.sortingOrder = myTile.coordinates.Y - myTile.coordinates.X;

                        isMoving = true;

                        return;

                    }
                }
            }
            
        }
        ContinueTurn();

    }

    private void ArriveOnCell()
    {
        if (myTile.item != null)
        {
            myTile.ActionItem(true);
        }

        PM -= 1;
        PA -= 1;
        isActionDone = true;

        ContinueTurn();
        return;

        
    }

    private void AttackCAC(HeroController hero)
    {

        if (PA >= stats.attacks[0].costPA)
        {
            if (stats.attacks[0].range <= 1)
            {
                hero.TakeDamages(stats.attacks[0].damages);  
            } else
            {
                List<HexCoordinates> path = new List<HexCoordinates>();
                path = TilesManager.instance.GetPath(myTile.coordinates, hero.myTile.coordinates, false, false);
                HexCell tileAimed;
                print(path[path.Count-1]);
                TilesManager.instance.mapTiles.TryGetValue(path[path.Count - 1], out tileAimed);
                foreach (HexCell tile in TilesManager.instance.GetRange(tileAimed.coordinates, stats.attacks[0].range-1, false, false))
                {
                    if(tile != myTile)
                    {
                        if (tile.hero)
                        {
                            tile.hero.TakeDamages(stats.attacks[0].damages);
                        } else if (tile.enemy)
                        {
                            tile.enemy.TakeDamages(stats.attacks[0].damages, "enemy", "melee");
                        }
                    } 
                }
            }
            PA -= stats.attacks[0].costPA;
            isActionDone = true;
            
        }

        ContinueTurn();
    }
    #endregion


    public void TakeDamages(int damages, string characterType, string attackSource)
    {
        health -= damages;
        health = Mathf.Clamp(health, 0, stats.health);

        GameObject txt = Instantiate(TXT_Damages, transform.position + new Vector3(0,50), transform.rotation);
        txt.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = damages.ToString();
        txt.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = damages.ToString();

        Destroy(txt, 1);


        if (health == 0)
        {
            Death(false, characterType, attackSource);
        }
    }

    public void SkipTurns(int turnsToSkip)
    {
        nbrTurnToSkip += turnsToSkip;
    }

    private void Death(bool isMyTurn, string characterType, string attackSource)
    {
        if(stats.Type == StatsEnemy.ENEMY_TYPE.KAMIKAZE && isMyTurn)
        {
            Explode();
            GetComponent<SpriteRenderer>().enabled = false;
            CombatSystem.instance.NextTurn();
        }

        if (characterType.Equals("cowboy"))
        {
            AchievementsManager.CgkImpif4cQQEAIQBA_temp_ok = true;

            AchievementsManager.CgkImpif4cQQEAIQCQ_temp_ok = false;
        }
        else if (characterType.Equals("soldier"))
        {
            AchievementsManager.CgkImpif4cQQEAIQCQ_temp_ok = false;
        }

        CombatSystem.instance.enemies.Remove(this);
        myTile.myTileSprite.color = TilesManager.instance.classicColor;
        myTile.enemy = null;
        Destroy(gameObject);
    }

    private void Explode()
    {
        foreach (HexCell tile in TilesManager.instance.GetRange(myTile.coordinates, stats.attacks[0].range, true, true))
        {
            if (tile != myTile)
            {
                if (tile.hero)
                {
                    tile.hero.TakeDamages(stats.attacks[0].damages);
                }
                else if (tile.enemy)
                {
                    tile.enemy.TakeDamages(stats.attacks[0].damages, "ennemy", "explosion");
                }
            }
        }
    }
}

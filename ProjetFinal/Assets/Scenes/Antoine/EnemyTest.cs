using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    public PlayerTest player;

    public HexCell myTile;

    private float cooldown = 2f;
    private float nextUpdate;


    public HexCell moveTo;

    public int range = 4;
    public int stayAway = 2;
    public bool canMoveAir = false;
    public bool canMoveWall = false;


    // Start is called before the first frame update
    void Start()
    {
        RaycastHit2D hitStart = Physics2D.Raycast(transform.position, Vector2.zero, Mathf.Infinity);
        if (hitStart)
        {
            if (hitStart.transform.GetComponent<HexCell>())
            {
                myTile = hitStart.transform.GetComponent<HexCell>();
            }
        }
    }

    private void Update()
    {

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {

            Vector3 touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

            Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
            RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);
            if (hitInformation)
            {

                if (hitInformation.transform.GetComponent<HexCell>() != null)
                {
                    foreach (var item in TilesManager.instance.mapTiles.Values)
                    {
                        if (item.tileType != HexCell.TILE_TYPE.WALL)
                            item.transform.GetComponent<SpriteRenderer>().color = Color.white;
                    }

                    myTile = hitInformation.transform.GetComponent<HexCell>();

                    gameObject.transform.position = myTile.gameObject.transform.position;

                    TakeOver();
                }
            }
        }

        if(Time.time > nextUpdate)
        {
            Debug.Log("ok");
            TakeOver();
            nextUpdate = Time.time + cooldown;

        }
        
    }


    private void TakeOver()
    {

        // If player already in line of sight 
        List<List<HexCell>> diagonals = TilesManager.instance.GetDiagonals(myTile.coordinates, range, true, false, TilesManager.instance.GetDirection(myTile.coordinates, player.myTile.coordinates));
        bool inRange = false;
        foreach (var diagonal in diagonals)
        {
            foreach (var item in diagonal)
            {
                if (item.isHero)
                {
                    inRange = true;
                    break;
                }
            }
        }

        // Can attack
        // If far enough

        if (TilesManager.instance.HeuristicDistance(myTile.coordinates, player.myTile.coordinates) > stayAway)
        {
            if (inRange)
            {
                AIAttack();
            }
            else
            {
                AIMoveClosestDiag();
            }
        }
        // If too close
        else
        {
            AIMoveAway();
        }
    }


    private void AIAttack()
    {
        Debug.Log("attack");
        gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;
    }

    private void AIMoveClosestDiag(bool keepDistance = false)
    {
        // Try and get nearest diagonals
        int result = TilesManager.instance.GetClosestDiagonal(myTile.coordinates, player.myTile.coordinates, keepDistance);
        if (result > -1)
        {
            // If diagonals found, move to closest by one tile
            TilesManager.instance.mapTiles.TryGetValue(TilesManager.instance.GetNeighboor(myTile.coordinates, result), out moveTo);

            if (TilesManager.instance.HeuristicDistance(moveTo.coordinates, player.myTile.coordinates) > stayAway)
            {
                // If can move to tile
                if (!moveTo.tileType.Equals(HexCell.TILE_TYPE.WALL) && !moveTo.tileType.Equals(HexCell.TILE_TYPE.HOLE))
                {
                    AIMoveToTarget();
                }
                else
                {
                    // If tile not ok, try get path to target
                    List<HexCoordinates> path = TilesManager.instance.GetPath(myTile.coordinates, player.myTile.coordinates, false, false);

                    foreach (var item in path)
                    {
                        Debug.Log(item);
                    }

                    // If path found
                    if (path.Count > 1)
                    {
                        TilesManager.instance.mapTiles.TryGetValue(path[path.Count - 1], out moveTo);

                        if (moveTo)
                        {
                            AIMoveToTarget();
                        }
                    }
                    else
                    {
                        Debug.LogWarning("No path to target");
                    }
                }
            }
            else
            {
                AIMoveClosestDiag(true);
            }

        }
        else
        {
            // If no diagonal found, try get path to player
            List<HexCoordinates> path = TilesManager.instance.GetPath(myTile.coordinates, player.myTile.coordinates, false, false);

            if (path.Count > 1)
            {
                TilesManager.instance.mapTiles.TryGetValue(path[path.Count - 1], out moveTo);


                if (moveTo)
                {
                    AIMoveToTarget();

                    Debug.LogWarning("2");

                }
            }
            else
            {
                Debug.LogWarning("No path to target 2");
            }
        }
    }
    private void AIMoveAway()
    {
        int direct = TilesManager.instance.GetDirection(player.myTile.coordinates, myTile.coordinates);

        List<HexCell> tiles = TilesManager.instance.GetImpactArc(myTile.coordinates, TilesManager.instance.GetNeighboor(myTile.coordinates, direct), false, false);
        moveTo = tiles[0];
        bool canMove = false;
        foreach (var item in tiles)
        {
            // Can move here
            if ((item.tileType.Equals(HexCell.TILE_TYPE.GROUND) || (item.tileType.Equals(HexCell.TILE_TYPE.WALL) && canMoveWall) || (item.tileType.Equals(HexCell.TILE_TYPE.HOLE) && canMoveAir)) && !item.isHero)
            {
                moveTo = item;
                canMove = true;
                break;
            }
        }

        if (canMove)
        {
            gameObject.transform.position = moveTo.transform.position;
        }
        else
        {
            tiles = TilesManager.instance.GetRadius(myTile.coordinates, 1, canMoveAir, canMoveWall);
            foreach (var item in tiles)
            {
                // Can move here
                if ((item.tileType.Equals(HexCell.TILE_TYPE.GROUND) || (item.tileType.Equals(HexCell.TILE_TYPE.WALL) && canMoveWall) || (item.tileType.Equals(HexCell.TILE_TYPE.HOLE) && canMoveAir)) && !item.isHero)
                {
                    moveTo = item;
                    canMove = true;
                    break;

                }
            }
        }

        if (canMove)
        {
            AIMoveTile();
        }
        else
        {
            Debug.Log("couldn't find path out of here");
        }
    }
    private void AIMoveToTarget()
    {
        AIMoveTile();
    }

    private void AIMoveTile()
    {
        gameObject.transform.position = moveTo.transform.position;
        myTile = moveTo;
    }
}

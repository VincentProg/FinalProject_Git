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

/*
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
            TakeOver();
            nextUpdate = Time.time + cooldown;

        }
        
    }


    private void TakeOver()
    {

        // If player already in line of sight 
        List<List<HexCell>> diagonals = TilesManager.instance.GetDiagonals(myTile.coordinates, range, true, false, TilesManager.instance.GetDirection(myTile.coordinates, player.myTile.coordinates));
        List<List<HexCell>> fov = TilesManager.instance.GetFOV(myTile, TilesManager.instance.HeuristicDistance(myTile.coordinates, player.myTile.coordinates), true);

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
        if(TilesManager.instance.HeuristicDistance(myTile.coordinates, player.myTile.coordinates) <= stayAway && fov[0].Contains(player.myTile))
        {
            AIMoveAway();
            Debug.Log("move away");

        }
        else
        {
            if (inRange)
            {
                AIAttack();
            }
            else
            {
                AIMoveClosestDiag(fov[0]);
            }
        }
    }


    private void AIAttack()
    {
        Debug.Log("attack");
        gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;
    }

    private void AIMoveClosestDiag(List<HexCell> fov)
    {
        // Try and get nearest diagonals
        moveTo = TilesManager.instance.GetClosestDiagonal(myTile.coordinates, player.myTile.coordinates, stayAway);
        if (moveTo)
        {
            if (fov.Contains(moveTo))
            {
                int direction = TilesManager.instance.GetDirection(myTile.coordinates, moveTo.coordinates);
                HexCoordinates newCoords = TilesManager.instance.GetNeighboor(myTile.coordinates, direction);
                TilesManager.instance.mapTiles.TryGetValue(newCoords, out moveTo);
                if (moveTo)
                    AIMoveToTarget();
            } 
            else
            {
                List<HexCoordinates> newCoords = TilesManager.instance.GetPath(myTile.coordinates, moveTo.coordinates, false, false);
                TilesManager.instance.mapTiles.TryGetValue(newCoords[newCoords.Count - 1], out moveTo);
                if (moveTo)
                    AIMoveToTarget();
            }           

        }
        else
        {
            List<HexCoordinates> newCoords = TilesManager.instance.GetPath(myTile.coordinates, player.myTile.coordinates, false, false);
            if(newCoords.Count > 2)
            {
                TilesManager.instance.mapTiles.TryGetValue(newCoords[newCoords.Count - 1], out moveTo);

                if (moveTo)
                {
                    if (TilesManager.instance.HeuristicDistance(myTile.coordinates, player.myTile.coordinates) <= stayAway && fov.Contains(player.myTile))
                    {
                        AIMoveAway();
                    }
                    else
                    {
                        AIMoveToTarget();
                    }
                }
            }

            Debug.LogWarning("No path to target 2");
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
            if ((item.tileType.Equals(HexCell.TILE_TYPE.GROUND) || (item.tileType.Equals(HexCell.TILE_TYPE.WALL) && canMoveWall) || (item.tileType.Equals(HexCell.TILE_TYPE.HOLE) && canMoveAir)) && !item.isPossessed())
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
                if ((item.tileType.Equals(HexCell.TILE_TYPE.GROUND) || (item.tileType.Equals(HexCell.TILE_TYPE.WALL) && canMoveWall) || (item.tileType.Equals(HexCell.TILE_TYPE.HOLE) && canMoveAir)) && !item.isPossessed())
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
    }*/
}

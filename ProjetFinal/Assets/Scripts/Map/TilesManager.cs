using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TilesManager : MonoBehaviour
{
    public static TilesManager instance;
    public Dictionary<HexCoordinates, HexCell> mapTiles = new Dictionary<HexCoordinates, HexCell>();
    [HideInInspector]
    public int numberOfTiles = 0;

    // List of chained positions around 0,0 (starting with bottom left and going clockwise)
    public List<HexCoordinates> tilesArround = new List<HexCoordinates>() { new HexCoordinates(0, -1), new HexCoordinates(-1, 0), new HexCoordinates(-1, 1), new HexCoordinates(0, 1), new HexCoordinates(1, 0), new HexCoordinates(1, -1) };

    public List<HexCell> _selectedTiles = new List<HexCell>();

    public HexCoordinates target;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);
    }

    public void ClearTiles(bool keepRangeSelected)
    {
        
        if (keepRangeSelected)
        {
            List<HexCell> cellsToRemove = new List<HexCell>();
            foreach (HexCell tile in _selectedTiles)
            {
                if (tile.selectionType == HexCell.SELECTION_TYPE.AIM_IMPACT || tile.selectionType == HexCell.SELECTION_TYPE.ORIGIN_IMPACT)
                {
                    tile.SelectCell(HexCell.SELECTION_TYPE.AIM);
                } else if(tile.selectionType == HexCell.SELECTION_TYPE.DISABLED_AIM || tile.selectionType == HexCell.SELECTION_TYPE.DISABLED_AIMIMPACT || tile.selectionType == HexCell.SELECTION_TYPE.DISABLEDAIM_IMPACT)
                {
                    tile.SelectCell(HexCell.SELECTION_TYPE.DISABLED_AIM);
                }
                else if(tile.selectionType != HexCell.SELECTION_TYPE.AIM)
                {
                    tile.UnselectCell();
                    cellsToRemove.Add(tile);
                }
                
            }
            for(int i = 0; i < cellsToRemove.Count; i++)
            {
                _selectedTiles.Remove(cellsToRemove[i]);
            }
        }
        else
        {
            foreach (HexCell tile in _selectedTiles)
            {
                tile.UnselectCell();
            }
            _selectedTiles.Clear();
        }
                

        
        
    }


    public List<HexCell> GetRadius(HexCoordinates center, int radius, bool passHole, bool passWall, bool passHero, bool passEnemy)
    {
        List<HexCell> results = new List<HexCell>();
        HexCell temp;

        center = new HexCoordinates(center.X + radius, center.Z);

        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < radius; j++)
            {
                HexCoordinates testCoords = GetNeighboor(center, i);

                mapTiles.TryGetValue(testCoords, out temp);

                if (temp)
                {
                    bool canPass = true;

                    if (!passHole)
                        if (temp.tileType.Equals(HexCell.TILE_TYPE.HOLE))
                            canPass = false;

                    if (!passWall)
                        if (temp.tileType.Equals(HexCell.TILE_TYPE.WALL))
                            canPass = false;
                    if (!passHero)
                        if (temp.hero != null)
                            canPass = false;
                    if (!passEnemy)
                        if (temp.enemy != null)
                            canPass = false;

                    if (canPass)
                    {
                        //temp.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                        //_coloredTiles.Add(temp.gameObject);
                        results.Add(temp);
                    }
                }
                center = testCoords;
            }
        }
        return results;
    }


    public List<HexCell> GetRange(HexCoordinates center, int radius, bool passHole, bool passWall)
    {
        List<HexCell> results = new List<HexCell>();
        HexCell temp;

        // Add player tile to result
        mapTiles.TryGetValue(center, out temp);
        results.Add(temp);


        for (int j = 1; j <= radius; j++)
        {
            HexCoordinates tempCenter = center;

            tempCenter = new HexCoordinates(tempCenter.X + j , tempCenter.Z);

            for (int i = 0; i < 6; i++)
            {
                for (int k = 0; k < j; k++)
                {
                    HexCoordinates testCoords = GetNeighboor(tempCenter, i);

                    mapTiles.TryGetValue(testCoords, out temp);
                    if (temp)
                    {
                        bool canPass = true;

                        if (!passHole)
                            if (temp.tileType.Equals(HexCell.TILE_TYPE.HOLE))
                                canPass = false;

                        if (!passWall)
                            if (temp.tileType.Equals(HexCell.TILE_TYPE.WALL))
                                canPass = false;

                        if (canPass)
                            results.Add(temp);                        
                    }
                    tempCenter = testCoords;
                }
            }

        }
        
        return results;
    }
    public List<HexCell> GetRangeInRadius(HexCoordinates center, int minRadius, int maxRadius, bool passHole, bool passWall, bool passHero, bool passEnemy)
    {
        List<HexCell> result = new List<HexCell>();

        if (minRadius == 0)
        {
            HexCell centerTile;
            mapTiles.TryGetValue(center, out centerTile);
            result.Add(centerTile);
        }

        for (int i = minRadius; i <= maxRadius; i++)
        {
            result.AddRange(GetRadius(center, i, passHole, passWall, passHero, passEnemy));
        }

        return result;
    }



    public List<HexCell> GetDiagonals(HexCoordinates center, int radius, bool passHole, bool passWall)
    {
        List<HexCell> results = new List<HexCell>();

        for (int i = 0; i < 6; i++)
        {
            HexCoordinates tempCenter = center;

            for (int j = 0; j < radius; j++)
            {
                HexCell temp;

                HexCoordinates testCoords = GetNeighboor(tempCenter, i);


                mapTiles.TryGetValue(testCoords, out temp);
                if (temp)
                {
                    if (!passHole)
                        if (temp.tileType.Equals(HexCell.TILE_TYPE.HOLE))
                            break;

                    if (!passWall)
                        if (temp.tileType.Equals(HexCell.TILE_TYPE.WALL))
                            break;

                    results.Add(temp);
                }
                tempCenter = testCoords;

            }
        }
        return results;
    }

    public List<HexCell> GetImpactArc(HexCoordinates center, HexCoordinates target, bool passHole, bool passWall)
    {
        List<HexCell> results = new List<HexCell>();
        int direction = GetDirection(center, target);

        if (direction > -1)
        {
            int newDirection = direction;

            HexCell temp;
            mapTiles.TryGetValue(target, out temp);
            if (temp.gameObject)
                results.Add(temp);

            newDirection = direction - 2;

            if (newDirection < 0)
                newDirection += 6;

            mapTiles.TryGetValue(GetNeighboor(target, newDirection), out temp);
            if (temp.gameObject)
            {
                bool canPass = true;

                if (!passHole)
                    if (temp.tileType.Equals(HexCell.TILE_TYPE.HOLE))
                        canPass = false;

                if (!passWall)
                    if (temp.tileType.Equals(HexCell.TILE_TYPE.WALL))
                        canPass = false;

                if (canPass)
                    results.Add(temp);
            }


            newDirection = direction + 2;


            if (newDirection > 5)
                newDirection -= 6;

            mapTiles.TryGetValue(GetNeighboor(target, newDirection), out temp);

            if (temp.gameObject)
            {
                bool canPass = true;

                if (!passHole)
                    if (temp.tileType.Equals(HexCell.TILE_TYPE.HOLE))
                        canPass = false;

                if (!passWall)
                    if (temp.tileType.Equals(HexCell.TILE_TYPE.WALL))
                        canPass = false;

                if (canPass)
                    results.Add(temp);
            }

        }
        return results;
    }


    public List<HexCell> GetImpactLine(HexCoordinates center, HexCoordinates target, int radius, bool passHole, bool passWall)
    {
        List<HexCell> results = new List<HexCell>();
        int direction = GetDirection(center, target);
        
        HexCoordinates lastTile = target;

        if (direction > -1)
        {
            HexCell temp;

            mapTiles.TryGetValue(target, out temp); 
            if(temp)
                results.Add(temp);

            for (int i = 0; i < radius - 1; i++)
            {
                lastTile = GetNeighboor(lastTile, direction);
                mapTiles.TryGetValue(lastTile, out temp);
                if (temp)
                {
                    if (!passHole)
                        if (temp.tileType.Equals(HexCell.TILE_TYPE.HOLE))
                            break;

                    if (!passWall)
                        if (temp.tileType.Equals(HexCell.TILE_TYPE.WALL))
                            break;

                    results.Add(temp);
                }
            }

        }
        return results;
    }

    public List<HexCoordinates> GetPath(HexCoordinates center, HexCoordinates target, bool passHole, bool passWall, int treshold = 500)
    {
        this.target = target;

        List<HexCoordinates> path = new List<HexCoordinates>();
        PriorityQueue<HexCoordinates, float> frontier = new PriorityQueue<HexCoordinates, float>();
        Dictionary<HexCoordinates, HexCoordinates> cameFrom = new Dictionary<HexCoordinates, HexCoordinates>();
        Dictionary<HexCoordinates, int> costSoFar = new Dictionary<HexCoordinates, int>();
        
        frontier.Enqueue(center, 0f);
        cameFrom.Add(center, center);
        costSoFar.Add(center, 0);

        HexCoordinates current;
        
        int failsafe = 0;

        while (frontier.Count > 0 && failsafe < treshold)
        {
            current = frontier.Dequeue();

            List<HexCoordinates> neighbors = new List<HexCoordinates>();
            HexCell temp;



            // Get all neighbors of current
            foreach (HexCell item in GetRadius(current, 1, passHole, passWall, true, true))
            {
                neighbors.Add(item.GetComponent<HexCell>().coordinates);
            }

            // Check neighbors
            foreach (HexCoordinates next in neighbors)
            {
                mapTiles.TryGetValue(next, out temp);

                // If current tile coords equal target coords
                if (next.Equals(target))
                {
                    mapTiles.TryGetValue(next, out temp);

                    if (temp)
                    {
                        temp.gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;
                        _selectedTiles.Add(temp);
                        cameFrom.Add(next, current);
                        frontier.Clear();
                        break;
                    }
                }


                if (temp)
                {
                    bool canPass = true;

                    if (temp.isPossessed())
                    {
                        canPass = false;
                    }
                    else
                    {

                        if (!passHole)
                            if (temp.tileType.Equals(HexCell.TILE_TYPE.HOLE))
                                canPass = false;

                            else if (!passWall)
                                if (temp.tileType.Equals(HexCell.TILE_TYPE.WALL))
                                    canPass = false;
                    }

                    if (canPass)
                    {
                        int newCost = costSoFar[current] + temp.movementCost;
                        if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                        {
                            temp.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                            _selectedTiles.Add(temp);

                            //frontier.Enqueue(next, HeuristicDistance(target, next));
                            float priority = newCost + HeuristicDistance(target, next);
                            frontier.Enqueue(next, priority);

                            if (costSoFar.ContainsKey(next))
                                costSoFar.Remove(next);
                            if (cameFrom.ContainsKey(next))
                                cameFrom.Remove(next);

                            costSoFar.Add(next, newCost);
                            cameFrom.Add(next, current);

                        }
                        else
                        {
                            temp.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                            _selectedTiles.Add(temp);
                            
                        }
                    }
                }
            }
            failsafe++;
        }

        current = target;

        while(!current.Equals(center) && failsafe < treshold)
        {
            path.Add(current);


            HexCell temp;
            mapTiles.TryGetValue(current, out temp);

            if (temp)
            {
                temp.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                _selectedTiles.Add(temp);
            }

            HexCoordinates tempCoord;
            cameFrom.TryGetValue(current, out tempCoord);
            current = tempCoord;

            failsafe++;
        }

        return path;
    }

    public List<List<HexCell>> GetFOV(HexCell centerCell, int radius, bool isBlockedByObjects)
    {
        List<HexCell> range = this.GetRange(centerCell.coordinates, radius, true, true);

        List<HexCell> inReach = new List<HexCell>();
        List<HexCell> notAccessible = new List<HexCell>();

        for (int i = 0; i < range.Count; i++)
        {
            if (!range[i].tileType.Equals(HexCell.TILE_TYPE.WALL))
            {
                Vector2 direction = new Vector2(range[i].gameObject.transform.position.x - centerCell.gameObject.transform.position.x, range[i].gameObject.transform.position.y - centerCell.gameObject.transform.position.y);
                float distance = direction.magnitude;

                RaycastHit2D[] hits = Physics2D.CircleCastAll(centerCell.gameObject.transform.position, .15f, direction, distance);

                if (hits.Length > 0)
                {
                    bool blocked = false;
                    foreach (var item in hits)
                    {
                        
                        if (item.transform.gameObject.GetComponent<HexCell>())
                        {
                            HexCell tileScript = item.transform.gameObject.GetComponent<HexCell>();
                            if (isBlockedByObjects)
                            {
                                if ((tileScript != centerCell && tileScript != range[i] && tileScript.isPossessed()) || tileScript.tileType.Equals(HexCell.TILE_TYPE.WALL))
                                {
                                    notAccessible.Add(range[i]);
                                    blocked = true;
                                    break;
                                }
                            } else
                            {
                                if (tileScript.tileType.Equals(HexCell.TILE_TYPE.WALL))
                                {
                                    notAccessible.Add(range[i]);
                                    blocked = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (!blocked)
                        inReach.Add(range[i]);
                }
            }
        }

        List<List<HexCell>> result = new List<List<HexCell>>();
        result.Add(inReach);
        result.Add(notAccessible);

        return result;
    }

    // HexCoordinates calculus methods
    private HexCoordinates AddCoords(HexCoordinates coords1, HexCoordinates coords2)
    {
        return new HexCoordinates(coords1.X + coords2.X, coords1.Z + coords2.Z);
    }
    private HexCoordinates MultiplyCoord(HexCoordinates coords, int offset)
    {
        return new HexCoordinates(coords.X * offset, coords.Z * offset);
    }

    private HexCoordinates AddIntToCoord(HexCoordinates coords, int offset)
    {
        return new HexCoordinates(coords.X + offset, coords.Z + offset);
    }

    private HexCoordinates GetNeighboor(HexCoordinates coords, int direction)
    {
        return new HexCoordinates(coords.X + tilesArround[direction].X, coords.Z + tilesArround[direction].Z);
    }

    public int HeuristicDistance(HexCoordinates coords1, HexCoordinates coords2)
    {
        return Mathf.Max(Mathf.Abs(coords1.X - coords2.X), Mathf.Abs(coords1.Y - coords2.Y), Mathf.Abs(coords1.Z - coords2.Z));
    }

    public int GetDirection(HexCoordinates center, HexCoordinates target)
    {
        float X = target.X - center.X;
        float Z = target.Z - center.Z;

        int direction = -1;

        Vector2 normalized = new Vector2(X, Z).normalized;

        var tolerance = 0.01;
        if (normalized.x == 0f && normalized.y == -1f)
        {
            direction = 0;
        }
        else if (normalized.x == -1f && normalized.y == 0f)
        {
            direction = 1;
        }
        else if (Mathf.Abs(normalized.x - (-.7f)) < tolerance && Mathf.Abs(normalized.y - .7f) < tolerance)
        {
            direction = 2;
        }
        else if (normalized.x == 0f && normalized.y == 1f)
        {
            direction = 3;
        }
        else if (normalized.x == 1f && normalized.y == 0f)
        {
            direction = 4;
        }
        else if (Mathf.Abs(normalized.x - .7f) < tolerance && Mathf.Abs(normalized.y - (-.7f)) < tolerance)
        {
            direction = 5;
        }

        return direction;
    }
}

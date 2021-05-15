using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TilesManager : MonoBehaviour
{
    public static TilesManager instance;
    public Dictionary<HexCoordinates, HexCell> mapTiles = new Dictionary<HexCoordinates, HexCell>();
    [HideInInspector]
    public int numberOfTiles = 0;

    public Color baseColor;
    public Color lightingColor;

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
                    tile.ModifySelection(HexCell.SELECTION_TYPE.AIM);
                } else if(tile.selectionType != HexCell.SELECTION_TYPE.AIM)
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


    public List<HexCell> GetRadius(HexCoordinates center, int radius = 1)
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
                if (temp && temp.canMoveHere)
                {
                    //temp.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                    //_coloredTiles.Add(temp.gameObject);
                    results.Add(temp);
                    temp.isSelected = true;
                }
                center = testCoords;
            }
        }
        return results;
    }


    public List<HexCell> GetRange(HexCoordinates center, int radius = 1)
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
                    if (temp && temp.canMoveHere)
                    {
                        results.Add(temp);
                    }
                    tempCenter = testCoords;
                }
            }

        }
        
        return results;
    }
    public List<List<HexCell>> GetMinMaxRange(HexCoordinates center, int minRadius, int maxRadius)
    {
 
        List<HexCell> minRange = GetRange(center, minRadius);
        List<HexCell> maxRange = new List<HexCell>();
        List<List<HexCell>> results = new List<List<HexCell>>();

        if (minRadius == 0)
        {
            HexCell centerTile;
            mapTiles.TryGetValue(center, out centerTile);
            maxRange.Add(centerTile);
        }

        for (int i = minRadius; i <= maxRadius; i++)
        {
            maxRange.AddRange(GetRadius(center, i));
        }

        results.Add(minRange);
        results.Add(maxRange);


        // results[0] = range interdite
        // results[1] = range accept�e

        return results;
    }
    
    public List<HexCell> GetDiagonals(HexCoordinates center, int radius = 1)
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
                if (temp && temp.canMoveHere)
                {
                    results.Add(temp);
                }
                tempCenter = testCoords;

            }
        }
        return results;
    }


    public List<HexCoordinates> GetPath(HexCoordinates center, HexCoordinates target, int treshold)
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
            foreach (HexCell item in GetRadius(current, 1))
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
                        Debug.Log("found");
                        frontier.Clear();
                        break;
                    }
                }


                if (temp)
                {
                    if (temp.canMoveHere)
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
        return Mathf.Abs(coords1.X - coords2.X) + Mathf.Abs(coords1.Z - coords2.Z);
    }
}

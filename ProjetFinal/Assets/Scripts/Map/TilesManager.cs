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

    private List<GameObject> _coloredTiles = new List<GameObject>();


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);
    }

    public void ClearTiles()
    {
        foreach(GameObject tile in _coloredTiles)
        {
            tile.GetComponent<SpriteRenderer>().color = Color.white;
        }
        _coloredTiles.Clear();
    }


    public List<GameObject> GetRadius(HexCoordinates center, int radius = 1)
    {
        List<GameObject> results = new List<GameObject>();

        center = new HexCoordinates(center.X + radius, center.Z);

        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < radius; j++)
            {
                HexCell temp;
                HexCoordinates testCoords = GetNeighboor(center, i);

                mapTiles.TryGetValue(testCoords, out temp);
                if (temp)
                {
                    temp.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                    _coloredTiles.Add(temp.gameObject);
                    results.Add(temp.gameObject);
                }
                center = testCoords;
            }
        }
        return results;
    }


    public List<GameObject> GetRange(HexCoordinates center, int radius = 1)
    {
        List<GameObject> results = new List<GameObject>();
        radius -= 1;

        for (int j = 1; j <= radius; j++)
        {
            HexCoordinates tempCenter = center;

            tempCenter = new HexCoordinates(tempCenter.X + j , tempCenter.Z);

            for (int i = 0; i < 6; i++)
            {
                for (int k = 0; k < j; k++)
                {
                    HexCell temp;
                    HexCoordinates testCoords = GetNeighboor(tempCenter, i);

                    mapTiles.TryGetValue(testCoords, out temp);
                    if (temp)
                    {
                        temp.gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
                        _coloredTiles.Add(temp.gameObject);
                        results.Add(temp.gameObject);
                    }
                    tempCenter = testCoords;
                }
            }

        }
        
        return results;
    }


    public List<GameObject> GetDiagonals(HexCoordinates center, int radius = 1)
    {
        List<GameObject> results = new List<GameObject>();


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
                    temp.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                    _coloredTiles.Add(temp.gameObject);
                    results.Add(temp.gameObject);
                }
                tempCenter = testCoords;

            }
        }
        return results;
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
}

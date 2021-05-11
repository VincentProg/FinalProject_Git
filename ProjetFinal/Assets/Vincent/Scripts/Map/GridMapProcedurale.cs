using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMapProcedurale : MonoBehaviour
{
    Dictionary<Vector2, Tile> grid = new Dictionary<Vector2, Tile>();

    public int numberOfTiles = 0;
    public int x;
    public int y;

    public GameObject tile;
    private void Start()
    {
        
        for(int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                Vector2 newPos;
                if (j % 2 == 0)
                {
                   newPos  = new Vector2(i, j * 0.85f);     
                } else
                {
                   newPos = new Vector2(i + 0.5f, j * 0.85f);
                }

                GameObject newTile = Instantiate(tile, newPos, transform.rotation);
                newTile.transform.parent = transform;
                Tile scriptTile = newTile.GetComponent<Tile>();
                scriptTile.id = numberOfTiles;
                numberOfTiles++;
                scriptTile.position = newPos;
                grid.Add(newPos, scriptTile);
            }
        }

    }
}

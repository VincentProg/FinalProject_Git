using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap : MonoBehaviour
{
    public static GridMap instance;
    public Dictionary<Vector2, Tile> grid = new Dictionary<Vector2, Tile>();
    public int numberOfTiles = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);
    }

}

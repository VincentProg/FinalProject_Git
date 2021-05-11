using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap : MonoBehaviour
{
    public static GridMap instance;
    public Dictionary<Vector2, Tile> mapTiles = new Dictionary<Vector2, Tile>();
    public int numberOfTiles = 0;

    public Color baseColor;
    public Color lightingColor;

    public List<Vector2> tilesArround = new List<Vector2>() { new Vector2(0.0f, 1.0f), new Vector2(0.870f, 0.50f), new Vector2(0.870f, -0.50f), new Vector2(0.0f, -1.0f), new Vector2(-0.870f, -0.50f), new Vector2(-0.870f, 0.50f) };


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);
    }

}

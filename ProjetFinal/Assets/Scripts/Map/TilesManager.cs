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

    public List<HexCoordinates> tilesArround = new List<HexCoordinates>() { new HexCoordinates(-1, 2), new HexCoordinates(0, 1), new HexCoordinates(1, -1), new HexCoordinates(1, -2), new HexCoordinates(0, -1), new HexCoordinates(-1, 1) };


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGrid : MonoBehaviour
{
    public static Dictionary<Vector2, GameObject> mapTiles = new Dictionary<Vector2, GameObject>();

    public Color baseColor;
    public static Color staticBaseColor;


    public GameObject tileContainer;

    // Start is called before the first frame update
    void Awake()
    {
        staticBaseColor = baseColor;
        // Get all tiles on start
        for (int i = 0; i < tileContainer.transform.childCount; i++)
        {
            mapTiles.Add(tileContainer.transform.GetChild(i).transform.position, tileContainer.transform.GetChild(i).gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2 position;
    public int id;


    void Start()
    {
        position = transform.position;
        id = GridMap.instance.numberOfTiles;
        GridMap.instance.numberOfTiles++;
        GridMap.instance.grid.Add(position, this);
    }
}

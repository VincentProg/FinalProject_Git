using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2 position;
    public int id;
    public bool canMoveHere;

    public Player player;

    void Start()
    {
        position = transform.position;
        GridMap.instance.mapTiles.Add(position, this);

    }
}

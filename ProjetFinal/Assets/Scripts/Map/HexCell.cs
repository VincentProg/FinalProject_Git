﻿using UnityEngine;

public class HexCell : MonoBehaviour {

	public HexCoordinates coordinates;
	public Color color;
	public int movementCost = 1;

	public bool canMoveHere = true;

	public HeroController hero;
	public EnemyController enemy;

	public bool isSelected;

	void Awake()
	{
		TilesManager.instance.mapTiles.Add(coordinates, this);
        if (!canMoveHere)
        {
			GetComponent<SpriteRenderer>().color = Color.gray;
        }
	}
}
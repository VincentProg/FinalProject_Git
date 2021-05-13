using UnityEngine;

public class HexCell : MonoBehaviour {

	public HexCoordinates coordinates;
	public Color color;

	public bool canMoveHere = true;

	public HeroController hero;
	public EnemyController enemy;

	public bool isSelected;

	void Awake()
	{
		TilesManager.instance.mapTiles.Add(coordinates, this);
	}
}
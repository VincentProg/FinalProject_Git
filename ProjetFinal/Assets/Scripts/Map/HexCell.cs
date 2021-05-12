using UnityEngine;

public class HexCell : MonoBehaviour {

	public HexCoordinates coordinates;
	public Color color;

	public bool canMoveHere = true;

	public Player player;

	void Start()
	{
		TilesManager.instance.mapTiles.Add(coordinates, this);
	}
}
using UnityEngine;
using System.Collections.Generic;

public class HexCell : MonoBehaviour {

	public HexCoordinates coordinates;
	public int movementCost = 1;

	public bool canMoveHere = true;

	[HideInInspector]
	public HeroController hero;
	[HideInInspector]
	public EnemyController enemy;

	[HideInInspector]
	public bool isSelected;
	public enum SELECTION_TYPE {NONE, MOVEMENT, AIM, IMPACT, AIM_IMPACT}
	[HideInInspector]
	public SELECTION_TYPE selectionType = SELECTION_TYPE.NONE;

	private SpriteRenderer sprite;
	[SerializeField]
	private List<Color> colors = new List<Color>();

	void Awake()
	{
		TilesManager.instance.mapTiles.Add(coordinates, this);
        if (!canMoveHere)
        {
			GetComponent<SpriteRenderer>().color = Color.gray;
        }

		sprite = GetComponent<SpriteRenderer>();
	}


	public void SelectCell( SELECTION_TYPE type )
    {
		isSelected = true;
		selectionType = type;

		switch (type)
        {
			case SELECTION_TYPE.MOVEMENT:
				sprite.color = colors[1];
				break;
			case SELECTION_TYPE.AIM:
				sprite.color = colors[2];
				break;
			case SELECTION_TYPE.IMPACT:
				sprite.color = colors[3];
				break;
			case SELECTION_TYPE.AIM_IMPACT:
				sprite.color = colors[4];
				break;

		}

		TilesManager.instance._selectedTiles.Add(this);

    }

	public void UnselectCell()
    {
		isSelected = false;
		selectionType = SELECTION_TYPE.NONE;
		sprite.color = colors[0];
	}
}
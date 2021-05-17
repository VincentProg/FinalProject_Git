using UnityEngine;
using System.Collections.Generic;

public class HexCell : MonoBehaviour {

	public HexCoordinates coordinates;
	public int movementCost = 1;


	[HideInInspector]
	public HeroController hero;
	[HideInInspector]
	public EnemyController enemy;
	[HideInInspector]
	public GameObject item;

	[HideInInspector]
	public bool isSelected;
	public enum SELECTION_TYPE {NONE, MOVEMENT, AIM, IMPACT, AIM_IMPACT, ORIGIN_AIM, ORIGIN_IMPACT}
	[HideInInspector]
	public SELECTION_TYPE selectionType = SELECTION_TYPE.NONE;

	private SpriteRenderer sprite;
	[SerializeField]
	private List<Color> colors = new List<Color>();

	public enum TILE_TYPE {GROUND, WALL, HOLE}
	public TILE_TYPE tileType;
	public List<Sprite> tileSprites;
	

	void Awake()
	{
		TilesManager.instance.mapTiles.Add(coordinates, this);
		sprite = GetComponent<SpriteRenderer>();

		if (tileType.Equals(TILE_TYPE.WALL))
			GetComponent<SpriteRenderer>().color = Color.red;
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
			case SELECTION_TYPE.ORIGIN_AIM:
				sprite.color = colors[5];
				break;
			case SELECTION_TYPE.ORIGIN_IMPACT:
				sprite.color = colors[6];
				break;

		}

		TilesManager.instance._selectedTiles.Add(this);

    }

	public void ModifySelection( SELECTION_TYPE type)
    {
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
			case SELECTION_TYPE.ORIGIN_AIM:
				sprite.color = colors[5];
				break;
			case SELECTION_TYPE.ORIGIN_IMPACT:
				sprite.color = colors[6];
				break;

		}
	}

	public void UnselectCell()
    {
		isSelected = false;
		selectionType = SELECTION_TYPE.NONE;
		sprite.color = colors[0];
	}

	public void UpdateTileDatas(TILE_TYPE type)
    {
		tileType = type;
		SpriteRenderer myTileSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
		switch (type)
        {
			case TILE_TYPE.GROUND:
				myTileSprite.sprite = tileSprites[1];
				break;

			case TILE_TYPE.WALL:
				myTileSprite.sprite = tileSprites[2];
				break;

			case TILE_TYPE.HOLE:
				myTileSprite.sprite = tileSprites[3];
				break;
        }
    }
}
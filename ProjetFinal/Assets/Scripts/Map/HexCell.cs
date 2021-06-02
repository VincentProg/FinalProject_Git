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
	private bool isSelected = false;
	public enum SELECTION_TYPE {NONE, MOVEMENT, AIM, IMPACT, AIM_IMPACT, ORIGIN_AIM, ORIGIN_IMPACT, DISABLED_AIM, DISABLED_IMPACT, DISABLED_AIMIMPACT, DISABLEDAIM_IMPACT}
	//[HideInInspector]
	public SELECTION_TYPE selectionType = SELECTION_TYPE.NONE;

	private SpriteRenderer sprite;
	[HideInInspector]
	public SpriteRenderer myTileSprite;
	[SerializeField]
	private List<Color> colors = new List<Color>();

	public enum TILE_TYPE {GROUND, WALL, HOLE}
	public TILE_TYPE tileType;
	[SerializeField]
	List<Color> myTileSpriteColors;

	private Animator anim;
	

	void Awake()
	{
		TilesManager.instance.mapTiles.Add(coordinates, this);
		sprite = GetComponent<SpriteRenderer>();
		myTileSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
		transform.GetChild(0).parent = transform.parent;
		anim = GetComponent<Animator>();

		GetComponent<SpriteRenderer>().sortingOrder = -100;

	}


	public void SelectCell( SELECTION_TYPE type )
    {
		if (!isSelected)
		{
			isSelected = true;
			TilesManager.instance._selectedTiles.Add(this);
		}
		
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
			case SELECTION_TYPE.DISABLED_AIM:
				sprite.color = colors[7];
				break;
			case SELECTION_TYPE.DISABLED_IMPACT:
				sprite.color = colors[8];
				break;
			case SELECTION_TYPE.DISABLED_AIMIMPACT:
				sprite.color = colors[8];
				break;
			case SELECTION_TYPE.DISABLEDAIM_IMPACT:
				sprite.color = colors[3];
				break;
		}

		GetComponent<SpriteRenderer>().sortingOrder = -coordinates.X;

		anim.SetTrigger("Appear");

    }


	public void UnselectCell()
    {
		isSelected = false;
		selectionType = SELECTION_TYPE.NONE;
		sprite.color = colors[0];
	}

	public void UpdateTileDatas(TILE_TYPE type)
    {
		if(myTileSprite == null)
        {
			myTileSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        }

		tileType = type;
		switch (type)
        {
			case TILE_TYPE.GROUND:
				myTileSprite.color = myTileSpriteColors[0];
				break;

			case TILE_TYPE.WALL:
				myTileSprite.color = myTileSpriteColors[1];
				break;

			case TILE_TYPE.HOLE:
				myTileSprite.color = myTileSpriteColors[2];
				break;
        }
    }

	public bool isPossessed()
	{
		if (hero != null || enemy != null)
		{

			return true;
		}
		else if (item != null)
		{
			if (item.GetComponent<Spawner>())
			{
				return true;
			}
		}
		return false;
	}


	public bool isHero()
	{
		if (hero)
			return true;
		return false;
	}


	public void ActionItem(bool isHero)
    {
        if (item.GetComponent<Mine>())
        {
			Mine mine = item.GetComponent<Mine>();
			if (isHero && !mine.isFriendlyFire) return;
			mine.Attack();
			AudioManager.instance.Play("dynamite");
			item = null;
        } else if (item.GetComponent<Spawner>())
        {
			//Destroy spawner
        }
    }

}
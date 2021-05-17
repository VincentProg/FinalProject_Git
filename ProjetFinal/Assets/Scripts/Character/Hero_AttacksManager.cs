using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_AttacksManager : MonoBehaviour
{

    public static Hero_AttacksManager instance;
    [HideInInspector]
    private UI_Attack UI_Caller;
    private Attack attack;

    private HexCell originTile;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);
    }

    public void ShowAttackRange(UI_Attack caller, Attack attackCalled)
    {
        TilesManager.instance.ClearTiles(false);
        UI_Caller = caller;
        attack = attackCalled;

        originTile = caller.hero.myTile;


        switch (attack.rangeType)
        {
            case Attack.RANGE_TYPE.OWNCELL:
                originTile.SelectCell(HexCell.SELECTION_TYPE.AIM);
                break;
            case Attack.RANGE_TYPE.LINE:
                foreach (HexCell tile in TilesManager.instance.GetDiagonals(originTile.coordinates, attack.rangeAttack, false, false))
                {
                    tile.SelectCell(HexCell.SELECTION_TYPE.AIM);
                }
                break;
            case Attack.RANGE_TYPE.RADIUS:
                foreach (HexCell tile in TilesManager.instance.GetRangeInRadius(originTile.coordinates, attack.radiusUnattackableAttack,attack.rangeAttack, false, false))
                {
                    tile.SelectCell(HexCell.SELECTION_TYPE.AIM);
                }
                break;
        }
    }

    public void ShowImpactRange(HexCell oTile)
    {
        TilesManager.instance.ClearTiles(true);
        originTile = oTile;

        switch (attack.impactType)
        {
            case Attack.IMPACT_TYPE.POINT:
                originTile.SelectCell(HexCell.SELECTION_TYPE.ORIGIN_IMPACT);
                break;
            case Attack.IMPACT_TYPE.LINES:
                originTile.SelectCell(HexCell.SELECTION_TYPE.ORIGIN_IMPACT);
                foreach (HexCell tile in TilesManager.instance.GetDiagonals(originTile.coordinates, attack.rangeImpact, true, true))
                {
                    if (tile.selectionType == HexCell.SELECTION_TYPE.AIM)
                    {
                        tile.SelectCell(HexCell.SELECTION_TYPE.AIM_IMPACT);
                    } else
                    {
                        tile.SelectCell(HexCell.SELECTION_TYPE.IMPACT);
                    }

                    if(tile == originTile)
                    {
                        tile.ModifySelection(HexCell.SELECTION_TYPE.ORIGIN_IMPACT);
                    }
                }
                break;
            case Attack.IMPACT_TYPE.ARC:
                break;
            case Attack.IMPACT_TYPE.RADIUS:
                foreach (HexCell tile in TilesManager.instance.GetRangeInRadius(originTile.coordinates, attack.radiusUnattackableImpact, attack.rangeImpact, false, false))
                {
                    if (tile.selectionType == HexCell.SELECTION_TYPE.AIM)
                    {
                        tile.SelectCell(HexCell.SELECTION_TYPE.AIM_IMPACT);
                    }
                    else
                    {
                        tile.SelectCell(HexCell.SELECTION_TYPE.IMPACT);
                    }

                    if (tile == originTile)
                    {
                        tile.ModifySelection(HexCell.SELECTION_TYPE.ORIGIN_IMPACT);
                    }
                }
                break;
            case Attack.IMPACT_TYPE.SPAWNOBJECT:
                originTile.SelectCell(HexCell.SELECTION_TYPE.AIM_IMPACT);
                break;
        }

        if (originTile.selectionType == HexCell.SELECTION_TYPE.AIM)
        {
            originTile.SelectCell(HexCell.SELECTION_TYPE.ORIGIN_AIM);
        }

    }

    public void LaunchAttack()
    {
        switch (attack.impactType)
        {
            case Attack.IMPACT_TYPE.SPAWNOBJECT:
                GameObject newObject = Instantiate(attack.spawnObject, originTile.transform.position, attack.spawnObject.transform.rotation);
                originTile.item = newObject;
                break;
            default:
                foreach(HexCell tile in TilesManager.instance._selectedTiles)
                {
                    if(tile.selectionType == HexCell.SELECTION_TYPE.IMPACT || tile.selectionType == HexCell.SELECTION_TYPE.ORIGIN_IMPACT)
                    {
                        if(tile.hero != null)
                        {
                            tile.hero.TakeDamages(attack.damages);
                        } else if(tile.enemy != null)
                        {
                            tile.enemy.TakeDamages(attack.damages);
                        }
                    }
                }
                break;
        }

        UI_Caller.ActivateAttack();

        TilesManager.instance.ClearTiles(false);

    }

}

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
        TilesManager.instance.ClearTiles();
        UI_Caller = caller;
        attack = attackCalled;

        originTile = caller.hero.myTile;

        switch (attack.rangeType)
        {
            case Attack.RANGE_TYPE.OWNCELL:
                originTile.SelectCell(HexCell.SELECTION_TYPE.AIM);
                break;
            case Attack.RANGE_TYPE.LINE:
                foreach (HexCell tile in TilesManager.instance.GetDiagonals(originTile.coordinates, attack.rangeAttack))
                {
                    tile.SelectCell(HexCell.SELECTION_TYPE.AIM);
                }
                break;
            case Attack.RANGE_TYPE.RADIUS:
                foreach (HexCell tile in TilesManager.instance.GetMinMaxRange(originTile.coordinates, attack.radiusUnattackableAttack,attack.rangeAttack)[1])
                {
                    tile.SelectCell(HexCell.SELECTION_TYPE.AIM);
                }
                break;
        }
    }

    public void ShowImpactRange(HexCell oTile)
    {

        originTile = oTile;


        switch (attack.rangeType)
        {
            case Attack.RANGE_TYPE.OWNCELL:
                originTile.SelectCell(HexCell.SELECTION_TYPE.AIM_IMPACT);
                break;
            case Attack.RANGE_TYPE.LINE:
                foreach (HexCell tile in TilesManager.instance.GetDiagonals(originTile.coordinates, attack.rangeAttack))
                {
                    if (tile.selectionType == HexCell.SELECTION_TYPE.AIM)
                    {
                        tile.SelectCell(HexCell.SELECTION_TYPE.AIM_IMPACT);
                    } else
                    {
                        tile.SelectCell(HexCell.SELECTION_TYPE.IMPACT);
                    }
                }
                break;
            case Attack.RANGE_TYPE.RADIUS:
                foreach (HexCell tile in TilesManager.instance.GetMinMaxRange(originTile.coordinates, attack.radiusUnattackableAttack, attack.rangeAttack)[1])
                {
                    if (tile.selectionType == HexCell.SELECTION_TYPE.AIM)
                    {
                        tile.SelectCell(HexCell.SELECTION_TYPE.AIM_IMPACT);
                    }
                    else
                    {
                        tile.SelectCell(HexCell.SELECTION_TYPE.IMPACT);
                    }
                }
                break;
        }

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_AttacksManager : MonoBehaviour
{

    public static Hero_AttacksManager instance;
    [HideInInspector]
    public UI_Attack UI_Caller;

    private HexCell originTile;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);
    }

    public void ShowAttackRange(UI_Attack caller, Attack attack)
    {
        print("SHOW ATTACK");
        TilesManager.instance.ClearTiles();
        UI_Caller = caller;

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

}

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
                TilesManager.instance.GetRange(originTile.coordinates, 1);
                break;
            case Attack.RANGE_TYPE.LINE:

                break;
            case Attack.RANGE_TYPE.RADIUS:

                break;
        }
    }

}

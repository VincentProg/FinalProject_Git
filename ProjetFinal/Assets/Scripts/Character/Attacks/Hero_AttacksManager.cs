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
    private HexCell playerTile;

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
        playerTile = originTile;

        List<List<HexCell>> TilesInRange = TilesManager.instance.GetFOV(originTile, attack.rangeAttack, attack.visionType == Attack.VISION_TYPE.WALLSHUMANS_BLOCK);

        switch (attack.rangeType)
        {
            case Attack.RANGE_TYPE.OWNCELL:
                originTile.SelectCell(HexCell.SELECTION_TYPE.AIM);
                break;
            case Attack.RANGE_TYPE.LINE:
                
                // SELECTION DES TILES EN FONCTION DE LEUR VISIBILITE
                if (attack.visionType == Attack.VISION_TYPE.SEE_EVERYTHING)
                {
                    foreach (List<HexCell> diagonal in TilesManager.instance.GetDiagonals(originTile.coordinates, attack.radiusUnattackableAttack,attack.rangeAttack, attack.canSelectHole, false, attack.canSelectHeroEnemySpawner))
                    {
                        foreach (var item in diagonal)
                        {
                            item.SelectCell(HexCell.SELECTION_TYPE.AIM);
                        }
                    }
                    print("showAttack");
                }
                else
                {
                    foreach (List<HexCell> diagonal in TilesManager.instance.GetDiagonals(originTile.coordinates, attack.radiusUnattackableAttack, attack.rangeAttack, attack.canSelectHole, false, attack.canSelectHeroEnemySpawner))
                    {
                        foreach (var tile in diagonal)
                        {
                            bool isInRange = false;
                            for (int i = 0; i < TilesInRange[0].Count; i++)
                            {
                                if (tile == TilesInRange[0][i])
                                {
                                    isInRange = true;
                                }
                            }

                            if (isInRange)
                            {
                                tile.SelectCell(HexCell.SELECTION_TYPE.AIM);

                            }
                            else
                            {
                                tile.SelectCell(HexCell.SELECTION_TYPE.DISABLED_AIM);
                            }
                        }
                    }
                }
                break;
            case Attack.RANGE_TYPE.RADIUS:

                // SELECTION DES TILES EN FONCTION DE LEUR VISIBILITE
                if(attack.impactType == Attack.IMPACT_TYPE.SPAWNOBJECT)
                {
                    foreach (HexCell tile in TilesManager.instance.GetRangeInRadius(originTile.coordinates, attack.radiusUnattackableAttack, attack.rangeAttack, false, false, false))
                    {
                        tile.SelectCell(HexCell.SELECTION_TYPE.AIM);
                    }return;
                }

                if (attack.visionType == Attack.VISION_TYPE.SEE_EVERYTHING)
                {
                    foreach (HexCell tile in TilesManager.instance.GetRangeInRadius(originTile.coordinates, attack.radiusUnattackableAttack, attack.rangeAttack, false, false, true))
                    {
                        tile.SelectCell(HexCell.SELECTION_TYPE.AIM);
                    }
                
                }
                else
                {

                    foreach (HexCell tile in TilesManager.instance.GetRangeInRadius(originTile.coordinates, attack.radiusUnattackableAttack, attack.rangeAttack, attack.canSelectHole, false, true))
                    {
                        bool isInRange = false;
                        for (int i = 0; i < TilesInRange[0].Count; i++)
                        {
                            if (tile == TilesInRange[0][i])
                            {
                                isInRange = true;
                            }
                        }

                        if (isInRange)
                        {
                            tile.SelectCell(HexCell.SELECTION_TYPE.AIM);
                        }
                        else
                        {
                            tile.SelectCell(HexCell.SELECTION_TYPE.DISABLED_AIM);
                        }
                    }
                }

                break;
        }
    }

    public void ShowImpactRange(HexCell oTile)
    {
        TilesManager.instance.ClearTiles(true);
        originTile = oTile;

        List<List<HexCell>> TilesInRange = TilesManager.instance.GetFOV(originTile, attack.rangeImpact, false);

        switch (attack.impactType)
        {
            case Attack.IMPACT_TYPE.POINT:
                originTile.SelectCell(HexCell.SELECTION_TYPE.ORIGIN_IMPACT);
                break;
            case Attack.IMPACT_TYPE.LINES:
                originTile.SelectCell(HexCell.SELECTION_TYPE.ORIGIN_IMPACT);
                // SELECTION DES TILES EN FONCTION DE LEUR VISIBILITE

                foreach (List<HexCell> diagonal in TilesManager.instance.GetDiagonals(originTile.coordinates, attack.radiusUnattackableImpact,attack.rangeImpact,true, true, true))
                {
                    foreach (var tile in diagonal)
                    {
                        bool isInRange = false;
                        for (int i = 0; i < TilesInRange[0].Count; i++)
                        {
                            if (tile == TilesInRange[0][i])
                            {
                                isInRange = true;
                            }
                        }

                        if (isInRange)
                        {
                            if (tile.selectionType == HexCell.SELECTION_TYPE.AIM)
                            {
                                tile.SelectCell(HexCell.SELECTION_TYPE.AIM_IMPACT);
                            }
                            else
                            {
                                if (tile.selectionType == HexCell.SELECTION_TYPE.DISABLED_AIM)
                                    tile.SelectCell(HexCell.SELECTION_TYPE.DISABLEDAIM_IMPACT);
                                else tile.SelectCell(HexCell.SELECTION_TYPE.IMPACT);

                            }
                        }

                        if (isInRange)
                        {
                            if (tile.selectionType == HexCell.SELECTION_TYPE.AIM)
                            {
                                tile.SelectCell(HexCell.SELECTION_TYPE.AIM_IMPACT);
                            }
                            else
                            {
                                if (tile.selectionType == HexCell.SELECTION_TYPE.DISABLED_AIM)
                                    tile.SelectCell(HexCell.SELECTION_TYPE.DISABLEDAIM_IMPACT);
                                else tile.SelectCell(HexCell.SELECTION_TYPE.IMPACT);

                            }
                        }
                        else
                        {
                            if (tile.selectionType == HexCell.SELECTION_TYPE.DISABLED_AIM)
                            {
                                tile.SelectCell(HexCell.SELECTION_TYPE.DISABLED_AIMIMPACT);
                            }
                            else tile.SelectCell(HexCell.SELECTION_TYPE.DISABLED_IMPACT);
                        }

                        if (tile == originTile)
                        {
                            tile.SelectCell(HexCell.SELECTION_TYPE.ORIGIN_IMPACT);
                        }
                    }
                }
                break;
            case Attack.IMPACT_TYPE.LINE:
                foreach (HexCell tile in TilesManager.instance.GetImpactLine(playerTile.coordinates, originTile.coordinates, attack.rangeImpact, true, true))
                {
                    bool isInRange = false;
                    for (int i = 0; i < TilesInRange[0].Count; i++)
                    {
                        if (tile == TilesInRange[0][i])
                        {
                            isInRange = true;
                        }
                    }

                    if (isInRange)
                    {
                        if (tile.selectionType == HexCell.SELECTION_TYPE.AIM)
                        {
                            tile.SelectCell(HexCell.SELECTION_TYPE.AIM_IMPACT);
                        }
                        else
                        {
                            if (tile.selectionType == HexCell.SELECTION_TYPE.DISABLED_AIM)
                                tile.SelectCell(HexCell.SELECTION_TYPE.DISABLEDAIM_IMPACT);
                            else tile.SelectCell(HexCell.SELECTION_TYPE.IMPACT);

                        }
                    }
                    else
                    {
                        if (tile.selectionType == HexCell.SELECTION_TYPE.DISABLED_AIM)
                        {
                            tile.SelectCell(HexCell.SELECTION_TYPE.DISABLED_AIMIMPACT);
                        }
                        else tile.SelectCell(HexCell.SELECTION_TYPE.DISABLED_IMPACT);
                    }

                    if (tile == originTile)
                    {
                        tile.SelectCell(HexCell.SELECTION_TYPE.ORIGIN_IMPACT);
                    }
                }
                break;
            case Attack.IMPACT_TYPE.ARC:
                foreach (HexCell tile in TilesManager.instance.GetImpactArc(playerTile.coordinates, originTile.coordinates, true, true))
                {
                    if (tile.tileType != HexCell.TILE_TYPE.WALL)
                    {
                        if (tile == originTile)
                        {
                            tile.SelectCell(HexCell.SELECTION_TYPE.ORIGIN_IMPACT);
                        }
                        else if (tile.selectionType == HexCell.SELECTION_TYPE.AIM)
                        {
                            tile.SelectCell(HexCell.SELECTION_TYPE.AIM_IMPACT);
                        }
                        else tile.SelectCell(HexCell.SELECTION_TYPE.IMPACT);
                    }
                    else tile.SelectCell(HexCell.SELECTION_TYPE.DISABLED_IMPACT);
                }
                break;
            case Attack.IMPACT_TYPE.RADIUS:
                // SELECTION DES TILES EN FONCTION DE LEUR VISIBILITE
                foreach (HexCell tile in TilesManager.instance.GetRangeInRadius(originTile.coordinates, attack.radiusUnattackableImpact, attack.rangeImpact, true, true, true))
                {
                    bool isInRange = false;
                    for (int i = 0; i < TilesInRange[0].Count; i++)
                    {
                        if (tile == TilesInRange[0][i])
                        {
                            isInRange = true;
                        }
                    }

                    if (isInRange)
                    {
                        if (tile.selectionType == HexCell.SELECTION_TYPE.AIM)
                        {
                            tile.SelectCell(HexCell.SELECTION_TYPE.AIM_IMPACT);
                        }
                        else
                        {
                            if (tile.selectionType == HexCell.SELECTION_TYPE.DISABLED_AIM)
                                tile.SelectCell(HexCell.SELECTION_TYPE.DISABLEDAIM_IMPACT);
                            else tile.SelectCell(HexCell.SELECTION_TYPE.IMPACT);

                        }
                    }
                    else
                    {
                        if (tile.selectionType == HexCell.SELECTION_TYPE.DISABLED_AIM)
                        {
                            tile.SelectCell(HexCell.SELECTION_TYPE.DISABLED_AIMIMPACT);
                        }
                        else tile.SelectCell(HexCell.SELECTION_TYPE.DISABLED_IMPACT);
                    }

                    if (tile == originTile)
                    {
                        tile.SelectCell(HexCell.SELECTION_TYPE.ORIGIN_IMPACT);
                    }
                }
                break;
            case Attack.IMPACT_TYPE.SPAWNOBJECT:
                originTile.SelectCell(HexCell.SELECTION_TYPE.ORIGIN_IMPACT);
                break;
            case Attack.IMPACT_TYPE.TELEPORT:
                originTile.SelectCell(HexCell.SELECTION_TYPE.ORIGIN_IMPACT);
                break;
        }

        if (originTile.selectionType == HexCell.SELECTION_TYPE.AIM)
        {
            originTile.SelectCell(HexCell.SELECTION_TYPE.ORIGIN_AIM);
        }

    }

    public void LaunchAttack(HeroController heroController)
    {
        if(attack.nomDuSon != "")
        AudioManager.instance.Play(attack.nomDuSon);

        foreach (Transform child in heroController.BTNS_KillSpawnerParent)
        {
            Destroy(child.gameObject);
        }

        switch (attack.impactType)
        {
            case Attack.IMPACT_TYPE.SPAWNOBJECT:
                GameObject newObject = Instantiate(attack.spawnObject, originTile.transform.position, attack.spawnObject.transform.rotation);
                originTile.item = newObject;
                if (newObject.GetComponent<Mine>())
                {
                    newObject.GetComponent<Mine>().myTile = originTile;
                    newObject.transform.localScale = new Vector3(.7f, .7f, .7f);
                }
                else if (newObject.GetComponent<Grenade>())
                {
                    Grenade grenadeScript = newObject.GetComponent<Grenade>();
                    grenadeScript.hero = playerTile.hero;
                    grenadeScript.myTile = originTile;
                    playerTile.hero.grenades.Add(grenadeScript);
                }
                break;
            case Attack.IMPACT_TYPE.TELEPORT:
                AchievementsManager.TriggerAchievement("CgkImpif4cQQEAIQBw", playerTile.coordinates);
                originTile.hero = playerTile.hero;
                playerTile.myTileSprite.color = TilesManager.instance.classicColor;
                playerTile.hero = null;
                originTile.hero.myTile = originTile;
                originTile.hero.transform.position = originTile.transform.position;
                originTile.myTileSprite.color = originTile.hero.myTileColor;

                GameObject particle = Instantiate(originTile.hero.teleportParticle, originTile.transform);
                particle.transform.localScale = new Vector3(.5f, .5f, .5f);
                particle.transform.localPosition = new Vector2(0, -.2f);

                break;
            default:
                foreach (HexCell tile in TilesManager.instance._selectedTiles)
                {
                    if (tile.selectionType == HexCell.SELECTION_TYPE.IMPACT || tile.selectionType == HexCell.SELECTION_TYPE.ORIGIN_IMPACT)
                    {
                        GameObject bullet = Instantiate(heroController.shootParticle, heroController.transform);

                        bullet.GetComponent<Bullet>().target = tile.transform;
                        bullet.GetComponent<Bullet>().impactParticle = heroController.bulletImpactParticle;

                        if (tile.hero != null)
                        {
                            if (heroController.heroType.Equals(HeroController.HERO_TYPE.COWBOY))
                            {
                                tile.hero.TakeDamages(attack.damages, "cowboy", "attack");

                            }
                            else if (heroController.heroType.Equals(HeroController.HERO_TYPE.SOLDIER))
                            {
                                tile.hero.TakeDamages(attack.damages, "soldier", "attack");
                                
                            }
                            else
                            {
                                tile.hero.TakeDamages(attack.damages, "enemy", "attack");
                            }
                        }
                        else if (tile.enemy != null)
                        {
                            if (heroController.heroType.Equals(HeroController.HERO_TYPE.COWBOY))
                            {
                                tile.enemy.TakeDamages(attack.damages, "cowboy", "attack");
                                

                            }
                            else if (heroController.heroType.Equals(HeroController.HERO_TYPE.SOLDIER))
                            {
                                tile.enemy.TakeDamages(attack.damages, "soldier", "attack");
                                

                            }
                            else
                            {
                                tile.enemy.TakeDamages(attack.damages, "enemy", "attack");

                            }

                        }
                        else if(tile.item != null && tile.item.GetComponent<Spawner>())
                        {
                            //SHOW SHIELD
                        }
                    }
                }
                break;
        }

        UI_Caller.ActivateAttack();
        TilesManager.instance.ClearTiles(false);

    }


}

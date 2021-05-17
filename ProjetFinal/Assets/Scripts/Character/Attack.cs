using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Attack", menuName = "Attack")]
public class Attack : ScriptableObject
{
    [TextArea]
    [Tooltip(" OWNCELL can be mixed with : POINT, LINE or RADIUS \n RANGE_TYPE.LINE & RANGETYPE.RADIUS can be mixed with all ATTACK_TYPE")]
    public Sprite sprite;
    public new string name;

    public int costPA;
    public int cooldown;
    public int nbrUsePerTurn;
    public int nbrUseTotal;

    public enum RANGE_TYPE {OWNCELL, LINE, RADIUS}
    public RANGE_TYPE rangeType;
    public int rangeAttack;
    public int radiusUnattackableAttack;


    public enum IMPACT_TYPE { POINT, LINES, LINE, ARC, RADIUS, SPAWNOBJECT }
    public IMPACT_TYPE impactType;
    public int damages;
    public int rangeImpact;
    public int radiusUnattackableImpact;

    public GameObject spawnObject;


}

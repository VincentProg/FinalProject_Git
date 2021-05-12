using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu (fileName = "New Attack", menuName = "Attack")]
public class Attacks : ScriptableObject
{

    public Image attackImage;
    public string attackName;
    public enum ATTACK_TYPE {MELEE, SHOT, RADIUS}
    public ATTACK_TYPE type;
    public int range;
    public int radiusUnattackable;

    public int damages;

}

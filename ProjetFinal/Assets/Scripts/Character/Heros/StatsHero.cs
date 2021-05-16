using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hero", menuName = "Hero")]
public class StatsHero : ScriptableObject
{

    public Sprite sprite;
    public string heroName;
    public int health;
    public int PM;
    public int PA;
    public bool isDofusPM;

}

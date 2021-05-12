using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class StatsEnemy : ScriptableObject
{
    public Sprite sprite;
    public string enemyName;
    public int health;
    public int PM;
    public int damages;
}

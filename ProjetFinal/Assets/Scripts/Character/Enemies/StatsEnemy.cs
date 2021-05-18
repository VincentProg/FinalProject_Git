using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class StatsEnemy : ScriptableObject
{
    [System.Serializable]
    public struct AttackStats
    {
        public int damages;
        public int costPA;
        public int impactRange;
    }

    public Sprite sprite;
    public string enemyName;
    public int health;
    public int PM;
    public int PA;

    public List<AttackStats> attacks;
}

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
        public int range;
    }

    public Sprite sprite;
    public string enemyName;
    public int health;
    public int PM;
    public int PA;

    public enum ENEMY_TYPE { CAC , DISTANCE};
    public ENEMY_TYPE Type;

    public List<AttackStats> attacks;
}

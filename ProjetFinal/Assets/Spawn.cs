using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public List<GameObject> enemyList = new List<GameObject>();
    public int nbOfEntityToSpawn;
    [SerializeField] private int nbEntityLeft;
    public int turnDelay;
    int nbOfTurnBeforeSpawning;
    public int maxHp;
    public int hp;

    [SerializeField] List<HexCell> adjacentCells = new List<HexCell>();
    public HexCell pos;
    void Start()
    {
        
        pos.item = gameObject;
        pos.enemy = null;
        pos.hero = null;
        nbEntityLeft = nbOfEntityToSpawn;
        nbOfTurnBeforeSpawning = 0;
        hp = maxHp;
    }

    void Update()
    {
    }

    public void SpawnEnemies()
    {
        nbEntityLeft = nbOfEntityToSpawn;
        if (nbOfTurnBeforeSpawning == 0)
        {
            while (nbEntityLeft != 0)
            {
                bool hasSpawned = false;
                for (int i = 0; i < adjacentCells.Count; i++)
                {
                    if (adjacentCells[i].enemy == null && adjacentCells[i].hero == null && adjacentCells[i].item == null && nbEntityLeft >0)
                    {
                        GameObject enemy = Instantiate(enemyList[Random.Range(0, enemyList.Count)], adjacentCells[i].gameObject.transform.position, transform.rotation);
                        CombatSystem.instance.enemies.Add(enemy.GetComponent<EnemyController>());
                        nbEntityLeft--;
                        adjacentCells[i].item = enemy;
                        hasSpawned = true;
                    }
                }
                if (!hasSpawned)
                {
                    break;
                }
            }
            nbOfTurnBeforeSpawning = turnDelay;
        }
        else
            nbOfTurnBeforeSpawning--;

        CombatSystem.instance.NextTurn();
    }

    public void TakeDamages(int damages)
    {
        hp -= damages;
        hp = Mathf.Clamp(hp, 0, maxHp);

        if (hp == 0)
        {
            Death();
        }
    }

    public void Death()
    {
        print("ta ded sa chakal");
    }
}

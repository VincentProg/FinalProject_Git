using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> enemyList = new List<GameObject>();
    public int nbOfEntityToSpawn;
    [SerializeField] private int nbEntityLeft;
    public int turnDelay;
    int nbOfTurnBeforeSpawning;

    private bool isDead;

    [SerializeField] List<HexCell> adjacentCells = new List<HexCell>();
    private HexCell myTile;
    [SerializeField]
    Color colorSprite;

    [SerializeField]
    Sprite spriteDead;



    void Start()
    {
        nbEntityLeft = nbOfEntityToSpawn;
        nbOfTurnBeforeSpawning = 0;

        CombatSystem.instance.spawners.Add(this);

        
        #region GET MY START TILE()
        // AJOUT TUILE DEPART
        RaycastHit2D hitStart = Physics2D.Raycast(transform.position, Vector2.zero, Mathf.Infinity);
        if (hitStart)
        {
            if (hitStart.transform.GetComponent<HexCell>())
            {
                myTile = hitStart.transform.GetComponent<HexCell>();
                myTile.item = gameObject;

                adjacentCells = TilesManager.instance.GetRadius(myTile.coordinates, 1, false, false, true);
            }
        }
        #endregion

        myTile.myTileSprite.color = colorSprite;
        GetComponent<SpriteRenderer>().sortingOrder = myTile.coordinates.Y - myTile.coordinates.X;
    }


    public void SpawnEnemies()
    {
        if (!isDead)
        {
            nbEntityLeft = nbOfEntityToSpawn;
            if (nbOfTurnBeforeSpawning == 0)
            {
                while (nbEntityLeft != 0)
                {
                    bool hasSpawned = false;
                    for (int i = 0; i < adjacentCells.Count; i++)
                    {
                        if (!adjacentCells[i].isPossessed() && adjacentCells[i].tileType == HexCell.TILE_TYPE.GROUND && nbEntityLeft > 0)
                        {
                            GameObject enemy = Instantiate(enemyList[Random.Range(0, enemyList.Count)], adjacentCells[i].gameObject.transform.position, transform.rotation);
                            EnemyController enemyScript = enemy.GetComponent<EnemyController>();
                            enemyScript.hasSpawned = true;
                            enemyScript.Initialize();
                            nbEntityLeft--;
                            adjacentCells[i].enemy = enemy.GetComponent<EnemyController>();
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
            else nbOfTurnBeforeSpawning--;
               

            CombatSystem.instance.NextTurn();
        }
    }


    public void Death()
    {
        isDead = true;
        myTile.myTileSprite.color = TilesManager.instance.classicColor;
        GetComponent<SpriteRenderer>().sprite = spriteDead;
        CombatSystem.instance.DestroySpawner(this);
    }
}

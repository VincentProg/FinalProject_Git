using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombatSystem : MonoBehaviour
{
    float cooldown = .05f;
    float next;

    public static CombatSystem instance { get; private set; }
    public enum CombatState
    {
        Start,
        PlayerTurn,
        Spawner,
        EnemiesTurn,
        Win,
        Lose
    }

    public CombatState state = CombatState.Start;

    public List<HeroController> heros;
    [HideInInspector]
    public List<EnemyController> enemies;
    public List<Spawner> spawners;
    int index = 0;
    bool heroesTurn;

    int nbrRound = 1;
    public TextMeshProUGUI nbrRoundTXT;

    // auto turn
/*    private void FixedUpdate()
    {
        if(Time.time > next)
        {
            next = Time.time + cooldown;
            heros[0].EndTurn();
        }
    }*/

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        nbrRoundTXT.text = nbrRound.ToString(); ;
        StartFight();
    }


    public void StartFight()
    {
        state = CombatState.PlayerTurn;
        heros[0].StartTurn();
    }


    public void NextTurn()
    {
        index++;

        switch (state)
        {
            case CombatState.PlayerTurn:
                TurnPlayer();
                break;
                
            case CombatState.Spawner:     
                TurnSpawner();
                break;

            case CombatState.EnemiesTurn:
                TurnEnemy();
                break;
            
        }     
    }

    private void TurnPlayer()
    {
        if (index < heros.Count)
        {
            heros[index].StartTurn();
        }
        else
        {
            state = CombatState.Spawner;
            index = 0;
            TurnSpawner();
        }
    }
    private void TurnSpawner()
    {
        if (index < spawners.Count)
        {
            spawners[index].SpawnEnemies();
        }
        else
        {
            state = CombatState.EnemiesTurn; // --> tour des ennemis
            index = 0;// reset de l'index
            TurnEnemy();
        }
    }
    private void TurnEnemy()
    {
        if (index < enemies.Count)
        {   
            enemies[index].StartTurn();
           
        }
        else
        {
            state = CombatState.PlayerTurn; // --> tour des players
            index = 0; // reset de l'index 
            nbrRound++;
            nbrRoundTXT.text = nbrRound.ToString();
            heros[index].StartTurn();
        }
    }

    public void Win()
    {
        state = CombatState.Win;
        print("WIN");
    }

    public void Loose()
    {
        state = CombatState.Lose;
    }

    public void DestroySpawner(Spawner spawner)
    {
        spawners.Remove(spawner);

        if(spawners.Count == 0)
        {
            Win();
        }
    }

}

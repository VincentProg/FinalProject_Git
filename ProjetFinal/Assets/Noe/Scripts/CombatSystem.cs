using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class CombatSystem : MonoBehaviour
{
    public GameManager gameManager;
    float cooldown = .05f;
    float next;

    public int killsSoldier;
    public int killsCowboy;

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

    public int nbrRound = 1;
    public UnityEngine.UI.Text nbrRoundTXT;

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
        ///nbrRoundTXT.text = nbrRound.ToString(); ;
        StartFight();
    }


    public void StartFight()
    {
        PlayGames.instance.initAchievements();
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
            NewRound();
        }
    }

    private void NewRound()
    {
        state = CombatState.PlayerTurn; // --> tour des players
        index = 0; // reset de l'index 
        nbrRound++;
        nbrRoundTXT.text = nbrRound.ToString();
        heros[index].StartTurn();

        AchievementsManager.TriggerAchievement("CgkImpif4cQQEAIQDg");

        if (nbrRound > 1)
            AchievementsManager.DeactivateAchievement("CgkImpif4cQQEAIQDA");
    }

    public void Win()
    {
        state = CombatState.Win;
        ButtonManager.instance.ShowWin();
        print("WIN");

        if (gameManager.levelNumber == 1)
            AchievementsManager.TriggerAchievement("CgkImpif4cQQEAIQAg");

        if (gameManager.isLastLevel)
            AchievementsManager.TriggerAchievement("CgkImpif4cQQEAIQAg");

        if (killsCowboy == 0 || killsSoldier == 0)
        {
            if(killsCowboy == 0 && killsSoldier == 0)
            {
                AchievementsManager.TriggerAchievement("CgkImpif4cQQEAIQCQ");

            }
            else
            {
                AchievementsManager.TriggerAchievement("CgkImpif4cQQEAIQCA");

            }
        }

        foreach (var item in heros)
        {
            if (item.health == 1)
                AchievementsManager.TriggerAchievement("CgkImpif4cQQEAIQDQ");
        }

    }

    public void Loose()
    {
        state = CombatState.Lose;
        ButtonManager.instance.ShowLose();
    }

    public void DestroySpawner(Spawner spawner)
    {
        print("spawner remaining : " + spawners.Count);
        spawners.Remove(spawner);

        if(spawners.Count == 0)
        {
            Win();
        }
    }

}

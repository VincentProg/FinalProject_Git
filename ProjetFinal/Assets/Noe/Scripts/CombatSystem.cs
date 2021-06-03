using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public int index = 0;
    bool heroesTurn;

    public int nbrRound = 1;
    public UnityEngine.UI.Text nbrRoundTXT;


    [Header("Particles prefabs")]
    public GameObject mineParticle;
    public GameObject spawnParticle;
    public GameObject kamikazeParticle;

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
        ///nbrRoundTXT.text = nbrRound.ToString(); 
        AudioManager.instance.StopPlayAll();
        AudioManager.instance.Play("music_level");
            
    }


    public void StartFight()
    {
       // PlayGames.instance.initAchievements();
        state = CombatState.PlayerTurn;
        heros[0].StartTurn();
    }


    public void NextTurn()
    {
        GameManager.instance.UnlitEnd();

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
        if(PlayerPrefs.GetInt("levelReached") <= SceneManager.GetActiveScene().buildIndex)
        {
            PlayerPrefs.SetInt("levelReached", SceneManager.GetActiveScene().buildIndex + 1);
        }
        print ("levelReached"+ SceneManager.GetActiveScene().buildIndex);
        AchievementsManager.TriggerAchievement("CgkImpif4cQQEAIQDQ");

        if (gameManager.levelNumber == 1)
            AchievementsManager.TriggerAchievement("CgkImpif4cQQEAIQAg");

        if (gameManager.isLastLevel)
            AchievementsManager.TriggerAchievement("CgkImpif4cQQEAIQAw");

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
        TilesManager.instance.ClearTiles(false);
        spawners.Remove(spawner);
        if (heros[0].isMyTurn)
        {
            heros[0].PA--;
            for (int i = 0; i < heros[0].attacks.Count; i++)
            {
                UI_Attack UIAttack = heros[0].myCanvas.transform.GetChild(0).transform.GetChild(i).GetComponent<UI_Attack>();
                UIAttack.UpdateBtnClickable(false);
            }
        } else
        {
            heros[1].PA -= 2;
            for (int i = 0; i < heros[1].attacks.Count; i++)
            {
                UI_Attack UIAttack = heros[1].myCanvas.transform.GetChild(0).transform.GetChild(i).GetComponent<UI_Attack>();
                UIAttack.UpdateBtnClickable(false);
            }
        }

        foreach (Transform child in heros[0].BTNS_KillSpawnerParent)
        {
            Destroy(child.gameObject);
        }

        if (spawners.Count == 0)
        {
            Win();
        }
    }

}

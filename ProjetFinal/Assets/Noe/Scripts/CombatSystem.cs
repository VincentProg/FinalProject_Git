using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombatSystem : MonoBehaviour
{
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
        if (state == CombatState.PlayerTurn)
        {
            if (index >= heros.Count) // SI ON ATTEINT LA FIN DE LA LISTE DES HEROS
            {

                state = CombatState.Spawner; // --> tour des spawner
                index = 0;// reset de l'index
            }
            else
            {
                heros[index].StartTurn(); //on joue le tour du player en question
                if(index == 0)
                {
                    //show "c'est a ton tour" anim
                }
            }
        }

        if (state == CombatState.Spawner)
        {
            if (index >= spawners.Count) // SI ON ATTEINT LA FIN DE LA LISTE DES SPAWNERS
            {
                state = CombatState.EnemiesTurn; // --> tour des ennemis
                index = 0;// reset de l'index
            }
            else
            {
                spawners[index].SpawnEnemies();
            }
        }

        if (state == CombatState.EnemiesTurn) // SI ON ATTEINT LA FIN DE LA LISTE DES ENNEMIS
        {
            //print("test");
            if (index >= enemies.Count - 1)
            {
                state = CombatState.PlayerTurn; // --> tour des players
                index = 0; // reset de l'index 
                nbrRound++;
                nbrRoundTXT.text = nbrRound.ToString();
                heros[index].StartTurn();
            }
            else
            {
                NextTurn();
            }
        }
        print(state);
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

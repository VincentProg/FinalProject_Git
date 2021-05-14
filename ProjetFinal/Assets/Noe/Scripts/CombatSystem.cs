using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    public static CombatSystem Instance { get; private set; }
    public enum CombatState
    {
        Start,
        PlayerTurn,
        EnnemiesTurn,
        Win,
        Lose
    }

    public CombatState state = CombatState.Start;

    public List<HeroController> heros;
    [HideInInspector]
    public List<EnemyController> ennemies;
    int index = 0;
    bool heroesTurn;
    int nbRound = 1;

    void Start()
    {
        StartFight();
    }

    private void StartFight()
    {
        heros[0].StartTurn();
    }

    public void NextTurn()
    {
        index++;

        if(state == CombatState.PlayerTurn)
        {
            if (index >= heros.Count - 1) // SI ON ATTEINT LA FIN DE LA LISTE DES HEROS
            {
                state = CombatState.EnnemiesTurn; // --> tour des ennemis
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

        if (state == CombatState.EnnemiesTurn) // SI ON ATTEINT LA FIN DE LA LISTE DES ENNEMIS
        {
            if (index >= ennemies.Count - 1)
            {
                state = CombatState.PlayerTurn; // --> tour des players
                index = 0; // reset de l'index
                nbRound++;
            }
            else
            {
                //ennemies[index].StartTurn();
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(state == CombatState.PlayerTurn || state == CombatState.EnnemiesTurn)
        {
            if (ennemies.Count == 0)
            {
                Win();
            }
        }
    }

    public void Win()
    {
        state = CombatState.Win;
    }

    public void Loose()
    {
        state = CombatState.Lose;
    }

}

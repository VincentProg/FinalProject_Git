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
        print("1");
        if (state == CombatState.PlayerTurn)
        {
            print("2");
            if (index >= heros.Count) // SI ON ATTEINT LA FIN DE LA LISTE DES HEROS
            {
                state = CombatState.EnnemiesTurn; // --> tour des ennemis
                index = 0;// reset de l'index
                print("3");
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
            print("test");
            if (index >= ennemies.Count - 1)
            {
                state = CombatState.PlayerTurn; // --> tour des players
                index = 0; // reset de l'index 
                nbrRound++;
                nbrRoundTXT.text = nbrRound.ToString();
                heros[index].StartTurn();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
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
    public List<EnemyController> enemies;
    int index = 0;
    bool heroesTurn;


    private void StartFight()
    {
        heros[index].StartTurn();


    }

    public void NextTurn()
    {

        index++;

    }

    public void CheckHealthOfEveryone(/* allies[], ennemies[] */)
    {
        /*isAllAlliesDead = true;
         *isAllEnnemiesDead = true;
         * for(int i = 0; i< allies.lengh; i++)
         * {
         *      if(allies[i].hp > 0)
         *      {
         *          isAllAlliesDead = false;
         *          break;
         *      }
         *          
         * }
         * 
         * 
         * for(int i = 0; i< ennemies.lengh; i++)
         * {
         *      if(ennemies[i].hp > 0)
         *      {
         *          isAllEnnemiesDead = false;
         *          break;
         *      }    
         * }
         * 
         * if(state == CombatState.EnnemiesTurn)
         * {
         *      StopCoroutine(EnnemiesTurn());
         *      
         * }
         * 
         * if(state == CombatState.PlayerTurn)
         * {
         *      StopCoroutine(PlayerTurn());
         * }
         * 
         * if(isAllAlliesDead){
         *      Lose();
         * }
         * if(isAllEnnemiesDead){
         *      Win();
         * }
         * 
         * */

        
    }

    public void Win()
    {
        state = CombatState.Win;
        //Show Win Screen
    }

    public void Lose()
    {
        state = CombatState.Lose;
        //Show Lose Screen
    }

    public IEnumerator StartCombat()
    {
        //spawn ennemies and allies on terrain;
        yield return new WaitForSeconds(0.01f);
    }

    public IEnumerator PlayerTurn()
    {
        //for (int i = 0; i< allies.Lengh; i++) --> player play
        yield return new WaitForSeconds(0.01f);
        StartCoroutine(EnnemyTurn());
    }
    public IEnumerator EnnemyTurn()
    {
        //foreach n in ennemies --> ia play
        yield return new WaitForSeconds(0.01f);
        StartCoroutine(PlayerTurn());
    }
}

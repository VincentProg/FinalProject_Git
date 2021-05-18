using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timeline : MonoBehaviour
{
    public Slider H1, H2;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        H1.value = (float)CombatSystem.instance.heros[0].health / CombatSystem.instance.heros[0].stats.health;
        H2.value = (float)CombatSystem.instance.heros[1].health / CombatSystem.instance.heros[1].stats.health;
    }
}

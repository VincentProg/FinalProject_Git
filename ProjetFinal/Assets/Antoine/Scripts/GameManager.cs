using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    int _currentRound = 0;
    public GameObject player;

    public int getRound
    {
        get { return _currentRound; }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }


    void StartNewRound()
    {
        _currentRound++;
    }
}

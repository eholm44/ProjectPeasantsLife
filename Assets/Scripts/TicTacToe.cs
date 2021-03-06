using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToe : MiniGame
{
    [SerializeField]
    private GameObject x;
    
    [SerializeField]
    private GameObject o;
    [SerializeField]
    private Transform oneOne;
    [SerializeField]
    private Transform oneTwo;
    [SerializeField]
    private Transform oneThree;
    [SerializeField]
    private Transform twoOne;
    [SerializeField]
    private Transform twoTwo;
    [SerializeField]
    private Transform twoThree;
    [SerializeField]
    private Transform threeOne;
    [SerializeField]
    private Transform threeTwo;
    [SerializeField]
    private Transform threeThree;

    // Start is called before the first frame update
    void Start()
    {
        numPlayers = 0;
        maxPlayers =2;

        players = new string[maxPlayers];
        
        gameName = gameObject.name;
    }

    public override void StartGame()
    {
        base.StartGame();

        
    }
}

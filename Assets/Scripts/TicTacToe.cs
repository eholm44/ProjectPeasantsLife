using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToe : MiniGame
{
    // Start is called before the first frame update
    void Start()
    {
        numPlayers = 0;
        maxPlayers =2;

        players = new GameObject[maxPlayers];
        
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        
    }
}

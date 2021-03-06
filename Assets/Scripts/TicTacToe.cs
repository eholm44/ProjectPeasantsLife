using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class TicTacToe : MiniGame
{
    private int playerTurn;
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

    private SyncDictionary<Transform, bool> myDict = new SyncDictionary<Transform, bool>();

    [Server]
    public override void AddPiece(string transformName, string playerName)
    {
        //Test if players turn
        if(!playerName.Equals(players[playerTurn]))
        {
            return;
        }
        Transform transformHit = FindTransform(transformName);
        //Get game piece to place
        GameObject gamePiece = GetGamePiece();
        if(transformHit == null)
        {
            return;
        }
        //Place game piece on board
        GameObject xInstance = Instantiate(gamePiece, transformHit.position, transformHit.rotation);
        xInstance.transform.SetParent(gameObject.transform);
        NetworkServer.Spawn(xInstance);
    }

    [Server]
    private Transform FindTransform(string transformName)
    {
        //Why does this work!?!
        foreach(Transform key in myDict.Keys)
        {
            if (key.name == transformName)
            {
                //game space taken
                if(myDict[key] == true)
                {
                    return null;
                }
                myDict[key] = true;
                return key;
            }
        }
        return null;
    }

    [Server]
    public override GameObject GetGamePiece()
    {
        if(playerTurn == 0)
        {
            playerTurn = 1;
            return x;
        }
        if(playerTurn == 1)
        {
            playerTurn = 0;
            return o;
        }
        return x;
    }

    [Server]
    public override void ClearBoard()
    {
        foreach(Transform child in gameObject.transform)
        {
            if(child.name.Contains("X") || child.name.Contains("O"))
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        myDict[oneOne] = false;
        myDict[oneTwo] = false;
        myDict[oneThree] = false;
        myDict[twoOne] = false;
        myDict[twoTwo] = false;
        myDict[twoThree] = false;
        myDict[threeOne] = false;
        myDict[threeTwo] = false;
        myDict[threeThree] = false;

        playerTurn = 0;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        myDict.Add(oneOne, false);
        myDict.Add(oneTwo, false);
        myDict.Add(oneThree, false);
        myDict.Add(twoOne, false);
        myDict.Add(twoTwo, false);
        myDict.Add(twoThree, false);
        myDict.Add(threeOne, false);
        myDict.Add(threeTwo, false);
        myDict.Add(threeThree, false);

        numPlayers = 0;
        maxPlayers =2;

        players = new string[maxPlayers];
        
        gameName = gameObject.name;

        playerTurn = 0;
    }

    public override void StartGame()
    {
        base.StartGame();

        gameStarted = true;
    }

    

    
}

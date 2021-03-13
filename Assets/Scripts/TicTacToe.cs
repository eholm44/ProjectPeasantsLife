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

    private SyncDictionary<Transform, GameObject> myDict = new SyncDictionary<Transform, GameObject>();

    private GameObject[] boardMatrix = new GameObject[9]{null,null,null,null,null,null,null,null,null};

    [Server]
    public override void AddPiece(string transformName, string playerName)
    {
        //Test if players turn
        if(!playerName.Equals(players[playerTurn]))
        {
            return;
        }
        Transform transformHit = FindTransform(transformName);
        if(transformHit == null)
        {
            return;
        }
        //Get game piece to place
        GameObject gamePiece = GetGamePiece();
        //Place game piece on board
        GameObject xInstance = Instantiate(gamePiece, transformHit.position, transformHit.rotation);

        if (transformHit == oneOne)
        {
            boardMatrix[0] = xInstance;
        }
        else if (transformHit == oneTwo)
        {
            boardMatrix[1] = xInstance;
        }
        else if (transformHit == oneThree)
        {
            boardMatrix[2] = xInstance;
        }
        else if (transformHit == twoOne)
        {
            boardMatrix[3] = xInstance;
        }
        else if (transformHit == twoTwo)
        {
            boardMatrix[4] = xInstance;
        }
        else if (transformHit == twoThree)
        {
            boardMatrix[5] = xInstance;
        }else if (transformHit == threeOne)
        {
            boardMatrix[6] = xInstance;
        }
        else if (transformHit == threeTwo)
        {
            boardMatrix[7] = xInstance;
        }
        else if (transformHit == threeThree)
        {
            boardMatrix[8] = xInstance;
        }

        //for (int i = 0; i < 3; i++)
        //{
        //    for (int j = 0; j < 3; j++)
        //    {
        //        Debug.Log(i + ", " + j + ": " + boardMatrix[i,j]);
        //    }
        //}

        NetworkServer.Spawn(xInstance);

        if (CheckForWin() == 1)
        {
            Debug.Log("Win");
        }
        else
        {
            Debug.Log("No Win");
        }
    }

    private Transform FindTransform(string transformName)
    {
        //Why does this work!?!
        foreach(Transform key in myDict.Keys)
        {
            if (key.name == transformName)
            {
                //game space taken
                if(myDict[key] != null)
                {
                    return null;
                }
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
        foreach(GameObject piece in myDict.Values)
        {
            GameObject.Destroy(piece);
        }

        playerTurn = 0;
    }

    [Server]
    public override void OnStartServer()
    {
        base.OnStartServer();

        myDict.Add(oneOne, null);
        myDict.Add(oneTwo, null);
        myDict.Add(oneThree, null);
        myDict.Add(twoOne, null);
        myDict.Add(twoTwo, null);
        myDict.Add(twoThree, null);
        myDict.Add(threeOne, null);
        myDict.Add(threeTwo, null);
        myDict.Add(threeThree, null);

        numPlayers = 0;
        maxPlayers =2;

        players = new string[maxPlayers];
        
        gameName = gameObject.name;

        playerTurn = 0;
    }

    [Server]
    private int CheckForWin()
    {
        if (GetName(0) == GetName(1) && GetName(1) == GetName(2) && GetName(0) != "bad")
        {
            return 1;
        }
        else if (GetName(3) == GetName(4) && GetName(4) == GetName(5) && GetName(3) != "bad")
        {
            return 1;
        }
        else if (GetName(6) == GetName(7) && GetName(7) == GetName(8) && GetName(6) != "bad")
        {
            return 1;
        }
        else if (GetName(0) == GetName(3) && GetName(3) == GetName(6) && GetName(0) != "bad")
        {
            return 1;
        }
        else if (GetName(1) == GetName(4) && GetName(4) == GetName(7) && GetName(1) != "bad")
        {
            return 1;
        }
        else if (GetName(2) == GetName(5) && GetName(5) == GetName(8) && GetName(2) != "bad")
        {
            return 1;
        }
        else if (GetName(0) == GetName(4) && GetName(4) == GetName(8) && GetName(0) != "bad")
        {
            return 1;
        }
        else if (GetName(2) == GetName(4) && GetName(4) == GetName(6) && GetName(2) != "bad")
        {
            return 1;
        }
        return 0;
    }

    [Server]
    private string GetName(int index)
    {
        if (boardMatrix[index] != null)
        {
            return boardMatrix[index].name;
        }
        return "bad";
    }

    
}

using UnityEngine;
using Mirror;
using System;
using TMPro;

public class MiniGame : Targetable
{
    public bool gameStarted;
    public int numPlayers;
    public int maxPlayers;
    public string[] players;

    public string gameName;

    public GameObject gameCamera;

    #region Server

    [Server]
    public void JoinGame(GameObject newPlayer)
    {
        //Test if game is full
        if (numPlayers == maxPlayers)
        {
            return;
        }
        foreach(string p in players)
        {
            if (p != null && p.Equals(newPlayer))
            {
                return;
            }
        }
        //Add player to game and increase number of players
        players[numPlayers] = newPlayer.GetComponentInChildren<TMP_Text>().text;
        numPlayers++;
        
        RpcUpdateGame(players);
    }

    [Server]
    public void LeaveGame(GameObject leavePlayer)
    {
        //Remove player from game
        for (int i = 0; i < maxPlayers; i++)
        {
            if (players[i] != null && players[i] == leavePlayer.GetComponentInChildren<TMP_Text>().text)
            {
                players[i] = null;
            }
        }
        numPlayers--;

        ClearBoard();

        RpcUpdateGame(players);
    }

    [Server]
    public virtual void StartGame(){}
    public virtual GameObject GetGamePiece(){return null;}
    public virtual void AddPiece(string transformName, string playerName){}

    public virtual void ClearBoard(){}

#endregion
#region Client
    //Update local side player array
    private void HandlePlayersUpdated(string[] oldPlayers, string[] newPlayers)
    {
        players = newPlayers;  
    }
    //Update other clients arrays
    [ClientRpc]
    private void RpcUpdateGame(string[] newPlayers)
    {
        players = newPlayers;
    }

#endregion
}

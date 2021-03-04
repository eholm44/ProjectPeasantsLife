using UnityEngine;
using Mirror;
using System;
using TMPro;

public class MiniGame : NetworkBehaviour
{
    public int numPlayers;
    public int maxPlayers;
    public string[] players;

    public string gameName;

    public Camera mainCamera;

    public GameObject gameCamera;

    public GameObject leaveButton;

    #region Server

    [Server]
    public void JoinGame(string newPlayer)
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
        players[numPlayers] = newPlayer;
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

        RpcUpdateGame(players);
    }

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

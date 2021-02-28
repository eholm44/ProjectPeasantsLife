using TMPro;
using UnityEngine;
using Mirror;

public class MiniGame : NetworkBehaviour
{
    public int numPlayers;
    public int maxPlayers;
    public string[] players;

    public string gameName;

    private Camera mainCamera;
    
    #region Server

    [Server]
    public void JoinGame(string newPlayer)
    {
        //Test if already in the game
        foreach(string p in players)
        {
            if (p != null && p.Equals(newPlayer))
            {
                return;
            }
        }
        //Test if game is full
        if (numPlayers == maxPlayers)
        {
            return;
        }
        //Add player to game and increase number of players
        players[numPlayers] = newPlayer;
        numPlayers++;
        
        RpcJoinGame(players);
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
    private void RpcJoinGame(string[] newPlayers)
    {
        players = newPlayers;
    }

#endregion
}

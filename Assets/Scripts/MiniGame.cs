using TMPro;
using UnityEngine;
using Mirror;

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
        //Add player to game and increase number of players
        players[numPlayers] = newPlayer;
        numPlayers++;
        
        RpcJoinGame(players);
    }

    [Server]
    public void LeaveGame(string leavePlayer)
    {
        Debug.Log("Here");
        //Remove player from game
        for (int i = 0; i < maxPlayers; i++)
        {
            if (players[i] != null && players[i] == leavePlayer)
            {
                players[i] = null;
            }
        }
        numPlayers--;

        RpcLeaveGame(players);
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
    [ClientRpc]
    private void RpcLeaveGame(string[] newPlayers)
    {
        players = newPlayers;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        mainCamera = Camera.main;
        mainCamera.gameObject.SetActive(true);
        gameCamera.SetActive(false);
    }

#endregion
}

using UnityEngine;
using Mirror;

public class MyNetworkManager : NetworkManager
{
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        Debug.Log("I Connected To A Server!");
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        MyNetworkPlayer player = conn.identity.GetComponent<MyNetworkPlayer>();

        player.SetDisplayName(numPlayers);

        Color displayColor = new Color(Random.Range(0f,1f), Random.Range(0f,1f), Random.Range(0f,1f));

        player.SetDisplayColor(displayColor);
    }
}

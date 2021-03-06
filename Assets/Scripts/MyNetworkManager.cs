using UnityEngine;
using Mirror;

public class MyNetworkManager : NetworkManager
{

    [SerializeField] private GameObject[] targets = null;
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        Debug.Log("I Connected To A Server!");
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        MyNetworkPlayer player = conn.identity.GetComponent<MyNetworkPlayer>();
        MyPlayerMovement movement = conn.identity.GetComponent<MyPlayerMovement>();

        player.SetDisplayName(numPlayers);

        Color displayColor = new Color(Random.Range(0f,1f), Random.Range(0f,1f), Random.Range(0f,1f));

        player.SetDisplayColor(displayColor);

        GameObject target = GetTarget();

        movement.SetTarget(target.name);
    }

    private GameObject GetTarget()
    {
        
        foreach (GameObject t in targets)
        {
            if (t.name == $"Target {numPlayers}")
            {
                return t;               
            }
        }
        return null;
    }
}

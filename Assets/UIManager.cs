using UnityEngine;

public class UIManager : MonoBehaviour
{   
    public GameObject localPlayer; 
    public void LeaveGame()
    {
        localPlayer.GetComponent<MyNetworkPlayer>().LeaveGame(localPlayer);
    }
}

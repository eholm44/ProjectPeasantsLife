using TMPro;
using UnityEngine;

public class MiniGame : MonoBehaviour
{
    public int numPlayers;
    public int maxPlayers;
    public GameObject[] players;

    private Camera mainCamera;
    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    virtual public void Update()
    {
        mainCamera = Camera.main;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit) && Input.GetMouseButtonDown(0))
        {
            JoinGame(mainCamera.GetComponent<CameraController>().target.gameObject.transform.parent.gameObject);
        }
    }

    public void JoinGame(GameObject player)
    {
        foreach(GameObject p in players)
        {
            //TODO: fix for mutliplayer
            if (p != null && p.Equals(player))
            {
                return;
            }
        }
        if (numPlayers == maxPlayers)
        {
            return;
        }

        players[numPlayers] = player;
        numPlayers++;
    }
}

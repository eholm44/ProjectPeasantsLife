using UnityEngine;
using Mirror;
using UnityEngine.AI;

public class MyPlayerMovement : NetworkBehaviour
{
    private Camera mainCamera;

    [SyncVar(hook=nameof(HandleTargetUpdated))]
    [SerializeField]
    private string target;

    [SyncVar(hook=nameof(HandlePositionUpdated))]
    private Vector3 position;

    [SyncVar(hook=nameof(HandleDistanceUpdated))]
    private float distance;

    public bool canMove = true;

    public LocalPlayerManager playerManager = null;

#region Server

    [Command]
    public void CmdSetPosition(Vector3 newPosition)
    {
        SetPosition(newPosition);
    }

    [Command]
    public void CmdJoinGame(MiniGame miniGame, string newPlayer)
    {
        string[] players = miniGame.players;
        //Test if already in the game
        foreach(string p in players)
        {
            if (p != null && p.Equals(newPlayer))
            {
                return;
            }
        }
        //Test if game is full
        if (miniGame.numPlayers == miniGame.maxPlayers)
        {
            return;
        }
        playerManager.currentMinigame = miniGame.gameObject;

        miniGame.JoinGame(newPlayer);

        
        miniGame.gameCamera.SetActive(true);
        mainCamera.gameObject.SetActive(false);
        miniGame.leaveButton.SetActive(true);
        
        canMove = false;
    }

    [Server]
    public void SetPosition(Vector3 newPosition)
    {
        position = newPosition;
    }

    [Server]
    public void SetTarget(string newTarget)
    {
        target = newTarget;
    }

    [Command]
    public void CmdSetDistance(float newDistance)
    {
        distance = newDistance;
    }

#endregion

#region Client

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        canMove = true;

        mainCamera = Camera.main;

        playerManager = GameObject.Find("PlayerManager").GetComponent<LocalPlayerManager>();

        //Create point on character for camera to center on
        GameObject child = new GameObject();
        child.transform.parent = gameObject.transform;
        child.transform.position = new Vector3(transform.position.x, transform.position.y + 1.15f, transform.position.z);

        mainCamera.GetComponent<CameraController>().target = child.transform;
    }


    [ClientCallback]
    private void Update()
    {
        if(!canMove)
        {
            return;
        }
        //Only act for clients player
        if(!hasAuthority)
        {
            return;
        }
        
        //Is left mouse button clicked TODO: change to new input system
        if(Input.GetMouseButton(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        //Check if clicking viable screen loacation
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            return;
        }

        //Adjust stopping distance based on what is clicked
        NavMeshAgent nav = gameObject.GetComponent<NavMeshAgent>();
        if (hit.collider.CompareTag("MiniGame"))
        {
            //Test if player close enough to join game
            if(Vector3.Distance(gameObject.transform.position,hit.transform.position) > 5f)
            {
                return;
            }
            CmdJoinGame(hit.collider.gameObject.GetComponent<MiniGame>(), GetComponent<MyNetworkPlayer>().displayName);
            CmdSetDistance(2f);
        }
        else
        {
            CmdSetDistance(.2f);
        }
        
        //Send players move target to server
        CmdSetPosition(hit.point);
        } 
    }

    private void HandleTargetUpdated(string oldTarget, string newTarget)
    {
        GameObject.Find(newTarget).transform.position = gameObject.transform.position;
        gameObject.GetComponent<AutoMove>().target = GameObject.Find(newTarget);   
    }

    private void HandlePositionUpdated(Vector3 oldPostion, Vector3 newPosition)
    {
        gameObject.GetComponent<AutoMove>().target.transform.position = newPosition;   
    }

    private void HandleDistanceUpdated(float oldDistance, float newDistance)
    {
        NavMeshAgent nav = gameObject.GetComponent<NavMeshAgent>();
        nav.stoppingDistance = newDistance;  
    }

#endregion
}


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

#region Server

    [Command]
    public void CmdSetTarget(Vector3 newPosition)
    {
        SetPosition(newPosition);
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

#endregion

#region Client

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        mainCamera = Camera.main;

        //Create point on character for camera to center on
        GameObject child = new GameObject();
        child.transform.parent = gameObject.transform;
        child.transform.position = new Vector3(transform.position.x, transform.position.y + 1.15f, transform.position.z);

        mainCamera.GetComponent<CameraController>().target = child.transform;
    }


    [ClientCallback]
    private void Update()
    {
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
            nav.stoppingDistance = 2f;
        }
        else{
            nav.stoppingDistance = .2f;
        }
        
        //Send players move target to server
        CmdSetTarget(hit.point);
        } 
    }

    private void HandleTargetUpdated(string oldTarget, string newTarget)
    {
        GameObject.Find(newTarget).transform.position = gameObject.transform.position;
        gameObject.GetComponent<AutoMove>().target = GameObject.Find(newTarget);   
    }

    private void HandlePositionUpdated(Vector3 oldPostion, Vector3 newPosition)
    {
        Debug.Log(gameObject.GetComponent<AutoMove>().target);
        gameObject.GetComponent<AutoMove>().target.transform.position = newPosition;   
    }

#endregion
}


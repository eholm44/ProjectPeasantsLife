using UnityEngine;
using Mirror;
using UnityEngine.AI;
using UnityEngine.InputSystem;

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

#region Server

    [Command]
    public void CmdSetPosition(Vector3 newPosition)
    {
        SetPosition(newPosition);
    }

    [Command]
    public void CmdSetDistance(float newDistance)
    {
        distance = newDistance;
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
        mainCamera = Camera.main;

        mainCamera.GetComponent<CameraController>().target = GetComponent<Targetable>().GetAimPoint();
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


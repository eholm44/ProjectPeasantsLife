using UnityEngine;
using Mirror;
using UnityEngine.AI;

public class MyPlayerMovement : NetworkBehaviour
{
    [SerializeField] private GameObject[] targets = null;

    private Camera mainCamera;

#region Server



#endregion

#region Client

    public override void OnStartAuthority()
    {
        
        base.OnStartAuthority();

        mainCamera = Camera.main;
        GameObject child = new GameObject();
        child.transform.parent = gameObject.transform;
        child.transform.position = new Vector3(transform.position.x, transform.position.y + 1.15f, transform.position.z);
        mainCamera.GetComponent<CameraController>().target = child.transform;
    }


    //[ClientCallback]
    private void Update()
    {
        
        if(!hasAuthority)
        {
            return;
        }
        
        if(Input.GetMouseButton(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            return;
        }
        
        gameObject.GetComponent<MyNetworkPlayer>().CmdSetTarget(hit.point);
        gameObject.GetComponent<AutoMove>().target.transform.position = hit.point;
        } 
    }

#endregion
}


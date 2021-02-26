using UnityEngine;
using Mirror;
using TMPro;

public class MyNetworkPlayer : NetworkBehaviour
{
    [SerializeField] private TMP_Text displayNameText = null;
    [SerializeField] private Renderer displayColorRenderer = null;

    //[SerializeField] private GameObject myTarget = null;
    
    [SyncVar(hook=nameof(HandleDisplayNameUpdated))]
    [SerializeField]
    private string displayName = "MissingName";

    [SyncVar(hook=nameof(HandleTargetUpdated))]
    private GameObject target = null;

    [SyncVar(hook=nameof(HandlePositionUpdated))]
    private Vector3 position;

    [SyncVar(hook=nameof(HandleDisplayColorUpdated))]
    [SerializeField]
    private Color displayColor = Color.black;

#region Server

    [Server]
    public void SetDisplayName(string newDisplayName)
    {
        displayName = newDisplayName;
    }

    [Server]
    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
    }

    [Server]
    public void SetPosition(Vector3 newPosition)
    {
        position = newPosition;
    }

    [Server]
    public void SetDisplayColor(Color newDisplayColor)
        {
            displayColor = newDisplayColor;
        }

    [Command]
    public void CmdSetTarget(Vector3 newPosition)
    {
        SetPosition(newPosition);
    }

    [Command]
    private void CmdSetDisplayName(string newDisplayName)
    {
    // TODO if is blacklisted
        if(newDisplayName.Length < 2 || newDisplayName.Length > 21)
        {
            Debug.Log("Name must be between 3 and 20 characters!");
            return;
        }
        foreach( char c in newDisplayName)
        {
            if (!char.IsNumber(c) && !char.IsLetter(c))
            {
                Debug.Log("Name must only contain letters or numbers!");
                return;
            }
        }
        RpcLogNewName(newDisplayName);

        SetDisplayName(newDisplayName);
    }  

#endregion

#region Client

    private void HandleDisplayColorUpdated(Color oldColor, Color newColor)
    {
        displayColorRenderer.material.SetColor("_Color", newColor);
    }

    private void HandleDisplayNameUpdated(string oldName, string newName)
    {
        displayNameText.text = newName; 
    }

    private void HandleTargetUpdated(GameObject oldTarget, GameObject newTarget)
    {
        target.transform.position = gameObject.transform.position;
        gameObject.GetComponent<AutoMove>().target = newTarget;   
    }

    private void HandlePositionUpdated(Vector3 oldPostion, Vector3 newPosition)
    {
        Debug.Log(gameObject.GetComponent<AutoMove>().target);
        gameObject.GetComponent<AutoMove>().target.transform.position = newPosition;   
    }

    [ContextMenu("Set My Name")]
    private void SetMyName()
    {
        CmdSetDisplayName("");
    }

    [ClientRpc]
    private void RpcLogNewName(string newDisplayName)
    {
        Debug.Log(newDisplayName);
    }

#endregion
}


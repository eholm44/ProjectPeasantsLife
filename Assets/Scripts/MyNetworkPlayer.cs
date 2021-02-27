using UnityEngine;
using Mirror;
using TMPro;

public class MyNetworkPlayer : NetworkBehaviour
{
    [SerializeField] private TMP_Text displayNameText = null;
    [SerializeField] private Renderer displayColorRenderer = null;
    
    [SyncVar(hook=nameof(HandleDisplayNameUpdated))]
    [SerializeField]
    private string displayName = "MissingName";

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
    public void SetDisplayColor(Color newDisplayColor)
    {
        displayColor = newDisplayColor;
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
        displayColorRenderer.material.color = newColor;
    }

    private void HandleDisplayNameUpdated(string oldName, string newName)
    {
        displayNameText.text = newName; 
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


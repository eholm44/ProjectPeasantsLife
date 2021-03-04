using UnityEngine;
using Mirror;
using TMPro;
using System.Collections.Generic;

public class MyNetworkPlayer : NetworkBehaviour
{
    
    [SerializeField] private TMP_Text displayNameText = null;
    [SerializeField] private Renderer displayColorRenderer = null;
    
    [SyncVar(hook=nameof(HandleDisplayNameUpdated))]
    public string displayName = "MissingName";

    [SyncVar(hook=nameof(HandleDisplayColorUpdated))]
    [SerializeField]
    private Color displayColor = Color.black;

    [SyncVar(hook=nameof(HandleMiniGameUpdated))]
    [SerializeField]
    public MiniGame miniGame;
    private Camera mainCamera = null;

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

        SetDisplayName(newDisplayName);
    }

    [Command]
    public void CmdLeavegame(GameObject localPlayer)
    {
        miniGame.LeaveGame(localPlayer);   
    }  

#endregion

#region Client

    public void LeaveGame(GameObject localPlayer)
    {
        
        CmdLeavegame(localPlayer);
        //TODO ERROR HERE
         if (isLocalPlayer)
        {
            mainCamera.gameObject.SetActive(true);
            miniGame.gameCamera.SetActive(false);
            miniGame.leaveButton.SetActive(false);
        }
        miniGame = null;
        
        RpcUpdateMiniGame(miniGame);
    }

    private void HandleDisplayColorUpdated(Color oldColor, Color newColor)
    {
        displayColorRenderer.material.color = newColor;
    }

    private void HandleDisplayNameUpdated(string oldName, string newName)
    {
        displayNameText.text = newName; 
    }

    private void HandleMiniGameUpdated(MiniGame oldMiniGame, MiniGame newMiniGame)
    {
        miniGame = newMiniGame; 
    }

    [ContextMenu("Set My Name")]
    private void SetMyName()
    {
        CmdSetDisplayName("");
    }

    public override void OnStartClient()
    {
        if (isLocalPlayer)
        {
            GameObject.Find("UIManager").GetComponent<UIManager>().localPlayer = gameObject;
        }
        mainCamera = Camera.main;
    }

    [ClientRpc]
    public void RpcUpdateMiniGame(MiniGame newMiniGame)
    {
        miniGame = newMiniGame;
    }

    

#endregion
}


using UnityEngine;
using Mirror;
using TMPro;

public class MyNetworkPlayer : NetworkBehaviour
{
    [SerializeField] private TMP_Text displayNameText = null;
    [SerializeField] private Renderer displayColorRenderer = null;
    
    [SyncVar(hook=nameof(HandleDisplayNameUpdated))]
    public string displayName = "MissingName";

    [SyncVar(hook=nameof(HandleDisplayColorUpdated))]
    [SerializeField]
    private Color displayColor = Color.black;

    private GameObject playerManager = null;

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

    [Command]
    public void CmdLeavegame()
    {
        Debug.Log("Here1");
        MiniGame miniGame = playerManager.GetComponent<LocalPlayerManager>().currentMinigame.GetComponent<MiniGame>();
        Debug.Log("Here2");
        miniGame.LeaveGame(displayName);
        Debug.Log("Here3");
        gameObject.GetComponent<MyPlayerMovement>().canMove = true;

        
        miniGame.mainCamera.gameObject.SetActive(true);
        miniGame.gameCamera.SetActive(false);
        miniGame.leaveButton.SetActive(false);
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

    public override void OnStartClient()
    {
        playerManager = GameObject.Find("PlayerManager");
        playerManager.GetComponent<LocalPlayerManager>().localPlayer = gameObject;
    }

#endregion
}


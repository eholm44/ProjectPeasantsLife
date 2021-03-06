using UnityEngine;
using Mirror;
using TMPro;

public class MyNetworkPlayer : Targetable
{
    [SerializeField] private TMP_Text displayNameText = null;
    [SerializeField] private Renderer displayColorRenderer = null;

    [SyncVar(hook=nameof(HandleDisplayNameUpdated))]
    public int displayName;

    [SyncVar(hook=nameof(HandleDisplayColorUpdated))]
    [SerializeField]
    private Color displayColor = Color.black;
    private Camera mainCamera = null;

    

#region Server

    [Server]
    public void SetDisplayName(int newDisplayName)
    {
        displayName = newDisplayName;
    }

    [Server]
    public void SetDisplayColor(Color newDisplayColor)
    {
        displayColor = newDisplayColor;
    }

    [Command]
    private void CmdSetDisplayName(int newDisplayName)
    {
        SetDisplayName(newDisplayName);
    }

    [Command]
    public void CmdLeaveGame(MiniGame miniGame, GameObject gamePlayer)
    {
        miniGame.LeaveGame(gamePlayer);
    }  

    [Command]
    public void CmdJoinGame(MiniGame miniGame, GameObject newPlayer)
    {
        miniGame.JoinGame(newPlayer);
    }

    [Command]
    public void CmdTryPlay(MiniGame miniGame, string transformName, string playerName)
    {
        miniGame.AddPiece(transformName, playerName);
    }

#endregion

#region Client

    private void HandleDisplayColorUpdated(Color oldColor, Color newColor)
    {
        displayColorRenderer.material.color = newColor;
    }

    private void HandleDisplayNameUpdated(int oldName, int newName)
    {
        displayNameText.text = newName.ToString(); 
    }

    public override void OnStartClient()
    {
        if (isLocalPlayer)
        {
            GameObject.Find("UIManager").GetComponent<UIManager>().localPlayer = gameObject;
        }
        mainCamera = Camera.main;
    }

    

#endregion
}


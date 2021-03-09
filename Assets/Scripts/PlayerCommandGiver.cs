using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCommandGiver : NetworkBehaviour
{
    private Camera mainCamera;
    [SerializeField] private UIManager uIManager = null;

    private MiniGame currentGame;

    private bool gameStarted;

    private bool canMove;

    private void Start()
    {
        gameStarted = false;
        mainCamera = Camera.main;

        uIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        canMove = true;
    }

    private void Update()
    {
        if(currentGame != null && currentGame.numPlayers == currentGame.maxPlayers)
        {
            gameStarted = true;
        }
        if(!gameStarted)
        {
            TryClick();
        }
        else
        {
            TryGameClick();
        }
    }

    public MiniGame GetMiniGame()
    {
        return currentGame;
    }

    public void TryMove(Vector3 point)
    {
        
        if(!canMove) {return;}
        GetComponent<MyPlayerMovement>().CmdSetPosition(point);
    }

    private void TryTarget(Targetable target)
    {
        if(!hasAuthority){return;}
        GetComponent<Targeter>().CmdSetTarget(target.gameObject);
    }

    private void TryJoinGame(MiniGame miniGame)
    {
        if(!hasAuthority){return;}
        canMove = false;
        miniGame.gameCamera.SetActive(true);
        mainCamera.gameObject.SetActive(false);
        uIManager.ActivateLeave();

        GetComponent<MyNetworkPlayer>().CmdJoinGame(miniGame, gameObject);
    }

    public void TryLeaveGame()
    {
        if(!hasAuthority){return;}
        GetComponent<Targeter>().ClearTarget();
        currentGame.gameCamera.SetActive(false);
        mainCamera.gameObject.SetActive(true);
        uIManager.DeactivateLeave();
        GetComponent<MyNetworkPlayer>().CmdLeaveGame(currentGame, gameObject);
        canMove = true;
        gameStarted = false;
    }

    private void TryPlay(string transformName)
    {
        GetComponent<MyNetworkPlayer>().CmdTryPlay(currentGame, transformName, gameObject.GetComponentInChildren<TMP_Text>().text);
    }

    private void TryGameClick()
    {
        if(Mouse.current.leftButton.wasPressedThisFrame)
        {
            RaycastHit[] hits;
            Ray ray = currentGame.gameCamera.GetComponent<Camera>().ScreenPointToRay(Mouse.current.position.ReadValue());
            hits = Physics.RaycastAll(currentGame.gameCamera.transform.position, ray.direction, Mathf.Infinity);

            foreach(RaycastHit h in hits)
            {
                if(h.collider.CompareTag("GamePoint"))
                {
                    if(!hasAuthority){return;}
                    TryPlay(h.collider.transform.name);
                }
            }
        }
    }

    private void TryClick()
    {
        if(!hasAuthority){return;}
        if(Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            //Check if clicking viable screen loacation
            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                return;
            }

            if(hit.collider.TryGetComponent<Targetable>(out Targetable target))
            {
                GetComponent<MyPlayerMovement>().CmdSetDistance(2f);
                if (hit.collider.CompareTag("MiniGame"))
                {
                    MiniGame miniGame = hit.collider.gameObject.GetComponent<MiniGame>();

                    //Test if can join minigame
                    string[] players = miniGame.players;
                    //Test if already in the game
                    foreach(string p in players)
                    {
                        if (p != null && p.Equals(GetComponent<MyNetworkPlayer>().displayName))
                        {
                            return;
                        }
                    }

                    //Test if game is full
                    if (miniGame.numPlayers == miniGame.maxPlayers)
                    {
                        return;
                    }
                    currentGame = miniGame;
                    TryJoinGame(miniGame);
                    gameStarted = true;
                }
                
                TryTarget(target);
                TryMove(hit.point);
                return;
            }
            GetComponent<MyPlayerMovement>().CmdSetDistance(.2f);
            TryMove(hit.point);
        }
    }
}

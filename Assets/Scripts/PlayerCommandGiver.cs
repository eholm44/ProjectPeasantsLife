using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCommandGiver : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] private UIManager uIManager = null;

    private MiniGame currentGame;

    private bool canMove;

    private void Start()
    {
        mainCamera = Camera.main;

        uIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        canMove = true;
    }

    private void Update()
    {
        if(!canMove) {return;}
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
                    if(Vector3.Distance(gameObject.transform.position,hit.transform.position) > 5f)
                    {
                        return;
                    }
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
                }
                
                TryTarget(target);
                TryMove(hit.point);
                return;
            }
            GetComponent<MyPlayerMovement>().CmdSetDistance(.2f);

            TryMove(hit.point);
        }
    }

    public MiniGame GetMiniGame()
    {
        return currentGame;
    }

    public void TryMove(Vector3 point)
    {
        GetComponent<MyPlayerMovement>().CmdSetPosition(point);
    }

    private void TryTarget(Targetable target)
    {
        GetComponent<Targeter>().CmdSetTarget(target.gameObject);
    }

    private void TryJoinGame(MiniGame miniGame)
    {
        canMove = false;
        miniGame.gameCamera.SetActive(true);
        mainCamera.gameObject.SetActive(false);
        uIManager.ActivateLeave();

        GetComponent<MyNetworkPlayer>().CmdJoinGame(miniGame, gameObject);
    }

    public void TryLeaveGame()
    {
        GetComponent<Targeter>().ClearTarget();
        currentGame.gameCamera.SetActive(false);
        mainCamera.gameObject.SetActive(true);
        uIManager.DeactivateLeave();

        GetComponent<MyNetworkPlayer>().CmdLeaveGame(currentGame, gameObject);
        canMove = true;
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{   
    public GameObject localPlayer;
    [SerializeField] private GameObject leaveMiniGame;
    [SerializeField] private GameObject settingsButton;
    [SerializeField] private GameObject menuPanel;
    public void LeaveGame()
    {
        localPlayer.GetComponent<PlayerCommandGiver>().TryLeaveGame();
    }

    public void ActivateLeave()
    {
        leaveMiniGame.SetActive(true);
    }

    public void ActivateMenu()
    {
        menuPanel.SetActive(true);
    }

    public void DeactivateLeave()
    {
        leaveMiniGame.SetActive(false);
    }
}

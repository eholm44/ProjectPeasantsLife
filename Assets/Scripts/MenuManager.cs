using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MenuManager : NetworkManager
{
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject createPanel;
    [SerializeField] GameObject loginPanel;
    [SerializeField] GameObject registerPanel;
    [SerializeField] GameObject yourWorldsPanel;
    [SerializeField] GameObject joinPanel;
    [SerializeField] GameObject exitButton;

    public void ActivateMain()
    {
        mainPanel.SetActive(true);
        createPanel.SetActive(false);
        loginPanel.SetActive(false);
        registerPanel.SetActive(false);
        yourWorldsPanel.SetActive(false);
        joinPanel.SetActive(false);
    }

    public void ActivateCreate()
    {
        mainPanel.SetActive(false);
        createPanel.SetActive(true);
        loginPanel.SetActive(false);
        registerPanel.SetActive(false);
        yourWorldsPanel.SetActive(false);
        joinPanel.SetActive(false);
    }

    public void ActivateLogin()
    {
        mainPanel.SetActive(false);
        createPanel.SetActive(false);
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
        yourWorldsPanel.SetActive(false);
        joinPanel.SetActive(false);
    }

    public void ActivateRegister()
    {
        mainPanel.SetActive(false);
        createPanel.SetActive(false);
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
        yourWorldsPanel.SetActive(false);
        joinPanel.SetActive(false);
    }

    public void ActivateWorlds()
    {
        mainPanel.SetActive(false);
        createPanel.SetActive(false);
        loginPanel.SetActive(false);
        registerPanel.SetActive(false);
        yourWorldsPanel.SetActive(true);
        joinPanel.SetActive(false);
    }

    public void ActivateJoin()
    {
        mainPanel.SetActive(false);
        createPanel.SetActive(false);
        loginPanel.SetActive(false);
        registerPanel.SetActive(false);
        yourWorldsPanel.SetActive(false);
        joinPanel.SetActive(true);
    }

}

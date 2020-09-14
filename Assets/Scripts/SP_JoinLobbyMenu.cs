using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SP_JoinLobbyMenu : MonoBehaviour
{
    [SerializeField] private SP_NetworkManager networkManager = null;

    [Header("UI")]
    [SerializeField] private GameObject landingPagePanel = null;
    [SerializeField] private TMP_InputField ipAddressInputField = null;
    [SerializeField] private Button joinButton = null;

    private const string PlayerLastIPKey = "LastIP";

    private void OnEnable()
    {
        SP_NetworkManager.OnClientConnected += HandleClientConnected;
        SP_NetworkManager.OnClientDisconnected += HandleClientDisconnected;
        SetUpInputField();
    }

    private void SetUpInputField()
    {
        string defaultName;

        if (!PlayerPrefs.HasKey(PlayerLastIPKey)) 
        {
            defaultName = "localhost";
        }

        defaultName = PlayerPrefs.GetString(PlayerLastIPKey);

        ipAddressInputField.text = defaultName;

        SetIP(defaultName);
    }

    private void OnDisable()
    {
        SP_NetworkManager.OnClientConnected += HandleClientConnected;
        SP_NetworkManager.OnClientDisconnected += HandleClientDisconnected;
    }

    public void SetIP(string name)
    {
        joinButton.interactable = !string.IsNullOrEmpty(name);
    }

    public void JoinLobby()
    {
        string ipAddress = ipAddressInputField.text;

        PlayerPrefs.SetString(PlayerLastIPKey, ipAddress);

        networkManager.networkAddress = ipAddress;
        networkManager.StartClient();

        joinButton.interactable = false;
    }

    private void HandleClientConnected()
    {
        joinButton.interactable = true;

        //IF CONNECTED, DISABLES IP-ADDRESS-POPUP

        gameObject.SetActive(false);
        landingPagePanel.SetActive(false);
    }

    private void HandleClientDisconnected()
    {
        //HAPPENS WHEN CONNECTION FAILS

        joinButton.interactable = true;
    }
}

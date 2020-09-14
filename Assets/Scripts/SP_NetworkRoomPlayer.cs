using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SP_NetworkRoomPlayer : NetworkBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject lobbyUI = null;
    [SerializeField] private TMP_Text[] playerNameTexts = new TMP_Text[8];
    [SerializeField] private TMP_Text[] playerReadyTexts = new TMP_Text[8];
    [SerializeField] private Button startGameButton = null;

    [SerializeField] private Button isReadyButton = null;
    [SerializeField] private TMP_Text isReadyButtonText = null;

    [SyncVar(hook = nameof(HandleDisplayNameChanged))]
    public string DisplayName = "Lade...";
    [SyncVar(hook = nameof(HandleReadyStatusChanged))]
    public bool IsReady = false;

    private bool isLeader;
    public bool IsLeader
    {
        set
        {
            isLeader = value;
            startGameButton.gameObject.SetActive(value);
        }
    }

    private SP_NetworkManager room;
    private SP_NetworkManager Room
    {
        get
        {
            if(room != null) { return room; }
            return room = NetworkManager.singleton as SP_NetworkManager;
        }
    }

    public override void OnStartAuthority()
    {
        CmdSetDisplayName(SP_PlayerNameInput.DisplayName);

        lobbyUI.SetActive(true);
    }

    public override void OnStartClient()
    {
        Room.RoomPlayers.Add(this);

        UpdateDisplay();
    }

    [System.Obsolete]
    public override void OnNetworkDestroy()
    {
        Room.RoomPlayers.Remove(this);

        UpdateDisplay();
    }

    public void HandleReadyStatusChanged(bool oldValue, bool newValue) => UpdateDisplay();
    public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();

    public void UpdateDisplay()
    {
        if (!hasAuthority)
        {
            foreach(var player in Room.RoomPlayers)
            {
                if(player.hasAuthority)
                {
                    player.UpdateDisplay();
                    break;
                }
            }

            return;
        }

        for(int i = 0; i < playerNameTexts.Length; i++)
        {
            playerNameTexts[i].text = "Warte auf Spieler...";
            playerReadyTexts[i].text = string.Empty;
        }

        for(int i = 0; i < Room.RoomPlayers.Count; i++)
        {
            playerNameTexts[i].text = Room.RoomPlayers[i].DisplayName;
            playerReadyTexts[i].text = Room.RoomPlayers[i].IsReady ?
                "<color=green>Bereit</color>" :
                "<color=red>Nicht bereit</color>";
        }
    }

    public void HandleReadyToStart(bool readyToStart)
    {
        if(!isLeader) { return; }

        startGameButton.interactable = readyToStart;
    }

    [Command]
    private void CmdSetDisplayName(string displayName)
    {
        DisplayName = displayName;
    }

    public void ReadyUp()
    {
        if (!IsReady)
        {
            isReadyButton.image.color = Color.gray;
            isReadyButtonText.text = "Nicht bereit?";
        }
        else
        {
            isReadyButton.image.color = Color.green;
            isReadyButtonText.text = "Bereit?";
        }

        CmdReadyUp();
    }

    [Command]
    public void CmdReadyUp()
    {
        IsReady = !IsReady;

        Room.NotifyPlayersOfReadyState();
    }

    [Command]
    public void CmdStartGame()
    {
        if (Room.RoomPlayers[0].connectionToClient != connectionToClient) { return; }

        Room.StartGame();
    }
}

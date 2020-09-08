using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SP_NetworkGamePlayer : NetworkBehaviour
{
    [SyncVar]
    private string displayName = "Lade...";

    private SP_NetworkManager room;
    private SP_NetworkManager Room
    {
        get
        {
            if(room != null) { return room; }
            return room = NetworkManager.singleton as SP_NetworkManager;
        }
    }

    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject);

        Room.GamePlayers.Add(this);
    }

    [System.Obsolete]
    public override void OnNetworkDestroy()
    {
        Room.GamePlayers.Remove(this);
    }

    [Server]
    public void SetDisplayName(string displayName)
    {
        this.displayName = displayName;
    }
}

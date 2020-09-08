using UnityEngine;

public class SP_MainMenu : MonoBehaviour
{
    [SerializeField] private SP_NetworkManager networkManager = null;

    [Header("UI")]
    [SerializeField] private GameObject landingPagePanel = null;

    public void HostLobby()
    {
        networkManager.StartHost();

        landingPagePanel.SetActive(false);
    }
}

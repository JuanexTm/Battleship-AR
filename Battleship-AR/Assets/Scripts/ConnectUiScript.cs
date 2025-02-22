using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ConnectUiScript : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;

    void Start()
    {
        hostButton.onClick.AddListener(HostButtonClick);
        clientButton.onClick.AddListener(ClientButtonClick);
    }

    private void HostButtonClick()
    {
        Debug.Log("Host button clicked");
        NetworkManager.Singleton.StartHost();
    }

    private void ClientButtonClick()
    {
        Debug.Log("Client button clicked");
        NetworkManager.Singleton.StartClient();
    }
}

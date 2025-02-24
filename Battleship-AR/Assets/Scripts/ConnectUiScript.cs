using System.Net;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;


public class ConnectUiScript : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;


    public NetworkManager networkManager;
    public TMP_InputField ipInputField; // Para que el cliente ingrese la IP
    public TMPro.TextMeshProUGUI ipDisplay; // Para mostrar la IP del host

    void Start()
    {
        hostButton.onClick.AddListener(HostButtonClick);
        clientButton.onClick.AddListener(ClientButtonClick);
        if (networkManager == null)
            networkManager = GetComponent<NetworkManager>();

        string localIP = GetLocalIPAddress();
        if (ipDisplay != null)
            ipDisplay.text = "Tu IP: " + localIP;
        

    }



    private void HostButtonClick()
    {
        Debug.Log("Host button clicked");
        StartAsHost();
    }

    private void ClientButtonClick()
    {
        Debug.Log("Client button clicked");
        ConnectToServer();
    }



    public void StartAsHost()
    {
        string localIP = GetLocalIPAddress();
        networkManager.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>().ConnectionData.Address = localIP;
        ipDisplay.text = localIP;
        networkManager.StartHost();
    }

    public void ConnectToServer()
    {
        // Obtén el transport y cambia la IP
        string ipIngresada = ipInputField.text; // Cliente ingresa la IP manualmente
        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.ConnectionData.Address = ipIngresada;
        transport.ConnectionData.Port = 7777; // Asegúrate de que el puerto es el mismo en host y cliente

        Debug.Log("Intentando conectar a: " + ipIngresada + ":" + transport.ConnectionData.Port);

        NetworkManager.Singleton.StartClient();
    }

   



    private string GetLocalIPAddress()
    {
        string localIP = "127.0.0.1";
        foreach (IPAddress ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }
        return localIP;
    }
}

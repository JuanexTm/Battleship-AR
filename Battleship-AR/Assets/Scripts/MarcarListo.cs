using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class MarcarListo : NetworkBehaviour
{
    Core core;
    public Color materialNoPresionable, materialPresionable, materialPresionado;
    bool listo;
    public GameObject uiInGame;


    private void Start()
    {
        core = GetComponentInParent<Core>();
        uiInGame.SetActive(false);
    }

    public void OnListo()
    {
        if (core.barcosPosicionados)
        {
            if(!listo)
            {
                listo = true;
                GameManagerNetwork.Instance.MarcarListoServerRpc(NetworkManager.Singleton.LocalClientId, true);
                GetComponent<Image>().color = materialPresionado;

            }
            else
            {
                listo = false;
                GameManagerNetwork.Instance.MarcarListoServerRpc(NetworkManager.Singleton.LocalClientId, false);
                GetComponent<Image>().color = materialPresionable;
            }
        }
        else
        {
            GameManagerNetwork.Instance.MarcarListoServerRpc(NetworkManager.Singleton.LocalClientId, false);
            GetComponent<Image>().color = materialNoPresionable;
        }
    }
    private void Update()
    {
        if (core.barcosPosicionados && !listo)
        {
            GetComponent<Image>().color = materialPresionable;
        }
        else if(!listo)
        {
            GetComponent<Image>().color = materialNoPresionable;
        }


        if (GameManagerNetwork.Instance.partidaIniciada.Value)
        {
            gameObject.SetActive(false);
            uiInGame.SetActive(true);

        }
    }
}

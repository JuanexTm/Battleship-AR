using Unity.Netcode;
using UnityEngine;

public class MarcarListo : NetworkBehaviour
{
    Core core;
    public Material materialNoPresionable, materialPresionable, materialPresionado;
    bool listo;

    private void Start()
    {
        core = GetComponentInParent<Core>();
    }

    private void OnMouseDown()
    {
        if (core.barcosPosicionados)
        {
            if(!listo)
            {
                listo = true;
                GameManagerNetwork.Instance.MarcarListoServerRpc(NetworkManager.Singleton.LocalClientId, true);
                GetComponent<Renderer>().material = materialPresionado;

            }
            else
            {
                listo = false;
                GameManagerNetwork.Instance.MarcarListoServerRpc(NetworkManager.Singleton.LocalClientId, false);
                GetComponent<Renderer>().material = materialPresionable;
            }
        }
        else
        {
            GameManagerNetwork.Instance.MarcarListoServerRpc(NetworkManager.Singleton.LocalClientId, false);
            GetComponent<Renderer>().material = materialNoPresionable;
        }
    }
    private void Update()
    {
        if (core.barcosPosicionados && !listo)
        {
            GetComponent<Renderer>().material = materialPresionable;
        }
        else if(!listo)
        {
            GetComponent<Renderer>().material = materialNoPresionable;
        }


        if (core.partidaComenzada)
        {
            Destroy(gameObject);
        }
    }
}

using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmarAtaque : MonoBehaviour
{
    Core core;
    public Color materialRojo, materialConfirmacion;
    bool presionable;
    private void Start()
    {
        GetComponent<Image>().color = materialRojo;
        core = GetComponentInParent<Core>();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(core.casilla != null)
        {
            presionable = true;
            GetComponent<Image>().color = materialConfirmacion;
        }
        else
        {
            presionable = false;
            GetComponent<Image>().color = materialRojo;
        }
    }

    public void OnConfirmar()
    {
        if(presionable)
        {
            int casillaDeAtaque = Convert.ToInt32(core.casilla.gameObject.name);
            GameManagerNetwork.Instance.RevisarAtaqueServerRpc(core.jugador, casillaDeAtaque);
            core.RevisarAtaque();
            

        }
    }
}

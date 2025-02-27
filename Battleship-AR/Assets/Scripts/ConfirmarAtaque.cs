using System;
using Unity.VisualScripting;
using UnityEngine;

public class ConfirmarAtaque : MonoBehaviour
{
    Core core;
    public Material materialRojo, materialConfirmacion;
    bool presionable;
    private void Start()
    {
        GetComponent<MeshRenderer>().material = materialRojo;
        core = GetComponentInParent<Core>();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(core.casilla != null)
        {
            presionable = true;
            GetComponent<MeshRenderer>().material = materialConfirmacion;
        }
        else
        {
            presionable = false;
            GetComponent<MeshRenderer>().material = materialRojo;
        }
    }

    private void OnMouseDown()
    {
        if(presionable)
        {
            int casillaDeAtaque = Convert.ToInt32(core.casilla.gameObject.name);
            GameManagerNetwork.Instance.RevisarAtaqueServerRpc(core.jugador -1, casillaDeAtaque);
            if(core.jugador == 1)
            {
                if (GameManagerNetwork.Instance.casillaAtacadaJugador1[0] == casillaDeAtaque)
                {
                    if(GameManagerNetwork.Instance.casillaAtacadaJugador1[1] == 0)
                    {
                        core.casilla.GetComponent<Casilla>().MarcarDaño(false);
                    }
                    else
                    {
                        core.casilla.GetComponent<Casilla>().MarcarDaño(true);
                    }
                }
            }
            else if(core.jugador == 2)
            {
                if (GameManagerNetwork.Instance.casillaAtacadaJugador2[0] == casillaDeAtaque)
                {
                    if (GameManagerNetwork.Instance.casillaAtacadaJugador2[1] == 0)
                    {
                        core.casilla.GetComponent<Casilla>().MarcarDaño(false);
                    }
                    else
                    {
                        core.casilla.GetComponent<Casilla>().MarcarDaño(true);
                    }
                }
            }

            core.casilla = null;
            GameManagerNetwork.Instance.LimpiarAtaquesServerRpc();
            

        }
    }
}

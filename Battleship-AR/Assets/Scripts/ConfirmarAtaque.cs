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
            GameManagerNetwork.Instance.RevisarAtaqueServerRpc(core.jugador, casillaDeAtaque);
            core.RevisarAtaque();
            

        }
    }
}

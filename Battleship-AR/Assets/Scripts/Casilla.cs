using System;
using TMPro;
using UnityEngine;

public class Casilla : MonoBehaviour
{
    Core core;
    Posiciones posiciones;
    public bool CasillaSeleccionada;
    public Mesh circulo, equis;
    public bool atacada;

    private void Start()
    {
        core = GetComponentInParent<Core>();
        posiciones = GetComponentInParent<Posiciones>();
    }
    private void OnMouseDown()
    {
        Debug.Log("Casilla presionada        partida inciada: " + GameManagerNetwork.Instance.partidaIniciada.Value +  "    Turno?: " + core.enTurno);
        if(core.barcoSeņalado != null && !GameManagerNetwork.Instance.partidaIniciada.Value)
        {
            int posicion = Convert.ToInt32(gameObject.name);
            Barco barco = core.barcoSeņalado.GetComponent<Barco>();
            if (posiciones.RevisarSiSePuedePonerBarco(posicion, barco.tamaņoDeBarco, barco.horizontal))
            {
                posiciones.ColocarBarco(core.barcoSeņalado, posicion);
                barco.enTablero = true;
            }
        }
        else if(GameManagerNetwork.Instance.partidaIniciada.Value && core.enTurno && core.ataqueDefensa.viendoEnemigo)
        {
            
            GetComponent<MeshRenderer>().enabled = true;
            core.casilla = GetComponent<Casilla>();
        }
    }

    public void MarcarDaņo(bool daņo)
    {
        if (daņo)
        {
            core.puntaje++;
            if (core.puntaje == 19)
            {
                GameManagerNetwork.Instance.WinStateServerRpc(core.jugador);
            }
        }
        atacada = true;
        GetComponent<MeshRenderer>().enabled = true;

        if (daņo) GetComponent<MeshFilter>().mesh = equis;
        else GetComponent<MeshFilter>().mesh = circulo;

        GetComponent<Collider>().enabled = false;
    }

    public void Explotar()
    {
        Debug.Log("Golpe recibido en la casilla " + gameObject.name);
    }
}

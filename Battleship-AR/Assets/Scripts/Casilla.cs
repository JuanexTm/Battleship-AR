using System;
using TMPro;
using UnityEngine;

public class Casilla : MonoBehaviour
{
    Core core;
    Posiciones posiciones;
    public bool CasillaSeleccionada;
    public Mesh circulo, equis;

    private void Start()
    {
        core = GetComponentInParent<Core>();
        posiciones = GetComponentInParent<Posiciones>();
    }
    private void OnMouseDown()
    {
        Debug.Log("Casilla presionada        partida inciada: " + GameManagerNetwork.Instance.partidaIniciada.Value +  "    Turno?: " + core.enTurno);
        if(core.barcoSeñalado != null && !GameManagerNetwork.Instance.partidaIniciada.Value)
        {
            int posicion = Convert.ToInt32(gameObject.name);
            Barco barco = core.barcoSeñalado.GetComponent<Barco>();
            if (posiciones.RevisarSiSePuedePonerBarco(posicion, barco.tamañoDeBarco, barco.horizontal))
            {
                posiciones.ColocarBarco(core.barcoSeñalado, posicion);
                barco.enTablero = true;
            }
        }
        else if(GameManagerNetwork.Instance.partidaIniciada.Value && core.enTurno && core.ataqueDefensa.viendoEnemigo)
        {
            
            GetComponent<MeshRenderer>().enabled = true;
            core.casilla = GetComponent<Casilla>();
        }
    }

    public void MarcarDaño(bool daño)
    {
        GetComponent<MeshRenderer>().enabled = true;

        if (daño) GetComponent<MeshFilter>().mesh = equis;
        else GetComponent<MeshFilter>().mesh = circulo;

        GetComponent<Collider>().enabled = false;
        enabled = false;
    }
}

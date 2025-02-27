using System;
using TMPro;
using UnityEngine;

public class Casilla : MonoBehaviour
{
    Core core;
    Posiciones posiciones;
    public bool CasillaSeleccionada;

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
        else if(GameManagerNetwork.Instance.partidaIniciada.Value && core.enTurno)
        {
            
            GetComponent<MeshRenderer>().enabled = true;
            core.casilla = GetComponent<Casilla>();
        }
    }
}

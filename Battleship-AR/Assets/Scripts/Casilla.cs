using System;
using TMPro;
using UnityEngine;

public class Casilla : MonoBehaviour
{
    Core core;
    Posiciones posiciones;

    private void Start()
    {
        core = GameObject.FindGameObjectWithTag("Tablero").GetComponent<Core>();
        posiciones = GameObject.FindGameObjectWithTag("Tablero").GetComponent<Posiciones>();
    }
    private void OnMouseDown()
    {
        if(core.barcoSeņalado != null)
        {
            int posicion = Convert.ToInt32(gameObject.name);
            Barco barco = core.barcoSeņalado.GetComponent<Barco>();
            if (posiciones.RevisarSiSePuedePonerBarco(posicion, barco.tamaņoDeBarco, barco.horizontal))
            {
                posiciones.ColocarBarco(core.barcoSeņalado, posicion);
            }
        }
    }
}

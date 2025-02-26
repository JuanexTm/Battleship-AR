using System;
using TMPro;
using UnityEngine;

public class Casilla : MonoBehaviour
{
    Core core;
    Posiciones posiciones;

    private void Start()
    {
        core = GetComponentInParent<Core>();
        posiciones = GetComponentInParent<Posiciones>();
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
                barco.enTablero = true;
            }
        }
    }
}

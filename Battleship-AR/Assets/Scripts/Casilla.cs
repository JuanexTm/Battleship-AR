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
        if(core.barcoSeñalado != null)
        {
            int posicion = Convert.ToInt32(gameObject.name);
            Barco barco = core.barcoSeñalado.GetComponent<Barco>();
            if (posiciones.RevisarSiSePuedePonerBarco(posicion, barco.tamañoDeBarco, barco.horizontal))
            {
                posiciones.ColocarBarco(core.barcoSeñalado, posicion);
                barco.enTablero = true;
            }
        }
    }
}

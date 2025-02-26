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
        if(core.barcoSe�alado != null)
        {
            int posicion = Convert.ToInt32(gameObject.name);
            Barco barco = core.barcoSe�alado.GetComponent<Barco>();
            if (posiciones.RevisarSiSePuedePonerBarco(posicion, barco.tama�oDeBarco, barco.horizontal))
            {
                posiciones.ColocarBarco(core.barcoSe�alado, posicion);
                barco.enTablero = true;
            }
        }
    }
}

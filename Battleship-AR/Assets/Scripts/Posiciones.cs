using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Posiciones : MonoBehaviour
{
    public List<Transform> posiciones;

    public List<int> posicionesOcupadas = new List<int>();

    Core core;



    private void Awake()
    {
        core = GetComponent<Core>();


        Transform[] todosLosHijos = GetComponentsInChildren<Transform>();

        for (int i = 0; i < 100; i++)
        {
            string name = i.ToString();
            foreach (Transform hijo in todosLosHijos)
            {
                if (hijo.name == name)
                {
                    posiciones.Add(hijo);
                    break; // Encontramos la posición, no hace falta seguir buscando
                }
            }
        }


    }

    public bool RevisarSiSePuedePonerBarco(int posicion, int tamañoBarco, bool horizontal)
    {

        int contador = 0;
        int casilla = posicion;
        int casillaAnterior = casilla;


        if (horizontal && casilla % 10 == 9) //Está a la derecha del todo
        {
            return false;
        }
        else if(!horizontal && casilla >= 90)//Está abajo del todo
        {
            return false;
        }
            
    
        
        while (contador < tamañoBarco)
        {
            if(horizontal && casillaAnterior % 10 == 9) //Se sale por la derecha
            {
                return false;
            }
            else if(!horizontal && casillaAnterior >= 90) // Se sale por debajo
            {
                return false;
            }
            
            if (posicionesOcupadas.Contains(casilla))
            {
                return false;
            }
            else
            {
                casillaAnterior = casilla;
                contador++;
                if(horizontal)
                {
                    casilla++;
                }
                else
                {
                    casilla += 10;
                }
            }
        }

        return true;

    }

    public void ColocarBarco(GameObject barco, int casillaDePosicion)
    {
        barco.transform.position = posiciones[casillaDePosicion].position;
        Barco barcoScript = barco.GetComponent<Barco>();

        barcoScript.casilla = casillaDePosicion;
        
        GestionarPosiciones(casillaDePosicion, barcoScript.tamañoDeBarco,barcoScript.horizontal, true); //OCUPAR EL ESPACIO

        core.barcoSeñalado = null;
        
    }

    public void GestionarPosiciones(int casillaDePosicion, int tamañoDelBarco, bool horizontal, bool ocupar)
    {

        int casilla = casillaDePosicion;
        for (int i = 0; i < tamañoDelBarco; i++)
        {
            if (ocupar)
            {
                if (!posicionesOcupadas.Contains(casilla))
                {
                    posicionesOcupadas.Add(casilla);
                }
            }
            else
            {
                posicionesOcupadas.Remove(casilla);
            }

            if (horizontal) casilla++;
            else casilla+=10;
        }
    }

    public void OnRotar()
    {
        GameObject barco = core.barcoSeñalado;
        Barco barcoScript = barco.GetComponent<Barco>();

        if (barco != null)
        {
            if (!barcoScript.posicionado)
            {
                if (barcoScript.horizontal)
                {
                    barcoScript.horizontal = false;
                }
                else
                {
                    barcoScript.horizontal = true;
                }
            }
            else if(RevisarSiSePuedePonerBarco(barcoScript.casilla,barcoScript.tamañoDeBarco, !barcoScript.horizontal))
                {
                if (barcoScript.horizontal)
                {
                    barcoScript.horizontal = false;
                }
                else
                {
                    barcoScript.horizontal = true;
                }

            }


        }
    }

    private void Update()
    {
        if (posicionesOcupadas.Count == 19)
        {
            core.barcosPosicionados = true;
        }
        else
        {
            core.barcosPosicionados = false;
        }
    }
}



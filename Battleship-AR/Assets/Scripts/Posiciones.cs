using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Posiciones : MonoBehaviour
{
    public List<Transform> posiciones;

    public List<int> posicionesOcupadas = new List<int>();

    Core core;
    bool posicionesRegistradas;



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
                    break; // Encontramos la posici�n, no hace falta seguir buscando
                }
            }
        }


    }

    public bool RevisarSiSePuedePonerBarco(int posicion, int tama�oBarco, bool horizontal)
    {

        int contador = 0;
        int casilla = posicion;
        int casillaAnterior = casilla;


        if (horizontal && casilla % 10 == 9) //Est� a la derecha del todo
        {
            return false;
        }
        else if(!horizontal && casilla >= 90)//Est� abajo del todo
        {
            return false;
        }
            
    
        
        while (contador < tama�oBarco)
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
        
        GestionarPosiciones(casillaDePosicion, barcoScript.tama�oDeBarco,barcoScript.horizontal, true); //OCUPAR EL ESPACIO

        core.barcoSe�alado = null;
        
    }

    public void GestionarPosiciones(int casillaDePosicion, int tama�oDelBarco, bool horizontal, bool ocupar)
    {

        int casilla = casillaDePosicion;
        for (int i = 0; i < tama�oDelBarco; i++)
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

        if (core.barcoSe�alado != null)
        {
            GameObject barco = core.barcoSe�alado;
            Barco barcoScript = barco.GetComponent<Barco>();
            

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
            else if(RevisarSiSePuedePonerBarco(barcoScript.casilla,barcoScript.tama�oDeBarco, !barcoScript.horizontal))
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
        if (!posicionesRegistradas && GameManagerNetwork.Instance.partidaIniciada.Value)
        {
            if(core.jugador== 1)
            {
                int[] posicionesOcupadasArreglo = new int[posicionesOcupadas.Count];
                int contador = 0;
                foreach (var item in posicionesOcupadas)
                {
                    posicionesOcupadasArreglo[contador] = item;
                    contador++;
                }
                GameManagerNetwork.Instance.RegistrarPosicionesJugador1ServerRpc(posicionesOcupadasArreglo);
            }
            else if(core.jugador == 2)
            {
                int[] posicionesOcupadasArreglo = new int[posicionesOcupadas.Count];
                int contador = 0;
                foreach (var item in posicionesOcupadas)
                {
                    posicionesOcupadasArreglo[contador] = item;
                    contador++;
                }
                GameManagerNetwork.Instance.RegistrarPosicionesJugador2ServerRpc(posicionesOcupadasArreglo);
            }
            posicionesRegistradas = true;
        }

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



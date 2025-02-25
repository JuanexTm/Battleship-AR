using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Posiciones : MonoBehaviour
{
    public List<Transform> posiciones;

    private List<int> posicionesOcupadas;



    private void Awake()
    {
        for (int i = 0; i < 100; i++)
        {
            string name = i.ToString();
            posiciones.Add(GameObject.Find(name).GetComponent<Transform>());

        }
    }

    public bool RevisarSiSePuedePonerBarco(int posicion, int tamañoBarco, bool horizontal)
    {

        int contador = 0;
        int casilla = posicion;
        int casillaAnterior = casilla;


        if (horizontal && casilla % 9 == 0) //Está a la derecha del todo
        {
            return false;
        }
        else if(!horizontal && casilla >= 90)//Está abajo del todo
        {
            return false;
        }
            
    
        
        while (contador < tamañoBarco)
        {
            if(horizontal && casillaAnterior % 9 == 0) //Se sale por la derecha
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

    public void GestionarPosiciones(int casillaDePosicion, int tamañoDelBarco, bool horizontal, bool ocupar)
    {

        int casilla = casillaDePosicion;
        for (int i = 0; i < tamañoDelBarco; i++)
        {
            if (ocupar)
            {
                posicionesOcupadas.Add(casilla);
            }
            else
            {
                posicionesOcupadas.Remove(casilla);
            }

            if (horizontal) casillaDePosicion++;
            else casillaDePosicion+=10;
        }
    }
}



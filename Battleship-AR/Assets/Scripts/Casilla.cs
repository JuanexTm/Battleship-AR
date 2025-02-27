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
    public GameObject explosion;
    public GameObject golpeErrado;
    MeshRenderer mr;
    bool errado;
    GameObject padre;

    bool explotado;

    private void Start()
    {
        core = GetComponentInParent<Core>();
        posiciones = GetComponentInParent<Posiciones>();
        mr = GetComponent<MeshRenderer>();

        padre = core.GetComponentInChildren<PadreHumo>().miniPadre;
    }

    private void Update()
    {
        if (!core.ataqueDefensa.viendoEnemigo)
        {
            mr.enabled = false;
        }
        else if(atacada)
        {
            mr.enabled = true;
        }
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
            
            mr.enabled = true;
            core.casilla = GetComponent<Casilla>();
        }
    }

    public void MarcarDaño(bool daño)
    {
        if (daño)
        {
            core.puntaje++;
            if (core.puntaje == 19)
            {
                GameManagerNetwork.Instance.WinStateServerRpc(core.jugador);
            }
        }
        atacada = true;
        mr.enabled = true;

        if (daño) GetComponent<MeshFilter>().mesh = equis;
        else GetComponent<MeshFilter>().mesh = circulo;

        GetComponent<Collider>().enabled = false;
    }

    public void Explotar()
    {
        if(explotado)
        {
            return;
        }
        else
        {

            Debug.Log("Golpe recibido en la casilla " + gameObject.name);
            Instantiate(explosion, transform.position, Quaternion.identity, padre.transform);
            explotado = true;
        }
    }

    public void Errado()
    {
        if(errado)
        {
            return;
        }
        else
        {

            Instantiate(golpeErrado, transform.position, Quaternion.identity);
            errado = true;
        }
    }
}

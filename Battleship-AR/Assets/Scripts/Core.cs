using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;


public class Core : NetworkBehaviour
{
    public TextMeshProUGUI estadoTexto;

    public GameObject barcoSeñalado;
    public float yOffSet;
    public float speed = 1;
    public int jugador;
    public Material material1, material2;
    public bool barcosPosicionados;
    public AtaqueDefensa ataqueDefensa;
    public GameObject botonAtaqueDefensa;
    public GameObject botonListo,botonRotar,botonConfirmarAtaque;
    public bool enTurno;
    bool botonesDesactivados;

    Posiciones posiciones;

    public AudioSource aS;
    public AudioClip audioExplosion, audioGotica;

    int golpes = -1;
    int errados = -1;

    public int puntaje;


    public Casilla casilla;
    Casilla casillaAnterior;

    private Material materialPredeterminado;



    private void Awake()
    {
        estadoTexto = GameObject.Find("Estado").GetComponent<TextMeshProUGUI>();
        posiciones = GetComponentInChildren<Posiciones>();
    }


    private void Start()
    {
        if(GameObject.Find("Tablero Host"))
        {
            gameObject.name = "Tablero Client";
            jugador = 2;
            GetComponent<Renderer>().material = material2;
            materialPredeterminado = material2;
        }
        else
        {
            gameObject.name = "Tablero Host";
            jugador = 1;
            GetComponent<Renderer>().material = material1;
            materialPredeterminado = material1;
        }
        if (NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer && jugador == 2)
        {
            GameManagerNetwork.Instance.RegistrarTableroServerRpc(NetworkManager.Singleton.LocalClientId, gameObject);
        }
        else if(NetworkManager.Singleton.IsServer && jugador == 1)
        {
            GameManagerNetwork.Instance.RegistrarTableroServerRpc(0, gameObject);
        }

        

        transform.SetParent(GameObject.Find("Tableros").transform);
        transform.localPosition = Vector3.zero;


        if (IsServer && gameObject.name == "Tablero Client") // Solo en el servidor, no en el host
        {
            transform.position = Vector3.one * 999;
            enabled = false;
        }

        if (IsClient && !IsServer && gameObject.name == "Tablero Host") // Solo en el cliente, no en el host
        {
            transform.position = Vector3.one * 999;
            enabled = false;
        }


    }



    private void Update()
    {

        if(jugador== 1)
        {
            if(golpes != GameManagerNetwork.Instance.golpesRecibidosJugador1.Value)
            {
                posiciones.posiciones[GameManagerNetwork.Instance.golpesRecibidosJugador1.Value].gameObject.GetComponent<Casilla>().Explotar();
            }

            if (errados != GameManagerNetwork.Instance.erradosRecibidosJugador1.Value)
            {
                posiciones.posiciones[GameManagerNetwork.Instance.erradosRecibidosJugador1.Value].gameObject.GetComponent<Casilla>().Errado();
            }

            errados = GameManagerNetwork.Instance.erradosRecibidosJugador1.Value;
            golpes = GameManagerNetwork.Instance.golpesRecibidosJugador1.Value;
            
        }
        else if(jugador == 2)
        {
            if (golpes != GameManagerNetwork.Instance.golpesRecibidosJugador1.Value)
            {
                posiciones.posiciones[GameManagerNetwork.Instance.golpesRecibidosJugador2.Value].gameObject.GetComponent<Casilla>().Explotar();
            }

            if (errados != GameManagerNetwork.Instance.erradosRecibidosJugador2.Value)
            {
                posiciones.posiciones[GameManagerNetwork.Instance.erradosRecibidosJugador2.Value].gameObject.GetComponent<Casilla>().Errado();
            }

            errados = GameManagerNetwork.Instance.erradosRecibidosJugador2.Value;
            golpes = GameManagerNetwork.Instance.golpesRecibidosJugador2.Value;
        }



        if (GameManagerNetwork.Instance.partidaIniciada.Value && !botonesDesactivados)
        {
            botonAtaqueDefensa.SetActive(true);
            botonListo.SetActive(false);
            botonRotar.SetActive(false);
            botonesDesactivados = true;

        }


        if (casilla != null && casillaAnterior == null)
        {
            casillaAnterior = casilla;
        }

        if(casilla != casillaAnterior)
        {
            if (!casillaAnterior.atacada)
            {
                casillaAnterior.GetComponent<MeshRenderer>().enabled = false;
            }
            casillaAnterior = casilla;
        }

        if(barcoSeñalado != null)
        {
            if (barcoSeñalado.transform.localPosition.y < barcoSeñalado.GetComponent<Barco>().alturaInicial + yOffSet)
            {
                barcoSeñalado.transform.localPosition += Vector3.up * speed * Time.deltaTime;
            }
        }

        if (GameManagerNetwork.Instance.partidaIniciada.Value)
        {
            if (jugador == 1)
            {
                enTurno = GameManagerNetwork.Instance.turnoJugador1.Value;
                estadoTexto.text = GameManagerNetwork.Instance.textoPlayer1.Value.ToString();
            }
            else
            {
                enTurno = !GameManagerNetwork.Instance.turnoJugador1.Value;
                estadoTexto.text = GameManagerNetwork.Instance.textoPlayer2.Value.ToString();
            }

            if (!IsServer)
            {

                
            }

            if (ataqueDefensa.viendoEnemigo)
            {
                if (materialPredeterminado == material1)
                {
                    GetComponent<Renderer>().material = material2;
                }
                else if (materialPredeterminado == material2)
                {
                    GetComponent<Renderer>().material = material1;
                }
            }
            else
            {
                GetComponent<Renderer>().material = materialPredeterminado;
            }

            if(ataqueDefensa.viendoEnemigo && enTurno)
            {

                botonConfirmarAtaque.SetActive(true);
            }
            else
            {
                botonConfirmarAtaque.SetActive(false);
            }

            
        }


    }

    public void RevisarAtaque()
    {
        StartCoroutine(EsperarYRevisarAtaque());
    }

    public IEnumerator EsperarYRevisarAtaque()
    {
        yield return new WaitForSeconds(0.6f);
        if (jugador == 2)
        {
            Debug.Log("Se presionó");
            if (GameManagerNetwork.Instance.casillaAtacadaJugador2Int.Value == 0)
            {
                casilla.GetComponent<Casilla>().MarcarDaño(false);
            }
            else
            {
                casilla.GetComponent<Casilla>().MarcarDaño(true);
            }
        }
        else if (jugador == 1)
        {
            if (GameManagerNetwork.Instance.casillaAtacadaJugador1Int.Value == 0)
            {
                casilla.GetComponent<Casilla>().MarcarDaño(false);
            }
            else
            {
                casilla.GetComponent<Casilla>().MarcarDaño(true);
            }
        }

        casilla = null;
    }




}

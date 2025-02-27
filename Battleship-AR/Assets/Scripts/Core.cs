using System.Linq;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

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


    public Casilla casilla;
    Casilla casillaAnterior;

    private Material materialPredeterminado;



    private void Awake()
    {
        estadoTexto = GameObject.Find("Estado").GetComponent<TextMeshProUGUI>();
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
            casillaAnterior.GetComponent<MeshRenderer>().enabled = false;
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

    





}

using System.Linq;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Core : NetworkBehaviour
{
    public GameObject barcoSeñalado;
    public float yOffSet;
    public float speed = 1;
    public int jugador;
    public Material material1, material2;
    public bool barcosPosicionados;
    public AtaqueDefensa ataqueDefensa;
    public GameObject botonAtaqueDefensa;
    public GameObject botonListo,botonRotar;
    public bool enTurno;

    public Casilla casilla;
    Casilla casillaAnterior;

    private Material materialPredeterminado;

    public NetworkVariable<bool> cambiosImportantes = new NetworkVariable<bool>(
        false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server
    );



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

        cambiosImportantes.OnValueChanged += (antes, despues) =>
        {
            if (despues)
            {
                AplicarCambiosClientRpc();
            }
        };

        transform.SetParent(GameObject.Find("Tableros").transform);
        transform.localPosition = Vector3.zero;


        if (IsServer && gameObject.name == "Tablero Client") // Solo en el servidor, no en el host
        {
            transform.position = Vector3.one * 999;
        }

        if (IsClient && !IsServer && gameObject.name == "Tablero Host") // Solo en el cliente, no en el host
        {
            transform.position = Vector3.one * 999;
        }


    }


    public override void OnNetworkSpawn()
    {
        cambiosImportantes.OnValueChanged += (oldValue, newValue) =>
        {
            if (newValue)
            {
                AplicarCambiosClientRpc();
            }
        };
    }

    private void Update()
    {
        
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
            }
            else
            {
                enTurno = !GameManagerNetwork.Instance.turnoJugador1.Value;
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
        }


        







    }

    [ClientRpc]
    private void AplicarCambiosClientRpc()
    {
        Debug.Log("Aplicando cambios importantes en Core...");

        if (GameManagerNetwork.Instance.partidaIniciada.Value)
        {
            botonAtaqueDefensa.SetActive(true);
            botonListo.SetActive(false);
            botonRotar.SetActive(false); Debug.Log("Rotar desactivado");
        }


        

        cambiosImportantes.Value = false; // Resetear después de aplicar los cambios
    }





}

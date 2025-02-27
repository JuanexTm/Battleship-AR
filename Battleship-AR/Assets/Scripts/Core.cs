using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class Core : MonoBehaviour
{
    public GameObject barcoSeñalado;
    public float yOffSet;
    public float speed = 1;
    public int jugador;
    public Material material1, material2;
    public bool barcosPosicionados;
    public bool enTurno;
    public bool partidaComenzada;
    public AtaqueDefensa ataqueDefensa;
    public GameObject botonAtaqueDefensa;
    public GameObject botonListo,botonRotar;

    private Material materialPredeterminado;

    private void Start()
    {
        if (NetworkManager.Singleton.IsClient)
        {
            GameManagerNetwork.Instance.RegistrarTableroServerRpc(NetworkManager.Singleton.LocalClientId, gameObject);
        }

        transform.SetParent(GameObject.Find("Tableros").transform);
        transform.localPosition = Vector3.zero;
        if (GameObject.FindGameObjectsWithTag("Tablero").Length > 1)
        {
            jugador = 2;
            GetComponent<Renderer>().material = material2;
            materialPredeterminado = material2;
        }
        else
        {
            jugador = 1;
            GetComponent<Renderer>().material = material1;
            materialPredeterminado = material1;
        }

        
    }

    private void Update()
    {
        if(barcoSeñalado != null)
        {
            if (barcoSeñalado.transform.localPosition.y < barcoSeñalado.GetComponent<Barco>().alturaInicial + yOffSet)
            {
                barcoSeñalado.transform.localPosition += Vector3.up * speed * Time.deltaTime;
            }
        }

        if (partidaComenzada)
        {
            botonAtaqueDefensa.SetActive(true);
            botonListo.SetActive(false);
            botonRotar.SetActive(false);
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

    


}

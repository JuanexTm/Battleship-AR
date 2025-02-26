using System.Linq;
using UnityEngine;

public class Core : MonoBehaviour
{
    public GameObject barcoSeñalado;
    public float yOffSet;
    public float speed = 1;
    public int jugador;
    public Material material1, material2;

    private void Start()
    {
        Debug.Log("YA DESPERTE DEL SUEÑO");
        transform.SetParent(GameObject.Find("Tableros").transform);
        transform.localPosition = Vector3.zero;
        if (GameObject.FindGameObjectsWithTag("Tablero").Length > 1)
        {
            jugador = 2;
            GetComponent<Renderer>().material = material2;
        }
        else
        {
            jugador = 1;
            GetComponent<Renderer>().material = material1;
            transform.localPosition += Vector3.forward * 0.04f;
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
        
    }

    


}

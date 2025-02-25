using UnityEngine;

public class Barco : MonoBehaviour
{
    public bool posicionado;
    public int casilla;
    public int tamañoDeBarco;
    public bool horizontal;

    private float alturaInicial;

    bool fueSeñalado;


    Core core;
    Posiciones posiciones;

    private void Start()
    {
        core = GameObject.FindGameObjectWithTag("Tablero").GetComponent<Core>();
        posiciones = GameObject.FindGameObjectWithTag("Tablero").GetComponent<Posiciones>();
        alturaInicial = transform.position.y;
    }



    public void Update()
    {
        if (horizontal)
        {
            transform.localRotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0,-90,0);
        }

        if(core.barcoSeñalado != gameObject && transform.position.y > alturaInicial)
        {
            transform.position += Vector3.down * core.speed * Time.deltaTime;
            if(fueSeñalado)
            {
                fueSeñalado = false;
                posiciones.GestionarPosiciones(casilla, tamañoDeBarco, horizontal, true); //Ocupa espacio
            }
            
        }

        if(core.barcoSeñalado == gameObject)
        {
            fueSeñalado = true;
        }

        
    }

    private void OnMouseDown()
    {

        if(core.barcoSeñalado == gameObject)
        {
            core.barcoSeñalado = null;
            posiciones.GestionarPosiciones(casilla, tamañoDeBarco, horizontal, true); //Ocupa espacio
        }
        else
        {
            core.barcoSeñalado = gameObject;

            if (!posicionado)
            {
                posicionado = true;
            }
            else
            {
                posiciones.GestionarPosiciones(casilla, tamañoDeBarco, horizontal, false); //LIBERA EL ESPACIO
            }
        }


    }
}

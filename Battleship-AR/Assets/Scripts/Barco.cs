using UnityEngine;

public class Barco : MonoBehaviour
{
    public bool posicionado;
    public int casilla;
    public int tamañoDeBarco;
    public bool horizontal;

    public float alturaInicial;

    bool fueSeñalado;

    public bool enTablero;


    Core core;
    Posiciones posiciones;

    private void Start()
    {
        core = GetComponentInParent<Core>();
        posiciones = GetComponentInParent<Posiciones>();
        alturaInicial = transform.localPosition.y;
    }



    public void Update()
    {
        if (horizontal)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0,90,0);
        }

        if(core.barcoSeñalado != gameObject && transform.localPosition.y > alturaInicial)
        {
            transform.localPosition += Vector3.down * core.speed * Time.deltaTime;
            if(fueSeñalado && enTablero)
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

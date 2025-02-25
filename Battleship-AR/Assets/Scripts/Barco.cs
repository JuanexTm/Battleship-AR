using UnityEngine;

public class Barco : MonoBehaviour
{
    public bool posicionado;
    public int casilla;
    public int tama�oDeBarco;
    public bool horizontal;

    private float alturaInicial;

    bool fueSe�alado;


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

        if(core.barcoSe�alado != gameObject && transform.position.y > alturaInicial)
        {
            transform.position += Vector3.down * core.speed * Time.deltaTime;
            if(fueSe�alado)
            {
                fueSe�alado = false;
                posiciones.GestionarPosiciones(casilla, tama�oDeBarco, horizontal, true); //Ocupa espacio
            }
            
        }

        if(core.barcoSe�alado == gameObject)
        {
            fueSe�alado = true;
        }

        
    }

    private void OnMouseDown()
    {

        if(core.barcoSe�alado == gameObject)
        {
            core.barcoSe�alado = null;
            posiciones.GestionarPosiciones(casilla, tama�oDeBarco, horizontal, true); //Ocupa espacio
        }
        else
        {
            core.barcoSe�alado = gameObject;

            if (!posicionado)
            {
                posicionado = true;
            }
            else
            {
                posiciones.GestionarPosiciones(casilla, tama�oDeBarco, horizontal, false); //LIBERA EL ESPACIO
            }
        }


    }
}

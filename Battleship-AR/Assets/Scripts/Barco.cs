using UnityEngine;

public class Barco : MonoBehaviour
{
    public bool posicionado;
    public int casilla;
    public int tama�oDeBarco;
    public bool horizontal;

    public float alturaInicial;

    bool fueSe�alado;

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

        if(core.barcoSe�alado != gameObject && transform.localPosition.y > alturaInicial)
        {
            transform.localPosition += Vector3.down * core.speed * Time.deltaTime;
            if(fueSe�alado && enTablero)
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

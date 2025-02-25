using UnityEngine;

public class Barco : MonoBehaviour
{
    public bool posicionado;
    private bool seleccionado;
    public int tamañoDeBarco;
    public bool horizontal;



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
    }
}

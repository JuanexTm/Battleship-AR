using UnityEngine;
using UnityEngine.UI;

public class Rotar : MonoBehaviour
{
    Posiciones posiciones;
    Core core;
    public Color activado, desactivado;
    
    private void Start()
    {
        posiciones = GetComponentInParent<Posiciones>();
        core = GetComponentInParent<Core>();    
    }

    private void Update()
    {
        if (core.barcoSeñalado != null)
        {
            GetComponent<Image>().color = activado;
        }
        else
        {
            GetComponent<Image>().color = desactivado;
        }
    }

    public void OnRotar()
    {
        posiciones.OnRotar();
    }
}

using UnityEngine;

public class Rotar : MonoBehaviour
{
    Posiciones posiciones;

    private void Start()
    {
        posiciones = GetComponentInParent<Posiciones>();
    }

    private void OnMouseDown()
    {
        posiciones.OnRotar();
    }
}

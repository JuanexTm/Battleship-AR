using UnityEngine;

public class CambioDeColor : MonoBehaviour
{
    private bool posicionConfigurada;

    private void Start()
    {
        //if (!posicionConfigurada)
        {
            posicionConfigurada = true;
            transform.position = Vector3.zero;
            transform.position += Vector3.one * Random.Range(-1, 1);
        }
    }
    private void OnMouseDown()
    {
        
        if(transform.localScale == Vector3.one)
        {
            transform.localScale = Vector3.one * 3;
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }
}

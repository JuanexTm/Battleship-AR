using UnityEngine;

public class Core : MonoBehaviour
{
    public GameObject barcoSeñalado;
    public float yOffSet;
    public float speed = 1;

    private void Update()
    {
        if(barcoSeñalado != null)
        {
            if (barcoSeñalado.transform.position.y < yOffSet)
            {
                barcoSeñalado.transform.position += Vector3.up * speed * Time.deltaTime;
            }
        }
        
    }


}

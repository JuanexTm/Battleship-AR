using UnityEngine;

public class Core : MonoBehaviour
{
    public GameObject barcoSeņalado;
    public float yOffSet;
    public float speed = 1;

    private void Update()
    {
        if(barcoSeņalado != null)
        {
            if (barcoSeņalado.transform.position.y < yOffSet)
            {
                barcoSeņalado.transform.position += Vector3.up * speed * Time.deltaTime;
            }
        }
        
    }


}

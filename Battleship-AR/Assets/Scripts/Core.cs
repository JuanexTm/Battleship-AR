using UnityEngine;

public class Core : MonoBehaviour
{
    public GameObject barcoSe�alado;
    public float yOffSet;
    public float speed = 1;

    private void Update()
    {
        if(barcoSe�alado != null)
        {
            if (barcoSe�alado.transform.position.y < yOffSet)
            {
                barcoSe�alado.transform.position += Vector3.up * speed * Time.deltaTime;
            }
        }
        
    }


}

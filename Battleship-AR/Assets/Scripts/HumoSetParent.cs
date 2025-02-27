using UnityEngine;

public class HumoSetParent : MonoBehaviour
{

    Transform padre;
    private void Start()
    {
        padre = GameObject.FindGameObjectWithTag("PadreHumo").GetComponent<Transform>();
        GetComponent<Transform>().SetParent(padre);
    }
}

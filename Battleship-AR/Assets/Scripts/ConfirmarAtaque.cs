using Unity.VisualScripting;
using UnityEngine;

public class ConfirmarAtaque : MonoBehaviour
{
    Core core;
    private void Start()
    {
        core = GetComponentInParent<Core>();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(core.casilla != null)
        {

        }
    }
}

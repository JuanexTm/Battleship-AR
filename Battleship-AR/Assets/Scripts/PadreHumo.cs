using UnityEngine;

public class PadreHumo : MonoBehaviour
{
    Core core;
    public GameObject miniPadre;

    private void Start()
    {
        core = GetComponentInParent<Core>();
    }

    private void Update()
    {
        if (core.ataqueDefensa.viendoEnemigo )
        {
            miniPadre.SetActive(false);
        }
        else
        {
            miniPadre.SetActive(true);
        }
    }
}

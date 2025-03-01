using TMPro;
using UnityEngine;

public class AtaqueDefensa : MonoBehaviour
{
    public bool viendoEnemigo;
    public TextMeshProUGUI texto;

    private void Start()
    {
        gameObject.SetActive(false);
    }
    private void Update()
    {
        if (viendoEnemigo)
        {
            texto.text = "Volver a tu campo";
        }
        else
        {
            texto.text = "Ir al ataque";
        }
    }

    public void OnCambiarVista()
    {
        viendoEnemigo = (viendoEnemigo == true) ? false : true;
    }
}

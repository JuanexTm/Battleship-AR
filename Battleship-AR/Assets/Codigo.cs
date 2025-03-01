using TMPro;
using UnityEngine;

public class Codigo : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TextMeshProUGUI texto;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (texto.text != GameObject.Find("SessionManager").GetComponent<SessionManager>().codigo)
        {
            texto.text = GameObject.Find("SessionManager").GetComponent<SessionManager>().codigo;
        }
        
    }
}

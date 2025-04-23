using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Personaje : MonoBehaviour
{
    public float Vida;
    public Image barraVida;

    // Start is called before the first frame update
    void Start()
    {
        Vida = 1;
        barraVida.fillAmount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("arma"))
        {
            Debug.Log("Dano");
            Vida = Vida - 0.1f;
            barraVida.fillAmount = Vida;
        }
    }
}

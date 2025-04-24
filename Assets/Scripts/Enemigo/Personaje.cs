using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Personaje : MonoBehaviour
{
    public float Vida;
    public Image barraVida;

    public float staminaMax = 100f;
    public float staminaActual;
    public float regeneracion = 10f;
    public float gastoPorCorrer = 20f;

    public Image barraStamina;

    private bool corriendo;

    // Start is called before the first frame update
    void Start()
    {
        Vida = 1;
        barraVida.fillAmount = 1;
        staminaActual = staminaMax;
        ActualizarBarra();
    }

    // Update is called once per frame
    void Update()
    {
        corriendo = Input.GetKey(KeyCode.LeftShift) && Input.GetAxis("Vertical") > 0;

        if (corriendo && staminaActual > 0)
        {
            staminaActual -= gastoPorCorrer * Time.deltaTime;
            if (staminaActual < 0) staminaActual = 0;
        }
        else if (staminaActual < staminaMax)
        {
            staminaActual += regeneracion * Time.deltaTime;
            if (staminaActual > staminaMax) staminaActual = staminaMax;
        }

        ActualizarBarra();
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

    void ActualizarBarra()
    {
        if (barraStamina != null)
        {
            barraStamina.fillAmount = staminaActual / staminaMax;
        }
    }
}

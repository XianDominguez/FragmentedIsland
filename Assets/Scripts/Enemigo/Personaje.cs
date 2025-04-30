using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Image;

public class Personaje : MonoBehaviour
{
    public float Vida;
    public Image barraVida;

    public AudioSource audioSource;
    Camera cam;

    public FirstPersonController firstPersonController;



    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
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

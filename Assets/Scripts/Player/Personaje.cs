using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.UI.Image;

public class Personaje : MonoBehaviour
{
    public float Vida;
    public Image barraVida;

    public AudioSource audioSource;
    Camera cam;

    public FirstPersonController firstPersonController;

    public GameObject pantallaGameOver;
    public GameObject objetoInteraccionE;
    RaycastHit raycast;


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f; // Despausa el juego
        pantallaGameOver.SetActive(false);
        firstPersonController.enabled = true;
        cam = Camera.main;
        Vida = 1;
        barraVida.fillAmount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vida <= 0)
        {
            Time.timeScale = 0f; // Pausa el juego
            firstPersonController.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            pantallaGameOver.SetActive(true);
        }

        if (Input.GetKey(KeyCode.E))
        {
            Debug.DrawRay(cam.transform.position, cam.transform.forward * 3, Color.green,3f);
            RaycastHit raycast;
            if(Physics.Raycast(cam.transform.position, cam.transform.forward, out raycast, 2.5f))
            {
                if(raycast.collider.gameObject.CompareTag("Material"))
                {
                    Debug.Log("Toca material");
                    ItemObject itemObject = raycast.collider.gameObject.GetComponent<ItemObject>();
                    itemObject.CogerObjeto();
                    objetoInteraccionE.SetActive(false);
                }
            }

        }
        
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out raycast, 2.5f))
        {
            if (raycast.collider.gameObject.CompareTag("Material"))
            {
                objetoInteraccionE.SetActive(true);
            }
        }
        else
        {
            objetoInteraccionE.SetActive(false);
        }
    }
 

    public void Reaparecer()
    {
        SceneManager.LoadScene("MapaPrincipal");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("arma"))
        {
            Debug.Log("Dano");
            Vida = Vida - 0.1f;
            barraVida.fillAmount = Vida;
        } 
    }


}

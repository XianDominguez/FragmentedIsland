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

    public SumarMaterial sumarMaterial;

    bool cofreRecogido = false;
    bool cofreDesenterrado = false;
    public Animator animatorCofre;


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
            GameObject pala = GameObject.Find("PalaIdle");

            RaycastHit raycast;
            if(Physics.Raycast(cam.transform.position, cam.transform.forward, out raycast, 2.5f))
            {
                if(raycast.collider.gameObject.CompareTag("Material"))
                {
                    ItemObject itemObject = raycast.collider.gameObject.GetComponent<ItemObject>();

                    itemObject.CogerObjeto();

                    Collider other = raycast.collider;

                    sumarMaterial.AnimacionSumar(other);

                    objetoInteraccionE.SetActive(false);
                }

                if (raycast.collider != null && raycast.collider.gameObject.CompareTag("Cofre"))
                {
                    GameObject cofre = raycast.collider.gameObject;

                    if (!cofreDesenterrado && pala.activeInHierarchy)
                    {
                        // Animar pala y cofre
                        Animator animPala = pala.GetComponent<Animator>();
                        animPala.SetBool("isSecondary", true);
                        animPala.SetTrigger("AccionSecundaria");
                        animPala.SetBool("bandera", false);
                        animPala.Play("PalaExcavar");

                        animatorCofre.Play("AbrirCofre");

                        cofreDesenterrado = true;
                    }
                    else if (cofreDesenterrado && !cofreRecogido)
                    {
                        Cofre scriptCofre = cofre.GetComponent<Cofre>();
                        scriptCofre.RecogerCofre(); // Entregar recompensa o similar

                        cofreRecogido = true;

                        objetoInteraccionE.SetActive(false); // Oculta el indicador de interacción
                    }
                }
            }

        }
        
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out raycast, 2.5f))
        {
            if (raycast.collider.gameObject.CompareTag("Material") || (raycast.collider.gameObject.CompareTag("Cofre") && !cofreRecogido) || (raycast.collider.gameObject.CompareTag("ArenaCofre") && !cofreDesenterrado))
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

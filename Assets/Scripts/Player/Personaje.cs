using System.Collections;
using System.Collections.Generic;
using SmartPoint;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.UI.Image;

public class Personaje : MonoBehaviour
{
    [Header("Vida")]
    [Space]
    public float vida;      
    public Image barraVida;

    public delegate void DeathHandler(GameObject entity);
    public event DeathHandler OnDeath;
    [Header("Asignaciones")]
    [Space]
    public AudioSource audioSource; 
    public FirstPersonController firstPersonController; //Controlador de movimiento de personaje 
    public SumarMaterial sumarMaterial;                 //Animacion de suma de material al recogerlo

    Camera cam;
    RaycastHit raycast;

    public CheckPoint checkPoint;

    [Header("UI")]
    [Space]
    public GameObject pantallaGameOver;
    public GameObject objetoInteraccionE;
    [Header("Animator")]
    [Space]
    public Animator animatorCofre;

    bool cofreRecogido = false;
    bool cofreDesenterrado = false;


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f; // Despausa el juego
        pantallaGameOver.SetActive(false);
        firstPersonController.enabled = true;
        cam = Camera.main;
        vida = 1;
        barraVida.fillAmount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (vida <= 0)
        {
            Time.timeScale = 0f; // Pausa el juego
            firstPersonController.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            pantallaGameOver.SetActive(true);
        }
        */

        if (Input.GetKeyDown(KeyCode.V))
        {
            vida -= 0.1f;
            barraVida.fillAmount = vida;

            if (vida <= 0f)
            {
                Debug.Log("O vida");

                vida = 0f; // evitar valores negativos
                barraVida.fillAmount = 0f;
                if (OnDeath != null)
                {
                    Debug.Log("OnDeath tiene suscriptores");
                }
                else
                {
                    Debug.Log("OnDeath est� vac�o");
                }
                Debug.Log("Invocando OnDeath de: " + gameObject.name);
                OnDeath?.Invoke(gameObject); // <- �AQU� INVOCAMOS EL EVENTO!
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            GameObject pala = GameObject.Find("PalaIdle");

            RaycastHit raycast;

            if(Physics.Raycast(cam.transform.position, cam.transform.forward, out raycast, 2.5f))
            {
                //Raycast qeu detecta un material recolectable a mano 

                if(raycast.collider.gameObject.CompareTag("Material"))
                {
                    ItemObject itemObject = raycast.collider.gameObject.GetComponent<ItemObject>();

                    itemObject.CogerObjeto();

                    Collider other = raycast.collider;

                    sumarMaterial.AnimacionSumar(other);

                    objetoInteraccionE.SetActive(false);
                }

                if(raycast.collider.gameObject.CompareTag("EspadaRecoger"))
                {
                    ToolBar toolBar = FindObjectOfType<ToolBar>();
                    toolBar.spritesArmas[0].SetActive(true);
                    toolBar.DesbloquearArma(0);

                    Destroy(raycast.collider.gameObject);
                }

                if (raycast.collider.gameObject.CompareTag("Horno"))
                {
                    Horno horno = raycast.collider.gameObject.GetComponent<Horno>();

                    horno.ComprobarPosibilidades(horno.receta,horno.sistemaDeInventario.inventario);
                    horno.IntentarCraftear();

                    objetoInteraccionE.SetActive(false);
                }



                //Logica cofre del tesoro

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

                        objetoInteraccionE.SetActive(false); // Oculta el indicador de interacci�n
                    }
                }
            }

        }
        
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out raycast, 2.5f))
        {
            if (raycast.collider.gameObject.CompareTag("Material") || (raycast.collider.gameObject.CompareTag("Cofre") && !cofreRecogido) || raycast.collider.gameObject.CompareTag("Horno") || raycast.collider.gameObject.CompareTag("EspadaRecoger"))
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
            vida -= 0.1f;
            barraVida.fillAmount = vida;

            if (vida <= 0f)
            {
                vida = 0f;
                barraVida.fillAmount = 0f;
                OnDeath?.Invoke(gameObject); // <- Invocamos tambi�n aqu�
            }
        }
    }

}

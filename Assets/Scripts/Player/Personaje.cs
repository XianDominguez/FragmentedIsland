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

    private CheckPoint checkPoint;

    [Header("UI")]
    [Space]
    public GameObject pantallaGameOver;
    public GameObject objetoInteraccionE;
    [Header("Animator")]
    [Space]
    public Animator animatorCofre;

    bool cofreRecogido = false;
    bool cofreDesenterrado = false;

    public MonoBehaviour controlCamara;
    public MonoBehaviour controlMovimiento;
    public MonoBehaviour personajeAnimaciones;

    public CanvasGroup fadeCanvas; // UI para fade (CanvasGroup con imagen negra)
    public float fadeDuration = 3f;
    public float fadeDead = 0.2f;

    public Image damageScreen;
    public float damageFlashDuration = 0.3f;

    private Coroutine flashCoroutine;

    public AudioClip audioMuerte;
    public AudioClip[] hitEsqueleto;
    public AudioClip[] hitZombie;
    public AudioClip[] hitEspiritu;
    public AudioClip[] hitGolem;

    // Start is called before the first frame update
    void Start()
    {
        damageScreen.enabled = false;

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
            if (Input.GetKeyDown(KeyCode.E))
            {
                GameObject pala = GameObject.Find("PalaIdle");

                RaycastHit raycast;
    
                if(Physics.Raycast(cam.transform.position, cam.transform.forward, out raycast, 2.5f))
                {
                    //Raycast que detecta un material recolectable a mano cuando el jugador lo mira

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

                        if (!cofreDesenterrado && pala.activeInHierarchy)   //Desentierra el tesoro cuando el jugador tiene la pala en la mano y presiona la E
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
        OnDeath?.Invoke(gameObject);
        StartCoroutine(ReaparecerConFade());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EspadaEsqueleto"))
        {
            vida -= 0.1f;
            barraVida.fillAmount = vida;
            if (flashCoroutine != null)
                StopCoroutine(flashCoroutine);

            flashCoroutine = StartCoroutine(FlashDamageScreen());

            if (hitEsqueleto.Length > 0)
            {
                int indice = Random.Range(0, hitEsqueleto.Length);
                audioSource.clip = hitEsqueleto[indice];
            }
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.Play();

            ComprobarVida();
        }
        if (other.CompareTag("PunoGolem"))
        {
            vida -= 0.05f;
            barraVida.fillAmount = vida;
            if (flashCoroutine != null)
                StopCoroutine(flashCoroutine);

            flashCoroutine = StartCoroutine(FlashDamageScreen());

            if (hitGolem.Length > 0)
            {
                int indice = Random.Range(0, hitGolem.Length);
                audioSource.clip = hitGolem[indice];
            }
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.Play();

            ComprobarVida();
        }
        if (other.CompareTag("PunoEspiritu"))
        {
            vida -= 0.2f;
            barraVida.fillAmount = vida;
            if (flashCoroutine != null)
                StopCoroutine(flashCoroutine);

            flashCoroutine = StartCoroutine(FlashDamageScreen());

            if (hitEspiritu.Length > 0)
            {
                int indice = Random.Range(0, hitEspiritu.Length);
                audioSource.clip = hitEspiritu[indice];
            }
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.Play();

            ComprobarVida();
        }
        if (other.CompareTag("ManoZombie"))
        {
            vida -= 0.3f;
            barraVida.fillAmount = vida;
            if (flashCoroutine != null)
                StopCoroutine(flashCoroutine);

            flashCoroutine = StartCoroutine(FlashDamageScreen());

            if (hitZombie.Length > 0)
            {
                int indice = Random.Range(0, hitZombie.Length);
                audioSource.clip = hitZombie[indice];
            }
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.Play();

            ComprobarVida();
        }
    }

    void ComprobarVida()
    {
        if (vida <= 0f)
        {
            personajeAnimaciones = GetComponentInChildren<PersonajeAnimaciones>();

            vida = 0f;
            barraVida.fillAmount = 0f;
            StartCoroutine(MorirConFade());

        }
    }


    private IEnumerator Fade(float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }
        fadeCanvas.alpha = to;
    }

    private IEnumerator ReaparecerConFade()
    {
        Time.timeScale = 1f;
        pantallaGameOver.SetActive(false);

        yield return StartCoroutine(Fade(1, 0, fadeDuration)); // Fade desde negro a juego

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        controlCamara.enabled = true;
        controlMovimiento.enabled = true;
        personajeAnimaciones.enabled = true;
    }

    private IEnumerator MorirConFade()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        controlCamara.enabled = false;
        controlMovimiento.enabled = false;
        personajeAnimaciones.enabled = false;

        yield return StartCoroutine(Fade(0, 1, fadeDead)); // Fade a negro

        Time.timeScale = 0f;
        pantallaGameOver.SetActive(true); // Mostrar pantalla Game Over después del fade
    }


    private IEnumerator FlashDamageScreen()
    {
        damageScreen.enabled = true;

        yield return new WaitForSeconds(damageFlashDuration);

        damageScreen.enabled = false;
    }

}

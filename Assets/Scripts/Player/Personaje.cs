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

    public delegate void DeathHandler(GameObject entity); //Evento para ejecutar caundo jugador se queda con la vida a 0
    public event DeathHandler OnDeath;
    [Header("Asignaciones")]
    [Space]
    public FirstPersonController firstPersonController; //Controlador total del personaje 
    public SumarMaterial sumarMaterial;                 //Animacion de suma de material al recogerlo

    public MonoBehaviour controlCamara;                  //Script controlador de camara del jugador 
    public MonoBehaviour controlMovimiento;              //Script controlador de movimiento de personaje 
    public MonoBehaviour personajeAnimaciones;           //Script controlador de las animaciones de la mano del personaje con su objeto

    Camera cam; //Camara del jugador 

    RaycastHit raycast; //Raycast que detecta hacia donde mira el jugador

    [Header("UI")]
    [Space]
    public GameObject pantallaGameOver;     //Pantalla de reaparicion ajustes o salir, cuando la vida del jugador es 0
    public GameObject objetoInteraccionE;   //Grafico que se muestra cuando puedes interactuar con el objeto al que miras
    [Header("Animator")]
    [Space]
    public Animator animatorCofre;  //Animacion del cofre al desenterrarlo

    bool cofreRecogido = false;   //Estado de si el cofre esta recogido o no
    bool cofreDesenterrado = false; //Estado de si el cofre esta desenterrado o no

    [Header("Fade")]
    [Space]

    public CanvasGroup fadeCanvas; // UI para fade (CanvasGroup con imagen negra)
    public float fadeDuration = 3f; //Tiempo que dura el fade de entrada a las casa o cueva
    public float fadeDead = 0.2f;   //Tiempo que dur el fade cuando la vida del jugador es 0

    [Header("Pantalla Daño")]
    [Space]
    public Image damageScreen;  //Imagen roja que aparece cuando el jugador pierde vida
    public float damageFlashDuration = 0.3f;    //Duracion de la imagen de dano en pantalla
    private Coroutine flashCoroutine;   //Corutina que controla la pantalla

    [Header("Musica y efectos")]
    [Space]
    public AudioSource audioSource; //Sonido que hace el player
    public AudioClip audioMuerte;   //Cuando la vida es 0
    public AudioClip[] hitEsqueleto;    //Cuando lo hace dano un enemigpo
    public AudioClip[] hitZombie;
    public AudioClip[] hitEspiritu;
    public AudioClip[] hitGolem;

    void Start()
    {
        pantallaGameOver.SetActive(false);
        damageScreen.enabled = false;

        Time.timeScale = 1f; // Despausa el juego
        firstPersonController.enabled = true; //Activa el control del jugador

        cam = Camera.main;

        vida = 1;
        barraVida.fillAmount = 1;
    }

    // Update is called once per frame
    void Update()
    { 

        if (Input.GetKeyDown(KeyCode.E))    //El jugador pulsa la E
            {
                GameObject pala = GameObject.Find("PalaIdle"); 

                RaycastHit raycast;
    
                if(Physics.Raycast(cam.transform.position, cam.transform.forward, out raycast, 2.5f))
                {
                    if(raycast.collider.gameObject.CompareTag("EspadaRecoger")) //Pulsa la E mirando a un objeto con un tag
                    {
                        ToolBar toolBar = FindObjectOfType<ToolBar>();  //Obtiene el script

                        toolBar.spritesArmas[0].SetActive(true);  //Activa el sprite
                        toolBar.DesbloquearArma(0); //Desbloquea su uso

                        Destroy(raycast.collider.gameObject); //Destruye la espada
                    }

                    if (raycast.collider.gameObject.CompareTag("Horno"))
                    {
                        Horno horno = raycast.collider.gameObject.GetComponent<Horno>();

                        horno.ComprobarPosibilidades(horno.receta,horno.sistemaDeInventario.inventario);    //Comprueba si se puede craftear el metal
                        horno.IntentarCraftear();   //Lo creaftea

                        objetoInteraccionE.SetActive(false); //Desactiva el mensaje de pulsar la E para interactuar
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
                            scriptCofre.RecogerCofre(); // Recoge los objetos dentro del cofre

                            cofreRecogido = true;

                            objetoInteraccionE.SetActive(false); // Oculta el indicador de interacción
                        }
                    }
                }

            }
        
        //******************************************** Se activa cuando el jugador mira a un objeto interactuable ******************************************

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out raycast, 2.5f))
        {
            if (raycast.collider.gameObject.CompareTag("Cofre") && !cofreRecogido || raycast.collider.gameObject.CompareTag("Horno") || raycast.collider.gameObject.CompareTag("EspadaRecoger"))
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
        if (other.CompareTag("EspadaEsqueleto"))    //Detecta que enemigo te hace dano
        {
            vida -= 0.1f;   //Resta la vida
            barraVida.fillAmount = vida;    //Actualiza la barra

            // Evita solapar múltiples flashes de daño
            if (flashCoroutine != null)
                StopCoroutine(flashCoroutine);

            flashCoroutine = StartCoroutine(FlashDamageScreen());   //Ejecuta el flash de dano

            if (hitEsqueleto.Length > 0)    //Selecciona el sonido de dolor del personaje segun el enemigo que te haga dano 
            {
                int indice = Random.Range(0, hitEsqueleto.Length);
                audioSource.clip = hitEsqueleto[indice];
            }
            audioSource.pitch = Random.Range(0.9f, 1.1f); //Cambia el pitch
            audioSource.Play(); //Ejecuta el sonido

            ComprobarVida();    //Comprueba la vida del personaje
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

    void ComprobarVida()    //Comprueba la vida del jugador despues de recibir dano
    {
        if (vida <= 0f) //Si no le queda vida
        {
            personajeAnimaciones = GetComponentInChildren<PersonajeAnimaciones>();

            vida = 1f;
            barraVida.fillAmount = 1f;
            StartCoroutine(MorirConFade()); //Ejecuta el fade 

        }
    }

    private IEnumerator Fade(float from, float to, float duration)  //Fade de la pantalla
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

    private IEnumerator ReaparecerConFade() //Fade que se ejecuta al reaparecer el jugador
    {
        Time.timeScale = 1f;//Despausa el juego

        pantallaGameOver.SetActive(false);

        yield return StartCoroutine(Fade(1, 0, fadeDuration)); // Fade desde negro a juego
        
        //Desbloquea el cursor y hace que no aparezca en patalla
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Activa todo movimiento del jugador
        controlCamara.enabled = true;
        controlMovimiento.enabled = true;
        personajeAnimaciones.enabled = true;
    }

    private IEnumerator MorirConFade()  //Fade que se ejecuta cuando la vida del jugador es 0
    {
        //Bloquea el cursor y hace que aprezca en patalla
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //Desactiva todo movimiento del jugador
        controlCamara.enabled = false;
        controlMovimiento.enabled = false;
        personajeAnimaciones.enabled = false;

        yield return StartCoroutine(Fade(0, 1, fadeDead)); // Fade a negro

        Time.timeScale = 0f;//Pausa el juego
        pantallaGameOver.SetActive(true); // Mostrar pantalla Game Over después del fade
    }


    private IEnumerator FlashDamageScreen() //Flash rojo al recibir dano
    {
        damageScreen.enabled = true;

        yield return new WaitForSeconds(damageFlashDuration);

        damageScreen.enabled = false;
    }

}

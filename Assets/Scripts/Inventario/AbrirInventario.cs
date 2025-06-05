using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbrirInventario : MonoBehaviour
{
    [Header("Asignaciones")]
    [Space]
    public MonoBehaviour controlCamara;
    public MonoBehaviour controlMovimiento;
    public MonoBehaviour personajeAnimaciones;
    [Header("UI in game")]
    [Space]
    public GameObject inventario;
    public GameObject UiCrafteo;
    public GameObject mapa;
    [Space]
    public bool banderaaInventario;
    public bool banderaCrafteo;
    public bool banderaMapa;
    bool inventarioAbierto;
    bool menuCrafteoAbierto;
    bool mapaAbierto;
    [Header("Sistema de Crafteo")]
    [Space]
    public Crafteo craftingSystem;
    [Space]

    public RecetasCrafteo recetaMadera;
    public RecetasCrafteo recetaHacha;
    public RecetasCrafteo recetaPico;
    [Header("Inventario")]
    [Space]
    public SistemaDeInventario sistemaDeInventario;
    [Header("Pantalla de crafteo")]
    [Space]
    public Button craftearPico;
    public Button craftearHacha;
    public Button craftearMadera;
    [Header("Audio")]
    [Space]
    public AudioSource audioPlayer;
    public AudioClip audioClipMapa;

    private void Awake()
    {
        inventario.SetActive(true);
        UiCrafteo.SetActive(true);
    }

    void Start()
    {
        inventario.SetActive(false);    //Desactiva los pantallas de inventario y crafteo
        UiCrafteo.SetActive(false);
        banderaaInventario = false;
        banderaCrafteo = false;

        inventarioAbierto = false;
        menuCrafteoAbierto  = false;

        craftearPico.interactable = false;  //Desactiva los botones de crafteo
        craftearHacha.interactable = false;
        craftearMadera.interactable = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !menuCrafteoAbierto && !mapaAbierto)   //Se activa cuando el jugador pulsa tab y no hay otra pantalla abierta
        {
            personajeAnimaciones = GetComponentInChildren<PersonajeAnimaciones>();

            if (banderaaInventario == false)    //Activa la pantalla de inventario y descativa el movimiento
            {
                inventario.SetActive(true);

                inventarioAbierto = true;

                banderaaInventario = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                controlCamara.enabled = false;
                controlMovimiento.enabled = false;
                personajeAnimaciones.enabled = false;
            }
            else //Vuelve al juego
            {
                inventario.SetActive(false);

                inventarioAbierto = false;

                banderaaInventario = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                controlCamara.enabled = true;
                personajeAnimaciones.enabled = true;
                controlMovimiento.enabled = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && !inventarioAbierto && !mapaAbierto)  //Se activa cuando el jugador pulsa la Q y no hay otra pantalla abierta
        {
            personajeAnimaciones = GetComponentInChildren<PersonajeAnimaciones>();

            if (banderaCrafteo == false)     //Activa la pantalla de crafteo y descativa el movimiento
            {
                UiCrafteo.SetActive(true);
                menuCrafteoAbierto = true;

                Crafteo crafteo = UiCrafteo.GetComponent<Crafteo>();    //Otiene el script de ctafteo y comprueba si se puede craftear algun objeto para activar el boton
                IntentarCraftear();
                IntentarCraftear2();
                IntentarCraftear3();
               

                banderaCrafteo = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                controlCamara.enabled = false;
                controlMovimiento.enabled = false;
                personajeAnimaciones.enabled = false;

            }
            else //Vuelve al juego
            {
                menuCrafteoAbierto = false;

                UiCrafteo.SetActive(false);
                banderaCrafteo = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                controlCamara.enabled = true;
                controlMovimiento.enabled = true;
                personajeAnimaciones.enabled = true;

            }
        }

        if (Input.GetKeyDown(KeyCode.M ) && !menuCrafteoAbierto && !inventarioAbierto)   //Se activa cuando el jugador pulsa la M y no hay otra pantalla abierta
        {
            personajeAnimaciones = GetComponentInChildren<PersonajeAnimaciones>();

            audioPlayer.PlayOneShot(audioClipMapa); //Ejecuta el sonido de abrir el mapa

            if (banderaMapa == false)   //Activa la pantalla del mapa y descativa el movimiento
            {
                mapa.SetActive(true);

                mapaAbierto = true;
                banderaMapa = true;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                controlCamara.enabled = false;
                controlMovimiento.enabled = false;
                personajeAnimaciones.enabled = false;
            }
            else //Vuelve al juego
            {
                mapa.SetActive(false);

                mapaAbierto = false;
                banderaMapa = false;

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                controlCamara.enabled = true;
                personajeAnimaciones.enabled = true;
                controlMovimiento.enabled = true;
            }
        }
    }

    public void IntentarCraftear()  //Busca en el inventario si tienens los objetos necesarios para craftear un objeto
    {
        bool exito = craftingSystem.ComprobarPosibilidades(recetaMadera, sistemaDeInventario.inventario);   //Comprueba e lcrafteo de los tablones

        if (exito)  //Se puede craftear
        {
            Debug.Log("Puedes craftear tablones");
            craftearMadera.interactable = true;//Se activa el boton
        } 
        else
        {
            craftearMadera.interactable = false;
        }
    }

    public void IntentarCraftear2()
    {
        bool exito = craftingSystem.ComprobarPosibilidades(recetaPico, sistemaDeInventario.inventario);

        if (exito)
        {
            if(craftingSystem.picoCrafteado == true)
            {
                craftearPico.interactable = false;
            }
            else
            {
                Debug.Log("Puedes craftear pico");
                craftearPico.interactable = true;
            }

        }   
        else
        {
            craftearPico.interactable = false;
        }
    }

    public void IntentarCraftear3()
    {
        bool exito = craftingSystem.ComprobarPosibilidades(recetaHacha, sistemaDeInventario.inventario);

        if (exito)
        {
            if(craftingSystem.hachaCrafteada == true)
            {
                craftearHacha.interactable = false;
                Debug.Log("Craft hacha no interact");
            }
            else
            {
                Debug.Log("Puedes craftear hacha");
                craftearHacha.interactable = true;
            }
         
        }
        else
        {
            craftearHacha.interactable = false;
            Debug.Log("Craft hacha no interact");   
        }
    }
}

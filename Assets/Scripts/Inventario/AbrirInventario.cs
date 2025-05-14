using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbrirInventario : MonoBehaviour
{
    public MonoBehaviour controlCamara;
    public MonoBehaviour controlMovimiento;
    public GameObject inventario;
    public GameObject UiCrafteo;
    public bool banderaaInventario;
    public bool banderaCrafteo;

    public Crafteo craftingSystem;
    public RecetasCrafteo recetaMadera;
    public RecetasCrafteo recetaHacha;
    public RecetasCrafteo recetaPico;

    public SistemaDeInventario sistemaDeInventario;


    public Button craftearPico;
    public Button craftearHacha;
    public Button craftearMadera;

    // Start is called before the first frame update
    private void Awake()
    {
        inventario.SetActive(true);
        UiCrafteo.SetActive(true);

    }

    void Start()
    {
        inventario.SetActive(false);
        UiCrafteo.SetActive(false);
        banderaaInventario = false;
        banderaCrafteo = false;

        craftearPico.interactable = false;
        craftearHacha.interactable = false;
        craftearMadera.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (banderaaInventario == false)
            {
                inventario.SetActive(true);
                banderaaInventario = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                controlCamara.enabled = false;
                controlMovimiento.enabled = false;
            }
            else
            {
                inventario.SetActive(false);
                banderaaInventario = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                controlCamara.enabled = true;
                controlMovimiento.enabled = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (banderaCrafteo == false)
            {
                UiCrafteo.SetActive(true);

                Crafteo crafteo = UiCrafteo.GetComponent<Crafteo>();
                IntentarCraftear();
                IntentarCraftear2();
                IntentarCraftear3();
               

                banderaCrafteo = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                controlCamara.enabled = false;
                controlMovimiento.enabled = false;
            }
            else
            {
                UiCrafteo.SetActive(false);
                banderaCrafteo = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                controlCamara.enabled = true;
                controlMovimiento.enabled = true;
            }
        }
    }

    public void IntentarCraftear()
    {
        bool exito = craftingSystem.ComprobarPosibilidades(recetaMadera, sistemaDeInventario.inventario);

        if (exito)
        {
            Debug.Log("Puedes craftear tablones");
            craftearMadera.interactable = true;
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
            Debug.Log("Puedes craftear pico");
            craftearPico.interactable = true;
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
            Debug.Log("Puedes craftear hacha");
            craftearHacha.interactable = true;
        }
        else
        {
            craftearHacha.interactable = false;
        }
    }
}

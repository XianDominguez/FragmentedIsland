using System.Collections;
using TMPro;
using UnityEngine;

public class Dialogo : MonoBehaviour
{
    [SerializeField] private GameObject iconoDialogo;
    [SerializeField] private GameObject panelDialogo;
    [SerializeField] private TMP_Text textoDialogo;
    [SerializeField, TextArea(4,6)] private string[] lineasDialogo;

    private FirstPersonController firstPersonController;
    private PersonajeAnimaciones personajeAnimaciones;
    
    private bool jugadorEnRango;
    private bool empezoDialogo;
    private int indexDialogo;
    private float velocidadEscritura = 0.025f;


    // Update is called once per frame
    void Update()
    {
        if(jugadorEnRango && Input.GetKeyDown(KeyCode.T))
        {
            if(!empezoDialogo)
            {
                EmpezarDialogo();
            }
            else if(textoDialogo.text == lineasDialogo[indexDialogo])
            {
                SiguienteDialogo();
            }
            else
            {
                StopAllCoroutines();
                textoDialogo.text = lineasDialogo[indexDialogo];
            }
        }

        if(empezoDialogo && Input.GetKeyDown(KeyCode.Escape))
        {
            empezoDialogo = false;
            panelDialogo.SetActive(false);
            iconoDialogo.SetActive(true);
            firstPersonController.enabled = true;
            personajeAnimaciones.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;

        }


    }

    void EmpezarDialogo()
    {
        empezoDialogo = true;
        panelDialogo.SetActive(true);
        iconoDialogo.SetActive(false);
        indexDialogo = 0;
        firstPersonController.enabled = false;
        personajeAnimaciones.enabled = false;
        StartCoroutine(MostrarTexto());
    }

    void SiguienteDialogo()
    {
        indexDialogo++;
        if(indexDialogo < lineasDialogo.Length)
        {
            StartCoroutine(MostrarTexto());
        }
        else
        {
            empezoDialogo = false;
            panelDialogo.SetActive(false);
            iconoDialogo.SetActive(true);
            firstPersonController.enabled = true;
            personajeAnimaciones.enabled = true;
        }
    }

    IEnumerator MostrarTexto()
    {
        textoDialogo.text = string.Empty;

        foreach(char ch in lineasDialogo[indexDialogo])
        {
            textoDialogo.text += ch;
            yield return new WaitForSeconds(velocidadEscritura);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            firstPersonController = other.gameObject.GetComponent<FirstPersonController>();
            personajeAnimaciones = other.gameObject.GetComponentInChildren<PersonajeAnimaciones>();


            jugadorEnRango = true;
            iconoDialogo.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            jugadorEnRango = false;
            iconoDialogo.SetActive(false);

        }
    }
}

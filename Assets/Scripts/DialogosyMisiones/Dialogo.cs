using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dialogo : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject iconoDialogo;
    [SerializeField] private GameObject panelDialogo;
    [SerializeField] private GameObject textHablar;
    [SerializeField] private TMP_Text textoDialogo;
    [SerializeField] private EtapaDialogo[] etapasDialogo;

    [SerializeField] private List<Sprite> spritesDialogo;

    [SerializeField] private GameObject objetoADestruir;
    [SerializeField] private InvetarioItemData metal;

    private FirstPersonController firstPersonController;
    private PersonajeAnimaciones personajeAnimaciones;

    private bool jugadorEnRango;
    private bool empezoDialogo;
    private int indexEtapa;
    private int indexLinea;
    private bool escribiendoTexto;

    private float velocidadEscritura = 0.025f;



    private void Update()
    {
        if (jugadorEnRango && Input.GetKeyDown(KeyCode.T))
        {
            DeterminarNpc();

            if (escribiendoTexto) return; // Evita interacción mientras se escribe

            
            EtapaDialogo etapaActual = etapasDialogo[indexEtapa];

            if (ControlMisiones.Instance.EstaMisionCompletada(etapaActual.idMision))
            {
                indexEtapa++;
                indexLinea = 0;
                EmpezarDialogo(); // Mostrar nueva etapa desde el principio
            }
            else if (!empezoDialogo)
            {
                EmpezarDialogo();
            }
            else if (textoDialogo.text == etapasDialogo[indexEtapa].lineas[indexLinea])
            {
                SiguienteDialogo();
            }
            else
            {
                StopAllCoroutines();
                textoDialogo.text = etapasDialogo[indexEtapa].lineas[indexLinea];
                escribiendoTexto = false;
            }
        }
    }

    void EmpezarDialogo()
    {
        SaltarEtapasInnecesarias();

        empezoDialogo = true;
        panelDialogo.SetActive(true);
        iconoDialogo.SetActive(false);
        indexLinea = 0;

        firstPersonController.enabled = false;
        personajeAnimaciones.enabled = false;
        Cursor.lockState = CursorLockMode.None;

        StartCoroutine(MostrarTexto());
    }

    void SiguienteDialogo()
    {
        indexLinea++;

        // Validaciones de seguridad
        if (indexEtapa >= etapasDialogo.Length)
        {
            TerminarDialogo();
            return;
        }

        if (indexLinea >= etapasDialogo[indexEtapa].lineas.Length)
        {
            EtapaDialogo etapaActual = etapasDialogo[indexEtapa];

            // Verificar si necesita misión
            if (etapaActual.requiereMision && !ControlMisiones.Instance.EstaMisionCompletada(etapaActual.idMision))
            {
                TerminarDialogo();
                indexLinea--; // Para que se repita la línea si se reinicia el diálogo
                return;
            }

            indexEtapa++;
            SaltarEtapasInnecesarias();
            indexLinea = 0;

            if (indexEtapa < etapasDialogo.Length)
            {
                StartCoroutine(MostrarTexto());
            }
            else
            {
                TerminarDialogo();
            }

            return;
        }

        // Si aún hay líneas en la misma etapa
        StartCoroutine(MostrarTexto());
    }
    

    IEnumerator MostrarTexto()
    {
        escribiendoTexto = true;
        textoDialogo.text = "";

        foreach (char ch in etapasDialogo[indexEtapa].lineas[indexLinea])
        {
            textoDialogo.text += ch;
            yield return new WaitForSeconds(velocidadEscritura);
        }

        escribiendoTexto = false;
    }

    void TerminarDialogo()
    {
        empezoDialogo = false;
        panelDialogo.SetActive(false);
        iconoDialogo.SetActive(true);
        firstPersonController.enabled = true;
        personajeAnimaciones.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        ControlMisiones.Instance.CompletarMision("hablar_" + gameObject.tag);
        
        if (ControlMisiones.Instance.EstaMisionCompletada("hablar_viejo"))
        {
            ToolBar toolBar = FindObjectOfType<ToolBar>();
            toolBar.spritesArmas[2].SetActive(true);
            toolBar.DesbloquearArma(2);
        }

        if (ControlMisiones.Instance.EstaMisionCompletada("forjar_lingote"))
        {
            // 1. Eliminar el ítem del inventario
            SistemaDeInventario.Instance.RemoveCantidad(metal, 1);

            // 2. Destruir el obstáculo del juego
            if (objetoADestruir != null)
            {
                Destroy(objetoADestruir);
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = true;
            iconoDialogo.SetActive(true);
            textHablar.SetActive(true);

            firstPersonController = other.GetComponent<FirstPersonController>();
            personajeAnimaciones = other.GetComponentInChildren<PersonajeAnimaciones>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = false;
            iconoDialogo.SetActive(false);
            textHablar.SetActive(false);

        }
    }

    void SaltarEtapasInnecesarias()
    {
        while (
            indexEtapa < etapasDialogo.Length &&
            etapasDialogo[indexEtapa].requiereMision &&
            ControlMisiones.Instance.EstaMisionCompletada(etapasDialogo[indexEtapa].idMision)
        )
        {
            indexEtapa++;
        }
    }

    void DeterminarNpc()
    {
        if(gameObject.CompareTag("Viejo"))
        {
            Image imagenTexto = panelDialogo.GetComponent<Image>();
            imagenTexto.sprite = spritesDialogo[0];
        }

        if (gameObject.CompareTag("herrero"))
        {
            Image imagenTexto = panelDialogo.GetComponent<Image>();
            imagenTexto.sprite = spritesDialogo[1];
        }

        if (gameObject.CompareTag("Ahoy"))
        {
            Image imagenTexto = panelDialogo.GetComponent<Image>();
            imagenTexto.sprite = spritesDialogo[2];
        }

        if (gameObject.CompareTag("Marinero"))
        {
            Image imagenTexto = panelDialogo.GetComponent<Image>();
            imagenTexto.sprite = spritesDialogo[3];
        }
    }
}

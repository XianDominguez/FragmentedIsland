using System.Collections;
using TMPro;
using UnityEngine;

public class Dialogo : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject iconoDialogo;
    [SerializeField] private GameObject panelDialogo;
    [SerializeField] private TMP_Text textoDialogo;
    [SerializeField] private EtapaDialogo[] etapasDialogo;

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
        if(Input.GetKeyDown(KeyCode.Alpha9))
        {
            ControlMisiones.Instance.CompletarMision("recoger_tesoro");
        }

        if (jugadorEnRango && Input.GetKeyDown(KeyCode.T))
        {
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
        ControlMisiones.Instance.CompletarMision("hablar_viejo");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = true;
            iconoDialogo.SetActive(true);

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
}

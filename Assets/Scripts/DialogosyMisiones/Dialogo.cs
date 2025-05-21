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

    private float velocidadEscritura = 0.025f;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha9))
        {
            ControlMisiones.Instance.CompletarMision("recoger_tesoro");
        }

        if (jugadorEnRango && Input.GetKeyDown(KeyCode.T))
        {
            EtapaDialogo etapaActual = etapasDialogo[indexEtapa];

            Debug.Log(indexEtapa);

            if (ControlMisiones.Instance.EstaMisionCompletada(etapaActual.idMision))
            {
                Debug.Log("Se ejecuta 1er if");
                indexEtapa++;
                SiguienteDialogo();

            }
            else if (!empezoDialogo)
            {
                EmpezarDialogo();
            }
            else if (textoDialogo.text == etapasDialogo[indexEtapa].lineas[indexLinea])
            {
                Debug.Log("Se ejecuta 3er if");
                SiguienteDialogo();
            }
            else
            {
                StopAllCoroutines();
                textoDialogo.text = etapasDialogo[indexEtapa].lineas[indexLinea];
            }
        }

        if (empezoDialogo && Input.GetKeyDown(KeyCode.Escape))
        {
            TerminarDialogo();
        }
    }

    void EmpezarDialogo()
    {
        //Debug.Log("Empieza el dialogo");

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
        //Debug.Log("Siguiente dialogo se suma 1 a indexlinea a " + indexLinea);
        //Debug.Log("El index de la etapa es " + indexEtapa);

        indexLinea++;
    
        //Debug.Log("Index de la linea ahora es" + indexLinea);

        if (indexLinea < etapasDialogo[indexEtapa].lineas.Length)
        {
            //Debug.Log("Se muestra el texto en Siguiente Dialogo");

            StartCoroutine(MostrarTexto());
        }
        else
        {
            //Debug.Log("El index de la etapa es " + indexEtapa + "    Se ejecuta desde else de siguiente dialogo");

            EtapaDialogo etapaActual = etapasDialogo[indexEtapa];

            // Verificar si necesita misión
            if (etapaActual.requiereMision && !ControlMisiones.Instance.EstaMisionCompletada(etapaActual.idMision))
            {
                //Debug.Log("El index de la etapa es " + indexEtapa + "    Se ejecuta desde verificacion de mision");

                TerminarDialogo();
                indexLinea--; // Repetir esta línea si se vuelve a presionar
                return;
            }

            indexEtapa++;
            indexLinea = 0;
            //Debug.Log("El index de la etapa es " + indexEtapa + "     Despues de etapa++, el index de la linea se pone en 0");


            if (indexEtapa < etapasDialogo.Length)
            {
                StartCoroutine(MostrarTexto());
                //Debug.Log(indexEtapa);

            }
            else
            {
                TerminarDialogo();
            }
        }
    }

    IEnumerator MostrarTexto()
    { 
        textoDialogo.text = "";

        foreach (char ch in etapasDialogo[indexEtapa].lineas[indexLinea])
        {
            textoDialogo.text += ch;
            yield return new WaitForSeconds(velocidadEscritura);
        }
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
}

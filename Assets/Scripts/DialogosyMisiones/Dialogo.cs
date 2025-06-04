using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SonidosNpc
{
    public string nombreNpc; // debe coincidir con el tag (ej: "viejo", "herrero", etc.)
    public List<AudioClip> clips;
}

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

    public AudioSource audioSource;

    public AudioSource sourceGolpeHerrero;
    public AudioClip golepHerrero;

    [SerializeField] private List<SonidosNpc> sonidosPorNpc;
    private Dictionary<string, List<AudioClip>> diccionarioSonidos;
    private string npcActualTag;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        diccionarioSonidos = new Dictionary<string, List<AudioClip>>();

        foreach (var entrada in sonidosPorNpc)
        {
            diccionarioSonidos[entrada.nombreNpc] = entrada.clips;
        }
    }

    private void Update()
    {
        if (jugadorEnRango && Input.GetKeyDown(KeyCode.T))
        {
            DeterminarNpc();

            if (escribiendoTexto) return; // Evita interacci�n mientras se escribe

            
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

            // Verificar si necesita misi�n
            if (etapaActual.requiereMision && !ControlMisiones.Instance.EstaMisionCompletada(etapaActual.idMision))
            {
                TerminarDialogo();
                indexLinea--; // Para que se repita la l�nea si se reinicia el di�logo
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

        // Si a�n hay l�neas en la misma etapa
        StartCoroutine(MostrarTexto());
    }


    IEnumerator MostrarTexto()
    {
        escribiendoTexto = true;
        textoDialogo.text = "";

        if (diccionarioSonidos.TryGetValue(npcActualTag, out List<AudioClip> sonidos) && sonidos.Count > 0)
        {
            AudioClip clipAleatorio = sonidos[Random.Range(0, sonidos.Count)];
            audioSource.PlayOneShot(clipAleatorio);
        }

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
            // 1. Eliminar el �tem del inventario
            SistemaDeInventario.Instance.RemoveCantidad(metal, 1);

            // 2. Destruir el obst�culo del juego
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
        Image imagenTexto = panelDialogo.GetComponent<Image>();

        if (gameObject.CompareTag("viejo"))
        {
            imagenTexto.sprite = spritesDialogo[0];
            npcActualTag = "viejo";
        }
        else if (gameObject.CompareTag("herrero"))
        {
            imagenTexto.sprite = spritesDialogo[1];
            npcActualTag = "herrero";
        }
        else if (gameObject.CompareTag("Ahoy"))
        {
            imagenTexto.sprite = spritesDialogo[2];
            npcActualTag = "Ahoy";
        }
        else if (gameObject.CompareTag("Marinero"))
        {
            imagenTexto.sprite = spritesDialogo[3];
            npcActualTag = "Marinero";
        }
    }

    public void AudioGolpe()
    {
        sourceGolpeHerrero.PlayOneShot(golepHerrero);
    }
}

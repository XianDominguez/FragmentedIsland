using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolBar : MonoBehaviour
{

    public List<Sprite> toolBarSprites = new List<Sprite>();
    public List<GameObject> spritesArmas = new List<GameObject>();
    public Image toolBar;
    private bool[] armasDesbloqueadas;

    public GameObject soloMano;
    public GameObject espadaMano;
    public GameObject palaMano;
    public GameObject picoMano;
    public GameObject hachaMano;

    private void Start()
    {
        armasDesbloqueadas = new bool[4]; // espada, pico, pala, hacha
        toolBar.sprite = toolBarSprites[0];

        soloMano.SetActive(true);
        espadaMano.SetActive(false);
        palaMano.SetActive(false);
        picoMano.SetActive(false);
        hachaMano.SetActive(false);
    }

    void QuitarMano()
    {
        soloMano.SetActive(false);
        espadaMano.SetActive(false);
        palaMano.SetActive(false);
        picoMano.SetActive(false);
        hachaMano.SetActive(false);
    }

    public void DesbloquearArma(int indice)
    {
        if (indice >= 0 && indice < armasDesbloqueadas.Length)
        {
            armasDesbloqueadas[indice] = true;
            Debug.Log("Arma desbloqueada: " + indice);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1) && armasDesbloqueadas[0])
        {
            PersonajeAnimaciones personajeAnimaciones = FindObjectOfType<PersonajeAnimaciones>();

            personajeAnimaciones.EstadoAnimaciones();
            if(personajeAnimaciones.animacionEjecutandose == false)
            {
                toolBar.sprite = toolBarSprites[0];
                QuitarMano();
                espadaMano.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && armasDesbloqueadas[1])
        {
            PersonajeAnimaciones personajeAnimaciones = FindObjectOfType<PersonajeAnimaciones>();

            personajeAnimaciones.EstadoAnimaciones();
            if (personajeAnimaciones.animacionEjecutandose == false)
            {
                toolBar.sprite = toolBarSprites[1];
                QuitarMano();
                picoMano.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && armasDesbloqueadas[2])
        {
            PersonajeAnimaciones personajeAnimaciones = FindObjectOfType<PersonajeAnimaciones>();

            personajeAnimaciones.EstadoAnimaciones();

            if (personajeAnimaciones.animacionEjecutandose == false)
            {
                toolBar.sprite = toolBarSprites[2];
                QuitarMano();
                palaMano.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && armasDesbloqueadas[3])
        {
            PersonajeAnimaciones personajeAnimaciones = FindObjectOfType<PersonajeAnimaciones>();

            personajeAnimaciones.EstadoAnimaciones();

            if (personajeAnimaciones.animacionEjecutandose == false)
            {
                toolBar.sprite = toolBarSprites[3];
                QuitarMano();
                hachaMano.SetActive(true);
            }
        }
    }
}

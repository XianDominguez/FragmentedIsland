using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolBar : MonoBehaviour
{
    public PersonajeAnimaciones personajeAnimaciones;

    public List<Sprite> toolBarSprites = new List<Sprite>();
    public Image toolBar;

    public GameObject espadaMano;
    public GameObject palaMano;
    public GameObject picoMano;
    public GameObject hachaMano;

    private void Start()
    {
        toolBar.sprite = toolBarSprites[0];
        espadaMano.SetActive(true);
        palaMano.SetActive(false);
        picoMano.SetActive(false);
        hachaMano.SetActive(false);
    }

    void QuitarMano()
    {
        espadaMano.SetActive(false);
        palaMano.SetActive(false);
        picoMano.SetActive(false);
        hachaMano.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            personajeAnimaciones.EstadoAnimaciones();
            if(personajeAnimaciones.animacionEjecutandose == false)
            {
                toolBar.sprite = toolBarSprites[0];
                QuitarMano();
                espadaMano.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            personajeAnimaciones.EstadoAnimaciones();
            if (personajeAnimaciones.animacionEjecutandose == false)
            {
                toolBar.sprite = toolBarSprites[1];
                QuitarMano();
                picoMano.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            personajeAnimaciones.EstadoAnimaciones();

            if (personajeAnimaciones.animacionEjecutandose == false)
            {
                toolBar.sprite = toolBarSprites[2];
                QuitarMano();
                palaMano.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
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

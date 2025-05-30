using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    private GameObject armaActual = null;

    private void Start()
    {
        armasDesbloqueadas = new bool[4]; // espada, pico, pala, hacha
        toolBar.sprite = toolBarSprites[0];

        armaActual = soloMano.gameObject;
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
        if (Input.GetKeyDown(KeyCode.Alpha1) && armasDesbloqueadas[0] && !espadaMano.activeInHierarchy)
        {
            Animator animActual = armaActual.GetComponent<Animator>();
            AnimatorStateInfo stateInfo = animActual.GetCurrentAnimatorStateInfo(0);
            
            if (!stateInfo.IsName("Ataque 1") && !stateInfo.IsName("Ataque 2"))
            {
                StartCoroutine(CambiarArmaConAnimacion(espadaMano, 0));
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && armasDesbloqueadas[1] && !picoMano.activeInHierarchy)
        {
            Animator animActual = armaActual.GetComponent<Animator>();
            AnimatorStateInfo stateInfo = animActual.GetCurrentAnimatorStateInfo(0);

            if (!stateInfo.IsName("Ataque 1") && !stateInfo.IsName("Ataque 2"))
            {
                StartCoroutine(CambiarArmaConAnimacion(picoMano, 1));
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && armasDesbloqueadas[2] && !palaMano.activeInHierarchy)
        {
            Animator animActual = armaActual.GetComponent<Animator>();
            AnimatorStateInfo stateInfo = animActual.GetCurrentAnimatorStateInfo(0);
            if (!stateInfo.IsName("Ataque 1") && !stateInfo.IsName("Ataque 2"))
            {
                StartCoroutine(CambiarArmaConAnimacion(palaMano, 2));
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && armasDesbloqueadas[3] && !hachaMano.activeInHierarchy)
        {
            Animator animActual = armaActual.GetComponent<Animator>();
            AnimatorStateInfo stateInfo = animActual.GetCurrentAnimatorStateInfo(0);
            if (!stateInfo.IsName("Ataque 1") && !stateInfo.IsName("Ataque 2"))
            {
                StartCoroutine(CambiarArmaConAnimacion(hachaMano, 3));
            }
        }
    }

    IEnumerator CambiarArmaConAnimacion(GameObject nuevaArma, int spriteIndex)
    {
            Animator animActual = armaActual.GetComponent<Animator>();
            if (animActual != null)
            {
                animActual.Play("Guardar");

                yield return new WaitForSeconds(0.2f);

            }
            armaActual.SetActive(false);


        // Actualizar sprite en la UI
        toolBar.sprite = toolBarSprites[spriteIndex];

        // Activar nueva arma
        nuevaArma.SetActive(true);

        Animator animNueva = nuevaArma.GetComponent<Animator>();
        if (animNueva != null)
        {
            animNueva.Play("Sacar");
        }

        armaActual = nuevaArma;
    }

   
}
    
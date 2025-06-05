using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hacha : MonoBehaviour
{
    [Header("Madera")]
    [Space]
    public SumarMaterial sumarMaterial;
    public int golpesArbol;
    private Animator animatorArbol;
    [Header("Objetos")]
    [Space]
    GameObject arbolCae;
    GameObject toconArbol;
    [Header("Audio")]
    [Space]
    public AudioSource arbolCaeAudioSource;
    [Space]
    public AudioClip arbolCayendo;
    public AudioClip talar1;
    public AudioClip talar2;
    public AudioClip troncoTalado;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ArbolPuente")) //Detecta a que arbol le da el jugador
        {
            if (golpesArbol < 2)    //Suma los golpes
            {
                golpesArbol++;
                if(golpesArbol > 1)
                {
                    arbolCaeAudioSource.PlayOneShot(talar1);
                }
                else
                {
                    arbolCaeAudioSource.PlayOneShot(talar2);

                }
            }
            else //Al 3er golpe el arbol cae y el jugador obtiene madera
            {

                Transform arbolCaeTransform = other.transform.parent.Find("ArbolCortadoPuente");
                Transform toconArbolTransform = other.transform.parent.Find("ArbolCortadoTocon");

                arbolCae = arbolCaeTransform.gameObject;
                toconArbol = toconArbolTransform.gameObject;

                ItemObject itemObject = other.gameObject.GetComponent<ItemObject>();
                itemObject.CogerObjeto();
                sumarMaterial.AnimacionSumar(other);

                Destroy(other.gameObject);
                arbolCae.SetActive(true);
                toconArbol.SetActive(true);

                animatorArbol = arbolCae.GetComponent<Animator>();

                animatorArbol.Play("ArbolVa");
                arbolCaeAudioSource.PlayOneShot(arbolCayendo);

                golpesArbol = 0;
            }

        }

        if (other.gameObject.CompareTag("ArbolMadera"))
        {
            if (golpesArbol < 2)
            {
                golpesArbol++;
                if (golpesArbol > 1)
                {
                    arbolCaeAudioSource.PlayOneShot(talar1);
                }
                else
                {
                    arbolCaeAudioSource.PlayOneShot(talar2);

                }
            }
            else //Al 3er golpe el arbol se corta y el jugador obtiene madera
            {
                ItemObject itemObject = other.gameObject.GetComponent<ItemObject>();
                itemObject.CogerObjeto();

                Transform arbol = other.gameObject.transform.parent;
                Transform tocon = arbol.Find("ArbolCortadoTocon");
                tocon.gameObject.SetActive(true);

                sumarMaterial.AnimacionSumar(other);
                arbolCaeAudioSource.PlayOneShot(troncoTalado);

                golpesArbol = 0;
            }

        }

    }
}

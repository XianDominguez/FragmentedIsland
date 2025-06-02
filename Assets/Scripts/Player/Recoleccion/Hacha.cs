using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hacha : MonoBehaviour
{
    public SumarMaterial sumarMaterial;

    public int golpesArbol;
    private Animator animatorArbol;

    GameObject arbolCae;
    GameObject toconArbol;

    public AudioSource arbolCaeAudioSource;

    public AudioClip arbolCayendo;
    public AudioClip talar1;
    public AudioClip talar2;
    public AudioClip troncoTalado;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ArbolPuente"))
        {
            if (golpesArbol < 2)
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
            else
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
            else
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

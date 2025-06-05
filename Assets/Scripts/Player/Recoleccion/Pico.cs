using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Pico : MonoBehaviour
{
    [Header("Metal")]
    [Space]
    public int menaMetalRecogida;
    private int menasDeMetal;
    public SumarMaterial sumarMaterial;
    [Header("Sonido")]
    [Space]
    public AudioSource audioMenas;
    [Space]
    public AudioClip romperMena;
    public AudioClip picarMena;



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MenaMetal"))   //Detectra que pica metal
        {
            if (menaMetalRecogida < 2)  //Suma la variable para que el jugador tenga que dar 3 golpes
            {
                audioMenas.PlayOneShot(picarMena);

                menaMetalRecogida++;
            }
            else //Rompe la mena y suma el material
            {
                audioMenas.PlayOneShot(romperMena); 

                ItemObject itemObject = other.gameObject.GetComponent<ItemObject>();
                itemObject.CogerObjeto();

                Transform padre = other.gameObject.transform.parent;
                Transform MenaDestr = padre.Find("MenaDestr");
                MenaDestr.gameObject.SetActive(true);

                sumarMaterial.AnimacionSumar(other);
                menasDeMetal++;
                if(menasDeMetal == 3)
                {
                    ControlMisiones.Instance.CompletarMision("picar_hierro");

                }
                menaMetalRecogida = 0;
            }

        }
    }


}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Pico : MonoBehaviour
{
    public int menaMetalRecogida;
    public int piedraRecogida;

    private int menasDeMetal;

    public SumarMaterial sumarMaterial;

    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("PiedraPicable"))
        {
            if(piedraRecogida < Random.Range(2,5))
            {
                ItemObject itemObject = other.gameObject.GetComponent<ItemObject>();

                itemObject.CogerMultiplesObjetos();

                sumarMaterial.AnimacionSumar(other);

                piedraRecogida++;
            }
            else
            {
                ItemObject itemObject = other.gameObject.GetComponent<ItemObject>();
                itemObject.CogerObjeto();
                sumarMaterial.AnimacionSumar(other);


            }

        }

        if (other.gameObject.CompareTag("MenaMetal"))
        {
            if (menaMetalRecogida < 2)
            {
                menaMetalRecogida++;
            }
            else
            {
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

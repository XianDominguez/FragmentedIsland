using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Pico : MonoBehaviour
{
    public int piedraRecogida;

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
    }
 
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pico : MonoBehaviour
{
    public int piedraRecogida;

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
                Debug.Log("funciona");
                ItemObject itemObject = other.gameObject.GetComponent<ItemObject>();
                itemObject.CogerMultiplesObjetos();
                piedraRecogida++;
            }
            else
            {
                ItemObject itemObject = other.gameObject.GetComponent<ItemObject>();
                itemObject.CogerObjeto();

            }
           
        }
    }
}

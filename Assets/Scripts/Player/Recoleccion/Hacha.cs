using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hacha : MonoBehaviour
{

    public int golpesArbol;
    private Animator animatorArbol;

    GameObject arbolCae;
    GameObject toconArbol;

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
        if (other.gameObject.CompareTag("ArbolPuente"))
        {
            if (golpesArbol < 2)
            {
                golpesArbol++;
            }
            else
            {
                Transform arbolCaeTransform = other.transform.parent.Find("ArbolCortadoPuente");
                Transform toconArbolTransform = other.transform.parent.Find("ArbolCortadoTocon");

                arbolCae = arbolCaeTransform.gameObject;
                toconArbol = toconArbolTransform.gameObject;


                Destroy(other.gameObject);
                arbolCae.SetActive(true);
                toconArbol.SetActive(true);

                animatorArbol = arbolCae.GetComponent<Animator>();

                animatorArbol.Play("ArbolVa");
            }

        }

    }
}

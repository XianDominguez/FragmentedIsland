using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cofre : MonoBehaviour
{

    public List<InvetarioItemData> itemsCofre;

    public SumarMaterial sumarMaterial;


    public void RecogerCofre()
    {
        StartCoroutine(SumarCofre());
        
    }

    private IEnumerator SumarCofre()
    {
        foreach (InvetarioItemData item in itemsCofre)
        {
            SistemaDeInventario.Instance.Add(item);
            sumarMaterial.SumarCofre(item);
            yield return new WaitForSeconds(1f); // Delay entre cada ítem
        }

        Transform mats = transform.Find("Mats");
        Destroy(mats.gameObject);
    }
}

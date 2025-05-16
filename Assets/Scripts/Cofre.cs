using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cofre : MonoBehaviour
{

    public List<InvetarioItemData> itemsCofre;
 
    public void RecogerCofre()
    {
        foreach (InvetarioItemData item in itemsCofre)
        {
            SistemaDeInventario.Instance.Add(item);
        }
        Transform mats = transform.Find("Mats");
        Destroy(mats.gameObject); // o gameObject.SetActive(false);

    }
}

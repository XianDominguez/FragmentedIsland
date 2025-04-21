using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
   public InvetarioItemData ItemData;

    public void CogerObjeto()
    {
        SistemaDeInventario.Instance.Add(ItemData);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            CogerObjeto();
        }
    }
}

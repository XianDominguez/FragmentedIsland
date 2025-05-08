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

    public void CogerMultiplesObjetos()
    {
        SistemaDeInventario.Instance.Add(ItemData);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor.Purchasing;
using UnityEngine;

public class Inventario : MonoBehaviour
{
    public GameObject itemSlotPrefab;


    private void Awake()
    {
        SistemaDeInventario.Instance.onInventoryChangedCallback += OnUpdateInventory;
    }
    // Start is called before the first frame update
    void Start()
    {
        //SistemaDeInventario.Instance.onInventoryChangedCallback += OnUpdateInventory;
    }

    public void OnUpdateInventory()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.transform.gameObject);
        }
        DrawInventory();
    }

    public void DrawInventory()
    {
        foreach (InventoryItem item in SistemaDeInventario.Instance.inventario)
        {
                AddInventorySlot(item);
        }
    }

    public void AddInventorySlot(InventoryItem item)
    {
        GameObject obj = Instantiate(itemSlotPrefab);
        obj.transform.SetParent(transform, false);

        ItemSlot slot = obj.GetComponent<ItemSlot>();
        slot.Set(item);
    }

}

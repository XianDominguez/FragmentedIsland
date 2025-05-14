using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SistemaDeInventario : MonoBehaviour
{
    public static SistemaDeInventario Instance;

    public delegate void onInventoryChanged();
    public event onInventoryChanged onInventoryChangedCallback;

    private Dictionary<InvetarioItemData, InventoryItem> _itemDictionary;
    public List<InventoryItem> inventario;

    private void Awake()
    {
        inventario = new List<InventoryItem>();
        _itemDictionary = new Dictionary<InvetarioItemData, InventoryItem>();
        Instance = this;
    }   

    public void Add(InvetarioItemData itemData)
    {
        if (_itemDictionary.TryGetValue(itemData, out InventoryItem value))
        {
            Debug.Log("Sumar stack en item");
            value.AddStack();
            onInventoryChangedCallback.Invoke();
        }
        else
        {
            Debug.Log("Agregar nuevo item");
            InventoryItem newItem = new InventoryItem(itemData);
            inventario.Add(newItem);
            _itemDictionary.Add(itemData, newItem);
            onInventoryChangedCallback.Invoke();
        }
    }

    public void Remove(InvetarioItemData itemData)
    {
        if (_itemDictionary.TryGetValue(itemData, out InventoryItem value))
        {
            value.RemoveStack();

            if (value.tamanoStack == 0)
            {
                inventario.Remove(value);
                _itemDictionary.Remove(itemData);
            }

            onInventoryChangedCallback?.Invoke();
        }
    }

    public void RemoveCantidad(InvetarioItemData itemData, int cantidad)
    {
        if (_itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            // Evita que cantidad negativa rompa el sistema
            int cantidadARemover = Mathf.Min(cantidad, item.tamanoStack);
            item.tamanoStack -= cantidadARemover;

            if (item.tamanoStack <= 0)
            {
                inventario.Remove(item);
                _itemDictionary.Remove(itemData);
            }

            onInventoryChangedCallback?.Invoke();
        }
    }

}

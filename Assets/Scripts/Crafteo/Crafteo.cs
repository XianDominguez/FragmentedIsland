using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class Crafteo : MonoBehaviour
{

    public bool ComprobarPosibilidades(RecetasCrafteo receta, List<InventoryItem> inventoryItems)
    {
        // 1. Verificar si el jugador tiene todos los ítems necesarios
        foreach (var requerido in receta.requiredItems)
        {
            var inventarioItem = inventoryItems.Find(i => i.data.id == requerido.item.id);
            if (inventarioItem == null || inventarioItem.tamanoStack < requerido.requiredQuantity) //aqui falla
            {
                Debug.Log("Falta: " + requerido.item.nombreItem);
                return false;
            }
        }   
        return true;
    }

    public void CraftearTablones(RecetasCrafteo receta)
    {
        // 2. Restar los ítems del inventario
        foreach (var requerido in receta.requiredItems)
        {
            for (int i = 0; i < requerido.requiredQuantity; i++)
            {
                SistemaDeInventario.Instance.RemoveCantidad(requerido.item, 1); 
            }
        }

        
        // 3. Agregar el ítem resultante
        SistemaDeInventario.Instance.Add(receta.resultItem);
        

    }

    public void CraftearHacha(RecetasCrafteo receta)
    {
        // 2. Restar los ítems del inventario
        foreach (var requerido in receta.requiredItems)
        {
            for (int i = 0; i < requerido.requiredQuantity; i++)
            {
                SistemaDeInventario.Instance.RemoveCantidad(requerido.item, 1);
            }
        }

        // 3. Agregar el ítem resultante
        SistemaDeInventario.Instance.Add(receta.resultItem);

    }

    public void CraftearPico(RecetasCrafteo receta)
    {
        // 2. Restar los ítems del inventario
        foreach (var requerido in receta.requiredItems)
        {
            for (int i = 0; i < requerido.requiredQuantity; i++)
            {
                SistemaDeInventario.Instance.RemoveCantidad(requerido.item, 1);
            }
        }

        // 3. Agregar el ítem resultante
        SistemaDeInventario.Instance.Add(receta.resultItem);

    }
}

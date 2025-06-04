using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Horno : MonoBehaviour
{
    public RecetasCrafteo receta;
    public SistemaDeInventario sistemaDeInventario;

    public AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }
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

    public void IntentarCraftear()
    {
        bool exito = ComprobarPosibilidades(receta, sistemaDeInventario.inventario);

        if (exito)
        {
            Debug.Log("Puedes craftear metal");
            CraftearMetal(receta);
        }
        else
        {
            Debug.Log("No puedes craftear metal");

        }
    }


    public void CraftearMetal(RecetasCrafteo receta)
    {
        audioSource.Play();

        // Restar los ítems del inventario
        foreach (var requerido in receta.requiredItems)
        {
            for (int i = 0; i < requerido.requiredQuantity; i++)
            {
                SistemaDeInventario.Instance.RemoveCantidad(requerido.item, 1);
            }
        }

        // Agregar el ítem resultante
        SistemaDeInventario.Instance.Add(receta.resultItem);

        ControlMisiones.Instance.CompletarMision("forjar_lingote");

    }

}

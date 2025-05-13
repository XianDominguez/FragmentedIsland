using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crafteo : MonoBehaviour
{

    public Button craftearPico;

    private void Start()
    {
        craftearPico.enabled = false;
    }

    public void ComprobarPosibilidades()
    {
        List<InventoryItem> items = SistemaDeInventario.Instance.inventario;

    }
}

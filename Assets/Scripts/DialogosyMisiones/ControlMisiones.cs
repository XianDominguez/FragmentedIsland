using System.Collections.Generic;
using UnityEngine;

public class ControlMisiones : MonoBehaviour
{
    public static ControlMisiones Instance;

    [Header("Misiones disponibles")]
    public List<Mision> misiones = new List<Mision>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void CompletarMision(string id)
    {
        Mision mision = misiones.Find(m => m.id == id);
        if (mision != null)
        {
            mision.completada = true;
            Debug.Log($"Misión completada: {mision.nombre}");
        }
        else
        {
            Debug.LogWarning($"No se encontró la misión con ID: {id}");
        }
    }

    public bool EstaMisionCompletada(string id)
    {
        Mision mision = misiones.Find(m => m.id == id);
        return mision != null && mision.completada;
    }

    public Mision ObtenerMision(string id)
    {
        return misiones.Find(m => m.id == id);
    }
}


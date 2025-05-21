using UnityEngine;

[System.Serializable]
public class Mision
{
    public string id;               // ID �nica
    public string nombre;           // Nombre visible
    [TextArea] public string descripcion;  // Texto descriptivo
    public bool completada;         // Estado de la misi�n
}
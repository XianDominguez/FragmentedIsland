using UnityEngine;

[System.Serializable]
public class EtapaDialogo
{
    [TextArea(4, 6)] public string[] lineas;
    public bool requiereMision;
    public string idMision; // ID de la misión que se necesita para continuar
}

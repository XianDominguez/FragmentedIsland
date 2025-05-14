using UnityEngine;

[CreateAssetMenu(fileName = "Inventory Item Data", menuName = "Inventory Sistem/Create Item", order = 0)]
public class InvetarioItemData : ScriptableObject
{
    public string id;
    public string nombreItem;
    public Sprite iconoItem;
}

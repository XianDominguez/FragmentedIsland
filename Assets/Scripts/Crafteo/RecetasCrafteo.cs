using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Crafting/Recipe")]
public class RecetasCrafteo : ScriptableObject
{
    public string recipeName;
    public List<ItemsNecesarios> requiredItems;
    public InvetarioItemData resultItem;
    public int resultQuantity = 1;
}

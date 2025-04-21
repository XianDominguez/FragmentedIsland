[System.Serializable]
public class InventoryItem 
{
    public InvetarioItemData data;
    public int tamanoStack;

    public InventoryItem (InvetarioItemData itemData)
    {
        data = itemData;
        AddStack();
    }

    public void AddStack()
    {
        tamanoStack++;
    }

    public void RemoveStack() 
    { 
        tamanoStack--; 
    }
}

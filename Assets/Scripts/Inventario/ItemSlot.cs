using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _itemName;

    [SerializeField]
    private Image _itemIcon;

    [SerializeField]
    private GameObject _stackObj;

    [SerializeField]
    private TextMeshProUGUI _stackNumber;

    public void Set(InventoryItem item)
    {
        _itemName.text = item.data.nombreItem;
        _itemIcon.sprite = item.data.iconoItem;

        if(item.tamanoStack <= 1)
        {
            _stackObj.SetActive(false);
            return;
        }

        _stackNumber.text = item.tamanoStack.ToString();
    }
}

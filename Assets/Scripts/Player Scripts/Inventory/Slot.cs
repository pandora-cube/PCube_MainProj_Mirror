using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Image image;
    [SerializeField] Inventory inventory;

    [SerializeField] TMP
    private Item _item;
    public Item item {
        get { return _item; }
        set
        {
            _item = value;
            if (_item != null)
            {
                image.sprite = item.itemImage;
                image.color = new Color(1, 1, 1, 1);
            }
            else image.color = new Color(1, 1, 1, 0);
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (_item != null) inventory.UseItem(item);
    }
}
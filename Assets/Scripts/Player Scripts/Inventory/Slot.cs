using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Slot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image image;
    [SerializeField] Inventory inventory;

    [SerializeField] private TextMeshProUGUI NameText;
    [SerializeField] private TextMeshProUGUI InfoText;
    [SerializeField] private GameObject InfoScreen;

    [SerializeField] RectTransform infoRect;
    RectTransform slotRect;

    private Item _item;
    [SerializeField] PlayerController controller;
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

    void Start()
    {
        slotRect = GetComponent<RectTransform>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (controller.UsingItem && _item != null) inventory.UseItem(item); // 아이템이 존재하고, 현재 사용 가능할 때
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_item != null)
        {
            NameText.text = _item.itemName;
            InfoText.text = _item.itemInfo;

            infoRect.position = slotRect.position;
            InfoScreen.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InfoScreen.SetActive(false);
    }
}
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items;

    [SerializeField] private Transform slotParent;
    [SerializeField] public Slot[] slots;

    [SerializeField] private BoxCollider2D Exit;

#if UNITY_EDITOR
    private void OnValidate()
    {
        slots = slotParent.GetComponentsInChildren<Slot>(); 
    }
#endif
    private void Awake()
    {
        FreshSlot();
    }

    public void FreshSlot()
    {
        int i = 0;
        for (; i<items.Count && i < slots.Length; i++) { slots[i].item = items[i]; }
        for (; i< slots.Length; i++) { slots[i].item = null; }
    }

    public void AddItem(Item _item)
    {
        if (items.Count < slots.Length)
        {
            items.Add(_item);
            FreshSlot();
        }
        else Debug.Log("슬롯이 가득 참");
    }

    public bool FindItem(string itemName)
    {
        //아이템 name으로 찾은 후, 사용
        for (int i = 0; i<items.Count; i++) 
            if (slots[i].item.itemName == itemName)
            {
                UseItem(slots[i].item);
                return true;
            }
        return false;
    }

    public void UseItem(Item _item)
    {
        if (items.Contains(_item))
        {
            Debug.Log(_item.name);
            items.Remove(_item);
            FreshSlot();

            // 아이템 사용 로직 추가
            if (_item.name == "keyItem")
            {
                Exit.isTrigger = true; // 출구 오픈
            }
        }
        else Debug.Log("해당 아이템이 없음");
    }
}

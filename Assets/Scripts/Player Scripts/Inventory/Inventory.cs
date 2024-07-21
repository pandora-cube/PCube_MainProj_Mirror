using System.Collections;
using System.Collections.Generic;
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

    public void UseItem(Item _item)
    {
        if (items.Contains(_item))
        {
            Debug.Log(_item.name);
            items.Remove(_item);
            FreshSlot();
            // 아이템 사용 로직 추가
            if (_item.name == "keyItem") Exit.isTrigger = true;
        }
        else Debug.Log("해당 아이템이 없음");
    }
}

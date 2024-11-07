using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pickup : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactText;
    [SerializeField] private Item item;

    private Inventory inventory => Inventory.instance;
    public void Interact()
    {
        if (inventory.items.Count < inventory.slots.Length && !inventory.FindItem(item) && !PlayerStateMachine.instance.isGhost)
        {
            inventory.AddItem(item);
            //Destroy(gameObject);
            Debug.Log("Interacted");
        }
    
    }

    public Transform GetTransform()
    {
        return transform;
    }
}

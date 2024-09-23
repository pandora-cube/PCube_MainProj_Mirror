using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pickup : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactText;
    [SerializeField] private Item item;
    [SerializeField] private Inventory inventory;
    [SerializeField] private PlayerController playerController;
    
    public void Interact()
    {
        if (inventory.items.Count < inventory.slots.Length && !inventory.FindItem(item) && !playerController.isGhost)
        {
            inventory.AddItem(item);
            //Destroy(gameObject);
            Debug.Log("Interacted");
        }
    
    }

    public string GetInteractText()
    {
        return interactText;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}

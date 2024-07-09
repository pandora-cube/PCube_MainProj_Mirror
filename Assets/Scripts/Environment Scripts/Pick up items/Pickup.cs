using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pickup : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactText;
    [SerializeField] private Item item;
    // Start is called before the first frame update
    public void Interact(Transform interactorTransform)
    {
        Inventory inventory = FindObjectOfType<Inventory>();

        if (inventory.items.Count < inventory.slots.Length)
        {
            inventory.AddItem(item);
            Destroy(gameObject);
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

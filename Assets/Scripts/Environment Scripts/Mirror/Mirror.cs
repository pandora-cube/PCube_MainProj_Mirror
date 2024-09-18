using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactText;
    public InteractableObject interactableObject;
    public void Interact()
    {
        PlayerController player = FindObjectOfType<PlayerController>();

        player.Transform();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactText;
    public InteractableObject interactableObject;

    void Start()
    {

    }
    public void Interact()
    {
        PlayerInteractionController player = FindObjectOfType<PlayerInteractionController>();

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

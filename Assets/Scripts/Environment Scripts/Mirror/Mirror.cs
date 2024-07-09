using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Mirror : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactText;
    public InteractableObject interactableObject;
    public void Interact(Transform interactorTransform)
    {
        PlayerController player = FindAnyObjectByType<PlayerController>();

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

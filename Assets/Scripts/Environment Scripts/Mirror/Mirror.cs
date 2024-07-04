using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactText;
    public void Interact(Transform interactorTransform)
    {
        PlayerController playerController = FindObjectOfType<PlayerController>();
        playerController.Transform();
        Debug.Log("Interacted");
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactText;
    public InteractableObject interactableObject;
    [SerializeField] private PlayerGhostHealthManager playerGhostHealthManager;

    void Start()
    {
        if (playerGhostHealthManager == null)
        {
            Debug.Log("player ghost health manager is null! gameObject: " + gameObject.name);
        }
    }
    public void Interact()
    {
        PlayerController player = FindObjectOfType<PlayerController>();

        player.Transform();
        playerGhostHealthManager.ghostTimeLimit = 60f;
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

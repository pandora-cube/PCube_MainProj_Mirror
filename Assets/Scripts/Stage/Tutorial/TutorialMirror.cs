using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMirror : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactText;
    public InteractableObject interactableObject;
    [SerializeField] Tutorial tutorial;
    [SerializeField] PlayerGhostHealthManager playerGhostHealthManager;
    private bool tutoPlay = true;

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
        playerGhostHealthManager.ghostTimeLimit = Mathf.Infinity;

        if (tutoPlay)
        {
            tutoPlay = false;
            if(player.isGhost) tutorial.StartCoroutine(tutorial.Tuto3_ghost());
            else tutorial.StartCoroutine(tutorial.Tuto6_normal());
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

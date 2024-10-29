using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMirror : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactText;
    public InteractableObject interactableObject;
    [SerializeField] Tutorial tutorial;
    private bool tutoPlay = true;

    void Start()
    {

    }
        
    public void Interact()
    {
        PlayerInteractionController player = FindObjectOfType<PlayerInteractionController>();
        player.Transform();

        if (tutoPlay)
        {
            tutoPlay = false;
            if(PlayerStateMachine.instance.isGhost) tutorial.StartCoroutine(tutorial.Tuto3_ghost());
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

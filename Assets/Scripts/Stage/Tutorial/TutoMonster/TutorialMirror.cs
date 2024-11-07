using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMirror : PlayerTutorial, IInteractable
{
    void Start()
    {

    }
        
    public void Interact()
    {
        PlayerInteractionController player = FindObjectOfType<PlayerInteractionController>();
        player.Transform();

        ReadyForTutorial();
    }

    void ReadyForTutorial()
    {
        base.PlayTutorial();
    }

    public Transform GetTransform()
    {
        return transform;
    }
}

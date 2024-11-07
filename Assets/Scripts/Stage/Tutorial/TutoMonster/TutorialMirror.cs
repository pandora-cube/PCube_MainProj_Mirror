using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMirror : PlayerTutorial, IInteractable
{        
    public void Interact()
    {
        PlayerInteractionController player = FindObjectOfType<PlayerInteractionController>();
        player.Transform();

        if(!thisTutoPlaying) ReadyForTutorial();
    }

    void ReadyForTutorial()
    {
        thisTutoPlaying = true;
        base.PlayTutorial();
    }

    public override void ExitTutorial()
    {
        //
    }

    public Transform GetTransform()
    {
        return transform;
    }
}

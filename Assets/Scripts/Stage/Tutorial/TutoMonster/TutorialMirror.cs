using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TutorialMirror : PlayerTutorial, IInteractable
{   
    [HideInInspector] public bool tuto6 = false;
    public virtual void Interact()
    {
        PlayerInteractionController player = FindObjectOfType<PlayerInteractionController>();
        player.Transform();

        if (tuto6 && !thisTutoPlaying) // tuto6 -> tuto5 대사를 봐야 true로 변환
        {
            tuto6 = false;
            base.PlayTutorial();
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }
}

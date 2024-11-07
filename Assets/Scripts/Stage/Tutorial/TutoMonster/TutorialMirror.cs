using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TutorialMirror : PlayerTutorial, IInteractable
{
    public GameObject CamCollider;
    PlayerStateMachine PlayerState => PlayerStateMachine.instance;
    [SerializeField] private PlayerGhostHealthManager playerGhostHealthManager;
    [SerializeField] TutorialFlowre[] tutorialFlowres;
    int dieFlowre = 0;
    public void Interact()
    {
        PlayerInteractionController player = FindObjectOfType<PlayerInteractionController>();
        player.Transform();

        if(!thisTutoPlaying) ReadyForTutorial();
    }

    void ReadyForTutorial()
    {
        //base.PlayTutorial();
        thisTutoPlaying = true;
        StartCoroutine(Tuto3_ghost());
    }

    public override IEnumerator StartTutorialDialog()
    {
        if (dollyTrack != null) dollyTrack.ActivateMyDollyTrack();

        yield return dialogSystem.StartCoroutine(dialogSystem.DialogProgress());

        if (dieFlowre == 6) ExitTutorial();
    }
    public override void ExitTutorial()
    {
        base.ExitTutorial();
    }

    private IEnumerator Tuto3_ghost()
    {
        CamCollider.SetActive(true);

        yield return StartCoroutine(StartTutorialDialog());

        PlayerState.canMove = false;
        playerGhostHealthManager.SetGhostTimeLimit(Mathf.Infinity);
        foreach (var flowre in tutorialFlowres) flowre.OpenFlowre = true;

        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(StartTutorialDialog());
    }

    public void Tuto4_flowre()
    {
        dieFlowre++;
        if (dieFlowre == 6) StartCoroutine(StartTutorialDialog());
        CamCollider.SetActive(false);
    }

    public Transform GetTransform()
    {
        return transform;
    }
}

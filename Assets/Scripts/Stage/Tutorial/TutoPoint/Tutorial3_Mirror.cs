using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Tutorial3_Mirror : TutorialMirror
{
    int dieFlowre = 0;
    public GameObject CamCollider;
    PlayerStateMachine PlayerState => PlayerStateMachine.instance;
    [SerializeField] private PlayerGhostHealthManager playerGhostHealthManager;
    [SerializeField] TutorialFlowre[] tutorialFlowres;

    public override void Interact()
    {
        base.Interact();
        
        if (!thisTutoPlaying) ReadyForTutorial(); // tuto3
    }

    public override IEnumerator StartTutorialDialog()
    {
        if (cameraPosition != null)cameraPosition.ActiveCameraProduction();

        yield return dialogSystem.StartCoroutine(dialogSystem.DialogProgress());

        if (dieFlowre == 6) ExitTutorial();
    }
    public override void ExitTutorial()
    {
        CamCollider.SetActive(false); // 해당 튜토 끝
        base.ExitTutorial();
    }

    void ReadyForTutorial()
    {
        //base.PlayTutorial();
        thisTutoPlaying = true;
        StartCoroutine(Tuto3_ghost());
    }

    private IEnumerator Tuto3_ghost()
    {
        CamCollider.SetActive(true); // 카메라 밖으로 움직일 수 없도록

        yield return StartCoroutine(StartTutorialDialog()); // 대화

        PlayerState.canMove = false; // 연출동안 움직임 제어
        playerGhostHealthManager.SetGhostTimeLimit(Mathf.Infinity); // ghost Limit Time 무한으로 설정
        foreach (var flowre in tutorialFlowres)
            if (flowre != null) flowre.OpenFlowre = true; // 튜토리얼 플로레 오픈

        yield return new WaitForSeconds(2f); // 2초 후 다시 대사

        yield return StartCoroutine(StartTutorialDialog());
    }

    public void Tuto4_flowre()
    {
        dieFlowre++; // 튜토리얼 플로레가 죽을때마다 +1
        if (dieFlowre == 6) StartCoroutine(StartTutorialDialog()); // 6마리가 다 죽으면 대사 시작
    }
}

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
        CamCollider.SetActive(false); // �ش� Ʃ�� ��
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
        CamCollider.SetActive(true); // ī�޶� ������ ������ �� ������

        yield return StartCoroutine(StartTutorialDialog()); // ��ȭ

        PlayerState.canMove = false; // ���⵿�� ������ ����
        playerGhostHealthManager.SetGhostTimeLimit(Mathf.Infinity); // ghost Limit Time �������� ����
        foreach (var flowre in tutorialFlowres)
            if (flowre != null) flowre.OpenFlowre = true; // Ʃ�丮�� �÷η� ����

        yield return new WaitForSeconds(2f); // 2�� �� �ٽ� ���

        yield return StartCoroutine(StartTutorialDialog());
    }

    public void Tuto4_flowre()
    {
        dieFlowre++; // Ʃ�丮�� �÷η��� ���������� +1
        if (dieFlowre == 6) StartCoroutine(StartTutorialDialog()); // 6������ �� ������ ��� ����
    }
}

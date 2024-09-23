using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] DialogSystem dialogSystem;
    CinemachineStateDrivenCamera stateCam;
    [SerializeField] CinemachineVirtualCamera tuto1Cam;
    int dieFlowre = 0;
    [SerializeField] TutorialFlowre[] tutorialFlowres;
    [SerializeField] private PlayerGhostHealthManager playerGhostHealthManager;

    private void Awake()
    {
        stateCam = GetComponent<CinemachineStateDrivenCamera>();
    }

    public IEnumerator Tuto1_flower()
    {
        stateCam.Priority = 15; tuto1Cam.Priority = 10;
        yield return dialogSystem.StartCoroutine(dialogSystem.DialogProgress());
        stateCam.Priority = 0; tuto1Cam.Priority = 0;
    }

    public IEnumerator Tuto2_mirror()
    {
        yield return dialogSystem.StartCoroutine(dialogSystem.DialogProgress());
    }

    public IEnumerator Tuto3_ghost()
    {
        stateCam.Priority = 15; tuto1Cam.Priority = 10;
        yield return dialogSystem.StartCoroutine(dialogSystem.DialogProgress());
        // ghost infinity
        playerGhostHealthManager.SetGhostTimeLimit(Mathf.Infinity);
        foreach (var flowre in tutorialFlowres) flowre.OpenFlowre = true;
        yield return new WaitForSeconds(2f);

        yield return dialogSystem.StartCoroutine(dialogSystem.DialogProgress());
    }

    public void Tuto4_flowre()
    {
        dieFlowre++;
        if (dieFlowre == 6 && dialogSystem != null)
        {
            dialogSystem.StartCoroutine(dialogSystem.DialogProgress());
            stateCam.Priority = 0; tuto1Cam.Priority = 0;
        }
    }

    public IEnumerator Tuto5_ghost()
    {
        yield return dialogSystem.StartCoroutine(dialogSystem.DialogProgress());
        // ghost 60f
        playerGhostHealthManager.SetGhostTimeLimit(60f);
        yield return new WaitForSeconds(20f);
        //isGhost�� die
    }

    public IEnumerator Tuto6_normal()
    {
        yield return dialogSystem.StartCoroutine(dialogSystem.DialogProgress());
    }

    public IEnumerator Tuto7_torch()
    {
        yield return dialogSystem.StartCoroutine(dialogSystem.DialogProgress());
    }
}

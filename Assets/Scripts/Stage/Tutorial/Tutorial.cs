using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] DialogSystem dialogSystem;
    CinemachineStateDrivenCamera stateCam;
    [SerializeField] CinemachineVirtualCamera mirrorCam;
    [SerializeField] CinemachineVirtualCamera radixCam;
    [SerializeField] GameObject CamCollider;
    [SerializeField] PlayerController playerController;
    int dieFlowre = 0;
    [SerializeField] TutorialFlowre[] tutorialFlowres;
    [SerializeField] private PlayerGhostHealthManager playerGhostHealthManager;

    private void Awake()
    {
        stateCam = GetComponent<CinemachineStateDrivenCamera>();
    }

    void Start()
    {
        if (playerGhostHealthManager == null) Debug.LogError("playerGhostHealthManager is null! GameObject: " + gameObject.name);
    }
    public IEnumerator Tuto1_flower()
    {
        stateCam.Priority = 15; mirrorCam.m_Lens.OrthographicSize = 8f;
        yield return dialogSystem.StartCoroutine(dialogSystem.DialogProgress());
        stateCam.Priority = 0;
    }

    public IEnumerator Tuto2_mirror()
    {
        yield return dialogSystem.StartCoroutine(dialogSystem.DialogProgress());
    }

    public IEnumerator Tuto3_ghost()
    {
        mirrorCam.m_Lens.OrthographicSize = 18f;
        stateCam.Priority = 15; CamCollider.SetActive(true);
        yield return dialogSystem.StartCoroutine(dialogSystem.DialogProgress());
        // ghost infinity
        playerController.canMove = false;
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
            stateCam.Priority = 0; CamCollider.SetActive(false);
        }
    }

    public IEnumerator Tuto5_ghost()
    {
        yield return dialogSystem.StartCoroutine(dialogSystem.DialogProgress());
        // ghost 60f
        playerGhostHealthManager.SetGhostTimeLimit(60f);
        yield return new WaitForSeconds(20f);
        //if(playerController.isGhost) playerGhostHealthManager.Die();
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

    public IEnumerator Tuto8_radix()
    {
        playerController.canMove = false;
        yield return new WaitForSeconds(0.5f);
        stateCam.Priority = 15; radixCam.Priority = 20;
        yield return dialogSystem.StartCoroutine(dialogSystem.DialogProgress());
        stateCam.Priority = 0; radixCam.Priority = 0;
    }
}

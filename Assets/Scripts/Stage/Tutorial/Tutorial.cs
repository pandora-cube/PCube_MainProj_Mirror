using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] DialogSystem dialogSystem;
    [SerializeField] GameObject CamCollider;
    [SerializeField] CinemachineVirtualCamera mirrorCam;
    [SerializeField] PlayerStateMachine playerStateMachine;
    CinemachineStateDrivenCamera stateCam;
    Animator animator;
    
    int dieFlowre = 0;
    [SerializeField] TutorialFlowre[] tutorialFlowres;
    [SerializeField] private PlayerGhostHealthManager playerGhostHealthManager;

    private void Awake()
    {
        stateCam = GetComponent<CinemachineStateDrivenCamera>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        if (playerGhostHealthManager == null) Debug.LogError("playerGhostHealthManager is null! GameObject: " + gameObject.name);
    }
    public IEnumerator Tuto1_flower()
    {
        stateCam.Priority = 15;
        animator.Play("Mirror");
        mirrorCam.m_Lens.OrthographicSize = 8f;

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

        playerStateMachine.canMove = false;
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
            stateCam.Priority = 0; mirrorCam.Priority = 0; CamCollider.SetActive(false);
        }
    }

    public IEnumerator Tuto5_ghost()
    {
        yield return dialogSystem.StartCoroutine(dialogSystem.DialogProgress());
        
        playerGhostHealthManager.SetGhostTimeLimit(60f);
        yield return new WaitForSeconds(20f);
        
        //if ghost -> die
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
        playerStateMachine.canMove = false;
        animator.Play("Radix");

        yield return new WaitForSeconds(0.5f);

        stateCam.Priority = 15;

        yield return new WaitForSeconds(1.5f);

        yield return dialogSystem.StartCoroutine(dialogSystem.DialogProgress());
        stateCam.Priority = 0;
    }
}

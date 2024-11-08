using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private DialogSystem dialogSystem => DialogSystem.instance;
    [SerializeField] GameObject CamCollider;
    
    int dieFlowre = 0;
    [SerializeField] private TutorialFlowre[] tutorialFlowres;
    [SerializeField] private PlayerGhostHealthManager playerGhostHealthManager;
    PlayerStateMachine PlayerState => PlayerStateMachine.instance;

    private void Awake()
    {
        
    }

    public IEnumerator Tuto3_ghost()
    {
        CamCollider.SetActive(true);

        yield return dialogSystem.StartCoroutine(dialogSystem.DialogProgress());

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
        PlayerState.canMove = false;
        //animator.Play("Radix");

        yield return new WaitForSeconds(0.5f);

        //stateCam.Priority = 15;

        yield return new WaitForSeconds(1.5f);

        yield return dialogSystem.StartCoroutine(dialogSystem.DialogProgress());
        //stateCam.Priority = 0;
    }
}

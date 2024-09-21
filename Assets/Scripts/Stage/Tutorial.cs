using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] DialogSystem dialogSystem;
    CinemachineStateDrivenCamera stateCam;
    [SerializeField] CinemachineVirtualCamera tuto1Cam;

    private void Awake()
    {
        stateCam = GetComponent<CinemachineStateDrivenCamera>();
    }
    private void Start()
    {
        
    }
    public IEnumerator Tuto1_flower()
    {
        stateCam.Priority = 15;
        tuto1Cam.Priority = 10;
        yield return dialogSystem.StartCoroutine(dialogSystem.DialogProgress());
        stateCam.Priority = 0;
        tuto1Cam.Priority = 0;
    }

    public IEnumerator Tuto2_mirror()
    {
        yield return null;
    }
}

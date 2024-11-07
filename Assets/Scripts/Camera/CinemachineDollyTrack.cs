using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineDollyTrack : MonoBehaviour
{
    public CinemachineVirtualCamera dollyCamera; // 참조할 카메라
    public CinemachinePath myDollyTrack; // 해당 오브젝트의 dollyTrack
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            ActivateMyDollyTrack();
        }
    }

    private void ActivateMyDollyTrack()
    {
        var dollyCart = dollyCamera.GetComponent<CinemachineDollyCart>();
        dollyCart.m_Path = myDollyTrack; // 이 오브젝트의 돌리 트랙을 할당
        dollyCart.m_Position = 0; // 트랙 시작 지점으로 초기화
        dollyCamera.Priority = 10; // 우선순위 설정하여 활성화
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            dollyCamera.Priority = 0; // 우선순위를 처음으로
        }
    }
}

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineDollyTrack : MonoBehaviour
{
    public CinemachineVirtualCamera dollyCamera; // 참조할 카메라
    public CinemachinePath myDollyTrack; // 해당 오브젝트의 dollyTrack

    PlayerCameraController cameraController => PlayerCameraController.instance;

    public void ActivateMyDollyTrack()
    {
        var dollyCart = dollyCamera.GetComponent<CinemachineDollyCart>();

        // 현재 카메라 위치를 0번째 waypoint로 설정
        myDollyTrack.m_Waypoints[0].position = myDollyTrack.transform.InverseTransformPoint(cameraController.CurCameraPosition().position); 
        
        dollyCart.m_Path = myDollyTrack; // 이 오브젝트의 돌리 트랙을 할당
        dollyCart.m_Position = 0; // 트랙 시작 지점으로 초기화
        cameraController.StartProductionCamera();
    }

    void Update()
    {
        // 카메라의 회전을 고정 
        dollyCamera.transform.rotation = Quaternion.Euler(0f, 0f, dollyCamera.transform.eulerAngles.z);
    }


    public void ExitCameraProduction()
    {
        cameraController.ReturnCameraPosition();
    }
}

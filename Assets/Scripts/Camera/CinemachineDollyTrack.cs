using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineDollyTrack : MonoBehaviour
{
    public CinemachineVirtualCamera dollyCamera; // 참조할 카메라
    public CinemachinePath[] myDollyTrack; // 해당 오브젝트의 dollyTrack
    CinemachineDollyCart dollyCart;

    PlayerCameraController cameraController => PlayerCameraController.instance;
    
    private void Awake()
    {
        dollyCart = dollyCamera.GetComponent<CinemachineDollyCart>();
    }

    public void ActivateMyDollyTrack(int cameraPathIndex)
    {
        // 현재 카메라 위치를 0번째 waypoint로 설정
        if (myDollyTrack == null) Debug.Log("dolly track is null");
        if (dollyCamera == null) Debug.Log("dolly camera is null");

        myDollyTrack[cameraPathIndex].m_Waypoints[0].position = myDollyTrack[cameraPathIndex].transform.InverseTransformPoint(dollyCamera.transform.position);
        dollyCart.m_Position = 0; // 트랙 시작 지점으로 초기화
        dollyCart.m_Speed = 15f;
        dollyCart.m_Path = myDollyTrack[cameraPathIndex];

        //Invoke(nameof(temp), 3f);
        //cameraController.ChangeAnimationState("Production");
    }

    public void TurnProductionCamera(Vector3 startPosition)
    {
        PlayerStateMachine.instance.canMove = false;
        cameraController.SetProductionCamera(startPosition, 10);
        cameraController.StartProductionCamera();
    }

    public void ExitProductionCamera()
    {
        PlayerStateMachine.instance.canMove = true;
        cameraController.ReturnCameraPosition();
    }

    void Update()
    {
        // 카메라의 회전을 고정 
        //dollyCamera.transform.rotation = Quaternion.Euler(0f, 0f, dollyCamera.transform.eulerAngles.z);
        dollyCamera.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }
}

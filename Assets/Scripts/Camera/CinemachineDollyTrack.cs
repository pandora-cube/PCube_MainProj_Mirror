using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineDollyTrack : MonoBehaviour
{
    public CinemachineVirtualCamera dollyCamera; // ������ ī�޶�
    public CinemachinePath myDollyTrack; // �ش� ������Ʈ�� dollyTrack

    PlayerCameraController cameraController => PlayerCameraController.instance;

    public void ActivateMyDollyTrack()
    {
        var dollyCart = dollyCamera.GetComponent<CinemachineDollyCart>();

        // ���� ī�޶� ��ġ�� 0��° waypoint�� ����
        myDollyTrack.m_Waypoints[0].position = myDollyTrack.transform.InverseTransformPoint(cameraController.CurCameraPosition().position); 
        
        dollyCart.m_Path = myDollyTrack; // �� ������Ʈ�� ���� Ʈ���� �Ҵ�
        dollyCart.m_Position = 0; // Ʈ�� ���� �������� �ʱ�ȭ
        cameraController.StartProductionCamera();
    }

    void Update()
    {
        // ī�޶��� ȸ���� ���� 
        dollyCamera.transform.rotation = Quaternion.Euler(0f, 0f, dollyCamera.transform.eulerAngles.z);
    }


    public void ExitCameraProduction()
    {
        cameraController.ReturnCameraPosition();
    }
}

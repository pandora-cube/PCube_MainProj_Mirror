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
        dollyCart.m_Path = myDollyTrack; // �� ������Ʈ�� ���� Ʈ���� �Ҵ�
        dollyCart.m_Position = 0; // Ʈ�� ���� �������� �ʱ�ȭ
        cameraController.StartProductionCamera();
    }

    public void ExitCameraProduction()
    {
        cameraController.ReturnCameraPosition();
    }
}

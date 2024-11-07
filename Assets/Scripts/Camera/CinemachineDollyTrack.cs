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

    void Update()
    {
        // ī�޶��� ȸ���� ���� (2D������ Z�ุ ����)
        dollyCamera.transform.rotation = Quaternion.Euler(0f, 0f, dollyCamera.transform.eulerAngles.z);
    }


    public void ExitCameraProduction()
    {
        cameraController.ReturnCameraPosition();
    }
}

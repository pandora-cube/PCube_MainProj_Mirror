using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineDollyTrack : MonoBehaviour
{
    public CinemachineVirtualCamera dollyCamera; // ������ ī�޶�
    public CinemachinePath myDollyTrack; // �ش� ������Ʈ�� dollyTrack

    PlayerCameraController cameraController => PlayerCameraController.instance;

    public void ActivateMyDollyTrack(Vector3 startPosition)
    {
        var dollyCart = dollyCamera.GetComponent<CinemachineDollyCart>();
        // ���� ī�޶� ��ġ�� 0��° waypoint�� ����

        dollyCart.m_Path = myDollyTrack; // �� ������Ʈ�� ���� Ʈ���� �Ҵ�
        dollyCart.m_Position = 0; // Ʈ�� ���� �������� �ʱ�ȭ
        dollyCart.m_Speed = 1f;

        cameraController.SetProductionCamera(startPosition, 10);
        cameraController.ChangeAnimationState("Production");
    }

    void temp(Vector3 startPosition)
    {
        cameraController.StartProductionCamera();
        Invoke(nameof(ExitCameraProduction), 3f);
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

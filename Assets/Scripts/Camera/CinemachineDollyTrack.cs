using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineDollyTrack : MonoBehaviour
{
    public CinemachineVirtualCamera dollyCamera; // ������ ī�޶�
    public CinemachinePath myDollyTrack; // �ش� ������Ʈ�� dollyTrack
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
        dollyCart.m_Path = myDollyTrack; // �� ������Ʈ�� ���� Ʈ���� �Ҵ�
        dollyCart.m_Position = 0; // Ʈ�� ���� �������� �ʱ�ȭ
        dollyCamera.Priority = 10; // �켱���� �����Ͽ� Ȱ��ȭ
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            dollyCamera.Priority = 0; // �켱������ ó������
        }
    }
}

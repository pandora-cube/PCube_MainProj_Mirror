using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PassageCameraTrack : MonoBehaviour
{
    [SerializeField] private CinemachinePath path;
    [SerializeField] private float speed;
    private CinemachineDollyCart dollyCart;

    void Start()
    {
        dollyCart = GetComponent<CinemachineDollyCart>();
        
        if (dollyCart != null)
        {
            dollyCart.m_Path = path;
            dollyCart.m_Speed = speed;
            dollyCart.m_PositionUnits = CinemachinePathBase.PositionUnits.Distance;
        }
    }

    void Update()
    {
        if (dollyCart != null && dollyCart.m_Path != null)
        {
            // 경로의 끝에 도달하면 이동 멈춤
            if (dollyCart.m_Position >= dollyCart.m_Path.PathLength)
            {
                dollyCart.m_Speed = 0f;
            }
        }
    }
}

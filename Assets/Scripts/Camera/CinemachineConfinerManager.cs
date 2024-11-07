using Cinemachine;
using UnityEditor;
using UnityEngine;

public class CinemachineConfinerManager : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D[] confiner2D;
    [SerializeField] private PlayerHorizontalMovement playerHorizontalMovement;

    int currentStage;
    int newStage;

    void Start()
    {
        currentStage = ProgressData.Instance.playerData.currentStage;
        playerHorizontalMovement.currentConfinerCollider = confiner2D[currentStage - 1];
        CinemachineConfiner2D[] confiners = GetComponentsInChildren<CinemachineConfiner2D>();
        foreach (CinemachineConfiner2D confiner in confiners)
        {
            if (confiner != null)
            {
                confiner.m_BoundingShape2D = confiner2D[currentStage - 1];
            }
        }

    }


    void Update()
    {
        newStage = ProgressData.Instance.playerData.currentStage;
        if (currentStage != newStage)
        {
            currentStage = newStage;
            CinemachineConfiner2D[] confiners = GetComponentsInChildren<CinemachineConfiner2D>();
            foreach (CinemachineConfiner2D confiner in confiners)
            {
                if (confiner == null) continue;

                confiner.m_BoundingShape2D = confiner2D[currentStage - 1];
            }
        }
        playerHorizontalMovement.currentConfinerCollider = confiner2D[currentStage - 1];

    }
}

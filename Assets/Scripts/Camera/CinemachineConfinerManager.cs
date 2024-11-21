using Cinemachine;
using UnityEditor;
using UnityEngine;

public class CinemachineConfinerManager : MonoBehaviour
{
    public static CinemachineConfinerManager instance;
    [SerializeField] private PolygonCollider2D[] confiner2D;
    [SerializeField] private PlayerHorizontalMovement playerHorizontalMovement;

    int currentStage, newStage;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(instance);
    }
    void Start()
    {
        ChangeStage();
    }

    public void UpdateConfinerSize()
    {
        CinemachineConfiner2D[] confiners = GetComponentsInChildren<CinemachineConfiner2D>();
        foreach (CinemachineConfiner2D confiner in confiners)
        {
            if (confiner == null) continue;
            confiner.InvalidateCache(); // ��� ĳ�� �ʱ�ȭ
            confiner.m_BoundingShape2D = confiner2D[currentStage - 1];
        }
    }

    public void ChangeStage()
    {
        newStage = ProgressData.Instance.playerData.currentStage;
        if (currentStage != newStage)
        {
            currentStage = newStage;
            UpdateConfinerSize();
        }
        playerHorizontalMovement.currentConfinerCollider = confiner2D[currentStage - 1];
    }
}

using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class CinemachineConfinerManager : MonoBehaviour
{
    [SerializeField] private BoxCollider2D[] confiner2D;

    int currentStage;
    int newStage;

    void Start()
    {
        currentStage = ProgressData.Instance.playerData.currentStage;
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
                if (confiner != null)
                {
                    confiner.m_BoundingShape2D = confiner2D[currentStage];
                }
            }
        }
    }
}

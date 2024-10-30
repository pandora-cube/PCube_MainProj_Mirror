using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowreAnimationController : MonoBehaviour
{
    private Animator flowreAnimator;
    private string currentState;

    public enum FlowreAnimationStates
    {
        flowreClosed,
        flowreIdle,
        flowreAttack
    }

    void Awake()
    {
        flowreAnimator = GetComponent<Animator>();
    }

    public void ChangeAnimationState (FlowreAnimationStates animationStates)
    {
        string newState = animationStates.ToString(); 
        if (currentState == newState) return;
        
        flowreAnimator.Play(newState);
        currentState = newState;
    }

    public float GetAnimationStateLength(string stateName)
    {
        if (flowreAnimator == null) return 0f;

        for (int i = 0; i < flowreAnimator.layerCount; i++)
        {
            AnimatorStateInfo stateInfo = flowreAnimator.GetCurrentAnimatorStateInfo(i);

            if (stateInfo.IsName(stateName)) return stateInfo.length;
        }
        return 0f;
    }
}

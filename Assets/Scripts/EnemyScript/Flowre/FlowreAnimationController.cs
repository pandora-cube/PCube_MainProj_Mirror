using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowreAnimationController : MonoBehaviour
{
    private Animator animator;
    private string currentState;

    public enum FlowreAnimationStates
    {
        flowreClosed,
        flowreIdle,
        flowreAttack
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeAnimationState (FlowreAnimationStates animationStates)
    {
        string newState = animationStates.ToString(); 
        if (currentState == newState) return;
        
        animator.Play(newState);
        currentState = newState;
    }

    public float GetAnimationStateLength(string stateName)
    {
        if (animator == null) return 0f;

        for (int i = 0; i < animator.layerCount; i++)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(i);

            if (stateInfo.IsName(stateName)) return stateInfo.length;
        }
        return 0f;
    }
}

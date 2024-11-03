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

    public void ChangeAnimationState(FlowreAnimationStates animationStates)
    {
        string newState = animationStates.ToString();
        if (currentState == newState) return;

        animator.Play(newState);
        currentState = newState;
    }

    public float GetAnimationStateLength(FlowreAnimationStates animationStates)
    {
        string stateName = animationStates.ToString();
        
        if (animator == null) return 0f;

        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name != stateName) continue;
            
            return clip.length;
        }

        return 0f;
    }
}

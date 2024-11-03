using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseAnimationController<T> : MonoBehaviour
{
    protected Animator animator;
    private string currentState;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeAnimationState(T animationState)
    {
        string newState = animationState.ToString();
        if (newState == currentState) return;

        animator.Play(newState);
        currentState = newState;
    }

    public float GetAnimationStateLength(T animationState)
    {
        string stateName = animationState.ToString();

        if (animator == null || animator.runtimeAnimatorController == null) return 0f;

        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name != stateName) continue;

            return clip.length;

        }
        return 0f;
    }
}

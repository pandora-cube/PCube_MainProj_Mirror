using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleaoreAnimationController : MonoBehaviour
{
    private Animator animator;

    private string currentState;

    public enum FleaoreAnimationStates
    {
        fleaoreIdle,
        fleaoreAttack
    }
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeAnimationState(FleaoreAnimationStates animationStates)
    {
        string newState = animationStates.ToString();
        if (currentState == newState) return;

        animator.Play(newState);
        currentState = newState;
    }
}

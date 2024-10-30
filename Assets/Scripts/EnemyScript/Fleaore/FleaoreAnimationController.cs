using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleaoreAnimationController : MonoBehaviour
{
    private Animator fleaoreAnimator;

    private string currentState;

    public enum FleaoreAnimationStates
    {
        fleaoreIdle,
        fleaoreAttack
    }
    void Awake()
    {
        fleaoreAnimator = GetComponent<Animator>();
    }

    public void ChangeAnimationState(FleaoreAnimationStates animationStates)
    {
        string newState = animationStates.ToString();
        if (currentState == newState) return;

        fleaoreAnimator.Play(newState);
        currentState = newState;
    }
}

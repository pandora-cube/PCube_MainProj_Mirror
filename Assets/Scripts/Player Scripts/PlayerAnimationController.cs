using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : PlayerStateMachine
{
    [Header("Animation Variables")]
    private string currentState;
    [SerializeField] private Animator normalAnimator;
    [SerializeField] private Animator ghostAnimator;
    [HideInInspector] private AnimatorStateInfo currentAnimation;

    public enum NormalAnimationStates
    {
        normalIdle,
        normalWalk,
    }

    public enum GhostAnimationStates
    {
        ghostIdle,
        ghostWalk,
        ghostAttack1,
        ghostAttack2,
        ghostAttack3,
    }    
    
    public void ChangeAnimationState(GhostAnimationStates animationStates)
    {
        if (!ghostGameObject.activeSelf) return; 

        string newState = animationStates.ToString();
        if (currentState == newState) return;

        ghostAnimator.Play(newState);
        currentState = newState;
    }

    public void ChangeAnimationState(NormalAnimationStates animationStates)
    {
        if (!normalGameObject.activeSelf) return;
        string newState = animationStates.ToString();
        if (currentState == newState) return;

        normalAnimator.Play(newState);
        currentState = newState;
    }

    public bool CheckIfAttackAnimationHasEnded()
    {
        currentAnimation = ghostAnimator.GetCurrentAnimatorStateInfo(0);
        if (!(currentAnimation.IsName("ghostAttack1") || currentAnimation.IsName("ghostAttack2") || currentAnimation.IsName("ghostAttack3"))) return true;
        if (currentAnimation.normalizedTime >= 1.0f && !ghostAnimator.IsInTransition(0)) return true;
        else return false;
    }
}

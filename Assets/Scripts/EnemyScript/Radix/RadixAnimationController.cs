using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadixAnimationController : MonoBehaviour
{
    private Animator animator;
    private string currentState;

    public enum RadixAnimationStates
    {
        radixIdle,
        radixEmerged,
        radixMove,
        radixAttack
    }
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        ChangeAnimationState(RadixAnimationStates.radixIdle);
    }

    public void ChangeAnimationState(RadixAnimationStates animationStates)
    {
        string newState = animationStates.ToString();
        if (currentState == newState) return;

        animator.Play(newState);
        currentState = newState;
    }

    /// <summary>
    /// finds the given animation state's length
    /// </summary>
    /// <param name="stateName">name of the animation state in the animation controller</param>
    /// <returns>length of animation state in float</returns>

    public float GetAnimationStateLength(string stateName)
    {
        if (animator == null || animator.runtimeAnimatorController == null)
            return 0f;

        RuntimeAnimatorController controller = animator.runtimeAnimatorController;

        foreach (AnimationClip clip in controller.animationClips)
        {
            if (clip.name == stateName)
            {
                return clip.length;
            }
        }

        return 0f;

    }
}

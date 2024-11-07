using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parryable : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    [Header("Animation Variables")]
    public string animationName;
    public int startFrame = 2;
    public int endFrame = 3;

    [Header("Parry Variables")]
    public float parryForce;

    private float fps;
    private float startTime;
    private float endTime;

    private bool parryable;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        if (animator == null) return;

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == animationName)
            {
                fps = clip.frameRate;
                startTime = startFrame / fps;
                endTime = endFrame / fps;
                break;
            }
        }
    }

    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(animationName))
        {
            float currentTime = stateInfo.normalizedTime * stateInfo.length;

            if (currentTime >= startTime && currentTime <= endTime) parryable = true;
        }
        else parryable = false;
    }

    public bool AttemptParry()
    {
        if (parryable)  OnParrySuccess();

        return parryable;
    }

    void OnParrySuccess()
    {
        Vector2 pushDirection = (transform.position - PlayerStateMachine.instance.transform.position).normalized;
        rb.AddForce(pushDirection * parryForce, ForceMode2D.Impulse);
    }
}

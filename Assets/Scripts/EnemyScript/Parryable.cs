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

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void Parry()
    {
        Vector2 pushDirection = (transform.position - PlayerStateMachine.instance.transform.position).normalized;
        rb.AddForce(pushDirection * parryForce, ForceMode2D.Impulse);
    }
}

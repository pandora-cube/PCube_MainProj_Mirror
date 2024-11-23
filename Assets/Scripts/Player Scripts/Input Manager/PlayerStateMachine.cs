using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerStateMachine : MonoBehaviour
{
    public bool isGhost;
    public bool isNormal;
    public bool canMove;
    public bool isCrawling;
    public bool isOnSlope;
    public bool canDash;
    public bool isGrounded;
    public bool wasGrounded;
    public bool isAttacking;
    public bool isDashing;
    public bool isTakingDamage;

    public static PlayerStateMachine instance;

    public UnityEvent OnLanded;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Init();
    }

    private void FixedUpdate()
    {
        if (!wasGrounded && isGrounded)
        {
            OnLanded.Invoke();
        }

        wasGrounded = isGrounded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Init();
    }

    void Init()
    {
        isNormal = true;
        isGhost = false;
        canMove = true;
        isCrawling = false;
        isOnSlope = false;
        canDash = true;
        isAttacking = false;
        isDashing = false;
        isTakingDamage = false;
        wasGrounded = false;
    }
}

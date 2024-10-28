using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float direction;
    [SerializeField] private float timeElapsed = 0f;
    private float timeNeeded = 1.5f;
    private bool isPeeking = false;

    [SerializeField] private CinemachineVirtualCamera normalCamera;
    [SerializeField] private CinemachineVirtualCamera ghostCamera;
    [SerializeField] private CinemachineVirtualCamera peekCamera;

    private Animator animator;
    private PlayerStateMachine playerStateMachine;
    private PlayerGroundChecker playerGroundChecker;
    private Vector3 peekCameraFinalPosition;

    void Awake()
    {
        animator = GetComponent<Animator>();
        playerStateMachine = FindObjectOfType<PlayerStateMachine>();
    }

    void Update()
    {
        if (Mathf.Approximately(direction, 0f)) StopPeekOverEdge(direction);
        else StartPeekOverEdge(direction);

        if (!isPeeking)
        {
            if (playerStateMachine.isNormal) animator.Play("NormalDefault");
            else animator.Play("GhostDefault");
        }
    }

    public void OnPressUpOrDown(InputAction.CallbackContext ctx)
    {
        direction = ctx.ReadValue<float>();
    }

     void StartPeekOverEdge(float peekDirection)
    {
        if (!playerGroundChecker.isGrounded) return;

        timeElapsed += Time.deltaTime;

        if (timeElapsed >= timeNeeded)
        {
            if (playerStateMachine.isNormal) peekCamera.transform.position = normalCamera.transform.position;
            else if (playerStateMachine.isGhost) peekCamera.transform.position = ghostCamera.transform.position;
            peekCamera.transform.position += new Vector3(0f, -2f);

            peekCameraFinalPosition = peekCamera.transform.position;

            animator.Play("Peeking");

            isPeeking = true;
            timeElapsed = 0f;
        }
    }

    void StopPeekOverEdge(float peekDirection)
    {
        if (!isPeeking) return;

        timeElapsed = 0f;

        if (playerStateMachine.isNormal)
        {
            normalCamera.transform.position = peekCameraFinalPosition;
        }
        else if (playerStateMachine.isGhost)
        {
            ghostCamera.transform.position = peekCameraFinalPosition;
        }

        peekCamera.transform.position = peekCameraFinalPosition;

        if (playerStateMachine.isNormal)
        {
            animator.Play("NormalDefault");
        }
        else
        {
            animator.Play("GhostDefault");
        }
        isPeeking = false;
    }
}
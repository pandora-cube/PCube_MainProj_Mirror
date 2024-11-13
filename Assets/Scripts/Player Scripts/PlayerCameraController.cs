using System.Collections;
using System.Collections.Generic;
using System.IO;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraController : MonoBehaviour
{
    public static PlayerCameraController instance;
    public bool isProducting = false;
    [SerializeField] private float direction;
    [SerializeField] private float timeElapsed = 0f;
    private float timeNeeded = 1.5f;
    private bool isPeeking = false;
    string currentState;

    [SerializeField] private CinemachineVirtualCamera normalCamera;
    [SerializeField] private CinemachineVirtualCamera ghostCamera;
    [SerializeField] private CinemachineVirtualCamera peekCamera;
    [SerializeField] private CinemachineVirtualCamera productionCamera;

    private Animator animator;
    private PlayerStateMachine PlayerState => PlayerStateMachine.instance;
    private Vector3 peekCameraFinalPosition;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(instance);

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Mathf.Approximately(direction, 0f)) StopPeeking();
        else StartPeeking();
    }

    public void OnHoldDown(InputAction.CallbackContext input)
    {
        direction = input.ReadValue<float>();
    }

    void StartPeeking()
    {
        if (!PlayerState.isGrounded) return;

        timeElapsed += Time.deltaTime;

        if (timeElapsed >= timeNeeded)
        {
            SetPeekCameraPosition();
            peekCameraFinalPosition = peekCamera.transform.position;

            ChangeAnimationState("Peeking");

            isPeeking = true;
            timeElapsed = 0f;
        }
    }

    void SetPeekCameraPosition()
    {
        if      (PlayerState.isNormal)  peekCamera.transform.position = normalCamera.transform.position;
        else if (PlayerState.isGhost)   peekCamera.transform.position = ghostCamera.transform.position;

        peekCamera.transform.position += new Vector3(0f, -2f);
    }

    void StopPeeking()
    {
        if (!isPeeking) return;

        timeElapsed = 0f;

        ResetCameraPosition();

        peekCamera.transform.position = peekCameraFinalPosition;

        ReturnCameraPosition();
        isPeeking = false;
    }

    void ResetCameraPosition()
    {
        if (PlayerState.isNormal) normalCamera.transform.position = peekCameraFinalPosition;
        else if (PlayerState.isGhost) ghostCamera.transform.position = peekCameraFinalPosition;
    }

    public void ReturnCameraPosition()
    {
        isProducting = false;
        if (PlayerState.isNormal) ChangeAnimationState("NormalDefault");
        else ChangeAnimationState("GhostDefault");
    }

    public void StartProductionCamera()
    {
        isProducting = true;
        ChangeAnimationState("Production");
    }

    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;

        animator.Play(newState);
        currentState = newState;
    }
    
    public Transform CurCameraPosition()
    {
        if (PlayerState.isNormal) return normalCamera.transform;
        else return ghostCamera.transform;
    }
}
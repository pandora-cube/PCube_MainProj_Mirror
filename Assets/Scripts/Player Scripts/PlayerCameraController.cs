using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraController : MonoBehaviour
{
    public static PlayerCameraController instance;

    //[SerializeField] private float direction;
    //[SerializeField] private float timeElapsed = 0f;
    //private float timeNeeded = 1.5f;
    //private bool isPeeking = false;

    [SerializeField] private CinemachineVirtualCamera normalCamera;
    [SerializeField] private CinemachineVirtualCamera ghostCamera;
    [SerializeField] private CinemachineVirtualCamera peekCamera;
    [SerializeField] private CinemachineVirtualCamera ProductionCamera;

    private Animator animator;
    private PlayerStateMachine PlayerState => PlayerStateMachine.instance;
    //private Vector3 peekCameraFinalPosition;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(instance);

        animator = GetComponent<Animator>();
    }

    //void Update()
    //{
    //    if (Mathf.Approximately(direction, 0f)) StopPeekOverEdge(direction);
    //    else StartPeekOverEdge(direction);
    //}

    //public void OnPressUpOrDown(InputAction.CallbackContext ctx)
    //{
    //    direction = ctx.ReadValue<float>();
    //}

    // void StartPeekOverEdge(float peekDirection)
    //{
    //    if (!PlayerState.isGrounded) return;

    //    timeElapsed += Time.deltaTime;

    //    if (timeElapsed >= timeNeeded)
    //    {
    //        if (PlayerState.isNormal) peekCamera.transform.position = normalCamera.transform.position;
    //        else if (PlayerState.isGhost) peekCamera.transform.position = ghostCamera.transform.position;
    //        peekCamera.transform.position += new Vector3(0f, -2f);

    //        peekCameraFinalPosition = peekCamera.transform.position;

    //        animator.Play("Peeking");

    //        isPeeking = true;
    //        timeElapsed = 0f;
    //    }
    //}

    //void StopPeekOverEdge(float peekDirection)
    //{
    //    if (!isPeeking) return;

    //    timeElapsed = 0f;

    //    if (PlayerState.isNormal)
    //    {
    //        normalCamera.transform.position = peekCameraFinalPosition;
    //    }
    //    else if (PlayerState.isGhost)
    //    {
    //        ghostCamera.transform.position = peekCameraFinalPosition;
    //    }

    //    peekCamera.transform.position = peekCameraFinalPosition;

    //    if (PlayerState.isNormal)
    //    {
    //        animator.Play("NormalDefault");
    //    }
    //    else
    //    {
    //        animator.Play("GhostDefault");
    //    }
    //    isPeeking = false;
    //}
    public void ReturnCameraPosition()
    {
        if (PlayerState.isNormal) animator.Play("NormalDefault");
        else animator.Play("GhostDefault");
    }
    public void StartProductionCamera()
    {
        animator.Play("Production");
    }
}
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float direction;
    [SerializeField] private float timeElapsed = 0f;
    private float timeNeeded = 1.5f;
    private bool isPeeking = false;

    [SerializeField] private CinemachineVirtualCamera normalCamera;
    [SerializeField] private CinemachineVirtualCamera peekCamera;

    private Animator animator;
    private PlayerController playerController;

    void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = FindAnyObjectByType<PlayerController>();
    }

    void Update()
    {
        if (Mathf.Approximately(direction, 0f)) StopPeekOverEdge(direction);
        else StartPeekOverEdge(direction);
    }

    public void OnPressUpOrDown(InputAction.CallbackContext ctx)
    {
        direction = ctx.ReadValue<float>();
    }

    void StartPeekOverEdge(float peekDirection)
    {
        if (!playerController.isGrounded) return;

        timeElapsed += Time.deltaTime;

        if (timeElapsed >= timeNeeded)
        {
            peekCamera.transform.position = normalCamera.transform.position;
            peekCamera.transform.position += new Vector3(0f, -2f);
            animator.Play("Peeking");

            isPeeking = true;
            timeElapsed = 0f;
            Debug.Log("PEEKING");
        }
    }

    void StopPeekOverEdge(float peekDirection)
    {
        if (!isPeeking) return;

        timeElapsed = 0f;
        normalCamera.transform.position = peekCamera.transform.position;
        peekCamera.transform.position -= new Vector3(normalCamera.transform.position.x, -2f * Time.deltaTime);

        if (playerController.isNormal)
        {
            animator.Play("NormalDefault");
        }
        else
        {
            animator.Play("GhostDefault");
        }
        isPeeking = false;

#if UNITY_EDITOR
        Debug.Log("STOPPING");
#endif
    }

}
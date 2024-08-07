using System.Collections;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private float direction;    
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

        Debug.Log("direction:" + direction);
    }
    
    void StopPeekOverEdge(float peekDirection)
    {
        timeElapsed = 0f;
        peekCamera.transform.position -= new Vector3(0f, -2f);
        animator.Play("Default");
    }
    void StartPeekOverEdge(float peekDirection)
    {
        if (!playerController.isGrounded) return;

        timeElapsed += Time.deltaTime;

        if (timeElapsed >= timeNeeded)
        {  
            peekCamera.transform.position += new Vector3(0f, -2f);
            animator.Play("Peeking");
            
            isPeeking = !isPeeking;
            timeElapsed = 0f;
        }
    }
}

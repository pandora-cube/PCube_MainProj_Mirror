using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float cameraSpeed = 1.5f;
    private float direction; 
    private float timeElapsed = 0f;
    private float timeNeeded = 1.5f;
    private float peekTimeElapsed = 0f;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Transform peekTransform;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private CinemachineConfiner2D cinemachineConfiner2D;
    [SerializeField] private PolygonCollider2D normalPolygonCollider2D;
    [SerializeField] private PolygonCollider2D peekPolygonCollider2D;


    bool isPeeking = false;

    private Vector3 velocity = Vector3.zero;

    void Awake()
    {
        if (playerController == null) Debug.LogError("PLAYER CONTROLLER IS NULL! Object: " + gameObject.name);
        if (virtualCamera == null) Debug.LogError("VIRTUAL CAM IS NULL! Object: " + gameObject.name);
        if (peekTransform == null) Debug.LogError("PEEK TRANSFORM IS NULL! Object: " + gameObject.name);
    }

    void Start()
    {
        peekTransform.position = virtualCamera.transform.position;
    }
    void Update()
    {
       ShiftCameraOnEdge();
    }

    public void OnPressUpOrDown(InputAction.CallbackContext ctx)
    {
        direction = ctx.ReadValue<float>();
        timeElapsed = 0f;
    }

    void ShiftCameraOnEdge()
    {
        if (!playerController.isGrounded || direction == 0)
        {
            timeElapsed = 0f;  // Reset the timer when player is not moving or not grounded
            cinemachineConfiner2D.m_BoundingShape2D = normalPolygonCollider2D;
            
            virtualCamera.Follow = playerTransform;

            isPeeking = false;
            return;
        }

        timeElapsed += Time.fixedDeltaTime;
        if (timeElapsed >= timeNeeded)
        {
            cinemachineConfiner2D.m_BoundingShape2D = peekPolygonCollider2D;
            MoveCameraPos();
            isPeeking = true;
            peekTimeElapsed += Time.deltaTime;
        }
    }

    void MoveCameraPos()
    {
        if (isPeeking) return;
        Vector3 targetPos = playerTransform.position + new Vector3(0, -0.1f, 0);
        peekTransform.position = Vector3.MoveTowards(peekTransform.position, targetPos, cameraSpeed * Time.deltaTime);
        virtualCamera.Follow = peekTransform;
    }
}

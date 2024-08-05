using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private float cameraSpeed = 1.5f;
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

    private Vector3 velocity = Vector3.zero;

    void Awake()
    {
        if (playerController == null) Debug.LogError("PLAYER CONTROLLER IS NULL! Object: " + gameObject.name);
        if (virtualCamera == null) Debug.LogError("VIRTUAL CAM IS NULL! Object: " + gameObject.name);
        if (peekTransform == null) Debug.LogError("PEEK TRANSFORM IS NULL! Object: " + gameObject.name);
    }

    void FixedUpdate()
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
            peekTransform.position = playerTransform.position;  // Reset peek position to player position
            virtualCamera.Follow = playerTransform;
            return;
        }

        timeElapsed += Time.fixedDeltaTime;
        if (timeElapsed >= timeNeeded)
        {
            cinemachineConfiner2D.m_BoundingShape2D = peekPolygonCollider2D;
            Vector3 targetPos = playerTransform.position + new Vector3(0, -0.5f, 0);
            peekTransform.position = Vector3.Lerp(peekTransform.position, targetPos, peekTimeElapsed/5f);
            virtualCamera.Follow = peekTransform;

            peekTimeElapsed += Time.deltaTime;
        }
    }
}

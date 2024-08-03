using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    private float cameraSpeed = 1f;
    private float direction;
    private float timeElapsed;
    private float timeNeeded = 1.5f;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private InputActionMap playerInputAction;

    void Awake()
    {
       if (playerController == null) Debug.LogError("PLAYER CONTROLLER IS NULL! Object: " + gameObject.name);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ShiftCameraOnEdge();
    }

    public void OnPressUpOrDown(InputAction.CallbackContext ctx)
    {
        direction = ctx.ReadValue<float>();
    }
    void ShiftCameraOnEdge()
    {
        if (!playerController.isGrounded || direction == 0) return;

        timeElapsed += Time.deltaTime;
        if (timeElapsed >= timeNeeded) transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y + 2f), cameraSpeed * Time.deltaTime);
    }
}

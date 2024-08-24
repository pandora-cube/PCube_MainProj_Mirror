using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Transform zoomTarget; // The position to zoom into
    [SerializeField] private float targetFov = 30f; // Desired FOV when zoomed in
    [SerializeField] private float zoomSpeed = 2f; // Speed of the zoom
    [SerializeField] private Button zoomButton; // UI Button to trigger the zoom

    private float originalFov;
    private Transform originalFollow;
    private bool isZoomingIn = false;

    void Start()
    {
        // Store the original FOV and follow target
        originalFov = virtualCamera.m_Lens.FieldOfView;
        originalFollow = virtualCamera.Follow;

        // Add listener to the button
        zoomButton.onClick.AddListener(OnZoomButtonClicked);
    }

    void Update()
    {
        if (isZoomingIn)
        {
            // Gradually adjust the FOV
            virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(virtualCamera.m_Lens.FieldOfView, targetFov, Time.deltaTime * zoomSpeed);

            // Gradually adjust the follow target position if required
            virtualCamera.Follow.position = Vector3.Lerp(virtualCamera.Follow.position, zoomTarget.position, Time.deltaTime * zoomSpeed);

            // Stop zooming once close enough to the target FOV
            if (Mathf.Abs(virtualCamera.m_Lens.FieldOfView - targetFov) < 0.1f)
            {
                virtualCamera.m_Lens.FieldOfView = targetFov;
                isZoomingIn = false;
            }
        }
    }

    void OnZoomButtonClicked()
    {
        // Start the zooming process
        virtualCamera.Follow = zoomTarget;
        isZoomingIn = true;
    }

    public void ResetZoom()
    {
        // Reset to the original FOV and follow target
        virtualCamera.m_Lens.FieldOfView = originalFov;
        virtualCamera.Follow = originalFollow;
        isZoomingIn = false;
    }
}

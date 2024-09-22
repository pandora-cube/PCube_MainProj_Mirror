using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDamageCamShake : MonoBehaviour
{
    public static PlayerDamageCamShake Instance { get; private set; }
    private ParallaxScrolling parallaxScrolling;
    private CinemachineVirtualCamera ghostCamera;
    [SerializeField] private float intensity;
    private float shakeTimer;
    [SerializeField] private float shakeTime;
    private float shakeTimerTotal;
    private float startingIntensity;
    private bool isShaking = false;

    [SerializeField] private PlayerInput playerInput;

    private void Awake()
    {
        ghostCamera = GetComponent<CinemachineVirtualCamera>();
        parallaxScrolling = FindObjectOfType<ParallaxScrolling>();

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;

            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = ghostCamera
                .GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            // Smoothly reduce the intensity
            float shakeProgress = shakeTimer / shakeTimerTotal;
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, 1 - shakeProgress);

            if (shakeTimer <= 0f)
            {
                // Shake finished, stop the effect
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
                isShaking = false;

                // Re-enable parallax scrolling
                parallaxScrolling.SetIsParallexEnabled(true);

                // Re-enable player input
                playerInput.enabled = true;
            }
        }
    }

    public void TriggerCameraShake()
    {
        if (!isShaking)
        {
            ShakeCamera();
        }
    }

    private void ShakeCamera()
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
            ghostCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        isShaking = true;
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;

        // Disable parallax scrolling to avoid background-only shaking
        parallaxScrolling.SetIsParallexEnabled(false);

        // Disable player input during the shake
        playerInput.enabled = false;

        shakeTimer = shakeTime;
        shakeTimerTotal = shakeTime;
        startingIntensity = intensity;
    }
}

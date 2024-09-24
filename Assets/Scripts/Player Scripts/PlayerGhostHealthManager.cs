using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using Cinemachine;
using UnityEngine.UI;

public class PlayerGhostHealthManager : MonoBehaviour, IDamageable
{
    [field: SerializeField] public float maxHealth { get; set; }
    [field: SerializeField] public float currentHealth { get; set; }
    [Header("Damage Effects")]
    [SerializeField] private float damageDelay;
    [SerializeField] private int numberOfBlinks;
    [SerializeField] private float blinkInterval;
    [Range(0f, 1f)][SerializeField] private float alphaValue;

    [Header("Collision Detection")]
    [SerializeField] private LayerMask obstacleLayer;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] SavePoints savePoints;
    [SerializeField] private float ghostTimer;
    [SerializeField] private float ghostTimeLimit;

    private SpriteRenderer spriteRenderer;
    [SerializeField] private bool isTakingDamage = false;
    bool isBlinked = false;

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float shakeTimer;
    private float shakeTimerTotal;
    private float startingIntensity;

    [SerializeField] float shakeIntensity;
    [SerializeField] float shakeTime;

    [Header("Damage Effects")]
    [SerializeField] private Image[] damageEffectImages;
    [SerializeField] private float damageEffectDuration;

    void OnEnable()
    {
        ghostTimer = 0f;
        maxHealth = 5f;
        currentHealth = maxHealth;
        isBlinked = false;
    }

    private void Awake()
    {
        if (cinemachineVirtualCamera == null) Debug.LogError("Cinemachine virtual cam is null! GameObject: +" + gameObject.name);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        maxHealth = 5f;
        currentHealth = maxHealth;
        ghostTimer = 0f;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            float newAmplitude = Mathf.Lerp(startingIntensity, 0f, 1 - shakeTimer / shakeTimerTotal);
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = newAmplitude;

            // Debug statements to check shake progress
            Debug.Log($"Shaking: Timer = {shakeTimer}, Amplitude = {newAmplitude}");

            if (shakeTimer <= 0)
                Debug.Log("Shake complete");
        }

        if (gameObject.activeSelf)
        {
            ghostTimer += Time.deltaTime;
            if (!isBlinked && (ghostTimer >= ghostTimeLimit - 15f))
                StartCoroutine(TriggerTimerEffect());
            if (ghostTimer >= ghostTimeLimit)
                Die();
        }
    }


    #region DAMAGE EFFECTS
    public void TriggerDamageEffects()
    {
        Debug.Log("Triggering damage effects, starting camera shake...");
        ShakeCamera();
    }

    private void ShakeCamera()
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        if (cinemachineBasicMultiChannelPerlin != null)
        {
            startingIntensity = shakeIntensity;
            shakeTimerTotal = shakeTime;
            shakeTimer = shakeTime;

            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = shakeIntensity;

            // Debug when the shake starts
            Debug.Log($"Shake started with intensity {shakeIntensity} for duration {shakeTime}");
        }
        else
        {
            Debug.LogError("Cinemachine Perlin component not found.");
        }
    }

    IEnumerator TriggerTimerEffect()
    {
        float halfDuration = damageEffectDuration / 2f;
        float elapsedTime = 0f;
        float targetAlpha = 1f; // Maximum alpha (fully visible)

        // Set initial alpha to 0 (fully transparent)
        foreach (Image e in damageEffectImages)
        {
            Color color = e.color;
            color.a = 0f;
            e.color = color;
            e.enabled = true; // Enable the images to start the effect
        }

        // Fade-in: Gradually increase alpha from 0 to 1 during the first half of the duration
        while (elapsedTime < halfDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, targetAlpha, elapsedTime / halfDuration);

            foreach (Image e in damageEffectImages)
            {
                Color color = e.color;
                color.a = alpha;
                e.color = color;
            }
            yield return null;
        }

        // Reset elapsed time for fade-out
        elapsedTime = 0f;

        // Fade-out: Gradually decrease alpha from 1 to 0 during the second half of the duration
        while (elapsedTime < halfDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(targetAlpha, 0f, elapsedTime / halfDuration);

            foreach (Image e in damageEffectImages)
            {
                Color color = e.color;
                color.a = alpha;
                e.color = color;
            }
            yield return null;
        }

        // After fading out, disable the images
        foreach (Image e in damageEffectImages)
        {
            e.enabled = false;
        }
    }

    #endregion

    public void ReadyToRestart()
    {
        currentHealth = maxHealth;
        ghostTimer = 0f;
        gameOverPanel.SetActive(false);
        Time.timeScale = 1f;
        savePoints.PlayerRespawn();
    }

    public void TakeDamage(float damageAmount)
    {
        if (isTakingDamage) return;
        isTakingDamage = true;
        currentHealth -= damageAmount;
        if (currentHealth <= 0) Die();

        TriggerDamageEffects();
        StartCoroutine(BlinkAfterTakingDamage());
        StartCoroutine(ResetIsTakingDamageBool());
    }

    public void Die()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    IEnumerator BlinkAfterTakingDamage()
    {
        if (isTakingDamage) yield break;
        isBlinked = true;
        Color spriteColor = spriteRenderer.color;
        for (int i = 0; i < numberOfBlinks; ++i)
        {
            spriteColor.a = alphaValue;
            spriteRenderer.color = spriteColor;

            yield return new WaitForSeconds(blinkInterval);

            spriteColor.a = 1;
            spriteRenderer.color = spriteColor;

            yield return new WaitForSeconds(blinkInterval);
        }
    }


    IEnumerator ResetIsTakingDamageBool()
    {
        yield return new WaitForSeconds(damageDelay);

        isTakingDamage = false;
    }

    #region GETTERS AND SETTERS
    public void SetGhostTimeLimit(float newTime)
    {
        ghostTimeLimit = newTime;
        ghostTimer = 0f;
    }
    #endregion //GETTERS AND SETTERS
}

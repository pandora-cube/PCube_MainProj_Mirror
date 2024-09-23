
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class PlayerDamageEffects : MonoBehaviour
{
    public static PlayerDamageEffects Instance { get; private set; }

    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float shakeTimer;
    private float shakeTimerTotal;
    private float startingIntensity;

    [SerializeField] float shakeIntensity;
    [SerializeField] float shakeTime;

    [Header("Damage Effects")]
    [SerializeField] private Image[] damageEffectImages;
    [SerializeField] private float damageEffectDuration;

    private void Awake()
    {
        Instance = this;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain =
                Mathf.Lerp(startingIntensity, 0f, 1 - shakeTimer / shakeTimerTotal);
        }
    }

    public void TriggerDamageEffects()
    {
        ShakeCamera();
        //StartCoroutine(TriggerDamageEffect());
    }
    private void ShakeCamera()
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = shakeIntensity;

        startingIntensity = shakeIntensity;
        shakeTimerTotal = shakeTime;
        shakeTimer = shakeTime;
    }

    IEnumerator TriggerDamageEffect()
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

}
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using Cinemachine;
using UnityEngine.UI;

public class PlayerGhostHealthManager : Damageable
{
    public bool isAttacked;

    [Header("Damage Effects")]
    [SerializeField] private float damageDelay;
    [SerializeField] private int numberOfBlinks;
    [SerializeField] private float blinkInterval;
    [Range(0f, 1f)][SerializeField] private float alphaValue;

    [Header("Collision Detection")]
    [SerializeField] private LayerMask obstacleLayer;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private float ghostTimer;
    [SerializeField] private float ghostTimeLimit;

    bool hasShownTimerEffect = false;

    [Header("Damage Effects")]
    [SerializeField] private Image[] timerEffectImages;
    [SerializeField] private float timerEffectDuration;
    [SerializeField] private float timerEffectRepeatRate;
    [SerializeField] private float minAlpha;
    private bool isEffectRunning = false;

    const int OBSTACLE_LAYER = 9;

    private Damageable damageable;
    private PlayerStateMachine PlayerState => PlayerStateMachine.instance;

    void OnEnable()
    {
        ghostTimer = 0f;
        maxHealth = 5f;
        currentHealth = maxHealth;
        hasShownTimerEffect = false;
        isEffectRunning = false;
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        damageable = GetComponent<Damageable>();
    }

    private void Start()
    {
        maxHealth = 5f;
        currentHealth = maxHealth;
        ghostTimer = 0f;
    }

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            ghostTimer += Time.deltaTime;
            if (!hasShownTimerEffect && (ghostTimer >= ghostTimeLimit - 15f))
            {
                InvokeRepeating("StartTriggerTimerEffect", 0, timerEffectRepeatRate);
            }
            if (ghostTimer >= ghostTimeLimit) Die();
        }
    }


    void StartTriggerTimerEffect()
    {
        if (isEffectRunning) return;
        StartCoroutine(TriggerTimerEffect());
    }

    IEnumerator TriggerTimerEffect()
    {
        isEffectRunning = true;
        float halfDuration = timerEffectDuration / 2f;
        float elapsedTime = 0f;
        float targetAlpha = 1f; // Maximum alpha (fully visible)

        // Set initial alpha to 0 (fully transparent)
        foreach (Image e in timerEffectImages)
        {
            Color color = e.color;
            color.a = minAlpha;
            e.color = color;
            e.enabled = true; // Enable the images to start the effect
        }

        // Fade-in: Gradually increase alpha from 0 to 1 during the first half of the duration
        while (elapsedTime < halfDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(minAlpha, targetAlpha, elapsedTime / halfDuration);

            foreach (Image e in timerEffectImages)
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
            float alpha = Mathf.Lerp(targetAlpha, minAlpha, elapsedTime / halfDuration);

            foreach (Image e in timerEffectImages)
            {
                Color color = e.color;
                color.a = alpha;
                e.color = color;
            }
            yield return null;
        }

        // After fading out, disable the images
        foreach (Image e in timerEffectImages)
        {
            e.enabled = false;
        }

        isEffectRunning = false;
    }

    public void ReadyToRestart()
    {
        currentHealth = maxHealth;
        ghostTimer = 0f;
        gameOverPanel.SetActive(false);
        Time.timeScale = 1f;
        SavePoints.instance.PlayerRespawn();
    }

    public void TakeDamage(float damageAmount)
    {
        if (PlayerState.isTakingDamage) return;
        PlayerState.isTakingDamage = true;
        currentHealth -= damageAmount;
        if (currentHealth <= 0) Die();
        
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
        hasShownTimerEffect = true;
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

        PlayerState.isTakingDamage = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == OBSTACLE_LAYER)
        {
            TakeDamage(1f);
        }
    }

    #region GETTERS AND SETTERS
    public void SetGhostTimeLimit(float newTime)
    {
        ghostTimeLimit = newTime;
        ghostTimer = 0f;
    }
    #endregion //GETTERS AND SETTERS
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGhostHealthManager : MonoBehaviour, IDamageable
{
    [field:SerializeField] public float maxHealth { get; set; }
    [field:SerializeField] public float currentHealth { get; set; }
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

    void OnEnable()
    {
        ghostTimer = 0f;
    }
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        maxHealth = 5f;
        currentHealth = maxHealth;
        ghostTimer = 0f;
    }

    void Update()
    {
        ghostTimer += Time.deltaTime;

        if (ghostTimer >= ghostTimeLimit) Die();
    }
    public void Die()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ReadyToRestart()
    {
        currentHealth = maxHealth;
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

        StartCoroutine(BlinkAfterTakingDamage());
        StartCoroutine(ResetIsTakingDamageBool());

    }


    IEnumerator BlinkAfterTakingDamage()
    {
        if (!isTakingDamage) yield break;
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
}

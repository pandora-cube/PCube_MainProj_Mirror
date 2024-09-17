using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGhostHealthManager : MonoBehaviour, IDamageable
{
    [field:SerializeField] public float maxHealth { get; set; }
    [field:SerializeField] public float currentHealth { get; set; }
    [SerializeField] private float damageDelay;
    [SerializeField] private int numberOfBlinks;
    [SerializeField] private float blinkInterval;
    [Range(0f, 1f)][SerializeField] private float alphaValue;

    [Header("Collision Detection")]
    [SerializeField] private LayerMask obstacleLayer;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverPanel;

    private SpriteRenderer spriteRenderer;
    [SerializeField] private bool isTakingDamage = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        maxHealth = 5f;
        currentHealth = maxHealth;
    }
    public void Die()
    {
        
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
        if (isTakingDamage) yield break;

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

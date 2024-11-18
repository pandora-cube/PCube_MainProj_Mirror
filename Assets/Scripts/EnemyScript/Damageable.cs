using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    [Tooltip("Is object's sprite facing right or left by default?")]
    [SerializeField] private bool isFacingRightByDefault;
    [SerializeField] private bool isPlayer;
    [SerializeField] private float knockbackForce;

    [Tooltip("Material to switch to during damage flash SFX.")]
    public Material flashMaterial;

    [Tooltip("Duration of the flash.")]
    public float duration;

    private SpriteRenderer spriteRenderer;
    public Material originalMaterial;
    private Rigidbody2D rigidbody2D;
    private Coroutine flashRoutine;

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        currentHealth = maxHealth;

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        Flash();
        ApplyKnockback();

        if (currentHealth <= 0) Die();
    }

    public void Die()
    {
        Destroy(gameObject);
    }


    public void Flash()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }

        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        spriteRenderer.material = flashMaterial;

        yield return new WaitForSeconds(duration);

        spriteRenderer.material = originalMaterial;

        flashRoutine = null;
    }

    public void ApplyKnockback()
    {
        float facingDirection;
        float knockbackDirection;

        if (isFacingRightByDefault) facingDirection = transform.localScale.x;
        else facingDirection = -transform.localScale.x;

        if (facingDirection > 0) knockbackDirection = -1f;
        else knockbackDirection = 1f;

        Vector2 knockbackVector = new Vector2(knockbackDirection * knockbackDirection, 0f);

        if (isPlayer)
        {
            if (PlayerStateMachine.instance.isGhost) PlayerComponents.instance.ghostRb.AddForce(knockbackVector * knockbackForce, ForceMode2D.Impulse);
        }
        else
        {
            rigidbody2D.AddForce(knockbackVector * knockbackForce, ForceMode2D.Impulse);
        }
    }
}

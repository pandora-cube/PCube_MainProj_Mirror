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
    private Rigidbody2D rb;
    private Coroutine flashRoutine;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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

    public void ApplyKnockback(Transform knockbackSource)
    {
        Vector2 knockbackDirection = (transform.position - knockbackSource.position).normalized;
        knockbackDirection.y = 0f;
        Vector2 knockbackVector = knockbackDirection * knockbackForce;

        if (isPlayer)
        {
            if (PlayerStateMachine.instance.isGhost)
            {
                PlayerComponents.instance.ghostRb.AddForce(knockbackVector, ForceMode2D.Impulse); //wtf?
                Debug.Log($"Force applied to player: {knockbackVector}");
            }
        }
        else
        {
            rb.AddForce(knockbackVector, ForceMode2D.Impulse);
            Debug.Log($"Force applied to enemy: {knockbackVector}");
        }
    }

}

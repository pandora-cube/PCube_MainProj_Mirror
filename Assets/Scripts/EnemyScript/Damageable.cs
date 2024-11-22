using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Damageable : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    [Tooltip("Material to switch to during damage flash SFX.")]
    public Material flashMaterial;

    [Tooltip("Duration of the flash.")]
    public float duration;

    protected SpriteRenderer spriteRenderer;
    public Material originalMaterial;
    private Coroutine flashRoutine;
    private Parryable parryable;

    private void Start()
    {
        currentHealth = maxHealth;

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;

        parryable = GetComponent<Parryable>();
    }

    public void TakeDamage(float damageAmount)
    {
        if ((parryable != null && parryable.IsInvincible()) || gameObject.CompareTag("Invincible"))
        {
            Debug.Log("Damage canceled: Parried or invincible!");
            return;
        }

        currentHealth -= damageAmount;

        Flash();

        if (currentHealth <= 0)
        {
            Die();
        }
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
}

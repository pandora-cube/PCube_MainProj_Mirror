using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    [Tooltip("Material to switch to during damage flash SFX.")]
    public Material flashMaterial;

    [Tooltip("Duration of the flash.")]
    public float duration;

    public SpriteRenderer spriteRenderer;
    public Material originalMaterial;
    public Coroutine flashRoutine;

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
}

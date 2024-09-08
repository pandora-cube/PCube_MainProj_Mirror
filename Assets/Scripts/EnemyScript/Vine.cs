using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vine : MonoBehaviour, IDamageable
{
    [field: SerializeField] public float maxHealth { get; set; }
    [field: SerializeField] public float currentHealth { get; set; }

    public void TakeDamage(float damageAmount)
    {
        Debug.Log("VINE DMG TAKEN");
        Die();
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    float maxHealth { get; set; }
    float currentHealth { get; set; }

    protected void Start()
    {
        currentHealth = maxHealth;
    }

    void TakeDamage(float damageAmount);
    void Die();
}

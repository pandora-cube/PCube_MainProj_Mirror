using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour, IDamageable
{

    [field: SerializeField] public float maxHealth { get; set; }
    public float currentHealth { get; set; }

    private void Start()
    {
        maxHealth = 3f;
        currentHealth = maxHealth;
    }
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0) Die();
    }
    public void Die()
    {
        Destroy(gameObject);
    }

   
}

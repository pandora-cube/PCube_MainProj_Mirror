using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vine : MonoBehaviour
{
    [field: SerializeField] public float maxHealth { get; set; }
    [field: SerializeField] public float currentHealth { get; set; }

    public void TakeDamage(float damageAmount)
    {
        Die();
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}

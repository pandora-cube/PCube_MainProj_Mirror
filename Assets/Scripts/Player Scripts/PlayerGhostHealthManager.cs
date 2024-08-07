using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGhostHealthManager : MonoBehaviour, IDamageable
{
    // Start is called before the first frame update
   
    [field:SerializeField] public float maxHealth { get; set; }
    [field:SerializeField] public float currentHealth { get; set; }

    [Header("Collision Detection")]
    [SerializeField] private LayerMask obstacleLayer;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverPanel;

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
        currentHealth -= damageAmount;

    }

    
}

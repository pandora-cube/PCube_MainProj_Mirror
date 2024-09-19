using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalHealthManager : MonoBehaviour, IDamageable
{
    [field: SerializeField] public float maxHealth { get ; set ; }
    [field: SerializeField] public float currentHealth { get ; set; }

    [Header("Collision Detection")]
    [SerializeField] private LayerMask obstacleLayer;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] SavePoints savePoints;

    const int OBSTACLE_LAYER = 9;
    
    void Start()
    {
        maxHealth = 1f;
        currentHealth = maxHealth;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == OBSTACLE_LAYER)
        {
            TakeDamage(1f);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0) Die();
    }

    public void Die()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ReadyToRestart()
    {
        currentHealth = maxHealth;
        gameOverPanel.SetActive(false);
        Time.timeScale = 1f;
        savePoints.PlayerRespawn();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalHealthManager : MonoBehaviour, IDamageable
{
    public float maxHealth { get ; set ; }
    public float currentHealth { get ; set; }

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
        //Invoke(nameof(ReadyToRestart), 1f);
    }

    public void ReadyToRestart()
    {
        savePoints.PlayerRespawn();
        Time.timeScale = 1f;
        //Time.timeScale = 1f;
    }
}
